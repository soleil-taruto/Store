using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_弾 : Enemy
	{
		public static Enemy_弾 Create自機狙い(double x, double y, double speed, double rot = 0.0)
		{
			return Create何か狙い(x, y, new D2Point(Game.I.Player.X, Game.I.Player.Y), speed, rot);
		}

		public static Enemy_弾 Create何か狙い(double x, double y, D2Point targetPt, double speed, double rot = 0.0)
		{
			double xa;
			double ya;

			DDUtils.MakeXYSpeed(x, y, targetPt.X, targetPt.Y, speed, out xa, out ya);
			DDUtils.Rotate(ref xa, ref ya, rot);

			return new Enemy_弾(x, y, xa, ya);
		}

		private double XAdd;
		private double YAdd;

		public Enemy_弾(double x, double y, double xa, double ya)
			: base(x, y, 0, 3, true)
		{
			this.XAdd = xa;
			this.YAdd = ya;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			const double CHARA_R = 8.0;

			for (; ; )
			{
				this.X += this.XAdd;
				this.Y += this.YAdd;

				// 壁衝突判定
				//if (Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X, this.Y))).Tile.IsWall())
				//    break;

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), CHARA_R * 1.2))
				{
					// 暫定_描画
					{
						DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
						DDDraw.DrawSetSize(CHARA_R * 2, CHARA_R * 2);
						DDDraw.DrawEnd();

						DDPrint.SetDebug((int)this.X - DDGround.ICamera.X, (int)this.Y - DDGround.ICamera.Y);
						DDPrint.SetBorder(new I3Color(0, 0, 0));
						DDPrint.Print("[敵弾]");
						DDPrint.Reset();
					}

					this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), CHARA_R);
				}
				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y));
			}
		}

		protected override void Killed()
		{
			// noop
		}
	}
}
