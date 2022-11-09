using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0008
	{
		//private const int N_MAX = 255;
		//private const int N_MAX = 65535;
		private const int N_MAX = 16777215;

		public void Test01()
		{
			ProcMain.WriteLog("ST"); // cout

			int[] n2dc = Enumerable.Range(0, N_MAX + 1).Select(dummy => -1).ToArray();
			Queue<int> odds = new Queue<int>();

			n2dc[1] = 0;
			n2dc[2] = 1;
			n2dc[4] = 2;

			odds.Enqueue(4);

			while (1 <= odds.Count)
			{
				for (int c = odds.Dequeue() * 2; c <= N_MAX; c *= 2)
				{
					if (n2dc[c] != -1) throw null; // test
					n2dc[c] = n2dc[c / 2] + 1;

					if ((c - 1) % 3 == 0)
					{
						int d = (c - 1) / 3;
						if (n2dc[d] != -1) throw null; // test
						n2dc[d] = n2dc[c];
						odds.Enqueue(d);
					}
				}
			}

			ProcMain.WriteLog("M1"); // cout

			for (int n = 1; n <= N_MAX; n++)
			{
				if (n2dc[n] == -1)
				{
					n2dc[n] = GetCollatzDivideCount(n, n2dc);

					for (int c = n * 2; c <= N_MAX; c *= 2)
					{
						if (n2dc[c] != -1) throw null; // test
						n2dc[c] = n2dc[c / 2] + 1;
					}
				}
			}

			ProcMain.WriteLog("ED"); // cout

			{
				int c;
				for (c = 1; c <= N_MAX && n2dc[c] != -1; c++) ;
				Console.WriteLine(c);
			}

			{
				int x = 0;

				for (int c = 1; c <= N_MAX; c++)
					if (n2dc[c] == -1)
						x++;

				Console.WriteLine(x);
			}
		}

		private static int GetCollatzDivideCount(BigInteger n, int[] n2dc)
		{
			int dc = 0;

			while (n != 1)
			{
				if (n < n2dc.Length && n2dc[(int)n] != -1)
					return dc + n2dc[(int)n];

				if (n % 2 == 0)
				{
					n /= 2;
					dc++;
				}
				else
				{
					n *= 3;
					n++;
				}
			}
			return dc;
		}
	}
}
