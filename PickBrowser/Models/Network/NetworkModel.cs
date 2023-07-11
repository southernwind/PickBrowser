using System.Collections.Generic;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services;

using Reactive.Bindings.Helpers;

namespace PickBrowser.Models.Network; 
public class NetworkModel {
	private readonly DownloadManageService _downloadManageService;
	public ReactiveCollection<NetworkRequest> RequestList {
		get;
	} = new();

	public NetworkModel(ProxyService proxyService,DownloadManageService downloadManageService) {
		proxyService
			.AfterSessionComplete
			.Where(x => x != null)
			.Where(x => x!.RequestHeaders.HTTPMethod != "CONNECT")
			.Subscribe(x => {
				lock (this) {
					this.RequestList.Add(new NetworkRequest(x!));
				}
			});
		this._downloadManageService = downloadManageService;
	 }

	public void AddDownloadQueue(IEnumerable<NetworkRequest> networkRequests) {
		this._downloadManageService.AddDownloadQueue(networkRequests);
	}
}
