using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

using PickBrowser.Models.Browser;

namespace PickBrowser.ViewModels;
public class BrowserPageViewModel {
	private static int profileCount = 0;
	public WebView2? WebView2 {
		get;
		set;
	}

	private readonly BrowserPageModel _browserModel;

	public ReactiveProperty<string> Url {
		get;
	}

	public ReadOnlyReactiveProperty<string> FaviconUri {
		get;
	}

	public ReadOnlyReactiveProperty<string> PageTitle {
		get;
	}

	public ReadOnlyReactiveProperty<bool> IsBusy {
		get;
	}

	public AsyncReactiveCommand BackCommand {
		get;
	}
	public AsyncReactiveCommand ForwardCommand {
		get;
	}
	public AsyncReactiveCommand<string> NavigateCommand {
		get;
	} = new();

	public AsyncReactiveCommand StopCommand {
		get;
	} = new();

	public AsyncReactiveCommand ReloadCommand {
		get;
	} = new();

	public AsyncReactiveCommand HomeCommand {
		get;
	} = new();

	public AsyncReactiveCommand CloseTab {
		get;
	} = new();

	public BrowserPageViewModel(BrowserPageModel browserModel) {
		this._browserModel = browserModel;
		this.BackCommand = browserModel.CanGoBack.ToAsyncReactiveCommand();
		this.BackCommand.Subscribe(browserModel.Back);
		this.ForwardCommand = browserModel.CanGoForward.ToAsyncReactiveCommand();
		this.ForwardCommand.Subscribe(browserModel.Forward);
		this.NavigateCommand.Subscribe(browserModel.Navigate);
		this.StopCommand.Subscribe(browserModel.Stop);
		this.ReloadCommand.Subscribe(browserModel.Reload);
		this.HomeCommand.Subscribe(browserModel.Home);
		this.Url = browserModel.Url;
		this.FaviconUri = browserModel.FaviconUri.ToReadOnlyReactiveProperty("");
		this.PageTitle = browserModel.PageTitle.ToReadOnlyReactiveProperty("");
		this.IsBusy = browserModel.IsBusy.ToReadOnlyReactiveProperty();
		this.CloseTab.Subscribe(this.Close);
	}

	public async Task CreateWebViewAsync() {
		this.WebView2 = new WebView2() {
			CreationProperties = new CoreWebView2CreationProperties() {
				UserDataFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + profileCount++.ToString() )
			}
		};
		var envOptions = new CoreWebView2EnvironmentOptions {
			AdditionalBrowserArguments = "--proxy-server=http://127.0.0.1:23081"
		};
		var env = await CoreWebView2Environment.CreateAsync(null, null, envOptions);
		var options = env.CreateCoreWebView2ControllerOptions();
		options.IsInPrivateModeEnabled = true;

		await this.WebView2.EnsureCoreWebView2Async(env);

		this._browserModel.SetWebView(this.WebView2);
	}

	public async Task Close() {
		this._browserModel.Close();
		var folder = this.WebView2?.CoreWebView2?.Environment.UserDataFolder;
		var processId = this.WebView2?.CoreWebView2?.BrowserProcessId;
		this.WebView2?.Dispose();
		if (processId != null) {
			var process = Process.GetProcessById((int)processId);
			await process.WaitForExitAsync();
		}
		if (!string.IsNullOrEmpty(folder) && System.IO.Directory.Exists(folder)) {
			System.IO.Directory.Delete(folder, true);
		}
	}
}
