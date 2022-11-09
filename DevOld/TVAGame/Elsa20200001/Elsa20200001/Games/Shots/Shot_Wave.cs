using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_Wave : Shot
	{
		private bool 左回転;

		public Shot_Wave(double x, double y, int direction, bool 左回転)
			: base(x, y, direction, 2, false, false)
		{
			this.左回転 = 左回転;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double ctrx = this.X;
			double ctry = this.Y;

			D2Point speed = GameCommon.GetXYSpeed(this.Direction, 5.5);

			for (int frame = 0; ; frame++)
			{
				ctrx += speed.X;
				ctry += speed.Y;

				double xadd = speed.Y * 20.0;
				double yadd = speed.X * 20.0;

				if (this.左回転)
					xadd *= -1.0;
				else
					yadd *= -1.0;

				double rate = Math.Sin(frame / 20.0);

				xadd *= rate;
				yadd *= rate;

				this.X = ctrx + xadd;
				this.Y = ctry + yadd;

				DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawRotate(frame / 2.0);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 10.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)); // カメラの外に出たら(画面から見えなくなったら)消滅する。
			}
		}
	}
}
