using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using PickBrowser.Models.Browser;
using PickBrowser.Models.Download;
using PickBrowser.Models.Network;
using PickBrowser.Services;
using PickBrowser.ViewModels;
using PickBrowser.Views;

using ScrapingLibrary;

namespace PickBrowser;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	public App() {
		Ioc.Default.ConfigureServices(
			new ServiceCollection()
				.AddSingleton<ProxyService>()
				.AddSingleton<DownloadManageService>()
				.AddSingleton<HttpClientWrapper>()
				.AddTransient<MainWindow>()
				.AddTransient<MainWindowViewModel>()
				.AddTransient<NetworkViewModel>()
				.AddTransient<NetworkModel>()
				.AddTransient<BrowserViewModel>()
				.AddTransient<BrowserModel>()
				.AddTransient<DownloadViewModel>()
				.AddTransient<DownloadModel>()
				.BuildServiceProvider()
			);

	}
	protected override void OnStartup(StartupEventArgs e) {
		UIDispatcherScheduler.Initialize();
		this.MainWindow = Ioc.Default.GetService<MainWindow>();
		ReactivePropertyScheduler.SetDefault(UIDispatcherScheduler.Default);
		this.MainWindow?.Show();
		base.OnStartup(e);

		var ps = Ioc.Default.GetService<ProxyService>();
		ps!.Start();
	}
}
