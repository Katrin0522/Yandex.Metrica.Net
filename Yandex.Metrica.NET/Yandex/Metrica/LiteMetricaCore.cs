using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;

namespace Yandex.Metrica
{
	[DataContract]
	internal class LiteMetricaCore : IExposable
	{
		protected object ActiveSessionLock;

		protected object PauseLock;

		public bool IsActivated => Config.Global.ApiKey != Guid.Empty;

		public bool IsPaused { get; private set; }

		[DataMember]
		protected SessionModel ActiveSession { get; set; }

		[DataMember]
		protected List<SessionModel> CompletedSessions { get; set; }

		[DataMember]
		protected List<ReportPackage> ReportPackages { get; set; }

		[DataMember]
		public int ReportedEventsCount { get; set; }

		[DataMember]
		public bool ForceSend { get; set; }

		public virtual async void Expose()
		{
			ActiveSessionLock = new object();
			PauseLock = new object();
			ActiveSession = ActiveSession ?? CreateSession();
			ReportPackages = ReportPackages ?? new List<ReportPackage>();
			CompletedSessions = CompletedSessions ?? new List<SessionModel>();
			SessionModel activeSession = ActiveSession;
			string text = ActiveSession.ReportParameters;
			if (text == null)
			{
				text = "".GlueGetList(await ServiceData.GetReportParameters());
			}
			activeSession.ReportParameters = text;
			IsPaused = true;
		}

		public virtual void TriggerForcedSend()
		{
			ForceSend = true;
		}

		public async void Wake(bool activateApiKey = false, bool forceWake = false)
		{
			if (!IsActivated)
			{
				return;
			}
			bool flag = false;
			lock (PauseLock)
			{
				bool isPaused = IsPaused;
				IsPaused = false;
				DateTime? lastLullTime = Config.Global.LastLullTime;
				int num;
				if (lastLullTime.HasValue)
				{
					DateTime utcNow = DateTime.UtcNow;
					DateTime? dateTime = lastLullTime;
					num = ((utcNow - dateTime > Config.Global.SessionTimeout) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag2 = (byte)num != 0;
				if (forceWake || (isPaused & flag2))
				{
					flag = true;
				}
			}
			if (flag)
			{
				SessionModel sessionModel = StartSession(activateApiKey);
				SessionModel sessionModel2 = sessionModel;
				string reportParameters = "".GlueGetList(await ServiceData.GetReportParameters());
				sessionModel2.ReportParameters = reportParameters;
				await ReportIdentityEvent();
			}
		}

		public void Lull()
		{
			if (IsActivated)
			{
				lock (PauseLock)
				{
					IsPaused = true;
					PauseSession();
					Config.Global.LastLullTime = DateTime.UtcNow;
				}
				Store.Snapshot();
			}
		}

		public bool? Flush()
		{
			if (CompletedSessions.Count == 0 && ActiveSession.events.Count == 0 && ReportPackages.Count == 0)
			{
				return null;
			}
			SessionModel activeSession;
			lock (ActiveSessionLock)
			{
				activeSession = ActiveSession;
				ActiveSession = new SessionModel
				{
					id = ActiveSession.id,
					events = new List<ReportMessage.Session.Event>(),
					EventCounter = activeSession.EventCounter,
					session_desc = activeSession.session_desc,
					ReportParameters = activeSession.ReportParameters,
					LastUpdateTimestamp = activeSession.LastUpdateTimestamp,
					LastEventTimestamp = activeSession.LastEventTimestamp,
					LastEventType = activeSession.LastEventType
				};
			}
			return FlushCompletedSessions(activeSession);
		}

		private bool? FlushCompletedSessions(SessionModel sourceSession)
		{
			lock (CompletedSessions)
			{
				if (sourceSession.events.Count > 0)
				{
					CompletedSessions.Add(sourceSession);
				}
				CompletedSessions.Where((SessionModel s) => s.ReportParameters == null).ForEach(async (SessionModel s) =>
				{
					string reportParameters = "".GlueGetList(await ServiceData.GetReportParameters());
					s.ReportParameters = reportParameters;
				});
				List<SessionModel> list = CompletedSessions.Where((SessionModel s) => !s.AsyncLocationLock).ToList();
				List<ReportPackage> collection = list.ToReportPackages();
				ReportPackages.AddRange(collection);
				list.ForEach((SessionModel s) =>
				{
					CompletedSessions.Remove(s);
				});
				if (ReportPackages.Count == 0)
				{
					return false;
				}
				ReportPackage[] array = ReportPackages.ToArray();
				foreach (ReportPackage reportPackage in array)
				{
					if (!Config.Global.OfflineMode && TryPostOrIgnore(reportPackage))
					{
						ReportPackages.Remove(reportPackage);
						reportPackage.Fade();
					}
				}
				RemoveOverflowedPackages(ReportPackages);
				ReportPackages.Where((ReportPackage p) => !p.Exists()).ForEach((ReportPackage p) =>
				{
					p.Keep();
				});
				return ReportPackages.Count == 0;
			}
		}

		private static bool TryPostOrIgnore(ReportPackage package)
		{
			try
			{
				if (package.Length <= 0)
				{
					throw new Exception("Empty package");
				}
				HttpResponseMessage result = package.PostAsync().Result;
				if (result == null)
				{
					return false;
				}
				return result.IsSuccessStatusCode || !IsValidRequestByStatusCode(result.StatusCode);
			}
			catch (Exception)
			{
				return true;
			}
		}

		public static bool IsValidRequestByStatusCode(HttpStatusCode code)
		{
			if (code != HttpStatusCode.BadRequest && code != HttpStatusCode.RequestEntityTooLarge)
			{
				return code != HttpStatusCode.RequestUriTooLong;
			}
			return false;
		}

		public void RemoveOverflowedPackages(List<ReportPackage> packages)
		{
			while (packages.Count > 0)
			{
				long num = packages.Aggregate(0L, (long size, ReportPackage package) => size + package.Length);
				long maxCacheSize = Config.Global.MaxCacheSize;
				if (maxCacheSize > 0 && num >= maxCacheSize)
				{
					packages[0].Fade();
					packages.RemoveAt(0);
					continue;
				}
				break;
			}
		}

		protected async Task<bool?> Refresh()
		{
			bool flag = DateTime.UtcNow - Config.Global.StartupTimestamp < Config.Global.StartupExpirationTimeSpan;
			bool flag2 = Critical.IsUuidRequired() || Critical.IsDeviceIdRequired() || string.IsNullOrWhiteSpace(Config.Global.ReportUrl);
			if (flag && !flag2)
			{
				return null;
			}
			if (!(await LiteClient.RefreshStartupAsync()))
			{
				return false;
			}
			Config.Global.StartupTimestamp = DateTime.UtcNow;
			Config.Global.Snapshot();
			return true;
		}

		private async Task ReportIdentityEvent()
		{
			if (!IsActivated)
			{
				return;
			}
			bool flag;
			lock (ActiveSessionLock)
			{
				if (ActiveSession == null || ActiveSession.EventCounter == 0L)
				{
					return;
				}
				DateTime identityTimestamp = Config.Global.IdentityTimestamp;
				flag = DateTime.UtcNow - identityTimestamp >= Config.Global.IdentitySendInterval;
				if (flag)
				{
					Config.Global.IdentityTimestamp = DateTime.UtcNow;
				}
			}
			if (flag)
			{
				await ServiceData.WaitExposeAsync();
				byte[] bytes = Encoding.UTF8.GetBytes(ServiceData.DeviceFingerprint);
				Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_IDENTITY, bytes));
				TriggerForcedSend();
			}
		}

