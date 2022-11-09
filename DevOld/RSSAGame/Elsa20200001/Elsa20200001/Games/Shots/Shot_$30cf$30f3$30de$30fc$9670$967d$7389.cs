using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_ハンマー陰陽玉 : Shot
	{
		private int Level;

		public Shot_ハンマー陰陽玉(double x, double y, bool facingLeft, int level)
			: base(x, y, facingLeft, LevelToAttackPoint(level), true, true)
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

		private static double LevelTo空気抵抗(int level)
		{
			switch (level)
			{
				case 1: return 0.97;
				case 2: return 0.975;
				case 3: return 0.98;
				case 4: return 0.99;

				default:
					throw null; // never
			}
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double SCALE = LevelToScale(this.Level);
			double R = SCommon.ToInt(24.0 * SCALE);

			double xAdd = this.FacingLeft ? -1.0 : 1.0;
			double yAdd = Game.I.Player.YSpeed * 0.2;

			DDUtils.MakeXYSpeed(0.0, 0.0, xAdd, yAdd, 20.0 * SCALE, out xAdd, out yAdd);

			for (int frame = 0; ; frame++)
			{
				if (Game.I.Status.Equipment != GameStatus.Equipment_e.ハンマー陰陽玉) // 武器を切り替えたら消滅
				{
					this.Kill();
					break;
				}
				double xaa;
				double yaa;

				// バネの加速度
				{
					xaa = (Game.I.Player.X - this.X) * 0.01;
					yaa = (Game.I.Player.Y - this.Y) * 0.01;
				}

				yaa += 1.0; // 重力加速度

				xAdd += xaa;
				yAdd += yaa;

				// 空気抵抗
				{
					double 空気抵抗 = LevelTo空気抵抗(this.Level);

					xAdd *= 空気抵抗;
					yAdd *= 空気抵抗;
				}

				this.X += xAdd;
				this.Y += yAdd;

				DDDraw.DrawBegin(Ground.I.Picture2.陰陽玉, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawSetSize(R * 2, R * 2);
				DDDraw.DrawRotate(frame / 10.0);
				DDDraw.DrawEnd();

				// プレイヤーとハンマーを繋ぐバネのような何かを描画する。
				{
					double d = DDUtils.GetDistance(this.X - Game.I.Player.X, this.Y - Game.I.Player.Y);

					if (10.0 < d)
					{
						double x = (this.X + Game.I.Player.X) / 2.0;
						double y = (this.Y + Game.I.Player.Y) / 2.0;
						double rot = DDUtils.GetAngle(this.X - Game.I.Player.X, this.Y - Game.I.Player.Y) + Math.PI / 2;

						DDDraw.SetAlpha(0.5);
						DDDraw.DrawBegin(Ground.I.Picture2.Laser[4], x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
						DDDraw.DrawSetSize_H(d);
						DDDraw.DrawRotate(rot);
						DDDraw.DrawEnd();
						DDDraw.Reset();
					}
				}

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);

				yield return true;
			}
		}
	}
}
