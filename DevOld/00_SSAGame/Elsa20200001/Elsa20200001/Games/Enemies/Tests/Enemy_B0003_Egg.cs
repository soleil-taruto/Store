using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	public class Enemy_B0003_Egg : Enemy
	{
		private double DestX;
		private double DestY;

		public Enemy_B0003_Egg(double x, double y, double destX, double destY)
			: base(x, y, 0, SCommon.IMAX, false)
		{
			this.DestX = destX;
			this.DestY = destY;
		}

		private const int FRAME_MAX = 40;
		private int Frame = 0;

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.P_Draw();
				yield return true;
			}
		}

		private void P_Draw()
		{
			if (FRAME_MAX < this.Frame)
			{
				Game.I.Enemies.Add(new Enemy_B0003(this.DestX, this.DestY));
				this.DeadFlag = true;
			}

			if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
			{
				double x = DDUtils.AToBRate(this.X, this.DestX, DDUtils.SCurve(this.Frame * 1.0 / FRAME_MAX));
				double y = DDUtils.AToBRate(this.Y, this.DestY, DDUtils.SCurve(this.Frame * 1.0 / FRAME_MAX));

				DDDraw.SetBright(1.0, 0.0, 0.0);
				DDDraw.DrawBegin(Ground.I.Picture.Enemy_B0003, x - DDGround.Camera.X, y - DDGround.Camera.Y);
				DDDraw.DrawRotate(this.Frame * 10.0 / FRAME_MAX);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(x, y), 50.0);
			}
			this.Frame++;
		}
	}
}
