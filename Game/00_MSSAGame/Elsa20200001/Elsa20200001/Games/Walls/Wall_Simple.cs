using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls
{
	public class Wall_Simple : Wall
	{
		private DDPicture Picture;

		public Wall_Simple(DDPicture picture)
		{
			this.Picture = picture;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			const double SLIDE_RATE = 0.1;

			int cam_w = Game.I.Map.W * GameConsts.TILE_W - DDConsts.Screen_W;
			int cam_h = Game.I.Map.H * GameConsts.TILE_H - DDConsts.Screen_H;

			double slide_w = cam_w * SLIDE_RATE;
			double slide_h = cam_h * SLIDE_RATE;

			double wall_w = slide_w + DDConsts.Screen_W;
			double wall_h = slide_h + DDConsts.Screen_H;

			D4Rect wallRect = DDUtils.AdjustRectExterior(
				this.Picture.GetSize().ToD2Size(),
				new D4Rect(0.0, 0.0, wall_w, wall_h)
				);

			for (; ; )
			{
				double x = cam_w == 0 ? 0.0 : DDGround.RealCamera.X / cam_w;
				double y = cam_h == 0 ? 0.0 : DDGround.RealCamera.Y / cam_h;

				x *= slide_w;
				y *= slide_h;

				DDDraw.DrawRect(
					this.Picture,
					wallRect.L - x,
					wallRect.T - y,
					wallRect.W,
					wallRect.H
					);

				DDCurtain.DrawCurtain(-0.5);

				yield return true;
			}
		}
	}
}
