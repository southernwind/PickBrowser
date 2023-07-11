using CommunityToolkit.Mvvm.ComponentModel;

using MvvmDialogs;

namespace PickBrowser.ViewModels;

public partial class ConfigWindowViewModel: ObservableObject, IModalDialogViewModel {
	public ConfigWindowViewModel() {
	}

	public bool? DialogResult {
		get;
	}
}
