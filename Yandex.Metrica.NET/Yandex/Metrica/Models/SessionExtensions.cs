using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;

namespace Yandex.Metrica.Models
{
	internal static class SessionExtensions
	{
		public const int MaxValueLength = 50000;

		public const int MaxNameLength = 1000;

		public static string Truncate(this string str, int maxLength, ref uint bytesTruncated)
		{
			if (str == null || str.Length <= maxLength)
			{
				return str;
			}
			string text = str.Substring(0, maxLength);
			bytesTruncated += (uint)(Encoding.UTF8.GetByteCount(str) - Encoding.UTF8.GetByteCount(text));
			return text;
		}

		public static byte[] Truncate(this byte[] bytes, int maxLength, ref uint bytesTruncated)
		{
			if (bytes == null || bytes.Length <= maxLength)
			{
				return bytes;
			}
			bytesTruncated += (uint)(bytes.Length - maxLength);
			return bytes.Take(maxLength).ToArray();
		}

		public static void AggregateEvents(this SessionModel session, ulong currentUnixTime, params ReportMessage.Session.Event[] items)
		{
			if (session?.session_desc?.start_time != null)
			{
				if (items.Length != 0)
				{
					session.LastEventTimestamp = currentUnixTime;
					session.LastEventType = items.Last().type;
				}
				if (Config.Global.LocationTracking)
				{
					session.AttachLocationAsync(items);
				}
				foreach (ReportMessage.Session.Event obj in items)
				{
					uint bytesTruncated = 0u;
					obj.name = obj.name.Truncate(1000, ref bytesTruncated);
					obj.value = obj.value.Truncate(50000, ref bytesTruncated);
					obj.bytes_truncated = bytesTruncated;
					obj.time = currentUnixTime - session.session_desc.start_time.timestamp;
					obj.number = session.EventCounter++;
					session.events.Add(obj);
					obj.AttachNetworkInfoAsync();
				}
			}
		}

		public static async void AttachLocationAsync(this SessionModel session, params ReportMessage.Session.Event[] items)
		{
			ReportMessage.Location location = Config.Global.CustomLocation;
			if (location == null)
			{
				session.AsyncLocationLock = true;
				await ServiceData.WaitExposeAsync();
				ReportMessage.Location location2 = location;
				location = await ServiceData.LocationTracker.Provide();
				session.AsyncLocationLock = false;
			}
			items.ForEach((ReportMessage.Session.Event i) =>
			{
				i.location = location;
			});
		}

		public static async void AttachNetworkInfoAsync(this ReportMessage.Session.Event item)
		{
			await ServiceData.WaitExposeAsync();
			item.network_info = await TaskEx.FromResult(ServiceData.NetworkTracker.Provide());
		}
	}
}
