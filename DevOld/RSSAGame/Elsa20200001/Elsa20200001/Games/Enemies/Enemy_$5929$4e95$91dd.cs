using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_天井針 : Enemy
	{
		public Enemy_天井針(double x, double y)
			: base(x, y, 0, 4, true)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			bool falling = false;
			double ySpeed = 0.0;

			const double GRAVITY = 0.5;
			const double SPEED_Y_MAX = 8.0;

			for (; ; )
			{
				if (
					this.Y < Game.I.Player.Y &&
					Math.Abs(Game.I.Player.X - this.X) < 50.0
					)
					falling = true;

				if (falling)
				{
					this.Y += ySpeed;

					ySpeed += GRAVITY;
					ySpeed = Math.Min(ySpeed, SPEED_Y_MAX);

					I2Point pt = GameCommon.ToTablePoint(new D2Point(this.X, this.Y));

					if (Game.I.Map.GetCell(pt).Tile.IsGround()) // ? 地面に落ちた。
					{
						this.DeadFlag = true;

						DDGround.EL.Add(SCommon.Supplier(this.E_地面に落ちた(
							pt.X * GameConsts.TILE_W + GameConsts.TILE_W / 2,
							pt.Y * GameConsts.TILE_H - GameConsts.TILE_H / 2
							)));

						break;
					}
				}

				DDDraw.DrawCenter(Ground.I.Picture.Stage02_Chip_g04_01, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);

				this.Crash = DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(16, 32));

				yield return true;
			}
		}

		private IEnumerable<bool> E_地面に落ちた(double x, double y)
		{
			DDPicture[] pictures = new DDPicture[]
			{
				Ground.I.Picture.Stage02_Chip_g04_02,
				Ground.I.Picture.Stage02_Chip_g04_03,
				Ground.I.Picture.Stage02_Chip_g04_04,
			};

			foreach (DDPicture picture in pictures)
			{
				for (int c = 0; c < 4; c++)
				{
					DDDraw.DrawCenter(picture, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);

					yield return true;
				}
			}
		}
	}
}
