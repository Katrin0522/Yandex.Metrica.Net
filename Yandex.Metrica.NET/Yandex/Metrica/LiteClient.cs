using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;

namespace Yandex.Metrica
{
	internal static class LiteClient
	{
		internal class HttpRequestHeaders
		{
			public const string Accept = "accept";

			public const string ServerDate = "date";

			public const string UserAgent = "user-agent";
		}

		internal class HttpRequestMethods
		{
			public const string Get = "GET";

			public const string Post = "POST";
		}

		public static async Task<HttpResponseMessage> PostAsync(this ReportPackage package)
		{
			Uri uri = new Uri(Config.Global.ReportUrl + "/report?".GlueGetList(new Dictionary<string, object>
			{
				{
					"deviceid",
					Critical.GetDeviceId()
				},
				{
					"uuid",
					Critical.GetUuid()
				}
			}, false) + package.UrlParameters);
			MetricaTrace.Write("REPORT request: " + uri.GetLeftPart(UriPartial.Path) + ", bytes=" + package.Length + ", events=" + package.EventTypes);
			return await PostAsync(uri, new MemoryStream(package.GetRawStream().ToArray()));
		}

		public static async Task<bool> RefreshStartupAsync()
		{
			StartupResponse startupResponse = await GetStartupAsync();
			if (startupResponse == null)
			{
				return Config.Global.ReportUrl != null;
			}
			if (startupResponse.DeviceId != null)
			{
				Critical.SetDeviceId(startupResponse.DeviceId);
			}
			if (startupResponse.Uuid != null)
			{
				Critical.SetUuid(startupResponse.Uuid);
			}
			Config.Global.ReportUrl = startupResponse.ReportUrl;
			return true;
		}

		public static async Task<HttpResponseMessage> PostAsync(Uri uri, Stream stream)
		{
			try
			{
				using (HttpResponseMessage response = await new HttpClient
				{
					DefaultRequestHeaders = { 
					{
						"user-agent",
						ServiceData.UserAgent
					} }
				}.PostAsync(uri, new StreamContent(stream)
				{
					Headers = 
					{
						ContentEncoding = { "gzip" }
					}
				}, CancellationToken.None))
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					MetricaTrace.Write("REPORT response: " + ((int)response.StatusCode) + " " + response.ReasonPhrase + ", body=" + responseBody);
					if (response.IsSuccessStatusCode)
					{
						return response;
					}
					if (responseBody == "Incorrect uuid")
					{
						Critical.SetUuid(null);
					}
					return response;
				}
			}
			catch (Exception exception)
			{
				MetricaTrace.Write("REPORT exception: " + exception);
				return null;
			}
		}

		private static async Task<StartupResponse> GetStartupAsync()
		{
			try
			{
				Uri baseUri = new Uri(Config.Global.CustomStartupUrl ?? "https://startup.mobile.yandex.net/");
				string text = "analytics/startup?query_hosts=1&".GlueGetList(await ServiceData.GetStartupParameters());
				if (Critical.GetUuid() != null)
				{
					text = text + "&uuid=" + Critical.GetUuid();
				}
				text = text + "&deviceid=" + Critical.GetDeviceId();
				Uri requestUri = new Uri(baseUri, text);
				MetricaTrace.Write("STARTUP request: " + requestUri.GetLeftPart(UriPartial.Path));
				using (HttpResponseMessage response = await new HttpClient
				{
					DefaultRequestHeaders = 
					{
						{
							"user-agent",
							ServiceData.UserAgent
						},
						{ "Accept", "application/json" }
					}
				}.GetAsync(requestUri, CancellationToken.None))
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					MetricaTrace.Write("STARTUP response: " + ((int)response.StatusCode) + " " + response.ReasonPhrase + ", body=" + responseBody);
					if (!response.IsSuccessStatusCode)
					{
						return null;
					}
					using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseBody)))
					{
						StartupResponse startupResponse = ((stream == null) ? null : (new DataContractJsonSerializer(typeof(StartupResponse)).ReadObject(stream) as StartupResponse));
						if (startupResponse == null)
						{
							return null;
						}
						startupResponse.ServerDateTime = response.Headers.Date?.DateTime ?? DateTime.UtcNow;
						startupResponse.ServerTimeOffset = startupResponse.ServerDateTime.ToUniversalTime() - DateTime.UtcNow;
						return startupResponse;
					}
				}
			}
			catch (Exception exception)
			{
				MetricaTrace.Write("STARTUP exception: " + exception);
				return null;
			}
		}
	}
}
