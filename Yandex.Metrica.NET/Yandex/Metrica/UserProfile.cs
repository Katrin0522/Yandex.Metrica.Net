using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SilentOrbit.ProtocolBuffers;

namespace Yandex.Metrica
{
	public sealed class UserProfile
	{
		private readonly List<UserProfileUpdate> _updates = new List<UserProfileUpdate>();

		internal bool IsEmpty => _updates.Count == 0;

		public UserProfile Apply(UserProfileUpdate update)
		{
			if (update == null)
			{
				throw new ArgumentNullException("update");
			}
			_updates.Add(update);
			return this;
		}

		internal byte[] ToProtobuf()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				foreach (UserProfileUpdate update in _updates)
				{
					byte[] attribute = update.ToProtobuf();
					stream.WriteByte(10);
					ProtocolParser.WriteBytes(stream, attribute);
				}
				return stream.ToArray();
			}
		}
	}

	public abstract class UserProfileUpdate
	{
		internal abstract byte[] ToProtobuf();
	}

	public static class Attribute
	{
		public static StringAttribute CustomString(string key)
		{
			return new StringAttribute(key);
		}

		public static NumberAttribute CustomNumber(string key)
		{
			return new NumberAttribute(key);
		}

		public static BooleanAttribute CustomBoolean(string key)
		{
			return new BooleanAttribute(key);
		}

		public static CounterAttribute CustomCounter(string key)
		{
			return new CounterAttribute(key);
		}

		public static NameAttribute Name()
		{
			return new NameAttribute();
		}

		public static GenderAttribute Gender()
		{
			return new GenderAttribute();
		}

		public static BirthDateAttribute BirthDate()
		{
			return new BirthDateAttribute();
		}

		public static NotificationsEnabledAttribute NotificationsEnabled()
		{
			return new NotificationsEnabledAttribute();
		}
	}

	public sealed class StringAttribute
	{
		private readonly string _key;

		internal StringAttribute(string key)
		{
			_key = ProfileUpdate.ValidateKey(key);
		}

		public UserProfileUpdate WithValue(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return new ProfileUpdate(_key, ProfileAttributeType.String, Encoding.UTF8.GetBytes(value), 0.0, false);
		}
	}

	public sealed class NumberAttribute
	{
		private readonly string _key;

		internal NumberAttribute(string key)
		{
			_key = ProfileUpdate.ValidateKey(key);
		}

		public UserProfileUpdate WithValue(double value)
		{
			return new ProfileUpdate(_key, ProfileAttributeType.Number, null, value, false);
		}
	}

	public sealed class BooleanAttribute
	{
		private readonly string _key;

		internal BooleanAttribute(string key)
		{
			_key = ProfileUpdate.ValidateKey(key);
		}

		public UserProfileUpdate WithValue(bool value)
		{
			return new ProfileUpdate(_key, ProfileAttributeType.Boolean, null, 0.0, value);
		}
	}

	public sealed class CounterAttribute
	{
		private readonly string _key;

		internal CounterAttribute(string key)
		{
			_key = ProfileUpdate.ValidateKey(key);
		}

		public UserProfileUpdate WithDelta(double value)
		{
			return new ProfileUpdate(_key, ProfileAttributeType.Counter, null, value, false);
		}
	}

	public sealed class NameAttribute
	{
		public UserProfileUpdate WithValue(string value)
		{
			return new StringAttribute("appmetrica_name").WithValue(value);
		}
	}

	public sealed class GenderAttribute
	{
		public enum Gender
		{
			Male,
			Female,
			Other
		}

		public UserProfileUpdate WithValue(Gender value)
		{
			string serializedValue = (value == Gender.Male) ? "M" : ((value == Gender.Female) ? "F" : "O");
			return new StringAttribute("appmetrica_gender").WithValue(serializedValue);
		}
	}

	public sealed class BirthDateAttribute
	{
		public UserProfileUpdate WithAge(int age)
		{
			if (age < 0)
			{
				throw new ArgumentOutOfRangeException("age");
			}
			return WithBirthDate(DateTime.UtcNow.Year - age);
		}

		public UserProfileUpdate WithBirthDate(int year)
		{
			return WithBirthDateValue(year.ToString("D4"));
		}

		public UserProfileUpdate WithBirthDate(int year, int month)
		{
			return WithBirthDateValue(new DateTime(year, month, 1).ToString("yyyy-MM"));
		}

		public UserProfileUpdate WithBirthDate(int year, int month, int day)
		{
			return WithBirthDateValue(new DateTime(year, month, day).ToString("yyyy-MM-dd"));
		}

		public UserProfileUpdate WithBirthDate(DateTime date)
		{
			return WithBirthDateValue(date.ToString("yyyy-MM-dd"));
		}

		private static UserProfileUpdate WithBirthDateValue(string value)
		{
			return new StringAttribute("appmetrica_birth_date").WithValue(value);
		}
	}

	public sealed class NotificationsEnabledAttribute
	{
		public UserProfileUpdate WithValue(bool value)
		{
			return new BooleanAttribute("appmetrica_notifications_enabled").WithValue(value);
		}
	}

	internal enum ProfileAttributeType
	{
		String,
		Number,
		Counter,
		Boolean
	}

	internal sealed class ProfileUpdate : UserProfileUpdate
	{
		private readonly string _key;
		private readonly ProfileAttributeType _type;
		private readonly byte[] _stringValue;
		private readonly double _numberValue;
		private readonly bool _booleanValue;

		public ProfileUpdate(string key, ProfileAttributeType type, byte[] stringValue, double numberValue, bool booleanValue)
		{
			_key = key;
			_type = type;
			_stringValue = stringValue;
			_numberValue = numberValue;
			_booleanValue = booleanValue;
		}

		internal static string ValidateKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentException("Profile attribute key cannot be empty.", "key");
			}
			if (key.Length > 200)
			{
				throw new ArgumentException("Profile attribute key cannot exceed 200 characters.", "key");
			}
			return key;
		}

		internal override byte[] ToProtobuf()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				stream.WriteByte(10);
				ProtocolParser.WriteBytes(stream, Encoding.UTF8.GetBytes(_key));
				stream.WriteByte(16);
				ProtocolParser.WriteUInt32(stream, (uint)_type);
				stream.WriteByte(26);
				ProtocolParser.WriteUInt32(stream, 0);
				byte[] value = SerializeValue();
				stream.WriteByte(34);
				ProtocolParser.WriteBytes(stream, value);
				return stream.ToArray();
			}
		}

		private byte[] SerializeValue()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				switch (_type)
				{
				case ProfileAttributeType.String:
					stream.WriteByte(10);
					ProtocolParser.WriteBytes(stream, _stringValue);
					break;
				case ProfileAttributeType.Number:
					WriteDouble(stream, 17, _numberValue);
					break;
				case ProfileAttributeType.Counter:
					WriteDouble(stream, 25, _numberValue);
					break;
				case ProfileAttributeType.Boolean:
					if (_booleanValue)
					{
						stream.WriteByte(32);
						stream.WriteByte(1);
					}
					break;
				}
				return stream.ToArray();
			}
		}

		private static void WriteDouble(Stream stream, byte fieldTag, double value)
		{
			stream.WriteByte(fieldTag);
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}
	}
}
