using System.Reactive.Concurrency;

using PickBrowser.Models.Browser;

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

	public BrowserViewModel(BrowserModel browserModel) {
		this.Tabs = browserModel.Tabs.ToReadOnlyReactiveCollection(x => new BrowserPageViewModel(x));
		this.Tabs.ObserveAddChanged().Subscribe(async x => {
			await x.CreateWebViewAsync();
		});

		this.OpenTabCommand.Subscribe(async () => {
			var tab = browserModel.OpenTab();
			await tab.EnsureInitializedAsync();
			this.CurrentTab.Value = this.Tabs.Last();
		});
	}
}
