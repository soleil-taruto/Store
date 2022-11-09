using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0009
	{
		public void Test01()
		{
			int[][] ans2 = Combination2(30)
				.ForEach(v => Array.Sort(v, SCommon.Comp))
				.OrderBy((a, b) => SCommon.Comp(a, b, SCommon.Comp))
				.ToArray();

			int[][] ans3 = Combination3(30)
				.ForEach(v => Array.Sort(v, SCommon.Comp))
				.OrderBy((a, b) => SCommon.Comp(a, b, SCommon.Comp))
				.ToArray();

			int[][] exp2 = Enumerable.Range(0, 30 * 30).Select(v => new int[] { v / 30, v % 30 })
				.Where(v => v[0] != v[1])
				.ForEach(v => Array.Sort(v, SCommon.Comp))
				.DistinctOrderBy((a, b) => SCommon.Comp(a, b, SCommon.Comp))
				.ToArray();

			int[][] exp3 = Enumerable.Range(0, 30 * 30 * 30).Select(v => new int[] { v / 900, (v % 900) / 30, v % 30 })
				.Where(v => v[0] != v[1] && v[1] != v[2] && v[2] != v[0])
				.ForEach(v => Array.Sort(v, SCommon.Comp))
				.DistinctOrderBy((a, b) => SCommon.Comp(a, b, SCommon.Comp))
				.ToArray();

			if (SCommon.Comp(ans2, exp2, (a, b) => SCommon.Comp(a, b, SCommon.Comp)) != 0)
				throw null;

			if (SCommon.Comp(ans3, exp3, (a, b) => SCommon.Comp(a, b, SCommon.Comp)) != 0)
				throw null;

			Console.WriteLine("OK!");
		}

		private int[][] Combination2(int count)
		{
			List<int[]> dest = new List<int[]>();

			for (int r = 1; r < count; r++)
			{
				for (int l = 0; l < r; l++)
				{
					dest.Add(new int[] { l, r });
				}
			}
			return dest.ToArray();
		}

		private int[][] Combination3(int count)
		{
			List<int[]> dest = new List<int[]>();

			for (int r = 2; r < count; r++)
			{
				for (int m = 1; m < r; m++)
				{
					for (int l = 0; l < m; l++)
					{
						dest.Add(new int[] { l, m, r });
					}
				}
			}
			return dest.ToArray();
		}
	}
}
