using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Scripts;
using Charlotte.Games.Battles;

namespace Charlotte.Games
{
	public class Game : IDisposable
	{
		public Map Map;
		public GameStatus Status;

		// <---- prm

		public static Game I;

		public Game()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public int X;
		public int Y;
		public int Direction; // { 2, 4, 6, 8 } == { 南, 西, 東, 北 }

		public void Perform()
		{
			DDCurtain.SetCurtain();

			if (this.Map.Find(out this.X, out this.Y, out this.Direction, wall => wall.Script is Script_入口) == false)
				throw new DDError();

			this.Map.Music.Play();

			int lastX = -1;
			int lastY = -1;
			int lastDirection = -1;

			// ループ初回でイベント・敵とエンカウントしても良いようにダンジョンを描画しておく
			Dungeon.Draw(this.GetWall, this.Map);

			for (; ; )
			{
				// ? ループ初回 || 移動した || 方向転換した
				if (
					lastX != this.X ||
					lastY != this.Y ||
					lastDirection != this.Direction
					)
				{
					Script script = this.Map[this.X, this.Y].GetWall(this.Direction).Script;

					if (script != null)
					{
						script.Perform(); // イベント実行
					}
					else
					{
						// ? 移動した。
						if (
							lastX != this.X ||
							lastY != this.Y
							)
						{
							if (SCommon.CRandom.GetInt(20) == 0) // ? 敵とエンカウント -- 暫定
							{
								using (new Battle())
								{
									Battle.I.Perform();
								}
							}
						}
					}

					lastX = this.X;
					lastY = this.Y;
					lastDirection = this.Direction;
				}

				// 暫定
				{
					if (DDInput.PAUSE.GetInput() == 1)
						break;
				}

				//startMoveOnMap:
				if (1 <= DDInput.DIR_8.GetInput())
				{
					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map, true);
						this.Draw();
						DDEngine.EachFrame();
					}
					if (this.Map[this.X, this.Y].GetWall(this.Direction).Kind == MapWall.Kind_e.WALL) // ? 壁衝突
					{
						Ground.I.SE.CrashToWall.Play();
					}
					else
					{
						switch (this.Direction)
						{
							case 4: this.X--; break;
							case 6: this.X++; break;
							case 8: this.Y--; break;
							case 2: this.Y++; break;

							default:
								throw null; // never
						}
					}
					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw();
						DDEngine.EachFrame();
					}
					goto endMoveOnMap;
				}
				if (1 <= DDInput.DIR_4.GetInput())
				{
					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw(scene.Rate);
						DDEngine.EachFrame();
					}
					this.Direction = GameCommon.RotL(this.Direction);

					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw(scene.Rate - 1.0);
						DDEngine.EachFrame();
					}
					goto endMoveOnMap;
				}
				if (1 <= DDInput.DIR_6.GetInput())
				{
					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw(-scene.Rate);
						DDEngine.EachFrame();
					}
					this.Direction = GameCommon.RotR(this.Direction);

					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw(1.0 - scene.Rate);
						DDEngine.EachFrame();
					}
					goto endMoveOnMap;
				}
				if (1 <= DDInput.DIR_2.GetInput())
				{
					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw(-scene.Rate);
						DDEngine.EachFrame();
					}
					this.Direction = GameCommon.RotR(this.Direction);

					foreach (DDScene scene in DDSceneUtils.Create(10))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw((1.0 - scene.Rate) * 2.0 - 1.0);
						DDEngine.EachFrame();
					}
					this.Direction = GameCommon.RotR(this.Direction);

					foreach (DDScene scene in DDSceneUtils.Create(5))
					{
						Dungeon.Draw(this.GetWall, this.Map);
						this.Draw(1.0 - scene.Rate);
						DDEngine.EachFrame();
					}
					goto endMoveOnMap;
				}
			endMoveOnMap:

				Dungeon.Draw(this.GetWall, this.Map);
				this.Draw();
				DDEngine.EachFrame();
			}

			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			DDMain.KeepMainScreen();

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
				DDEngine.EachFrame();
			}
		}

		public MapWall.Kind_e GetWall(int x, int y, int direction)
		{
			switch (Game.I.Direction)
			{
				case 4: return Game.I.Map[Game.I.X - y, Game.I.Y - x].GetWall(GameCommon.RotL(direction)).Kind;
				case 6: return Game.I.Map[Game.I.X + y, Game.I.Y + x].GetWall(GameCommon.RotR(direction)).Kind;
				case 8: return Game.I.Map[Game.I.X + x, Game.I.Y - y].GetWall(direction).Kind;
				case 2: return Game.I.Map[Game.I.X - x, Game.I.Y + y].GetWall(10 - direction).Kind;

				default:
					throw null; // never
			}
		}

		/// <summary>
		/// ダンジョン中のゲーム画面の描画を行う。
		/// 主要なゲーム画面であるため、色々なところから呼び出される想定
		/// -- イベント中からも呼び出される想定
		/// </summary>
		public void Draw(double xSlideRate = 0.0)
		{
			DDDraw.DrawCenter(Dungeon.GetScreen().ToPicture(), DDConsts.Screen_W / 2 + xSlideRate * 90.0, DDConsts.Screen_H / 2 - 150);

#if true
			DDDraw.DrawSimple(Ground.I.Picture.ダンジョン枠, 0, 0);
#else
			// 仮枠線
			{
				DDDraw.SetBright(0, 0, 0);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, 85, DDConsts.Screen_H);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, DDConsts.Screen_W - 85, 0, 85, DDConsts.Screen_H);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, 10);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 385, DDConsts.Screen_W, DDConsts.Screen_H - 385);
				DDDraw.Reset();
			}
#endif
		}
	}
}
