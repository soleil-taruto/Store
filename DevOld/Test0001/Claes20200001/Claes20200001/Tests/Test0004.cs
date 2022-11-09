using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0004
	{
		public void Test01()
		{
			for (int n = 1; n < 9998; n++)
			{
				Console.WriteLine(n + " --> " + Collatz(n));
			}
		}

		public void Test02()
		{
			Dictionary<int, int> dict = new Dictionary<int, int>();

			for (int n = 1; n < 1000000; n++)
			{
				//if (n % 1000 == 0) Console.WriteLine(n); // cout

				int c = Collatz(n);

				if (!dict.ContainsKey(c))
					dict[c] = n; // 最小値を記憶
			}

			foreach (int c in dict.Keys.OrderBy(SCommon.Comp))
			{
				Console.WriteLine(dict[c] + " --> " + c);
			}
		}

		public void Test03()
		{
			Dictionary<int, int[]> dict = new Dictionary<int, int[]>();

			for (int n = 1; n < 65535; n++)
			//for (int n = 1; n < 255; n++)
			//for (int n = 1; n < 1000000; n++)
			{
				//if (n % 1000 == 0) Console.WriteLine(n); // cout

				int c = Collatz(n);

				if (!dict.ContainsKey(c))
					dict[c] = new int[2];

				dict[c][0] = n;
				dict[c][1]++;
			}

			foreach (int c in dict.Keys.OrderBy(SCommon.Comp))
			{
				Console.WriteLine(dict[c][0] + " --> " + c + " (" + dict[c][1] + ")");
			}
		}

		private int Collatz(BigInteger n)
		{
			int c;

#if true // 割る２を何回行うか -> c
			c = 0;
			while (n % 2 == 0) { n /= 2; c++; }
			while (n != 1)
			{
				n = n * 3 + 1;
				while (n % 2 == 0) { n /= 2; c++; }
			}
#elif true // 奇数から次の奇数を求める処理を何回行うか -> c
			while (n % 2 == 0) n /= 2;
			for (c = 0; n != 1; c++)
			{
				n = n * 3 + 1;
				while (n % 2 == 0) n /= 2;
			}
#else // コラッツ関数を何回行うか -> c
			for (c = 0; n != 1; c++)
			{
				if (n % 2 == 1)
					n = n * 3 + 1;
				else
					n /= 2;
			}
#endif
			return c;
		}
	}
}
