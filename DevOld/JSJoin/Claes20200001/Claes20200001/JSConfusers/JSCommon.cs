﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.JSConfusers
{
	public static class JSCommon
	{
		/// <summary>
		/// JavaScript-の識別子に使用可能な文字か判定する。
		/// </summary>
		/// <param name="chr">判定する文字</param>
		/// <returns>JavaScript-の識別子に使用可能な文字か</returns>
		public static bool IsJSWordChar(char chr)
		{
			return
				SCommon.ALPHA.Contains(chr) ||
				SCommon.alpha.Contains(chr) ||
				SCommon.DECIMAL.Contains(chr) ||
				chr == '$' ||
				chr == '_' ||
				0x100 <= (uint)chr; // ? 日本語
		}

		/// <summary>
		/// 新しい識別子を生成する。
		/// 重複を考慮しなくて良いランダムな文字列を返す。
		/// </summary>
		/// <returns>新しい識別子</returns>
		public static string CreateNewIdent(Predicate<string> accept)
		{
			for (int c = 0; ; c++)
			{
				string name = TryCreateNameNew();

				if (accept(name))
					return name;

				if (1000 < c) // rough limit
					throw new Exception("識別子をほぼ使い果たしました。");
			}
		}

		/// <summary>
		/// 新しい識別子を作成する。
		/// 標準のクラス名 List, StringBuilder などと被らない名前を返すこと。
		/// -- 今の実装は厳密にこれを回避していない。@ 2020.11.x
		/// </summary>
		/// <returns>新しい識別子</returns>
		private static string TryCreateNameNew()
		{
			return
				SCommon.CRandom.ChooseOne(JSResource.英単語リスト_動詞) +
				SCommon.CRandom.ChooseOne(JSResource.英単語リスト_形容詞) +
				SCommon.CRandom.ChooseOne(JSResource.ランダムな単語リスト) +
				SCommon.CRandom.ChooseOne(JSResource.英単語リスト_名詞);
		}
	}
}
