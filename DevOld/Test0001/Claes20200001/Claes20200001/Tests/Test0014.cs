using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0014
	{
		public void Test01()
		{
			IEnumerable<AngleInfo> angles = new AngleInfo[]
			{
				new AngleInfo() { Degree =  0.0, Point = new D2Point(1.0, 0.0) },
				new AngleInfo() { Degree = 90.0, Point = new D2Point(0.0, 1.0) },
			};

			for (int c = 0; c < 10; c++)
				angles = Subdivide(angles);

			foreach (AngleInfo angle in angles)
			{
				Console.WriteLine("Cos " + angle.Degree.ToString("F9") + " degree is " + angle.Point.X.ToString("F15"));
				Console.WriteLine("Sin " + angle.Degree.ToString("F9") + " degree is " + angle.Point.Y.ToString("F15"));
			}
		}

		private IEnumerable<AngleInfo> Subdivide(IEnumerable<AngleInfo> angles)
		{
			IEnumerator<AngleInfo> reader = angles.GetEnumerator();

			if (!reader.MoveNext())
				throw new Exception("angles is empty");

			AngleInfo last = reader.Current;

			while (reader.MoveNext())
			{
				AngleInfo next = reader.Current;

				double mDeg = (last.Degree + next.Degree) / 2.0;
				D2Point mPt = (last.Point + next.Point) / 2.0;
				double d = Math.Sqrt(mPt.X * mPt.X + mPt.Y * mPt.Y);
				mPt /= d;

				yield return last;
				yield return new AngleInfo() { Degree = mDeg, Point = mPt };

				last = next;
			}
			yield return last;
		}

		private class AngleInfo
		{
			public double Degree;
			public D2Point Point;
		}
	}
}
