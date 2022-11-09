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
			for (int c = 0; c < 20; c++)
			{
				Test01_a();
			}
		}

		public void Test01_a()
		{
			const int TEST_COUNT = 10000;
			int p = 0;

			for (int testcnt = 0; testcnt < TEST_COUNT; testcnt++)
			{
				if (Test01_b())
				{
					p++;
				}
			}
			Console.WriteLine(((double)p / TEST_COUNT).ToString("F9"));
		}

		private bool Test01_b()
		{
			bool[] seats = new bool[100];

			seats[SCommon.CRandom.GetInt(100)] = true;

			for (int i = 1; i < 99; i++)
			{
				if (seats[i])
				{
					int r;

					do
					{
						r = SCommon.CRandom.GetInt(100);
					}
					while (seats[r]);

					seats[r] = true;
				}
				else
				{
					seats[i] = true;
				}
			}
			return !seats[99];
		}

		public void Test02()
		{
			double l = 1.0;
			double r = 2.0;

			//while (SCommon.MICRO < (r - l))
			//for (int c = 0; c < 30; c++)
			//for (int c = 0; c < 100; c++)
			for (int c = 0; c < 300; c++)
			{
				double m = (l + r) / 2;

				if (m * m < 2.0)
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

				Console.WriteLine(m.ToString("F20"));
			}
		}

		public void Test03()
		{
			double SPEED = 0.0001;

			for (double rate = -0.9; rate < 0.9; rate += 0.01)
			{
				Test03_a(SPEED, SPEED * rate);
			}
		}

		private void Test03_a(double speed, double wind)
		{
			double t = 0.0;

			t += 1.0 / (speed + wind);
			t += 1.0 / (speed - wind);

			Console.WriteLine(speed.ToString("F6") + " + " + wind.ToString("F6") + " ==> " + t.ToString("F9"));
		}

		public void Test04()
		{
			Console.WriteLine(1.0 / SCommon.MICRO);
			Console.WriteLine(1.0 / 1E-200);
			Console.WriteLine(1E+200 / 1E-200);

			double inf = 1E+200 / 1E-200;

			Console.WriteLine(inf < 0.0); // False
			Console.WriteLine(inf > 0.0); // True
			Console.WriteLine(inf < SCommon.IMAX); // False
			Console.WriteLine(inf > SCommon.IMAX); // True
			Console.WriteLine(inf < 1E+200); // False
			Console.WriteLine(inf > 1E+200); // True
			Console.WriteLine(0.0 < 1E+200); // True
			Console.WriteLine(0.0 > 1E+200); // False
		}

		public void Test05()
		{
			string version = typeof(System.String).Assembly.GetName().Version.ToString();

			//string version = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

			Console.WriteLine(version);
		}
	}
}
