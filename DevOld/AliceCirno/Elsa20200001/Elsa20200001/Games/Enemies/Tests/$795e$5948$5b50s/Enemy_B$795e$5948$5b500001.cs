using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.Tests.神奈子s
{
	public class Enemy_B神奈子0001 : Enemy
	{
		public Enemy_B神奈子0001(double x, double y)
			: base(x, y, 100, 1, false)
		{ }

		private Func<bool> SpecialDraw = () => false;

		protected override IEnumerable<bool> E_Draw()
		{
			// ---- game_制御 ----

			Ground.I.Music.神さびた古戦場.Play();

			// ----

			int boss_rot = 0;
			double boss_approach_rate = 1.0;

			for (int frame = 0; ; frame++)
			{
				if (!this.SpecialDraw())
				{
					double x = Math.Cos(boss_rot / 100.0);
					double y = Math.Sin(boss_rot / 100.0);

					boss_rot++;

					x *= 0.4;
					y *= 0.4;

					x += 0.5;
					y += 0.5;

					x *= Game.I.Map.W * GameConsts.TILE_W;
					y *= Game.I.Map.H * GameConsts.TILE_H;

					DDUtils.Approach(ref this.X, x, boss_approach_rate);
					DDUtils.Approach(ref this.Y, y, boss_approach_rate);
					DDUtils.Approach(ref boss_approach_rate, 0.97, 0.99);

					if (frame % 30 == 0)
					{
						Game.I.Enemies.Add(new Enemy_B神奈子_Tama(this.X, this.Y));
					}

					bool facingLeft = Game.I.Player.X < this.X;

					DDDraw.DrawBegin(Ground.I.Picture2.Enemy_神奈子[0], this.X, this.Y);
					DDDraw.DrawSlide(20.0, 10.0);
					DDDraw.DrawZoom_X(facingLeft ? 1 : -1);
					DDDraw.DrawEnd();
				}

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 80.0);

				yield return true;
			}
		}

		protected override void P_Damaged(Shot shot, int damagePoint)
		{
			this.SpecialDraw = SCommon.Supplier(this.E_HitBack());
			EnemyCommon.Damaged(this, shot, damagePoint);
			Game.I.Enemies.Add(new Enemy_B神奈子_Tama(this.X, this.Y)); // 撃ち返し
		}

		private IEnumerable<bool> E_HitBack()
		{
			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				double xBuru = DDUtils.Random.GetReal2() * 10.0;
				double yBuru = DDUtils.Random.GetReal2() * 10.0;

				bool facingLeft = Game.I.Player.X < this.X;

				DDDraw.DrawBegin(Ground.I.Picture2.Enemy_神奈子[12], this.X + xBuru, this.Y + yBuru);
				DDDraw.DrawSlide(20.0, -10.0);
				DDDraw.DrawZoom_X(facingLeft ? 1 : -1);
				DDDraw.DrawEnd();

				yield return true;
			}
		}

		protected override void P_Killed(bool destroyed)
		{
			敵弾一掃();
			Game.I.Enemies.Add(new Enemy_B神奈子9901(this.X, this.Y));
		}

		private static void 敵弾一掃()
		{
			foreach (Enemy enemy in Game.I.Enemies.Iterate())
				if (enemy is Enemy_B神奈子_Tama)
					enemy.Kill();
		}
	}
}
