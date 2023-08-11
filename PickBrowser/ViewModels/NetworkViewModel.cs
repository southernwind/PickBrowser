using Org.BouncyCastle.Asn1.Ocsp;

using PickBrowser.Models.Network;
using PickBrowser.Models.Network.Objects;

using Reactive.Bindings.Helpers;

namespace PickBrowser.ViewModels;
public class NetworkViewModel {
	public ReadOnlyReactiveCollection<NetworkRequestViewModel> RequestList {
		get;
	}

	public IFilteredReadOnlyObservableCollection<NetworkRequestViewModel> FilteredRequestList {
		get;
	}

	public ReactiveProperty<string> UrlFilter {
		get;
	} = new("");

	public ReactiveProperty<NetworkRequestViewModel> SelectedRequest {
		get;
	} = new();

	public ReactiveCommand<NetworkRequestViewModel> DownloadCommand {
		get;
	} = new();

	public ReactiveCommand ClearHistoryCommand {
		get;
	} = new();

	public ReactiveCommand CheckAllCommand {
		get;
	} = new();

	public ReactiveCommand AllCheckedFileDownloadCommand {
		get;
	} = new();

	public NetworkViewModel(NetworkModel networkModel) {
		this.RequestList =  networkModel.RequestList.ToReadOnlyReactiveCollection(x => new NetworkRequestViewModel(x));
		this.FilteredRequestList = this.RequestList.ToFilteredReadOnlyObservableCollection(this.Filter);
		this.UrlFilter
			.ObserveOnUIDispatcher()
			.Subscribe(urlFilter => {
				this.FilteredRequestList.Refresh(this.Filter);
			});
		this.DownloadCommand.Subscribe(x => networkModel.AddDownloadQueue(new[] { x.NetworkRequest }));
		this.ClearHistoryCommand.Subscribe(networkModel.ClearHistory);
		this.CheckAllCommand.Subscribe(() => {
			var checkedValue = !this.FilteredRequestList.All(x => x.Checked.Value);
			foreach (var vm in this.FilteredRequestList) {
				vm.Checked.Value = checkedValue;
			}
		});
		this.AllCheckedFileDownloadCommand.Subscribe(() => {
			var targets =this.RequestList.Where(x => x.Checked.Value).ToList();
			networkModel.AddDownloadQueue(targets.Select(x => x.NetworkRequest));
			foreach (var t in targets) {
				t.Checked.Value = false;
			}
		});
	}

	private bool  Filter(NetworkRequestViewModel request) {
		return request.RequestUrl.Contains(this.UrlFilter.Value);
	}
}
