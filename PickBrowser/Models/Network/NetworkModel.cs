using PickBrowser.Models.Network.Objects;

namespace PickBrowser.Models.Network; 
internal class NetworkModel {
	public ReactiveCollection<NetworkRequest> RequestList {
		get;
	} = new();

	public NetworkModel() {
		this.RequestList.Add(new("https://dummy.com/"));
	 }
}
