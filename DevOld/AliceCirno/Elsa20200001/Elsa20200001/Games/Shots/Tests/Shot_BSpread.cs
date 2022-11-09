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
	public class Shot_BSpread : Shot
	{
		private int Direction; // この自弾の進行方向(8方向_テンキー方式)
		private double R;

		public Shot_BSpread(double x, double y, int direction, double r)
			: base(x, y, 1, false)
		{
			this.Direction = direction;
			this.R = r;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			D2Point speed = GameCommon.GetXYSpeed(this.Direction, 10.0);

			DDUtils.Rotate(ref speed.X, ref speed.Y, this.R);

			for (int frame = 0; ; frame++)
			{
				this.X += speed.X;
				this.Y += speed.Y;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y)).Tile.GetKind() == Tile.Kind_e.WALL) // 壁に当たったら自滅する。
				{
					this.Kill();
					break;
				}

				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
				DDDraw.DrawRotate(frame / 2.0);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 10.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)); // カメラの外に出たら(画面から見えなくなったら)消滅する。
			}
		}
	}
}
