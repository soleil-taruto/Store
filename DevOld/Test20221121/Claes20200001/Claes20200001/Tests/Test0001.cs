using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Commons;
using Charlotte.Utilities;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			foreach (string file in Directory.GetFiles(@"C:\temp"))
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".bmp" ||
					ext == ".gif" ||
					ext == ".jpg" ||
					ext == ".jpeg" ||
					ext == ".png"
					)
				{
					Test01_a(file);
				}
			}
		}

		private void Test01_a(string file)
		{
			for (int mode = 1; mode <= 2; mode++)
			{
				foreach (double rate in new double[] { 0.1, 0.333, 3.0, 10.0 })
				{
					Console.WriteLine(mode + ", " + rate);

					Canvas canvas = Canvas.LoadFromFile(file);

					switch (mode)
					{
						case 1:
							canvas.FilterAllDot(dot =>
							{
								dot = new I4Color(
									SCommon.ToInt(Math.Min(255.0, dot.R * rate)),
									SCommon.ToInt(Math.Min(255.0, dot.G * rate)),
									SCommon.ToInt(Math.Min(255.0, dot.B * rate)),
									dot.A
									);

								return dot;
							});
							break;

						case 2:
							canvas.FilterAllDot(dot =>
							{
								dot = new I4Color(
									SCommon.ToInt(Math.Max(0.0, 255.0 - (255.0 - dot.R) * rate)),
									SCommon.ToInt(Math.Max(0.0, 255.0 - (255.0 - dot.G) * rate)),
									SCommon.ToInt(Math.Max(0.0, 255.0 - (255.0 - dot.B) * rate)),
									dot.A
									);

								return dot;
							});
							break;

						default:
							throw null; // never
					}

					canvas.Save(SCommon.NextOutputPath() + ".png");
				}
			}
			for (int mode = 0; mode <= 6; mode++)
			{
				Console.WriteLine(mode);

				int lw = (mode + 0) * 32 - 0;
				int hi = (mode + 1) * 32 - 1;

				Canvas canvas = Canvas.LoadFromFile(file);

				Func<int, int> colFltr = value =>
				{
					if (value <= lw)
						return 0;

					if (hi <= value)
						return 255;

					return SCommon.ToInt((value - lw) * 255.0 / (hi - lw));
				};

				canvas.FilterAllDot(dot =>
				{
					dot = new I4Color(
						colFltr(dot.R),
						colFltr(dot.G),
						colFltr(dot.B),
						dot.A
						);

					return dot;
				});

				canvas.Save(SCommon.NextOutputPath() + ".png");
			}
		}
	}
}
