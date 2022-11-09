using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_敵弾
	/// </summary>
	public class Enemy_TestTama0001 : Enemy
	{
		public Enemy_TestTama0001(double x, double y)
			: base(x, y, 0, Kind_e.通常敵)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			D2Point speed = DDUtils.AngleToPoint(DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y), 8.0);

			for (; ; )
			{
				this.X += speed.X;
				this.Y += speed.Y;

				DDDraw.DrawCenter(Ground.I.Picture.Tama0001, this.X, this.Y);

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 16.0);

				yield return !DDUtils.IsOutOfScreen(new D2Point(this.X, this.Y), 16.0);
			}
		}

		protected override void P_Damaged(Shot shot, int damagePoint)
		{
			// noop
		}

		protected override void P_Killed(bool destroyed)
		{
			// noop
		}
	}
}
