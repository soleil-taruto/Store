using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0006
	{
		public void Test01()
		{
			Test01_a(1);
			Test01_a(2);
			Test01_a(5);
			Test01_a(10);
			Test01_a(20);
			Test01_a(50);
		}

		private void Test01_a(int div)
		{
			const int TEST_COUNT = 5;
			double total = 0.0;

			for (int testcnt = 0; testcnt < TEST_COUNT; testcnt++)
				total += Test01_a2(div);

			Console.WriteLine(div + " ==> " + (total / TEST_COUNT).ToString("F9"));
		}

		private double Test01_a2(int div)
		{
			double l = 0.0;
			double r = 1.0;

			for (int c = 0; c < 50; c++)
			{
				double m = (l + r) / 2;
				double rate = Test01_b(m, div * 3);

				if (rate < 0.95)
				{
					l = m;
				}
				else
				{
					r = m;
				}
			}

			{
				double m = (l + r) / 2;
				double rate = Test01_b(m, div);

				//Console.WriteLine(div + " ==> " + rate.ToString("F9"));

				return rate;
			}
		}

		private double Test01_b(double rateOnce, int count)
		{
			const int TEST_COUNT = 10000;
			int p = 0;

			for (int testcnt = 0; testcnt < TEST_COUNT; testcnt++)
			{
				for (int c = 0; c < count; c++)
				{
					if (SCommon.CRandom.GetReal1() < rateOnce)
					{
						p++;
						break;
					}
				}
			}
			return (double)p / TEST_COUNT;
		}
	}
}
