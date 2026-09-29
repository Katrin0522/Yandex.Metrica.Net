using System.IO;
using System.Text;
using SilentOrbit.ProtocolBuffers;

namespace Yandex.Metrica.Legacy
{
	internal class Config
	{
		internal class Location
		{
			public double Lat { get; set; }

			public double Lon { get; set; }

			internal static Location Deserialize(Stream stream)
			{
				Location location = new Location();
				Deserialize(stream, location);
				return location;
			}

			internal static Location DeserializeLengthDelimited(Stream stream)
			{
				Location location = new Location();
				DeserializeLengthDelimited(stream, location);
				return location;
			}

			internal static Location DeserializeLength(Stream stream, int length)
			{
				Location location = new Location();
				DeserializeLength(stream, length, location);
				return location;
			}

			internal static Location Deserialize(byte[] buffer)
			{
				Location location = new Location();
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, location);
					return location;
				}
			}

			internal static Location Deserialize(byte[] buffer, Location instance)
			{
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					Deserialize(stream, instance);
					return instance;
				}
			}

			internal static Location Deserialize(Stream stream, Location instance)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				while (true)
				{
					int num = stream.ReadByte();
					switch (num)
					{
					case 9:
						instance.Lat = binaryReader.ReadDouble();
						continue;
					case 17:
						instance.Lon = binaryReader.ReadDouble();
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

			internal static Location DeserializeLengthDelimited(Stream stream, Location instance)
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
						instance.Lat = binaryReader.ReadDouble();
						continue;
					case 17:
						instance.Lon = binaryReader.ReadDouble();
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

			internal static Location DeserializeLength(Stream stream, int length, Location instance)
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
						instance.Lat = binaryReader.ReadDouble();
						continue;
					case 17:
						instance.Lon = binaryReader.ReadDouble();
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

			internal static void Serialize(Stream stream, Location instance)
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				MemoryStream stream2 = ProtocolParser.Stack.Pop();
				stream.WriteByte(9);
				binaryWriter.Write(instance.Lat);
				stream.WriteByte(17);
				binaryWriter.Write(instance.Lon);
				ProtocolParser.Stack.Push(stream2);
			}

			internal static byte[] SerializeToBytes(Location instance)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					Serialize(memoryStream, instance);
					return memoryStream.ToArray();
				}
			}

			internal static void SerializeLengthDelimited(Stream stream, Location instance)
			{
				byte[] array = SerializeToBytes(instance);
				ProtocolParser.WriteUInt32(stream, (uint)array.Length);
				stream.Write(array, 0, array.Length);
			}
		}

		public string UUID { get; set; }

		public ulong LastStartupTime { get; set; }

		public ulong LastStopTime { get; set; }

		public ulong CurrentSessionId { get; set; }

		public ulong CurrentSessionEventCounter { get; set; }

		public bool Init { get; set; }

		public string ApiKey { get; set; }

		public ulong DispatchPeriodMilliseconds { get; set; }

		public uint MaxReportsCount { get; set; }

		public bool TrackLocationEnabled { get; set; }

		public bool ReportCrashesEnabled { get; set; }

		public string CustomStartupUrl { get; set; }

		public string CustomAppVersion { get; set; }

		public Location CustomLocation { get; set; }

		public string ReportUrl { get; set; }

		public string CheckUpdatesUrl { get; set; }

		public bool Suspended { get; set; }

		public ulong BackgroundSessionEventCounter { get; set; }

		public ulong SessionInactivityTimeoutMilliseconds { get; set; }

		public ulong LastIdentityEventMilliseconds { get; set; }

		public long ServerTimeOffset { get; set; }

		internal static Config Deserialize(Stream stream)
		{
			Config config = new Config();
			Deserialize(stream, config);
			return config;
		}

		internal static Config DeserializeLengthDelimited(Stream stream)
		{
			Config config = new Config();
			DeserializeLengthDelimited(stream, config);
			return config;
		}

		internal static Config DeserializeLength(Stream stream, int length)
		{
			Config config = new Config();
			DeserializeLength(stream, length, config);
			return config;
		}

		internal static Config Deserialize(byte[] buffer)
		{
			Config config = new Config();
			using (MemoryStream stream = new MemoryStream(buffer))
			{
				Deserialize(stream, config);
				return config;
			}
		}

		internal static Config Deserialize(byte[] buffer, Config instance)
		{
			using (MemoryStream stream = new MemoryStream(buffer))
			{
				Deserialize(stream, instance);
				return instance;
			}
		}

		internal static Config Deserialize(Stream stream, Config instance)
		{
			while (true)
			{
				int num = stream.ReadByte();
				switch (num)
				{
				case 10:
					instance.UUID = ProtocolParser.ReadString(stream);
					continue;
				case 16:
					instance.LastStartupTime = ProtocolParser.ReadUInt64(stream);
					continue;
				case 24:
					instance.LastStopTime = ProtocolParser.ReadUInt64(stream);
					continue;
				case 32:
					instance.CurrentSessionId = ProtocolParser.ReadUInt64(stream);
					continue;
				case 40:
					instance.CurrentSessionEventCounter = ProtocolParser.ReadUInt64(stream);
					continue;
				case 48:
					instance.Init = ProtocolParser.ReadBool(stream);
					continue;
				case 58:
					instance.ApiKey = ProtocolParser.ReadString(stream);
					continue;
				case 64:
					instance.DispatchPeriodMilliseconds = ProtocolParser.ReadUInt64(stream);
					continue;
				case 72:
					instance.MaxReportsCount = ProtocolParser.ReadUInt32(stream);
					continue;
				case 80:
					instance.TrackLocationEnabled = ProtocolParser.ReadBool(stream);
					continue;
				case 88:
					instance.ReportCrashesEnabled = ProtocolParser.ReadBool(stream);
					continue;
				case 98:
					instance.CustomStartupUrl = ProtocolParser.ReadString(stream);
					continue;
				case 106:
					instance.CustomAppVersion = ProtocolParser.ReadString(stream);
					continue;
				case 114:
					if (instance.CustomLocation == null)
					{
						instance.CustomLocation = Location.DeserializeLengthDelimited(stream);
					}
					else
					{
						Location.DeserializeLengthDelimited(stream, instance.CustomLocation);
					}
					continue;
				case 122:
					instance.ReportUrl = ProtocolParser.ReadString(stream);
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
					if (key.WireType == Wire.LengthDelimited)
					{
						instance.CheckUpdatesUrl = ProtocolParser.ReadString(stream);
					}
					break;
				case 17u:
					if (key.WireType == Wire.Varint)
					{
						instance.Suspended = ProtocolParser.ReadBool(stream);
					}
					break;
				case 18u:
					if (key.WireType == Wire.Varint)
					{
						instance.BackgroundSessionEventCounter = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 19u:
					if (key.WireType == Wire.Varint)
					{
						instance.SessionInactivityTimeoutMilliseconds = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 20u:
					if (key.WireType == Wire.Varint)
					{
						instance.LastIdentityEventMilliseconds = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 21u:
					if (key.WireType == Wire.Varint)
					{
						instance.ServerTimeOffset = (long)ProtocolParser.ReadUInt64(stream);
					}
					break;
				default:
					ProtocolParser.SkipKey(stream, key);
					break;
				}
			}
		}

		internal static Config DeserializeLengthDelimited(Stream stream, Config instance)
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
					instance.UUID = ProtocolParser.ReadString(stream);
					continue;
				case 16:
					instance.LastStartupTime = ProtocolParser.ReadUInt64(stream);
					continue;
				case 24:
					instance.LastStopTime = ProtocolParser.ReadUInt64(stream);
					continue;
				case 32:
					instance.CurrentSessionId = ProtocolParser.ReadUInt64(stream);
					continue;
				case 40:
					instance.CurrentSessionEventCounter = ProtocolParser.ReadUInt64(stream);
					continue;
				case 48:
					instance.Init = ProtocolParser.ReadBool(stream);
					continue;
				case 58:
					instance.ApiKey = ProtocolParser.ReadString(stream);
					continue;
				case 64:
					instance.DispatchPeriodMilliseconds = ProtocolParser.ReadUInt64(stream);
					continue;
				case 72:
					instance.MaxReportsCount = ProtocolParser.ReadUInt32(stream);
					continue;
				case 80:
					instance.TrackLocationEnabled = ProtocolParser.ReadBool(stream);
					continue;
				case 88:
					instance.ReportCrashesEnabled = ProtocolParser.ReadBool(stream);
					continue;
				case 98:
					instance.CustomStartupUrl = ProtocolParser.ReadString(stream);
					continue;
				case 106:
					instance.CustomAppVersion = ProtocolParser.ReadString(stream);
					continue;
				case 114:
					if (instance.CustomLocation == null)
					{
						instance.CustomLocation = Location.DeserializeLengthDelimited(stream);
					}
					else
					{
						Location.DeserializeLengthDelimited(stream, instance.CustomLocation);
					}
					continue;
				case 122:
					instance.ReportUrl = ProtocolParser.ReadString(stream);
					continue;
				}
				Key key = ProtocolParser.ReadKey((byte)num2, stream);
				switch (key.Field)
				{
				case 0u:
					throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
				case 16u:
					if (key.WireType == Wire.LengthDelimited)
					{
						instance.CheckUpdatesUrl = ProtocolParser.ReadString(stream);
					}
					break;
				case 17u:
					if (key.WireType == Wire.Varint)
					{
						instance.Suspended = ProtocolParser.ReadBool(stream);
					}
					break;
				case 18u:
					if (key.WireType == Wire.Varint)
					{
						instance.BackgroundSessionEventCounter = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 19u:
					if (key.WireType == Wire.Varint)
					{
						instance.SessionInactivityTimeoutMilliseconds = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 20u:
					if (key.WireType == Wire.Varint)
					{
						instance.LastIdentityEventMilliseconds = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 21u:
					if (key.WireType == Wire.Varint)
					{
						instance.ServerTimeOffset = (long)ProtocolParser.ReadUInt64(stream);
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

		internal static Config DeserializeLength(Stream stream, int length, Config instance)
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
					instance.UUID = ProtocolParser.ReadString(stream);
					continue;
				case 16:
					instance.LastStartupTime = ProtocolParser.ReadUInt64(stream);
					continue;
				case 24:
					instance.LastStopTime = ProtocolParser.ReadUInt64(stream);
					continue;
				case 32:
					instance.CurrentSessionId = ProtocolParser.ReadUInt64(stream);
					continue;
				case 40:
					instance.CurrentSessionEventCounter = ProtocolParser.ReadUInt64(stream);
					continue;
				case 48:
					instance.Init = ProtocolParser.ReadBool(stream);
					continue;
				case 58:
					instance.ApiKey = ProtocolParser.ReadString(stream);
					continue;
				case 64:
					instance.DispatchPeriodMilliseconds = ProtocolParser.ReadUInt64(stream);
					continue;
				case 72:
					instance.MaxReportsCount = ProtocolParser.ReadUInt32(stream);
					continue;
				case 80:
					instance.TrackLocationEnabled = ProtocolParser.ReadBool(stream);
					continue;
				case 88:
					instance.ReportCrashesEnabled = ProtocolParser.ReadBool(stream);
					continue;
				case 98:
					instance.CustomStartupUrl = ProtocolParser.ReadString(stream);
					continue;
				case 106:
					instance.CustomAppVersion = ProtocolParser.ReadString(stream);
					continue;
				case 114:
					if (instance.CustomLocation == null)
					{
						instance.CustomLocation = Location.DeserializeLengthDelimited(stream);
					}
					else
					{
						Location.DeserializeLengthDelimited(stream, instance.CustomLocation);
					}
					continue;
				case 122:
					instance.ReportUrl = ProtocolParser.ReadString(stream);
					continue;
				}
				Key key = ProtocolParser.ReadKey((byte)num2, stream);
				switch (key.Field)
				{
				case 0u:
					throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
				case 16u:
					if (key.WireType == Wire.LengthDelimited)
					{
						instance.CheckUpdatesUrl = ProtocolParser.ReadString(stream);
					}
					break;
				case 17u:
					if (key.WireType == Wire.Varint)
					{
						instance.Suspended = ProtocolParser.ReadBool(stream);
					}
					break;
				case 18u:
					if (key.WireType == Wire.Varint)
					{
						instance.BackgroundSessionEventCounter = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 19u:
					if (key.WireType == Wire.Varint)
					{
						instance.SessionInactivityTimeoutMilliseconds = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 20u:
					if (key.WireType == Wire.Varint)
					{
						instance.LastIdentityEventMilliseconds = ProtocolParser.ReadUInt64(stream);
					}
					break;
				case 21u:
					if (key.WireType == Wire.Varint)
					{
						instance.ServerTimeOffset = (long)ProtocolParser.ReadUInt64(stream);
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

		internal static void Serialize(Stream stream, Config instance)
		{
			MemoryStream memoryStream = ProtocolParser.Stack.Pop();
			if (instance.UUID != null)
			{
				stream.WriteByte(10);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.UUID));
			}
			stream.WriteByte(16);
			ProtocolParser.WriteUInt64(stream, instance.LastStartupTime);
			stream.WriteByte(24);
			ProtocolParser.WriteUInt64(stream, instance.LastStopTime);
			stream.WriteByte(32);
			ProtocolParser.WriteUInt64(stream, instance.CurrentSessionId);
			stream.WriteByte(40);
			ProtocolParser.WriteUInt64(stream, instance.CurrentSessionEventCounter);
			stream.WriteByte(48);
			ProtocolParser.WriteBool(stream, instance.Init);
			if (instance.ApiKey != null)
			{
				stream.WriteByte(58);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ApiKey));
			}
			stream.WriteByte(64);
			ProtocolParser.WriteUInt64(stream, instance.DispatchPeriodMilliseconds);
			stream.WriteByte(72);
			ProtocolParser.WriteUInt32(stream, instance.MaxReportsCount);
			stream.WriteByte(80);
			ProtocolParser.WriteBool(stream, instance.TrackLocationEnabled);
			stream.WriteByte(88);
			ProtocolParser.WriteBool(stream, instance.ReportCrashesEnabled);
			if (instance.CustomStartupUrl != null)
			{
				stream.WriteByte(98);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.CustomStartupUrl));
			}
			if (instance.CustomAppVersion != null)
			{
				stream.WriteByte(106);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.CustomAppVersion));
			}
			if (instance.CustomLocation != null)
			{
				stream.WriteByte(114);
				memoryStream.SetLength(0L);
				Location.Serialize(memoryStream, instance.CustomLocation);
				uint val = (uint)memoryStream.Length;
				ProtocolParser.WriteUInt32(stream, val);
				memoryStream.WriteTo(stream);
			}
			if (instance.ReportUrl != null)
			{
				stream.WriteByte(122);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.ReportUrl));
			}
			if (instance.CheckUpdatesUrl != null)
			{
				stream.WriteByte(130);
				stream.WriteByte(1);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(instance.CheckUpdatesUrl));
			}
			stream.WriteByte(136);
			stream.WriteByte(1);
			ProtocolParser.WriteBool(stream, instance.Suspended);
			stream.WriteByte(144);
			stream.WriteByte(1);
			ProtocolParser.WriteUInt64(stream, instance.BackgroundSessionEventCounter);
			stream.WriteByte(152);
			stream.WriteByte(1);
			ProtocolParser.WriteUInt64(stream, instance.SessionInactivityTimeoutMilliseconds);
			stream.WriteByte(160);
			stream.WriteByte(1);
			ProtocolParser.WriteUInt64(stream, instance.LastIdentityEventMilliseconds);
			stream.WriteByte(168);
			stream.WriteByte(1);
			ProtocolParser.WriteUInt64(stream, (ulong)instance.ServerTimeOffset);
			ProtocolParser.Stack.Push(memoryStream);
		}

		internal static byte[] SerializeToBytes(Config instance)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serialize(memoryStream, instance);
				return memoryStream.ToArray();
			}
		}

		internal static void SerializeLengthDelimited(Stream stream, Config instance)
		{
			byte[] array = SerializeToBytes(instance);
			ProtocolParser.WriteUInt32(stream, (uint)array.Length);
			stream.Write(array, 0, array.Length);
		}
	}
}