		private SessionModel StartSession(bool isFirstSession = false)
		{
			SessionModel activeSession;
			lock (ActiveSessionLock)
			{
				EnsureActiveSessionFinished();
				if (ActiveSession != null && ActiveSession.EventCounter != 0)
				{
					lock (CompletedSessions)
					{
						CompletedSessions.Add(ActiveSession);
					}
				}
				ActiveSession = CreateSession();
				Report((!isFirstSession) ? new ReportMessage.Session.Event[1] { EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_START) } : new ReportMessage.Session.Event[3]
				{
					EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_FIRST),
					EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_START),
					EventFactory.Create((!Config.Global.HandleFirstActivationAsUpdate) ? ReportMessage.Session.Event.EventType.EVENT_INIT : ReportMessage.Session.Event.EventType.EVENT_UPDATE)
				});
				Config.Global.LastWakeTime = DateTime.UtcNow;
				activeSession = ActiveSession;
			}
			TriggerForcedSend();
			return activeSession;
		}

		private void PauseSession(ulong? pauseTimestamp = null)
		{
			lock (ActiveSessionLock)
			{
				if (ActiveSession != null && ActiveSession.EventCounter != 0L)
				{
					ulong timestamp = pauseTimestamp ?? DateTime.UtcNow.ToUnixTime();
					Report(timestamp, EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_ALIVE));
				}
			}
		}

		private void EnsureActiveSessionFinished()
		{
			lock (ActiveSessionLock)
			{
				if (ActiveSession == null || ActiveSession.EventCounter == 0L || (ActiveSession.LastEventType.HasValue && ActiveSession.LastEventType.Value == 7))
				{
					return;
				}
				ulong? num = ActiveSession.LastUpdateTimestamp ?? ActiveSession.LastEventTimestamp;
				if (num.HasValue)
				{
					if (ActiveSession.LastEventTimestamp.HasValue && num < ActiveSession.LastEventTimestamp)
					{
						num = ActiveSession.LastEventTimestamp;
					}
					PauseSession(num.Value);
				}
			}
		}

		private static SessionModel CreateSession()
		{
			ulong num = DateTime.UtcNow.ToUnixTime();
			return new SessionModel
			{
				id = num,
				events = new List<ReportMessage.Session.Event>(),
				session_desc = new ReportMessage.Session.SessionDesc
				{
					locale = Config.GetLocale(),
					session_type = (ServiceData.Lifecycler.IsBackgroundTask ? ReportMessage.Session.SessionDesc.SessionType.SESSION_BACKGROUND : ReportMessage.Session.SessionDesc.SessionType.SESSION_FOREGROUND),
					start_time = new ReportMessage.Time
					{
						timestamp = num,
						time_zone = (int)DateTimeOffset.Now.Offset.TotalSeconds
					}
				}
			};
		}

		public void Report(params ReportMessage.Session.Event[] items)
		{
			Report(DateTime.UtcNow.ToUnixTime(), items);
		}

		public void Report(ulong timestamp, params ReportMessage.Session.Event[] items)
		{
			if (Config.Global.ApiKey == Guid.Empty)
			{
				throw new ArgumentException("ApiKey is empty");
			}
			lock (ActiveSessionLock)
			{
				ActiveSession.AggregateEvents(timestamp, items);
				ReportedEventsCount += items.Length;
			}
		}
	}
}
