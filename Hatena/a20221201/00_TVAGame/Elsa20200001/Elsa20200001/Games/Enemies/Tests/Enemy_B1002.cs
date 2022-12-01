using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

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
				double accelX;
				double accelY;

				DDUtils.MakeXYSpeed(this.X, this.Y, Game.I.Player.X, Game.I.Player.Y, 0.3, out accelX, out accelY);

				this.XAdd += accelX;
				this.YAdd += accelY;

				// 加速度制限
				{
					const double ACCEL_MAX = 8.0;

					double accel = DDUtils.GetDistance(this.XAdd, this.YAdd);

					if (ACCEL_MAX < accel)
					{
						double m = ACCEL_MAX / accel;

						this.XAdd *= m;
						this.YAdd *= m;
					}
				}

				this.X += this.XAdd;
				this.Y += this.YAdd;

				const double R = 20.0;
				bool bounced = false;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - R, this.Y)).Tile.GetKind() == Tile.Kind_e.WALL)
				{
					this.XAdd = Math.Abs(this.XAdd);
					bounced = true;
				}
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + R, this.Y)).Tile.GetKind() == Tile.Kind_e.WALL)
				{
					this.XAdd = Math.Abs(this.XAdd) * -1;
					bounced = true;
				}
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y - R)).Tile.GetKind() == Tile.Kind_e.WALL)
				{
					this.YAdd = Math.Abs(this.YAdd);
					bounced = true;
				}
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y + R)).Tile.GetKind() == Tile.Kind_e.WALL)
				{
					this.YAdd = Math.Abs(this.YAdd) * -1;
					bounced = true;
				}

				if (bounced)
				{
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
