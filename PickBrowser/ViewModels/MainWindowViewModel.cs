namespace PickBrowser.ViewModels;

public class MainWindowViewModel(NetworkViewModel networkViewModel, BrowserViewModel browserViewModel, DownloadViewModel downloadViewModel) {
	public NetworkViewModel NetworkViewModel {
		get;
	} = networkViewModel;
	public BrowserViewModel BrowserViewModel {
		get;
	} = browserViewModel;
	public DownloadViewModel DownloadViewModel {
		get;
	} = downloadViewModel;
}
