using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Yandex.Metrica.Aero;

namespace Yandex.Metrica.Models
{
	[DataContract]
	internal class Config : IExposable
	{
		private static readonly TimeSpan MinSessionTimeout = TimeSpan.FromSeconds(10.0);

		private static readonly TimeSpan MinDispatchPeriod = TimeSpan.FromSeconds(4.0);

		private static readonly int MinFlushThresholdEventsCounts = 7;

		private static readonly TimeSpan MinFlushThresholdTimeout = TimeSpan.FromSeconds(4.0);

		private static readonly TimeSpan MinStartupExpirationTimeSpan = TimeSpan.FromDays(1.0);

		private static readonly TimeSpan MinIdentitySendIntervalTimeSpan = TimeSpan.FromDays(1.0);

		public static Config Global { get; internal set; } = Store.Get<Config>(new object[0]);

		[DataMember]
		public Guid ApiKey { get; internal set; }

		[DataMember]
		public bool OfflineMode { get; set; }

		[DataMember]
		public bool CrashTracking { get; set; }

		[DataMember]
		public bool LocationTracking { get; set; }

		[DataMember]
		public string CustomAppId { get; set; }

		[DataMember]
		public Version CustomAppVersion { get; set; }

		[DataMember]
		public TimeSpan SessionTimeout { get; private set; }

		[DataMember]
		public bool HandleFirstActivationAsUpdate { get; set; }

		[DataMember]
		public long MaxCacheSize { get; set; }

		[DataMember]
		public Guid Id { get; private set; }

		[DataMember]
		public List<Guid> KnownKeys { get; set; }

		[DataMember]
		public TimeSpan StartupExpirationTimeSpan { get; set; }

		[DataMember]
		public DateTime StartupTimestamp { get; set; }

		[DataMember]
		public TimeSpan IdentitySendInterval { get; set; }

		[DataMember]
		public DateTime IdentityTimestamp { get; set; }

		[DataMember]
		public DateTime? LastWakeTime { get; set; }

		[DataMember]
		public DateTime? LastLullTime { get; set; }

		[DataMember]
		public TimeSpan DispatchPeriod { get; set; }

		[DataMember]
		public int FlushThresholdEventsCounts { get; private set; }

		[DataMember]
		public TimeSpan FlushThresholdTimeout { get; private set; }

		[DataMember]
		public string CustomStartupUrl { get; set; }

		[DataMember]
		public string ReportUrl { get; set; }

		[DataMember]
		public string UserProfileId { get; set; }

		internal bool IsNew { get; private set; }

		internal DataContractJsonSerializerSettings JsonSerializerSettings { get; set; }

		public Version LibraryVersion => new Version("3.5.1");

		[DataMember]
		public ReportMessage.Location CustomLocation { get; set; }

		void IExposable.Expose()
		{
			IsNew = Id == Guid.Empty;
			Id = (IsNew ? Guid.NewGuid() : Id);
			KnownKeys = KnownKeys ?? new List<Guid>();
			SessionTimeout = (IsNew ? MinSessionTimeout : SessionTimeout);
			DispatchPeriod = (IsNew ? MinDispatchPeriod : DispatchPeriod);
			MaxCacheSize = (IsNew ? 5242880 : MaxCacheSize);
			StartupExpirationTimeSpan = (IsNew ? MinStartupExpirationTimeSpan : StartupExpirationTimeSpan);
			StartupTimestamp = (IsNew ? (DateTime.UtcNow - StartupExpirationTimeSpan) : StartupTimestamp);
			IdentitySendInterval = (IsNew ? MinIdentitySendIntervalTimeSpan : IdentitySendInterval);
			IdentityTimestamp = (IsNew ? (DateTime.UtcNow - IdentitySendInterval) : IdentityTimestamp);
			CrashTracking = IsNew || CrashTracking;
			LocationTracking = IsNew || LocationTracking;
			FlushThresholdEventsCounts = (IsNew ? 7 : FlushThresholdEventsCounts);
			FlushThresholdTimeout = (IsNew ? TimeSpan.FromSeconds(90.0) : FlushThresholdTimeout);
			JsonSerializerSettings = new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			};
			CheckValues();
		}

		private void CheckValues()
		{
			SessionTimeout = ((SessionTimeout < MinSessionTimeout) ? MinSessionTimeout : SessionTimeout);
			DispatchPeriod = ((DispatchPeriod < MinDispatchPeriod) ? MinDispatchPeriod : DispatchPeriod);
			FlushThresholdEventsCounts = ((FlushThresholdEventsCounts < MinFlushThresholdEventsCounts) ? MinFlushThresholdEventsCounts : FlushThresholdEventsCounts);
			FlushThresholdTimeout = ((FlushThresholdTimeout < MinFlushThresholdTimeout) ? MinFlushThresholdTimeout : FlushThresholdTimeout);
			StartupExpirationTimeSpan = ((StartupExpirationTimeSpan < MinStartupExpirationTimeSpan) ? MinStartupExpirationTimeSpan : StartupExpirationTimeSpan);
			IdentitySendInterval = ((IdentitySendInterval < MinIdentitySendIntervalTimeSpan) ? MinIdentitySendIntervalTimeSpan : IdentitySendInterval);
		}

		public void SetCustomLocation(YandexMetrica.Location location)
		{
			CustomLocation = ((location == null) ? null : new ReportMessage.Location
			{
				lat = location.Lat,
				lon = location.Lon,
				speed = location.Speed,
				altitude = location.Altitude,
				direction = location.Direction,
				precision = location.Precision,
				timestamp = location.Timestamp
			});
		}

		public void SetSessionTimeout(TimeSpan value)
		{
			SessionTimeout = value;
			CheckValues();
		}

		public void SetFlushThresholdEventsCounts(int value)
		{
			FlushThresholdEventsCounts = value;
			CheckValues();
		}

		public void SetFlushThresholdTimeout(TimeSpan value)
		{
			FlushThresholdTimeout = value;
			CheckValues();
		}

		internal static string GetLocale()
		{
			return GetSpecificName(CultureInfo.CurrentUICulture, RegionInfo.CurrentRegion.TwoLetterISORegionName);
		}

		internal static string GetSpecificName(CultureInfo culture, string regionName)
		{
			if (!culture.IsNeutralCulture)
			{
				return culture.Name;
			}
			string text = culture.TextInfo.CultureName;
			if (text == culture.Name)
			{
				text = culture.TwoLetterISOLanguageName + "-" + regionName;
			}
			return text;
		}
	}
}
