using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0005
	{
		public void Test01()
		{
			foreach (int countScale in new int[] { 100, 300, 1000, 3000, 10000 })
			{
				foreach (int limitScale in new int[] { 100, 10000, 1000000 })
				{
					Console.WriteLine(countScale + ", " + limitScale);

					for (int testcnt = 0; testcnt < 1000; testcnt++)
					{
						Test01_a(countScale, limitScale);
					}
					Console.WriteLine("OK");
				}
			}
			Console.WriteLine("OK!");
		}

		private void Test01_a(int countScale, int limitScale)
		{
			int[] arr = GetRandInts(countScale, limitScale);

			Array.Sort(arr, (a, b) => a - b);

			// 範囲外の存在しない値(前方)
			Test01_b(arr, -1);

			// 範囲外の存在しない値(後方)
			Test01_b(arr, SCommon.IMAX);

			if (1 <= arr.Length)
			{
				int target = SCommon.CRandom.ChooseOne(arr);

				// 範囲内の存在する値
				Test01_b(arr, target);

				for (int index = 0; index < arr.Length; index++)
					if (arr[index] >= target)
						arr[index]++;

				// 範囲内の存在しない値
				Test01_b(arr, target);
			}
		}

		private void Test01_b(int[] arr, int target)
		{
			int[] range1 = GetOuterRange_v1(arr, target, SCommon.Comp);
			int[] range2 = GetOuterRange_v2(arr, target, SCommon.Comp);

			if (SCommon.Comp(range1, range2, SCommon.Comp) != 0) // ? 不一致
				throw null; // BUG !!!
		}

		private int[] GetOuterRange_v1<T>(T[] arr, T target, Comparison<T> comp)
		{
			int l = -1;
			int r = arr.Length;

			while (l + 1 < r)
			{
				int m = (l + r) / 2;
				int ret = comp(arr[m], target);

				if (ret < 0)
				{
					l = m;
				}
				else if (ret > 0)
				{
					r = m;
				}
				else
				{
					l = GetBorder(arr, v => comp(v, target) != 0, l, m);
					r = GetBorder(arr, v => comp(v, target) == 0, m, r) + 1;
					break;
				}
			}
			return new int[] { l, r };
		}

		private int GetBorder<T>(T[] arr, Predicate<T> matchForLeft, int l, int r)
		{
			while (l + 1 < r)
			{
				int m = (l + r) / 2;

				if (matchForLeft(arr[m]))
				{
					l = m;
				}
				else
				{
					r = m;
				}
			}
			return l;
		}

		private int[] GetOuterRange_v2<T>(T[] arr, T target, Comparison<T> comp)
		{
			int l;
			int r;

			for (l = arr.Length - 1; 0 <= l; l--)
				if (comp(arr[l], target) < 0)
					break;

			for (r = 0; r < arr.Length; r++)
				if (comp(arr[r], target) > 0)
					break;

			return new int[] { l, r };
		}

		private int[] GetRandInts(int countScale, int limitScale)
		{
			int count = SCommon.CRandom.GetInt(countScale);
			int limit = SCommon.CRandom.GetInt(limitScale) + 1;

			return Enumerable.Range(1, count).Select(dummy => SCommon.CRandom.GetInt(limit)).ToArray();
		}
	}
}
