using System.Collections.Generic;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services;

namespace PickBrowser.Models.Network;
public class NetworkModel {
	private readonly DownloadManageService _downloadManageService;
	public ReactiveCollection<NetworkRequest> RequestList {
		get;
	} = new();

	public NetworkModel(ProxyService proxyService,DownloadManageService downloadManageService) {
		proxyService
			.ResponseHeadersAvailable
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

	public void ClearHistory() {
		this.RequestList.Clear();
	}
}
