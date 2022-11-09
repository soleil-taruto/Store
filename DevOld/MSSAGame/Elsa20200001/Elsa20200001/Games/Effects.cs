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
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.6, 0.3);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(3.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> FireBall爆発(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 1.0, 0.0);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(1.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> EnemyKilled(double x, double y)
		{
			foreach (DDPicture picture in Ground.I.Picture2.EnemyKilled)
			{
				for (int c = 0; c < 5; c++)
				{
					DDDraw.SetAlpha(0.8);
					DDDraw.DrawBegin(
						picture,
						x - DDGround.ICamera.X,
						y - DDGround.ICamera.Y
						);
					DDDraw.DrawZoom(1.5);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					y += 1.0;

					yield return true;
				}
			}
		}

		public static IEnumerable<bool> 閉鎖_開放(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(15))
			{
				DDDraw.SetAlpha(0.5);
				DDDraw.SetBright(0.0, 1.0, 1.0);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteBox, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawSetSize(GameConsts.TILE_W, GameConsts.TILE_H);
				DDDraw.DrawZoom(1.0 - scene.Rate * 0.5);
				DDDraw.DrawRotate(Math.PI * 2.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
