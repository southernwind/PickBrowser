using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services.Config;
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
	protected readonly NetworkRequest _networkRequest;
	protected readonly HttpClientWrapper _hc;
	protected readonly ConfigManageService _configManageService;

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

	public ReactiveProperty<string> RenamedName {
		get;
	} = new();

	public AsyncReactiveCommand RetryCommand {
		get;
	} = new();

	public DownloadTask(NetworkRequest networkRequest,HttpClientWrapper hc, ConfigManageService configManageService) {
		this._hc = hc;
		this._configManageService = configManageService;
		this._networkRequest = networkRequest;
		this.Name.Value = networkRequest.RequestUrl;
		if (configManageService.Config.GeneralConfig.RenameRule.Value == RenameRuleEnum.PageTitle) {
			this.RenamedName.Value = networkRequest.DocumentTitle;
		}
		this.RetryCommand.Subscribe(async () => {
			this.Error.Value = null;
			this.HasError.Value = false;
			await this.ExecuteAsync();
		});
	}

	public async Task ExecuteAsync() {
		var extension = Path.GetExtension(Regex.Replace(this._networkRequest.AbsoluteUrl, @"(\?|#).*$", ""));
		try {
			if (extension == ".m3u8") {
				await this.M3u8ExecuteAsync();
			} else {
				var request = new HttpRequestMessage();
				request.Method = HttpClientUtils.StringToHttpMethod(this._networkRequest.Method);
				request.RequestUri = new Uri(this._networkRequest.AbsoluteUrl);
				HttpClientUtils.SetHttpRequestHeaders(request.Headers, this._networkRequest.RequestHeaders);
				if (this._configManageService.Config.GeneralConfig.HttpRangeHeader.Value == HttpRangeHeaderEnum.Remove) {
					request.Headers.Remove("Range");
				}
				var response = await this._hc.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
				if (!response.IsSuccessStatusCode) {
					throw new Exception($"Status Code = {response.StatusCode}");
				}

				using var stream = await response.ToStreamAsync();
				Directory.CreateDirectory(this._configManageService.Config.GeneralConfig.SaveFolder.Value);
				var tempDir = Path.Combine(this._configManageService.Config.GeneralConfig.TempFolder.Value, $"{DateTime.Now.Ticks}-{Thread.CurrentThread.ManagedThreadId}");
				Directory.CreateDirectory(tempDir);
				var path = Path.Combine(tempDir, Path.GetFileName(request.RequestUri.LocalPath.ToString()));
				using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
					var bufferSize = 1024 * 1024;
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

				if (!string.IsNullOrEmpty(this.RenamedName.Value)) {
					this.Rename(path, Path.Combine(this._configManageService.Config.GeneralConfig.SaveFolder.Value, this.RenamedName.Value.FilePathEscape()));
				}
				Directory.Delete(tempDir, true);

				this.IsCompleted.Value = true;

			}
		}catch(Exception ex) {
			this.Error.Value = ex;
			this.HasError.Value = true;
		}
	}

	private async Task M3u8ExecuteAsync() {
		try {
			this.DownloadedFileSize.Value = 0;
			this.FileSize.Value = 0;
			var process = new Process();
			process.StartInfo.FileName = "ffmpeg.exe";

			var tempDir = Path.Combine(this._configManageService.Config.GeneralConfig.TempFolder.Value, $"{DateTime.Now.Ticks}-{Thread.CurrentThread.ManagedThreadId}");
			Directory.CreateDirectory(tempDir);
			var path = Path.Combine(tempDir, Path.GetFileName(this._networkRequest.AbsoluteUrl));
			process.StartInfo.Arguments = $"-protocol_whitelist file,http,https,tcp,tls,crypto -i {this._networkRequest.AbsoluteUrl} -c copy -bsf:a aac_adtstoasc -movflags faststart {path}";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			Console.WriteLine($"{process.StartInfo.FileName} {process.StartInfo.Arguments}");
			process.Start();
			await process.WaitForExitAsync();
			if (process.ExitCode != 0) {
				Console.WriteLine(process.ExitCode);
				throw new Exception($"Exit Code = {process.ExitCode}");
			}

			if (!string.IsNullOrEmpty(this.RenamedName.Value)) {
				this.Rename(path, Path.Combine(this._configManageService.Config.GeneralConfig.SaveFolder.Value, this.RenamedName.Value.FilePathEscape()));
			}
			Directory.Delete(tempDir, true);
			this.ProgressRate.Value = 1;
			this.IsCompleted.Value = true;
		} catch (Exception ex) {
			this.Error.Value = ex;
			this.HasError.Value = true;
		}
	}

	private void Rename(string path, string movedPath,int count=0) {
		try {
			File.Move(path, movedPath);
		}catch(IOException) {
			if (count <= 0) {
				throw;
			}
			Thread.Sleep((int)Math.Pow(count - 10, 2) * 1000);
			this.Rename(path, movedPath, count - 1);
		}

	}
}
