using System.Collections.Generic;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services;
using PickBrowser.Services.Events;

namespace PickBrowser.Models.Network;
public class NetworkModel {
	private readonly DownloadManageService _downloadManageService;
	public ReactiveCollection<NetworkRequest> RequestList {
		get;
	} = new();

	public NetworkModel(
		ProxyService proxyService,
		DownloadManageService downloadManageService,
		ConfigManageService configManageService,
		BrowsingEventService browsingEventService) {
		proxyService
			.RequestHeadersAvailable
			.Where(x => x != null)
			.Where(x => x!.RequestHeaders.HTTPMethod != "CONNECT")
			.Subscribe(x => {
				lock (this) {
					this.RequestList.Add(new NetworkRequest(browsingEventService.CurrentDocumentTitle.Value, x!));
				}
			});

		proxyService
			.ResponseHeadersAvailable
			.Where(x => x != null)
			.Where(x => x!.RequestHeaders.HTTPMethod != "CONNECT")
			.Subscribe(x => {
				var nr = this.RequestList.Where(r => r.SessionId == x!.id).FirstOrDefault();
				if (nr != null) {
					nr.Update(x!);
					return;
				}
				this.RequestList.Add(new NetworkRequest(browsingEventService.CurrentDocumentTitle.Value, x!));
			});

		this._downloadManageService = downloadManageService;
		browsingEventService.NavigationStarting.Subscribe(x => {
			if (configManageService.Config.GeneralConfig.ClearNetworkWhenNavigationStarting.Value) {
				this.ClearHistory();
			}
		});
	 }

	public void AddDownloadQueue(IEnumerable<NetworkRequest> networkRequests) {
		this._downloadManageService.AddDownloadQueue(networkRequests);
	}

	public void ClearHistory() {
		this.RequestList.Clear();
	}
}
