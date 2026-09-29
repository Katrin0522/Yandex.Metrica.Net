using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aero.Specific;
using Yandex.Metrica.Models;

namespace Yandex.Metrica.Legacy
{
	internal static class LegacyManager
	{
		[DataContract]
		internal class MigrationData
		{
			[DataMember]
			public bool MigratedFromVersion2 { get; set; }

			[DataMember]
			public bool IsCleaned { get; set; }
		}

		private const string LegacyConfigFileName = "Yandex.Metrica.Config";

		private static readonly string[] LegacyKeys = new string[2] { "Yandex.Metrica.Config", "Yandex.Metrica.Sessions" };

		public static MigrationData Data { get; private set; }

		public static async Task CompleteMigration()
		{
			if (Memory.ActiveBox.Check<MigrationData>())
			{
				Data = Memory.ActiveBox.Revive<MigrationData>(null, new object[0]);
			}
			else
			{
				try
				{
					Config config = await new IsolatedStreamStorage<Config>(new ConfigProtoSerializer()).ReadAsync("Yandex.Metrica.Config");
					Guid item = new Guid(config.ApiKey);
					if (!Yandex.Metrica.Models.Config.Global.KnownKeys.Contains(item))
					{
						Yandex.Metrica.Models.Config.Global.KnownKeys.Add(item);
					}
					Critical.SetUuid(config.UUID);
					Memory.ActiveBox.Keep(Yandex.Metrica.Models.Config.Global);
					Yandex.Metrica.Models.Config.Global.CrashTracking = config.ReportCrashesEnabled;
					Yandex.Metrica.Models.Config.Global.LocationTracking = config.TrackLocationEnabled;
					Yandex.Metrica.Models.Config.Global.CustomAppVersion = new Version(config.CustomAppVersion);
				}
				catch (Exception)
				{
				}
				Memory memory = new Memory(new KeyFileStorage(), "{0}");
				bool flag = LegacyKeys.Any((string k) => memory.Check<object>(k));
				Data = new MigrationData
				{
					MigratedFromVersion2 = flag,
					IsCleaned = !flag
				};
				Memory.ActiveBox.Keep(Data);
			}
			if (!Data.IsCleaned)
			{
				Data.IsCleaned = Clean();
				if (Data.IsCleaned)
				{
					Memory.ActiveBox.Keep(Data);
				}
			}
		}

		private static bool Clean()
		{
			try
			{
				Memory memory = new Memory(new KeyFileStorage(), "{0}");
				LegacyKeys.Where((string key) => memory.Check<object>(key)).ForEach((string k) =>
				{
					memory.Destroy<object>(k);
				});
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
