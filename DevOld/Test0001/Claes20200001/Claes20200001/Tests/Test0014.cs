using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Charlotte.Tests
{
	public class Test0014
	{
		public void Test01()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				if (testcnt % 1000 == 0) Console.WriteLine("testcnt: " + testcnt); // cout

				ulong a = GetULongRand();
				ulong b = GetULongRand();
				ulong ans1;
				ulong ans2;

				ans1 = Add64(a, b);
				unchecked { ans2 = a + b; }

				if (ans1 != ans2)
					throw null;

				ans1 = Sub64(a, b);
				unchecked { ans2 = a - b; }

				if (ans1 != ans2)
					throw null;

				ans1 = Mul64(a, b);
				unchecked { ans2 = a * b; }

				if (ans1 != ans2)
					throw null;

				b &= ChooseOne(
					ulong.MaxValue,
					0x0000ffffffffffffUL,
					0x00000000ffffffffUL,
					0x000000000000ffffUL
					);

				if (b == 0)
					b = 1;

				ans1 = Div64(a, b);
				ans2 = a / b;

				if (ans1 != ans2)
					throw null;

				//Console.WriteLine("OK");
			}
			Console.WriteLine("OK!");
		}

		private static ulong Add64(ulong a, ulong b)
		{
			while (b != 0)
			{
				ulong na = a ^ b;
				b = (a & b) << 1;
				a = na;
			}
			return a;
		}

		private static ulong Sub64(ulong a, ulong b)
		{
			b ^= ulong.MaxValue;
			b = Add64(b, 1);
			return Add64(a, b);
		}

		private static ulong Mul64(ulong a, ulong b)
		{
			ulong c = 0;

			for (; b != 0; b >>= 1, a <<= 1)
				if ((b & 1) != 0)
					c = Add64(c, a);

			return c;
		}

		private static ulong Div64(ulong a, ulong b)
		{
			ulong c = 0;
			ulong d = 1;

			while ((b & (1UL << 63)) == 0)
			{
				b <<= 1;
				d <<= 1;
			}
			while (d != 0)
			{
				if (a >= b)
				{
					a = Sub64(a, b);
					c |= d;
				}
				b >>= 1;
				d >>= 1;
			}
			return c;
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

		private static T ChooseOne<T>(params T[] values)
		{
			return values[(int)(GetULongRand() % (ulong)values.Length)];
		}

		// ====
	}
}
