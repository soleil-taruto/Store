using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Chaser : Enemy
	{
		public Enemy_Chaser(double x, double y)
			: base(x, y, 3, 3, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			const double CHARA_R = 20.0;

			for (int frame = 0; ; frame++)
			{
				while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), CHARA_R)) // 画面外に居るときは休止する。
					yield return true;

				double xa;
				double ya;

				DDUtils.MakeXYSpeed(this.X, this.Y, Game.I.Player.X, Game.I.Player.Y, 0.5, out xa, out ya);

				this.X += xa;
				this.Y += ya;

				if (frame != 0 && frame % 240 == 0)
				{
					Game.I.Enemies.Add(Enemy_弾.Create自機狙い(this.X, this.Y, 3.0));
				}

				//if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), CHARA_R * 1.2)) // 画面外では休止
				{
					// 暫定_描画
					{
						DDDraw.SetBright(new I3Color(200, 200, 0));
						DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
						DDDraw.DrawSetSize(CHARA_R * 2, CHARA_R * 2);
						DDDraw.DrawEnd();
						DDDraw.Reset();

						DDPrint.SetDebug((int)this.X - DDGround.ICamera.X, (int)this.Y - DDGround.ICamera.Y);
						DDPrint.SetBorder(new I3Color(0, 0, 0));
						DDPrint.Print("[チェーサー_" + this.HP + "]");
						DDPrint.Reset();
					}

					this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), CHARA_R);
				}
				yield return true;
			}
		}
	}
}
