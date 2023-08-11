using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using MvvmDialogs;

using PickBrowser.Services;
using PickBrowser.Services.Config;

namespace PickBrowser.ViewModels;

public partial class ConfigWindowViewModel: ObservableObject, IModalDialogViewModel {
	private readonly ConfigManageService _configManageService;
	public ReactiveCommand SaveCommand {
		get;
	} = new();

	public ReactiveProperty<HttpRangeHeaderEnum> HttpRangeHeader {
		get;
	} = new();

	public Dictionary<HttpRangeHeaderEnum, string> HttpRangeHeaderCandidate {
		get {
			return new Dictionary<HttpRangeHeaderEnum, string>() {
				{ HttpRangeHeaderEnum.None,"None"},
				{ HttpRangeHeaderEnum.Remove,"Remove" }
			};
		}
	}

	public ReactiveProperty<RenameRuleEnum> RenameRule {
		get;
	} = new();

	public Dictionary<RenameRuleEnum, string> RenameRuleCandidate {
		get {
			return new Dictionary<RenameRuleEnum, string>() {
				{ RenameRuleEnum.None,"None"},
				{ RenameRuleEnum.PageTitle,"PageTitle" }
			};
		}
	}

	public ConfigWindowViewModel(ConfigManageService configManageService, IDialogService dialogService) {
		this._configManageService = configManageService;
		this.Load();

		this.SaveCommand.Subscribe(() => {
			configManageService.Config.GeneralConfig.HttpRangeHeader.Value = this.HttpRangeHeader.Value;
			configManageService.Config.GeneralConfig.RenameRule.Value = this.RenameRule.Value;
			configManageService.Save();
			dialogService.Close(this);
		});
	}

	public void Load() {
		this.HttpRangeHeader.Value = this._configManageService.Config.GeneralConfig.HttpRangeHeader.Value;
		this.RenameRule.Value = this._configManageService.Config.GeneralConfig.RenameRule.Value;
	}

	public bool? DialogResult {
		get;
	} = true;
}
