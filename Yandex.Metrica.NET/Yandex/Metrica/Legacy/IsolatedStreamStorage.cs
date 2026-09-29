using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace Yandex.Metrica.Legacy
{
	internal class IsolatedStreamStorage<T>
	{
		private readonly IStreamSerializer<T> _serializer;

		public IsolatedStreamStorage(IStreamSerializer<T> sessionsProtoSerializer)
		{
			_serializer = sessionsProtoSerializer;
		}

		public Task SaveAsync(string resourceLocation, T obj)
		{
			using (IsolatedStorageFileStream stream = GetStore().OpenFile(resourceLocation, FileMode.Create))
			{
				_serializer.Serialize(stream, obj);
			}
			return Task.FromResult(new object());
		}

		public Task<T> ReadAsync(string resourceLocation)
		{
			try
			{
				IsolatedStorageFile store = GetStore();
				if (store.FileExists(resourceLocation))
				{
					using (IsolatedStorageFileStream stream = store.OpenFile(resourceLocation, FileMode.Open))
					{
						return Task.FromResult(_serializer.Deserialize(stream));
					}
				}
			}
			catch
			{
			}
			return Task.FromResult(default(T));
		}

		public Task DeleteAsync(string resourceLocation)
		{
			try
			{
				IsolatedStorageFile store = GetStore();
				if (store.FileExists(resourceLocation))
				{
					store.DeleteFile(resourceLocation);
				}
			}
			catch
			{
			}
			return Task.FromResult(new object());
		}

		private static IsolatedStorageFile GetStore()
		{
			return IsolatedStorageFile.GetUserStoreForDomain();
		}
	}
}
