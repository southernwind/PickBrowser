using System.Collections.Generic;
using System.Reflection;

namespace PickBrowser.Services.Config.Objects {

	/// <summary>
	/// 設定基底クラス
	/// </summary>
	public class ConfigBase : IConfigBase {
		/// <summary>
		/// 設定差分出力
		/// </summary>
		/// <returns>
		/// プロパティ名をキーとする設定値Dictionary
		/// </returns>
		public Dictionary<string, dynamic> Export() {
			return this.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.PropertyType.GetInterfaces().Any(t => t == typeof(IConfigItem)))
				.Select(x => (x.Name, ConfigItem: (x.GetValue(this) as IConfigItem)!))
				.Where(x => x.ConfigItem.HasDiff())
				.ToDictionary(x => x.Name, x => ((dynamic)x.ConfigItem).Value);
		}

		/// <summary>
		/// 設定差分読み込み
		/// </summary>
		/// <param name="config">
		/// プロパティ名をキーとする設定値Dictionary
		/// </param>
		public void Import(Dictionary<string, dynamic> config) {
			foreach (var ci in this.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.PropertyType.GetInterfaces().Any(t => t == typeof(IConfigItem)))
				.Select(x => (x.Name, ConfigItem: (x.GetValue(this) as IConfigItem)!))) {
				if (config.TryGetValue(ci.Name, out var value)) {
					ci.ConfigItem.SetValue(value);
				}
			}
		}

		/// <summary>
		/// デフォルトロード
		/// </summary>
		public void LoadDefault() {
			foreach (var ci in this.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.PropertyType.GetInterfaces().Any(t => t == typeof(IConfigItem)))
				.Select(x => (x.Name, ConfigItem: (x.GetValue(this) as IConfigItem)!))) {
				ci.ConfigItem.SetDefaultValue();
			}
		}
	}
}
