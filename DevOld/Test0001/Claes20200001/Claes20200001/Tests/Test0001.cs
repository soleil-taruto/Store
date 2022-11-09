using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;
using System.IO;

namespace Charlotte.Tests
{
	public class Test0001
	{
		/// <summary>
		/// フェルマーの小定理
		/// p == 素数
		/// a == 0 以上 p 未満の整数
		/// のとき
		/// (a ^ p) % p == a となる。
		/// </summary>
		public void Test01()
		{
			for (int p = 2; p < 2000; p++) // rough limit
			{
				if (IsPrime(p))
				{
					Console.WriteLine("p: " + p); // cout

					for (int a = 0; a < p; a++)
					{
						int v = 1;

						for (int c = 0; c < p; c++)
						{
							v *= a;
							v %= p;
						}

						// (a ^ p) % p == a となるはず！

						if (v != a)
							throw null;
					}
				}
			}
		}

		private static bool IsPrime(int v)
		{
			for (int c = 2; c < v; c++)
			{
				if (v % c == 0)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 正規分布の曲線
		/// </summary>
		public void Test02()
		{
			double[] map = new double[] { 1.0 };

			for (int c = 0; c < 3000; c++) // rough limit
			{
				double[] next = new double[map.Length + 1];

				for (int i = 0; i < map.Length; i++)
				{
					next[i + 0] += map[i] * 0.5;
					next[i + 1] += map[i] * 0.5;
				}
				map = next;

				// 両端の小さい値を除去
				while (map[0] < SCommon.MICRO) map = map.Skip(1).ToArray();
				while (map[map.Length - 1] < SCommon.MICRO) map = map.Take(map.Length - 1).ToArray();
			}

			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				foreach (double v in map)
				{
					writer.WriteCell(v.ToString("F9"));
					writer.EndRow();
				}
			}
		}

		/// <summary>
		/// 正規分布の曲線
		/// </summary>
		public void Test03()
		{
			const int SPAN = 50;
			int[] map = new int[SPAN * 2 + 1];

			for (int c = 0; c < 1000000; c++) // rough limit
			{
				if (c % 10000 == 0) Console.WriteLine(c); // cout

				int v = SPAN;

				for (int i = 0; i < SPAN; i++)
					v += SCommon.CRandom.GetInt(2) * 2 - 1;

				map[v]++;
			}

			// 両端の小さい値を除去
			// 注意：SPAN-によってインデックスが偶数または奇数の位置が 0 になる。
			map = map.Where(v => v != 0).ToArray();

			using (CsvFileWriter writer = new CsvFileWriter(Common.NextOutputPath() + ".csv"))
			{
				foreach (double v in map)
				{
					writer.WriteCell(v.ToString("F9"));
					writer.EndRow();
				}
			}
		}

		public void Test04()
		{
			for (int testcnt = 0; testcnt < 100; testcnt++)
			{
				byte[][] parts = MakeParts().ToArray();
				byte[][] destParts;

				using (WorkingDir wd = new WorkingDir())
				{
					string file = wd.MakePath();

					Test04_Write(file, parts);
					destParts = Test04_Read(file);
				}

				if (SCommon.Comp(parts, destParts, SCommon.Comp) != 0) // ? 不一致
					throw null; // 想定外
			}

			Console.WriteLine("OK!");
		}

		private void Test04_Write(string file, byte[][] parts)
		{
			using (FileStream writer = new FileStream(file, FileMode.Create, FileAccess.Write))
			{
				SCommon.WritePartInt(writer, parts.Length);

				foreach (byte[] part in parts)
					SCommon.WritePart(writer, part);
			}
		}

		private byte[][] Test04_Read(string file)
		{
			using (FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				byte[][] dest = new byte[SCommon.ReadPartInt(reader)][];

				for (int index = 0; index < dest.Length; index++)
					dest[index] = SCommon.ReadPart(reader);

				return dest;
			}
		}

		private IEnumerable<byte[]> MakeParts()
		{
			int count = SCommon.CRandom.GetInt(20);

			for (int index = 0; index < count; index++)
			{
				yield return SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(300));
			}
		}

		public void Test05()
		{
			Test06_a("ABCDABCD".ToArray(), chr => chr == 'C', 0, 2);
			Test06_a("ABCDABCD".ToArray(), chr => chr == 'C', 2, 2);
			Test06_a("ABCDABCD".ToArray(), chr => chr == 'C', 3, 6);

			Console.WriteLine("OK!");
		}

		private void Test06_a<T>(T[] arr, Predicate<T> match, int startIndex, int assume)
		{
			int ans = SCommon.IndexOf(arr, match, startIndex);

			if (ans != assume)
				throw null; // 想定外
		}

		public void Test06()
		{
			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				// int
				{
					int value = SCommon.CRandom.GetInt(SCommon.IMAX) * SCommon.CRandom.ChooseOne(new int[] { -1, 1 });
					byte[] data = SCommon.IntToBytes(value);
					int ans = SCommon.ToInt(data);

					if (value != ans)
						throw null; // 想定外
				}

				// long
				{
					long value = SCommon.CRandom.GetLong(SCommon.IMAX_64) * SCommon.CRandom.ChooseOne(new int[] { -1, 1 });
					byte[] data = SCommon.LongToBytes(value);
					long ans = SCommon.ToLong(data);

					if (value != ans)
						throw null; // 想定外
				}
			}

			Console.WriteLine("OK!");
		}

		public void Test07()
		{
			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				byte[][] src = Enumerable.Range(0, SCommon.CRandom.GetInt(100))
					.Select(dummy => SCommon.CRandom.GetBytes(SCommon.CRandom.GetInt(100)))
					.ToArray();

				byte[] enc = SCommon.SplittableJoin(src);
				byte[][] dec = SCommon.Split(enc);

				if (SCommon.Comp(src, dec, SCommon.Comp) != 0) // ? 不一致
					throw null; // 想定外
			}

			Console.WriteLine("OK!");
		}
	}
}
