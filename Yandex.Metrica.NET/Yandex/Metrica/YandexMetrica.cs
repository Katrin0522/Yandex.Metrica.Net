using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aero.Specific;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Legacy;
using Yandex.Metrica.Models;

namespace Yandex.Metrica
{
	public static class YandexMetrica
	{
		[DataContract]
		public class Location
		{
			[DataMember(Name = "lat")]
			public double Lat { get; set; }

			[DataMember(Name = "lon")]
			public double Lon { get; set; }

			[DataMember(Name = "timestamp")]
			public ulong Timestamp { get; set; }

			[DataMember(Name = "precision")]
			public uint Precision { get; set; }

			[DataMember(Name = "direction")]
			public uint Direction { get; set; }

			[DataMember(Name = "speed")]
			public uint Speed { get; set; }

			[DataMember(Name = "altitude")]
			public int Altitude { get; set; }
		}

		public class YandexMetricaConfig
		{
			public Guid ApiKey => InternalConfig.ApiKey;

			public Version LibraryVersion => InternalConfig.LibraryVersion;

			public bool OfflineMode
			{
				get
				{
					return InternalConfig.OfflineMode;
				}
				set
				{
					InternalConfig.OfflineMode = value;
				}
			}

			public bool CrashTracking
			{
				get
				{
					return InternalConfig.CrashTracking;
				}
				set
				{
					InternalConfig.CrashTracking = value;
				}
			}

			public bool LocationTracking
			{
				get
				{
					return InternalConfig.LocationTracking;
				}
				set
				{
					InternalConfig.LocationTracking = value;
				}
			}

			public string CustomAppId
			{
				get
				{
					return InternalConfig.CustomAppId;
				}
				set
				{
					InternalConfig.CustomAppId = value;
				}
			}

			public Version CustomAppVersion
			{
				get
				{
					return InternalConfig.CustomAppVersion;
				}
				set
				{
					InternalConfig.CustomAppVersion = value;
				}
			}

			public TimeSpan SessionTimeout
			{
				get
				{
					return InternalConfig.SessionTimeout;
				}
				set
				{
					InternalConfig.SetSessionTimeout(value);
				}
			}

			public bool HandleFirstActivationAsUpdate
			{
				get
				{
					return InternalConfig.HandleFirstActivationAsUpdate;
				}
				set
				{
					InternalConfig.HandleFirstActivationAsUpdate = value;
				}
			}

			public void SetCustomLocation(Location location)
			{
				InternalConfig.SetCustomLocation(location);
			}
		}

		private static LiteMetricaService _liteMetricaService;

		private static readonly object ActivationLock;

		private static readonly object CacheLock;

		private static readonly List<ReportMessage.Session.Event> Cache;

		public static readonly YandexMetricaConfig Config;

		internal static Yandex.Metrica.Models.Config InternalConfig => Yandex.Metrica.Models.Config.Global;

		static YandexMetrica()
		{
			ActivationLock = new object();
			CacheLock = new object();
			Cache = new List<ReportMessage.Session.Event>();
			Config = new YandexMetricaConfig();
			string current = YandexMetricaFolder.Current;
			if (current == null)
			{
				throw new Exception("You should specify valid 'YandexMetricaFolder.Current' before.");
			}
			Memory.ActiveBox = new Memory(new KeyFileStorage(), Path.Combine(current, "Yandex.Metrica.{0}.json"));
			Lifecycler lifecycler = Store.Get<Lifecycler>(new object[0]);
			lifecycler.UnhandledException += (object sender, EventArgs args) =>
			{
				if (Adapter.IsInternalException(args))
				{
					Adapter.TryHandleException(args);
					Memory.ActiveBox.Destroy<LiteMetricaService>();
					Memory.ActiveBox.Destroy<Yandex.Metrica.Models.Config>();
					Store.Container.Remove(typeof(LiteMetricaService));
					Store.Container.Remove(typeof(Yandex.Metrica.Models.Config));
					_liteMetricaService = Store.Get<LiteMetricaService>(new object[0]);
				}
			};
			if (lifecycler.IsBackgroundTask)
			{
				Memory.ActiveBox = new Memory(new KeyFileStorage(), Path.Combine(current, "Yandex.Metrica.{0}.b.json"));
			}
		}

		private static void Report(ReportMessage.Session.Event item)
		{
			if (InternalConfig.ApiKey == Guid.Empty)
			{
				throw new ArgumentException("ApiKey is empty");
			}
			if (_liteMetricaService == null)
			{
				lock (CacheLock)
				{
					if (_liteMetricaService == null)
					{
						Cache.Add(item);
					}
					return;
				}
			}
			_liteMetricaService.Report(item);
		}

