using System;

namespace Yandex.Metrica.Patterns
{
	internal abstract class ADataProvider<TData>
	{
		public TData Provide()
		{
			try
			{
				return ProvideOrThrowException();
			}
			catch (Exception)
			{
				return ProvideDefault();
			}
		}

		protected abstract TData ProvideOrThrowException();

		protected virtual TData ProvideDefault()
		{
			return default(TData);
		}
	}
}
