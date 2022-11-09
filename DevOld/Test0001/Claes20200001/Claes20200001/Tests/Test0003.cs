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
			foreach (int a in new int[] { 10, 30, 100, 300, 1000 })
			{
				foreach (int b in new int[] { 10, 30, 100, 300, 1000 })
				{
					Console.WriteLine(a + ", " + b); // cout

					for (int testcnt = 0; testcnt < 1000; testcnt++)
					{
						if (testcnt % 100 == 0) Console.WriteLine(a + ", " + b + ", " + testcnt); // cout

						Test01_a(a, b);
					}
				}
			}
			Console.WriteLine("OK!");
		}

		private void Test01_a(int valScale, int numScale)
		{
			string[] loList = MakeRandStrings(valScale, numScale, "_LO");
			string[] beList = MakeRandStrings(valScale, numScale, "_BE");
			string[] roList = MakeRandStrings(valScale, numScale, "_RO");

			Array.Sort(loList, SCommon.Comp);
			Array.Sort(beList, SCommon.Comp);
			Array.Sort(roList, SCommon.Comp);

			string[] lList = beList.Concat(loList).ToArray();
			string[] rList = beList.Concat(roList).ToArray();

			SCommon.CRandom.Shuffle(lList);
			SCommon.CRandom.Shuffle(rList);

			List<string> ansLOList = new List<string>();
			List<string> ansBEList1 = new List<string>();
			List<string> ansBEList2 = new List<string>();
			List<string> ansROList = new List<string>();

			SCommon.Merge(lList, rList, SCommon.Comp, ansLOList, ansBEList1, ansBEList2, ansROList);

			if (SCommon.Comp(loList, ansLOList, SCommon.Comp) != 0)
				throw null;

			if (SCommon.Comp(beList, ansBEList1, SCommon.Comp) != 0)
				throw null;

			if (SCommon.Comp(beList, ansBEList2, SCommon.Comp) != 0)
				throw null;

			if (SCommon.Comp(roList, ansROList, SCommon.Comp) != 0)
				throw null;
		}

		private string[] MakeRandStrings(int valScale, int numScale, string suffix)
		{
			return Enumerable.Range(0, SCommon.CRandom.GetInt(numScale))
				.Select(dummy => SCommon.CRandom.GetInt(valScale).ToString("D3") + suffix)
				.ToArray();
		}

		public void Test02()
		{
			// 配列
			{
				string[] arr1 = new string[] { "CCC", "BBB", "AAA" };
				string[] arr2 = new string[] { "789", "456", "123" };

				List<string> only1 = new List<string>();
				List<string> both1 = new List<string>();
				List<string> both2 = new List<string>();
				List<string> only2 = new List<string>();

				SCommon.Merge(arr1, arr2, SCommon.Comp, only1, both1, both2, only2);
			}

			// リスト
			{
				List<string> list1 = new string[] { "CCC", "BBB", "AAA" }.ToList();
				List<string> list2 = new string[] { "789", "456", "123" }.ToList();

				List<string> only1 = new List<string>();
				List<string> both1 = new List<string>();
				List<string> both2 = new List<string>();
				List<string> only2 = new List<string>();

				SCommon.Merge(list1, list2, SCommon.Comp, only1, both1, both2, only2);
			}

			Console.WriteLine("OK!");
		}
	}
}
