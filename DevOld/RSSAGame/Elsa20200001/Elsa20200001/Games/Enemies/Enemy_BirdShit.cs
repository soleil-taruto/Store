using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_BirdShit : Enemy
	{
		public Enemy_BirdShit(double x, double y)
			: base(x, y, 0, 1, true)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			double ySpeed = 0.0;

			const double GRAVITY = 1.0;
			const double SPEED_Y_MAX = 8.0;

			for (; ; )
			{
				this.Y += ySpeed;

				ySpeed += GRAVITY;
				ySpeed = Math.Min(ySpeed, SPEED_Y_MAX);

				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X, this.Y))).Tile.IsGround()) // ? 地面に落ちた。
					break;

				DDDraw.DrawCenter(Ground.I.Picture.Teki_a01_Shit01, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);

				this.Crash = DDCrashUtils.Point(new D2Point(this.X, this.Y));

				yield return true;
			}
		}
	}
}
