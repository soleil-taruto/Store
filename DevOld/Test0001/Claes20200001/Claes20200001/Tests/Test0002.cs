using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				ulong a = SCommon.CRandom.GetULong();
				ulong b = SCommon.CRandom.GetULong();
				ulong m = SCommon.CRandom.GetULong();

				if (m == 0)
					continue;

				ulong ans1 = Test01_b1(a, b, m);
				ulong ans2 = Test01_b2(a, b, m);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		private ulong Test01_b1(ulong a, ulong b, ulong m)
		{
			const ulong MA = (1UL << 63) - 0;
			const ulong MB = (1UL << 63) - 1;

			ulong r = (ulong.MaxValue % m + 1) % m;

			for (int c = 0; c < 2; c++)
			{
				int k = 0;

				if ((a & MA) != 0) k++;
				if ((b & MA) != 0) k++;

				a = (a & MB) + (b & MB);

				if ((a & MA) != 0) k++;

				a &= MB;

				if ((k & 1) != 0)
					a |= MA;

				b = (k & 2) != 0 ? r : 0;
			}
			return (a + b) % m;
		}

		private ulong Test01_b2(ulong a, ulong b, ulong m)
		{
			return ModAdd64(a, b, m);
		}

		// ====

		private static ulong ModAdd64(ulong a, ulong b, ulong m)
		{
			ulong r = (ulong.MaxValue % m + 1) % m;

			while (ulong.MaxValue - a < b)
			{
				unchecked { a += b; }
				b = r;
			}
			return (a + b) % m;
		}
	}
}
