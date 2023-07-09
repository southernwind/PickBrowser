using System.Windows.Controls;

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
			await this.WebView.EnsureCoreWebView2Async();
			bvm.SetWebView(this.WebView);
		};
	}
}
