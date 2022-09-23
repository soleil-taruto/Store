using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Charlotte.Commons;

namespace Charlotte.JSConfusers
{
	public class JSResource
	{
		#region 予約語リスト

		private static string RES_予約語リスト = Common.ReadResTextFile("RES_予約語リスト.txt");

		#endregion

		// ======
		// ======
		// ======

		public static string[] 予約語リスト = SCommon.TextToLines(RES_予約語リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // 空行とコメント行を除去
			.Distinct()
			.ToArray();
	}
}
