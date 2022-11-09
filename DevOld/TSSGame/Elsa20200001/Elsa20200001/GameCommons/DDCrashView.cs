using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public class DDCrashView : IDisposable
	{
		private static readonly I3Color DefaultColor = new I3Color(0, 255, 255);
		private const double POINT_WH = 4.0;

		private DDSubScreen MyScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		public DDCrashView()
		{
			using (this.MyScreen.Section())
			{
				DDCurtain.DrawCurtain();
			}
		}

		public void Draw(DDCrash crash)
		{
			Draw(new DDCrash[] { crash });
		}

		public void Draw(DDCrash crash, I3Color color)
		{
			Draw(new DDCrash[] { crash }, color);
		}

		public void Draw(IEnumerable<DDCrash> crashes)
		{
			Draw(crashes, DefaultColor);
		}

		public void Draw(IEnumerable<DDCrash> crashes, I3Color color)
		{
			DDDraw.SetBright(color);

			using (this.MyScreen.Section())
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
								DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, crash.Pt.X - DDGround.ICamera.X, crash.Pt.Y - DDGround.ICamera.Y);
								DDDraw.DrawSetSize(POINT_WH, POINT_WH);
								DDDraw.DrawEnd();
								break;

							case DDCrashUtils.Kind_e.CIRCLE:
								DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, crash.Pt.X - DDGround.ICamera.X, crash.Pt.Y - DDGround.ICamera.Y);
								DDDraw.DrawSetSize(crash.R * 2.0, crash.R * 2.0);
								DDDraw.DrawEnd();
								break;

							case DDCrashUtils.Kind_e.RECT:
								DDDraw.DrawRect(
									Ground.I.Picture.WhiteBox,
									crash.Rect.L - DDGround.ICamera.X,
									crash.Rect.T - DDGround.ICamera.Y,
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
			DDDraw.Reset();
		}

		public DDPicture GetPicture()
		{
			return this.MyScreen.ToPicture();
		}

		public void DrawToScreen(double a = 0.3)
		{
			DDDraw.SetAlpha(a);
			DDDraw.DrawSimple(this.GetPicture(), 0, 0);
			DDDraw.Reset();
		}

		public void Dispose()
		{
			if (this.MyScreen != null)
			{
				this.MyScreen.Dispose();
				this.MyScreen = null;
			}
		}
	}
}
