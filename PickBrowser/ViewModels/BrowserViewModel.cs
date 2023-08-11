using System.Reactive.Concurrency;

using PickBrowser.Models.Browser;
using PickBrowser.Services;

namespace PickBrowser.ViewModels;
public class BrowserViewModel {
	public ReadOnlyReactiveCollection<BrowserPageViewModel> Tabs {
		get;
	}

	public ReactiveProperty<BrowserPageViewModel> CurrentTab {
		get;
	} = new();

	public ReactiveCommand OpenTabCommand {
		get;
	} = new();

	public BrowserViewModel(BrowserModel browserModel,ConfigManageService configManageService) {
		this.Tabs = browserModel.Tabs.ToReadOnlyReactiveCollection(x => new BrowserPageViewModel(x, configManageService));
		this.Tabs.ObserveAddChanged().Subscribe(async x => {
			await x.CreateWebViewAsync();
		});

		this.OpenTabCommand.Subscribe(() => {
			this.Tabs.ObserveAddChanged().FirstAsync().Subscribe(x => {
				this.CurrentTab.Value = x;
			});
			var tab = browserModel.OpenTab();
		});
	}
}
