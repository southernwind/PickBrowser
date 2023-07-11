using System.Collections.Generic;

using Fiddler;

using PickBrowser.Services.Config.Objects;

namespace PickBrowser.Services.Config;
public class Config {
	public GeneralConfig GeneralConfig {
		get;
	}

	public CertConfig CertConfig {
		get;
	}

	public ConfigBase[] ConfigList {
		get {
			return new ConfigBase[] { this.GeneralConfig, this.CertConfig };
		}
	}

	public Config(GeneralConfig generalConfig, CertConfig certConfig) {
		this.GeneralConfig = generalConfig;
		this.CertConfig = certConfig;
	}

	public void Import(Dictionary<Type, Dictionary<string, dynamic>> config) {
		foreach (var c in this.ConfigList) {
			if (config.TryGetValue(c.GetType(), out var d)) {
				c.Import(d);
			}
		}
	}
}
