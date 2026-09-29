using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;
using Yandex.Metrica.Properties;
using Yandex.Metrica.Providers;

namespace Yandex.Metrica
{
	internal static class ServiceData
	{
		public const string StartupUrl = "https://startup.mobile.yandex.net/";

		public const string SdkName = "com.yandex.mobile.metrica.sdk";

		public const string UnknownValue = "unknown";

		public static ProductInfo Product { get; private set; }

		public static DeviceProperties Device { get; private set; }

		public static NetworkDataProvider NetworkTracker { get; private set; }

		public static LocationDataProvider LocationTracker { get; internal set; }

		public static ILifecycler Lifecycler { get; private set; }

		public static string DeviceFingerprint { get; private set; }

		public static string UserAgent { get; private set; }

		public static bool IsExposed { get; private set; }

		static ServiceData()
		{
			Expose();
		}

		private static async void Expose()
		{
			Lifecycler = Store.Get<Lifecycler>(new object[0]);
			Product = Identification.GetProductInfo();
			Device = await Store.Get<DeviceDataProvider>(new object[0]).Provide();
			DeviceFingerprint = await Store.Get<FingerprintDataProvider>(new object[0]).Provide();
			NetworkTracker = Store.Get<NetworkDataProvider>(new object[0]);
			LocationTracker = Store.Get<LocationDataProvider>(new object[0]);
			UserAgent = string.Format("{0}/{1}.{2} ", "com.yandex.mobile.metrica.sdk", "3.5.1", 246) + $"({Device.Manufacturer} {Device.ModelName}; {Device.OSPlatform} {Device.OSVersion})";
			IsExposed = true;
		}

		public static async Task WaitExposeAsync()
		{
			if (IsExposed)
			{
				return;
			}
			await TaskEx.Run(() =>
			{
				while (!IsExposed)
				{
					TaskEx.Delay(TimeSpan.FromMilliseconds(250.0)).Wait();
				}
			});
		}

		private static async Task<Dictionary<string, object>> GetCommonRequestParameters()
		{
			await WaitExposeAsync();
			string value = (string.IsNullOrWhiteSpace(Config.Global.CustomAppId) ? Product.Id : Config.Global.CustomAppId);
			return new Dictionary<string, object>
			{
				{ "app_id", value },
				{ "model", Device.ModelName },
				{
					"locale",
					Config.GetLocale()
				},
				{
					"os_version",
					Device.OSVersion.ToString()
				},
				{ "manufacturer", Device.Manufacturer },
				{ "app_platform", Device.StartupPlatform },
				{ "screen_dpi", Device.Dpi },
				{
					"scalefactor",
					Device.ScaleFactor.ToString(CultureInfo.InvariantCulture)
				},
				{
					"screen_width",
					Device.ScreenWidth.ToString(CultureInfo.InvariantCulture)
				},
				{
					"screen_height",
					Device.ScreenHeight.ToString(CultureInfo.InvariantCulture)
				},
				{ "analytics_sdk_version", 351 }
			};
		}

		public static async Task<Dictionary<string, object>> GetStartupParameters()
		{
			Dictionary<string, object> parameters = default(Dictionary<string, object>);
			Dictionary<string, object> dictionary = parameters;
			parameters = await GetCommonRequestParameters();
			new Dictionary<string, object>
			{
				{ "protocol_version", 2 },
				{ "analytics_sdk_version_name", "3.5.1" }
			}.ForEach((KeyValuePair<string, object> p) =>
			{
				parameters[p.Key] = p.Value;
			});
			return parameters;
		}

		public static async Task<Dictionary<string, object>> GetReportParameters()
		{
			Dictionary<string, object> parameters = default(Dictionary<string, object>);
			Dictionary<string, object> dictionary = parameters;
			parameters = await GetCommonRequestParameters();
			Version version = Config.Global.CustomAppVersion ?? Product.Version;
			new Dictionary<string, object>
			{
				{
					"api_key_128",
					Config.Global.ApiKey
				},
				{ "app_framework", "native" },
				{
					"windows_aid",
					Identification.GetAdvertisingId()
				},
				{
					"device_type",
					Device.GetDeviceType().ToLower()
				},
				{ "analytics_sdk_build_number", 246 },
				{
					"analytics_sdk_build_type",
					AssemblyProperties.Edition
				},
				{ "app_build_number", version.Build },
				{
					"app_version_name",
					version.ToString()
				},
				{ "app_platform", "WindowsPhone" }
			}.ForEach((KeyValuePair<string, object> p) =>
			{
				parameters[p.Key] = p.Value;
			});
			return parameters;
		}
	}
}
