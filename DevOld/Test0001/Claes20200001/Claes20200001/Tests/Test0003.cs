using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0003
	{
		public void Test01()
		{
			Test01_a(20, 60, 50, 30); // -> 30
			Test01_a(20, 60, 40, 40); // -> 16.91751...
		}

		private void Test01_a(double a, double b, double c, double d)
		{
			// https://ja.wikipedia.org/wiki/%E3%83%A9%E3%83%B3%E3%82%B0%E3%83%AC%E3%83%BC%E3%81%AE%E5%95%8F%E9%A1%8C
			// https://upload.wikimedia.org/wikipedia/commons/7/73/Int_quadrangle.jpg

			// degree -> rad
			//
			a *= Math.PI / 180.0;
			b *= Math.PI / 180.0;
			c *= Math.PI / 180.0;
			d *= Math.PI / 180.0;

			D2Point pB = new D2Point(0.0, 0.0);
			D2Point pC = new D2Point(1.0, 0.0);

			// Y-軸のプラス方向は下とすることに注意！

			D2Point pA = GetCrossPoint(pB, pB + AngleToPoint(-(a + b), 1.0), pC, pC + AngleToPoint(Math.PI + c, 1.0));
			D2Point pD = GetCrossPoint(pB, pB + AngleToPoint(-b, 1.0), pC, pC + AngleToPoint(Math.PI + (c + d), 1.0));

			Console.WriteLine("B: " + pB);
			Console.WriteLine("C: " + pC);
			Console.WriteLine("A: " + pA);
			Console.WriteLine("D: " + pD);

			double angDA = GetAngle(pA - pD);
			double angDB = GetAngle(pB - pD);

			double e = angDA - angDB;

			// rad -> degree;
			//
			e *= 180.0 / Math.PI;

			Console.WriteLine(e.ToString("F9"));
		}

		/// <summary>
		/// p1, p2 が通る直線と q1, q2 が通る直線の交点を返す。
		/// </summary>
		/// <param name="p1">点の座標</param>
		/// <param name="p2">点の座標</param>
		/// <param name="q1">点の座標</param>
		/// <param name="q2">点の座標</param>
		/// <returns>交点の座標</returns>
		private D2Point GetCrossPoint(D2Point p1, D2Point p2, D2Point q1, D2Point q2)
		{
			double pxd = p2.X - p1.X;
			double qxd = q2.X - q1.X;
			double pyd = p2.Y - p1.Y;
			double qyd = q2.Y - q1.Y;

			double pn = p1.Y * p2.X - p1.X * p2.Y;
			double qn = q1.Y * q2.X - q1.X * q2.Y;

			double d = pyd * qxd - pxd * qyd;

			Console.WriteLine("d: " + d.ToString("F9")); // test

			double x = (qn * pxd - pn * qxd) / d;
			double y = (qn * pyd - pn * qyd) / d;

			return new D2Point(x, y);
		}

		public static D2Point AngleToPoint(double angle, double distance)
		{
			return new D2Point(
				distance * Math.Cos(angle),
				distance * Math.Sin(angle)
				);
		}

		public static double GetAngle(D2Point pt)
		{
			return GetAngle(pt.X, pt.Y);
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

			double r1 = 0.0;
			double r2 = Math.PI / 2.0;
			double t = y / x;
			double rm;

			for (int c = 1; ; c++)
			{
				rm = (r1 + r2) / 2.0;

				//if (10 <= c)
				if (50 <= c)
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
