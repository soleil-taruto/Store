using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0004
	{
		public void Test01()
		{
			for (double distance = 0.0001; distance < 10000.0; distance *= 3.16)
			{
				for (double angle = 0.0; angle < Math.PI * 2.0; angle += 0.0001)
				{
					double x = Math.Cos(angle) * distance;
					double y = Math.Sin(angle) * distance;

					double ret = GetAngle(x, y);
					double diff = ret - angle;

					Console.WriteLine(distance.ToString("F9") + ", " + angle.ToString("F9") + " ==> " + diff.ToString("F9"));

					if (Math.Abs(diff) > SCommon.MICRO)
						throw null; // BUG !!!
				}
				//Console.WriteLine("OK");
			}
			Console.WriteLine("OK!");
		}

		public void Test02()
		{
			const double distance = 100.0;

			for (int a = 1; a < 8; a++)
			{
				for (int b = 0; b < 10000; b++)
				{
					for (int bSgn = -1; bSgn <= 1; bSgn += 2)
					{
						double angle = (Math.PI / 4.0) * a + b * 0.00000001 * bSgn;

						double x = Math.Cos(angle) * distance;
						double y = Math.Sin(angle) * distance;

						double ret = GetAngle(x, y);
						double diff = ret - angle;

						Console.WriteLine(distance.ToString("F9") + ", " + angle.ToString("F9") + " ==> " + diff.ToString("F9"));

						if (Math.Abs(diff) > SCommon.MICRO)
							throw null; // BUG !!!
					}
				}
			}
			Console.WriteLine("OK!");
		}

		/// <summary>
		/// 原点から指定座標への角度を返す。
		/// ラジアン角 (0.0 ～ Math.PI * 2.0)
		/// 右真横 (0,0 -> 1,0 方向) を 0.0 として時計回り。但し、X軸プラス方向は右、Y軸プラス方向は下である。
		/// 1周は Math.PI * 2.0
		/// </summary>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns>角度</returns>
		public static double GetAngle(double x, double y)
		{
			if (y < 0.0) return Math.PI * 2.0 - GetAngle(x, -y);
			if (x < 0.0) return Math.PI - GetAngle(-x, y);

			if (x < y) return Math.PI / 2.0 - GetAngle(y, x);
			if (x < SCommon.MICRO) return 0.0; // 極端に原点に近い座標の場合、常に右真横を返す。

			if (y == 0.0) return 0.0;
			if (y == x) return Math.PI / 4.0;

			double r1 = 0.0;
			double r2 = Math.PI / 4.0;
			double t = y / x;
			double rm;

			for (int c = 1; ; c++)
			{
				rm = (r1 + r2) / 2.0;

				//if (10 <= c) // for Game
				if (50 < c)
					break;

				double rmt = Math.Tan(rm);

				if (t < rmt)
					r2 = rm;
				else
					r1 = rm;
			}
			return rm;
		}
	}
}
