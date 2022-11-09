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
			for (double sps = 0.00001; sps <= 0.001; sps += 0.00001)
			{
				Test01_a(sps);
			}
		}

		private void Test01_a(double sps)
		{
			double m = 0;
			double s = 1.0;

			for (int t = 0; t < 3600; t++)
			{
				m += 1.0 / s;
				s += sps;
			}
			double m1 = m;

			for (int t = 0; t < 3600; t++)
			{
				m += 1.0 / s;
				s += sps;
			}
			double m2 = m;

			Console.WriteLine(sps.ToString("F5") + " ==> " + (m2 / m1).ToString("F3"));
		}

		public void Test02()
		{
			double l = 0.0;
			double r = 0.001;

			for (int c = 0; c < 100; c++)
			{
				double m = (l + r) / 2;
				double rate = Test02_a(m);

				if (1.5 < rate)
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
				double s = 1.0 / m;

				Console.WriteLine(s.ToString("F9"));

				DateTime t = new DateTime(2022, 1, 1, 12, 0, 0);
				t -= TimeSpan.FromSeconds(s);

				Console.WriteLine(t.ToString("yyyy/MM/dd HH:mm:ss.fff"));
			}
		}

		private double Test02_a(double sps)
		{
			double m = 0;
			double s = 1.0;

			for (int t = 0; t < 3600; t++)
			{
				m += 1.0 / s;
				s += sps;
			}
			double m1 = m;

			for (int t = 0; t < 3600; t++)
			{
				m += 1.0 / s;
				s += sps;
			}
			double m2 = m;

			return m2 / m1;
		}

		public void Test03()
		{
			Test03_a(1);
			Test03_a(3);
			Test03_a(10);
			Test03_a(30);
			Test03_a(100);
			Test03_a(300);
			Test03_a(1000);
			Test03_a(3000);
			Test03_a(10000);
			Test03_a(30000);
		}

		public void Test03_a(int secDenom)
		{
			double l = 0.0;
			double r = 0.001;

			for (int c = 0; c < 100; c++)
			{
				double m = (l + r) / 2;
				double rate = Test03_b(m, secDenom);

				if (1.5 < rate)
				{
					l = m;
				}
				else
				{
					r = m;
				}
				Console.Write(".");
			}
			Console.WriteLine("");

			{
				double m = (l + r) / 2;
				double s = 1.0 / m;
				s /= secDenom;

				Console.WriteLine(s.ToString("F9"));

				DateTime t = new DateTime(2022, 1, 1, 12, 0, 0);
				t -= TimeSpan.FromSeconds(s);

				Console.WriteLine(secDenom + " --> " + t.ToString("yyyy/MM/dd HH:mm:ss.fff"));
			}
		}

		private double Test03_b(double sps, int secDenom)
		{
			double m = 0;
			double s = 1.0;

			for (int t = 0; t < 3600 * secDenom; t++)
			{
				m += 1.0 / s;
				s += sps;
			}
			double m1 = m;

			for (int t = 0; t < 3600 * secDenom; t++)
			{
				m += 1.0 / s;
				s += sps;
			}
			double m2 = m;

			return m2 / m1;
		}

		public void Test04()
		{
			const int TEST_COUNT = 10;
			double total = 0.0;

			for (int testcnt = 0; testcnt < TEST_COUNT; testcnt++)
			{
				total += Test04_a();
			}
			Console.WriteLine((total / TEST_COUNT).ToString("F9"));
		}

		public double Test04_a()
		{
			double l = 0.0;
			double r = 1.0;

			for (int c = 0; c < 20; c++)
			{
				double m = (l + r) / 2;
				double rate = Test04_b(m, 1800);

				if (rate < 0.95)
				{
					l = m;
				}
				else
				{
					r = m;
				}
				Console.Write(".");
			}
			Console.WriteLine("");

			{
				double m = (l + r) / 2;
				double rate = Test04_b(m, 600);

				Console.WriteLine(rate.ToString("F9"));

				return rate;
			}
		}

		private double Test04_b(double sps, int sec)
		{
			//const int TEST_COUNT = 100;
			//const int TEST_COUNT = 1000;
			const int TEST_COUNT = 10000;
			int p = 0;

			for (int testcnt = 0; testcnt < TEST_COUNT; testcnt++)
			{
				for (int t = 0; t < sec; t++)
				{
					if (SCommon.CRandom.GetReal1() < sps)
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
