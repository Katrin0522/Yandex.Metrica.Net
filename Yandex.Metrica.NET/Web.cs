using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;

public static class Web
{
	public static async Task<string> Request(string address, string method = "GET", object content = null, string contentType = "application/json")
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
		request.Method = method;
		if (content != null)
		{
			request.ContentType = contentType;
			using (Stream stream = await Task.Factory.FromAsync((Func<AsyncCallback, object, IAsyncResult>)request.BeginGetRequestStream, (Func<IAsyncResult, Stream>)request.EndGetRequestStream, (object)null))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(content.ToJson());
				stream.Write(bytes, 0, bytes.Length);
			}
		}
		using (HttpWebResponse httpWebResponse = (HttpWebResponse)(await Task.Factory.FromAsync((Func<AsyncCallback, object, IAsyncResult>)request.BeginGetResponse, (Func<IAsyncResult, WebResponse>)request.EndGetResponse, (object)null)))
		{
			using (Stream stream2 = httpWebResponse.GetResponseStream())
			{
				using (StreamReader streamReader = new StreamReader(stream2 ?? new MemoryStream(), Encoding.UTF8))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
	}
}
