using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Text;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Models;

namespace Yandex.Metrica.Aides
{
	internal static class DataExtensions
	{
		public static string GlueGetList(this string url, Dictionary<string, object> args, bool removeLastAmp = true)
		{
			StringBuilder stringBuilder = new StringBuilder(url);
			bool flag = false;
			foreach (KeyValuePair<string, object> arg in args)
			{
				if (arg.Value != null)
				{
					stringBuilder.Append<string>(arg.Key, "=", arg.Value.ToString(), "&");
					flag = true;
				}
			}
			if (flag & removeLastAmp)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		public static string ToJsonString<T>(this T obj) where T : class
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new DataContractJsonSerializer(typeof(T)).WriteObject(memoryStream, obj);
				byte[] array = memoryStream.ToArray();
				return Encoding.UTF8.GetString(array, 0, array.Length);
			}
		}

		public static string ToProtobufString<T>(this T obj) where T : ReportMessage
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ReportMessage.Serialize(memoryStream, obj);
				byte[] array = memoryStream.ToArray();
				return Encoding.UTF8.GetString(array, 0, array.Length);
			}
		}

		public static void Write(this ReportMessage message, Stream requestStream)
		{
			using (GZipStream stream = new GZipStream(requestStream, CompressionMode.Compress, true))
			{
				ReportMessage.Serialize(stream, message);
			}
		}
	}
}
