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

		private class ReceiptInfo
		{
			public string GroupName; // 摘要
			public int Month;
			public int Price;
		}

		private void Main4()
		{
			Console.WriteLine("< " + Consts.R_FILE); // cout
			Console.WriteLine("> " + Consts.W_FILE); // cout

			string[][] rows;

			using (CsvFileReader reader = new CsvFileReader(Consts.R_FILE, SCommon.ENCODING_SJIS))
			{
				rows = reader.ReadToEnd();
			}

			ReceiptInfo[] receipts = rows
				.Select(row => new ReceiptInfo()
				{
					GroupName = row[4],
					Month = int.Parse(row[0]) / 100 % 100,
					Price = int.Parse(row[1]),
				})
				.ToArray();

			string[] groupNames = receipts
				.Select(receipt => receipt.GroupName)
				.OrderBy(SCommon.Comp)
				.OrderedDistinct((a, b) => a == b)
				.ToArray();

			using (CsvFileWriter writer = new CsvFileWriter(Consts.W_FILE, false, SCommon.ENCODING_SJIS))
			{
				writer.WriteCell("");

				foreach (string groupName in groupNames)
					writer.WriteCell(groupName);

				writer.EndRow();

				for (int month = 1; month <= 12; month++)
				{
					writer.WriteCell(month + "月");

					foreach (string groupName in groupNames)
						writer.WriteCell("" + receipts
							.Where(receipt => receipt.GroupName == groupName && receipt.Month == month)
							.Select(receipt => receipt.Price)
							.Sum());

					writer.EndRow();
				}
			}

			Console.WriteLine("********************"); // cout
			Console.WriteLine("**** SUCCESSFUL ****"); // cout
			Console.WriteLine("********************"); // cout

			Thread.Sleep(2000); // 目視のため
		}
	}
}
