using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0007
	{
		//private const int N_MAX = 255;
		//private const int N_MAX = 65535;
		private const int N_MAX = 16777215;

		public void Test01()
		{
			Dictionary<int, int> dc2n = new Dictionary<int, int>();
			Dictionary<int, int> n2dc = new Dictionary<int, int>();

			for (int n = 1; n <= N_MAX; n++)
			{
				int dc = GetCollatzDivideCount(n);

				if (dc2n.ContainsKey(dc))
					dc2n[dc] = -1;
				else
					dc2n[dc] = n;
			}
			foreach (int dc in dc2n.Keys)
			{
				int n = dc2n[dc];

				if (n != -1)
					n2dc[n] = dc;
			}

			Console.WriteLine(string.Format("1 以上 {0} 以下の整数において、それを初期値としたコラッツ数列が最初に 1 に達するまでに「割る2」を行う回数が唯一のもの：", N_MAX));

			foreach (int n in n2dc.Keys.OrderBy(v => v))
				Console.WriteLine(string.Format("初期値 {0} のとき割る2を {1} 回行う。割る2を行う回数が {2} 回となる初期値は {3} 以外に(この範囲には)ない。", n, n2dc[n], n2dc[n], n));
		}

		private static int GetCollatzDivideCount(long n)
		{
			int dc = 0;

			while (n != 1)
			{
				if (n % 2 == 0)
				{
					n /= 2;
					dc++;
				}
				else
				{
					if ((long.MaxValue - 1) / 3 < n)
						throw new OverflowException();

					n *= 3;
					n++;
				}
			}
			return dc;
		}
	}
}
