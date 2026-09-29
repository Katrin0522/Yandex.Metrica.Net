using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Yandex.Metrica.Aero;

namespace Yandex.Metrica.Aides
{
	public static class JsonSerializer
	{
		internal static readonly long DatetimeMinTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

		private static string GetIndent(int indentLevel, string indent)
		{
			string text = string.Empty;
			for (int i = 0; i < indentLevel; i++)
			{
				text += indent;
			}
			return text;
		}

		public static string ToJson(this object value, Type memberType = null)
		{
			return value.ToJson(JsonProfile.GetFormatted(), memberType);
		}

		public static string ToJson(this object value, JsonProfile profile, Type memberType = null, int indentLevel = 1)
		{
			if (!value.IsSimpleValue(profile))
			{
				if (!value.IsArray(profile))
				{
					return value.ToObject(profile, memberType, indentLevel);
				}
				return value.ToArray(profile, indentLevel);
			}
			return value.ToSimpleValue(profile);
		}

		private static bool IsSimpleValue(this object value, JsonProfile profile)
		{
			if (value != null && !(value is string) && !(value is Guid) && !(value is Uri) && !(value is DateTime) && !(value is decimal) && !(value is Enum) && !value.GetType().GetTypeInfo().IsPrimitive)
			{
				if (profile.SimpleDictionaryFormat)
				{
					if (!value.GetType().Name.StartsWith(profile.KeyValuePairName))
					{
						return value is DictionaryEntry;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private static bool IsArray(this object value, JsonProfile profile)
		{
			if (profile.SimpleDictionaryFormat && value is IDictionary)
			{
				return false;
			}
			ICollection collection = value as ICollection;
			if (collection != null)
			{
				return !collection.GetType().GetCustomAttributes(typeof(DataContractAttribute), true).Any();
			}
			return false;
		}

		private static string ToSimpleValue(this object value, JsonProfile profile)
		{
			if (value == null)
			{
				return profile.NullLiteral;
			}
			if (value is string || value is Guid || value is Uri)
			{
				return "\"" + Escape(value.ToString()) + "\"";
			}
			if (value is decimal)
			{
				return value.Of<decimal>().ToString("G", CultureInfo.InvariantCulture);
			}
			if (value is double)
			{
				return value.Of<double>().ToString("G", CultureInfo.InvariantCulture);
			}
			if (value is float)
			{
				return value.Of<float>().ToString("G", CultureInfo.InvariantCulture);
			}
			if (value is Enum)
			{
				return value.Of<int>().ToString();
			}
			if (profile.SimpleDictionaryFormat && value is DictionaryEntry)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)value;
				return string.Format(profile.DictionaryEntryPattern, Escape(dictionaryEntry.Key.ToString()), dictionaryEntry.Value.ToJson(profile, typeof(object)));
			}
			if (value is DateTime)
			{
				DateTime dateTime = value.Of<DateTime>();
				if (profile.DateTimeFormat == null)
				{
					return "\"\\/Date(" + (dateTime.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000 + "+" + DateTimeOffset.Now.Offset.ToString("hhmm") + ")\\/\"";
				}
				return dateTime.ToString(profile.DateTimeFormat.FormatProvider);
			}
			if (!object.Equals(value, true))
			{
				if (!object.Equals(value, false))
				{
					return Escape(value.ToString());
				}
				return profile.FalseLiteral;
			}
			return profile.TrueLiteral;
		}

		private static string ToArray(this object value, JsonProfile profile, int indentLevel)
		{
			ICollection collection = value as ICollection;
			if (collection == null || collection.Count == 0)
			{
				return profile.EmptyArray;
			}
			string emptyDelimiter = profile.NewLine + GetIndent(indentLevel - 1, profile.IndentChars);
			string indent = profile.NewLine + GetIndent(indentLevel, profile.IndentChars);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(collection, profile, profile.Delimiter, emptyDelimiter, indent, indentLevel);
			return string.Concat("[", stringBuilder, "]");
		}

		private static string ToObject(this object value, JsonProfile profile, Type memberType, int indentLevel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = profile.NewLine + GetIndent(indentLevel, profile.IndentChars);
			string text2 = profile.NewLine + GetIndent(indentLevel - 1, profile.IndentChars);
			IDictionary dictionary = value as IDictionary;
			if (dictionary != null && profile.SimpleDictionaryFormat)
			{
				if (dictionary.Count == 0)
				{
					return profile.EmptyObject;
				}
				stringBuilder.Append(dictionary, profile, profile.Delimiter, text2, text);
			}
			else
			{
				Type type = value.GetType();
				Dictionary<string, MemberInfo> dataMembers = GetDataMembers(value);
				bool flag = dataMembers.Count == 0;
				if (memberType != null && memberType != type)
				{
					string value2 = (flag ? text2 : profile.Delimiter);
					stringBuilder.Append(text);
					string text3 = type.FullName.Substring((type.Namespace != null) ? (type.Namespace.Length + 1) : 0).Replace("+", ".");
					stringBuilder.Append(string.Format(profile.DictionaryEntryPattern, "__type", "\"" + text3 + ":#" + type.Namespace + "\""));
					stringBuilder.Append(value2);
					if (flag)
					{
						return string.Concat("{", stringBuilder, "}");
					}
				}
				if (flag)
				{
					return profile.EmptyObject;
				}
				Dictionary<string, string> items = dataMembers.ToDictionary((KeyValuePair<string, MemberInfo> p) => p.Key, (KeyValuePair<string, MemberInfo> p) => p.Value.GetValue(value).ToJson(profile, p.Value.GetMemberType(), indentLevel + 1));
				stringBuilder.Append(items, profile.DictionaryEntryPattern, profile.Delimiter, text2, text);
			}
			return string.Concat("{", stringBuilder, "}");
		}

		private static Dictionary<string, MemberInfo> GetDataMembers(object item)
		{
			Type type = item.GetType();
			return ((item is DictionaryEntry) ? type.GetMembers().ToDictionary((MemberInfo i) => i, (MemberInfo i) => (DataMemberAttribute)null).ToList() : (from p in type.GetMembers().ToDictionary((MemberInfo i) => i, (MemberInfo i) => i.GetCustomAttribute<DataMemberAttribute>())
				where p.Value != null
				orderby p.Value.Name, p.Value.Order
				select p).ToList()).ToDictionary((KeyValuePair<MemberInfo, DataMemberAttribute> p) =>
			{
				string name;
				if (p.Value != null)
				{
					name = p.Value.Name;
					if (name == null)
					{
						return p.Key.Name;
					}
				}
				else
				{
					name = p.Key.Name;
				}
				return name;
			}, (KeyValuePair<MemberInfo, DataMemberAttribute> p) => p.Key);
		}

		private static Type GetMemberType(this MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.FieldType;
			}
			return null;
		}

		private static object GetValue(this MemberInfo memberInfo, object obj)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(obj, null);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(obj);
			}
			return null;
		}

