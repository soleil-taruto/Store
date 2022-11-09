using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;

namespace Charlotte.Games.Shots
{
	public class Shot_MagnetAir : Shot
	{
		private int Level;

		public Shot_MagnetAir(double x, double y, bool facingLeft, int level)
			: base(x, y, facingLeft, LevelToAttackPoint(level), false, false)
		{
			this.Level = level;
		}

		private static int LevelToAttackPoint(int level)
		{
			switch (level)
			{
				case 1: return 1;
				case 2: return 2;
				case 3: return 4;
				case 4: return 8;

				default:
					throw null; // never
			}
		}

		private static double LevelToScale(int level)
		{
			switch (level)
			{
				case 1: return 1.0;
				case 2: return 1.5;
				case 3: return 2.0;
				case 4: return 2.5;

				default:
					throw null; // never
			}
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double SCALE = LevelToScale(this.Level);
			double R = SCommon.ToInt(24.0 * SCALE);

			int yAddDir = 0;

			for (; ; )
			{
				if (yAddDir == 0)
				{
					this.X += 10.0 * (this.FacingLeft ? -1 : 1);
					yAddDir = this.上下索敵(R);
				}
				else
				{
					this.Y += 10.0 * yAddDir;
				}

				DDDraw.SetBright(new I3Color(0, 192, 255));
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawSetSize(R * 2, R * 2);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				DDPrint.SetDebug(
					(int)this.X - DDGround.ICamera.X - 12,
					(int)this.Y - DDGround.ICamera.Y - 8
					);
				DDPrint.SetBorder(new I3Color(0, 0, 0));
				DDPrint.Print("MA" + this.Level);
				DDPrint.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), R); // カメラから出たら消滅する。
			}
		}

		private int 上下索敵(double R) // ? { -1, 0, 1 } == { 上に居る, 見つからない, 下に居る }
		{
			double X_SPAN_後方 = R;
			double X_SPAN_前方 = 5.0;

			double X_SPAN_L;
			double X_SPAN_R;

			if (this.FacingLeft) // ? 左へ進行している。
			{
				X_SPAN_L = X_SPAN_前方; // 左が前方
				X_SPAN_R = X_SPAN_後方; // 右が後方
			}
			else
			{
				X_SPAN_L = X_SPAN_後方; // 左が後方
				X_SPAN_R = X_SPAN_前方; // 右が前方
			}

			foreach (Enemy enemy in Game.I.Enemies.Iterate())
			{
				if (
					1 <= enemy.HP &&
					!DDUtils.IsOutOfCamera(new D2Point(enemy.X, enemy.Y)) &&
					SCommon.IsRange(enemy.X, this.X - X_SPAN_L, this.X + X_SPAN_R)
					)
					return enemy.Y < this.Y ? -1 : 1;
			}
			return 0;
		}
	}
}
