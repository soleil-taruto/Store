using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	// ----
	// == 使い方 ==
	// Becky!Ver2 の受信箱にある "[Vector] 登録内容確認のお願い" を全て選択して
	// ファイル / エクスポート
	// -- eml形式(1メール 1ファイル)
	// -- C:\temp に出力する。
	// Test01 を実行する。
	// -> C:\temp\Output.csv が作成される。
	// ----

	public class Test0011
	{
		/// <summary>
		/// ダウンロード数情報
		/// </summary>
		private class DLCountInfo
		{
			/// <summary>
			/// 集計日付 - 年
			/// </summary>
			public int Y;

			/// <summary>
			/// 集計日付 - 月
			/// </summary>
			public int M;

			/// <summary>
			/// 集計日付 - 日
			/// </summary>
			public int D;

			/// <summary>
			/// ファイル番号
			/// </summary>
			public int FileNo;

			/// <summary>
			/// ソフト名
			/// </summary>
			public string SoftName;

			/// <summary>
			/// ダウンロード数
			/// </summary>
			public int Count;

			public string GetYMD()
			{
				return this.Y + "年" + this.M.ToString("D2") + "月" + this.D.ToString("D2") + "日(" + this.FileNo.ToString("D4") + ")";
			}
		}

		private const string INPUT_DIR = @"C:\temp";
		private const string INPUT_EXT = ".eml";
		private const string OUTPUT_FILE = @"C:\temp\Output.csv";

		public void Test01()
		{
			List<DLCountInfo> dlCounts = new List<DLCountInfo>();
			int fileNo = 1;

			foreach (string file in Directory.GetFiles(INPUT_DIR).OrderBy(SCommon.Comp)) // 一応ソートしとく
			{
				if (!SCommon.EndsWithIgnoreCase(file, INPUT_EXT)) // ? 拡張子が異なる。-> 対象外
					continue;

				string[] lines = File.ReadAllLines(file, Encoding.GetEncoding(50220));

				int y = -1;
				int m = -1;
				int d = -1;

				foreach (string line in lines)
				{
					if (line.StartsWith("(このリストは"))
					{
						int[] ymd = line.Substring(7, 10).Split('/').Select(v => int.Parse(v)).ToArray();

						y = ymd[0];
						m = ymd[1];
						d = ymd[2];

						break;
					}
				}

				string softName = "Dummy";

				foreach (string line in lines)
				{
					if (line.StartsWith("(Y) "))
					{
						softName = line.Substring(4);
					}
					else if (line.StartsWith("ダウンロード数"))
					{
						int count = int.Parse(line.Substring(7).Trim());

						dlCounts.Add(new DLCountInfo()
						{
							Y = y,
							M = m,
							D = d,
							FileNo = fileNo,
							SoftName = softName,
							Count = count,
						});
					}
				}

				fileNo++;
			}

			string[] softNames = dlCounts.Select(v => v.SoftName).DistinctOrderBy(SCommon.Comp).ToArray();

			using (CsvFileWriter writer = new CsvFileWriter(OUTPUT_FILE))
			{
				writer.WriteCell("");

				foreach (string softName in softNames)
					writer.WriteCell(softName);

				writer.EndRow();

				foreach (string ymd in dlCounts.Select(v => v.GetYMD()).DistinctOrderBy(SCommon.Comp))
				{
					writer.WriteCell(ymd);

					foreach (string softName in softNames)
					{
						DLCountInfo dlCount = dlCounts.FirstOrDefault(v => v.GetYMD() == ymd && v.SoftName == softName);

						if (dlCount == null)
							writer.WriteCell("");
						else
							writer.WriteCell(dlCount.Count.ToString());
					}
					writer.EndRow();
				}
			}

			Console.WriteLine("OK!");
		}
	}
}
