using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_Normal : Shot
	{
		private bool FacingLeft;
		private bool UwamukiFlag;

		public Shot_Normal(double x, double y, bool facingLeft, bool uwamukiFlag)
			: base(x, y, 1, false)
		{
			this.FacingLeft = facingLeft;
			this.UwamukiFlag = uwamukiFlag;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			// 初期位置で壁に当たっていたら自滅する。
			// -- 薄い壁にくっついて撃つと、壁の向こうに射出されてしまうのを防ぐ
			if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y)).Tile.IsWall())
			{
				this.Kill();
				yield break;
			}

			for (; ; )
			{
#if true
				this.X += 8.0 * (this.FacingLeft ? -1 : 1);
#else // for Pochimetto
				if (this.UwamukiFlag)
					this.Y -= 12.0;
				else
					this.X += 12.0 * (this.FacingLeft ? -1 : 1);
#endif

				if (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y))) // カメラから出たら消滅する。
					break;

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y)).Tile.IsWall()) // 壁に当たったら自滅する。
				{
					this.Kill();
					break;
				}

				DDDraw.SetBright(0.0, 1.0, 0.5);
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
				DDDraw.DrawSetSize(10.0, 10.0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 5.0);

				yield return true;
			}
		}
	}
}
