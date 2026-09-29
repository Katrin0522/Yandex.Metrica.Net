using System;

namespace Yandex.Metrica
{
	public class YandexMetricaActivator
	{
		public string ApiKey
		{
			get
			{
				return YandexMetrica.Config.ApiKey.ToString();
			}
			set
			{
				if (!DesignMode)
				{
					YandexMetrica.Activate(new Guid(value));
				}
			}
		}

		private static bool DesignMode => false;
	}
}
