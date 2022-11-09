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
	public class Enemy_Test0002 : Enemy
	{
		public Enemy_Test0002(double x, double y)
			: base(x, y, 10, Kind_e.通常敵)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (int frame = 1; ; frame++)
			{
				if (frame % 20 == 0)
					Game.I.Enemies.Add(new Enemy_TestTama0001(this.X, this.Y));

				D2Point speed = DDUtils.AngleToPoint(
					DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y),
					2.5
					);

				this.X += speed.X;
				this.Y += speed.Y;

				DDDraw.DrawCenter(Ground.I.Picture.Enemy0002, this.X, this.Y);

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 64.0);

				yield return true; // 自機をホーミングするので、画面外に出て行かない。
			}
		}
	}
}
