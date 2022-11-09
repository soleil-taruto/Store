using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0014
	{
		public void Test01()
		{
			Test01_a(1);
			Test01_a(2);
			Test01_a(3);
			Test01_a(5);
			Test01_a(10);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int lenlmt)
		{
			// SCommon.CreateSet テスト
			{
				HashSet<string> hs = SCommon.CreateSet();
				List<string> list = new List<string>();

				for (int testcnt = 1; testcnt <= 10000; testcnt++)
				{
					string str = new string(Enumerable
						.Range(0, SCommon.CRandom.GetInt(lenlmt))
						.Select(dummy => SCommon.CRandom.ChooseOne(SCommon.ASCII.ToArray()))
						.ToArray());

					hs.Add(str);

					if (!list.Any(v => v == str))
						list.Add(str);

					if (testcnt % 1000 == 0)
					{
						string[] t1 = hs.ToArray();
						string[] t2 = list.ToArray();

						Console.WriteLine("CS " + lenlmt + ", " + testcnt + " --> " + t1.Length + ", " + t2.Length); // cout

						Array.Sort(t1, SCommon.Comp);
						Array.Sort(t2, SCommon.Comp);

						if (SCommon.Comp(t1, t2, SCommon.Comp) != 0) // ? 不一致
							throw null; // bug !!!
					}
				}
			}

			// SCommon.CreateSetIgnoreCase テスト
			{
				HashSet<string> hs = SCommon.CreateSetIgnoreCase();
				List<string> list = new List<string>();

				for (int testcnt = 1; testcnt <= 10000; testcnt++)
				{
					string str = new string(Enumerable
						.Range(0, SCommon.CRandom.GetInt(lenlmt))
						.Select(dummy => SCommon.CRandom.ChooseOne(SCommon.ASCII.ToArray()))
						.ToArray());

					hs.Add(str);

					if (!list.Any(v => v.ToLower() == str.ToLower()))
						list.Add(str);

					if (testcnt % 1000 == 0)
					{
						string[] t1 = hs.ToArray();
						string[] t2 = list.ToArray();

						Console.WriteLine("IC " + lenlmt + ", " + testcnt + " --> " + t1.Length + ", " + t2.Length); // cout

						Array.Sort(t1, SCommon.Comp);
						Array.Sort(t2, SCommon.Comp);

						if (SCommon.Comp(t1, t2, SCommon.Comp) != 0) // ? 不一致
							throw null; // bug !!!
					}
				}
			}
		}

		public void Test02()
		{
			Test02_a(new string[] { "123", "ABC", "abc" });
			Test02_a(new string[] { "123456", "ABCDEF", "abcdef" });
			Test02_a(new string[] { "123456789", "ABCDEFGHI", "abcdefghi" });
			Test02_a(new string[] { @"

123456789
123456789
123456789
123456789
123456789
123456789
123456789
123456789
123456789

", @"

ABCDEFGHI
ABCDEFGHI
ABCDEFGHI
ABCDEFGHI
ABCDEFGHI
ABCDEFGHI
ABCDEFGHI
ABCDEFGHI
ABCDEFGHI

", @"

abcdefghi
abcdefghi
abcdefghi
abcdefghi
abcdefghi
abcdefghi
abcdefghi
abcdefghi
abcdefghi

"
			});
			Test02_a(new string[] { @"

123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789
123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789

", @"

ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI
ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI ABCDEFGHI

", @"

abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi
abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi abcdefghi

"
			});
		}

		private void Test02_a(string[] src)
		{
			string enc = SCommon.Serializer.I.Join(src);

			int s1 = src.Select(v => v.Length).Sum();
			int s2 = enc.Length;

			Console.WriteLine((double)s2 / s1);
		}

		public void Test03()
		{
			Console.WriteLine(double.IsNaN(double.NaN)); // True
			Console.WriteLine(double.IsNaN(double.PositiveInfinity)); // False
			Console.WriteLine(double.IsNaN(double.NegativeInfinity)); // False

			Console.WriteLine(double.IsInfinity(double.NaN)); // False
			Console.WriteLine(double.IsInfinity(double.PositiveInfinity)); // True
			Console.WriteLine(double.IsInfinity(double.NegativeInfinity)); // True

			Console.WriteLine(double.IsPositiveInfinity(double.NaN)); // False
			Console.WriteLine(double.IsPositiveInfinity(double.PositiveInfinity)); // True
			Console.WriteLine(double.IsPositiveInfinity(double.NegativeInfinity)); // False

			Console.WriteLine(double.IsNegativeInfinity(double.NaN)); // False
			Console.WriteLine(double.IsNegativeInfinity(double.PositiveInfinity)); // False
			Console.WriteLine(double.IsNegativeInfinity(double.NegativeInfinity)); // True

			// ----

			try
			{
				SCommon.ToInt(double.NaN);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			try
			{
				SCommon.ToInt(double.PositiveInfinity);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			try
			{
				SCommon.ToInt(double.NegativeInfinity);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void Test04()
		{
			using (StreamWriter writer = new StreamWriter(Common.NextOutputPath() + ".txt", false, Encoding.Unicode))
			{
				foreach (char chr in SCommon.GetJChars().ToArray().DistinctOrderBy((a, b) => (int)a - (int)b))
				{
					writer.WriteLine(((int)chr).ToString("x4") + " " + chr);
				}
			}
		}

		public void Test05()
		{
			Test05_a(100);
			Test05_a(300);
			Test05_a(1000);
			Test05_a(3000);
			Test05_a(10000);
			Test05_a(30000);
			Test05_a(100000);
			Test05_a(300000);
			Test05_a(1000000);
			Test05_a(3000000);
		}

		private void Test05_a(int scale)
		{
			HashSet<string> hs = SCommon.CreateSet();

			for (int count = 1; count <= scale; count++)
				hs.Add("" + count);

			for (int step = 1; step <= 10; step++)
			{
				HashSet<string> hs2 = SCommon.CreateSet();

				foreach (string h in hs)
				{
					string h2 = "" + h.GetHashCode();

					if (!hs2.Contains(h2))
						hs2.Add(h2);
				}
				hs = hs2;
			}
			Console.WriteLine(((double)hs.Count / scale).ToString("F6") + " " + scale + " ==> " + hs.Count);
		}

		public void Test06()
		{
			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				Test06_a(writer, 100);
				Test06_a(writer, 300);
				Test06_a(writer, 1000);
				Test06_a(writer, 3000);
				Test06_a(writer, 10000);
				Test06_a(writer, 30000);
				Test06_a(writer, 100000);
				Test06_a(writer, 300000);
				Test06_a(writer, 1000000);
				Test06_a(writer, 3000000);
			}
		}

		private void Test06_a(CsvFileWriter writer, int scale)
		{
			Console.WriteLine(scale); // cout

			HashSet<string> hs = SCommon.CreateSet();

			for (int count = 1; count <= scale; count++)
				hs.Add("" + count);

			for (int step = 1; step <= 30; step++)
			{
				Console.WriteLine(scale + ", " + step); // cout

				{
					HashSet<string> hs2 = SCommon.CreateSet();

					foreach (string h in hs)
					{
						string h2 = "" + ((uint)h.GetHashCode() % scale);

						if (!hs2.Contains(h2))
							hs2.Add(h2);
					}
					hs = hs2;
				}

				writer.WriteCell(((double)hs.Count() / scale).ToString("F9"));
			}
			writer.EndRow();
		}

		public void Test07()
		{
			foreach (int a in new int[] { 10, 30, 100 })
			{
				foreach (int b in new int[] { 10, 30, 100 })
				{
					foreach (int c in new int[] { 10, 30, 100 })
					{
						foreach (int d in new int[] { 10, 30, 100 })
						{
							Console.WriteLine(a + ", " + b + ", " + c + ", " + d); // cout

							for (int testcnt = 0; testcnt < 100; testcnt++)
							{
								Test07_a(a, b, c, d);
							}
						}
					}
				}
			}

			Console.WriteLine("OK!"); // cout
		}

		private void Test07_a(int loNum, int roNum, int beNum, int valScale)
		{
			string[] lo = Enumerable.Range(1, loNum).Select(dummy => SCommon.CRandom.GetInt(valScale) + "_1").ToArray();
			string[] ro = Enumerable.Range(1, roNum).Select(dummy => SCommon.CRandom.GetInt(valScale) + "_2").ToArray();
			string[] be = Enumerable.Range(1, beNum).Select(dummy => SCommon.CRandom.GetInt(valScale) + "_B").ToArray();

			Array.Sort(lo, SCommon.Comp);
			Array.Sort(ro, SCommon.Comp);
			Array.Sort(be, SCommon.Comp);

			string[] list1 = lo.Concat(be).ToArray();
			string[] list2 = ro.Concat(be).ToArray();

			SCommon.CRandom.Shuffle(list1);
			SCommon.CRandom.Shuffle(list2);

			List<string> only1 = new List<string>();
			List<string> both1 = new List<string>();
			List<string> both2 = new List<string>();
			List<string> only2 = new List<string>();

			SCommon.Merge(list1, list2, SCommon.Comp, only1, both1, both2, only2);

			if (SCommon.Comp(only1, lo, SCommon.Comp) != 0) // ? 不一致
				throw null; // bug !

			if (SCommon.Comp(both1, be, SCommon.Comp) != 0) // ? 不一致
				throw null; // bug !

			if (SCommon.Comp(both2, be, SCommon.Comp) != 0) // ? 不一致
				throw null; // bug !

			if (SCommon.Comp(only2, ro, SCommon.Comp) != 0) // ? 不一致
				throw null; // bug !
		}

		public void Test08()
		{
			// サロゲートペアとか無いよね？
			// --> 無い。@ 2022.5.x

			foreach (char chr in SCommon.GetJCharCodes())
			{
				byte[] chrBytes = new byte[] 
				{
					(byte)(chr >> 8), 
					(byte)(chr & 0xff),
				};

				string chrStr = SCommon.ENCODING_SJIS.GetString(chrBytes);

				if (chrStr.Length != 1)
					throw null;

				if (chrStr.ToCharArray().Length != 1)
					throw null;
			}

			// ----

			for (byte bChr = 0xa1; bChr <= 0xdf; bChr++)
			{
				byte[] chrBytes = new byte[] 
				{
					bChr,
				};

				string chrStr = SCommon.ENCODING_SJIS.GetString(chrBytes);

				if (chrStr.Length != 1)
					throw null;

				if (chrStr.ToCharArray().Length != 1)
					throw null;
			}
		}

		public void Test09()
		{
			for (byte bChr = 0x00; bChr <= 0x7e; bChr++) // ASCII with control-code
			{
				Test09_a(bChr);
			}
			for (byte bChr = 0xa1; bChr <= 0xdf; bChr++) // 半角カナ
			{
				Test09_a(bChr);
			}
		}

		private void Test09_a(byte bChr)
		{
			char unicode = SCommon.ENCODING_SJIS.GetString(new byte[] { bChr })[0];

			Console.WriteLine(string.Format("{0:x2} => {1:x4} (diff={2})", (int)bChr, (int)unicode, (int)unicode - (int)bChr));
		}

		public void Test10()
		{
			for (int testcnt = 0; testcnt < 3000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("a_testcnt: " + testcnt);

				Test10_a();
			}

			// ----

			for (int testcnt = 0; testcnt < 1000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("b_testcnt: " + testcnt);

				Test10_b();
			}

			// ----

			for (int testcnt = 0; testcnt < 3000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("c_testcnt: " + testcnt);

				Test10_c();
			}

			Console.WriteLine("OK!");
		}

		private void Test10_a()
		{
			char[] strChrEnt = ("\t\r\n " + SCommon.HALF + SCommon.GetJChars()).ToArray();
			string str = new string(Enumerable
				.Range(1, SCommon.CRandom.GetInt(300))
				.Select(dummy => SCommon.CRandom.ChooseOne(strChrEnt))
				.ToArray());

			byte[] b1 = SCommon.GetSJISBytes(str);
			byte[] b2 = SCommon.ENCODING_SJIS.GetBytes(str);

			if (SCommon.Comp(b1, b2) != 0) // ? 不一致
				throw null;
		}

		private void Test10_b()
		{
			string str = new string(Enumerable
				.Range(1, SCommon.CRandom.GetInt(100))
				.Select(dummy => (char)SCommon.CRandom.GetUInt16())
				.ToArray());

			byte[] bs = SCommon.GetSJISBytes(str);

			for (int index = 0; index < bs.Length; index++)
			{
				byte b = bs[index];

				if (b <= 0x7e || (0xa1 <= b && b <= 0xdf)) // ASCII with control-code || 半角カナ
				{
					// noop
				}
				else if (IsSJISChar(bs, index)) // ? 2バイト文字
				{
					index++;
				}
				else // ? SJIS-の文字ではない。
				{
					throw null; // never
				}
			}
		}

		public void Test10_c()
		{
			string str = new string(Enumerable
				.Range(1, SCommon.CRandom.GetInt(100))
				.Select(dummy => (char)SCommon.CRandom.GetUInt16())
				.ToArray());

			string s = SCommon.ToJString(str, true, true, true, true);

			s.ToString(); // チェックすることが無い。
		}

		private bool IsSJISChar(byte[] bs, int index)
		{
			byte b1 = 0;

			foreach (byte b in SCommon.GetJCharBytes())
			{
				if (b1 == 0)
				{
					b1 = b;
				}
				else
				{
					byte b2 = b;

					if (
						b1 == bs[index + 0] &&
						b2 == bs[index + 1]
						)
						return true;

					b1 = 0;
				}
			}
			return false;
		}

		public void Test11()
		{
			for (int testcnt = 0; testcnt < 1000; testcnt++)
			{
				if (testcnt % 100 == 0) Console.WriteLine("testcnt: " + testcnt); // cout

				int[] src = Enumerable
					.Range(0, SCommon.CRandom.GetInt(1000))
					.Select(dummy => SCommon.CRandom.GetRange(-SCommon.IMAX, SCommon.IMAX))
					.ToArray();

				int[] dest;

				using (WorkingDir wd = new WorkingDir())
				{
					string file = wd.MakePath();

					using (FileStream writer = new FileStream(file, FileMode.Create, FileAccess.Write))
					{
						SCommon.WritePartInt(writer, src.Length);

						foreach (int value in src)
						{
							SCommon.WritePartInt(writer, value);
						}
					}

					// ----

					using (FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
					{
						dest = new int[SCommon.ReadPartInt(reader)];

						for (int index = 0; index < dest.Length; index++)
						{
							dest[index] = SCommon.ReadPartInt(reader);
						}
					}
				}

				if (SCommon.Comp(src, dest, SCommon.Comp) != 0) // ? 不一致
					throw null; // bug !
			}

			Console.WriteLine("OK!"); // cout
		}

		public void Test12()
		{
			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				for (int testcnt = 0; testcnt < 20; testcnt++)
				{
					for (int k = 1; k <= 10; k++)
					{
						const int DENOM = 1000;
						double numer = 0.0;

						for (int index = 0; index < DENOM; index++)
						{
							double value = 1.0;

							for (int c = 0; c < k; c++)
							{
								double v = (double)SCommon.CRandom.GetUInt() / uint.MaxValue; // 0.0 ～ 1.0

								value = Math.Min(value, v);
							}
							numer += value;
						}
						double ans = numer / DENOM;

						writer.WriteCell(ans.ToString("F9"));

						// ans == 1.0 / (k + 1)
					}
					writer.EndRow();
				}
			}
		}

		public void Test13()
		{
			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				for (int k = 1; k <= 10; k++)
				{
					Console.WriteLine(k); // cout

					const int DIV = 30;
					int[] counts = new int[DIV];

					const int DENOM = 1000000;

					for (int index = 0; index < DENOM; index++)
					{
						double value = 1.0;

						for (int c = 0; c < k; c++)
						{
							double v = (double)SCommon.CRandom.GetUInt() / uint.MaxValue; // 0.0 ～ 1.0

							value = Math.Min(value, v);
						}
						counts[Math.Min((int)(value * DIV), DIV - 1)]++;
					}
					writer.WriteRow(counts.Select(v => "" + v).ToArray());
				}
			}
		}

		public void Test14()
		{
			DateTime stTm;
			DateTime edTm;

			List<byte[]> blocks = new List<byte[]>();

			for (int count = 1; count <= 200; count++)
			{
				Console.WriteLine("テスト.1 " + count);

				blocks.Add(SCommon.CRandom.GetBytes(2000000)); // 2MB
			}
			Common.Pause();

			stTm = DateTime.Now;
			for (int count = 1; count <= 1000; count++)
			{
				Console.WriteLine("テスト.2 " + count);

				blocks.Add(SCommon.CRandom.GetBytes(2000000)); // 2MB
				SCommon.FastDesertElement(blocks, SCommon.CRandom.GetInt(blocks.Count));
			}
			edTm = DateTime.Now;
			Console.WriteLine(edTm - stTm);
			Common.Pause();

			stTm = DateTime.Now;
			for (int count = 1; count <= 1000; count++)
			{
				Console.WriteLine("テスト.3 " + count);

				blocks.Add(SCommon.CRandom.GetBytes(2000000)); // 2MB
				SCommon.FastDesertElement(blocks, SCommon.CRandom.GetInt(blocks.Count));

				if (count % 100 == 0)
					GC.Collect();
			}
			edTm = DateTime.Now;
			Console.WriteLine(edTm - stTm);
			Common.Pause();

			stTm = DateTime.Now;
			for (int count = 1; count <= 1000; count++)
			{
				Console.WriteLine("テスト.4 " + count);

				blocks.Add(SCommon.CRandom.GetBytes(2000000)); // 2MB
				SCommon.FastDesertElement(blocks, SCommon.CRandom.GetInt(blocks.Count));
				GC.Collect();
			}
			edTm = DateTime.Now;
			Console.WriteLine(edTm - stTm);
			Common.Pause();

			blocks = null;

			Common.Pause();

			GC.Collect();

			Common.Pause();
		}

		public void Test15()
		{
			Test15_a(@"C:\");
			Test15_a(@"C:\Dev\Tests\Test0001\Claes20200001\Claes20200001\Tests");
			Test15_a(@"C:\temp");
			Test15_a(@"C:\temp\1\2\3\4\5"); // 存在しなくても良いらしい。
			Test15_a(@"C:\1\2\3\4\5\6\7\8\9");
			//Test15_a(@"X:\"); // 存在しないドライブは例外を投げる。
		}

		private void Test15_a(string dir)
		{
			Console.WriteLine(dir);

			DriveInfo drvInfo = new DriveInfo(dir);

			Console.WriteLine(drvInfo.AvailableFreeSpace);
		}
	}
}
