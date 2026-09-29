using System;
using System.Reflection;
using System.Text;

namespace Yandex.Metrica.Aides
{
	internal static class Adapter
	{
		public static Type GetTypeInfo(this Type type)
		{
			return type;
		}

		public static Assembly GetEntryAssembly()
		{
			return Assembly.GetEntryAssembly();
		}

		public static byte[] ExtractData(object o)
		{
			UnhandledExceptionEventArgs e = o as UnhandledExceptionEventArgs;
			if (e != null)
			{
				return Encoding.UTF8.GetBytes(e.ExceptionObject.ToString());
			}
			return null;
		}

		public static bool IsInternalException(object o)
		{
			return (o as UnhandledExceptionEventArgs)?.ExceptionObject.ToString().Contains("Yandex.Metrica") ?? false;
		}

		public static void TryHandleException(object o)
		{
		}
	}
}
