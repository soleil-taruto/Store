using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

namespace Charlotte.Games.Shots
{
	public class Shot_Bounce : Shot
	{
		// 跳ね返り可能な間は「壁をすり抜ける」を真にしておく。

		public Shot_Bounce(double x, double y, int direction)
			: base(x, y, direction, 1, true, false)
		{ }

		private const int BOUNCE_MAX = 3;

		protected override IEnumerable<bool> E_Draw()
		{
			D2Point speed = GameCommon.GetXYSpeed(this.Direction, 8.0);
			int bouncedCount = 0;

			for (int frame = 0; ; frame++)
			{
				this.X += speed.X;
				this.Y += speed.Y;

				if (this.壁をすり抜ける) // ? 跳ね返り可能
				{
					bool bounced = false;

					if (this.IsInsideWall(-10, 0) || this.IsInsideWall(10, 0))
					{
						speed.X *= -1.0;
						bounced = true;
					}
					if (this.IsInsideWall(0, -10) || this.IsInsideWall(0, 10))
					{
						speed.Y *= -1.0;
						bounced = true;
					}
					if (bounced)
					{
						DDGround.EL.Add(SCommon.Supplier(Effects.跳ねた(this.X, this.Y)));
						bouncedCount++;

						if (BOUNCE_MAX <= bouncedCount)
							this.壁をすり抜ける = false;
					}
				}
				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawRotate(frame / 2.0);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 10.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)); // カメラの外に出たら(画面から見えなくなったら)消滅する。
			}
		}

		private bool IsInsideWall(double xa, double ya)
		{
			int x = SCommon.ToInt(this.X + xa) / GameConsts.TILE_W;
			int y = SCommon.ToInt(this.Y + ya) / GameConsts.TILE_H;

			return Game.I.Map.GetCell(x, y).Tile.GetKind() == Tile.Kind_e.WALL;
		}
	}
}
