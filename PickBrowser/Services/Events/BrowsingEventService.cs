using System.Reactive;
using System.Reactive.Subjects;

using Microsoft.Web.WebView2.Core;

namespace PickBrowser.Services.Events;
public class BrowsingEventService {
	public Subject<CoreWebView2NavigationStartingEventArgs> NavigationStarting {
		get;
	} = new();

	public ReactiveProperty<string> CurrentDocumentTitle {
		get;
	} = new();

	public BrowsingEventService() {
	}
}
