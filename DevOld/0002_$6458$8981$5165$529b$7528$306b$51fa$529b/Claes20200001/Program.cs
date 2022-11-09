using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
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
			// -- choose one --

			Main4();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//Common.Pause();
		}

		private void Main4()
		{
			Console.WriteLine("INPUT YEAR:");
			Main4_Year(int.Parse(Console.ReadLine()));
		}

		private void Main4_Year(int year)
		{
			Console.WriteLine("★★★★★");
			Console.WriteLine("★" + year + "年★");
			Console.WriteLine("★★★★★");

			Console.WriteLine("< " + Consts.R_MASTER_FILE); // cout
			Console.WriteLine("> " + Consts.W_FILE); // cout

			if (year < 1000 || 9999 < year)
				throw new Exception("Bad year");

			if (!File.Exists(Consts.R_MASTER_FILE))
				throw new Exception("no R_MASTER_FILE");

			SCommon.DeletePath(Consts.W_DIR);
			SCommon.CreateDir(Consts.W_DIR);

			string[][] rows;

			using (CsvFileReader reader = new CsvFileReader(Consts.R_MASTER_FILE, SCommon.ENCODING_SJIS))
			{
				rows = reader.ReadToEnd();
			}

			rows = rows
				.Where(row => int.Parse(row[0]) / 10000 == year)
				.ToArray();

			Array.Sort(rows, (a, b) =>
			{
				int ret;

				ret = SCommon.Comp(a[3], b[3]); // 但し書き
				if (ret != 0)
					return ret;

				ret = SCommon.Comp(a[0], b[0]); // 年月日
				if (ret != 0)
					return ret;

				ret = SCommon.Comp(string.Join(":", a), string.Join(":", b));
				return ret;
			});

			using (CsvFileWriter writer = new CsvFileWriter(Consts.W_FILE, false, SCommon.ENCODING_SJIS))
			{
				foreach (string[] row in rows)
				{
					row[4] = "★ここへ摘要を入力★";

					writer.WriteRow(row);
				}
			}

			SCommon.Batch(new string[] { "START " + Consts.W_DIR }); // 出力先フォルダを開く

			Console.WriteLine("********************"); // cout
			Console.WriteLine("**** SUCCESSFUL ****"); // cout
			Console.WriteLine("********************"); // cout

			Thread.Sleep(2000); // 目視のため
		}
	}
}
