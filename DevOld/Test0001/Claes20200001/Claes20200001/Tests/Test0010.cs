using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Charlotte.Tests
{
	public class Test0010
	{
		public void Test01()
		{
			Test01_a(2);
			Test01_a(3);
			Test01_a(5);
			Test01_a(7);
			Test01_a(11);
			Test01_a(13);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int valueScale)
		{
			HashSet<string> hs1 = new HashSet<string>();
			HashSet<int[]> hs2 = new HashSet<int[]>(new IntsIEC());

			for (int testcnt = 0; testcnt < 300000; testcnt++)
			{
				int[] value = GetRandIntList(GetRandInt(valueScale));
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

		// Int配列 -> 文字列
		private static string ToString(int[] arr)
		{
			return string.Join("_", arr);
		}

		public class IntsIEC : IEqualityComparer<int[]>
		{
			public bool Equals(int[] a, int[] b)
			{
				if (a.Length != b.Length)
					return false;

				for (int index = 0; index < a.Length; index++)
					if (a[index] != b[index])
						return false;

				return true;
			}

			public int GetHashCode(int[] a)
			{
				// HACK: HachCode.Combine にこういう使い方が想定されているのか疑問。

				int digest = 0;

				foreach (int value in a)
					digest = HashCode.Combine(digest, value);

				return digest;
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

		private static int GetRandInt(int modulo)
		{
			return (int)(GetULongRand() % (ulong)modulo);
		}

		private static int[] GetRandIntList(int size)
		{
			int[] arr = new int[size];

			for (int index = 0; index < size; index++)
				arr[index] = (int)(uint)GetULongRand();

			return arr;
		}

		// ====

		// なんか HashCode 無いっぽいので、ダミーを実装しておく。
		// .NET Core 2.1 (2018) 以降にはあるっぽい。
		public static class HashCode
		{
			public static int Combine(int a, int b)
			{
				ulong x = (ulong)(uint)a;
				ulong y = (ulong)(uint)b;

				x = ((x << 32) | y) % 4294967291;

				return (int)(uint)x;
			}
		}
	}
}
