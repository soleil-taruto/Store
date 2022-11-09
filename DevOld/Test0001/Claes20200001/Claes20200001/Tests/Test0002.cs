using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0002
	{
		public void Test01()
		{
			Test01_a(100);
			Test01_a(10000);
			Test01_a(1000000);
			Test01_a(100000000);
			Test01_a(300000000); // Release で 0:30くらい @ 2022.5.6
			//Test01_a(1000000000); // Release で 1:30くらい 100MBくらい使用 @ 2022.5.6
			//Test01_a(3000000000); // Release で 4:30くらい 300MBくらい使用 @ 2022.5.6
		}

		private void Test01_a(long limit)
		{
			ProcMain.WriteLog("ST " + limit);

			CompositeList composites = new CompositeList();

			for (long numb = 3; numb < limit; numb += 2)
			{
				if (!composites[numb])
				{
					if (limit / numb <= numb)
					{
						continue;
					}
					for (long n = numb * numb; n < limit; n += numb * 2)
					{
						composites[n] = true;
					}

					if (numb < 20)
						ProcMain.WriteLog("C-done " + numb);
				}
			}

			ProcMain.WriteLog("CNT");

			long count = 0;

			for (long numb = 2; numb < limit; numb++)
			{
				if (!composites[numb])
				{
					count++;
				}
			}

			ProcMain.WriteLog("CNT-done");

			Console.WriteLine(limit + " ==> " + count);
		}

		private class CompositeList
		{
			private BitList Inner = new BitList();

			public bool this[long index]
			{
				get
				{
					if (index <= 1)
						return true;

					if (index == 2)
						return false;

					if (index % 2 == 0)
						return true;

					return this.Inner[index / 2 - 1];
				}

				set
				{
					if (index <= 2)
						throw null; // dont

					if (index % 2 == 0)
						throw null; // dont

					this.Inner[index / 2 - 1] = value;
				}
			}
		}
	}
}
