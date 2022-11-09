using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using DxLibDLL;
using Charlotte.Games.Walls;

namespace Charlotte.Games
{
	public static class マップ移動
	{
		private static DDSubScreen CurrScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
		private static DDSubScreen NextScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		/// <summary>
		/// マップ移動のモーション画面
		/// 移動方向：{ 2, 4, 6, 8 } == { 下, 左, 右, 上 }
		/// </summary>
		/// <param name="currMap">移動元のマップ</param>
		/// <param name="nextMap">移動先のマップ</param>
		/// <param name="currMapCamera">移動元のマップでのカメラ位置</param>
		/// <param name="moveDirection">移動方向</param>
		/// <param name="plStatus">プレイヤーの状態(呼び出し前：移動前の状態, 呼び出し後：移動後の状態)</param>
		public static void Perform(Map currMap, Map nextMap, D2Point currMapCamera, int exitDirection, GameStatus.StartPlayerStatusInfo plStatus)
		{
			int intoDirection = 10 - exitDirection;
			D2Point nextMapCamera;

			{
				int x1;
				int x2;
				int y1;
				int y2;

				if (
					!FindStartPoint(out x1, out y1, currMap, exitDirection) ||
					!FindStartPoint(out x2, out y2, nextMap, intoDirection)
					)
				{
					// マップ未整備・不備のため、適当な位置を設定する。-> 次マップの中央

					plStatus.X = nextMap.W * GameConsts.TILE_W / 2;
					plStatus.Y = nextMap.H * GameConsts.TILE_H / 2;

					return;
				}

				double xd = (x2 - x1) * GameConsts.TILE_W;
				double yd = (y2 - y1) * GameConsts.TILE_H;

				double x = currMapCamera.X + xd;
				double y = currMapCamera.Y + yd;

				double l = 0.0;
				double t = 0.0;
				double r = nextMap.W * GameConsts.TILE_W - DDConsts.Screen_W;
				double b = nextMap.H * GameConsts.TILE_H - DDConsts.Screen_H;

				D2Point pt;

				switch (exitDirection)
				{
					case 2: pt = new D2Point(x, t); break;
					case 4: pt = new D2Point(r, y); break;
					case 6: pt = new D2Point(l, y); break;
					case 8: pt = new D2Point(x, b); break;

					default:
						throw null; // never
				}
				nextMapCamera = pt;

				const double PL_XY_MARGIN = 10.0;

				double pl_l = PL_XY_MARGIN;
				double pl_t = PL_XY_MARGIN;
				double pl_r = nextMap.W * GameConsts.TILE_W - PL_XY_MARGIN;
				double pl_b = nextMap.H * GameConsts.TILE_H - PL_XY_MARGIN;

				switch (exitDirection)
				{
					case 2:
						plStatus.X += xd;
						plStatus.Y = pl_t;
						break;

					case 4:
						plStatus.X = pl_r;
						plStatus.Y += yd;
						break;

					case 6:
						plStatus.X = pl_l;
						plStatus.Y += yd;
						break;

					case 8:
						plStatus.X += xd;
						plStatus.Y = pl_b;
						break;

					default:
						throw null; // never
				}
			}

			DrawToScreen(CurrScreen, currMap, currMapCamera);
			DrawToScreen(NextScreen, nextMap, nextMapCamera);

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				double x1 = 0.0;
				double y1 = 0.0;
				double x2 = 0.0;
				double y2 = 0.0;

				switch (exitDirection)
				{
					case 2:
						y1 = (1.0 - scene.Rate) * DDConsts.Screen_H - DDConsts.Screen_H;
						y2 = (1.0 - scene.Rate) * DDConsts.Screen_H;
						break;

					case 4:
						x1 = scene.Rate * DDConsts.Screen_W;
						x2 = scene.Rate * DDConsts.Screen_W - DDConsts.Screen_W;
						break;

					case 6:
						x1 = (1.0 - scene.Rate) * DDConsts.Screen_W - DDConsts.Screen_W;
						x2 = (1.0 - scene.Rate) * DDConsts.Screen_W;
						break;

					case 8:
						y1 = scene.Rate * DDConsts.Screen_H;
						y2 = scene.Rate * DDConsts.Screen_H - DDConsts.Screen_H;
						break;
				}

				DDDraw.DrawSimple(CurrScreen.ToPicture(), x1, y1);
				DDDraw.DrawSimple(NextScreen.ToPicture(), x2, y2);

				DDEngine.EachFrame();
			}
		}

		private static bool FindStartPoint(out int x1, out int y1, Map map, int direction)
		{
			return map.FindCell(out x1, out y1, cell =>
			{
				string enemyName = cell.EnemyName;

				if (enemyName == GameConsts.ENEMY_NONE)
					return false;

				Enemy enemy = EnemyCatalog.Create(enemyName, -1, -1);

				return enemy is Enemy_スタート地点 && ((Enemy_スタート地点)enemy).Direction == direction;
			});
		}

		private static void DrawToScreen(DDSubScreen screen, Map map, D2Point mapCamera)
		{
			int cam_l = (int)mapCamera.X;
			int cam_t = (int)mapCamera.Y;
			int cam_r = cam_l + DDConsts.Screen_W;
			int cam_b = cam_t + DDConsts.Screen_H;

			int l = cam_l / GameConsts.TILE_W;
			int t = cam_t / GameConsts.TILE_H;
			int r = cam_r / GameConsts.TILE_W;
			int b = cam_b / GameConsts.TILE_H;

			using (screen.Section())
			{
				DX.ClearDrawScreen();

				{
					Map bk_map = Game.I.Map;
					D2Point bk_camera = DDGround.Camera;
					I2Point bk_iCamera = DDGround.ICamera;

					// Hack
					{
						Game.I.Map = map;
						DDGround.Camera = new D2Point(cam_l, cam_t);
						DDGround.ICamera = new I2Point(cam_l, cam_t);
					}

					WallCreator.Create(map.WallName).Draw();

					// restore
					{
						Game.I.Map = bk_map;
						DDGround.Camera = bk_camera;
						DDGround.ICamera = bk_iCamera;
					}
				}

				DDGround.EL.Clear();

				for (int x = l; x <= r; x++)
				{
					for (int y = t; y <= b; y++)
					{
						double cell_x = x * GameConsts.TILE_W + GameConsts.TILE_W / 2;
						double cell_y = y * GameConsts.TILE_H + GameConsts.TILE_H / 2;

						map.GetCell(x, y).Tile.Draw(cell_x - cam_l, cell_y - cam_t, x, y);
					}
				}

				DDGround.EL.ExecuteAllTask(); // 前面に表示するタイルのため
			}
		}
	}
}
