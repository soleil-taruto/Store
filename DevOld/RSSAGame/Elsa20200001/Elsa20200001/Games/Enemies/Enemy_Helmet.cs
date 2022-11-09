using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Helmet : Enemy
	{
		public Enemy_Helmet(double x, double y)
			: base(x, y, 10, 5, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			const double CHARA_R = 20.0;

			double ySpeed = 0.0;

			for (int frame = 0; ; frame++)
			{
				while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), CHARA_R)) // 画面外に居るときは休止する。
					yield return true;

				const double GRAVITY = 1.0;
				const double Y_SPEED_MAX = 10.0;

				this.Y += ySpeed;

				ySpeed += GRAVITY;
				ySpeed = Math.Min(ySpeed, Y_SPEED_MAX);

				if (GameCommon.壁処理(ref this.X, ref this.Y, new D2Point[] { new D2Point(0.0, CHARA_R) }))
				{
					ySpeed = 0.0;
				}

				const int FRAME_DIV = 200;

				int frameDiv = frame / FRAME_DIV;
				int frameMod = frame % FRAME_DIV;

				this.防御中 = frameDiv % 2 == 0;

				if (!this.防御中)
				{
					if (
						frameMod == 20 ||
						frameMod == 60 ||
						frameMod == 100
						)
					{
						int xSign = Game.I.Player.X < this.X ? -1 : 1;

						Game.I.Enemies.Add(new Enemy_弾(this.X, this.Y, 3.5 * xSign, -3.5));
						Game.I.Enemies.Add(new Enemy_弾(this.X, this.Y, 5.0 * xSign, 0.0));
						Game.I.Enemies.Add(new Enemy_弾(this.X, this.Y, 3.5 * xSign, 3.5));
					}
				}

				//if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), CHARA_R * 1.2)) // 画面外は休止
				{
					// 暫定_描画
					{
						DDDraw.SetBright(this.防御中 ? new I3Color(64, 192, 64) : new I3Color(255, 128, 128));
						DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
						DDDraw.DrawSetSize(CHARA_R * 2, CHARA_R * 2);
						DDDraw.DrawEnd();
						DDDraw.Reset();

						DDPrint.SetDebug((int)this.X - DDGround.ICamera.X, (int)this.Y - DDGround.ICamera.Y);
						DDPrint.SetBorder(new I3Color(0, 0, 0));
						DDPrint.Print(string.Format("[メット◆ル_{0}_{1}]", this.HP, this.防御中 ? "防" : "攻"));
						DDPrint.Reset();
					}

					this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), CHARA_R);
				}
				yield return true;
			}
		}
	}
}
