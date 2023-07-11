using System.Collections.Generic;

namespace PickBrowser.Services.Config.Objects {
	/// <summary>
	/// コレクションの設定値アイテム
	/// </summary>
	/// <typeparam name="T">型</typeparam>
	public class ConfigCollection<T> : ReactiveCollection<T>, IConfigItem<IEnumerable<T>> {
		/// <summary>
		/// デフォルト値生成関数
		/// </summary>
		private readonly Func<IEnumerable<T>> _defaultValueCreator;

		/// <summary>
		/// 実際の値
		/// </summary>
		public IEnumerable<T> Value {
			get {
				return this;
			}
		}

		[Obsolete("for serialize")]
		public ConfigCollection() {
			this._defaultValueCreator = null!;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="defaultValue">デフォルト値</param>
		public ConfigCollection(params T[] defaultValue) {
			this._defaultValueCreator = () => defaultValue;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="defaultValue">デフォルト値</param>
		public ConfigCollection(IEnumerable<T> defaultValue) {
			this._defaultValueCreator = () => defaultValue;
		}

		public ConfigCollection(Func<IEnumerable<T>> defaultValueCreator) {
			this._defaultValueCreator = defaultValueCreator;
		}

		/// <summary>
		/// デフォルト値に戻す
		/// </summary>
		public void SetDefaultValue() {
			this.Clear();
			foreach (var item in this._defaultValueCreator()) {
				this.Add(item);
			}
		}

		/// <summary>
		/// デフォルト値との比較
		/// </summary>
		/// <returns>比較結果</returns>
		public bool HasDiff() {
			return !this.Value.SequenceEqual(this._defaultValueCreator());
		}

		/// <summary>
		/// 値の再設定
		/// </summary>
		/// <param name="value">設定する値</param>
		public void SetValue(dynamic value) {
			this.Clear();
			foreach (var item in value) {
				this.Add(item);
			}
		}
	}
}
