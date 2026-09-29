using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using SilentOrbit.ProtocolBuffers;

namespace Yandex.Metrica.Models
{
	[DataContract]
	internal class ReportMessage
	{
		public enum OptionalBool
		{
			OPTIONAL_BOOL_UNDEFINED = -1,
			OPTIONAL_BOOL_FALSE,
			OPTIONAL_BOOL_TRUE
		}

		[DataContract]
		public class Time
		{
			[DataMember]
			public ulong timestamp { get; set; }

			[DataMember]
			public int time_zone { get; set; }

			[DataMember]
			public long? server_time_offset { get; set; }

			public static Time Deserialize(Stream stream)
			{
				Time time = new Time();
				Deserialize(stream, time);
				return time;
			}

			public static Time DeserializeLengthDelimited(Stream stream)
			{
				Time time = new Time();
				DeserializeLengthDelimited(stream, time);
				return time;
			}

			public static Time DeserializeLength(Stream stream, int length)
			{
				Time time = new Time();
				DeserializeLength(stream, length, time);
				return time;
			}

			public static Time Deserialize(byte[] buffer)
			{
				Time time = new Time();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, time);
					return time;
				}
			}

			public static Time Deserialize(byte[] buffer, Time instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static Time Deserialize(Stream stream, Time instance)
			{
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 8:
						instance.timestamp = ProtocolParser.ReadUInt64(stream);
						continue;
					case 16:
						instance.time_zone = ProtocolParser.ReadZInt32(stream);
						continue;
					case 24:
						instance.server_time_offset = (long)ProtocolParser.ReadUInt64(stream);
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
			}

			public static Time DeserializeLengthDelimited(Stream stream, Time instance)
			{
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 8:
						instance.timestamp = ProtocolParser.ReadUInt64(stream);
						continue;
					case 16:
						instance.time_zone = ProtocolParser.ReadZInt32(stream);
						continue;
					case 24:
						instance.server_time_offset = (long)ProtocolParser.ReadUInt64(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static Time DeserializeLength(Stream stream, int length, Time instance)
			{
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 8:
						instance.timestamp = ProtocolParser.ReadUInt64(stream);
						continue;
					case 16:
						instance.time_zone = ProtocolParser.ReadZInt32(stream);
						continue;
					case 24:
						instance.server_time_offset = (long)ProtocolParser.ReadUInt64(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, Time instance)
			{
				MemoryStream stream2 = ProtocolParser.Stack.Pop();
				stream.WriteByte(8);
				ProtocolParser.WriteUInt64(stream, instance.timestamp);
				stream.WriteByte(16);
				ProtocolParser.WriteZInt32(stream, instance.time_zone);
				if (instance.server_time_offset.HasValue)
				{
					stream.WriteByte(24);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.server_time_offset.Value);
				}
				ProtocolParser.Stack.Push(stream2);
			}

			public static byte[] SerializeToBytes(Time instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, Time instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataContract]
		public class Location
		{
			public enum Provider
			{
				PROVIDER_UNKNOWN,
				PROVIDER_GPS,
				PROVIDER_NETWORK
			}

			[DataMember]
			public double lat { get; set; }

			[DataMember]
			public double lon { get; set; }

			[DataMember]
			public ulong? timestamp { get; set; }

			[DataMember]
			public uint? precision { get; set; }

			[DataMember]
			public uint? direction { get; set; }

			[DataMember]
			public uint? speed { get; set; }

			[DataMember]
			public int? altitude { get; set; }

			[DataMember]
			public Provider? provider { get; set; }

			[DataMember]
			public OptionalBool? enabled { get; set; }

			public static Location Deserialize(Stream stream)
			{
				Location location = new Location();
				Deserialize(stream, location);
				return location;
			}

			public static Location DeserializeLengthDelimited(Stream stream)
			{
				Location location = new Location();
				DeserializeLengthDelimited(stream, location);
				return location;
			}

			public static Location DeserializeLength(Stream stream, int length)
			{
				Location location = new Location();
				DeserializeLength(stream, length, location);
				return location;
			}

			public static Location Deserialize(byte[] buffer)
			{
				Location location = new Location();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, location);
					return location;
				}
			}

			public static Location Deserialize(byte[] buffer, Location instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static Location Deserialize(Stream stream, Location instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				instance.provider = Provider.PROVIDER_UNKNOWN;
				instance.enabled = OptionalBool.OPTIONAL_BOOL_UNDEFINED;
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 9:
						instance.lat = binaryReader.ReadDouble();
						continue;
					case 17:
						instance.lon = binaryReader.ReadDouble();
						continue;
					case 24:
						instance.timestamp = ProtocolParser.ReadUInt64(stream);
						continue;
					case 32:
						instance.precision = ProtocolParser.ReadUInt32(stream);
						continue;
					case 40:
						instance.direction = ProtocolParser.ReadUInt32(stream);
						continue;
					case 48:
						instance.speed = ProtocolParser.ReadUInt32(stream);
						continue;
					case 56:
						instance.altitude = (int)ProtocolParser.ReadUInt64(stream);
						continue;
					case 64:
						instance.provider = (Provider)ProtocolParser.ReadUInt64(stream);
						continue;
					case 72:
						instance.enabled = (OptionalBool)ProtocolParser.ReadUInt64(stream);
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
			}

			public static Location DeserializeLengthDelimited(Stream stream, Location instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				instance.provider = Provider.PROVIDER_UNKNOWN;
				instance.enabled = OptionalBool.OPTIONAL_BOOL_UNDEFINED;
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 9:
						instance.lat = binaryReader.ReadDouble();
						continue;
					case 17:
						instance.lon = binaryReader.ReadDouble();
						continue;
					case 24:
						instance.timestamp = ProtocolParser.ReadUInt64(stream);
						continue;
					case 32:
						instance.precision = ProtocolParser.ReadUInt32(stream);
						continue;
					case 40:
						instance.direction = ProtocolParser.ReadUInt32(stream);
						continue;
					case 48:
						instance.speed = ProtocolParser.ReadUInt32(stream);
						continue;
					case 56:
						instance.altitude = (int)ProtocolParser.ReadUInt64(stream);
						continue;
					case 64:
						instance.provider = (Provider)ProtocolParser.ReadUInt64(stream);
						continue;
					case 72:
						instance.enabled = (OptionalBool)ProtocolParser.ReadUInt64(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static Location DeserializeLength(Stream stream, int length, Location instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				instance.provider = Provider.PROVIDER_UNKNOWN;
				instance.enabled = OptionalBool.OPTIONAL_BOOL_UNDEFINED;
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 9:
						instance.lat = binaryReader.ReadDouble();
						continue;
					case 17:
						instance.lon = binaryReader.ReadDouble();
						continue;
					case 24:
						instance.timestamp = ProtocolParser.ReadUInt64(stream);
						continue;
					case 32:
						instance.precision = ProtocolParser.ReadUInt32(stream);
						continue;
					case 40:
						instance.direction = ProtocolParser.ReadUInt32(stream);
						continue;
					case 48:
						instance.speed = ProtocolParser.ReadUInt32(stream);
						continue;
					case 56:
						instance.altitude = (int)ProtocolParser.ReadUInt64(stream);
						continue;
					case 64:
						instance.provider = (Provider)ProtocolParser.ReadUInt64(stream);
						continue;
					case 72:
						instance.enabled = (OptionalBool)ProtocolParser.ReadUInt64(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, Location instance)
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				MemoryStream stream2 = ProtocolParser.Stack.Pop();
				stream.WriteByte(9);
				binaryWriter.Write(instance.lat);
				stream.WriteByte(17);
				binaryWriter.Write(instance.lon);
				if (instance.timestamp.HasValue)
				{
					stream.WriteByte(24);
					ProtocolParser.WriteUInt64(stream, instance.timestamp.Value);
				}
				if (instance.precision.HasValue)
				{
					stream.WriteByte(32);
					ProtocolParser.WriteUInt32(stream, instance.precision.Value);
				}
				if (instance.direction.HasValue)
				{
					stream.WriteByte(40);
					ProtocolParser.WriteUInt32(stream, instance.direction.Value);
				}
				if (instance.speed.HasValue)
				{
					stream.WriteByte(48);
					ProtocolParser.WriteUInt32(stream, instance.speed.Value);
				}
				if (instance.altitude.HasValue)
				{
					stream.WriteByte(56);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.altitude.Value);
				}
				if (instance.provider.HasValue)
				{
					stream.WriteByte(64);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.provider.Value);
				}
				if (instance.enabled.HasValue)
				{
					stream.WriteByte(72);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.enabled.Value);
				}
				ProtocolParser.Stack.Push(stream2);
			}

			public static byte[] SerializeToBytes(Location instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, Location instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataContract]
		public class Session
		{
			public enum ConnectionType
			{
				CONNECTION_CELL,
				CONNECTION_WIFI,
				CONNECTION_UNDEFINED
			}

			[DataContract]
			public class WifiNetworkInfo
			{
				[DataMember]
				public string mac { get; set; }

				[DataMember]
				public int? signal_strength { get; set; }

				[DataMember]
				public string ssid { get; set; }

				[DataMember]
				public bool? is_connected { get; set; }

				public static WifiNetworkInfo Deserialize(Stream stream)
				{
					WifiNetworkInfo wifiNetworkInfo = new WifiNetworkInfo();
					Deserialize(stream, wifiNetworkInfo);
					return wifiNetworkInfo;
				}

				public static WifiNetworkInfo DeserializeLengthDelimited(Stream stream)
				{
					WifiNetworkInfo wifiNetworkInfo = new WifiNetworkInfo();
					DeserializeLengthDelimited(stream, wifiNetworkInfo);
					return wifiNetworkInfo;
				}

				public static WifiNetworkInfo DeserializeLength(Stream stream, int length)
				{
					WifiNetworkInfo wifiNetworkInfo = new WifiNetworkInfo();
					DeserializeLength(stream, length, wifiNetworkInfo);
					return wifiNetworkInfo;
				}

				public static WifiNetworkInfo Deserialize(byte[] buffer)
				{
					WifiNetworkInfo wifiNetworkInfo = new WifiNetworkInfo();
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, wifiNetworkInfo);
						return wifiNetworkInfo;
					}
				}

				public static WifiNetworkInfo Deserialize(byte[] buffer, WifiNetworkInfo instance)
				{
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, instance);
						return instance;
					}
				}

				public static WifiNetworkInfo Deserialize(Stream stream, WifiNetworkInfo instance)
				{
					instance.is_connected = false;
					while (true)
					{
						int num = stream.ReadByte();
						switch (num)
						{
						case 10:
							instance.mac = ProtocolParser.ReadString(stream);
							continue;
						case 16:
							instance.signal_strength = ProtocolParser.ReadZInt32(stream);
							continue;
						case 26:
							instance.ssid = ProtocolParser.ReadString(stream);
							continue;
						case 32:
							instance.is_connected = ProtocolParser.ReadBool(stream);
							continue;
						case -1:
							return instance;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
				}

				public static WifiNetworkInfo DeserializeLengthDelimited(Stream stream, WifiNetworkInfo instance)
				{
					instance.is_connected = false;
					long num = ProtocolParser.ReadUInt32(stream);
					num += stream.Position;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 10:
							instance.mac = ProtocolParser.ReadString(stream);
							continue;
						case 16:
							instance.signal_strength = ProtocolParser.ReadZInt32(stream);
							continue;
						case 26:
							instance.ssid = ProtocolParser.ReadString(stream);
							continue;
						case 32:
							instance.is_connected = ProtocolParser.ReadBool(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static WifiNetworkInfo DeserializeLength(Stream stream, int length, WifiNetworkInfo instance)
				{
					instance.is_connected = false;
					long num = stream.Position + length;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 10:
							instance.mac = ProtocolParser.ReadString(stream);
							continue;
						case 16:
							instance.signal_strength = ProtocolParser.ReadZInt32(stream);
							continue;
						case 26:
							instance.ssid = ProtocolParser.ReadString(stream);
							continue;
						case 32:
							instance.is_connected = ProtocolParser.ReadBool(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static void Serialize(Stream stream, WifiNetworkInfo instance)
				{
					MemoryStream stream2 = ProtocolParser.Stack.Pop();
					if (instance.mac == null)
					{
						throw new ProtocolBufferException("mac is required by the proto specification.");
					}
					stream.WriteByte(10);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.mac));
					if (instance.signal_strength.HasValue)
					{
						stream.WriteByte(16);
						ProtocolParser.WriteZInt32(stream, instance.signal_strength.Value);
					}
					if (instance.ssid != null)
					{
						stream.WriteByte(26);
						ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ssid));
					}
					if (instance.is_connected.HasValue)
					{
						stream.WriteByte(32);
						ProtocolParser.WriteBool(stream, instance.is_connected.Value);
					}
					ProtocolParser.Stack.Push(stream2);
				}

				public static byte[] SerializeToBytes(WifiNetworkInfo instance)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						Serialize(memoryStream, instance);
						return memoryStream.ToArray();
					}
				}

