using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tests
{
	public class Test0004
	{
		private const double SNOW_INIT = 1.0;
		private const double SNOWPLOW_POWER = 1.0;
		private const double SNOW_PER_MILLIS_EXPECT_LW = 0.0;
		private const double SNOW_PER_MILLIS_EXPECT_HI = 1.0;
		private const int SNOW_PER_MILLIS_BIN_SEARCH_MAX = 50;
		private const int MILLIS_PER_HOUR = 3600000;

		public void Test01()
		{
			double l = SNOW_PER_MILLIS_EXPECT_LW;
			double r = SNOW_PER_MILLIS_EXPECT_HI;

			for (int c = 0; c < SNOW_PER_MILLIS_BIN_SEARCH_MAX; c++)
			{
				double m = (l + r) / 2;
				double rate = GetDistanceRate(m);

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
				double millis = SNOW_INIT / m;

				TimeSpan t = new TimeSpan(12, 0, 0) - TimeSpan.FromMilliseconds(millis);

				Console.WriteLine(t.ToString("hh\\:mm\\:ss\\.fff"));
			}
		}

		private double GetDistanceRate(double snowPerMillis)
		{
			double snow = SNOW_INIT;
			double d = 0.0;

			for (int t = 0; t < MILLIS_PER_HOUR; t++)
			{
				d += SNOWPLOW_POWER / snow;
				snow += snowPerMillis;
			}
			double d1 = d;

			for (int t = 0; t < MILLIS_PER_HOUR; t++)
			{
				d += SNOWPLOW_POWER / snow;
				snow += snowPerMillis;
			}
			double d2 = d;

			return d2 / d1;
		}
	}
}
