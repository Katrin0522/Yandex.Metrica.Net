using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Yandex.Metrica.Aides
{
	public static class Standard
	{
		public static TItem FromJsonStandard<TItem>(this string json, DataContractJsonSerializerSettings settings = null)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TItem), settings ?? new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true,
				KnownTypes = new List<Type> { typeof(Dictionary<string, object>) }
			});
			using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
			{
				return (TItem)dataContractJsonSerializer.ReadObject(stream);
			}
		}

		public static string ToJsonStandard<TItem>(this TItem item, DataContractJsonSerializerSettings settings = null)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TItem), settings ?? new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true,
				KnownTypes = new List<Type> { typeof(Dictionary<string, object>) }
			});
			using (MemoryStream memoryStream = new MemoryStream())
			{
				dataContractJsonSerializer.WriteObject(memoryStream, item);
				memoryStream.Position = 0L;
				byte[] array = memoryStream.ToArray();
				return Encoding.Unicode.GetString(array, 0, array.Length);
			}
		}
	}
}
