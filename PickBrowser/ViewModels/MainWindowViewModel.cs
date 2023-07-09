namespace PickBrowser.ViewModels;

internal class MainWindowViewModel {
	public NetworkViewModel NetworkViewModel {
		get;
	}
	public BrowserViewModel BrowserViewModel{
		get;
	}

	public MainWindowViewModel(NetworkViewModel networkViewModel, BrowserViewModel browserViewModel) {
		this.NetworkViewModel = networkViewModel;
		this.BrowserViewModel = browserViewModel;
	}
}
