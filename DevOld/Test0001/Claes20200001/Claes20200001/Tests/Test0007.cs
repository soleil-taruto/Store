using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0007
	{
		public void Test01()
		{
			Test01_a(3000, 10);
			Test01_a(1000, 30);
			Test01_a(300, 100);
			Test01_a(100, 300);
			Test01_a(30, 1000);
			Test01_a(10, 3000);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int testCount, int scaleLength)
		{
			Test01_a2(testCount, scaleLength, 10);
			Test01_a2(testCount, scaleLength, 300);
			Test01_a2(testCount, scaleLength, 10000);
		}

		private void Test01_a2(int testCount, int scaleLength, int valueScale)
		{
			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				int[] values = Enumerable
					.Range(0, SCommon.CRandom.GetInt(scaleLength))
					.Select(dummy => SCommon.CRandom.GetInt(valueScale))
					.ToArray();

				Array.Sort(values, SCommon.Comp);

				// ====

				RangeInfo[] ranges = Grouping(values, SCommon.Comp).ToArray();

				Check(values, ranges);
			}
			Console.WriteLine("OK");
		}

		private void Check(int[] values, RangeInfo[] ranges)
		{
			foreach (RangeInfo range in ranges)
				if (range.Count < 1) // ? 空のグループ
					throw null;

			if (ranges.Select(range => range.Count).Sum() != values.Length) // ? 不正な長さ
				throw null;

			foreach (RangeInfo range in ranges)
				for (int index = 1; index < range.Count; index++)
					if (values[range.Start + index - 1] != values[range.Start + index]) // ? グループ内の不一致
						throw null;

			SCommon.ForEachPair(ranges, (a, b) =>
			{
				if (values[a.Start] == values[b.Start]) // ? 違うグループ間での一致
					throw null;
			});
		}

		// ====

		private class RangeInfo
		{
			/// <summary>
			/// 開始位置(この位置の要素を含む)
			/// </summary>
			public int Start;

			/// <summary>
			/// 終了位置(この位置の要素を含まない)
			/// </summary>
			public int End;

			/// <summary>
			/// 範囲内の要素の個数
			/// </summary>
			public int Count
			{
				get
				{
					return this.End - this.Start;
				}
			}
		}

		private IEnumerable<RangeInfo> Grouping<T>(IList<T> list, Comparison<T> comp)
		{
			int[] boundaries = GetBoundaries(list, comp).ToArray();

			for (int index = 1; index < boundaries.Length; index++)
			{
				yield return new RangeInfo()
				{
					Start = boundaries[index - 1],
					End = boundaries[index],
				};
			}
		}

		private IEnumerable<int> GetBoundaries<T>(IList<T> list, Comparison<T> comp)
		{
			if (1 <= list.Count)
			{
				yield return 0;

				for (int index = 1; index < list.Count; index++)
					if (comp(list[index - 1], list[index]) != 0)
						yield return index;

				yield return list.Count;
			}
		}
	}
}
