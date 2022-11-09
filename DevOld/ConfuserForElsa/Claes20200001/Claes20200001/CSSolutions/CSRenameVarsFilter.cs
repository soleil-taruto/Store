using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Charlotte.Commons;

namespace Charlotte.CSSolutions
{
	public class CSRenameVarsFilter
	{
		private Dictionary<string, string> 変換テーブル = SCommon.CreateDictionary<string>();

		public string Filter(string name)
		{
			if (
				name == "" ||
				SCommon.DECIMAL.Contains(name[0]) ||
				CSResources.予約語と予約語クラス名のリスト.Contains(name)
				)
				return name;

			string nameNew;

			if (this.変換テーブル.ContainsKey(name))
			{
				nameNew = this.変換テーブル[name];
			}
			else
			{
				nameNew = this.CreateNameNew();
				this.変換テーブル.Add(name, nameNew);
			}
			return nameNew;
		}

		private Dictionary<string, object> CNN_Names = SCommon.CreateDictionary<object>();

		public string CreateNameNew()
		{
			string nameNew;
			int countTry = 0;

			do
			{
				if (1000 < ++countTry)
					throw new Exception("とても運が悪いか、新しい名前をほぼ生成し尽くしました。");

				nameNew = this.TryCreateNameNew();
			}
			while (this.CNN_Names.ContainsKey(nameNew) || CSResources.予約語と予約語クラス名のリスト.Contains(nameNew));

			this.CNN_Names.Add(nameNew, null);
			return nameNew;
		}

		/// <summary>
		/// 新しい識別子を作成する。
		/// 標準のクラス名 List, StringBuilder などと被らない名前を返すこと。
		/// -- 今の実装は厳密にこれを回避していない。@ 2020.11.x
		/// </summary>
		/// <returns>新しい識別子</returns>
		private string TryCreateNameNew()
		{
			// Test0002.Test01 より、頻度の高い範囲 @ 2022.2.27
			//
			const int NAME_LEN_MIN = 25;
			const int NAME_LEN_MAX = 32;

			// Test0002.Test01 より、頻度の最も高い長さ @ 2022.2.27
			//
			//const int NAME_LEN_MIN = 28;
			//const int NAME_LEN_MAX = 28;

			for (int c = 0; c < 1000; c++) // 十分なトライ回数 -- rough limit
			{
				// 似非英語名
				string name =
					SCommon.CRandom.ChooseOne(CSResources.英単語リスト_動詞) +
					SCommon.CRandom.ChooseOne(CSResources.英単語リスト_形容詞) +
					SCommon.CRandom.ChooseOne(CSResources.ランダムな単語リスト) +
					SCommon.CRandom.ChooseOne(CSResources.英単語リスト_名詞);

				if (NAME_LEN_MIN <= name.Length && name.Length <= NAME_LEN_MAX)
					return name;
			}
			throw new Exception("とても運が悪いか、しきい値に問題があります。");
		}

		public string ForTest_Get似非英語名_本番用()
		{
			return this.TryCreateNameNew();
		}

		public string ForTest_Get似非英語名()
		{
			return
				SCommon.CRandom.ChooseOne(CSResources.英単語リスト_動詞) +
				SCommon.CRandom.ChooseOne(CSResources.英単語リスト_形容詞) +
				SCommon.CRandom.ChooseOne(CSResources.ランダムな単語リスト) +
				SCommon.CRandom.ChooseOne(CSResources.英単語リスト_名詞);
		}

		public string[][] ForTest_Get似非英語名用単語集リスト()
		{
			return new string[][]
			{
				CSResources.英単語リスト_動詞,
				CSResources.英単語リスト_形容詞,
				CSResources.ランダムな単語リスト,
				CSResources.英単語リスト_名詞,
			};
		}

		public bool Is予約語クラス名(string name)
		{
			return CSResources.予約語クラス名リスト.Contains(name);
		}

		public IEnumerable<KeyValuePair<string, string>> Get変換テーブル()
		{
			return this.変換テーブル;
		}
	}
}
