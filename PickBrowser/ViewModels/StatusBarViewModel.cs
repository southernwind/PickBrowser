using CommunityToolkit.Mvvm.ComponentModel;

using MvvmDialogs;

namespace PickBrowser.ViewModels;

public partial class StatusBarViewModel : ObservableObject {
	public ReactiveCommand OpenConfigWindow {
		get;
	} = new();

	public StatusBarViewModel(ConfigWindowViewModel configWindowViewModel,IDialogService dialogService) {
		this.OpenConfigWindow.Subscribe(() => {
			dialogService.ShowDialog(this, configWindowViewModel);
		});
	}
}

