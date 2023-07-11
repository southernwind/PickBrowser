using System.Collections.Generic;

namespace PickBrowser.Services.Config.Objects; 
/// <summary>
/// 設定クラスベースインターフェイス
/// </summary>
public interface IConfigBase {
	/// <summary>
	/// 設定差分出力
	/// </summary>
	/// <returns>
	/// プロパティ名をキーとする設定値Dictionary
	/// </returns>
	Dictionary<string, dynamic> Export();

	/// <summary>
	/// 設定差分読み込み
	/// </summary>
	/// <param name="config">
	/// プロパティ名をキーとする設定値Dictionary
	/// </param>
	void Import(Dictionary<string, dynamic> config);

	/// <summary>
	/// デフォルトロード
	/// </summary>
	void LoadDefault();
}
