using PickBrowser.Services.Config.Objects;

namespace PickBrowser.Services.Config;

public enum HttpRangeHeaderEnum {
	None,
	Remove
}
public enum RenameRuleEnum {
	None,
	PageTitle
}
public class GeneralConfig : ConfigBase {
	public ConfigItem<string> HomeUrl {
		get;
	} = new("about:blank");

	public ConfigItem<ushort> Port {
		get;
	} = new(23081);

	public ConfigItem<string> SaveFolder {
		get;
	} = new("Save");
	public ConfigItem<string> TempFolder {
		get;
	} = new("Temp");

	public ConfigItem<HttpRangeHeaderEnum> HttpRangeHeader {
		get;
	} = new(HttpRangeHeaderEnum.Remove);
	public ConfigItem<RenameRuleEnum> RenameRule {
		get;
	} = new(RenameRuleEnum.None);

	public ConfigItem<bool> ClearNetworkWhenNavigationStarting {
		get;
	} = new(false);

	public GeneralConfig() {
	}
}
