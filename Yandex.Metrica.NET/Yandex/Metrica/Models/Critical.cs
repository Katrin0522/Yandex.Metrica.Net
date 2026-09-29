using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
	internal static class Critical
	{
		[DataContract]
		internal class CriticalConfig
		{
			[DataMember]
			public string DeviceId { get; set; }

			[DataMember]
			public string Uuid { get; set; }

			[DataMember]
			public List<Guid> ApiKeys { get; set; }
		}

		private static readonly object UuidChangesLock;

		private static CriticalConfig Data { get; }

		static Critical()
		{
			if (Memory.ActiveBox.Check<CriticalConfig>())
			{
				Data = Memory.ActiveBox.Revive<CriticalConfig>(null, new object[0]);
			}
			else
			{
				Data = new CriticalConfig();
				SetDeviceId(Identification.GetDeviceId());
			}
			Data.ApiKeys = Data.ApiKeys ?? new List<Guid>();
			UuidChangesLock = new object();
		}

		public static void SetUuid(string uuid)
		{
			if (uuid == null)
			{
				return;
			}
			bool flag = false;
			lock (UuidChangesLock)
			{
				if (Data.Uuid == null)
				{
					flag = true;
					Data.Uuid = uuid;
				}
			}
			if (flag)
			{
				Submit();
			}
		}

		public static void SetDeviceId(string deviceId)
		{
			Data.DeviceId = deviceId;
			Submit();
		}

		public static void Submit()
		{
			Memory.ActiveBox.Keep(Data);
		}

		public static string GetUuid()
		{
			return Data.Uuid;
		}

		public static string GetDeviceId()
		{
			return Data.DeviceId;
		}

		public static bool IsUuidRequired()
		{
			return string.IsNullOrWhiteSpace(GetUuid());
		}

		public static bool IsDeviceIdRequired()
		{
			return string.IsNullOrWhiteSpace(GetDeviceId());
		}

		public static void AddApiKey(Guid apiKey)
		{
			Data.ApiKeys.Add(apiKey);
		}

		public static Guid[] GetApiKeys()
		{
			return Data.ApiKeys.ToArray();
		}
	}
}
