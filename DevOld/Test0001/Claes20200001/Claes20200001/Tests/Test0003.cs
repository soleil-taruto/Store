using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			Comparison<int> comp = (a, b) => a - b;

			// 空リスト
			{
				int[] list = new int[0];

				// ソート実行
				Array.Sort(list, comp);
				GnomeSort(list, comp);
				InsertionSort(list, comp);
				CombSort(list, comp);
			}

			for (int testcnt = 0; testcnt < 10000; testcnt++)
			{
				int count = SCommon.CRandom.GetRange(1, 100);
				int vallmt = SCommon.CRandom.GetRange(1, 100);
				int[] list = Enumerable.Range(0, count).Select(dummy => SCommon.CRandom.GetInt(vallmt)).ToArray();
				int[] list_01 = list.ToArray(); // copy
				int[] list_02 = list.ToArray(); // copy
				int[] list_03 = list.ToArray(); // copy
				int[] list_04 = list.ToArray(); // copy

				// ソート実行
				Array.Sort(list_01, comp);
				GnomeSort(list_02, comp);
				InsertionSort(list_03, comp);
				CombSort(list_04, comp);

				if (SCommon.Comp(list_01, list_02, comp) != 0)
					throw null;

				if (SCommon.Comp(list_01, list_03, comp) != 0)
					throw null;

				if (SCommon.Comp(list_01, list_04, comp) != 0)
					throw null;
			}
			Console.WriteLine("OK!");
		}

		public void Test02()
		{
			Comparison<int> comp = (a, b) => a - b;

			foreach (int count in new int[] { 10000, 30000, 100000, 300000, 1000000, 3000000 })
			{
				foreach (int vallmt in new int[] { count / 100, count, count * 100 })
				{
					for (int testcnt = 0; testcnt < 3; testcnt++)
					{
						int[] list = Enumerable.Range(0, count).Select(dummy => SCommon.CRandom.GetInt(vallmt)).ToArray();
						int[] list_01 = list.ToArray(); // copy
						int[] list_02 = list.ToArray(); // copy

						DateTime t1 = DateTime.Now;
						Array.Sort(list_01, comp);
						DateTime t2 = DateTime.Now;
						CombSort(list_02, comp);
						DateTime t3 = DateTime.Now;

						Console.WriteLine("count, vallmt: " + count + ", " + vallmt);
						Console.WriteLine("STD-Sort: " + (t2 - t1).TotalMilliseconds);
						Console.WriteLine("CombSort: " + (t3 - t2).TotalMilliseconds);
						Console.WriteLine("CombSort time / STD-Sort time: " + ((t3 - t2).TotalMilliseconds / (t2 - t1).TotalMilliseconds).ToString("F3"));
					}
				}
			}
		}

		// ====
		// ソート
		// ====

		private static void GnomeSort<T>(IList<T> list, Comparison<T> comp)
		{
			for (int index = 1; index < list.Count; )
			{
				if (comp(list[index - 1], list[index]) > 0) // ? 逆順
				{
					// 入れ替え
					T tmp = list[index - 1];
					list[index - 1] = list[index];
					list[index] = tmp;

					if (index == 1) // ? 先頭を入れ替えた
						index++;
					else
						index--;
				}
				else // ? 正順
				{
					index++;
				}
			}
		}

		private static void InsertionSort<T>(IList<T> list, Comparison<T> comp)
		{
			for (int index = 1; index < list.Count; index++)
			{
				if (comp(list[index - 1], list[index]) > 0) // ? 逆順
				{
					T tmp = list[index];

					int ndx = index;
					do
					{
						list[ndx] = list[ndx - 1];
						ndx--;
					}
					while (0 < ndx && comp(list[ndx - 1], tmp) > 0); // ? 先頭ではない && 逆順

					list[ndx] = tmp;
				}
			}
		}

		private static void CombSort<T>(IList<T> list, Comparison<T> comp)
		{
			for (int h = list.Count; ; )
			{
				h = (int)(h / 1.3);

				if (h < 7) // しきい値 Test0004.Test01 より @ 2022.3.17
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
