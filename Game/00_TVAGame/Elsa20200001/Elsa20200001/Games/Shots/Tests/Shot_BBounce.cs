using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

namespace Charlotte.Games.Shots.Tests
{
	/// <summary>
	/// テスト用_自弾
	/// </summary>
	public class Shot_BBounce : Shot
	{
		private int Direction; // この自弾の進行方向(8方向_テンキー方式)

		public Shot_BBounce(double x, double y, int direction)
			: base(x, y, 1, false)
		{
			this.Direction = direction;
		}

		private const int BOUNCE_MAX = 3;

		protected override IEnumerable<bool> E_Draw()
		{
			D2Point speed = GameCommon.GetXYSpeed(this.Direction, 8.0);
			int bouncedCount = 0;

			for (int frame = 0; ; frame++)
			{
				this.X += speed.X;
				this.Y += speed.Y;

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
				if (bounced) // ? 壁に当たった。
				{
					if (bouncedCount < BOUNCE_MAX) // ? まだ跳ね返り可能
					{
						DDGround.EL.Add(SCommon.Supplier(Effects.B跳ねた(this.X, this.Y)));
						bouncedCount++;
					}
					else // ? 跳ね返り不能
					{
						this.Kill();
						break;
					}
				}

				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
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
