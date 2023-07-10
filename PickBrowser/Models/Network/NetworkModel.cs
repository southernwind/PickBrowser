using System.Reactive.Linq;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services;

namespace PickBrowser.Models.Network; 
internal class NetworkModel {
	public ReactiveCollection<NetworkRequest> RequestList {
		get;
	} = new();

	public NetworkModel(ProxyService proxyService) {
		proxyService
			.AfterSessionComplete
			.Where(x => x != null)
			.Subscribe(x => {
				this.RequestList.Add(new NetworkRequest(x!.url));
			});
	 }
}
