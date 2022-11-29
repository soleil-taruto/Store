using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			for (int testcnt = 0; testcnt < 30; testcnt++)
			{
				//int[] arr1 = GetRandIntList(100).ToArray();
				//int[] arr1 = GetRandIntList(1000).ToArray();
				int[] arr1 = GetRandIntList(10000).ToArray();
				int[] arr2 = arr1.ToArray(); // Cloning

				Array.Sort(arr1, (a, b) => a - b);

				arr2 = MergeSort(arr2, (a, b) => a - b).ToArray();

				for (int index = 0; index < arr1.Length; index++)
					if (arr1[index] != arr2[index])
						throw null;

				//Console.WriteLine("OK");
			}
			Console.WriteLine("OK!");
		}

		private static IEnumerable<T> MergeSort<T>(IEnumerable<T> list, Comparison<T> comp)
		{
			Queue<IEnumerable<T>> q = new Queue<IEnumerable<T>>(list.Select(v => new T[] { v }));

			if (q.Count == 0)
				return new T[0];

			while (2 <= q.Count)
				q.Enqueue(E_Merge(q.Dequeue(), q.Dequeue(), comp));

			return q.Dequeue();
		}

		private static IEnumerable<T> E_Merge<T>(IEnumerable<T> v1, IEnumerable<T> v2, Comparison<T> comp)
		{
			IEnumerator<T> a = v1.GetEnumerator();
			IEnumerator<T> b = v2.GetEnumerator();

			if (!a.MoveNext()) throw null;
			if (!b.MoveNext()) throw null;

			for (; ; )
			{
				int ret = comp(a.Current, b.Current);

				if (ret <= 0)
				{
					yield return a.Current;
					if (!a.MoveNext()) { a = b; break; }
				}
				if (0 <= ret)
				{
					yield return b.Current;
					if (!b.MoveNext()) break;
				}
			}
			do { yield return a.Current; } while (a.MoveNext());
		}

		// ====
		// Random
		// ====

		private static RandomNumberGenerator Csprng = new RNGCryptoServiceProvider();

		private static uint GetRandUInt()
		{
			byte[] data = new byte[4];
			uint value = 0;

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
			return (int)(GetRandUInt() % (uint)modulo);
		}

		private static IEnumerable<int> GetRandIntList(int scale)
		{
			int count = GetRandInt(scale);
			int limit = GetRandInt(scale) + 1;

			for (int index = 0; index < count; index++)
				yield return GetRandInt(limit);
		}

		// ====

		public void Test02()
		{
			for (int testcnt = 0; testcnt < 100; testcnt++)
			{
				var arr = GetRandIntList(60)
					.ToArray().Skip(0); // リストを確定する。

				Console.WriteLine("Before: " + string.Join(", ", arr));

				arr = MergeSort(arr, (a, b) => a - b);

				Console.WriteLine("After : " + string.Join(", ", arr));
			}
		}
	}
}
