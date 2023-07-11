using System.Collections.Generic;

namespace PickBrowser.Services.Config.Objects {
	/// <summary>
	/// 単一値の設定アイテム
	/// </summary>
	/// <typeparam name="T">型</typeparam>
	public class ConfigItem<T> : ReactiveProperty<T>, IConfigItem<T> {
		/// <summary>
		/// デフォルト値生成関数
		/// </summary>
		private readonly Func<T> _defaultValueCreator;

		[Obsolete("for serialize")]
		public ConfigItem() {
			this._defaultValueCreator = null!;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="defaultValue">デフォルト値</param>
		public ConfigItem(T defaultValue) {
			this._defaultValueCreator = () => defaultValue;
		}

		/// <summary>
		/// デフォルト値に戻す
		/// </summary>
		public void SetDefaultValue() {
			this.Value = this._defaultValueCreator();
		}

		/// <summary>
		/// デフォルト値との比較
		/// </summary>
		/// <returns>比較結果</returns>
		public bool HasDiff() {
			return !EqualityComparer<T>.Default.Equals(this.Value, this._defaultValueCreator());
		}

		/// <summary>
		/// 値の再設定
		/// </summary>
		/// <param name="value">設定する値</param>
		public void SetValue(dynamic value) {
			this.Value = value;
		}
	}
}
