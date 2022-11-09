using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Walls
{
	public class Wall_Simple : Wall
	{
		private DDPicture Picture;
		private double Zoom;

		public Wall_Simple(DDPicture picture)
		{
			this.Picture = picture;
			this.Zoom = this.GetZoom();
		}

		private const double SLIDE_RATE = 0.1;

		private double GetZoom()
		{
			double w = DDConsts.Screen_W + (Game.I.Map.W * GameConsts.TILE_W - DDConsts.Screen_W) * SLIDE_RATE;
			double h = DDConsts.Screen_H + (Game.I.Map.H * GameConsts.TILE_H - DDConsts.Screen_H) * SLIDE_RATE;

			double zw = w / this.Picture.Get_W();
			double zh = h / this.Picture.Get_H();

			double z = Math.Max(zw, zh);

			z *= 1.01; // margin

			return z;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				DDDraw.DrawBegin(this.Picture, DDConsts.Screen_W / 2.0, DDConsts.Screen_H / 2.0);
				DDDraw.DrawZoom(this.Zoom);
				DDDraw.DrawSlide(
					((Game.I.Map.W * GameConsts.TILE_W - DDConsts.Screen_W) / 2.0 - DDGround.Camera.X) * SLIDE_RATE,
					((Game.I.Map.H * GameConsts.TILE_H - DDConsts.Screen_H) / 2.0 - DDGround.Camera.Y) * SLIDE_RATE
					);
				DDDraw.DrawEnd();

				DDCurtain.DrawCurtain(-0.5);

				yield return true;
			}
		}
	}
}
