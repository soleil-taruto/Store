using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_FireBall : Shot
	{
		public Shot_FireBall(double x, double y, bool facingLeft)
			: base(x, y, facingLeft, 2, true, false) // 自力で壁から跳ねるので、壁貫通にしておく。
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			// 薄い壁にくっついて撃つと、壁の向こうに射出されてしまうのを防ぐ
			{
				foreach (int xd in new int[] { 0, 20, 40 })
				{
					foreach (int yd in new int[] { -10, 0, 10 })
					{
						if (Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X - xd * (this.FacingLeft ? -1 : 1), this.Y + yd))).Tile.IsWall())
						{
							DDGround.EL.Add(SCommon.Supplier(Effects.FireBall爆発(this.X, this.Y)));
							goto endFunc;
							//yield break;
						}
					}
				}
			}

			double yAdd = 0.0;
			int bouncedCount = 0;

			for (int frame = 0; ; frame++)
			{
				this.X += 8.0 * (this.FacingLeft ? -1 : 1);
				this.Y += yAdd;

				yAdd += 0.8; // += 重力加速度

				DDUtils.Minim(ref yAdd, 19.0); // 落下速度制限

				// 跳ね返り
				{
					const double R = 20.0;
					bool bounced = false;

					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y)).Tile.IsWall())
					{
						this.FacingLeft = false;
						bounced = true;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y)).Tile.IsWall())
					{
						this.FacingLeft = true;
						bounced = true;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y - R)).Tile.IsWall() && yAdd < 0.0)
					{
						yAdd *= -0.98;
						bounced = true;
					}
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y + R)).Tile.IsWall() && 0.0 < yAdd)
					{
						yAdd *= -0.98;
						bounced = true;

						while (
							!Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y)).Tile.IsWall() &&
							Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y + R)).Tile.IsWall()
							)
							this.Y--;
					}

					if (bounced)
					{
						bouncedCount++;

						if (20 <= bouncedCount) // ? 跳ね返り回数オーバー
						{
							DDGround.EL.Add(SCommon.Supplier(Effects.FireBall爆発(this.X, this.Y)));
							break;
						}
					}
				}

				DDDraw.DrawBegin(Ground.I.Picture2.FireBall[14 + frame % 7], this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.25);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 20.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)); // カメラから出たら消滅する。
			}

		endFunc:
			;
		}

		protected override void Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.FireBall爆発(this.X, this.Y)));
		}
	}
}
