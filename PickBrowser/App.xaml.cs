using System.IO;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using MvvmDialogs;

using PickBrowser.Models.Browser;
using PickBrowser.Models.Download;
using PickBrowser.Models.Network;
using PickBrowser.Services;
using PickBrowser.Services.Config;
using PickBrowser.ViewModels;
using PickBrowser.Views;

using ScrapingLibrary;

namespace PickBrowser;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	private readonly string _configFilePath;
	public App() {
		var baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
		this._configFilePath = Path.Combine(baseDirectory, "PickBrowser.config");

		Ioc.Default.ConfigureServices(
			new ServiceCollection()
				.AddSingleton<ProxyService>()
				.AddSingleton<DownloadManageService>()
				.AddSingleton<HttpClientWrapper>()
				.AddSingleton<ConfigManageService>()
				.AddSingleton<Config>()
				.AddSingleton<GeneralConfig>()
				.AddSingleton<CertConfig>()
				.AddTransient<MainWindow>()
				.AddTransient<MainWindowViewModel>()
				.AddTransient<NetworkViewModel>()
				.AddTransient<NetworkModel>()
				.AddTransient<BrowserViewModel>()
				.AddTransient<BrowserModel>()
				.AddTransient<BrowserPageModel>()
				.AddTransient<DownloadViewModel>()
				.AddTransient<DownloadModel>()
				.AddTransient<StatusBarViewModel>()
				.AddTransient<ConfigWindowViewModel>()
				.AddSingleton<IDialogService, DialogService>()
				.BuildServiceProvider()
			);

	}
	protected override void OnStartup(StartupEventArgs e) {
		var cms = Ioc.Default.GetRequiredService<ConfigManageService>();
		cms.SetFilePath(this._configFilePath);
		cms.Load();

		UIDispatcherScheduler.Initialize();
		this.MainWindow = Ioc.Default.GetRequiredService<MainWindow>();
		ReactivePropertyScheduler.SetDefault(UIDispatcherScheduler.Default);
		this.MainWindow.Show();
		base.OnStartup(e);

		var ps = Ioc.Default.GetRequiredService<ProxyService>();
		ps.Start();
	}
}
