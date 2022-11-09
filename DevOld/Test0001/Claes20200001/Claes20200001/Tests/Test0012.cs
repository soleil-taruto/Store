using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0012
	{
		public void Test01()
		{
			for (int degree = 0; degree < 360; degree++)
			{
				double rad = degree * (Math.PI / 180.0);

				double s1 = Sin(rad);
				double s2 = Math.Sin(rad);

				double c1 = Cos(rad);
				double c2 = Math.Cos(rad);

				// しゃーないけど 90, 270 のとき全然合わない。
				//
				double t1 = Tan(rad);
				double t2 = Math.Tan(rad);

				//Console.WriteLine(degree + " ==> " + (s1 - s2).ToString("F9") + " , " + (c1 - c2).ToString("F9") + " , " + (t1 - t2).ToString("F9"));
				//Console.WriteLine(degree + " ==> " + (s1 - s2).ToString("F20") + " , " + (c1 - c2).ToString("F20") + " , " + (t1 - t2).ToString("F20"));
				Console.WriteLine(degree + " ==> " + Math.Abs(s1 - s2).ToString("F20") + " , " + Math.Abs(c1 - c2).ToString("F20") + " , " + Math.Abs(t1 - t2).ToString("F20"));
			}
		}

		// ----
		// ----
		// ----

		private const double PI = 3.14159265358979;

		private double Tan(double rad)
		{
			return Sin(rad) / Cos(rad);
		}

		private double Sin(double rad)
		{
			return Cos(rad - PI / 2.0);
		}

		private double Cos(double rad)
		{
			while (rad < 0.0)
				rad += PI * 2.0;

			while (PI * 2.0 < rad)
				rad -= PI * 2.0;

			return M_Cos(rad);
		}

		private double M_Cos(double rad)
		{
			if (PI < rad)
				return M_Cos(PI * 2.0 - rad);

			if (PI / 2.0 < rad)
				return M_Cos(PI - rad) * -1.0;

			D2Point lPt = new D2Point(1.0, 0.0);
			D2Point rPt = new D2Point(0.0, 1.0);
			double lRad = 0.0;
			double rRad = PI / 2.0;

			for (int c = 0; c < 50; c++)
			{
				D2Point mPt = (lPt + rPt) / 2.0;
				double d = M_Sqrt(mPt.X * mPt.X + mPt.Y * mPt.Y);
				mPt /= d;
				double mRad = (lRad + rRad) / 2.0;

				if (mRad < rad)
				{
					lPt = mPt;
					lRad = mRad;
				}
				else
				{
					rPt = mPt;
					rRad = mRad;
				}
			}

			{
				D2Point mPt = (lPt + rPt) / 2.0;

				return mPt.X;
			}
		}

		private double M_Sqrt(double value)
		{
			if (value < 0.0)
				throw null; // Bad params

			double l = 0.0;
			double r = Math.Max(1.0, value);

			for (int c = 0; c < 50; c++)
			{
				double m = (l + r) / 2.0;
				double mv = m * m;

				if (mv < value)
					l = m;
				else
					r = m;
			}

			{
				double m = (l + r) / 2.0;

				return m;
			}
		}
	}
}
