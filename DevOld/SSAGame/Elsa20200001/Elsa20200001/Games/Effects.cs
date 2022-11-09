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
				DDDraw.DrawBegin(Ground.I.Picture.Dummy, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
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
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
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
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
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
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(3.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> 空中ジャンプの足場(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(1.0 - scene.Rate);
				DDDraw.SetBright(0.5, 1.0, 1.0);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.5 + 1.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> バズーカ発射(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(5))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.5, 0.5);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(1.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static IEnumerable<bool> ほむらシールド終了(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(1.0 - scene.Rate);
				DDDraw.SetBright(0.0, 0.0, 0.0);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.5 + 1.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
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
