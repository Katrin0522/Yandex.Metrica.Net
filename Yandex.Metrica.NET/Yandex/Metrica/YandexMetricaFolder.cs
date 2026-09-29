namespace Yandex.Metrica
{
	public static class YandexMetricaFolder
	{
		public static string Current { get; private set; }

		static YandexMetricaFolder()
		{
			Current = "";
			Current = null;
		}

		public static void SetCurrent(string path)
		{
			Current = path ?? "";
		}
	}
}
