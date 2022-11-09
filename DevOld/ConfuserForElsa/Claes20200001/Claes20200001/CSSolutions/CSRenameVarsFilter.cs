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
		/// <summary>
		/// テスト用
		/// null == 無効
		/// null 以外 == この文字列を「置き換え禁止ワードのリスト」から除去する。
		/// </summary>
		public static string 置き換え禁止ワードの例外ワード = null;

		/// <summary>
		/// テスト用
		/// </summary>
		/// <returns>置き換え禁止ワードのリスト</returns>
		public string[] Get置き換え禁止ワードのリスト()
		{
			return this.置き換え禁止ワードのリスト;
		}

		private string[] 置き換え禁止ワードのリスト = SCommon.TextToLines(CSResources.予約語リスト + CSConsts.CRLF + CSResources.予約語クラス名リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.Where(v => v != 置き換え禁止ワードの例外ワード) // テスト用
			.ToArray();

		private List<string> 置き換え禁止ワード処理履歴 = new List<string>();
		private int Count置き換え実施 = 0;
		private Dictionary<string, string> 変換テーブル = SCommon.CreateDictionary<string>();

		public string Filter(string name)
		{
			if (
				name == "" ||
				SCommon.DECIMAL.Contains(name[0])
				)
				return name;

			if (this.置き換え禁止ワードのリスト.Contains(name))
			{
				this.置き換え禁止ワード処理履歴.Add(name);
				return name;
			}
			this.Count置き換え実施++;
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

		public void Write置き換え禁止ワード処理履歴(string file)
		{
			Dictionary<string, int> word2Count = SCommon.CreateDictionary<int>();

			foreach (string word in this.置き換え禁止ワード処理履歴)
			{
				if (word2Count.ContainsKey(word))
					word2Count[word]++;
				else
					word2Count.Add(word, 1);
			}
			KeyValuePair<string, int>[] words = word2Count.ToArray();

			Array.Sort(words, (a, b) =>
			{
				int ret = SCommon.Comp(a.Value, b.Value) * -1; // 件数_多い順
				if (ret != 0)
					return ret;

				ret = SCommon.Comp(a.Key.Length, b.Key.Length); // ワード_短い順
				if (ret != 0)
					return ret;

				ret = SCommon.Comp(a.Key, b.Key); // ワード_辞書順
				return ret;
			});

			using (CsvFileWriter writer = new CsvFileWriter(file))
			{
				foreach (KeyValuePair<string, int> word in words)
				{
					writer.WriteCell(word.Key);
					writer.WriteCell(word.Value.ToString());
					writer.EndRow();
				}

				// ついでに出力
				writer.WriteRow(new string[] { "置き換え禁止件数", this.置き換え禁止ワード処理履歴.Count.ToString() });
				writer.WriteRow(new string[] { "置き換え実施件数", this.Count置き換え実施.ToString() });
			}
		}

		private Dictionary<string, object> CNN_Names = SCommon.CreateDictionary<object>();

		public string CreateNameNew()
		{
			string nameNew;
			int countTry = 0;

			do
			{
				if (1000 < ++countTry)
					throw new Exception("想定外のトライ回数 -- 非常に運が悪いか NameNew をほぼ生成し尽くした。");

				nameNew = this.TryCreateNameNew();
			}
			while (this.CNN_Names.ContainsKey(nameNew) || this.置き換え禁止ワードのリスト.Contains(nameNew));

			this.CNN_Names.Add(nameNew, null);
			return nameNew;
		}

		private string[] ランダムな単語リスト = SCommon.TextToLines(CSResources.ランダムな単語リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.Distinct()
			//.Select(v => { if (!Regex.IsMatch(v, "^[A-Z][a-z]*$")) throw new Exception(v); return v; }) // チェック
			.ToArray();

		private string[] 英単語リスト = SCommon.TextToLines(CSResources.英単語リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.Select(v => v.Substring(0, v.IndexOf('\t'))) // 品詞の部分を除去
			.Select(v => v.Substring(0, 1).ToUpper() + v.Substring(1).ToLower()) // 先頭の文字だけ大文字にする。-- 全て小文字のはずなので .ToLower() は不要だけど念の為
			.Distinct()
			//.Select(v => { if (!Regex.IsMatch(v, "^[A-Z][a-z]*$")) throw new Exception(v); return v; }) // チェック
			.ToArray();

		private string[] 英単語リスト_前置詞 = Get英単語リスト("前置詞");
		private string[] 英単語リスト_形容詞 = Get英単語リスト("形容詞");
		private string[] 英単語リスト_代名詞 = Get英単語リスト("代名詞");
		private string[] 英単語リスト_名詞 = Get英単語リスト("名詞");
		private string[] 英単語リスト_副詞 = Get英単語リスト("副詞");
		private string[] 英単語リスト_動詞 = Get英単語リスト("動詞");

		private static string[] Get英単語リスト(string 品詞)
		{
			return SCommon.TextToLines(CSResources.英単語リスト)
				.Select(v => v.Trim())
				.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
				.Where(v => v.Contains(品詞)) // 品詞の絞り込み
				.Select(v => v.Substring(0, v.IndexOf('\t'))) // 品詞の部分を除去
				.Select(v => v.Substring(0, 1).ToUpper() + v.Substring(1).ToLower()) // 先頭の文字だけ大文字にする。-- 全て小文字のはずなので .ToLower() は不要だけど念の為
				.Distinct()
				//.Select(v => { if (!Regex.IsMatch(v, "^[A-Z][a-z]*$")) throw new Exception(v); return v; }) // チェック
				.ToArray();
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
					SCommon.CRandom.ChooseOne(this.英単語リスト_動詞) +
					SCommon.CRandom.ChooseOne(this.英単語リスト_形容詞) +
					SCommon.CRandom.ChooseOne(this.ランダムな単語リスト) +
					SCommon.CRandom.ChooseOne(this.英単語リスト_名詞);

				if (NAME_LEN_MIN <= name.Length && name.Length <= NAME_LEN_MAX)
					return name;
			}
			throw new Exception("非常に運が悪いか、しきい値に問題があります。");
		}

		public string ForTest_Get似非英語名_本番用()
		{
			return this.TryCreateNameNew();
		}

		public string ForTest_Get似非英語名()
		{
			return
				SCommon.CRandom.ChooseOne(this.英単語リスト_動詞) +
				SCommon.CRandom.ChooseOne(this.英単語リスト_形容詞) +
				SCommon.CRandom.ChooseOne(this.ランダムな単語リスト) +
				SCommon.CRandom.ChooseOne(this.英単語リスト_名詞);
		}

		public string[][] ForTest_Get似非英語名用単語集リスト()
		{
			return new string[][]
			{
				this.英単語リスト_動詞,
				this.英単語リスト_形容詞,
				this.ランダムな単語リスト,
				this.英単語リスト_名詞,
			};
		}

		private string[] 予約語クラス名リスト = SCommon.TextToLines(CSResources.予約語クラス名リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // ? 空行ではない && コメント行ではない
			.ToArray();

		public bool Is予約語クラス名(string name)
		{
			return 予約語クラス名リスト.Contains(name);
		}

		public IEnumerable<KeyValuePair<string, string>> Get変換テーブル()
		{
			return this.変換テーブル;
		}
	}
}