		private static void Append(this StringBuilder jsonBuilder, Dictionary<string, string> items, string keyValuePattern, string actualDelimiter, string emptyDelimiter, string indent)
		{
			int num = 1;
			foreach (KeyValuePair<string, string> item in items)
			{
				string value = ((num++ == items.Count) ? emptyDelimiter : actualDelimiter);
				jsonBuilder.Append(indent);
				jsonBuilder.Append(string.Format(keyValuePattern, item.Key, item.Value));
				jsonBuilder.Append(value);
			}
		}

		private static void Append(this StringBuilder jsonBuilder, IDictionary items, JsonProfile profile, string actualDelimiter, string emptyDelimiter, string indent)
		{
			int num = 1;
			foreach (object item in items)
			{
				string value = ((num++ == items.Count) ? emptyDelimiter : actualDelimiter);
				jsonBuilder.Append(indent);
				jsonBuilder.Append(item.ToJson(profile));
				jsonBuilder.Append(value);
			}
		}

		private static void Append(this StringBuilder jsonBuilder, ICollection items, JsonProfile profile, string actualDelimiter, string emptyDelimiter, string indent, int indentLevel)
		{
			int num = 1;
			foreach (object item in items)
			{
				string value = ((num++ == items.Count) ? emptyDelimiter : actualDelimiter);
				jsonBuilder.Append(indent);
				jsonBuilder.Append(item.ToJson(profile, typeof(object), indentLevel + 1));
				jsonBuilder.Append(value);
			}
		}

		public static string Escape(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in value)
			{
				switch (c)
				{
				case '"':
					stringBuilder.Append("\\\"");
					continue;
				case '\\':
					stringBuilder.Append("\\\\");
					continue;
				case '/':
					stringBuilder.Append("\\/");
					continue;
				}
				int num = c;
				if (num < 32 || num > 127)
				{
					stringBuilder.AppendFormat("\\u{0:x04}", num);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
