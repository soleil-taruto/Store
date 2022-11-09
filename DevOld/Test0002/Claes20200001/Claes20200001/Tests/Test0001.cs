using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Numerics;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				if (testcnt % 1000 == 0) Console.WriteLine(testcnt); // cout

				ulong b = SCommon.CRandom.GetULong();
				ulong e = SCommon.CRandom.GetULong();
				ulong m = SCommon.CRandom.GetULong();

				if (m == 0)
					continue;

				ulong ans1 = Test01_b1(b, e, m);
				ulong ans2 = Test01_b2(b, e, m);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		private ulong Test01_b1(ulong b, ulong e, ulong m)
		{
			return ModPow64(b, e, m);
		}

		private ulong Test01_b2(ulong b, ulong e, ulong m)
		{
			return (ulong)BigInteger.ModPow(b, e, m);
		}

		public void Test02()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				if (testcnt % 100000 == 0) Console.WriteLine(testcnt); // cout

				ulong b = SCommon.CRandom.GetULong();
				ulong e = SCommon.CRandom.GetULong();
				ulong m = SCommon.CRandom.GetULong();

				if (m == 0)
					continue;

				ulong ans1 = Test02_b1(b, e, m);
				ulong ans2 = Test02_b2(b, e, m);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		private ulong Test02_b1(ulong b, ulong e, ulong m)
		{
			return ModAdd64(b, e, m);
		}

		private ulong Test02_b2(ulong b, ulong e, ulong m)
		{
			return (ulong)(((BigInteger)b + e) % m);
		}

		// ====

		private static ulong ModPow64(ulong b, ulong e, ulong m)
		{
			ulong a = 1;

			for (; 1 <= e; e >>= 1)
			{
				if ((e & 1) != 0)
					a = ModMul64(a, b, m);

				b = ModMul64(b, b, m);
			}
			return a;
		}

		private static ulong ModMul64(ulong b, ulong e, ulong m)
		{
			ulong a = 0;

			for (; 1 <= e; e >>= 1)
			{
				if ((e & 1) != 0)
					a = ModAdd64(a, b, m);

				b = ModAdd64(b, b, m);
			}
			return a;
		}

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
