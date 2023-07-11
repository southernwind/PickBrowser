using System.Collections.Generic;
using System.IO;
using System.Xaml;

namespace PickBrowser.Services;
public class ConfigManageService {
	private string? _configFilePath;
	public Config.Config Config {
		get;
	}

	public ConfigManageService(Config.Config config) {
		this.Config = config;
	}

	/// <summary>
	/// ファイルパス設定
	/// </summary>
	/// <param name="path">パス</param>
	public void SetFilePath(string path) {
		this._configFilePath = path;
	}

	/// <summary>
	/// 保存
	/// </summary>
	public void Save() {
		if (this._configFilePath is null) {
			throw new InvalidOperationException();
		}
		using var ms = new MemoryStream();
		var d = this.Config.ConfigList.ToDictionary(x => x.GetType(), x => x.Export());
		XamlServices.Save(ms, d);
		using var fs = File.Create(this._configFilePath);
		ms.WriteTo(fs);
	}

	/// <summary>
	/// 設定ロード
	/// </summary>
	public void Load() {
		if (this._configFilePath is null) {
			throw new InvalidOperationException();
		}
		this.LoadDefault();
		if (!File.Exists(this._configFilePath)) {
			return;
		}

		if (XamlServices.Load(this._configFilePath) is not Dictionary<Type, Dictionary<string, dynamic>> config) {
			return;
		}

		this.Config.Import(config);
	}

	/// <summary>
	/// デフォルト設定ロード
	/// </summary>
	private void LoadDefault() {
		foreach (var c in this.Config.ConfigList) {
			c.LoadDefault();
		}
	}

}
