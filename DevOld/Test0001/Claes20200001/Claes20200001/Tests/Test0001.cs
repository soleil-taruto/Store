using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			//long timeStamp = 16010101085959; // 例外：有効な Win32 FileTime ではありません。
			long timeStamp = 16010101090000; // ok
			//long timeStamp = 19691231235959; // ok
			//long timeStamp = 19700101000000; // ok
			//long timeStamp = 30001231235959; // ok
			//long timeStamp = 99991231235959; // ok
			//long timeStamp = 100000101000000; // DateTimeが例外を投げる。

			File.WriteAllBytes(@"C:\temp\1.txt", SCommon.EMPTY_BYTES);

			FileInfo fileInfo = new FileInfo(@"C:\temp\1.txt");

			fileInfo.CreationTime = SCommon.SimpleDateTime.FromTimeStamp(timeStamp).ToDateTime();
			fileInfo.LastWriteTime = SCommon.SimpleDateTime.FromTimeStamp(timeStamp).ToDateTime();
			fileInfo.LastAccessTime = SCommon.SimpleDateTime.FromTimeStamp(timeStamp).ToDateTime();

			Console.WriteLine("OK!");
		}

		public void Test02()
		{
			for (int testcnt = 0; testcnt < 1000; testcnt++)
			{
				Console.WriteLine("testcnt: " + testcnt);

				int count = SCommon.CRandom.GetInt(100) + 1;

				{
					// 重複無し
					string[] lines = Enumerable.Range(1, count)
						.Select(v => "" + v)
						.ToArray();

					SCommon.CRandom.Shuffle(lines);

					if (SCommon.HasSame(lines, SCommon.Comp)) // ? 重複有り
						throw null; // 想定外
				}

				{
					// 重複有り
					string[] lines = Enumerable.Range(1, count)
						.Select(v => "" + v)
						.Concat(new string[] { "" + (SCommon.CRandom.GetInt(count) + 1) })
						.ToArray();

					SCommon.CRandom.Shuffle(lines);

					if (!SCommon.HasSame(lines, SCommon.Comp)) // ? 重複無し
						throw null; // 想定外
				}

				{
					// 重複無し
					string[] lines = Enumerable.Range(1, count)
						.Select(v => "" + v)
						.ToArray();

					SCommon.CRandom.Shuffle(lines);

					SCommon.ForEachPair(lines, (a, b) =>
					{
						if (a == b)
							throw null; // 想定外
					});
				}

				{
					// 重複有り
					string[] lines = Enumerable.Range(1, count)
						.Select(v => "" + v)
						.Concat(new string[] { "" + (SCommon.CRandom.GetInt(count) + 1) })
						.ToArray();

					SCommon.CRandom.Shuffle(lines);

					bool threw = false;
					try
					{
						SCommon.ForEachPair(lines, (a, b) =>
						{
							if (a == b)
								throw null;
						});
					}
					catch
					{
						threw = true;
					}
					if (!threw)
						throw null; // 想定外
				}
			}

			Console.WriteLine("OK!");
		}

		public void Test03()
		{
			string[][] rows;

			using (CsvFileReader reader = new CsvFileReader(@"C:\temp\1.csv"))
			{
				rows = reader.ReadToEnd();
			}

			using (CsvFileWriter writer = new CsvFileWriter(@"C:\temp\2.csv"))
			{
				writer.WriteRows(rows);
			}
		}

		public void Test04()
		{
			string retOnly = @"
";

			Console.WriteLine(string.Join(", ", retOnly.Select(chr => "" + (int)chr))); // 13, 10
		}
	}
}
