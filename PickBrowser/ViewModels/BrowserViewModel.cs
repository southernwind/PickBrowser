using Microsoft.Web.WebView2.Wpf;

using PickBrowser.Models.Browser;

using Reactive.Bindings.Extensions;

namespace PickBrowser.ViewModels;
internal class BrowserViewModel {
	private readonly BrowserModel _browserModel;
	public ReactiveProperty<string> Url {
		get;
	}

	public ReadOnlyReactiveProperty<bool> IsBusy {
		get;
	}

	public ReactiveCommand BackCommand {
		get;
	}
	public ReactiveCommand ForwardCommand {
		get;
	}
	public ReactiveCommand<string> NavigateCommand {
		get;
	} = new();

	public ReactiveCommand StopCommand {
		get;
	} = new();

	public ReactiveCommand ReloadCommand {
		get;
	} = new();

	public ReactiveCommand HomeCommand {
		get;
	} = new();

	public BrowserViewModel(BrowserModel browserModel) {
		this._browserModel = browserModel;
		this.BackCommand = browserModel.CanGoBack.ToReactiveCommand();
		this.BackCommand.Subscribe(browserModel.Back);
		this.ForwardCommand = browserModel.CanGoForward.ToReactiveCommand();
		this.ForwardCommand.Subscribe(browserModel.Forward);
		this.NavigateCommand.Subscribe(browserModel.Navigate);
		this.StopCommand.Subscribe(browserModel.Stop);
		this.ReloadCommand.Subscribe(browserModel.Reload);
		this.HomeCommand.Subscribe(browserModel.Home);
		this.Url = browserModel.Url;
		this.IsBusy = browserModel.IsBusy.ToReadOnlyReactiveProperty();
	}

	public void SetWebView(WebView2 webView2) {
		this._browserModel.SetWebView(webView2);
	}
}
