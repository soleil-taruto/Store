using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			//Test01_a(100000, 20, 1000);
			//Test01_a(2000, 2000, 1000000);
			//Test01_a(30, 200000, 1000000000);

			Test01_a(100000, 30, 30);
			Test01_a(10000, 200, 200);
			Test01_a(1000, 1000, 1000);
			Test01_a(200, 2000, 2000);
			Test01_a(30, 3000, 3000);

			Console.WriteLine("OK!");
		}

		private void Test01_a(int testCount, int nMax, int aMax)
		{
			for (int testcnt = 0; testcnt < testCount; testcnt++)
			{
				int[] aArr = Enumerable.Range(0, SCommon.CRandom.GetRange(2, nMax)).Select(dummy => SCommon.CRandom.GetRange(0, aMax)).ToArray();

				int ans1 = Test01_b1(aArr);
				int ans2 = Test01_b2(aArr);

				if (ans1 != ans2)
					throw null;
			}
			Console.WriteLine("OK");
		}

		private int Test01_b1(int[] aArr)
		{
			int aMin = aArr.Min();

			Queue<int> q = new Queue<int>(aArr.Select(a => a - aMin).Where(a => 1 <= a));

			if (q.Count == 0)
				return 1;

			while (2 <= q.Count)
			{
				int a = q.Dequeue();
				int b = q.Dequeue();

				int gcd = GetGCD(a, b);

				q.Enqueue(gcd);
			}

			{
				int gcd = q.Dequeue();

				return 2 <= gcd ? 1 : 2;
			}
		}

		private int GetGCD(int a, int b)
		{
			for (; ; )
			{
				int t = a % b;

				if (t == 0)
					return b;

				a = b;
				b = t;
			}
		}

		private int Test01_b2(int[] aArr)
		{
			int aMax = aArr.Max();
			int kindMin = int.MaxValue;

			for (int m = 2; m <= Math.Max(2, aMax + 1); m++)
			{
				bool[] modMap = new bool[aMax + 1];
				int kind = 0;

				foreach (int a in aArr)
				{
					int mod = a % m;

					if (!modMap[mod])
					{
						modMap[mod] = true;
						kind++;
					}
				}
				kindMin = Math.Min(kindMin, kind);
			}
			return kindMin;
		}
	}
}
