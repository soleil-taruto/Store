using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Walls
{
	public class Wall_東方風 : Wall
	{
		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				double map_w = Game.I.Map.W * GameConsts.TILE_W;
				double map_h = Game.I.Map.H * GameConsts.TILE_H;
				double camXMax = map_w - DDConsts.Screen_W;
				double camYMax = map_h - DDConsts.Screen_H;

				// 最背面
				{
					double xRate = DDGround.Camera.X / camXMax;
					double yRate = DDGround.Camera.Y / camYMax;

					DDDraw.DrawSimple(
						Ground.I.Picture.Stage01_Bg_a01,
						-(Ground.I.Picture.Stage01_Bg_a01.Get_W() - DDConsts.Screen_W) * xRate,
						-(Ground.I.Picture.Stage01_Bg_a01.Get_H() - DDConsts.Screen_H) * yRate
						);
				}

				// 山
				{
					DDPicture picture = Ground.I.Picture.Stage01_Bg_a02;
					double xSlideRate = 0.2;
					double ySlideRate = 0.2;
					double x = -DDGround.Camera.X * xSlideRate;
					double y = (DDConsts.Screen_H - 500.0) + (camYMax - DDGround.Camera.Y) * ySlideRate;
					int ix = (int)x;
					int iy = (int)y;

					ix %= picture.Get_W();

					for (int cx = ix; cx < DDConsts.Screen_W; cx += picture.Get_W())
					{
						DDDraw.DrawSimple(picture, cx, iy);
					}
				}

				// 森
				{
					DDPicture picture = Ground.I.Picture.Stage01_Bg_a03;
					double xSlideRate = 0.3;
					double ySlideRate = 0.3;
					double x = -DDGround.Camera.X * xSlideRate;
					double y = (DDConsts.Screen_H - 416.0) + (camYMax - DDGround.Camera.Y) * ySlideRate;
					int ix = (int)x;
					int iy = (int)y;

					ix %= picture.Get_W();

					for (int cx = ix; cx < DDConsts.Screen_W; cx += picture.Get_W())
					{
						DDDraw.DrawSimple(picture, cx, iy);
					}
					iy += picture.Get_H();

					if (iy < DDConsts.Screen_H)
					{
						//DDDraw.SetBright(new I3Color(15, 25, 19));
						DDDraw.SetBright(new I3Color(16, 48, 16));
						DDDraw.DrawRect_LTRB(Ground.I.Picture.WhiteBox, 0, iy, DDConsts.Screen_W, DDConsts.Screen_H);
						DDDraw.Reset();
					}
				}

				// 木
				{
					DDPicture picture_01 = Ground.I.Picture.Stage01_Bg_Item01;
					DDPicture picture_02 = Ground.I.Picture.Stage01_Bg_Item02;

					const int CYCLE_SPAN = 1000;
					const int WOOD_01_X = 200;
					const int WOOD_02_X = 700;

					int x = -DDGround.ICamera.X;
					int y = (int)(map_h - 96 - DDGround.ICamera.Y);

					x %= CYCLE_SPAN;
					x -= CYCLE_SPAN;

					for (int cx = x; cx < DDConsts.Screen_W; cx += CYCLE_SPAN)
					{
						DDDraw.DrawSimple(picture_01, cx + WOOD_01_X, y - picture_01.Get_H());
						DDDraw.DrawSimple(picture_02, cx + WOOD_02_X, y - picture_02.Get_H());
					}
				}

				yield return true;
			}
		}
	}
}
