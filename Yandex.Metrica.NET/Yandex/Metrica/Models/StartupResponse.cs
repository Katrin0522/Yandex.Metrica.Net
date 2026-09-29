using System;
using System.Runtime.Serialization;

namespace Yandex.Metrica.Models
{
	[DataContract]
	internal class StartupResponse
	{
		[DataContract]
		internal class ValueContainer
		{
			[DataMember(Name = "value")]
			public string Value { get; set; }
		}

		[DataContract]
		internal class HostContainer
		{
			[DataContract]
			internal class HostList
			{
				[DataContract]
				public class Host
				{
					[DataMember(Name = "url")]
					public string Url { get; set; }
				}

				[DataMember(Name = "report")]
				public Host Report { get; set; }
			}

			[DataMember(Name = "list")]
			internal HostList List { get; set; }
		}

		[DataMember(Name = "query_hosts")]
		public HostContainer QueryHosts { get; set; }

		[DataMember(Name = "uuid")]
		public ValueContainer UuidContainer { get; set; }

		[DataMember(Name = "device_id")]
		public ValueContainer DeviceIdContainer { get; set; }

		[DataMember]
		public TimeSpan ServerTimeOffset { get; set; }

		[DataMember]
		public DateTime ServerDateTime { get; set; }

		public string Uuid => UuidContainer?.Value;

		public string DeviceId => DeviceIdContainer?.Value;

		public string ReportUrl => QueryHosts?.List?.Report?.Url;
	}
}
