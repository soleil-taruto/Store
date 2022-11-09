using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class Effects
	{
		public static IEnumerable<bool> 小爆発(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(5))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.5, 0.5);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.3 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> 中爆発(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(1.0 - scene.Rate * 0.3);
				DDDraw.SetBright(1.0, 0.3, 0.3);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(3.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
