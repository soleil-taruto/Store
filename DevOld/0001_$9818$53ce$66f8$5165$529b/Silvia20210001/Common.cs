using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static string ToZenDigit(string str)
		{
			return str
				.Replace("0", "０")
				.Replace("1", "１")
				.Replace("2", "２")
				.Replace("3", "３")
				.Replace("4", "４")
				.Replace("5", "５")
				.Replace("6", "６")
				.Replace("7", "７")
				.Replace("8", "８")
				.Replace("9", "９");
		}

		public static string 名前フィルタ(string str)
		{
			str = str.Replace("\t", "");
			str = str.Replace("\r", "");
			str = str.Replace("\n", "");

			str = str.Replace(" ", "");
			str = str.Replace("　", "");

			str = SCommon.ENCODING_SJIS.GetString(SCommon.ENCODING_SJIS.GetBytes(str));

			str = Strings.StrConv(str, VbStrConv.Wide);

			return str;
		}
	}
}
