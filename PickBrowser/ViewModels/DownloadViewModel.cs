using PickBrowser.Models.Download;
using PickBrowser.Services.Objects;

namespace PickBrowser.ViewModels;
public class DownloadViewModel {
	public ReadOnlyReactiveCollection<DownloadTask> DownloadTasks {
		get;
	}
	public DownloadViewModel(DownloadModel downloadModel) {
		this.DownloadTasks = downloadModel.DownloadTasks;
	}
}
