using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.JSChecks
{
	public static class JSCommon
	{
		public static bool IsJSWordChar(char chr)
		{
			return
				('0' <= chr && chr <= '9') ||
				('A' <= chr && chr <= 'Z') ||
				('a' <= chr && chr <= 'z') ||
				chr == '$' ||
				chr == '_' ||
				IsJChar(chr);
		}

		private static BitList JChars = null;

		public static bool IsJChar(char chr)
		{
			if (JChars == null)
				JChars = P_GetJChars();

			return JChars[(long)chr];
		}

		private static BitList P_GetJChars()
		{
			BitList dest = new BitList();

			foreach (char chr in SCommon.GetJChars())
				dest[(long)chr] = true;

			return dest;
		}

		public static string RemoveLiteralString(string line)
		{
			for (int index = 0; index < line.Length; index++)
			{
				if (line[index] == '\'' || line[index] == '"') // ? 文字列の開始
				{
					int start = index; // 文字列の開始_引用符の位置

					for (; ; )
					{
						index++;

						if (line.Length <= index)
							throw new Exception("文字列の途中で行が終わりました。");

						if (line[index] == '\\')
						{
							index++;
						}
						else
						{
							if (
								line[index] == '\'' ||
								line[index] == '"'
								)
								break;
						}
					}
					int end = index + 1; // 文字列の終了_引用符の次の位置

					line =
						line.Substring(0, start) +
						MaskLiteralString(line.Substring(start, end - start)) +
						line.Substring(end);
				}
			}
			return line;
		}

		private static string MaskLiteralString(string str)
		{
			return new string(Enumerable.Repeat(' ', str.Length).ToArray());
		}
	}
}
