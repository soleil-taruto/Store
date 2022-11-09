using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		/// <summary>
		/// パス文字列を比較する。
		/// 同じフォルダ内のローカル名が辞書順になるようにする。
		/// </summary>
		/// <param name="a">パス文字列_A</param>
		/// <param name="b">パス文字列_B</param>
		/// <returns>比較結果</returns>
		public static int CompPath(string a, string b)
		{
			a = ConvForCompPath(a);
			b = ConvForCompPath(b);

			return SCommon.CompIgnoreCase(a, b);
		}

		private static string ConvForCompPath(string str)
		{
			string[] tokens = str.Split('\\');

			for (int index = 0; index < tokens.Length; index++)
				tokens[index] = (index + 1 < tokens.Length ? "2_" : "1_") + tokens[index];

			str = string.Join("\t", tokens);
			return str;
		}

		/// <summary>
		/// ランダムな識別子を生成する。
		/// 重複を考慮しなくて良いランダムな文字列を返す。
		/// </summary>
		/// <returns>新しい識別子</returns>
		public static string CreateRandIdent()
		{
			return
				"A_" +
				SCommon.CRandom.GetULong().ToString("D20") + "_" +
				SCommon.CRandom.GetULong().ToString("D20") +
				"_Z";
		}

		/// <summary>
		/// リソースファイル(テキストファイル)を読み込む
		/// </summary>
		/// <param name="localName">リソースファイルのローカル名</param>
		/// <returns>リソースファイルの内容</returns>
		public static string ReadResTextFile(string localName)
		{
			string resFile = Path.Combine(ProcMain.SelfDir, localName);

			if (!File.Exists(resFile))
			{
				resFile = Path.Combine(@"..\..\..\..\doc", localName);

				if (!File.Exists(resFile))
					throw new Exception("リソースファイル \"" + localName + "\" が存在しません。");
			}
			return File.ReadAllText(resFile, SCommon.ENCODING_SJIS);
		}

		/// <summary>
		/// string.Replace(oldValue, valueNew) と同じ
		/// 但し、valueNew は毎回 getValueNew を実行した戻り値を使用する。
		/// </summary>
		/// <param name="text">処理前のテキスト</param>
		/// <param name="oldValue">置き換え前のパターン</param>
		/// <param name="getValueNew">置き換え後のパターンの取得先</param>
		/// <returns>処理後のテキスト</returns>
		public static string Replace(string text, string oldValue, Func<string> getValueNew)
		{
			List<string> dest = new List<string>();

			for (; ; )
			{
				string[] slnd = SCommon.ParseIsland(text, oldValue);

				if (slnd == null)
					break;

				dest.Add(slnd[0]);
				dest.Add(getValueNew());
				text = slnd[2];
			}
			dest.Add(text);
			return string.Join("", dest);
		}

		public static char HexCharsToUnicodeChar(params char[] hexChrs)
		{
			int value = 0;

			foreach (char hexChr in hexChrs)
			{
				int hexVal = SCommon.hexadecimal.IndexOf(char.ToLower(hexChr));

				if (hexVal == -1)
					throw new Exception("Bad hex-char: " + (int)hexChr);

				value <<= 4;
				value |= hexVal;
			}
			return (char)value;
		}
	}
}
