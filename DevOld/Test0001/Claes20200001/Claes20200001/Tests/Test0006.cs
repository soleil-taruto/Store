using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;

namespace Charlotte.Tests
{
	public class Test0006
	{
		public void Test01()
		{
			ulong[] MASKS = new ulong[] { 0xf, 0xff, 0xffff, 0xffffffff, ulong.MaxValue };

			foreach (ulong m1 in MASKS)
			{
				foreach (ulong m2 in MASKS)
				{
					foreach (ulong m3 in MASKS)
					{
						for (int testcnt = 0; testcnt < 1000; testcnt++)
						{
							Test01_a(m1, m2, m3);
						}
					}
				}
			}
			Console.WriteLine("OK!");
		}

		private void Test01_a(ulong m1, ulong m2, ulong m3)
		{
			ulong a = GetULongRand() & m1;
			ulong b = GetULongRand() & m2;
			ulong m = GetULongRand() & m3;

			if (m == 0) // テスト不可 -> スキップ
				return;

			ulong c = ModPow64(a, b, m);

			Console.WriteLine("ModPow ( " + a + " , " + b + " , " + m + " ) == " + c);

			ulong d = Test01_b(a, b, m);

			if (c != d)
				throw null;
		}

		private static ulong ModPow64(ulong b, ulong e, ulong m)
		{
			ulong a = 1;

			for (; 1 <= e; e >>= 1)
			{
				if ((e & 1) != 0)
					a = ModMul64(a, b, m);

				b = ModMul64(b, b, m);
			}
			return a % m;
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

		// ====
		// Random
		// ====

		private static RandomNumberGenerator Csprng = new RNGCryptoServiceProvider();

		private static ulong GetULongRand()
		{
			byte[] data = new byte[8];
			ulong value = 0;

			Csprng.GetBytes(data);

			foreach (byte b in data)
			{
				value <<= 8;
				value |= b;
			}
			return value;
		}

		// ====

		private ulong Test01_b(ulong a, ulong b, ulong m)
		{
			//BigInteger aa = new BigInteger(a);
			//BigInteger bb = new BigInteger(b);
			//BigInteger mm = new BigInteger(m);

			//return (ulong)Test01_ModPow(aa, bb, mm);

			// ----

			return (ulong)Test01_ModPow(a, b, m);
		}

		private BigInteger Test01_ModPow(BigInteger a, BigInteger b, BigInteger m)
		{
			if (b == 0)
			{
				return 1 % m;
			}
			if (b == 1)
			{
				return a % m;
			}
			if (b % 2 == 1)
			{
				return (Test01_ModPow(a, b - 1, m) * a) % m;
			}
			return Test01_ModPow((a * a) % m, b / 2, m);
		}
	}
}
