using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class Effects
	{
		/// <summary>
		/// エフェクト・テスト -- ★サンプルとしてキープ
		/// 追加例：
		/// -- DDGround.EL.Add(SCommon.Supplier(Effects.TestEffect(400.0, 300.0)));
		/// </summary>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns>エフェクト</returns>
		public static IEnumerable<bool> TestEffect(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				DDDraw.SetBright(1.0, 0.5, 0.5);
				DDDraw.DrawBegin(Ground.I.Picture.Dummy, x - DDGround.Camera.X, y - DDGround.Camera.Y);
				DDDraw.DrawRotate(scene.Rate * Math.PI * 2.0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> 小爆発(double x, double y) // ★サンプルとしてキープ
		{
			foreach (DDScene scene in DDSceneUtils.Create(5))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.5, 0.5);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.Camera.X, y - DDGround.Camera.Y);
				DDDraw.DrawZoom(0.3 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> 中爆発(double x, double y) // ★サンプルとしてキープ
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.6, 0.3);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.Camera.X, y - DDGround.Camera.Y);
				DDDraw.DrawZoom(1.5 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> 大爆発(double x, double y) // ★サンプルとしてキープ
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(0.6, 0.8, 1.0);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.Camera.X, y - DDGround.Camera.Y);
				DDDraw.DrawZoom(3.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> PlayerDead(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(GameConsts.PLAYER_DEAD_FRAME_MAX))
			{
				DDDraw.SetAlpha(0.5);
				DDDraw.DrawBegin(Ground.I.Picture.Player, x, y);
				DDDraw.DrawRotate(scene.Rate * 5.0);
				DDDraw.DrawZoom(1.0 + scene.Rate * 5.0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