		private static void ActivateInternal(Guid apiKey)
		{
			try
			{
				LegacyManager.CompleteMigration().RunSynchronously();
			}
			catch (Exception)
			{
			}
			MigrateApiKeys();
			LiteMetricaService liteMetricaService = Store.Get<LiteMetricaService>(new object[0]);
			if (Critical.GetApiKeys().Contains(apiKey))
			{
				liteMetricaService.Wake(false, true);
			}
			else
			{
				Critical.AddApiKey(apiKey);
				liteMetricaService.Wake(true, true);
				Critical.Submit();
			}
			lock (CacheLock)
			{
				liteMetricaService.Report(Cache.ToArray());
				Cache.Clear();
				_liteMetricaService = liteMetricaService;
			}
			liteMetricaService.ForceSend = true;
		}

		private static void MigrateApiKeys()
		{
			Guid[] apiKeys = Critical.GetApiKeys();
			Guid[] array = InternalConfig.KnownKeys.Where((Guid k) => !apiKeys.Contains(k)).ToArray();
			array.ForEach(Critical.AddApiKey);
			if (array.Any())
			{
				Critical.Submit();
			}
		}

		internal static void Reset()
		{
			string customStartupUrl = Yandex.Metrica.Models.Config.Global.CustomStartupUrl;
			Guid apiKey = Yandex.Metrica.Models.Config.Global.ApiKey;
			Memory.ActiveBox.Destroy<Yandex.Metrica.Models.Config>();
			Memory.ActiveBox.Destroy<Critical.CriticalConfig>();
			Memory.ActiveBox.Destroy<LiteMetricaService>();
			Critical.SetUuid(null);
			Store.Container.Remove(typeof(LiteMetricaService));
			Store.Container.Remove(typeof(Yandex.Metrica.Models.Config));
			Yandex.Metrica.Models.Config config = Store.Get<Yandex.Metrica.Models.Config>(new object[0]);
			config.ApiKey = apiKey;
			config.CustomStartupUrl = customStartupUrl;
			config.Snapshot();
			Yandex.Metrica.Models.Config.Global = config;
			_liteMetricaService = null;
		}

		public static void ReportEvent(string eventName)
		{
			Report(EventFactory.Create(eventName));
		}

		public static void ReportEvent(string eventName, string jsonData)
		{
			Report(EventFactory.Create(eventName, jsonData));
		}

		public static void ReportEvent<TItem>(string eventName, TItem serializableItem)
		{
			Report(EventFactory.Create(eventName, serializableItem));
		}

		public static void ReportUnhandledException(Exception exсeption)
		{
			Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_CRASH, exсeption.ToString()));
		}

		public static void ReportError(string message, Exception exсeption)
		{
			Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_ERROR, exсeption.ToString(), message));
		}

		public static void ReportLaunchUri(Uri uri)
		{
			if (!(uri == null) && !string.IsNullOrEmpty(uri.AbsoluteUri))
			{
				Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_OPEN, "{\"link\":" + uri.ToJson() + ",\"type\":\"open\"}"));
			}
		}

		internal static void ReportInternalEvent(int type, string name, string value, Dictionary<string, object> environment)
		{
			if (type < 1 || type > 99 || type == 14 || type == 15)
			{
				byte[] value2 = ((value == null) ? null : Encoding.UTF8.GetBytes(value));
				string environment2 = environment?.ToJson(JsonProfile.GetCompact());
				Report(EventFactory.Create((ReportMessage.Session.Event.EventType)type, value2, name, environment2));
			}
		}

		public static void Snapshot()
		{
			if (_liteMetricaService == null)
			{
				int num = 0;
				while (_liteMetricaService == null && num < 7)
				{
					TaskEx.Delay(TimeSpan.FromMilliseconds(250.0)).Wait();
					num++;
				}
			}
			if (_liteMetricaService != null)
			{
				_liteMetricaService.Lull();
				_liteMetricaService.ForceSend = true;
				_liteMetricaService.Flush();
			}
		}

		public static void Activate(string apiKey)
		{
			Activate(new Guid(apiKey));
		}

		public static void Activate(Guid apiKey)
		{
			InternalConfig.ApiKey = apiKey;
			Task.Factory.StartNew(() =>
			{
				lock (ActivationLock)
				{
					ActivateInternal(apiKey);
				}
			});
		}
	}
}
