using PickBrowser.Services.Config.Objects;

namespace PickBrowser.Services.Config;
public class CertConfig :ConfigBase{
	public ConfigItem<string?> Cert {
		get;
	} = new(null);

	public ConfigItem<string?> Key {
		get;
	} = new(null);


	public CertConfig() {
	}
}
