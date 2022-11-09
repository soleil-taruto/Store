using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			Dictionary<string, int> counters = SCommon.CreateDictionary<int>();

			for (int testcnt = 0; testcnt < 1000000; testcnt++)
			{
				ulong a = SCommon.CRandom.GetULong();
				ulong b = SCommon.CRandom.GetULong();
				ulong m = SCommon.CRandom.GetULong();

				if (m == 0)
					continue;

				ModAdd64(a, b, m);

				{
					string k = "" + LoopCount;

					if (counters.ContainsKey(k))
						counters[k]++;
					else
						counters[k] = 1;
				}
			}

			foreach (string k in counters.Keys.OrderBy((a, b) => int.Parse(a) - int.Parse(b)))
				Console.WriteLine(k + " --> " + counters[k]);
		}

		private static int LoopCount;

		private static ulong ModAdd64(ulong a, ulong b, ulong m)
		{
			ulong r = (ulong.MaxValue % m + 1) % m;

			LoopCount = 0;

			while (ulong.MaxValue - a < b)
			{
				unchecked { a += b; }
				b = r;

				LoopCount++;
			}
			return (a + b) % m;
		}
	}
}
