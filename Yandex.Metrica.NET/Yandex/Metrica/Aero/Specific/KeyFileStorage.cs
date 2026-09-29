using System.IO;

namespace Yandex.Metrica.Aero.Specific
{
	internal class KeyFileStorage : IStorage
	{
		public Stream GetReadStream(string key)
		{
			return File.OpenRead(key);
		}

		public Stream GetWriteStream(string key)
		{
			return File.Open(key, FileMode.Create);
		}

		public void DeleteKey(string key)
		{
			if (File.Exists(key))
			{
				File.Delete(key);
			}
		}

		public bool HasKey(string key)
		{
			return File.Exists(key);
		}

		public long Length(string key)
		{
			if (!File.Exists(key))
			{
				return 0L;
			}
			return File.OpenRead(key).Length;
		}

		public bool DirectoryExists(string path)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				return Directory.Exists(path);
			}
			return true;
		}

		public void CreateDirectory(string path)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}
