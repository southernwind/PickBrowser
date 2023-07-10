using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Wpf;

using PickBrowser.Services;

namespace PickBrowser.Models.Browser;

internal class BrowserModel {
	public WebView2? WebView {
		get;
		private set;
	}

	public ReactiveProperty<string> Url {
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

	public BrowserModel() {
	}

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
		webView2.NavigationCompleted += (_, _) => {
			this.IsBusy.Value = false;
		};
	}

	public void Navigate(string url) {
		this.WebView?.CoreWebView2.Navigate(url);
	}

	public void Stop() {
		this.WebView?.Stop();
	}

	public void Reload() {
		this.WebView?.Reload();
	}

	public void Back() {
		if (this.WebView?.CanGoBack ?? false) {
			this.WebView.GoBack();
		}
	}
	public void Forward() {
		if (this.WebView?.CanGoForward ?? false) {
			this.WebView.GoForward();
		}
	}

	public void Home() {
		this.Navigate("https://www.google.com/");
	}
}
