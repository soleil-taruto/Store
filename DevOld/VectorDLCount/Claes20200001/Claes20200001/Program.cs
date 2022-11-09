using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4();
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			//Common.Pause();
		}

		private void Main4()
		{
			// -- choose one --

			Main5();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}

		/// <summary>
		/// ダウンロード数情報
		/// </summary>
		private class DLCountInfo
		{
			/// <summary>
			/// 集計年月 - 年
			/// </summary>
			public int Y;

			/// <summary>
			/// 集計年月 - 月
			/// </summary>
			public int M;

			/// <summary>
			/// ソフト名
			/// </summary>
			public string SoftName;

			/// <summary>
			/// ダウンロード数
			/// </summary>
			public int Count;

			public string GetYM()
			{
				return this.Y + "年" + this.M.ToString("D2") + "月";
			}
		}

		private const string INPUT_DIR = @"C:\temp";
		private const string INPUT_EXT = ".eml";
		private const string OUTPUT_FILE = @"C:\temp\Output.csv";

		private void Main5()
		{
			List<DLCountInfo> dlCounts = new List<DLCountInfo>();

			foreach (string file in Directory.GetFiles(INPUT_DIR))
			{
				if (!SCommon.EndsWithIgnoreCase(file, INPUT_EXT)) // ? 拡張子が異なる。-> 対象外
					continue;

				string[] lines = File.ReadAllLines(file, Encoding.GetEncoding(50220));

				int y = -1;
				int m = -1;

				foreach (string line in lines)
				{
					if (line.StartsWith("(このリストは"))
					{
						int[] ymd = line.Substring(7, 10).Split('/').Select(v => int.Parse(v)).ToArray();

						y = ymd[0];
						m = ymd[1];

						// 月の前半に作成されたリスト -> 先月分と見なす。
						// 月の後半に作成されたリスト -> 今月分と見なす。

						if (ymd[2] <= 15) // ? 月の前半 -> 先月にする。
						{
							m--;

							if (m < 1)
							{
								y--;
								m = 12;
							}
						}

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
							SoftName = softName,
							Count = count,
						});
					}
				}
			}

			string[] softNames = dlCounts.Select(v => v.SoftName).DistinctOrderBy(SCommon.Comp).ToArray();

			using (CsvFileWriter writer = new CsvFileWriter(OUTPUT_FILE))
			{
				writer.WriteCell("");

				foreach (string softName in softNames)
					writer.WriteCell(softName);

				writer.EndRow();

				foreach (string ym in dlCounts.Select(v => v.GetYM()).DistinctOrderBy(SCommon.Comp))
				{
					writer.WriteCell(ym);

					foreach (string softName in softNames)
					{
						DLCountInfo dlCount = dlCounts.FirstOrDefault(v => v.GetYM() == ym && v.SoftName == softName);

						if (dlCount == null)
							writer.WriteCell("");
						else
							writer.WriteCell(dlCount.Count.ToString());
					}
					writer.EndRow();
				}
			}
		}
	}
}
