using PickBrowser.Services;
using PickBrowser.Services.Objects;

namespace PickBrowser.Models.Download; 
public class DownloadModel {
	private readonly DownloadManageService _downloadManageService;
	public ReadOnlyReactiveCollection<DownloadTask> DownloadTasks {
		get;
	}

	public DownloadModel(DownloadManageService downloadManageService) {
		this._downloadManageService = downloadManageService;
		this.DownloadTasks = downloadManageService.DownloadQueue.ToReadOnlyReactiveCollection();
	}
}
