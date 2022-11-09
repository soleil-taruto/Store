using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Charlotte.Tests
{
	public class Test0008
	{
		public void Test01()
		{
			Test01_a(10);
			Test01_a(100);
			Test01_a(1000);
			Test01_a(10000);
			Test01_a(100000);
			Test01_a(1000000);
			Test01_a(10000000);
			Test01_a(100000000);
			Test01_a(1000000000);
			Test01_a(10000000000);

			Console.WriteLine("OK!");
		}

		public void Test01_a(ulong valueScale)
		{
			HashSet<string> hs1 = new HashSet<string>();
			HashSet<string> hs2 = new HashSet<string>(new Adler32StringIEC());

			for (int testcnt = 0; testcnt < 300000; testcnt++)
			{
				string value = (GetULongRand() % valueScale).ToString();

				bool c1 = hs1.Contains(value);
				bool c2 = hs2.Contains(value);

				if (c1 != c2)
					throw null; // BUG !!!

				if (c1)
				{
					hs1.Remove(value);
					hs2.Remove(value);
				}
				else
				{
					hs1.Add(value);
					hs2.Add(value);
				}
			}
			Console.WriteLine("OK");
		}

		public class Adler32StringIEC : IEqualityComparer<string>
		{
			public bool Equals(string a, string b)
			{
				return a == b;
			}

			public int GetHashCode(string a)
			{
				return (int)Adler32.ComputeHash(Encoding.UTF8.GetBytes(a));
			}
		}

		public static class Adler32
		{
			private const uint MODULO = 65521;

			public static uint ComputeHash(IEnumerable<byte> data)
			{
				uint a = 1;
				uint b = 0;

				foreach (byte c in data)
				{
					a = (a + c) % MODULO;
					b = (b + a) % MODULO;
				}
				return a | (b << 16);
			}
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

		public void Test02()
		{
			Test02_a(2);
			Test02_a(3);
			Test02_a(5);
			Test02_a(7);
			Test02_a(11);
			Test02_a(13);

			Console.WriteLine("OK!");
		}

		public void Test02_a(int valueScale)
		{
			HashSet<string> hs1 = new HashSet<string>();
			HashSet<byte[]> hs2 = new HashSet<byte[]>(new Adler32BytesIEC());

			for (int testcnt = 0; testcnt < 100000; testcnt++)
			{
				byte[] value = GetRandBytes(GetRandInt(valueScale));
				string strValue = ToString(value);

				bool c1 = hs1.Contains(strValue);
				bool c2 = hs2.Contains(value);

				if (c1 != c2)
					throw null; // BUG !!!

				if (c1)
				{
					hs1.Remove(strValue);
					hs2.Remove(value);
				}
				else
				{
					hs1.Add(strValue);
					hs2.Add(value);
				}
			}
			Console.WriteLine("OK");
		}

		// バイト列 -> Hex文字列
		private string ToString(byte[] data)
		{
			return string.Join("", data.Select(v => v.ToString("x2")));
		}

		public class Adler32BytesIEC : IEqualityComparer<byte[]>
		{
			public bool Equals(byte[] a, byte[] b)
			{
				if (a.Length != b.Length)
					return false;

				for (int index = 0; index < a.Length; index++)
					if (a[index] != b[index])
						return false;

				return true;
			}

			public int GetHashCode(byte[] a)
			{
				return (int)Adler32.ComputeHash(a);
			}
		}

		// ====
		// Random
		// ====

		private static int GetRandInt(int modulo)
		{
			return (int)(GetULongRand() % (ulong)modulo);
		}

		private static byte[] GetRandBytes(int size)
		{
			byte[] data = new byte[size];
			Csprng.GetBytes(data);
			return data;
		}

		// ====
	}
}
