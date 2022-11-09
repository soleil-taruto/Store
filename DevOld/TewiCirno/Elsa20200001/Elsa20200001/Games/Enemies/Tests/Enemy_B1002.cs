using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	public class Enemy_B1002 : Enemy
	{
		private double XAdd;
		private double YAdd;

		public Enemy_B1002(double x, double y, double xAdd, double yAdd)
			: base(x, y, 1, 1, true)
		{
			this.XAdd = xAdd;
			this.YAdd = yAdd;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			int bouncedCount = 0;

			for (; ; )
			{
				this.YAdd += 0.3; // 重力加速度

				this.YAdd = Math.Min(this.YAdd, 8.0); // 落下最高速度

				this.X += this.XAdd;
				this.Y += this.YAdd;

				const double R = 20.0;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y)).Tile.IsWall())
				{
					this.XAdd = Math.Abs(this.XAdd);
				}
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y)).Tile.IsWall())
				{
					this.XAdd = Math.Abs(this.XAdd) * -1;
				}
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y + R)).Tile.IsWall())
				{
					this.YAdd = Math.Abs(this.YAdd) * -1;
					bouncedCount++;

					if (5 <= bouncedCount) // バウンド回数上限
					{
						//DDGround.EL.Add(SCommon.Supplier(Effects.B中爆発(this.X, this.Y)));
						this.Kill();
						break;
					}
				}

				DDDraw.SetBright(1.0, 0.25, 0.25);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
				DDDraw.DrawSetSize(R * 2.0, R * 2.0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);

				yield return true;
			}
		}
	}
}
