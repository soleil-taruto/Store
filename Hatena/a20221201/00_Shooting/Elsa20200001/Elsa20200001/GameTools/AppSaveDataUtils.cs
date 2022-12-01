using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameTools
{
	/// <summary>
	/// このアプリ固有のセーブデータ
	/// </summary>
	public static class AppSaveDataUtils
	{
		/// <summary>
		/// このアプリ固有のセーブデータを返す。
		/// 使用しなくなった項目は削除せず、新しい項目はリストの最後に追加すること。
		/// -- 古いセーブデータを読み込んだとき、判別できるように。
		/// </summary>
		/// <returns>このアプリ固有のセーブデータ</returns>
		public static IEnumerable<string> GetAppLines()
		{
			yield return "" + Ground.I.NovelMessageSpeed;
			yield return "" + Ground.I.CanContinueStageNumber;
		}

		/// <summary>
		/// このアプリ固有のセーブデータを反映する。
		/// リストの長さが足らない場合、例外を投げること。
		/// -- 古いセーブデータはリストが短い場合がある。
		/// ---- 読み込めるところまで正しく読み込み、不足分はデフォルト値のままにしておく。
		/// </summary>
		/// <param name="lines">このアプリ固有のセーブデータ</param>
		public static void UngetAppLines(string[] lines)
		{
			int c = 0;

			Ground.I.NovelMessageSpeed = int.Parse(lines[c++]);
			Ground.I.CanContinueStageNumber = int.Parse(lines[c++]);
		}
	}
}
