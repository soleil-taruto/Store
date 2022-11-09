using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0004
	{
		// hBdr == 7 のとき最速となる。@ 2022.3.17

		public void Test01()
		{
			Comparison<int> comp = (a, b) => a - b;

			//for (int hBdr = 2; hBdr <= 20; hBdr++)
			//for (int hBdr = 5; hBdr <= 9; hBdr++)
			for (int hBdr = 6; hBdr <= 8; hBdr++)
			{
				DateTime stTm = DateTime.Now;

				for (int testcnt = 0; testcnt < 30000; testcnt++)
				{
					int scale = 1000;
					//int scale = 3000;

					int count = SCommon.CRandom.GetRange(1, scale);
					int vallmt = SCommon.CRandom.GetRange(1, scale);
					int[] list = Enumerable.Range(0, count).Select(dummy => SCommon.CRandom.GetInt(vallmt)).ToArray();

					CombSort(list, comp, hBdr);
				}
				DateTime edTm = DateTime.Now;

				Console.WriteLine(hBdr + " -> " + (edTm - stTm).TotalMilliseconds.ToString("F3"));
			}
		}

		// ====
		// ソート
		// ====

		private static void CombSort<T>(IList<T> list, Comparison<T> comp, int hBdr)
		{
			for (int h = list.Count; ; )
			{
				h = (int)(h / 1.3);

				if (h < hBdr)
					break;

				for (int index = h; index < list.Count; index++)
				{
					if (comp(list[index - h], list[index]) > 0) // ? 逆順
					{
						// 入れ替え
						T tmp = list[index - h];
						list[index - h] = list[index];
						list[index] = tmp;
					}
				}
			}
			for (int h = 1; h < list.Count; h++)
			{
				for (int index = h; 0 < index; index--)
				{
					if (comp(list[index - 1], list[index]) > 0) // ? 逆順
					{
						// 入れ替え
						T tmp = list[index - 1];
						list[index - 1] = list[index];
						list[index] = tmp;
					}
					else // ? 正順
					{
						break;
					}
				}
			}
		}
	}
}
