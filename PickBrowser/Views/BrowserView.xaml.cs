using System.Windows.Controls;

using Microsoft.Web.WebView2.Core;

using PickBrowser.ViewModels;

namespace PickBrowser.Views;
/// <summary>
/// BrowserView.xaml の相互作用ロジック
/// </summary>
public partial class BrowserView : UserControl {
	public BrowserView() {
		this.InitializeComponent();
		this.DataContextChanged += async (_, _) => {
			if (this.DataContext is not BrowserViewModel bvm) {
				throw new Exception("BrowserViewModel not found");
			}
			var Options = new CoreWebView2EnvironmentOptions();
			Options.AdditionalBrowserArguments = "--proxy-server=http://127.0.0.1:23081";
			var env = await CoreWebView2Environment.CreateAsync(null, null, Options);
			await this.WebView.EnsureCoreWebView2Async(env);
			bvm.SetWebView(this.WebView);
		};
	}
}
