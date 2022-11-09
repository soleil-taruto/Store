using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests.神奈子s
{
	public class Enemy_B神奈子_Tama : Enemy
	{
		private const double SPEED = 6.0;
		private const double R = 20;

		private double XAdd;
		private double YAdd;

		public Enemy_B神奈子_Tama(double x, double y)
			: base(x, y, 0, 1, true)
		{
			DDUtils.MakeXYSpeed(this.X, this.Y, Game.I.Player.X, Game.I.Player.Y, SPEED, out this.XAdd, out this.YAdd);
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.X += this.XAdd;
				this.Y += this.YAdd;

				DDDraw.SetBright(new I3Color(255, 64, 255));
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X, this.Y);
				DDDraw.DrawSetSize(R * 2, R * 2);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);

				yield return true;
			}
		}

		protected override void P_Killed(bool destroyed)
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.B大爆発(this.X, this.Y)));
		}
	}
}
