using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_敵
	/// </summary>
	public class Enemy_B0002 : Enemy
	{
		public Enemy_B0002(double x, double y)
			: base(x, y, 10, 3, false)
		{ }

		private const int HIT_BACK_FRAME_MAX = 10;

		private int HitBackFrame = 0; // 0 == 無効, 1～ ヒットバック中

		protected override IEnumerable<bool> E_Draw()
		{
			for (int frame = 0; ; frame++)
			{
				DDPicture picture = Ground.I.Picture.Enemy_B0002_01;
				double SPEED = 2.0;
				double xBuru = 0.0;
				double yBuru = 0.0;

				if (1 <= this.HitBackFrame)
				{
					int frm = this.HitBackFrame - 1;

					if (HIT_BACK_FRAME_MAX < frm)
					{
						this.HitBackFrame = 0;
						goto endHitBack;
					}
					this.HitBackFrame++;

					// ----

					double rate = (double)frm / HIT_BACK_FRAME_MAX;

					picture = Ground.I.Picture.Enemy_B0002_02;
					SPEED = 0.0;
					xBuru = (1.0 - rate) * 30.0 * DDUtils.Random.Real();
					yBuru = (1.0 - rate) * 30.0 * DDUtils.Random.Real();
				}
			endHitBack:

				switch (frame / 60 % 4)
				{
					case 0: this.X += SPEED; break;
					case 1: this.Y += SPEED; break;
					case 2: this.X -= SPEED; break;
					case 3: this.Y -= SPEED; break;

					default:
						throw null; // never
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 100.0))
				{
					double xZoom = this.X < Game.I.Player.X ? -1.0 : 1.0;

					DDDraw.DrawBegin(
						picture,
						this.X - DDGround.ICamera.X + xBuru,
						this.Y - DDGround.ICamera.Y + yBuru
						);
					DDDraw.DrawSetSize(100.0, 100.0);
					DDDraw.DrawZoom_X(xZoom);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					this.Crash = DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(100.0, 100.0));
				}
				yield return true;
			}
		}

		public override void Damaged(Shot shot)
		{
			//this.X += 10.0 * (shot.FacingLeft ? -1 : 1); // ヒットバック
			this.HitBackFrame = 1;
			base.Damaged(shot);
		}
	}
}
