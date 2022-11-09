using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class Dungeon
	{
		public static class Layout
		{
			public const int SCREEN_W = 970;
			public const int SCREEN_H = 530;

			public static readonly D4Rect FRONT_WALL_0 = new D4Rect(30 * 0, 24 * 0, SCREEN_W - (30 + 30) * 0, SCREEN_H - (24 + 8) * 0);
			public static readonly D4Rect FRONT_WALL_1 = new D4Rect(30 * 8, 24 * 8, SCREEN_W - (30 + 30) * 8, SCREEN_H - (24 + 8) * 8);
			public static readonly D4Rect FRONT_WALL_2 = new D4Rect(30 * 12, 24 * 12, SCREEN_W - (30 + 30) * 12, SCREEN_H - (24 + 8) * 12);
			public static readonly D4Rect FRONT_WALL_3 = new D4Rect(30 * 14, 24 * 14, SCREEN_W - (30 + 30) * 14, SCREEN_H - (24 + 8) * 14);
			public static readonly D4Rect FRONT_WALL_4 = new D4Rect(30 * 15, 24 * 15, SCREEN_W - (30 + 30) * 15, SCREEN_H - (24 + 8) * 15);

			public static readonly D4Rect WALK_FRONT_WALL_0 = new D4Rect(30 * -8, 24 * -8, SCREEN_W - (30 + 30) * -16, SCREEN_H - (24 + 8) * -8);
			public static readonly D4Rect WALK_FRONT_WALL_1 = new D4Rect(30 * 4, 24 * 4, SCREEN_W - (30 + 30) * 4, SCREEN_H - (24 + 8) * 4);
			public static readonly D4Rect WALK_FRONT_WALL_2 = new D4Rect(30 * 10, 24 * 10, SCREEN_W - (30 + 30) * 10, SCREEN_H - (24 + 8) * 10);
			public static readonly D4Rect WALK_FRONT_WALL_3 = new D4Rect(30 * 13, 24 * 13, SCREEN_W - (30 + 30) * 13, SCREEN_H - (24 + 8) * 13);
			public static readonly D4Rect WALK_FRONT_WALL_4 = new D4Rect(30 * 14.5, 24 * 14.5, SCREEN_W - (30 + 30) * 14.5, SCREEN_H - (24 + 8) * 14.5);
		}

		public delegate MapWall.Kind_e GetWall_d(int x, int y, int direction);

		private static DDSubScreen Screen = new DDSubScreen(Layout.SCREEN_W, Layout.SCREEN_H);
		private static GetWall_d GetWall;
		private static Map Map;

		public static DDSubScreen GetScreen()
		{
			return Screen;
		}

		public static void Draw(GetWall_d getWall, Map map, bool walk = false)
		{
			GetWall = getWall;
			Map = map;

			Draw_Main(walk);

			GetWall = null;
			Map = null;
		}

		private static void Draw_Main(bool walk)
		{
			using (Screen.Section())
			{
				DDDraw.DrawRect(Map.BackgroundPicture, 0, 0, Layout.SCREEN_W, Layout.SCREEN_H);

				if (walk)
				{
					DrawLayer(Layout.WALK_FRONT_WALL_4, Layout.WALK_FRONT_WALL_3, 3);
					DrawLayer(Layout.WALK_FRONT_WALL_3, Layout.WALK_FRONT_WALL_2, 2);
					DrawLayer(Layout.WALK_FRONT_WALL_2, Layout.WALK_FRONT_WALL_1, 1);
					DrawLayer(Layout.WALK_FRONT_WALL_1, Layout.WALK_FRONT_WALL_0, 0);
				}
				else
				{
					DrawLayer(Layout.FRONT_WALL_4, Layout.FRONT_WALL_3, 3);
					DrawLayer(Layout.FRONT_WALL_3, Layout.FRONT_WALL_2, 2);
					DrawLayer(Layout.FRONT_WALL_2, Layout.FRONT_WALL_1, 1);
					DrawLayer(Layout.FRONT_WALL_1, Layout.FRONT_WALL_0, 0);
				}
			}
		}

		private static void DrawLayer(D4Rect frontBaseRect, D4Rect behindBaseRect, int y)
		{
			DrawWall(GetWall(0, y, 8), frontBaseRect.Poly, y + 0.5);

			int x;

			for (x = 1; ; x++)
			{
				D4Rect frontRect = frontBaseRect;

				frontRect.L = frontBaseRect.L + x * frontBaseRect.W;

				if (Layout.SCREEN_W <= frontRect.L)
					break;

				DrawWall(GetWall(x, y, 8), frontRect.Poly, y + 0.5);

				frontRect.L = frontBaseRect.L - x * frontBaseRect.W;

				DrawWall(GetWall(-x, y, 8), frontRect.Poly, y + 0.5);
			}
			for (x -= 2; 0 <= x; x--)
			{
				D4Rect frontRect = frontBaseRect;
				D4Rect behindRect = behindBaseRect;

				frontRect.L = frontBaseRect.L + x * frontBaseRect.W;
				behindRect.L = behindBaseRect.L + x * behindBaseRect.W;

				DrawWall(GetWall(x, y, 6), new P4Poly(frontRect.RT, behindRect.RT, behindRect.RB, frontRect.RB), y);

				frontRect.L = frontBaseRect.L - x * frontBaseRect.W;
				behindRect.L = behindBaseRect.L - x * behindBaseRect.W;

				DrawWall(GetWall(-x, y, 4), new P4Poly(behindRect.LT, frontRect.LT, frontRect.LB, behindRect.LB), y);
			}
		}

		private static void DrawWall(MapWall.Kind_e kind, P4Poly poly, double y)
		{
			DDPicture picture;

			switch (kind)
			{
				case MapWall.Kind_e.NONE:
					return;

				case MapWall.Kind_e.WALL:
					picture = Map.WallPicture;
					break;

				case MapWall.Kind_e.GATE:
					picture = Map.GatePicture;
					break;

				default:
					throw null; // never
			}
			double bright = 1.0 - y / 10.0;

			DDDraw.SetBright(bright, bright, bright);
			DDDraw.DrawFree(picture, poly);
			DDDraw.Reset();
		}
	}
}
