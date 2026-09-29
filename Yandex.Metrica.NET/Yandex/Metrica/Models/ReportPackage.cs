using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
	[DataContract]
	internal class ReportPackage
	{
		public const int MaxReportPackageLength = 229376;

		private ReportMessage _reportMessage;

		private MemoryStream _rawStream;

		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string UrlParameters { get; set; }

		public bool IsLarge => Length > 229376;

		public long Length => _rawStream?.Length ?? Memory.ActiveBox.Storage.Length(Key);

		public ReportPackage(string urlParameters, IEnumerable<ReportMessage.Session> sessions)
		{
			Key = string.Format(Memory.ActiveBox.KeyFormat, Guid.NewGuid());
			UrlParameters = urlParameters;
			_reportMessage = ToReportMessage(sessions);
			_rawStream = GetRawStream();
		}

		~ReportPackage()
		{
			_rawStream?.Dispose();
		}

		public bool Exists()
		{
			return Memory.ActiveBox.Storage.HasKey(Key);
		}

		public void Fade()
		{
			Memory.ActiveBox.Storage.DeleteKey(Key);
			_rawStream?.Dispose();
		}

		public void Keep()
		{
			if (_reportMessage == null)
			{
				return;
			}
			using (Stream stream = Memory.ActiveBox.Storage.GetWriteStream(Key))
			{
				ReportMessage.Serialize(stream, _reportMessage);
			}
		}

		public void Revive()
		{
			using (Stream stream = Memory.ActiveBox.Storage.GetReadStream(Key))
			{
				_reportMessage = ReportMessage.Deserialize(stream);
			}
		}

		public MemoryStream GetRawStream()
		{
			if (_rawStream != null)
			{
				_rawStream.Position = 0L;
				return _rawStream;
			}
			if (Exists())
			{
				Revive();
			}
			_rawStream = new MemoryStream();
			_reportMessage?.Write(_rawStream);
			_rawStream.Position = 0L;
			return _rawStream;
		}

		private static ReportMessage ToReportMessage(IEnumerable<ReportMessage.Session> sessions)
		{
			DateTimeOffset now = DateTimeOffset.Now;
			return new ReportMessage
			{
				sessions = sessions.ToList(),
				send_time = new ReportMessage.Time
				{
					timestamp = now.DateTime.ToUnixTime(),
					time_zone = (int)now.Offset.TotalSeconds
				}
			};
		}

		public List<ReportPackage> Split()
		{
			int count = _reportMessage.sessions.Count;
			int num = count / 2;
			if (count > 1)
			{
				return new List<ReportPackage>
				{
					new ReportPackage(UrlParameters, _reportMessage.sessions.GetRange(0, num)),
					new ReportPackage(UrlParameters, _reportMessage.sessions.GetRange(num, count - num))
				};
			}
			if (!IsLarge)
			{
				return new List<ReportPackage> { this };
			}
			return new List<ReportPackage>();
		}
	}
}
