using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.Tests
{
	public class Enemy_BTrump : Enemy
	{
		public Enemy_BTrump(double x, double y)
			: base(x, y, 30, 1, false)
		{ }

		private bool Reversed = false;
		private Func<bool> SpecialDraw = () => false;

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				if (!this.SpecialDraw())
				{
					DDDraw.DrawCenter(Ground.I.Picture.Enemy_TrumpFrame, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);

					DDPicture picture = this.Reversed ?
						Ground.I.Picture.Enemy_TrumpS01 :
						Ground.I.Picture.Enemy_TrumpBack;

					DDDraw.DrawCenter(picture, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);

					this.Crash = DDCrashUtils.Rect(D4Rect.XYWH(
						this.X,
						this.Y,
						Ground.I.Picture.Enemy_TrumpFrame.Get_W(),
						Ground.I.Picture.Enemy_TrumpFrame.Get_H()
						));
				}
				yield return true;
			}
		}

		protected override void P_Damaged(Shot shot, int damagePoint)
		{
			this.Reversed = !this.Reversed;
			this.SpecialDraw = SCommon.Supplier(this.E_Turn());
			EnemyCommon.Damaged(this, shot, damagePoint);
		}

		private IEnumerable<bool> E_Turn()
		{
			foreach (DDScene scene in DDSceneUtils.Create(60))
			{
				double wRate = Math.Cos(scene.Rate * Math.PI);
				bool b = !this.Reversed;

				if (wRate < 0.0)
				{
					wRate *= -1.0;
					b = !b;
				}
				if (SCommon.MICRO < wRate)
				{
					DDDraw.DrawBegin(Ground.I.Picture.Enemy_TrumpFrame, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
					DDDraw.DrawZoom_X(wRate);
					DDDraw.DrawEnd();

					DDPicture picture = b ?
						Ground.I.Picture.Enemy_TrumpS01 :
						Ground.I.Picture.Enemy_TrumpBack;

					DDDraw.DrawBegin(picture, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
					DDDraw.DrawZoom_X(wRate);
					DDDraw.DrawEnd();
				}

				this.Crash = DDCrashUtils.Rect(D4Rect.XYWH(
					this.X,
					this.Y,
					Ground.I.Picture.Enemy_TrumpFrame.Get_W() * wRate,
					Ground.I.Picture.Enemy_TrumpFrame.Get_H()
					));

				yield return true;
			}
		}
	}
}
