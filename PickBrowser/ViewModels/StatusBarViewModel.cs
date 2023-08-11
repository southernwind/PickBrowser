using System.Diagnostics;
using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;

using MvvmDialogs;

using PickBrowser.Services;

namespace PickBrowser.ViewModels;

public partial class StatusBarViewModel : ObservableObject {
	public ReactiveCommand OpenConfigWindow {
		get;
	} = new();

	public ReactiveCommand OpenSaveDirectoryCommand {
		get;
	} = new();
	public StatusBarViewModel(ConfigWindowViewModel configWindowViewModel,IDialogService dialogService,ConfigManageService configManageService) {
		this.OpenConfigWindow.Subscribe(() => {
			dialogService.ShowDialog(this, configWindowViewModel);
		});

		this.OpenSaveDirectoryCommand.Subscribe(() => {			
			Process.Start("explorer.exe", Path.GetFullPath(configManageService.Config.GeneralConfig.SaveFolder.Value));
		});
	}
}

