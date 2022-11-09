using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class ReceiptInfoHelper
	{
		public const string RECEIPT_FILE = @"..\..\..\..\dat\Receipt.csv";

		public static void Load(string file)
		{
			Ground.I.ReceiptInfos.Clear();

			if (File.Exists(file) == false)
				return;

			using (CsvFileReader reader = new CsvFileReader(file))
			{
				for (; ; )
				{
					string[] row = reader.ReadRow();

					if (row == null)
						break;

					ReceiptInfo ri = new ReceiptInfo()
					{
						日付 = int.Parse(row[0]),
						金額 = int.Parse(row[1]),
						支払先 = row[2],
						但し書き = row[3],
						Option = row[4],
					};

					{
						long timeStamp = ri.日付 * 1000000L;

						if (SCommon.TimeStampToSec.ToTimeStamp(SCommon.TimeStampToSec.ToSec(timeStamp)) != timeStamp)
							throw new Exception("不正な日付です。" + ri.日付);
					}

					if (ri.金額 < 1)
						throw new Exception("不正な金額です。" + ri.金額);

					if (ri.支払先 == "")
						throw new Exception("支払先が空です。");

					if (ri.但し書き == "")
						throw new Exception("但し書きが空です。");

					Ground.I.ReceiptInfos.Add(ri);
				}
			}
		}

		public static void Save(string file)
		{
			SCommon.BackupFile(file); // 2bs

			using (CsvFileWriter writer = new CsvFileWriter(file))
			{
				foreach (ReceiptInfo ri in Ground.I.ReceiptInfos)
				{
					Write(writer, ri);
				}
			}
		}

		public static void Add(string file, ReceiptInfo ri)
		{
			SCommon.BackupFile(file); // 2bs

			using (CsvFileWriter writer = new CsvFileWriter(file, true))
			{
				Write(writer, ri);
			}
		}

		private static void Write(CsvFileWriter writer, ReceiptInfo ri)
		{
			writer.WriteRow(new string[]
			{
				"" + ri.日付,
				"" + ri.金額,
				ri.支払先,
				ri.但し書き,
				ri.Option,
			});
		}

		public delegate string GetMember(ReceiptInfo ri);

		public static string[] GetEntries(GetMember getMember)
		{
			HashSet<string> dest = new HashSet<string>();

			foreach (ReceiptInfo ri in Ground.I.ReceiptInfos)
			{
				string value = getMember(ri);

				if (dest.Contains(value) == false)
				{
					dest.Add(value);
				}
			}
			string[] ret = dest.ToArray();
			Array.Sort(ret, SCommon.Comp);
			return ret;
		}
	}
}
