using System.Collections.Generic;

namespace PickBrowser.Extensions {
	/// <summary>
	/// コレクション拡張メソッドクラス
	/// </summary>
	public static class CollectionEx {
		/// <summary>
		/// 複数追加
		/// </summary>
		/// <typeparam name="T">要素型</typeparam>
		/// <param name="source">追加先コレクション</param>
		/// <param name="items">追加するコレクション</param>
		public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items) {
			foreach (var item in items.ToArray()) {
				source.Add(item);
			}
		}

		/// <summary>
		/// 複数追加
		/// </summary>
		/// <typeparam name="T">要素型</typeparam>
		/// <param name="source">追加先コレクション</param>
		/// <param name="items">追加するコレクション</param>
		public static void AddRange<T>(this ICollection<T> source, params T[] items) {
			source.AddRange(items as IEnumerable<T>);
		}
	}
}