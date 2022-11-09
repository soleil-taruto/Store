using System;
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
		public static string CreateNewIdent()
		{
			// crand 128 bit -> 重複を想定しない。

			return
				"Gattonero_A_" +
				SCommon.CRandom.GetUInt64().ToString("D20") + "_" +
				SCommon.CRandom.GetUInt64().ToString("D20") +
				"_Z";
		}
	}
}
