using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_敵
	/// </summary>
	public class Enemy_Test0001 : Enemy
	{
		public Enemy_Test0001(double x, double y)
			: base(x, y, 1, Kind_e.通常敵)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.X -= 3.0;
				DDDraw.DrawCenter(Ground.I.Picture.Enemy0001, this.X, this.Y);
				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 48.0);
				yield return !DDUtils.IsOutOfScreen(new D2Point(this.X, this.Y), 48.0);
			}
		}
	}
}
