using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica
{
	[DataContract]
	internal class LiteMetricaService : LiteMetricaCore
	{
		private CancellationTokenSource _tokenSource;

		[DataMember]
		public DateTime? LastFlushTime { get; set; }

		~LiteMetricaService()
		{
			if (ActiveSessionLock != null)
			{
				DateTime? lastLullTime = Config.Global.LastLullTime;
				if (!lastLullTime.HasValue || lastLullTime < Config.Global.LastWakeTime)
				{
					Lull();
				}
			}
		}

		public override void Expose()
		{
			base.Expose();
			Subscribe(Store.Get<Lifecycler>(new object[0]));
			Task.Factory.StartNew(Postman);
		}

		private void Subscribe(ILifecycler lifecycler)
		{
			lifecycler.End += (object sender, EventArgs args) =>
			{
				Lull();
			};
			lifecycler.Resume += (object sender, EventArgs args) =>
			{
				Wake();
			};
			lifecycler.Suspend += (object sender, EventArgs args) =>
			{
				Lull();
			};
			lifecycler.UnhandledException += (object sender, EventArgs args) =>
			{
				if (!Adapter.IsInternalException(args))
				{
					if (Config.Global.CrashTracking)
					{
						Config.Global.CrashTracking = false;
						Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_CRASH, Adapter.ExtractData(args)));
						Config.Global.CrashTracking = true;
						Store.Snapshot();
					}
					else
					{
						Lull();
					}
				}
			};
		}

		public override void TriggerForcedSend()
		{
			base.TriggerForcedSend();
			_tokenSource?.Cancel();
		}

		private async Task Wait(TimeSpan delay)
		{
			try
			{
				if (_tokenSource == null)
				{
					await TaskEx.Delay(delay);
				}
				else if (!_tokenSource.IsCancellationRequested)
				{
					await TaskEx.Delay(delay, _tokenSource.Token);
				}
			}
			catch (Exception)
			{
			}
		}

		private async void Postman()
		{
			int fails = 0;
			while (true)
			{
				await ServiceData.WaitExposeAsync();
				try
				{
					_tokenSource = new CancellationTokenSource();
					lock (PauseLock)
					{
						if (!IsPaused)
						{
							ActiveSession.LastUpdateTimestamp = DateTime.UtcNow.ToUnixTime();
						}
					}
					int num;
					if (!ForceSend && ReportedEventsCount < Config.Global.FlushThresholdEventsCounts && LastFlushTime.HasValue)
					{
						DateTime utcNow = DateTime.UtcNow;
						DateTime? lastFlushTime = LastFlushTime;
						num = ((utcNow - lastFlushTime > Config.Global.FlushThresholdTimeout) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					if (num != 0)
					{
						await Refresh();
						bool? flag = Flush();
						if (flag.HasValue && flag.Value)
						{
							LastFlushTime = DateTime.UtcNow;
							ReportedEventsCount = 0;
							ForceSend = false;
							fails = 0;
						}
						if (flag.HasValue)
						{
							this.Snapshot();
						}
						if (flag.HasValue && !flag.Value)
						{
							fails++;
						}
						if (fails > 6)
						{
							fails = 6;
						}
						if (fails > 0)
						{
							await Wait(TimeSpan.FromSeconds(Math.Pow(2.0, fails)));
						}
					}
					await Wait(Config.Global.DispatchPeriod);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
