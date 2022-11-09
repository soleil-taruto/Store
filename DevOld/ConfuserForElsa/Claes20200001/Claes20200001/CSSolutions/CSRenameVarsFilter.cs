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
		private Dictionary<string, string> OrigName2NameNew = SCommon.CreateDictionary<string>();
		private Dictionary<string, string> NameNew2OrigName = SCommon.CreateDictionary<string>();

		public string GetNameNew(string name)
		{
			string nameNew;

			if (this.OrigName2NameNew.ContainsKey(name))
			{
				nameNew = this.OrigName2NameNew[name];
			}
			else
			{
				nameNew = this.CreateNameNew();

				this.OrigName2NameNew.Add(name, nameNew);
				this.NameNew2OrigName.Add(nameNew, name);
			}
			return nameNew;
		}

		public string CreateNameNew()
		{
			string nameNew;
			int countTry = 0;

			do
			{
				if (1000 < ++countTry)
					throw new Exception("とても運が悪いか、新しい名前をほぼ使い果たしました。");

				nameNew = this.TryCreateNameNew();
			}
			while (this.NameNew2OrigName.ContainsKey(nameNew) || CSResources.予約語リスト.Contains(nameNew));

			return nameNew;
		}

		public IEnumerable<KeyValuePair<string, string>> Get変換テーブル()
		{
			return this.OrigName2NameNew;
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
	}
}
