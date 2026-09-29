using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
	internal class DeviceDataProvider : ADataProvider<Task<DeviceProperties>>
	{
		private const string StartupPlatform = "dotnet";

		protected override async Task<DeviceProperties> ProvideOrThrowException()
		{
			DeviceProperties deviceProperties = new DeviceProperties
			{
				OSPlatform = Environment.OSVersion.Platform.ToString(),
				OSVersion = Environment.OSVersion.Version,
				StartupPlatform = "dotnet"
			};
			try
			{
				FillDeviceProperties(deviceProperties);
			}
			catch (Exception)
			{
			}
			try
			{
				FillDisplayProperties(deviceProperties);
			}
			catch (Exception)
			{
			}
			return await Task.FromResult(deviceProperties);
		}

		private static void FillDeviceProperties(DeviceProperties deviceProperties)
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher(new ManagementScope("\\\\.\\ROOT\\cimv2"), new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct")).Get().GetEnumerator())
			{
				if (!managementObjectEnumerator.MoveNext())
				{
					return;
				}
				foreach (PropertyData property in managementObjectEnumerator.Current.Properties)
				{
					string text = ((property.Value == null) ? null : property.Value.ToString());
					text = (string.IsNullOrWhiteSpace(text) ? "unknown" : text);
					switch (property.Name)
					{
					case "Vendor":
						deviceProperties.Manufacturer = text;
						break;
					case "Name":
						deviceProperties.ModelName = text;
						break;
					case "UUID":
					{
						Guid result;
						deviceProperties.Id = (Guid.TryParse(text, out result) ? result.ToString("N") : Identification.GetDeviceId());
						break;
					}
					}
				}
			}
		}

		private static void FillDisplayProperties(DeviceProperties deviceProperties)
		{
			deviceProperties.ScaleFactor = 1f;
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher(new ManagementScope("\\\\.\\ROOT\\cimv2"), new ObjectQuery("SELECT * FROM CIM_VideoController")).Get().GetEnumerator())
				{
					if (managementObjectEnumerator.MoveNext())
					{
						foreach (PropertyData property in managementObjectEnumerator.Current.Properties)
						{
							object value = property.Value;
							if (value == null)
							{
								continue;
							}
							if (property.Name == "CurrentHorizontalResolution")
							{
								deviceProperties.ScreenWidth = Convert.ToUInt32(value);
							}
							else if (property.Name == "CurrentVerticalResolution")
							{
								deviceProperties.ScreenHeight = Convert.ToUInt32(value);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (deviceProperties.ScreenWidth == 0U || deviceProperties.ScreenHeight == 0U)
			{
				try
				{
					int width = GetSystemMetrics(0);
					int height = GetSystemMetrics(1);
					if (width > 0 && height > 0)
					{
						deviceProperties.ScreenWidth = (uint)width;
						deviceProperties.ScreenHeight = (uint)height;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		[DllImport("user32.dll")]
		private static extern int GetSystemMetrics(int systemMetric);
	}
}
