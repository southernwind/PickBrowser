using System.Net.Http;
using System.Net.Http.Headers;

using Fiddler;

namespace PickBrowser.Services.Objects.Utils;
internal static class HttpClientUtils {
	public static HttpMethod StringToHttpMethod(string method) {
		switch(method) {
			case "GET":
				return HttpMethod.Get;
			case "POST":
				return HttpMethod.Post;
			case "PUT":
				return HttpMethod.Put;
			case "DELETE":
				return HttpMethod.Delete;
		}
		throw new Exception("unknown method");
	}

	public static void SetHttpRequestHeaders(HttpRequestHeaders requestHeaders, HTTPRequestHeaders source) {
		foreach (var header in source) {
			requestHeaders.Add(header.Name, header.Value);
		}
	}
}
