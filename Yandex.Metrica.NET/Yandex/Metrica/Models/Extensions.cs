using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
	internal static class Extensions
	{
		public const int MaxValueLength = 50000;

		public static byte[] ToEventValue(this string value, out bool isTruncated)
		{
			byte[] array = ((value == null) ? null : Encoding.UTF8.GetBytes(value));
			isTruncated = array != null && array.Length > 50000;
			return isTruncated ? array.Take(50000).ToArray() : array;
		}

		public static List<ReportPackage> ToReportPackages(this List<SessionModel> sessions)
		{
			Dictionary<SessionModel, string> dictionary = sessions.ToDictionary((SessionModel s) => s, (SessionModel s) => s.ReportParameters);
			Dictionary<string, List<ReportMessage.Session>> dictionary2 = dictionary.Values.Distinct().ToDictionary((string p) => p, (string p) => new List<ReportMessage.Session>());
			foreach (KeyValuePair<SessionModel, string> item in dictionary)
			{
				dictionary2[item.Value].Add(item.Key);
			}
			List<ReportPackage> list = dictionary2.Select((KeyValuePair<string, List<ReportMessage.Session>> i) => new ReportPackage(i.Key, i.Value)).ToList();
			while (true)
			{
				List<ReportPackage> list2 = list.Where((ReportPackage p) => p.IsLarge).ToList();
				if (list2.Count == 0)
				{
					break;
				}
				foreach (ReportPackage item2 in list2)
				{
					list.Remove(item2);
					list.AddRange(item2.Split());
				}
			}
			return list;
		}

		public static ReportMessage.Location ToMetricaLocation(this Geocoordinate geocoordinate)
		{
			return new ReportMessage.Location
			{
				lat = geocoordinate.Latitude,
				lon = geocoordinate.Longitude,
				precision = (uint)geocoordinate.Accuracy,
				timestamp = geocoordinate.Timestamp.DateTime.ToUnixTime(),
				speed = (geocoordinate.Speed.HasValue ? ((uint)geocoordinate.Speed.Value) : 0u),
				direction = (geocoordinate.Heading.HasValue ? ((uint)geocoordinate.Heading.Value) : 0u),
				altitude = (geocoordinate.Altitude.HasValue ? ((int)geocoordinate.Altitude.Value) : 0)
			};
		}
	}
}
