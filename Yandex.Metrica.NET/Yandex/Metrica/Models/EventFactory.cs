using System.Text;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
	internal static class EventFactory
	{
		public static ReportMessage.Session.Event Create(ReportMessage.Session.Event.EventType type, byte[] value = null, string name = null, string environment = null)
		{
			return new ReportMessage.Session.Event
			{
				type = (uint)type,
				name = name,
				value = value,
				environment = environment
			};
		}

		public static ReportMessage.Session.Event Create(ReportMessage.Session.Event.EventType type, string value, string name = null)
		{
			return Create(type, (value == null) ? null : Encoding.UTF8.GetBytes(value), name);
		}

		public static ReportMessage.Session.Event Create(string name, string jsonData = null)
		{
			return Create(ReportMessage.Session.Event.EventType.EVENT_CLIENT, jsonData, name);
		}

		public static ReportMessage.Session.Event Create<TItem>(string name, TItem serializableItem)
		{
			string value = serializableItem.ToJson(JsonProfile.GetFormatted());
			return Create(ReportMessage.Session.Event.EventType.EVENT_CLIENT, value, name);
		}
	}
}
