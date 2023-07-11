using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services.Objects.Utils;

using ScrapingLibrary;

namespace PickBrowser.Services.Objects;
public enum TaskPriority {
	Highest,
	VeryHigh,
	High,
	AboveNormal,
	Normal,
	BelowNormal,
	Low,
	VeryLow,
	Lowest
}
public class DownloadTask {
	private readonly NetworkRequest _networkRequest;
	private readonly HttpClientWrapper _hc;
	public ReactiveProperty<string> Name {
		get;
	} = new();

	public ReactiveProperty<TaskPriority> Priority {
		get;
	} = new();

	public ReactiveProperty<bool> HasError {
		get;
	} = new();

	public ReactiveProperty<bool> IsCompleted {
		get;
	} = new();

	public DownloadTask(NetworkRequest networkRequest,HttpClientWrapper hc) {
		this._hc = hc;
		this._networkRequest = networkRequest;
		this.Name.Value = networkRequest.RequestUrl;
	}

	public async Task ExecuteAsync() {
		var request = new HttpRequestMessage();
		request.Method = HttpClientUtils.StringToHttpMethod(this._networkRequest.Method);
		request.RequestUri = new Uri(this._networkRequest.AbsoluteUrl);
		HttpClientUtils.SetHttpRequestHeaders(request.Headers, this._networkRequest.RequestHeaders);
		var response = await this._hc.SendAsync(request);
		Directory.CreateDirectory("Save");
		await File.WriteAllBytesAsync(Path.Combine("Save", Path.GetFileName(request.RequestUri.ToString())), await response.ToBinaryAsync());
	}
}
