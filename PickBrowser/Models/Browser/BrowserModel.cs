using System.Reactive.Concurrency;

using CommunityToolkit.Mvvm.DependencyInjection;

namespace PickBrowser.Models.Browser;
public class BrowserModel {
	public ReactiveCollection<BrowserPageModel> Tabs {
		get;
	} = new();

	public BrowserModel() {

	}

	public BrowserPageModel OpenTab() {
		var tab = Ioc.Default.GetRequiredService<BrowserPageModel>();
		this.Tabs.Add(tab);
		tab.SetParent(this);
		return tab;
	}

	public void CloseTab(BrowserPageModel tab) {
		this.Tabs.Remove(tab);
	}
}
