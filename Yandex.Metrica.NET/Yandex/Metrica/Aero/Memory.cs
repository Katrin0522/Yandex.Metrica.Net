using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;

namespace Yandex.Metrica.Aero
{
	internal class Memory : IMemoryBox
	{
		public readonly DataContractJsonSerializerSettings Settings = new DataContractJsonSerializerSettings
		{
			UseSimpleDictionaryFormat = true,
			KnownTypes = new List<Type>
			{
				typeof(Type),
				typeof(Dictionary<string, string>)
			}
		};

		public static Memory ActiveBox { get; set; }

		public IStorage Storage { get; }

		public string KeyFormat { get; }

		public string IndentChars { get; set; }

		public bool Indent { get; set; }

		public List<Type> KnownTypes => Settings.KnownTypes as List<Type>;

		public Memory(IStorage storage, string keyFormat = "{0}.json", bool indent = true, string indentChars = "  ")
		{
			Storage = storage;
			KeyFormat = keyFormat;
			Indent = indent;
			IndentChars = indentChars;
		}

		public TValue Revive<TValue>(string key = null, params object[] constructorArgs)
		{
			try
			{
				Type typeFromHandle = typeof(TValue);
				if (!System.Attribute.IsDefined(typeFromHandle, typeof(DataContractAttribute)) && !System.Attribute.IsDefined(typeFromHandle, typeof(CollectionDataContractAttribute)))
				{
					return (TValue)Activator.CreateInstance(typeFromHandle, constructorArgs);
				}
				string key2 = MakeStorageKey(key, typeof(TValue));
				using (Stream stream = Storage.GetReadStream(key2))
				{
					CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
					Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
					try
					{
						TValue val = (TValue)new DataContractJsonSerializer(typeFromHandle, Settings).ReadObject(stream);
						if (object.Equals(val, null))
						{
							throw new Exception();
						}
						return val;
					}
					catch (Exception)
					{
						return (TValue)Activator.CreateInstance(typeFromHandle, constructorArgs);
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture;
					}
				}
			}
			catch
			{
				return (TValue)Activator.CreateInstance(typeof(TValue), constructorArgs);
			}
		}

		public void Keep<TValue>(TValue item, string key = null)
		{
			try
			{
				Type type = item.GetType();
			if (!System.Attribute.IsDefined(type, typeof(DataContractAttribute)) && !System.Attribute.IsDefined(type, typeof(CollectionDataContractAttribute)))
				{
					return;
				}
				string key2 = MakeStorageKey(key, type);
				using (Stream stream = Storage.GetWriteStream(key2))
				{
					CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
					Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
					try
					{
						using (XmlDictionaryWriter xmlDictionaryWriter = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, Indent, IndentChars))
						{
							new DataContractJsonSerializer(type, Settings).WriteObject(xmlDictionaryWriter, item);
							xmlDictionaryWriter.Flush();
						}
					}
					catch (Exception)
					{
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public bool Check<TValue>(string key = null)
		{
			return Storage.HasKey(MakeStorageKey(key, typeof(TValue)));
		}

		public void Destroy<TValue>(string key = null)
		{
			Storage.DeleteKey(MakeStorageKey(key, typeof(TValue)));
		}

		private string MakeStorageKey(string key, Type type)
		{
			string text = string.Format(KeyFormat ?? "{0}", key ?? type.Name);
			string directoryName = Path.GetDirectoryName(text);
			if (directoryName != null && !Storage.DirectoryExists(directoryName))
			{
				Storage.CreateDirectory(directoryName);
			}
			return text;
		}
	}
}
