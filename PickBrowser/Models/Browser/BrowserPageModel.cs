using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Wpf;

using PickBrowser.Services.Config;

namespace PickBrowser.Models.Browser;

public class BrowserPageModel {
	private readonly Config _config;
	private BrowserModel? _parent;
	public WebView2? WebView {
		get;
		private set;
	}

	public ReactiveProperty<string> Url {
		get;
	} = new();

	public ReactiveProperty<string> FaviconUri {
		get;
	} = new();

	public ReactiveProperty<string> PageTitle {
		get;
	} = new();

	public ReactiveProperty<bool> CanGoBack {
		get;
	} = new();
	public ReactiveProperty<bool> CanGoForward {
		get;
	} = new();
	public ReactiveProperty<bool> IsBusy {
		get;
	} = new();

	public BrowserPageModel(Config config) {
		this._config = config;
	}

	public ReactiveProperty<bool> IsInitialized {
		get;
	} = new(false);

#pragma warning disable CS8774 // 終了時にメンバーには null 以外の値が含まれている必要があります。
	[MemberNotNull(nameof(this.WebView))]
	public async Task EnsureInitializedAsync() {
		await this.IsInitialized.Where(x => x).FirstAsync();
	}
#pragma warning restore CS8774 // 終了時にメンバーには null 以外の値が含まれている必要があります。

	[MemberNotNull(nameof(this.WebView))]
	public void SetWebView(WebView2 webView2) {
		this.WebView = webView2;
		webView2.CoreWebView2.HistoryChanged += (_, _) => {
			this.CanGoBack.Value = webView2.CanGoBack;
			this.CanGoForward.Value = webView2.CanGoForward;
		};
		webView2.NavigationStarting += (_,e) => {
			this.IsBusy.Value = true;
			this.Url.Value = e.Uri;
		};
		webView2.NavigationCompleted += (_,_) => {
			this.IsBusy.Value = false;
		};
		webView2.CoreWebView2.NewWindowRequested += async (_, e) => {
			e.Handled = true;
			var newTab = this._parent!.OpenTab();
			await newTab.Navigate(e.Uri);
		};

		webView2.CoreWebView2.DocumentTitleChanged += (_, _) => {
			this.PageTitle.Value = webView2.CoreWebView2.DocumentTitle;
		};
		webView2.CoreWebView2.SourceChanged += (_, _) => {
			this.Url.Value = webView2.CoreWebView2.Source;
		};

		webView2.CoreWebView2.FaviconChanged += (_, _) => {
			this.FaviconUri.Value = webView2.CoreWebView2.FaviconUri;
		};

		this.IsInitialized.Value = true;
	}

	public async Task Navigate(string url) {
		await this.EnsureInitializedAsync();
		try {
			this.WebView.CoreWebView2.Navigate(url);
		} catch (ArgumentException) {

		}
	}

	public async Task Stop() {
		await this.EnsureInitializedAsync();
		this.WebView.Stop();
	}

	public async Task Reload() {
		await this.EnsureInitializedAsync();
		this.WebView.Reload();
	}

	public async Task Back() {
		await this.EnsureInitializedAsync();
		if (this.WebView.CanGoBack) {
			this.WebView.GoBack();
		}
	}
	public async Task Forward() {
		await this.EnsureInitializedAsync();
		if (this.WebView.CanGoForward) {
			this.WebView.GoForward();
		}
	}

	public async Task Home() {
		await this.Navigate(this._config.GeneralConfig.HomeUrl.Value);
	}

	public void SetParent(BrowserModel parent) {
		this._parent = parent;
	}

	public void Close() {
		this._parent!.CloseTab(this);
	}
}
