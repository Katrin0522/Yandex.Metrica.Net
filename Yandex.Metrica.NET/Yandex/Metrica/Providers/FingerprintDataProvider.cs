using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
	internal class FingerprintDataProvider : ADataProvider<Task<string>>
	{
		private class DiskSpace
		{
			public ulong? FreeSpace { get; set; }

			public ulong? TotalSpace { get; set; }
		}

		private static async Task<string> GetDeviceFingerprint()
		{
			string publisherApps = GetPublisherApps();
			ulong bootTime = DateTime.UtcNow.ToUnixTime() - (ulong)Environment.TickCount / 1000uL;
			DiskSpace diskSpace = await GetDiskSpace();
			ulong? num = ((diskSpace == null) ? ((ulong?)null) : (diskSpace.TotalSpace / 1024));
			ulong? num2 = ((diskSpace == null) ? ((ulong?)null) : (diskSpace.FreeSpace / 1024));
			return $"{{\n    \"dfid\": {{\n        \"tds\": {num ?? 0},\n        \"fds\": {num2 ?? 0},\n        \"boot_time\": {bootTime},\n        \"apps\": {{\n                \"version\": 0,\n                \"names\": [{publisherApps}]\n                }}\n    }}\n}}";
		}

		private static string GetPublisherApps()
		{
			List<string> list = null;
			try
			{
				list = new List<string>();
			}
			catch
			{
				list = new List<string>();
			}
			if (list.Count <= 0)
			{
				return string.Empty;
			}
			return "\"" + list.Aggregate((string a, string b) => a + "\", \"" + b) + "\"";
		}

		private static async Task<DiskSpace> GetDiskSpace()
		{
			try
			{
				ulong? num = 0uL;
				ulong? num2 = num;
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					if (driveInfo.IsReady && driveInfo.DriveType == DriveType.Fixed)
					{
						num = (ulong?)((long?)num + driveInfo.AvailableFreeSpace);
						num2 = (ulong?)((long?)num2 + driveInfo.TotalSize);
					}
				}
				return new DiskSpace
				{
					TotalSpace = num2,
					FreeSpace = num
				};
			}
			catch (Exception)
			{
				return null;
			}
		}

		protected override Task<string> ProvideOrThrowException()
		{
			return GetDeviceFingerprint();
		}
	}
}
