using System.Net.NetworkInformation;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
	internal class NetworkDataProvider : ADataProvider<ReportMessage.Session.Event.NetworkInfo>
	{
		protected override ReportMessage.Session.Event.NetworkInfo ProvideOrThrowException()
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				return null;
			}
			return new ReportMessage.Session.Event.NetworkInfo
			{
				connection_type = ReportMessage.Session.ConnectionType.CONNECTION_WIFI
			};
		}
	}
}
