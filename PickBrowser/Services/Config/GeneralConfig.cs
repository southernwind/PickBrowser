using PickBrowser.Services.Config.Objects;

namespace PickBrowser.Services.Config;
public class GeneralConfig :ConfigBase{
	public ConfigItem<string> HomeUrl {
		get;
	} = new("about:blank");

	public GeneralConfig() {
	}
}
