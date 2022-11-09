using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_跳ねる陰陽玉 : Shot
	{
		private int Level;

		public Shot_跳ねる陰陽玉(double x, double y, bool facingLeft, int level)
			: base(x, y, facingLeft, LevelToAttackPoint(level), true, false) // 自力で壁から跳ねるので、壁貫通にしておく。
		{
			this.Level = level;
		}

		private static int LevelToAttackPoint(int level)
		{
			switch (level)
			{
				case 1: return 10;
				case 2: return 15;
				case 3: return 20;
				case 4: return 25;

				default:
					throw null; // never
			}
		}

		private static int LevelToR(int level)
		{
			switch (level)
			{
				case 1: return 32;
				case 2: return 48;
				case 3: return 64;
				case 4: return 80;

				default:
					throw null; // never
			}
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double R = LevelToR(this.Level); // 自弾半径
			const double X_ADD = 8.0; // 横移動速度
			const double GRAVITY = 0.8; // 重力加速度
			const double Y_ADD_MAX = 19.0; // 落下最高速度
			double Y_ADD_FIRST = Game.I.Player.YSpeed - 6.0; // 初期_縦移動速度
			const double K = 0.98; // 跳ね返り係数

			double yAdd = Y_ADD_FIRST;
			int bouncedCount = 0;

			// 初期位置調整
			{
				this.X += R * (this.FacingLeft ? -1 : 1);
				this.Y -= R;
			}

			for (int frame = 0; ; frame++)
			{
				this.X += X_ADD * (this.FacingLeft ? -1 : 1);
				this.Y += yAdd;

				yAdd += GRAVITY;

				DDUtils.Minim(ref yAdd, Y_ADD_MAX);

				// 跳ね返り
				{
					int xBounce = 0;
					int yBounce = 0;

					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y)).Tile.IsWall())
					{
						xBounce += 3;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y)).Tile.IsWall())
					{
						xBounce -= 3;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y - R)).Tile.IsWall())
					{
						yBounce += 3;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y + R)).Tile.IsWall())
					{
						yBounce -= 3;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R / Consts.ROOT_OF_2, this.Y - R / Consts.ROOT_OF_2)).Tile.IsWall())
					{
						xBounce += 2;
						yBounce += 2;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R / Consts.ROOT_OF_2, this.Y - R / Consts.ROOT_OF_2)).Tile.IsWall())
					{
						xBounce -= 2;
						yBounce += 2;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R / Consts.ROOT_OF_2, this.Y + R / Consts.ROOT_OF_2)).Tile.IsWall())
					{
						xBounce += 2;
						yBounce -= 2;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R / Consts.ROOT_OF_2, this.Y + R / Consts.ROOT_OF_2)).Tile.IsWall())
					{
						xBounce -= 2;
						yBounce -= 2;
					}

					DDUtils.ToRange(ref xBounce, -1, 1);
					DDUtils.ToRange(ref yBounce, -1, 1);

					bool bounced = xBounce != 0 || yBounce != 0;

					if (bounced)
					{
						bouncedCount++;

						if (20 <= bouncedCount) // ? 跳ね返り回数オーバー
						{
							//DDGround.EL.Add(SCommon.Supplier(Effects.FireBall爆発(this.X, this.Y)));
							break;
						}
					}

					if (xBounce == -1)
					{
						this.FacingLeft = true;
					}
					else if (xBounce == 1)
					{
						this.FacingLeft = false;
					}

					if (yBounce == -1)
					{
						if (0.0 < yAdd)
							yAdd *= -K;
					}
					else if (yBounce == 1)
					{
						if (yAdd < 0.0)
							yAdd *= -K;
					}
				}

				DDDraw.DrawBegin(Ground.I.Picture2.陰陽玉, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawSetSize(R * 2, R * 2);
				DDDraw.DrawRotate(frame / 10.0);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), R); // カメラから出たら消滅する。
			}
		}
	}
}
