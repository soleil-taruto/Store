using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots.Tests
{
	/// <summary>
	/// テスト用_自弾
	/// </summary>
	public class Shot_B0001 : Shot
	{
		private bool FacingLeft;

		public Shot_B0001(double x, double y, bool facingLeft)
			: base(x, y, 1, false)
		{
			this.FacingLeft = facingLeft;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.X += 8.0 * (this.FacingLeft ? -1 : 1);

				if (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y))) // カメラから出たら消滅する。
					break;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y)).Tile.IsWall()) // 壁に当たったら自滅する。
				{
					this.Kill();
					break;
				}

				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 5.0);

				yield return true;
			}
		}
	}
}
