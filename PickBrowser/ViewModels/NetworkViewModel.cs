using PickBrowser.Models.Network;
using PickBrowser.Models.Network.Objects;

namespace PickBrowser.ViewModels;
internal class NetworkViewModel {
	public ReadOnlyReactiveCollection<NetworkRequest> RequestList {
		get;
	}
	public NetworkViewModel(NetworkModel networkModel) {
		this.RequestList = networkModel.RequestList.ToReadOnlyReactiveCollection();
	}
}
