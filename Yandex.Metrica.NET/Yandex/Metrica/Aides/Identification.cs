using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using Yandex.Metrica.Models;

namespace Yandex.Metrica.Aides
{
	internal static class Identification
	{
		public static string GetDeviceId()
		{
			return ServiceData.Device.Id ?? string.Empty;
		}

		public static string GetNetworkAdapterId()
		{
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface networkInterface = allNetworkInterfaces.FirstOrDefault((NetworkInterface i) => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) ?? allNetworkInterfaces.FirstOrDefault((NetworkInterface i) => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet) ?? allNetworkInterfaces.FirstOrDefault();
				if (networkInterface == null)
				{
					return null;
				}
				string id = networkInterface.Id;
				return string.IsNullOrEmpty(id) ? null : id;
			}
			catch
			{
				return null;
			}
		}

		public static string GetAdvertisingId()
		{
			return null;
		}

		public static ProductInfo GetProductInfo()
		{
			Assembly entryAssembly = Adapter.GetEntryAssembly();
			if (!(entryAssembly == null))
			{
				return new ProductInfo
				{
					Id = entryAssembly.GetName().Name,
					Version = entryAssembly.GetName().Version
				};
			}
			return new ProductInfo
			{
				Id = Config.Global.Id.ToString(),
				Version = new Version("0.0")
			};
		}
	}
}
