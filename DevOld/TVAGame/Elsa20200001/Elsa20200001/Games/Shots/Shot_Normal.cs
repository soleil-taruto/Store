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
		public Shot_Normal(double x, double y, int direction)
			: base(x, y, direction, 3, false, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			D2Point speed = GameCommon.GetXYSpeed(this.Direction, 10.0);

			for (int frame = 0; ; frame++)
			{
				this.X += speed.X;
				this.Y += speed.Y;

				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawRotate(frame / 2.0);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 10.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)); // カメラの外に出たら(画面から見えなくなったら)消滅する。
			}
		}
	}
}