				public static void SerializeLengthDelimited(Stream stream, WifiNetworkInfo instance)
				{
					byte[] array = SerializeToBytes(instance);
					ProtocolParser.WriteUInt32(stream, (uint)array.Length);
					stream.Write(array, 0, array.Length);
				}
			}

			[DataContract]
			public class SessionDesc
			{
				public enum SessionType
				{
					SESSION_FOREGROUND,
					SESSION_BACKGROUND
				}

				[DataContract]
				public class Location
				{
					[DataMember]
					public double lat { get; set; }

					[DataMember]
					public double lon { get; set; }

					public static Location Deserialize(Stream stream)
					{
						Location location = new Location();
						Deserialize(stream, location);
						return location;
					}

					public static Location DeserializeLengthDelimited(Stream stream)
					{
						Location location = new Location();
						DeserializeLengthDelimited(stream, location);
						return location;
					}

					public static Location DeserializeLength(Stream stream, int length)
					{
						Location location = new Location();
						DeserializeLength(stream, length, location);
						return location;
					}

					public static Location Deserialize(byte[] buffer)
					{
						Location location = new Location();
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, location);
							return location;
						}
					}

					public static Location Deserialize(byte[] buffer, Location instance)
					{
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, instance);
							return instance;
						}
					}

					public static Location Deserialize(Stream stream, Location instance)
					{
						BinaryReader binaryReader = new BinaryReader(stream);
						while (true)
						{
							int num = stream.ReadByte();
							switch (num)
							{
							case 9:
								instance.lat = binaryReader.ReadDouble();
								continue;
							case 17:
								instance.lon = binaryReader.ReadDouble();
								continue;
							case -1:
								return instance;
							}
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
					}

					public static Location DeserializeLengthDelimited(Stream stream, Location instance)
					{
						BinaryReader binaryReader = new BinaryReader(stream);
						long num = ProtocolParser.ReadUInt32(stream);
						num += stream.Position;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 9:
								instance.lat = binaryReader.ReadDouble();
								continue;
							case 17:
								instance.lon = binaryReader.ReadDouble();
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static Location DeserializeLength(Stream stream, int length, Location instance)
					{
						BinaryReader binaryReader = new BinaryReader(stream);
						long num = stream.Position + length;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 9:
								instance.lat = binaryReader.ReadDouble();
								continue;
							case 17:
								instance.lon = binaryReader.ReadDouble();
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static void Serialize(Stream stream, Location instance)
					{
						BinaryWriter binaryWriter = new BinaryWriter(stream);
						MemoryStream stream2 = ProtocolParser.Stack.Pop();
						stream.WriteByte(9);
						binaryWriter.Write(instance.lat);
						stream.WriteByte(17);
						binaryWriter.Write(instance.lon);
						ProtocolParser.Stack.Push(stream2);
					}

					public static byte[] SerializeToBytes(Location instance)
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							Serialize(memoryStream, instance);
							return memoryStream.ToArray();
						}
					}

					public static void SerializeLengthDelimited(Stream stream, Location instance)
					{
						byte[] array = SerializeToBytes(instance);
						ProtocolParser.WriteUInt32(stream, (uint)array.Length);
						stream.Write(array, 0, array.Length);
					}
				}

				[DataContract]
				public class DeprecatedNetworkInfo
				{
					[DataContract]
					public class CellularNetworkInfo
					{
						[DataMember]
						public string network_type { get; set; }

						[DataMember]
						public uint? country_code { get; set; }

						[DataMember]
						public uint? operator_id { get; set; }

						[DataMember]
						public uint? cell_id { get; set; }

						[DataMember]
						public uint? lac { get; set; }

						[DataMember]
						public int? signal_strength { get; set; }

						[DataMember]
						public string operator_name { get; set; }

						public static CellularNetworkInfo Deserialize(Stream stream)
						{
							CellularNetworkInfo cellularNetworkInfo = new CellularNetworkInfo();
							Deserialize(stream, cellularNetworkInfo);
							return cellularNetworkInfo;
						}

						public static CellularNetworkInfo DeserializeLengthDelimited(Stream stream)
						{
							CellularNetworkInfo cellularNetworkInfo = new CellularNetworkInfo();
							DeserializeLengthDelimited(stream, cellularNetworkInfo);
							return cellularNetworkInfo;
						}

						public static CellularNetworkInfo DeserializeLength(Stream stream, int length)
						{
							CellularNetworkInfo cellularNetworkInfo = new CellularNetworkInfo();
							DeserializeLength(stream, length, cellularNetworkInfo);
							return cellularNetworkInfo;
						}

						public static CellularNetworkInfo Deserialize(byte[] buffer)
						{
							CellularNetworkInfo cellularNetworkInfo = new CellularNetworkInfo();
							using (MemoryStream stream = new MemoryStream(buffer))
							{
								Deserialize(stream, cellularNetworkInfo);
								return cellularNetworkInfo;
							}
						}

						public static CellularNetworkInfo Deserialize(byte[] buffer, CellularNetworkInfo instance)
						{
							using (MemoryStream stream = new MemoryStream(buffer))
							{
								Deserialize(stream, instance);
								return instance;
							}
						}

						public static CellularNetworkInfo Deserialize(Stream stream, CellularNetworkInfo instance)
						{
							while (true)
							{
								int num = stream.ReadByte();
								switch (num)
								{
								case 10:
									instance.network_type = ProtocolParser.ReadString(stream);
									continue;
								case 16:
									instance.country_code = ProtocolParser.ReadUInt32(stream);
									continue;
								case 24:
									instance.operator_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 32:
									instance.cell_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 40:
									instance.lac = ProtocolParser.ReadUInt32(stream);
									continue;
								case 48:
									instance.signal_strength = ProtocolParser.ReadZInt32(stream);
									continue;
								case 58:
									instance.operator_name = ProtocolParser.ReadString(stream);
									continue;
								case -1:
									return instance;
								}
								Key key = ProtocolParser.ReadKey((byte)num, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
						}

						public static CellularNetworkInfo DeserializeLengthDelimited(Stream stream, CellularNetworkInfo instance)
						{
							long num = ProtocolParser.ReadUInt32(stream);
							num += stream.Position;
							while (stream.Position < num)
							{
								int num2 = stream.ReadByte();
								switch (num2)
								{
								case -1:
									throw new EndOfStreamException();
								case 10:
									instance.network_type = ProtocolParser.ReadString(stream);
									continue;
								case 16:
									instance.country_code = ProtocolParser.ReadUInt32(stream);
									continue;
								case 24:
									instance.operator_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 32:
									instance.cell_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 40:
									instance.lac = ProtocolParser.ReadUInt32(stream);
									continue;
								case 48:
									instance.signal_strength = ProtocolParser.ReadZInt32(stream);
									continue;
								case 58:
									instance.operator_name = ProtocolParser.ReadString(stream);
									continue;
								}
								Key key = ProtocolParser.ReadKey((byte)num2, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							if (stream.Position != num)
							{
								throw new ProtocolBufferException("Read past max limit");
							}
							return instance;
						}

						public static CellularNetworkInfo DeserializeLength(Stream stream, int length, CellularNetworkInfo instance)
						{
							long num = stream.Position + length;
							while (stream.Position < num)
							{
								int num2 = stream.ReadByte();
								switch (num2)
								{
								case -1:
									throw new EndOfStreamException();
								case 10:
									instance.network_type = ProtocolParser.ReadString(stream);
									continue;
								case 16:
									instance.country_code = ProtocolParser.ReadUInt32(stream);
									continue;
								case 24:
									instance.operator_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 32:
									instance.cell_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 40:
									instance.lac = ProtocolParser.ReadUInt32(stream);
									continue;
								case 48:
									instance.signal_strength = ProtocolParser.ReadZInt32(stream);
									continue;
								case 58:
									instance.operator_name = ProtocolParser.ReadString(stream);
									continue;
								}
								Key key = ProtocolParser.ReadKey((byte)num2, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							if (stream.Position != num)
							{
								throw new ProtocolBufferException("Read past max limit");
							}
							return instance;
						}

						public static void Serialize(Stream stream, CellularNetworkInfo instance)
						{
							MemoryStream stream2 = ProtocolParser.Stack.Pop();
							if (instance.network_type != null)
							{
								stream.WriteByte(10);
								ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.network_type));
							}
							if (instance.country_code.HasValue)
							{
								stream.WriteByte(16);
								ProtocolParser.WriteUInt32(stream, instance.country_code.Value);
							}
							if (instance.operator_id.HasValue)
							{
								stream.WriteByte(24);
								ProtocolParser.WriteUInt32(stream, instance.operator_id.Value);
							}
							if (instance.cell_id.HasValue)
							{
								stream.WriteByte(32);
								ProtocolParser.WriteUInt32(stream, instance.cell_id.Value);
							}
							if (instance.lac.HasValue)
							{
								stream.WriteByte(40);
								ProtocolParser.WriteUInt32(stream, instance.lac.Value);
							}
							if (instance.signal_strength.HasValue)
							{
								stream.WriteByte(48);
								ProtocolParser.WriteZInt32(stream, instance.signal_strength.Value);
							}
							if (instance.operator_name != null)
							{
								stream.WriteByte(58);
								ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.operator_name));
							}
							ProtocolParser.Stack.Push(stream2);
						}

						public static byte[] SerializeToBytes(CellularNetworkInfo instance)
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								Serialize(memoryStream, instance);
								return memoryStream.ToArray();
							}
						}

						public static void SerializeLengthDelimited(Stream stream, CellularNetworkInfo instance)
						{
							byte[] array = SerializeToBytes(instance);
							ProtocolParser.WriteUInt32(stream, (uint)array.Length);
							stream.Write(array, 0, array.Length);
						}
					}

					[DataMember]
					public ConnectionType connection_type { get; set; }

					[DataMember]
					public CellularNetworkInfo cellular_network_info { get; set; }

					[DataMember]
					public List<WifiNetworkInfo> wifi_networks { get; set; }

					public static DeprecatedNetworkInfo Deserialize(Stream stream)
					{
						DeprecatedNetworkInfo deprecatedNetworkInfo = new DeprecatedNetworkInfo();
						Deserialize(stream, deprecatedNetworkInfo);
						return deprecatedNetworkInfo;
					}

					public static DeprecatedNetworkInfo DeserializeLengthDelimited(Stream stream)
					{
						DeprecatedNetworkInfo deprecatedNetworkInfo = new DeprecatedNetworkInfo();
						DeserializeLengthDelimited(stream, deprecatedNetworkInfo);
						return deprecatedNetworkInfo;
					}

					public static DeprecatedNetworkInfo DeserializeLength(Stream stream, int length)
					{
						DeprecatedNetworkInfo deprecatedNetworkInfo = new DeprecatedNetworkInfo();
						DeserializeLength(stream, length, deprecatedNetworkInfo);
						return deprecatedNetworkInfo;
					}

					public static DeprecatedNetworkInfo Deserialize(byte[] buffer)
					{
						DeprecatedNetworkInfo deprecatedNetworkInfo = new DeprecatedNetworkInfo();
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, deprecatedNetworkInfo);
							return deprecatedNetworkInfo;
						}
					}

					public static DeprecatedNetworkInfo Deserialize(byte[] buffer, DeprecatedNetworkInfo instance)
					{
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, instance);
							return instance;
						}
					}

					public static DeprecatedNetworkInfo Deserialize(Stream stream, DeprecatedNetworkInfo instance)
					{
						if (instance.wifi_networks == null)
						{
							instance.wifi_networks = new List<WifiNetworkInfo>();
						}
						while (true)
						{
							int num = stream.ReadByte();
							switch (num)
							{
							case 8:
								instance.connection_type = (ConnectionType)ProtocolParser.ReadUInt64(stream);
								continue;
							case 18:
								if (instance.cellular_network_info == null)
								{
									instance.cellular_network_info = CellularNetworkInfo.DeserializeLengthDelimited(stream);
								}
								else
								{
									CellularNetworkInfo.DeserializeLengthDelimited(stream, instance.cellular_network_info);
								}
								continue;
							case 26:
								instance.wifi_networks.Add(WifiNetworkInfo.DeserializeLengthDelimited(stream));
								continue;
							case -1:
								return instance;
							}
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
					}

					public static DeprecatedNetworkInfo DeserializeLengthDelimited(Stream stream, DeprecatedNetworkInfo instance)
					{
						if (instance.wifi_networks == null)
						{
							instance.wifi_networks = new List<WifiNetworkInfo>();
						}
						long num = ProtocolParser.ReadUInt32(stream);
						num += stream.Position;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 8:
								instance.connection_type = (ConnectionType)ProtocolParser.ReadUInt64(stream);
								continue;
							case 18:
								if (instance.cellular_network_info == null)
								{
									instance.cellular_network_info = CellularNetworkInfo.DeserializeLengthDelimited(stream);
								}
								else
								{
									CellularNetworkInfo.DeserializeLengthDelimited(stream, instance.cellular_network_info);
								}
								continue;
							case 26:
								instance.wifi_networks.Add(WifiNetworkInfo.DeserializeLengthDelimited(stream));
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static DeprecatedNetworkInfo DeserializeLength(Stream stream, int length, DeprecatedNetworkInfo instance)
					{
						if (instance.wifi_networks == null)
						{
							instance.wifi_networks = new List<WifiNetworkInfo>();
						}
						long num = stream.Position + length;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 8:
								instance.connection_type = (ConnectionType)ProtocolParser.ReadUInt64(stream);
								continue;
							case 18:
								if (instance.cellular_network_info == null)
								{
									instance.cellular_network_info = CellularNetworkInfo.DeserializeLengthDelimited(stream);
								}
								else
								{
									CellularNetworkInfo.DeserializeLengthDelimited(stream, instance.cellular_network_info);
								}
								continue;
							case 26:
								instance.wifi_networks.Add(WifiNetworkInfo.DeserializeLengthDelimited(stream));
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static void Serialize(Stream stream, DeprecatedNetworkInfo instance)
					{
						MemoryStream memoryStream = ProtocolParser.Stack.Pop();
						stream.WriteByte(8);
						ProtocolParser.WriteUInt64(stream, (ulong)instance.connection_type);
						if (instance.cellular_network_info != null)
						{
							stream.WriteByte(18);
							memoryStream.SetLength(0L);
							CellularNetworkInfo.Serialize(memoryStream, instance.cellular_network_info);
							uint val = (uint)memoryStream.Length;
							ProtocolParser.WriteUInt32(stream, val);
							memoryStream.WriteTo(stream);
						}
						if (instance.wifi_networks != null)
						{
							foreach (WifiNetworkInfo wifi_network in instance.wifi_networks)
							{
								stream.WriteByte(26);
								memoryStream.SetLength(0L);
								WifiNetworkInfo.Serialize(memoryStream, wifi_network);
								uint val2 = (uint)memoryStream.Length;
								ProtocolParser.WriteUInt32(stream, val2);
								memoryStream.WriteTo(stream);
							}
						}
						ProtocolParser.Stack.Push(memoryStream);
					}

					public static byte[] SerializeToBytes(DeprecatedNetworkInfo instance)
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							Serialize(memoryStream, instance);
							return memoryStream.ToArray();
						}
					}

					public static void SerializeLengthDelimited(Stream stream, DeprecatedNetworkInfo instance)
					{
						byte[] array = SerializeToBytes(instance);
						ProtocolParser.WriteUInt32(stream, (uint)array.Length);
						stream.Write(array, 0, array.Length);
					}
				}

				[DataMember]
				public Time start_time { get; set; }

				[DataMember]
				public string locale { get; set; }

				[DataMember]
				public Location location { get; set; }

				[DataMember]
				public DeprecatedNetworkInfo network_info { get; set; }

				[DataMember]
				public SessionType? session_type { get; set; }

				public static SessionDesc Deserialize(Stream stream)
				{
					SessionDesc sessionDesc = new SessionDesc();
					Deserialize(stream, sessionDesc);
					return sessionDesc;
				}

				public static SessionDesc DeserializeLengthDelimited(Stream stream)
				{
					SessionDesc sessionDesc = new SessionDesc();
					DeserializeLengthDelimited(stream, sessionDesc);
					return sessionDesc;
				}

				public static SessionDesc DeserializeLength(Stream stream, int length)
				{
					SessionDesc sessionDesc = new SessionDesc();
					DeserializeLength(stream, length, sessionDesc);
					return sessionDesc;
				}

				public static SessionDesc Deserialize(byte[] buffer)
				{
					SessionDesc sessionDesc = new SessionDesc();
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, sessionDesc);
						return sessionDesc;
					}
				}

				public static SessionDesc Deserialize(byte[] buffer, SessionDesc instance)
				{
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, instance);
						return instance;
					}
				}

				public static SessionDesc Deserialize(Stream stream, SessionDesc instance)
				{
					while (true)
					{
						int num = stream.ReadByte();
						switch (num)
						{
						case 10:
							if (instance.start_time == null)
							{
								instance.start_time = Time.DeserializeLengthDelimited(stream);
							}
							else
							{
								Time.DeserializeLengthDelimited(stream, instance.start_time);
							}
							continue;
						case 18:
							instance.locale = ProtocolParser.ReadString(stream);
							continue;
						case 26:
							if (instance.location == null)
							{
								instance.location = Location.DeserializeLengthDelimited(stream);
							}
							else
							{
								Location.DeserializeLengthDelimited(stream, instance.location);
							}
							continue;
						case 34:
							if (instance.network_info == null)
							{
								instance.network_info = DeprecatedNetworkInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
							}
							continue;
						case 40:
							instance.session_type = (SessionType)ProtocolParser.ReadUInt64(stream);
							continue;
						case -1:
							return instance;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
				}

				public static SessionDesc DeserializeLengthDelimited(Stream stream, SessionDesc instance)
				{
					long num = ProtocolParser.ReadUInt32(stream);
					num += stream.Position;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 10:
							if (instance.start_time == null)
							{
								instance.start_time = Time.DeserializeLengthDelimited(stream);
							}
							else
							{
								Time.DeserializeLengthDelimited(stream, instance.start_time);
							}
							continue;
						case 18:
							instance.locale = ProtocolParser.ReadString(stream);
							continue;
						case 26:
							if (instance.location == null)
							{
								instance.location = Location.DeserializeLengthDelimited(stream);
							}
							else
							{
								Location.DeserializeLengthDelimited(stream, instance.location);
							}
							continue;
						case 34:
							if (instance.network_info == null)
							{
								instance.network_info = DeprecatedNetworkInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
							}
							continue;
						case 40:
							instance.session_type = (SessionType)ProtocolParser.ReadUInt64(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static SessionDesc DeserializeLength(Stream stream, int length, SessionDesc instance)
				{
					long num = stream.Position + length;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 10:
							if (instance.start_time == null)
							{
								instance.start_time = Time.DeserializeLengthDelimited(stream);
							}
							else
							{
								Time.DeserializeLengthDelimited(stream, instance.start_time);
							}
							continue;
						case 18:
							instance.locale = ProtocolParser.ReadString(stream);
							continue;
						case 26:
							if (instance.location == null)
							{
								instance.location = Location.DeserializeLengthDelimited(stream);
							}
							else
							{
								Location.DeserializeLengthDelimited(stream, instance.location);
							}
							continue;
						case 34:
							if (instance.network_info == null)
							{
								instance.network_info = DeprecatedNetworkInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								DeprecatedNetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
							}
							continue;
						case 40:
							instance.session_type = (SessionType)ProtocolParser.ReadUInt64(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static void Serialize(Stream stream, SessionDesc instance)
				{
					MemoryStream memoryStream = ProtocolParser.Stack.Pop();
					if (instance.start_time == null)
					{
						throw new ProtocolBufferException("start_time is required by the proto specification.");
					}
					stream.WriteByte(10);
					memoryStream.SetLength(0L);
					Time.Serialize(memoryStream, instance.start_time);
					uint val = (uint)memoryStream.Length;
					ProtocolParser.WriteUInt32(stream, val);
					memoryStream.WriteTo(stream);
					if (instance.locale == null)
					{
						throw new ProtocolBufferException("locale is required by the proto specification.");
					}
					stream.WriteByte(18);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.locale));
					if (instance.location != null)
					{
						stream.WriteByte(26);
						memoryStream.SetLength(0L);
						Location.Serialize(memoryStream, instance.location);
						uint val2 = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val2);
						memoryStream.WriteTo(stream);
					}
					if (instance.network_info != null)
					{
						stream.WriteByte(34);
						memoryStream.SetLength(0L);
						DeprecatedNetworkInfo.Serialize(memoryStream, instance.network_info);
						uint val3 = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val3);
						memoryStream.WriteTo(stream);
					}
					if (instance.session_type.HasValue)
					{
						stream.WriteByte(40);
						ProtocolParser.WriteUInt64(stream, (ulong)instance.session_type.Value);
					}
					ProtocolParser.Stack.Push(memoryStream);
				}

				public static byte[] SerializeToBytes(SessionDesc instance)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						Serialize(memoryStream, instance);
						return memoryStream.ToArray();
					}
				}

				public static void SerializeLengthDelimited(Stream stream, SessionDesc instance)
				{
					byte[] array = SerializeToBytes(instance);
					ProtocolParser.WriteUInt32(stream, (uint)array.Length);
					stream.Write(array, 0, array.Length);
				}
			}

			[DataContract]
			public class Event
			{
				public enum EventType
				{
					EVENT_INIT = 1,
					EVENT_START,
					EVENT_CRASH,
					EVENT_CLIENT,
					EVENT_REFERRER,
					EVENT_ERROR,
					EVENT_ALIVE,
					EVENT_IDENTITY,
					EVENT_AD_CLICK,
					EVENT_AD_INSTALL,
					EVENT_STATBOX,
					EVENT_ACCOUNT,
					EVENT_FIRST,
					EVENT_PUSH_TOKEN,
					EVENT_NOTIFICATION,
					EVENT_OPEN,
					EVENT_UPDATE,
					EVENT_PERMISSIONS,
					EVENT_APP_FEATURES
				}

				public enum EncryptionMode
				{
					NONE,
					RSA_AES_CBC
				}

				[DataContract]
				public class NetworkInfo
				{
					[DataContract]
					public class Cell
					{
						public enum Type
						{
							TYPE_DEFAULT,
							TYPE_GSM,
							TYPE_CDMA,
							TYPE_WCDMA,
							TYPE_LTE
						}

						[DataMember]
						public uint? cell_id { get; set; }

						[DataMember]
						public int? signal_strength { get; set; }

						[DataMember]
						public uint? lac { get; set; }

						[DataMember]
						public uint? country_code { get; set; }

						[DataMember]
						public uint? operator_id { get; set; }

						[DataMember]
						public string operator_name { get; set; }

						[DataMember]
						public bool? is_connected { get; set; }

						[DataMember]
						public Type? type { get; set; }

						[DataMember]
						public uint? pci { get; set; }

						public static Cell Deserialize(Stream stream)
						{
							Cell cell = new Cell();
							Deserialize(stream, cell);
							return cell;
						}

						public static Cell DeserializeLengthDelimited(Stream stream)
						{
							Cell cell = new Cell();
							DeserializeLengthDelimited(stream, cell);
							return cell;
						}

						public static Cell DeserializeLength(Stream stream, int length)
						{
							Cell cell = new Cell();
							DeserializeLength(stream, length, cell);
							return cell;
						}

						public static Cell Deserialize(byte[] buffer)
						{
							Cell cell = new Cell();
							using (MemoryStream stream = new MemoryStream(buffer))
							{
								Deserialize(stream, cell);
								return cell;
							}
						}

						public static Cell Deserialize(byte[] buffer, Cell instance)
						{
							using (MemoryStream stream = new MemoryStream(buffer))
							{
								Deserialize(stream, instance);
								return instance;
							}
						}

						public static Cell Deserialize(Stream stream, Cell instance)
						{
							instance.is_connected = false;
							instance.type = Type.TYPE_DEFAULT;
							while (true)
							{
								int num = stream.ReadByte();
								switch (num)
								{
								case 8:
									instance.cell_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 16:
									instance.signal_strength = ProtocolParser.ReadZInt32(stream);
									continue;
								case 24:
									instance.lac = ProtocolParser.ReadUInt32(stream);
									continue;
								case 32:
									instance.country_code = ProtocolParser.ReadUInt32(stream);
									continue;
								case 40:
									instance.operator_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 50:
									instance.operator_name = ProtocolParser.ReadString(stream);
									continue;
								case 56:
									instance.is_connected = ProtocolParser.ReadBool(stream);
									continue;
								case 64:
									instance.type = (Type)ProtocolParser.ReadUInt64(stream);
									continue;
								case 72:
									instance.pci = ProtocolParser.ReadUInt32(stream);
									continue;
								case -1:
									return instance;
								}
								Key key = ProtocolParser.ReadKey((byte)num, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
						}

						public static Cell DeserializeLengthDelimited(Stream stream, Cell instance)
						{
							instance.is_connected = false;
							instance.type = Type.TYPE_DEFAULT;
							long num = ProtocolParser.ReadUInt32(stream);
							num += stream.Position;
							while (stream.Position < num)
							{
								int num2 = stream.ReadByte();
								switch (num2)
								{
								case -1:
									throw new EndOfStreamException();
								case 8:
									instance.cell_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 16:
									instance.signal_strength = ProtocolParser.ReadZInt32(stream);
									continue;
								case 24:
									instance.lac = ProtocolParser.ReadUInt32(stream);
									continue;
								case 32:
									instance.country_code = ProtocolParser.ReadUInt32(stream);
									continue;
								case 40:
									instance.operator_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 50:
									instance.operator_name = ProtocolParser.ReadString(stream);
									continue;
								case 56:
									instance.is_connected = ProtocolParser.ReadBool(stream);
									continue;
								case 64:
									instance.type = (Type)ProtocolParser.ReadUInt64(stream);
									continue;
								case 72:
									instance.pci = ProtocolParser.ReadUInt32(stream);
									continue;
								}
								Key key = ProtocolParser.ReadKey((byte)num2, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							if (stream.Position != num)
							{
								throw new ProtocolBufferException("Read past max limit");
							}
							return instance;
						}

						public static Cell DeserializeLength(Stream stream, int length, Cell instance)
						{
							instance.is_connected = false;
							instance.type = Type.TYPE_DEFAULT;
							long num = stream.Position + length;
							while (stream.Position < num)
							{
								int num2 = stream.ReadByte();
								switch (num2)
								{
								case -1:
									throw new EndOfStreamException();
								case 8:
									instance.cell_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 16:
									instance.signal_strength = ProtocolParser.ReadZInt32(stream);
									continue;
								case 24:
									instance.lac = ProtocolParser.ReadUInt32(stream);
									continue;
								case 32:
									instance.country_code = ProtocolParser.ReadUInt32(stream);
									continue;
								case 40:
									instance.operator_id = ProtocolParser.ReadUInt32(stream);
									continue;
								case 50:
									instance.operator_name = ProtocolParser.ReadString(stream);
									continue;
								case 56:
									instance.is_connected = ProtocolParser.ReadBool(stream);
									continue;
								case 64:
									instance.type = (Type)ProtocolParser.ReadUInt64(stream);
									continue;
								case 72:
									instance.pci = ProtocolParser.ReadUInt32(stream);
									continue;
								}
								Key key = ProtocolParser.ReadKey((byte)num2, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							if (stream.Position != num)
							{
								throw new ProtocolBufferException("Read past max limit");
							}
							return instance;
						}

						public static void Serialize(Stream stream, Cell instance)
						{
							MemoryStream stream2 = ProtocolParser.Stack.Pop();
							if (instance.cell_id.HasValue)
							{
								stream.WriteByte(8);
								ProtocolParser.WriteUInt32(stream, instance.cell_id.Value);
							}
							if (instance.signal_strength.HasValue)
							{
								stream.WriteByte(16);
								ProtocolParser.WriteZInt32(stream, instance.signal_strength.Value);
							}
							if (instance.lac.HasValue)
							{
								stream.WriteByte(24);
								ProtocolParser.WriteUInt32(stream, instance.lac.Value);
							}
							if (instance.country_code.HasValue)
							{
								stream.WriteByte(32);
								ProtocolParser.WriteUInt32(stream, instance.country_code.Value);
							}
							if (instance.operator_id.HasValue)
							{
								stream.WriteByte(40);
								ProtocolParser.WriteUInt32(stream, instance.operator_id.Value);
							}
							if (instance.operator_name != null)
							{
								stream.WriteByte(50);
								ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.operator_name));
							}
							if (instance.is_connected.HasValue)
							{
								stream.WriteByte(56);
								ProtocolParser.WriteBool(stream, instance.is_connected.Value);
							}
							if (instance.type.HasValue)
							{
								stream.WriteByte(64);
								ProtocolParser.WriteUInt64(stream, (ulong)instance.type.Value);
							}
							if (instance.pci.HasValue)
							{
								stream.WriteByte(72);
								ProtocolParser.WriteUInt32(stream, instance.pci.Value);
							}
							ProtocolParser.Stack.Push(stream2);
						}

						public static byte[] SerializeToBytes(Cell instance)
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								Serialize(memoryStream, instance);
								return memoryStream.ToArray();
							}
						}

						public static void SerializeLengthDelimited(Stream stream, Cell instance)
						{
							byte[] array = SerializeToBytes(instance);
							ProtocolParser.WriteUInt32(stream, (uint)array.Length);
							stream.Write(array, 0, array.Length);
						}
					}

					[DataContract]
					public class WifiAccessPoint
					{
						public enum State
						{
							STATE_UNKNOWN,
							STATE_DISABLED,
							STATE_ENABLED
						}

						[DataMember]
						public string ssid { get; set; }

						[DataMember]
						public State? state { get; set; }

						public static WifiAccessPoint Deserialize(Stream stream)
						{
							WifiAccessPoint wifiAccessPoint = new WifiAccessPoint();
							Deserialize(stream, wifiAccessPoint);
							return wifiAccessPoint;
						}

						public static WifiAccessPoint DeserializeLengthDelimited(Stream stream)
						{
							WifiAccessPoint wifiAccessPoint = new WifiAccessPoint();
							DeserializeLengthDelimited(stream, wifiAccessPoint);
							return wifiAccessPoint;
						}

						public static WifiAccessPoint DeserializeLength(Stream stream, int length)
						{
							WifiAccessPoint wifiAccessPoint = new WifiAccessPoint();
							DeserializeLength(stream, length, wifiAccessPoint);
							return wifiAccessPoint;
						}

						public static WifiAccessPoint Deserialize(byte[] buffer)
						{
							WifiAccessPoint wifiAccessPoint = new WifiAccessPoint();
							using (MemoryStream stream = new MemoryStream(buffer))
							{
								Deserialize(stream, wifiAccessPoint);
								return wifiAccessPoint;
							}
						}

						public static WifiAccessPoint Deserialize(byte[] buffer, WifiAccessPoint instance)
						{
							using (MemoryStream stream = new MemoryStream(buffer))
							{
								Deserialize(stream, instance);
								return instance;
							}
						}

						public static WifiAccessPoint Deserialize(Stream stream, WifiAccessPoint instance)
						{
							instance.state = State.STATE_UNKNOWN;
							while (true)
							{
								int num = stream.ReadByte();
								switch (num)
								{
								case 10:
									instance.ssid = ProtocolParser.ReadString(stream);
									continue;
								case 16:
									instance.state = (State)ProtocolParser.ReadUInt64(stream);
									continue;
								case -1:
									return instance;
								}
								Key key = ProtocolParser.ReadKey((byte)num, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
						}

						public static WifiAccessPoint DeserializeLengthDelimited(Stream stream, WifiAccessPoint instance)
						{
							instance.state = State.STATE_UNKNOWN;
							long num = ProtocolParser.ReadUInt32(stream);
							num += stream.Position;
							while (stream.Position < num)
							{
								int num2 = stream.ReadByte();
								switch (num2)
								{
								case -1:
									throw new EndOfStreamException();
								case 10:
									instance.ssid = ProtocolParser.ReadString(stream);
									continue;
								case 16:
									instance.state = (State)ProtocolParser.ReadUInt64(stream);
									continue;
								}
								Key key = ProtocolParser.ReadKey((byte)num2, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							if (stream.Position != num)
							{
								throw new ProtocolBufferException("Read past max limit");
							}
							return instance;
						}

						public static WifiAccessPoint DeserializeLength(Stream stream, int length, WifiAccessPoint instance)
						{
							instance.state = State.STATE_UNKNOWN;
							long num = stream.Position + length;
							while (stream.Position < num)
							{
								int num2 = stream.ReadByte();
								switch (num2)
								{
								case -1:
									throw new EndOfStreamException();
								case 10:
									instance.ssid = ProtocolParser.ReadString(stream);
									continue;
								case 16:
									instance.state = (State)ProtocolParser.ReadUInt64(stream);
									continue;
								}
								Key key = ProtocolParser.ReadKey((byte)num2, stream);
								if (key.Field == 0)
								{
									throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
								}
								ProtocolParser.SkipKey(stream, key);
							}
							if (stream.Position != num)
							{
								throw new ProtocolBufferException("Read past max limit");
							}
							return instance;
						}

						public static void Serialize(Stream stream, WifiAccessPoint instance)
						{
							MemoryStream stream2 = ProtocolParser.Stack.Pop();
							if (instance.ssid == null)
							{
								throw new ProtocolBufferException("ssid is required by the proto specification.");
							}
							stream.WriteByte(10);
							ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ssid));
							if (instance.state.HasValue)
							{
								stream.WriteByte(16);
								ProtocolParser.WriteUInt64(stream, (ulong)instance.state.Value);
							}
							ProtocolParser.Stack.Push(stream2);
						}

						public static byte[] SerializeToBytes(WifiAccessPoint instance)
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								Serialize(memoryStream, instance);
								return memoryStream.ToArray();
							}
						}

						public static void SerializeLengthDelimited(Stream stream, WifiAccessPoint instance)
						{
							byte[] array = SerializeToBytes(instance);
							ProtocolParser.WriteUInt32(stream, (uint)array.Length);
							stream.Write(array, 0, array.Length);
						}
					}

					[DataMember]
					public List<Cell> cell { get; set; }

					[DataMember]
					public List<WifiNetworkInfo> wifi_networks { get; set; }

					[DataMember]
					public ConnectionType? connection_type { get; set; }

					[DataMember]
					public string cellular_network_type { get; set; }

					[DataMember]
					public WifiAccessPoint wifi_access_point { get; set; }

					public static NetworkInfo Deserialize(Stream stream)
					{
						NetworkInfo networkInfo = new NetworkInfo();
						Deserialize(stream, networkInfo);
						return networkInfo;
					}

					public static NetworkInfo DeserializeLengthDelimited(Stream stream)
					{
						NetworkInfo networkInfo = new NetworkInfo();
						DeserializeLengthDelimited(stream, networkInfo);
						return networkInfo;
					}

					public static NetworkInfo DeserializeLength(Stream stream, int length)
					{
						NetworkInfo networkInfo = new NetworkInfo();
						DeserializeLength(stream, length, networkInfo);
						return networkInfo;
					}

					public static NetworkInfo Deserialize(byte[] buffer)
					{
						NetworkInfo networkInfo = new NetworkInfo();
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, networkInfo);
							return networkInfo;
						}
					}

					public static NetworkInfo Deserialize(byte[] buffer, NetworkInfo instance)
					{
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, instance);
							return instance;
						}
					}

					public static NetworkInfo Deserialize(Stream stream, NetworkInfo instance)
					{
						if (instance.cell == null)
						{
							instance.cell = new List<Cell>();
						}
						if (instance.wifi_networks == null)
						{
							instance.wifi_networks = new List<WifiNetworkInfo>();
						}
						instance.connection_type = ConnectionType.CONNECTION_UNDEFINED;
						while (true)
						{
							int num = stream.ReadByte();
							switch (num)
							{
							case 10:
								instance.cell.Add(Cell.DeserializeLengthDelimited(stream));
								continue;
							case 18:
								instance.wifi_networks.Add(WifiNetworkInfo.DeserializeLengthDelimited(stream));
								continue;
							case 24:
								instance.connection_type = (ConnectionType)ProtocolParser.ReadUInt64(stream);
								continue;
							case 34:
								instance.cellular_network_type = ProtocolParser.ReadString(stream);
								continue;
							case 42:
								if (instance.wifi_access_point == null)
								{
									instance.wifi_access_point = WifiAccessPoint.DeserializeLengthDelimited(stream);
								}
								else
								{
									WifiAccessPoint.DeserializeLengthDelimited(stream, instance.wifi_access_point);
								}
								continue;
							case -1:
								return instance;
							}
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
					}

					public static NetworkInfo DeserializeLengthDelimited(Stream stream, NetworkInfo instance)
					{
						if (instance.cell == null)
						{
							instance.cell = new List<Cell>();
						}
						if (instance.wifi_networks == null)
						{
							instance.wifi_networks = new List<WifiNetworkInfo>();
						}
						instance.connection_type = ConnectionType.CONNECTION_UNDEFINED;
						long num = ProtocolParser.ReadUInt32(stream);
						num += stream.Position;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 10:
								instance.cell.Add(Cell.DeserializeLengthDelimited(stream));
								continue;
							case 18:
								instance.wifi_networks.Add(WifiNetworkInfo.DeserializeLengthDelimited(stream));
								continue;
							case 24:
								instance.connection_type = (ConnectionType)ProtocolParser.ReadUInt64(stream);
								continue;
							case 34:
								instance.cellular_network_type = ProtocolParser.ReadString(stream);
								continue;
							case 42:
								if (instance.wifi_access_point == null)
								{
									instance.wifi_access_point = WifiAccessPoint.DeserializeLengthDelimited(stream);
								}
								else
								{
									WifiAccessPoint.DeserializeLengthDelimited(stream, instance.wifi_access_point);
								}
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static NetworkInfo DeserializeLength(Stream stream, int length, NetworkInfo instance)
					{
						if (instance.cell == null)
						{
							instance.cell = new List<Cell>();
						}
						if (instance.wifi_networks == null)
						{
							instance.wifi_networks = new List<WifiNetworkInfo>();
						}
						instance.connection_type = ConnectionType.CONNECTION_UNDEFINED;
						long num = stream.Position + length;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 10:
								instance.cell.Add(Cell.DeserializeLengthDelimited(stream));
								continue;
							case 18:
								instance.wifi_networks.Add(WifiNetworkInfo.DeserializeLengthDelimited(stream));
								continue;
							case 24:
								instance.connection_type = (ConnectionType)ProtocolParser.ReadUInt64(stream);
								continue;
							case 34:
								instance.cellular_network_type = ProtocolParser.ReadString(stream);
								continue;
							case 42:
								if (instance.wifi_access_point == null)
								{
									instance.wifi_access_point = WifiAccessPoint.DeserializeLengthDelimited(stream);
								}
								else
								{
									WifiAccessPoint.DeserializeLengthDelimited(stream, instance.wifi_access_point);
								}
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static void Serialize(Stream stream, NetworkInfo instance)
					{
						MemoryStream memoryStream = ProtocolParser.Stack.Pop();
						if (instance.cell != null)
						{
							foreach (Cell item in instance.cell)
							{
								stream.WriteByte(10);
								memoryStream.SetLength(0L);
								Cell.Serialize(memoryStream, item);
								uint val = (uint)memoryStream.Length;
								ProtocolParser.WriteUInt32(stream, val);
								memoryStream.WriteTo(stream);
							}
						}
						if (instance.wifi_networks != null)
						{
							foreach (WifiNetworkInfo wifi_network in instance.wifi_networks)
							{
								stream.WriteByte(18);
								memoryStream.SetLength(0L);
								WifiNetworkInfo.Serialize(memoryStream, wifi_network);
								uint val2 = (uint)memoryStream.Length;
								ProtocolParser.WriteUInt32(stream, val2);
								memoryStream.WriteTo(stream);
							}
						}
						if (instance.connection_type.HasValue)
						{
							stream.WriteByte(24);
							ProtocolParser.WriteUInt64(stream, (ulong)instance.connection_type.Value);
						}
						if (instance.cellular_network_type != null)
						{
							stream.WriteByte(34);
							ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.cellular_network_type));
						}
						if (instance.wifi_access_point != null)
						{
							stream.WriteByte(42);
							memoryStream.SetLength(0L);
							WifiAccessPoint.Serialize(memoryStream, instance.wifi_access_point);
							uint val3 = (uint)memoryStream.Length;
							ProtocolParser.WriteUInt32(stream, val3);
							memoryStream.WriteTo(stream);
						}
						ProtocolParser.Stack.Push(memoryStream);
					}

					public static byte[] SerializeToBytes(NetworkInfo instance)
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							Serialize(memoryStream, instance);
							return memoryStream.ToArray();
						}
					}

					public static void SerializeLengthDelimited(Stream stream, NetworkInfo instance)
					{
						byte[] array = SerializeToBytes(instance);
						ProtocolParser.WriteUInt32(stream, (uint)array.Length);
						stream.Write(array, 0, array.Length);
					}
				}

				[DataContract]
				public class Account
				{
					[DataMember]
					public string id { get; set; }

					[DataMember]
					public string type { get; set; }

					[DataMember]
					public string options { get; set; }

					public static Account Deserialize(Stream stream)
					{
						Account account = new Account();
						Deserialize(stream, account);
						return account;
					}

					public static Account DeserializeLengthDelimited(Stream stream)
					{
						Account account = new Account();
						DeserializeLengthDelimited(stream, account);
						return account;
					}

					public static Account DeserializeLength(Stream stream, int length)
					{
						Account account = new Account();
						DeserializeLength(stream, length, account);
						return account;
					}

					public static Account Deserialize(byte[] buffer)
					{
						Account account = new Account();
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, account);
							return account;
						}
					}

					public static Account Deserialize(byte[] buffer, Account instance)
					{
						using (MemoryStream stream = new MemoryStream(buffer))
						{
							Deserialize(stream, instance);
							return instance;
						}
					}

					public static Account Deserialize(Stream stream, Account instance)
					{
						while (true)
						{
							int num = stream.ReadByte();
							switch (num)
							{
							case 10:
								instance.id = ProtocolParser.ReadString(stream);
								continue;
							case 18:
								instance.type = ProtocolParser.ReadString(stream);
								continue;
							case 26:
								instance.options = ProtocolParser.ReadString(stream);
								continue;
							case -1:
								return instance;
							}
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
					}

					public static Account DeserializeLengthDelimited(Stream stream, Account instance)
					{
						long num = ProtocolParser.ReadUInt32(stream);
						num += stream.Position;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 10:
								instance.id = ProtocolParser.ReadString(stream);
								continue;
							case 18:
								instance.type = ProtocolParser.ReadString(stream);
								continue;
							case 26:
								instance.options = ProtocolParser.ReadString(stream);
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static Account DeserializeLength(Stream stream, int length, Account instance)
					{
						long num = stream.Position + length;
						while (stream.Position < num)
						{
							int num2 = stream.ReadByte();
							switch (num2)
							{
							case -1:
								throw new EndOfStreamException();
							case 10:
								instance.id = ProtocolParser.ReadString(stream);
								continue;
							case 18:
								instance.type = ProtocolParser.ReadString(stream);
								continue;
							case 26:
								instance.options = ProtocolParser.ReadString(stream);
								continue;
							}
							Key key = ProtocolParser.ReadKey((byte)num2, stream);
							if (key.Field == 0)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						if (stream.Position != num)
						{
							throw new ProtocolBufferException("Read past max limit");
						}
						return instance;
					}

					public static void Serialize(Stream stream, Account instance)
					{
						MemoryStream stream2 = ProtocolParser.Stack.Pop();
						if (instance.id == null)
						{
							throw new ProtocolBufferException("id is required by the proto specification.");
						}
						stream.WriteByte(10);
						ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.id));
						if (instance.type != null)
						{
							stream.WriteByte(18);
							ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.type));
						}
						if (instance.options != null)
						{
							stream.WriteByte(26);
							ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.options));
						}
						ProtocolParser.Stack.Push(stream2);
					}

					public static byte[] SerializeToBytes(Account instance)
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							Serialize(memoryStream, instance);
							return memoryStream.ToArray();
						}
					}

					public static void SerializeLengthDelimited(Stream stream, Account instance)
					{
						byte[] array = SerializeToBytes(instance);
						ProtocolParser.WriteUInt32(stream, (uint)array.Length);
						stream.Write(array, 0, array.Length);
					}
				}

				[DataMember]
				public ulong number { get; set; }

				[DataMember]
				public ulong time { get; set; }

				[DataMember]
				public uint type { get; set; }

				[DataMember]
				public string name { get; set; }

				[DataMember]
				public byte[] value { get; set; }

				[DataMember]
				public Location location { get; set; }

				[DataMember]
				public NetworkInfo network_info { get; set; }

				[DataMember]
				public string environment { get; set; }

				[DataMember]
				public Account account { get; set; }

				[DataMember]
				public uint? bytes_truncated { get; set; }

				[DataMember]
				public EncryptionMode? encryption_mode { get; set; }

				public static Event Deserialize(Stream stream)
				{
					Event obj = new Event();
					Deserialize(stream, obj);
					return obj;
				}

				public static Event DeserializeLengthDelimited(Stream stream)
				{
					Event obj = new Event();
					DeserializeLengthDelimited(stream, obj);
					return obj;
				}

				public static Event DeserializeLength(Stream stream, int length)
				{
					Event obj = new Event();
					DeserializeLength(stream, length, obj);
					return obj;
				}

				public static Event Deserialize(byte[] buffer)
				{
					Event obj = new Event();
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, obj);
						return obj;
					}
				}

				public static Event Deserialize(byte[] buffer, Event instance)
				{
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, instance);
						return instance;
					}
				}

				public static Event Deserialize(Stream stream, Event instance)
				{
					instance.encryption_mode = EncryptionMode.NONE;
					while (true)
					{
						int num = stream.ReadByte();
						switch (num)
						{
						case 8:
							instance.number = ProtocolParser.ReadUInt64(stream);
							continue;
						case 16:
							instance.time = ProtocolParser.ReadUInt64(stream);
							continue;
						case 24:
							instance.type = ProtocolParser.ReadUInt32(stream);
							continue;
						case 34:
							instance.name = ProtocolParser.ReadString(stream);
							continue;
						case 42:
							instance.value = ProtocolParser.ReadBytes(stream);
							continue;
						case 50:
							if (instance.location == null)
							{
								instance.location = Location.DeserializeLengthDelimited(stream);
							}
							else
							{
								Location.DeserializeLengthDelimited(stream, instance.location);
							}
							continue;
						case 58:
							if (instance.network_info == null)
							{
								instance.network_info = NetworkInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								NetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
							}
							continue;
						case 66:
							instance.environment = ProtocolParser.ReadString(stream);
							continue;
						case 74:
							if (instance.account == null)
							{
								instance.account = Account.DeserializeLengthDelimited(stream);
							}
							else
							{
								Account.DeserializeLengthDelimited(stream, instance.account);
							}
							continue;
						case 80:
							instance.bytes_truncated = ProtocolParser.ReadUInt32(stream);
							continue;
						case 96:
							instance.encryption_mode = (EncryptionMode)ProtocolParser.ReadUInt64(stream);
							continue;
						case -1:
							return instance;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
				}

				public static Event DeserializeLengthDelimited(Stream stream, Event instance)
				{
					instance.encryption_mode = EncryptionMode.NONE;
					long num = ProtocolParser.ReadUInt32(stream);
					num += stream.Position;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 8:
							instance.number = ProtocolParser.ReadUInt64(stream);
							continue;
						case 16:
							instance.time = ProtocolParser.ReadUInt64(stream);
							continue;
						case 24:
							instance.type = ProtocolParser.ReadUInt32(stream);
							continue;
						case 34:
							instance.name = ProtocolParser.ReadString(stream);
							continue;
						case 42:
							instance.value = ProtocolParser.ReadBytes(stream);
							continue;
						case 50:
							if (instance.location == null)
							{
								instance.location = Location.DeserializeLengthDelimited(stream);
							}
							else
							{
								Location.DeserializeLengthDelimited(stream, instance.location);
							}
							continue;
						case 58:
							if (instance.network_info == null)
							{
								instance.network_info = NetworkInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								NetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
							}
							continue;
						case 66:
							instance.environment = ProtocolParser.ReadString(stream);
							continue;
						case 74:
							if (instance.account == null)
							{
								instance.account = Account.DeserializeLengthDelimited(stream);
							}
							else
							{
								Account.DeserializeLengthDelimited(stream, instance.account);
							}
							continue;
						case 80:
							instance.bytes_truncated = ProtocolParser.ReadUInt32(stream);
							continue;
						case 96:
							instance.encryption_mode = (EncryptionMode)ProtocolParser.ReadUInt64(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static Event DeserializeLength(Stream stream, int length, Event instance)
				{
					instance.encryption_mode = EncryptionMode.NONE;
					long num = stream.Position + length;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 8:
							instance.number = ProtocolParser.ReadUInt64(stream);
							continue;
						case 16:
							instance.time = ProtocolParser.ReadUInt64(stream);
							continue;
						case 24:
							instance.type = ProtocolParser.ReadUInt32(stream);
							continue;
						case 34:
							instance.name = ProtocolParser.ReadString(stream);
							continue;
						case 42:
							instance.value = ProtocolParser.ReadBytes(stream);
							continue;
						case 50:
							if (instance.location == null)
							{
								instance.location = Location.DeserializeLengthDelimited(stream);
							}
							else
							{
								Location.DeserializeLengthDelimited(stream, instance.location);
							}
							continue;
						case 58:
							if (instance.network_info == null)
							{
								instance.network_info = NetworkInfo.DeserializeLengthDelimited(stream);
							}
							else
							{
								NetworkInfo.DeserializeLengthDelimited(stream, instance.network_info);
							}
							continue;
						case 66:
							instance.environment = ProtocolParser.ReadString(stream);
							continue;
						case 74:
							if (instance.account == null)
							{
								instance.account = Account.DeserializeLengthDelimited(stream);
							}
							else
							{
								Account.DeserializeLengthDelimited(stream, instance.account);
							}
							continue;
						case 80:
							instance.bytes_truncated = ProtocolParser.ReadUInt32(stream);
							continue;
						case 96:
							instance.encryption_mode = (EncryptionMode)ProtocolParser.ReadUInt64(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static void Serialize(Stream stream, Event instance)
				{
					MemoryStream memoryStream = ProtocolParser.Stack.Pop();
					stream.WriteByte(8);
					ProtocolParser.WriteUInt64(stream, instance.number);
					stream.WriteByte(16);
					ProtocolParser.WriteUInt64(stream, instance.time);
					stream.WriteByte(24);
					ProtocolParser.WriteUInt32(stream, instance.type);
					if (instance.name != null)
					{
						stream.WriteByte(34);
						ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
					}
					if (instance.value != null)
					{
						stream.WriteByte(42);
						ProtocolParser.WriteBytes(stream, instance.value);
					}
					if (instance.location != null)
					{
						stream.WriteByte(50);
						memoryStream.SetLength(0L);
						Location.Serialize(memoryStream, instance.location);
						uint val = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val);
						memoryStream.WriteTo(stream);
					}
					if (instance.network_info != null)
					{
						stream.WriteByte(58);
						memoryStream.SetLength(0L);
						NetworkInfo.Serialize(memoryStream, instance.network_info);
						uint val2 = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val2);
						memoryStream.WriteTo(stream);
					}
					if (instance.environment != null)
					{
						stream.WriteByte(66);
						ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.environment));
					}
					if (instance.account != null)
					{
						stream.WriteByte(74);
						memoryStream.SetLength(0L);
						Account.Serialize(memoryStream, instance.account);
						uint val3 = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val3);
						memoryStream.WriteTo(stream);
					}
					if (instance.bytes_truncated.HasValue)
					{
						stream.WriteByte(80);
						ProtocolParser.WriteUInt32(stream, instance.bytes_truncated.Value);
					}
					if (instance.encryption_mode.HasValue)
					{
						stream.WriteByte(96);
						ProtocolParser.WriteUInt64(stream, (ulong)instance.encryption_mode.Value);
					}
					ProtocolParser.Stack.Push(memoryStream);
				}

				public static byte[] SerializeToBytes(Event instance)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						Serialize(memoryStream, instance);
						return memoryStream.ToArray();
					}
				}

				public static void SerializeLengthDelimited(Stream stream, Event instance)
				{
					byte[] array = SerializeToBytes(instance);
					ProtocolParser.WriteUInt32(stream, (uint)array.Length);
					stream.Write(array, 0, array.Length);
				}
			}

			[DataMember]
			public ulong id { get; set; }

			[DataMember]
			public SessionDesc session_desc { get; set; }

			[DataMember]
			public List<Event> events { get; set; }

			public static Session Deserialize(Stream stream)
			{
				Session session = new Session();
				Deserialize(stream, session);
				return session;
			}

			public static Session DeserializeLengthDelimited(Stream stream)
			{
				Session session = new Session();
				DeserializeLengthDelimited(stream, session);
				return session;
			}

			public static Session DeserializeLength(Stream stream, int length)
			{
				Session session = new Session();
				DeserializeLength(stream, length, session);
				return session;
			}

			public static Session Deserialize(byte[] buffer)
			{
				Session session = new Session();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, session);
					return session;
				}
			}

			public static Session Deserialize(byte[] buffer, Session instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static Session Deserialize(Stream stream, Session instance)
			{
				if (instance.events == null)
				{
					instance.events = new List<Event>();
				}
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 8:
						instance.id = ProtocolParser.ReadUInt64(stream);
						continue;
					case 18:
						if (instance.session_desc == null)
						{
							instance.session_desc = SessionDesc.DeserializeLengthDelimited(stream);
						}
						else
						{
							SessionDesc.DeserializeLengthDelimited(stream, instance.session_desc);
						}
						continue;
					case 26:
						instance.events.Add(Event.DeserializeLengthDelimited(stream));
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
			}

			public static Session DeserializeLengthDelimited(Stream stream, Session instance)
			{
				if (instance.events == null)
				{
					instance.events = new List<Event>();
				}
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 8:
						instance.id = ProtocolParser.ReadUInt64(stream);
						continue;
					case 18:
						if (instance.session_desc == null)
						{
							instance.session_desc = SessionDesc.DeserializeLengthDelimited(stream);
						}
						else
						{
							SessionDesc.DeserializeLengthDelimited(stream, instance.session_desc);
						}
						continue;
					case 26:
						instance.events.Add(Event.DeserializeLengthDelimited(stream));
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static Session DeserializeLength(Stream stream, int length, Session instance)
			{
				if (instance.events == null)
				{
					instance.events = new List<Event>();
				}
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 8:
						instance.id = ProtocolParser.ReadUInt64(stream);
						continue;
					case 18:
						if (instance.session_desc == null)
						{
							instance.session_desc = SessionDesc.DeserializeLengthDelimited(stream);
						}
						else
						{
							SessionDesc.DeserializeLengthDelimited(stream, instance.session_desc);
						}
						continue;
					case 26:
						instance.events.Add(Event.DeserializeLengthDelimited(stream));
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, Session instance)
			{
				MemoryStream memoryStream = ProtocolParser.Stack.Pop();
				stream.WriteByte(8);
				ProtocolParser.WriteUInt64(stream, instance.id);
				if (instance.session_desc == null)
				{
					throw new ProtocolBufferException("session_desc is required by the proto specification.");
				}
				stream.WriteByte(18);
				memoryStream.SetLength(0L);
				SessionDesc.Serialize(memoryStream, instance.session_desc);
				uint val = (uint)memoryStream.Length;
				ProtocolParser.WriteUInt32(stream, val);
				memoryStream.WriteTo(stream);
				if (instance.events != null)
				{
					foreach (Event @event in instance.events)
					{
						stream.WriteByte(26);
						memoryStream.SetLength(0L);
						Event.Serialize(memoryStream, @event);
						uint val2 = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val2);
						memoryStream.WriteTo(stream);
					}
				}
				ProtocolParser.Stack.Push(memoryStream);
			}

			public static byte[] SerializeToBytes(Session instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, Session instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataContract]
		public class RequestParameters
		{
			public enum DeviceType
			{
				PHONE = 1,
				TABLET,
				PHABLET,
				TV,
				DESKTOP
			}

			public enum AppFramework
			{
				NATIVE,
				UNITY,
				XAMARIN,
				REACT,
				CORDOVA
			}

			public enum KitBuildType
			{
				UNDEFINED = 0,
				NET_DESKTOP = 1,
				WINRT8 = 2,
				WINRT10 = 3,
				WP8S = 4,
				WP81 = 5,
				UWP10 = 6,
				WP7S = 7,
				SOURCE = 100,
				STATIC = 101,
				DYNAMIC = 102,
				PUBLIC = 200,
				PUBLIC_SNAPSHOT = 201,
				INTERNAL = 202,
				INTERNAL_SNAPSHOT = 203,
				LIMITED = 204,
				LIMITED_SNAPSHOT = 205
			}

			[DataContract]
			public class Clid
			{
				[DataMember]
				public string name { get; set; }

				[DataMember]
				public ulong value { get; set; }

				public static Clid Deserialize(Stream stream)
				{
					Clid clid = new Clid();
					Deserialize(stream, clid);
					return clid;
				}

				public static Clid DeserializeLengthDelimited(Stream stream)
				{
					Clid clid = new Clid();
					DeserializeLengthDelimited(stream, clid);
					return clid;
				}

				public static Clid DeserializeLength(Stream stream, int length)
				{
					Clid clid = new Clid();
					DeserializeLength(stream, length, clid);
					return clid;
				}

				public static Clid Deserialize(byte[] buffer)
				{
					Clid clid = new Clid();
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, clid);
						return clid;
					}
				}

				public static Clid Deserialize(byte[] buffer, Clid instance)
				{
					using (MemoryStream stream = new MemoryStream(buffer))
					{
						Deserialize(stream, instance);
						return instance;
					}
				}

				public static Clid Deserialize(Stream stream, Clid instance)
				{
					while (true)
					{
						int num = stream.ReadByte();
						switch (num)
						{
						case 10:
							instance.name = ProtocolParser.ReadString(stream);
							continue;
						case 16:
							instance.value = ProtocolParser.ReadUInt64(stream);
							continue;
						case -1:
							return instance;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
				}

				public static Clid DeserializeLengthDelimited(Stream stream, Clid instance)
				{
					long num = ProtocolParser.ReadUInt32(stream);
					num += stream.Position;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 10:
							instance.name = ProtocolParser.ReadString(stream);
							continue;
						case 16:
							instance.value = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static Clid DeserializeLength(Stream stream, int length, Clid instance)
				{
					long num = stream.Position + length;
					while (stream.Position < num)
					{
						int num2 = stream.ReadByte();
						switch (num2)
						{
						case -1:
							throw new EndOfStreamException();
						case 10:
							instance.name = ProtocolParser.ReadString(stream);
							continue;
						case 16:
							instance.value = ProtocolParser.ReadUInt64(stream);
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num2, stream);
						if (key.Field == 0)
						{
							throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
						}
						ProtocolParser.SkipKey(stream, key);
					}
					if (stream.Position != num)
					{
						throw new ProtocolBufferException("Read past max limit");
					}
					return instance;
				}

				public static void Serialize(Stream stream, Clid instance)
				{
					MemoryStream stream2 = ProtocolParser.Stack.Pop();
					if (instance.name == null)
					{
						throw new ProtocolBufferException("name is required by the proto specification.");
					}
					stream.WriteByte(10);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
					stream.WriteByte(16);
					ProtocolParser.WriteUInt64(stream, instance.value);
					ProtocolParser.Stack.Push(stream2);
				}

				public static byte[] SerializeToBytes(Clid instance)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						Serialize(memoryStream, instance);
						return memoryStream.ToArray();
					}
				}

				public static void SerializeLengthDelimited(Stream stream, Clid instance)
				{
					byte[] array = SerializeToBytes(instance);
					ProtocolParser.WriteUInt32(stream, (uint)array.Length);
					stream.Write(array, 0, array.Length);
				}
			}

			[DataMember]
			public string uuid { get; set; }

			[DataMember]
			public string device_id { get; set; }

			[DataMember]
			public string app_platform { get; set; }

			[DataMember]
			public string app_version_name { get; set; }

			[DataMember]
			public uint? kit_version { get; set; }

			[DataMember]
			public uint? api_key { get; set; }

			[DataMember]
			public string app_id { get; set; }

			[DataMember]
			public string manufacturer { get; set; }

			[DataMember]
			public string model { get; set; }

			[DataMember]
			public string os_version { get; set; }

			[DataMember]
			public uint? screen_width { get; set; }

			[DataMember]
			public uint? screen_height { get; set; }

			[DataMember]
			public uint? screen_dpi { get; set; }

			[DataMember]
			public double? scale_factor { get; set; }

			[DataMember]
			public string locale { get; set; }

			[DataMember]
			public DeviceType? device_type { get; set; }

			[DataMember]
			public bool? is_rooted { get; set; }

			[DataMember]
			public uint? app_build_number { get; set; }

			[DataMember]
			public string ifv { get; set; }

			[DataMember]
			public string android_id { get; set; }

			[DataMember]
			public string adv_id { get; set; }

			[DataMember]
			public uint? client_kit_version { get; set; }

			[DataMember]
			public List<Clid> clids { get; set; }

			[DataMember]
			public string api_key_128 { get; set; }

			[DataMember]
			public string ifa { get; set; }

			[DataMember]
			public AppFramework? app_framework { get; set; }

			[DataMember]
			public string windows_aid { get; set; }

			[DataMember]
			public string storage_type { get; set; }

			[DataMember]
			public uint? os_api_level { get; set; }

			[DataMember]
			public KitBuildType? kit_build_type { get; set; }

			[DataMember]
			public uint? kit_build_number { get; set; }

			[DataMember]
			public bool? app_debuggable { get; set; }

			public static RequestParameters Deserialize(Stream stream)
			{
				RequestParameters requestParameters = new RequestParameters();
				Deserialize(stream, requestParameters);
				return requestParameters;
			}

			public static RequestParameters DeserializeLengthDelimited(Stream stream)
			{
				RequestParameters requestParameters = new RequestParameters();
				DeserializeLengthDelimited(stream, requestParameters);
				return requestParameters;
			}

			public static RequestParameters DeserializeLength(Stream stream, int length)
			{
				RequestParameters requestParameters = new RequestParameters();
				DeserializeLength(stream, length, requestParameters);
				return requestParameters;
			}

			public static RequestParameters Deserialize(byte[] buffer)
			{
				RequestParameters requestParameters = new RequestParameters();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, requestParameters);
					return requestParameters;
				}
			}

			public static RequestParameters Deserialize(byte[] buffer, RequestParameters instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static RequestParameters Deserialize(Stream stream, RequestParameters instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				if (instance.clids == null)
				{
					instance.clids = new List<Clid>();
				}
				instance.app_framework = AppFramework.NATIVE;
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 10:
						instance.uuid = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.device_id = ProtocolParser.ReadString(stream);
						continue;
					case 26:
						instance.app_platform = ProtocolParser.ReadString(stream);
						continue;
					case 34:
						instance.app_version_name = ProtocolParser.ReadString(stream);
						continue;
					case 40:
						instance.kit_version = ProtocolParser.ReadUInt32(stream);
						continue;
					case 48:
						instance.api_key = ProtocolParser.ReadUInt32(stream);
						continue;
					case 58:
						instance.app_id = ProtocolParser.ReadString(stream);
						continue;
					case 66:
						instance.manufacturer = ProtocolParser.ReadString(stream);
						continue;
					case 74:
						instance.model = ProtocolParser.ReadString(stream);
						continue;
					case 82:
						instance.os_version = ProtocolParser.ReadString(stream);
						continue;
					case 88:
						instance.screen_width = ProtocolParser.ReadUInt32(stream);
						continue;
					case 96:
						instance.screen_height = ProtocolParser.ReadUInt32(stream);
						continue;
					case 104:
						instance.screen_dpi = ProtocolParser.ReadUInt32(stream);
						continue;
					case 113:
						instance.scale_factor = binaryReader.ReadDouble();
						continue;
					case 122:
						instance.locale = ProtocolParser.ReadString(stream);
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					switch (key.Field)
					{
					case 0u:
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					case 16u:
						if (key.WireType == Wire.Varint)
						{
							instance.device_type = (DeviceType)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 17u:
						if (key.WireType == Wire.Varint)
						{
							instance.is_rooted = ProtocolParser.ReadBool(stream);
						}
						break;
					case 18u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_build_number = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 19u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.ifv = ProtocolParser.ReadString(stream);
						}
						break;
					case 20u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.android_id = ProtocolParser.ReadString(stream);
						}
						break;
					case 21u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.adv_id = ProtocolParser.ReadString(stream);
						}
						break;
					case 22u:
						if (key.WireType == Wire.Varint)
						{
							instance.client_kit_version = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 23u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.clids.Add(Clid.DeserializeLengthDelimited(stream));
						}
						break;
					case 24u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.api_key_128 = ProtocolParser.ReadString(stream);
						}
						break;
					case 25u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.ifa = ProtocolParser.ReadString(stream);
						}
						break;
					case 26u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_framework = (AppFramework)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 27u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.windows_aid = ProtocolParser.ReadString(stream);
						}
						break;
					case 28u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.storage_type = ProtocolParser.ReadString(stream);
						}
						break;
					case 29u:
						if (key.WireType == Wire.Varint)
						{
							instance.os_api_level = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 30u:
						if (key.WireType == Wire.Varint)
						{
							instance.kit_build_type = (KitBuildType)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 31u:
						if (key.WireType == Wire.Varint)
						{
							instance.kit_build_number = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 32u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_debuggable = ProtocolParser.ReadBool(stream);
						}
						break;
					default:
						ProtocolParser.SkipKey(stream, key);
						break;
					}
				}
			}

			public static RequestParameters DeserializeLengthDelimited(Stream stream, RequestParameters instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				if (instance.clids == null)
				{
					instance.clids = new List<Clid>();
				}
				instance.app_framework = AppFramework.NATIVE;
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 10:
						instance.uuid = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.device_id = ProtocolParser.ReadString(stream);
						continue;
					case 26:
						instance.app_platform = ProtocolParser.ReadString(stream);
						continue;
					case 34:
						instance.app_version_name = ProtocolParser.ReadString(stream);
						continue;
					case 40:
						instance.kit_version = ProtocolParser.ReadUInt32(stream);
						continue;
					case 48:
						instance.api_key = ProtocolParser.ReadUInt32(stream);
						continue;
					case 58:
						instance.app_id = ProtocolParser.ReadString(stream);
						continue;
					case 66:
						instance.manufacturer = ProtocolParser.ReadString(stream);
						continue;
					case 74:
						instance.model = ProtocolParser.ReadString(stream);
						continue;
					case 82:
						instance.os_version = ProtocolParser.ReadString(stream);
						continue;
					case 88:
						instance.screen_width = ProtocolParser.ReadUInt32(stream);
						continue;
					case 96:
						instance.screen_height = ProtocolParser.ReadUInt32(stream);
						continue;
					case 104:
						instance.screen_dpi = ProtocolParser.ReadUInt32(stream);
						continue;
					case 113:
						instance.scale_factor = binaryReader.ReadDouble();
						continue;
					case 122:
						instance.locale = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					switch (key.Field)
					{
					case 0u:
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					case 16u:
						if (key.WireType == Wire.Varint)
						{
							instance.device_type = (DeviceType)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 17u:
						if (key.WireType == Wire.Varint)
						{
							instance.is_rooted = ProtocolParser.ReadBool(stream);
						}
						break;
					case 18u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_build_number = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 19u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.ifv = ProtocolParser.ReadString(stream);
						}
						break;
					case 20u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.android_id = ProtocolParser.ReadString(stream);
						}
						break;
					case 21u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.adv_id = ProtocolParser.ReadString(stream);
						}
						break;
					case 22u:
						if (key.WireType == Wire.Varint)
						{
							instance.client_kit_version = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 23u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.clids.Add(Clid.DeserializeLengthDelimited(stream));
						}
						break;
					case 24u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.api_key_128 = ProtocolParser.ReadString(stream);
						}
						break;
					case 25u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.ifa = ProtocolParser.ReadString(stream);
						}
						break;
					case 26u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_framework = (AppFramework)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 27u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.windows_aid = ProtocolParser.ReadString(stream);
						}
						break;
					case 28u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.storage_type = ProtocolParser.ReadString(stream);
						}
						break;
					case 29u:
						if (key.WireType == Wire.Varint)
						{
							instance.os_api_level = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 30u:
						if (key.WireType == Wire.Varint)
						{
							instance.kit_build_type = (KitBuildType)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 31u:
						if (key.WireType == Wire.Varint)
						{
							instance.kit_build_number = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 32u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_debuggable = ProtocolParser.ReadBool(stream);
						}
						break;
					default:
						ProtocolParser.SkipKey(stream, key);
						break;
					}
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static RequestParameters DeserializeLength(Stream stream, int length, RequestParameters instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				if (instance.clids == null)
				{
					instance.clids = new List<Clid>();
				}
				instance.app_framework = AppFramework.NATIVE;
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 10:
						instance.uuid = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.device_id = ProtocolParser.ReadString(stream);
						continue;
					case 26:
						instance.app_platform = ProtocolParser.ReadString(stream);
						continue;
					case 34:
						instance.app_version_name = ProtocolParser.ReadString(stream);
						continue;
					case 40:
						instance.kit_version = ProtocolParser.ReadUInt32(stream);
						continue;
					case 48:
						instance.api_key = ProtocolParser.ReadUInt32(stream);
						continue;
					case 58:
						instance.app_id = ProtocolParser.ReadString(stream);
						continue;
					case 66:
						instance.manufacturer = ProtocolParser.ReadString(stream);
						continue;
					case 74:
						instance.model = ProtocolParser.ReadString(stream);
						continue;
					case 82:
						instance.os_version = ProtocolParser.ReadString(stream);
						continue;
					case 88:
						instance.screen_width = ProtocolParser.ReadUInt32(stream);
						continue;
					case 96:
						instance.screen_height = ProtocolParser.ReadUInt32(stream);
						continue;
					case 104:
						instance.screen_dpi = ProtocolParser.ReadUInt32(stream);
						continue;
					case 113:
						instance.scale_factor = binaryReader.ReadDouble();
						continue;
					case 122:
						instance.locale = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					switch (key.Field)
					{
					case 0u:
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					case 16u:
						if (key.WireType == Wire.Varint)
						{
							instance.device_type = (DeviceType)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 17u:
						if (key.WireType == Wire.Varint)
						{
							instance.is_rooted = ProtocolParser.ReadBool(stream);
						}
						break;
					case 18u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_build_number = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 19u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.ifv = ProtocolParser.ReadString(stream);
						}
						break;
					case 20u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.android_id = ProtocolParser.ReadString(stream);
						}
						break;
					case 21u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.adv_id = ProtocolParser.ReadString(stream);
						}
						break;
					case 22u:
						if (key.WireType == Wire.Varint)
						{
							instance.client_kit_version = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 23u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.clids.Add(Clid.DeserializeLengthDelimited(stream));
						}
						break;
					case 24u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.api_key_128 = ProtocolParser.ReadString(stream);
						}
						break;
					case 25u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.ifa = ProtocolParser.ReadString(stream);
						}
						break;
					case 26u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_framework = (AppFramework)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 27u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.windows_aid = ProtocolParser.ReadString(stream);
						}
						break;
					case 28u:
						if (key.WireType == Wire.LengthDelimited)
						{
							instance.storage_type = ProtocolParser.ReadString(stream);
						}
						break;
					case 29u:
						if (key.WireType == Wire.Varint)
						{
							instance.os_api_level = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 30u:
						if (key.WireType == Wire.Varint)
						{
							instance.kit_build_type = (KitBuildType)ProtocolParser.ReadUInt64(stream);
						}
						break;
					case 31u:
						if (key.WireType == Wire.Varint)
						{
							instance.kit_build_number = ProtocolParser.ReadUInt32(stream);
						}
						break;
					case 32u:
						if (key.WireType == Wire.Varint)
						{
							instance.app_debuggable = ProtocolParser.ReadBool(stream);
						}
						break;
					default:
						ProtocolParser.SkipKey(stream, key);
						break;
					}
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, RequestParameters instance)
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				MemoryStream memoryStream = ProtocolParser.Stack.Pop();
				if (instance.uuid != null)
				{
					stream.WriteByte(10);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.uuid));
				}
				if (instance.device_id != null)
				{
					stream.WriteByte(18);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.device_id));
				}
				if (instance.app_platform != null)
				{
					stream.WriteByte(26);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.app_platform));
				}
				if (instance.app_version_name != null)
				{
					stream.WriteByte(34);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.app_version_name));
				}
				if (instance.kit_version.HasValue)
				{
					stream.WriteByte(40);
					ProtocolParser.WriteUInt32(stream, instance.kit_version.Value);
				}
				if (instance.api_key.HasValue)
				{
					stream.WriteByte(48);
					ProtocolParser.WriteUInt32(stream, instance.api_key.Value);
				}
				if (instance.app_id != null)
				{
					stream.WriteByte(58);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.app_id));
				}
				if (instance.manufacturer != null)
				{
					stream.WriteByte(66);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.manufacturer));
				}
				if (instance.model != null)
				{
					stream.WriteByte(74);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.model));
				}
				if (instance.os_version != null)
				{
					stream.WriteByte(82);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.os_version));
				}
				if (instance.screen_width.HasValue)
				{
					stream.WriteByte(88);
					ProtocolParser.WriteUInt32(stream, instance.screen_width.Value);
				}
				if (instance.screen_height.HasValue)
				{
					stream.WriteByte(96);
					ProtocolParser.WriteUInt32(stream, instance.screen_height.Value);
				}
				if (instance.screen_dpi.HasValue)
				{
					stream.WriteByte(104);
					ProtocolParser.WriteUInt32(stream, instance.screen_dpi.Value);
				}
				if (instance.scale_factor.HasValue)
				{
					stream.WriteByte(113);
					binaryWriter.Write(instance.scale_factor.Value);
				}
				if (instance.locale != null)
				{
					stream.WriteByte(122);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.locale));
				}
				if (instance.device_type.HasValue)
				{
					stream.WriteByte(128);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.device_type.Value);
				}
				if (instance.is_rooted.HasValue)
				{
					stream.WriteByte(136);
					stream.WriteByte(1);
					ProtocolParser.WriteBool(stream, instance.is_rooted.Value);
				}
				if (instance.app_build_number.HasValue)
				{
					stream.WriteByte(144);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt32(stream, instance.app_build_number.Value);
				}
				if (instance.ifv != null)
				{
					stream.WriteByte(154);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ifv));
				}
				if (instance.android_id != null)
				{
					stream.WriteByte(162);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.android_id));
				}
				if (instance.adv_id != null)
				{
					stream.WriteByte(170);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.adv_id));
				}
				if (instance.client_kit_version.HasValue)
				{
					stream.WriteByte(176);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt32(stream, instance.client_kit_version.Value);
				}
				if (instance.clids != null)
				{
					foreach (Clid clid in instance.clids)
					{
						stream.WriteByte(186);
						stream.WriteByte(1);
						memoryStream.SetLength(0L);
						Clid.Serialize(memoryStream, clid);
						uint val = (uint)memoryStream.Length;
						ProtocolParser.WriteUInt32(stream, val);
						memoryStream.WriteTo(stream);
					}
				}
				if (instance.api_key_128 != null)
				{
					stream.WriteByte(194);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.api_key_128));
				}
				if (instance.ifa != null)
				{
					stream.WriteByte(202);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ifa));
				}
				if (instance.app_framework.HasValue)
				{
					stream.WriteByte(208);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.app_framework.Value);
				}
				if (instance.windows_aid != null)
				{
					stream.WriteByte(218);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.windows_aid));
				}
				if (instance.storage_type != null)
				{
					stream.WriteByte(226);
					stream.WriteByte(1);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.storage_type));
				}
				if (instance.os_api_level.HasValue)
				{
					stream.WriteByte(232);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt32(stream, instance.os_api_level.Value);
				}
				if (instance.kit_build_type.HasValue)
				{
					stream.WriteByte(240);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt64(stream, (ulong)instance.kit_build_type.Value);
				}
				if (instance.kit_build_number.HasValue)
				{
					stream.WriteByte(248);
					stream.WriteByte(1);
					ProtocolParser.WriteUInt32(stream, instance.kit_build_number.Value);
				}
				if (instance.app_debuggable.HasValue)
				{
					stream.WriteByte(128);
					stream.WriteByte(2);
					ProtocolParser.WriteBool(stream, instance.app_debuggable.Value);
				}
				ProtocolParser.Stack.Push(memoryStream);
			}

			public static byte[] SerializeToBytes(RequestParameters instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, RequestParameters instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataContract]
		public class EnvironmentVariable
		{
			[DataMember]
			public string name { get; set; }

			[DataMember]
			public string value { get; set; }

			public static EnvironmentVariable Deserialize(Stream stream)
			{
				EnvironmentVariable environmentVariable = new EnvironmentVariable();
				Deserialize(stream, environmentVariable);
				return environmentVariable;
			}

			public static EnvironmentVariable DeserializeLengthDelimited(Stream stream)
			{
				EnvironmentVariable environmentVariable = new EnvironmentVariable();
				DeserializeLengthDelimited(stream, environmentVariable);
				return environmentVariable;
			}

			public static EnvironmentVariable DeserializeLength(Stream stream, int length)
			{
				EnvironmentVariable environmentVariable = new EnvironmentVariable();
				DeserializeLength(stream, length, environmentVariable);
				return environmentVariable;
			}

			public static EnvironmentVariable Deserialize(byte[] buffer)
			{
				EnvironmentVariable environmentVariable = new EnvironmentVariable();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, environmentVariable);
					return environmentVariable;
				}
			}

			public static EnvironmentVariable Deserialize(byte[] buffer, EnvironmentVariable instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static EnvironmentVariable Deserialize(Stream stream, EnvironmentVariable instance)
			{
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 10:
						instance.name = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.value = ProtocolParser.ReadString(stream);
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
			}

			public static EnvironmentVariable DeserializeLengthDelimited(Stream stream, EnvironmentVariable instance)
			{
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 10:
						instance.name = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.value = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static EnvironmentVariable DeserializeLength(Stream stream, int length, EnvironmentVariable instance)
			{
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 10:
						instance.name = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.value = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, EnvironmentVariable instance)
			{
				MemoryStream stream2 = ProtocolParser.Stack.Pop();
				if (instance.name == null)
				{
					throw new ProtocolBufferException("name is required by the proto specification.");
				}
				stream.WriteByte(10);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
				if (instance.value == null)
				{
					throw new ProtocolBufferException("value is required by the proto specification.");
				}
				stream.WriteByte(18);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.value));
				ProtocolParser.Stack.Push(stream2);
			}

			public static byte[] SerializeToBytes(EnvironmentVariable instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, EnvironmentVariable instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataContract]
		public class NetworkInterface
		{
			[DataMember]
			public string name { get; set; }

			[DataMember]
			public string mac { get; set; }

			public static NetworkInterface Deserialize(Stream stream)
			{
				NetworkInterface networkInterface = new NetworkInterface();
				Deserialize(stream, networkInterface);
				return networkInterface;
			}

			public static NetworkInterface DeserializeLengthDelimited(Stream stream)
			{
				NetworkInterface networkInterface = new NetworkInterface();
				DeserializeLengthDelimited(stream, networkInterface);
				return networkInterface;
			}

			public static NetworkInterface DeserializeLength(Stream stream, int length)
			{
				NetworkInterface networkInterface = new NetworkInterface();
				DeserializeLength(stream, length, networkInterface);
				return networkInterface;
			}

			public static NetworkInterface Deserialize(byte[] buffer)
			{
				NetworkInterface networkInterface = new NetworkInterface();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, networkInterface);
					return networkInterface;
				}
			}

			public static NetworkInterface Deserialize(byte[] buffer, NetworkInterface instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static NetworkInterface Deserialize(Stream stream, NetworkInterface instance)
			{
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 10:
						instance.name = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.mac = ProtocolParser.ReadString(stream);
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
			}

			public static NetworkInterface DeserializeLengthDelimited(Stream stream, NetworkInterface instance)
			{
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 10:
						instance.name = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.mac = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static NetworkInterface DeserializeLength(Stream stream, int length, NetworkInterface instance)
			{
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 10:
						instance.name = ProtocolParser.ReadString(stream);
						continue;
					case 18:
						instance.mac = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, NetworkInterface instance)
			{
				MemoryStream stream2 = ProtocolParser.Stack.Pop();
				if (instance.name == null)
				{
					throw new ProtocolBufferException("name is required by the proto specification.");
				}
				stream.WriteByte(10);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.name));
				if (instance.mac == null)
				{
					throw new ProtocolBufferException("mac is required by the proto specification.");
				}
				stream.WriteByte(18);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.mac));
				ProtocolParser.Stack.Push(stream2);
			}

			public static byte[] SerializeToBytes(NetworkInterface instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, NetworkInterface instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataContract]
		public class SimInfo
		{
			[DataMember]
			public uint? country_code { get; set; }

			[DataMember]
			public uint? operator_id { get; set; }

			[DataMember]
			public string operator_name { get; set; }

			[DataMember]
			public bool? data_roaming { get; set; }

			[DataMember]
			public string icc_id { get; set; }

			public static SimInfo Deserialize(Stream stream)
			{
				SimInfo simInfo = new SimInfo();
				Deserialize(stream, simInfo);
				return simInfo;
			}

			public static SimInfo DeserializeLengthDelimited(Stream stream)
			{
				SimInfo simInfo = new SimInfo();
				DeserializeLengthDelimited(stream, simInfo);
				return simInfo;
			}

			public static SimInfo DeserializeLength(Stream stream, int length)
			{
				SimInfo simInfo = new SimInfo();
				DeserializeLength(stream, length, simInfo);
				return simInfo;
			}

			public static SimInfo Deserialize(byte[] buffer)
			{
				SimInfo simInfo = new SimInfo();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, simInfo);
					return simInfo;
				}
			}

			public static SimInfo Deserialize(byte[] buffer, SimInfo instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			public static SimInfo Deserialize(Stream stream, SimInfo instance)
			{
				instance.data_roaming = false;
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 8:
						instance.country_code = ProtocolParser.ReadUInt32(stream);
						continue;
					case 16:
						instance.operator_id = ProtocolParser.ReadUInt32(stream);
						continue;
					case 26:
						instance.operator_name = ProtocolParser.ReadString(stream);
						continue;
					case 32:
						instance.data_roaming = ProtocolParser.ReadBool(stream);
						continue;
					case 42:
						instance.icc_id = ProtocolParser.ReadString(stream);
						continue;
					case -1:
						return instance;
					}
					Key key = ProtocolParser.ReadKey((byte)num, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
			}

			public static SimInfo DeserializeLengthDelimited(Stream stream, SimInfo instance)
			{
				instance.data_roaming = false;
				long num = ProtocolParser.ReadUInt32(stream);
				num += stream.Position;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 8:
						instance.country_code = ProtocolParser.ReadUInt32(stream);
						continue;
					case 16:
						instance.operator_id = ProtocolParser.ReadUInt32(stream);
						continue;
					case 26:
						instance.operator_name = ProtocolParser.ReadString(stream);
						continue;
					case 32:
						instance.data_roaming = ProtocolParser.ReadBool(stream);
						continue;
					case 42:
						instance.icc_id = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static SimInfo DeserializeLength(Stream stream, int length, SimInfo instance)
			{
				instance.data_roaming = false;
				long num = stream.Position + length;
				while (stream.Position < num)
				{
					int num2 = stream.ReadByte();
					switch (num2)
					{
					case -1:
						throw new EndOfStreamException();
					case 8:
						instance.country_code = ProtocolParser.ReadUInt32(stream);
						continue;
					case 16:
						instance.operator_id = ProtocolParser.ReadUInt32(stream);
						continue;
					case 26:
						instance.operator_name = ProtocolParser.ReadString(stream);
						continue;
					case 32:
						instance.data_roaming = ProtocolParser.ReadBool(stream);
						continue;
					case 42:
						instance.icc_id = ProtocolParser.ReadString(stream);
						continue;
					}
					Key key = ProtocolParser.ReadKey((byte)num2, stream);
					if (key.Field == 0)
					{
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					ProtocolParser.SkipKey(stream, key);
				}
				if (stream.Position != num)
				{
					throw new ProtocolBufferException("Read past max limit");
				}
				return instance;
			}

			public static void Serialize(Stream stream, SimInfo instance)
			{
				MemoryStream stream2 = ProtocolParser.Stack.Pop();
				if (instance.country_code.HasValue)
				{
					stream.WriteByte(8);
					ProtocolParser.WriteUInt32(stream, instance.country_code.Value);
				}
				if (instance.operator_id.HasValue)
				{
					stream.WriteByte(16);
					ProtocolParser.WriteUInt32(stream, instance.operator_id.Value);
				}
				if (instance.operator_name != null)
				{
					stream.WriteByte(26);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.operator_name));
				}
				if (instance.data_roaming.HasValue)
				{
					stream.WriteByte(32);
					ProtocolParser.WriteBool(stream, instance.data_roaming.Value);
				}
				if (instance.icc_id != null)
				{
					stream.WriteByte(42);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.icc_id));
				}
				ProtocolParser.Stack.Push(stream2);
			}

			public static byte[] SerializeToBytes(SimInfo instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			public static void SerializeLengthDelimited(Stream stream, SimInfo instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		[DataMember]
		public Time send_time { get; set; }

		[DataMember]
		public Time receive_time { get; set; }

		[DataMember]
		public List<Session> sessions { get; set; }

		[DataMember]
		public RequestParameters report_request_parameters { get; set; }

		[DataMember]
		public List<EnvironmentVariable> app_environment { get; set; }

		[DataMember]
		public List<NetworkInterface> network_interfaces { get; set; }

		[DataMember]
		public List<string> imei { get; set; }

		[DataMember]
		public List<SimInfo> sim_info { get; set; }

		internal static ReportMessage Deserialize(Stream stream)
		{
			ReportMessage reportMessage = new ReportMessage();
			Deserialize(stream, reportMessage);
			return reportMessage;
		}

		internal static ReportMessage DeserializeLengthDelimited(Stream stream)
		{
			ReportMessage reportMessage = new ReportMessage();
			DeserializeLengthDelimited(stream, reportMessage);
			return reportMessage;
		}

		internal static ReportMessage DeserializeLength(Stream stream, int length)
		{
			ReportMessage reportMessage = new ReportMessage();
			DeserializeLength(stream, length, reportMessage);
			return reportMessage;
		}

		internal static ReportMessage Deserialize(byte[] buffer)
		{
			ReportMessage reportMessage = new ReportMessage();
			using (MemoryStream stream = new MemoryStream(buffer))
			{
				Deserialize(stream, reportMessage);
				return reportMessage;
			}
		}

		internal static ReportMessage Deserialize(byte[] buffer, ReportMessage instance)
		{
			using (MemoryStream stream = new MemoryStream(buffer))
			{
				Deserialize(stream, instance);
				return instance;
			}
		}

		internal static ReportMessage Deserialize(Stream stream, ReportMessage instance)
		{
			if (instance.sessions == null)
			{
				instance.sessions = new List<Session>();
			}
			if (instance.app_environment == null)
			{
				instance.app_environment = new List<EnvironmentVariable>();
			}
			if (instance.network_interfaces == null)
			{
				instance.network_interfaces = new List<NetworkInterface>();
			}
			if (instance.imei == null)
			{
				instance.imei = new List<string>();
			}
			if (instance.sim_info == null)
			{
				instance.sim_info = new List<SimInfo>();
			}
			while (true)
			{
				int num = stream.ReadByte();
				switch (num)
				{
				case 10:
					if (instance.send_time == null)
					{
						instance.send_time = Time.DeserializeLengthDelimited(stream);
					}
					else
					{
						Time.DeserializeLengthDelimited(stream, instance.send_time);
					}
					continue;
				case 18:
					if (instance.receive_time == null)
					{
						instance.receive_time = Time.DeserializeLengthDelimited(stream);
					}
					else
					{
						Time.DeserializeLengthDelimited(stream, instance.receive_time);
					}
					continue;
				case 26:
					instance.sessions.Add(Session.DeserializeLengthDelimited(stream));
					continue;
				case 34:
					if (instance.report_request_parameters == null)
					{
						instance.report_request_parameters = RequestParameters.DeserializeLengthDelimited(stream);
					}
					else
					{
						RequestParameters.DeserializeLengthDelimited(stream, instance.report_request_parameters);
					}
					continue;
				case 58:
					instance.app_environment.Add(EnvironmentVariable.DeserializeLengthDelimited(stream));
					continue;
				case 66:
					instance.network_interfaces.Add(NetworkInterface.DeserializeLengthDelimited(stream));
					continue;
				case 74:
					instance.imei.Add(ProtocolParser.ReadString(stream));
					continue;
				case 82:
					instance.sim_info.Add(SimInfo.DeserializeLengthDelimited(stream));
					continue;
				case -1:
					return instance;
				}
				Key key = ProtocolParser.ReadKey((byte)num, stream);
				if (key.Field == 0)
				{
					throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
				}
				ProtocolParser.SkipKey(stream, key);
			}
		}

		internal static ReportMessage DeserializeLengthDelimited(Stream stream, ReportMessage instance)
		{
			if (instance.sessions == null)
			{
				instance.sessions = new List<Session>();
			}
			if (instance.app_environment == null)
			{
				instance.app_environment = new List<EnvironmentVariable>();
			}
			if (instance.network_interfaces == null)
			{
				instance.network_interfaces = new List<NetworkInterface>();
			}
			if (instance.imei == null)
			{
				instance.imei = new List<string>();
			}
			if (instance.sim_info == null)
			{
				instance.sim_info = new List<SimInfo>();
			}
			long num = ProtocolParser.ReadUInt32(stream);
			num += stream.Position;
			while (stream.Position < num)
			{
				int num2 = stream.ReadByte();
				switch (num2)
				{
				case -1:
					throw new EndOfStreamException();
				case 10:
					if (instance.send_time == null)
					{
						instance.send_time = Time.DeserializeLengthDelimited(stream);
					}
					else
					{
						Time.DeserializeLengthDelimited(stream, instance.send_time);
					}
					continue;
				case 18:
					if (instance.receive_time == null)
					{
						instance.receive_time = Time.DeserializeLengthDelimited(stream);
					}
					else
					{
						Time.DeserializeLengthDelimited(stream, instance.receive_time);
					}
					continue;
				case 26:
					instance.sessions.Add(Session.DeserializeLengthDelimited(stream));
					continue;
				case 34:
					if (instance.report_request_parameters == null)
					{
						instance.report_request_parameters = RequestParameters.DeserializeLengthDelimited(stream);
					}
					else
					{
						RequestParameters.DeserializeLengthDelimited(stream, instance.report_request_parameters);
					}
					continue;
				case 58:
					instance.app_environment.Add(EnvironmentVariable.DeserializeLengthDelimited(stream));
					continue;
				case 66:
					instance.network_interfaces.Add(NetworkInterface.DeserializeLengthDelimited(stream));
					continue;
				case 74:
					instance.imei.Add(ProtocolParser.ReadString(stream));
					continue;
				case 82:
					instance.sim_info.Add(SimInfo.DeserializeLengthDelimited(stream));
					continue;
				}
				Key key = ProtocolParser.ReadKey((byte)num2, stream);
				if (key.Field == 0)
				{
					throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
				}
				ProtocolParser.SkipKey(stream, key);
			}
			if (stream.Position != num)
			{
				throw new ProtocolBufferException("Read past max limit");
			}
			return instance;
		}

		internal static ReportMessage DeserializeLength(Stream stream, int length, ReportMessage instance)
		{
			if (instance.sessions == null)
			{
				instance.sessions = new List<Session>();
			}
			if (instance.app_environment == null)
			{
				instance.app_environment = new List<EnvironmentVariable>();
			}
			if (instance.network_interfaces == null)
			{
				instance.network_interfaces = new List<NetworkInterface>();
			}
			if (instance.imei == null)
			{
				instance.imei = new List<string>();
			}
			if (instance.sim_info == null)
			{
				instance.sim_info = new List<SimInfo>();
			}
			long num = stream.Position + length;
			while (stream.Position < num)
			{
				int num2 = stream.ReadByte();
				switch (num2)
				{
				case -1:
					throw new EndOfStreamException();
				case 10:
					if (instance.send_time == null)
					{
						instance.send_time = Time.DeserializeLengthDelimited(stream);
					}
					else
					{
						Time.DeserializeLengthDelimited(stream, instance.send_time);
					}
					continue;
				case 18:
					if (instance.receive_time == null)
					{
						instance.receive_time = Time.DeserializeLengthDelimited(stream);
					}
					else
					{
						Time.DeserializeLengthDelimited(stream, instance.receive_time);
					}
					continue;
				case 26:
					instance.sessions.Add(Session.DeserializeLengthDelimited(stream));
					continue;
				case 34:
					if (instance.report_request_parameters == null)
					{
						instance.report_request_parameters = RequestParameters.DeserializeLengthDelimited(stream);
					}
					else
					{
						RequestParameters.DeserializeLengthDelimited(stream, instance.report_request_parameters);
					}
					continue;
				case 58:
					instance.app_environment.Add(EnvironmentVariable.DeserializeLengthDelimited(stream));
					continue;
				case 66:
					instance.network_interfaces.Add(NetworkInterface.DeserializeLengthDelimited(stream));
					continue;
				case 74:
					instance.imei.Add(ProtocolParser.ReadString(stream));
					continue;
				case 82:
					instance.sim_info.Add(SimInfo.DeserializeLengthDelimited(stream));
					continue;
				}
				Key key = ProtocolParser.ReadKey((byte)num2, stream);
				if (key.Field == 0)
				{
					throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
				}
				ProtocolParser.SkipKey(stream, key);
			}
			if (stream.Position != num)
			{
				throw new ProtocolBufferException("Read past max limit");
			}
			return instance;
		}

		internal static void Serialize(Stream stream, ReportMessage instance)
		{
			MemoryStream memoryStream = ProtocolParser.Stack.Pop();
			if (instance.send_time == null)
			{
				throw new ProtocolBufferException("send_time is required by the proto specification.");
			}
			stream.WriteByte(10);
			memoryStream.SetLength(0L);
			Time.Serialize(memoryStream, instance.send_time);
			uint val = (uint)memoryStream.Length;
			ProtocolParser.WriteUInt32(stream, val);
			memoryStream.WriteTo(stream);
			if (instance.receive_time != null)
			{
				stream.WriteByte(18);
				memoryStream.SetLength(0L);
				Time.Serialize(memoryStream, instance.receive_time);
				uint val2 = (uint)memoryStream.Length;
				ProtocolParser.WriteUInt32(stream, val2);
				memoryStream.WriteTo(stream);
			}
			if (instance.sessions != null)
			{
				foreach (Session session in instance.sessions)
				{
					stream.WriteByte(26);
					memoryStream.SetLength(0L);
					Session.Serialize(memoryStream, session);
					uint val3 = (uint)memoryStream.Length;
					ProtocolParser.WriteUInt32(stream, val3);
					memoryStream.WriteTo(stream);
				}
			}
			if (instance.report_request_parameters != null)
			{
				stream.WriteByte(34);
				memoryStream.SetLength(0L);
				RequestParameters.Serialize(memoryStream, instance.report_request_parameters);
				uint val4 = (uint)memoryStream.Length;
				ProtocolParser.WriteUInt32(stream, val4);
				memoryStream.WriteTo(stream);
			}
			if (instance.app_environment != null)
			{
				foreach (EnvironmentVariable item in instance.app_environment)
				{
					stream.WriteByte(58);
					memoryStream.SetLength(0L);
					EnvironmentVariable.Serialize(memoryStream, item);
					uint val5 = (uint)memoryStream.Length;
					ProtocolParser.WriteUInt32(stream, val5);
					memoryStream.WriteTo(stream);
				}
			}
			if (instance.network_interfaces != null)
			{
				foreach (NetworkInterface network_interface in instance.network_interfaces)
				{
					stream.WriteByte(66);
					memoryStream.SetLength(0L);
					NetworkInterface.Serialize(memoryStream, network_interface);
					uint val6 = (uint)memoryStream.Length;
					ProtocolParser.WriteUInt32(stream, val6);
					memoryStream.WriteTo(stream);
				}
			}
			if (instance.imei != null)
			{
				foreach (string item2 in instance.imei)
				{
					stream.WriteByte(74);
					ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(item2));
				}
			}
			if (instance.sim_info != null)
			{
				foreach (SimInfo item3 in instance.sim_info)
				{
					stream.WriteByte(82);
					memoryStream.SetLength(0L);
					SimInfo.Serialize(memoryStream, item3);
					uint val7 = (uint)memoryStream.Length;
					ProtocolParser.WriteUInt32(stream, val7);
					memoryStream.WriteTo(stream);
				}
			}
			ProtocolParser.Stack.Push(memoryStream);
		}

		internal static byte[] SerializeToBytes(ReportMessage instance)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serialize(memoryStream, instance);
				return memoryStream.ToArray();
			}
		}

		internal static void SerializeLengthDelimited(Stream stream, ReportMessage instance)
		{
			byte[] array = SerializeToBytes(instance);
			ProtocolParser.WriteUInt32(stream, (uint)array.Length);
			stream.Write(array, 0, array.Length);
		}
	}
}
