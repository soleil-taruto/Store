using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.CSSolutions;

namespace Charlotte.Tests
{
	public class Test0002
	{
		/// <summary>
		/// 似非英語名の長さの頻度分布
		/// </summary>
		public void Test01()
		{
			CSRenameVarsFilter filter = new CSRenameVarsFilter();
			int[] mapNameLenCount = new int[100];

			for (int testcnt = 0; testcnt < 1000000; testcnt++) // テスト回数
			{
				string name = filter.ForTest_Get似非英語名();

				mapNameLenCount[name.Length]++;
			}
			for (int index = 0; index < mapNameLenCount.Length; index++)
			{
				Console.WriteLine(index.ToString("D3") + " ==> " + mapNameLenCount[index]);
			}
			SCommon.Pause();
		}

		/// <summary>
		/// 使われない単語がどれくらいあるか
		/// </summary>
		public void Test02()
		{
			CSRenameVarsFilter filter = new CSRenameVarsFilter();
			string[][] wordMapList = filter.ForTest_Get似非英語名用単語集リスト();
			int WORD_NUM = wordMapList.Length;
			HashSet<string>[] wordMaps = new HashSet<string>[WORD_NUM];

			for (int index = 0; index < WORD_NUM; index++)
			{
				wordMaps[index] = new HashSet<string>();

				foreach (string word in wordMapList[index])
					wordMaps[index].Add(word);
			}

			// テスト回数
			for (int testcnt = 0; testcnt < 100000; testcnt++) // 全て使われる。
			//for (int testcnt = 0; testcnt < 10000; testcnt++) // ほぼ全て使われる。(稀に少し残る)
			//for (int testcnt = 0; testcnt < 1000; testcnt++) // ほとんど使われる。(0～140程度残る)
			//for (int testcnt = 0; testcnt < 100; testcnt++) // ほとんど残る。
			{
				string name = filter.ForTest_Get似非英語名_本番用();
				string[] words = NameToWords(name);

				if (words.Length != WORD_NUM)
					throw null; // never

				for (int index = 0; index < WORD_NUM; index++)
					if (wordMaps[index].Contains(words[index]))
						wordMaps[index].Remove(words[index]);
			}

			// 結果
			for (int index = 0; index < WORD_NUM; index++)
			{
				Console.WriteLine(wordMaps[index].Count + " / " + wordMapList[index].Length);
			}
			SCommon.Pause();
		}

		private static string[] NameToWords(string name)
		{
			List<string> words = new List<string>();
			int top = 0;

			for (int index = 1; index < name.Length; index++)
			{
				if (char.IsUpper(name[index]))
				{
					words.Add(name.Substring(top, index - top));
					top = index;
				}
			}
			words.Add(name.Substring(top));
			return words.ToArray();
		}
	}
}
