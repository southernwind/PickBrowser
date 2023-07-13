using System.Collections.Generic;
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

	public ReactiveProperty<long> FileSize {
		get;
	} = new();

	public ReactiveProperty<long> DownloadedFileSize {
		get;
	} = new();


	public ReactiveProperty<double> ProgressRate {
		get;
	} = new();

	public ReactiveProperty<TaskPriority> Priority {
		get;
	} = new();

	public ReactiveProperty<bool> HasError {
		get;
	} = new();

	public ReactiveProperty<Exception?> Error {
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
		var response = await this._hc.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
		if (!response.IsSuccessStatusCode) {
			this.HasError.Value = true;
			this.Error.Value = new Exception($"Status Code = {response.StatusCode}");
		}

		using var stream = await response.ToStreamAsync();
		Directory.CreateDirectory("Save");
		var path = Path.Combine("Save", Path.GetFileName(request.RequestUri.LocalPath.ToString()));
		using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
		var bufferSize = 1024;
		var buffer = new byte[bufferSize];
		var fileSize = (long)response.Content.Headers.ContentLength!;
		this.FileSize.Value = fileSize;
		var completed = 0d;
		while (true) {
			var t = await stream.ReadAsync(buffer.AsMemory(0, bufferSize));
			if (t == 0) {
				break;
			}
			completed += t;
			this.ProgressRate.Value = completed / fileSize;
			this.DownloadedFileSize.Value = (long)completed;
			await fileStream.WriteAsync(buffer.AsMemory(0, t));
		}
	}
}
