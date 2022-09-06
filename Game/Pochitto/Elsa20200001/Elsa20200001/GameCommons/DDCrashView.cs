using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDCrashView
	{
		private const double POINT_WH = 4.0;

		public static void Draw(IEnumerable<DDCrash> crashes, I3Color color, double a = 1.0)
		{
			DDDraw.SetAlpha(a);
			DDDraw.SetBright(color);

			Draw(crashes);

			DDDraw.Reset();
		}

		public static void Draw(IEnumerable<DDCrash> crashes)
		{
			Queue<IEnumerable<DDCrash>> q = new Queue<IEnumerable<DDCrash>>();

			q.Enqueue(crashes);

			while (1 <= q.Count)
			{
				foreach (DDCrash crash in q.Dequeue())
				{
					switch (crash.Kind)
					{
						case DDCrashUtils.Kind_e.NONE:
							break;

						case DDCrashUtils.Kind_e.POINT:
							DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, crash.Pt.X - DDGround.Camera.X, crash.Pt.Y - DDGround.Camera.Y);
							DDDraw.DrawSetSize(POINT_WH, POINT_WH);
							DDDraw.DrawEnd();
							break;

						case DDCrashUtils.Kind_e.CIRCLE:
							DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, crash.Pt.X - DDGround.Camera.X, crash.Pt.Y - DDGround.Camera.Y);
							DDDraw.DrawSetSize(crash.R * 2.0, crash.R * 2.0);
							DDDraw.DrawEnd();
							break;

						case DDCrashUtils.Kind_e.RECT:
							DDDraw.DrawRect(
								Ground.I.Picture.WhiteBox,
								crash.Rect.L - DDGround.Camera.X,
								crash.Rect.T - DDGround.Camera.Y,
								crash.Rect.W,
								crash.Rect.H
								);
							break;

						case DDCrashUtils.Kind_e.MULTI:
							q.Enqueue(crash.Crashes);
							break;

						default:
							throw null; // never
					}
				}
			}
		}
	}
}
