using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots.Tests
{
	/// <summary>
	/// 通常弾
	/// </summary>
	public class Shot_Test0001 : Shot
	{
		public Shot_Test0001(double x, double y)
			: base(x, y, 1, false, Kind_e.通常弾)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.X += 10;

				DDDraw.SetAlpha(0.5);
				DDDraw.DrawCenter(Ground.I.Picture.Shot0001, this.X, this.Y);
				DDDraw.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 16.0);

				yield return !DDUtils.IsOutOfScreen(new D2Point(this.X, this.Y), 16.0);
			}
		}
	}
}
