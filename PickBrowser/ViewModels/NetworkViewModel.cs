using PickBrowser.Models.Network;
using PickBrowser.Models.Network.Objects;

using Reactive.Bindings.Helpers;

namespace PickBrowser.ViewModels;
public class NetworkViewModel {
	public ReadOnlyReactiveCollection<NetworkRequest> RequestList {
		get;
	}

	public IFilteredReadOnlyObservableCollection<NetworkRequest> FilteredRequestList {
		get;
	}

	public ReactiveProperty<string> UrlFilter {
		get;
	} = new("");

	public ReactiveProperty<NetworkRequest> SelectedRequest {
		get;
	} = new();

	public ReactiveCommand<NetworkRequest> DownloadCommand {
		get;
	} = new();

	public NetworkViewModel(NetworkModel networkModel) {
		this.RequestList =  networkModel.RequestList.ToReadOnlyReactiveCollection();
		this.FilteredRequestList = this.RequestList.ToFilteredReadOnlyObservableCollection(this.Filter);
		this.UrlFilter
			.ObserveOnUIDispatcher()
			.Subscribe(urlFilter => {
				this.FilteredRequestList.Refresh(this.Filter);
			});
		this.DownloadCommand.Subscribe(x => networkModel.AddDownloadQueue(new[] { x }));
	}

	private bool  Filter(NetworkRequest request) {
		return request.RequestUrl.Contains(this.UrlFilter.Value);
	}
}
