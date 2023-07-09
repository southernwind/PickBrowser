using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using PickBrowser.Models.Browser;
using PickBrowser.Models.Network;
using PickBrowser.Services;
using PickBrowser.ViewModels;

namespace PickBrowser;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	public App() {
		Ioc.Default.ConfigureServices(
			new ServiceCollection()
				.AddSingleton<ProxyService>()
				.AddTransient<MainWindowViewModel>()
				.AddTransient<NetworkViewModel>()
				.AddTransient<NetworkModel>()
				.AddTransient<BrowserViewModel>()
				.AddTransient<BrowserModel>()
				.BuildServiceProvider()
			);

		this.InitializeComponent();
	}
}
