namespace PickBrowser.ViewModels;

public class MainWindowViewModel(NetworkViewModel networkViewModel, BrowserViewModel browserViewModel, DownloadViewModel downloadViewModel, StatusBarViewModel statusBarViewModel) {
	public NetworkViewModel NetworkViewModel {
		get;
	} = networkViewModel;
	public BrowserViewModel BrowserViewModel {
		get;
	} = browserViewModel;
	public DownloadViewModel DownloadViewModel {
		get;
	} = downloadViewModel;
	public StatusBarViewModel StatusBarViewModel {
		get;
	} = statusBarViewModel;
}
