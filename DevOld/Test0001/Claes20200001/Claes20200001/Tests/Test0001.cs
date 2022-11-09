using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			Console.WriteLine(((int)'\0').ToString()); // 0

			Console.WriteLine(((int)'\b').ToString()); // 8
			Console.WriteLine(((int)'\t').ToString()); // 9
			Console.WriteLine(((int)'\n').ToString()); // 10
			Console.WriteLine(((int)'\v').ToString()); // 11
			Console.WriteLine(((int)'\f').ToString()); // 12
			Console.WriteLine(((int)'\r').ToString()); // 13

			Console.WriteLine(((int)'"').ToString()); // 34
			Console.WriteLine(((int)'\'').ToString()); // 39
			Console.WriteLine(((int)'\\').ToString()); // 92
			Console.WriteLine(((int)'`').ToString()); // 96
		}

		public void Test02()
		{
			for (int testcnt = 0; testcnt < 100; testcnt++)
			{
				double value = (double)SCommon.CRandom.GetInt(SCommon.IMAX) / SCommon.IMAX;

				Console.WriteLine(value.ToString("F20"));
			}
			for (int testcnt = 0; testcnt < 100; testcnt++)
			{
				double value = (double)SCommon.CRandom.GetLong(SCommon.IMAX_64) / SCommon.IMAX_64;

				Console.WriteLine(value.ToString("F20"));
			}
			for (int testcnt = 0; testcnt < 100; testcnt++)
			{
				double value = (double)(SCommon.CRandom.GetULong() & ((1UL << 52) - 1)) / (1UL << 52);

				Console.WriteLine(value.ToString("F20"));
			}

			// ----
			// ----
			// ----

			{
				double value = (double)((1UL << 52) - 1);

				Console.WriteLine(value.ToString("F20")); // 4503599627370500.00000000000000000000

				value += 1.0;

				Console.WriteLine(value.ToString("F20")); // 4503599627370500.00000000000000000000
			}

			// ----
			// ----
			// ----

			for (int testcnt = 1; testcnt < 64; testcnt++)
			{
				double value = 1.0 + 1.0 / (1UL << testcnt);

				Console.WriteLine(testcnt.ToString("D2") + " ==> " + value.ToString("F20"));
			}

			Test02_a(0);
			Test02_a(1);
			Test02_a(2);
			Test02_a(3);

			Test02_a(uint.MaxValue - 3);
			Test02_a(uint.MaxValue - 2);
			Test02_a(uint.MaxValue - 1);
			Test02_a(uint.MaxValue);

			// ----

			Test02_b(0);
			Test02_b(1);
			Test02_b(2);
			Test02_b(3);

			Test02_b(SCommon.IMAX - 3);
			Test02_b(SCommon.IMAX - 2);
			Test02_b(SCommon.IMAX - 1);
			Test02_b(SCommon.IMAX);
		}

		private void Test02_a(uint numer)
		{
			double value = (double)numer / uint.MaxValue;

			Console.WriteLine(value.ToString("F20"));
		}

		private void Test02_b(int numer)
		{
			double value = (double)numer / SCommon.IMAX;

			Console.WriteLine(value.ToString("F20"));
		}

		public void Test03()
		{
			for (int c = 0; c < 100; c++)
			{
				using (WorkingDir wd = new WorkingDir())
				{
					for (int d = 0; d < 100; d++)
					{
						Console.WriteLine(wd.MakePath());
					}
				}
			}
		}

		public void Test04()
		{
			for (int testcnt = 0; testcnt < 1000; testcnt++)
			{
				Console.WriteLine(SCommon.CRandom.GetRange(1000, 9999));
			}
			for (int testcnt = 0; testcnt < 1000; testcnt++)
			{
				Console.WriteLine(SCommon.CRandom.GetLong(9000) + 1000);
			}

			// ----
			// ----
			// ----

			try
			{
				SCommon.CRandom.GetInt(0); // 例外
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.WriteLine("想定された例外");
			}
		}
	}
}
