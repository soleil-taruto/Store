using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0016
	{
		public void Test01()
		{
			Queue<AngleInfo> angles = new Queue<AngleInfo>(new AngleInfo[]
			{
				new AngleInfo() { Degree =  0.0, Point = new D2Point(1.0, 0.0) },
				new AngleInfo() { Degree = 90.0, Point = new D2Point(0.0, 1.0) },
				null, // ender
			});

			Subdivide(angles, 10);

			foreach (AngleInfo angle in angles)
			{
				Console.WriteLine("Cos " + angle.Degree.ToString("F9") + " degree is " + angle.Point.X.ToString("F15"));
				Console.WriteLine("Sin " + angle.Degree.ToString("F9") + " degree is " + angle.Point.Y.ToString("F15"));

				//Console.WriteLine($"Cos {angle.Degree:F9} degree is {angle.Point.X:F15}");
				//Console.WriteLine($"Sin {angle.Degree:F9} degree is {angle.Point.Y:F15}");
			}
		}

		private static void Subdivide(Queue<AngleInfo> angles, int depth)
		{
			AngleInfo last = angles.Dequeue();

			for (; ; )
			{
				AngleInfo next = angles.Dequeue();

				if (next == null) // is ender
				{
					angles.Enqueue(last);

					if (--depth <= 0)
						break;

					angles.Enqueue(null); // put ender
					last = angles.Dequeue();
					continue;
				}

				double mDeg = (last.Degree + next.Degree) / 2.0;
				D2Point mPt = (last.Point + next.Point) / 2.0;
				double d = M_Sqrt(mPt.X * mPt.X + mPt.Y * mPt.Y);
				mPt /= d;

				angles.Enqueue(last);
				angles.Enqueue(new AngleInfo() { Degree = mDeg, Point = mPt });

				last = next;
			}
		}

		private static double M_Sqrt(double value)
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

		private class AngleInfo
		{
			public double Degree;
			public D2Point Point;
		}
	}
}
