using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0005
	{
		/// <summary>
		/// 足し算
		/// </summary>
		public void Test01()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				ulong a = SCommon.CRandom.GetULong();
				ulong b = SCommon.CRandom.GetULong();

				ulong ans1 = Test01_b1(a, b);
				ulong ans2 = Test01_b2(a, b);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		private ulong Test01_b1(ulong a, ulong b)
		{
			while (b != 0)
			{
				ulong na = a ^ b;
				b = (a & b) << 1;
				a = na;
			}
			return a;
		}

		private ulong Test01_b2(ulong a, ulong b)
		{
			unchecked
			{
				return a + b;
			}
		}

		/// <summary>
		/// 掛け算
		/// </summary>
		public void Test02()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				ulong a = SCommon.CRandom.GetULong();
				ulong b = SCommon.CRandom.GetULong();

				ulong ans1 = Test02_b1(a, b);
				ulong ans2 = Test02_b2(a, b);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		private ulong Test02_b1(ulong s, ulong e)
		{
			ulong a = 0;

			for (; e != 0; e >>= 1)
			{
				if ((e & 1) != 0)
				{
					ulong b = s;

					while (b != 0)
					{
						ulong na = a ^ b;
						b = (a & b) << 1;
						a = na;
					}
				}
				s <<= 1;
			}
			return a;
		}

		private ulong Test02_b2(ulong a, ulong b)
		{
			unchecked
			{
				return a * b;
			}
		}

		/// <summary>
		/// 引き算
		/// </summary>
		public void Test03()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				ulong a = SCommon.CRandom.GetULong();
				ulong b = SCommon.CRandom.GetULong();

				ulong ans1 = Test03_b1(a, b);
				ulong ans2 = Test03_b2(a, b);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		private ulong Test03_b1(ulong a, ulong b)
		{
			b ^= ulong.MaxValue;
			unchecked { b++; }

			while (b != 0)
			{
				ulong na = a ^ b;
				b = (a & b) << 1;
				a = na;
			}
			return a;
		}

		private ulong Test03_b2(ulong a, ulong b)
		{
			unchecked
			{
				return a - b;
			}
		}

		/// <summary>
		/// 割り算
		/// </summary>
		public void Test04()
		{
			Test04_a(ulong.MaxValue);
			Test04_a(0x0000ffffffffffffUL);
			Test04_a(0x00000000ffffffffUL);
			Test04_a(0x000000000000ffffUL);

			Console.WriteLine("OK!");
		}

		private void Test04_a(ulong m)
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				ulong a = SCommon.CRandom.GetULong();
				ulong b = SCommon.CRandom.GetULong();

				b &= m;

				if (b == 0)
					continue;

				ulong ans1 = Test04_b1(a, b);
				ulong ans2 = Test04_b2(a, b);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private ulong Test04_b1(ulong a, ulong b)
		{
			ulong d = 1;
			ulong n = 0;

			while ((b & (1UL << 63)) == 0)
			{
				b <<= 1;
				d <<= 1;
			}
			while (d != 0)
			{
				if (a >= b)
				{
					a -= b;
					n |= d;
				}
				b >>= 1;
				d >>= 1;
			}
			return n;
		}

		private ulong Test04_b2(ulong a, ulong b)
		{
			return a / b;
		}
	}
}
