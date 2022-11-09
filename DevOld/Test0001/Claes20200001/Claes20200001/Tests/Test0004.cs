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
			D2Point[] Points = Enumerable.Range(0, 91).Select(dummy => new D2Point(0.0, 1.0)).ToArray();

			Points[0].X = 1.0;
			Points[0].Y = 0.0;

			for (int c = 0; c < 24000; c++)
			{
				for (int d = 1; d <= 89; d++)
				{
					Points[d] = (Points[d - 1] + Points[d + 1]) / 2.0;
					Points[d] /= GetDistance(Points[d]);
					//Points[d].X = Math.Sqrt(1.0 - Points[d].Y * Points[d].Y);
					//Points[d].Y = Math.Sqrt(1.0 - Points[d].X * Points[d].X);
				}
			}

			for (int d = 0; d <= 90; d++)
			{
				//Console.WriteLine(string.Format("Cos {0} degree is {1:F12}", d, Points[d].X));
				//Console.WriteLine(string.Format("Sin {0} degree is {1:F12}", d, Points[d].Y));

				//Console.WriteLine("Cos " + d + " : " + (Points[d].X - Math.Cos(d * Math.PI / 180.0)).ToString("F12"));
				//Console.WriteLine("Sin " + d + " : " + (Points[d].Y - Math.Sin(d * Math.PI / 180.0)).ToString("F12"));

				Console.WriteLine("Cos " + d + " : " + (Points[d].X - Math.Cos(d * Math.PI / 180.0)).ToString("F13"));
				Console.WriteLine("Sin " + d + " : " + (Points[d].Y - Math.Sin(d * Math.PI / 180.0)).ToString("F13"));
			}
		}

		private double GetDistance(D2Point pt)
		{
			return Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
		}
	}
}
