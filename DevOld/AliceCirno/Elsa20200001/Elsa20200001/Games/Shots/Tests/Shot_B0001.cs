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
	public class Shot_B0001 : Shot
	{
		private int Direction; // この自弾の進行方向(8方向_テンキー方式)

		public Shot_B0001(double x, double y, int direction)
			: base(x, y, 1, false)
		{
			this.Direction = direction;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				double speed = 8.0;
				double nanameSpeed = speed / Consts.ROOT_OF_2;

				switch (this.Direction)
				{
					case 4: this.X -= speed; break;
					case 6: this.X += speed; break;
					case 8: this.Y -= speed; break;
					case 2: this.Y += speed; break;

					case 1:
						this.X -= nanameSpeed;
						this.Y += nanameSpeed;
						break;

					case 3:
						this.X += nanameSpeed;
						this.Y += nanameSpeed;
						break;

					case 7:
						this.X -= nanameSpeed;
						this.Y -= nanameSpeed;
						break;

					case 9:
						this.X += nanameSpeed;
						this.Y -= nanameSpeed;
						break;

					default:
						throw null; // never
				}

				if (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y))) // カメラの外に出たら(画面から見えなくなったら)消滅する。
					break;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y)).Tile.GetKind() == Tile.Kind_e.WALL) // 壁に当たったら自滅する。
				{
					this.Kill();
					break;
				}

				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 5.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)); // カメラの外に出たら(画面から見えなくなったら)消滅する。
			}
		}
	}
}
