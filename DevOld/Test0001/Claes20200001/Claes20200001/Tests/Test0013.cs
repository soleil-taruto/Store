using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0013
	{
		public void Test01()
		{
			for (double degree = 0.0; degree < 360.0; degree += 0.1)
			{
				double rad = degree * Math.PI / 180.0;

				double s1 = Sin(degree);
				double s2 = Math.Sin(rad);

				double c1 = Cos(degree);
				double c2 = Math.Cos(rad);

				double t1 = Tan(degree);
				double t2 = Math.Tan(rad);

				Console.WriteLine(string.Format("{0:F1} 度のときの誤差 ==> Sin : {1:F20} , Cos : {2:F20} , Tan : {3:F20}"
					, degree
					, Math.Abs(s1 - s2)
					, Math.Abs(c1 - c2)
					, Math.Abs(t1 - t2)
					));
			}
		}

		public void Test02()
		{
			for (double value = 0.0; value < 10.0; value += 0.01)
			{
				double r1 = M_Sqrt(value);
				double r2 = Math.Sqrt(value);

				Console.WriteLine(string.Format("{0:F2} の平方根の誤差 ==> {1:F20}"
					, value
					, Math.Abs(r1 - r2)
					));
			}
		}

		// ----
		// ----
		// ----

		private double Tan(double degree)
		{
			return Sin(degree) / Cos(degree);
		}

		private double Sin(double degree)
		{
			return Cos(degree - 90.0);
		}

		private double Cos(double degree)
		{
			while (degree < 0.0)
				degree += 360.0;

			while (360.0 < degree)
				degree -= 360.0;

			return M_Cos(degree);
		}

		private double M_Cos(double degree)
		{
			if (180.0 < degree)
				return M_Cos(360.0 - degree);

			if (90.0 < degree)
				return M_Cos(180.0 - degree) * -1.0;

			D2Point lPt = new D2Point(1.0, 0.0);
			D2Point rPt = new D2Point(0.0, 1.0);
			double lDeg = 0.0;
			double rDeg = 90.0;

			for (int c = 0; c < 50; c++)
			{
				D2Point mPt = (lPt + rPt) / 2.0;
				double d = M_Sqrt(mPt.X * mPt.X + mPt.Y * mPt.Y);
				mPt /= d;
				double mDeg = (lDeg + rDeg) / 2.0;

				if (mDeg < degree)
				{
					lPt = mPt;
					lDeg = mDeg;
				}
				else
				{
					rPt = mPt;
					rDeg = mDeg;
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
				throw new ArgumentException();

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
