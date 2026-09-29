using System;

namespace Yandex.Metrica
{
	internal static class MetricaTrace
	{
		private const int MaxMessageLength = 1024;

		public static void Write(string message)
		{
			try
			{
				Action<string> handler = YandexMetrica.Config.TraceHandler;
				if (handler != null)
				{
					handler("[AppMetrica.NET] " + Truncate(message));
				}
			}
			catch
			{
				// Diagnostics must never affect analytics delivery.
			}
		}

		private static string Truncate(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "<empty>";
			}
			value = value.Replace("\r", "\\r").Replace("\n", "\\n");
			return (value.Length <= MaxMessageLength) ? value : (value.Substring(0, MaxMessageLength) + "…");
		}
	}
}
