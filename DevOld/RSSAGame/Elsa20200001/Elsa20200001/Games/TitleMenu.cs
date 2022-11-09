using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameProgressMasters;
using Charlotte.Games.Tests;
using Charlotte.Novels;

namespace Charlotte.Games
{
	public class TitleMenu : IDisposable
	{
		public static TitleMenu I;

		public TitleMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		#region DrawWall

		private DrawWallTask DrawWall = new DrawWallTask();

		private class DrawWallTask : DDTask
		{
			public bool TopMenuLeaved = false;

			public override IEnumerable<bool> E_Task()
			{
				DDTaskList el = new DDTaskList();

				//el.Add(SCommon.Supplier(this.Effect_0001(1, 2, 3)));
				//el.Add(SCommon.Supplier(this.Effect_0001(4, 5, 6)));
				//el.Add(SCommon.Supplier(this.Effect_0001(7, 8, 9)));

				for (int frame = 0; ; frame++)
				{
					DDDraw.SetBright(new I3Color(32, 0, 0));
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();

					if (!this.TopMenuLeaved)
					{
						DDPrint.SetBorder(new I3Color(128, 0, 0));
						DDPrint.SetPrint(30, 30, 0, 60);
						DDPrint.Print("〇ックマン風(仮)");
						DDPrint.Reset();
					}

					el.ExecuteAllTask_Reverse();

					yield return true;
				}
			}

			private IEnumerable<bool> Effect_0001(int dummy_01, int dummy_02, int dummy_03)
			{
				for (; ; )
					yield return true;
			}
		}

		#endregion

		#region TopMenu

		private TopMenuTask TopMenu = new TopMenuTask();

		private class TopMenuTask : DDTask
		{
			public const int ITEM_NUM = 5;
			public int SelectIndex = 0;

			public override IEnumerable<bool> E_Task()
			{
				Func<bool>[] drawItems = new Func<bool>[ITEM_NUM];

				for (int index = 0; index < ITEM_NUM; index++)
					drawItems[index] = SCommon.Supplier(this.E_DrawItem(index));

				for (; ; )
				{
					for (int index = 0; index < ITEM_NUM; index++)
						drawItems[index]();

					yield return true;
				}
			}

			private IEnumerable<bool> E_DrawItem(int selfIndex)
			{
				DDPicture picture = Ground.I.Picture.TitleMenuItems[selfIndex];

				const double ITEM_UNSEL_X = 160.0;
				const double ITEM_UNSEL_A = 0.5;
				const double ITEM_SEL_X = 180.0;
				const double ITEM_SEL_A = 1.0;
				const double ITEM_Y = 270.0;
				const double ITEM_Y_STEP = 50.0;

				double x = ITEM_SEL_X;
				double y = ITEM_Y + selfIndex * ITEM_Y_STEP;
				double a = ITEM_UNSEL_A;
				double realX = ITEM_UNSEL_X;
				double realY = y;
				double realA = a;

				for (; ; )
				{
					x = this.SelectIndex == selfIndex ? ITEM_SEL_X : ITEM_UNSEL_X;
					a = this.SelectIndex == selfIndex ? ITEM_SEL_A : ITEM_UNSEL_A;

					DDUtils.Approach(ref realX, x, 0.93);
					DDUtils.Approach(ref realA, a, 0.93);

					DDDraw.SetAlpha(realA);
					DDDraw.DrawCenter(picture, realX, realY);
					DDDraw.Reset();

					yield return true;
				}
			}
		}

		#endregion

		private DDSimpleMenu SimpleMenu;

		public void Perform()
		{
			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			DDEngine.FreezeInput();

			Ground.I.Music.Title.Play();

			this.SimpleMenu = new DDSimpleMenu()
			{
				BorderColor = new I3Color(64, 0, 0),
				WallDrawer = this.DrawWall.Execute,
			};

			this.TopMenu.SelectIndex = 0;

			for (; ; )
			{
				bool cheatFlag;

				{
					int bk_freezeInputFrame = DDEngine.FreezeInputFrame;
					DDEngine.FreezeInputFrame = 0;
					cheatFlag = 1 <= DDInput.DIR_6.GetInput();
					DDEngine.FreezeInputFrame = bk_freezeInputFrame;
				}

				if (DDInput.DIR_8.IsPound())
					this.TopMenu.SelectIndex--;

				if (DDInput.DIR_2.IsPound())
					this.TopMenu.SelectIndex++;

				this.TopMenu.SelectIndex += TopMenuTask.ITEM_NUM;
				this.TopMenu.SelectIndex %= TopMenuTask.ITEM_NUM;

				if (DDInput.A.GetInput() == 1) // ? 決定ボタン押下
				{
					switch (this.TopMenu.SelectIndex)
					{
						case 0:
							if (DDConfig.LOG_ENABLED && cheatFlag)
							{
								this.DrawWall.TopMenuLeaved = true;
								this.CheatMainMenu();
								this.DrawWall.TopMenuLeaved = false; // restore
							}
							else
							{
								this.LeaveTitleMenu();

								using (new GameProgressMaster())
								{
									GameProgressMaster.I.Perform();
								}
								this.ReturnTitleMenu();
							}
							break;

						case 1:
							{
								this.LeaveTitleMenu();

								using (new GameProgressMaster())
								{
									GameProgressMaster.I.Perform_コンテニュー();
								}
								this.ReturnTitleMenu();
							}
							break;

						case 2:
							{
								this.DrawWall.TopMenuLeaved = true;

								using (new OmakeMenu())
								{
									OmakeMenu.I.SimpleMenu = this.SimpleMenu;
									OmakeMenu.I.Perform();
								}
								this.DrawWall.TopMenuLeaved = false; // restore
							}
							break;

						case 3:
							{
								this.DrawWall.TopMenuLeaved = true;

								using (new SettingMenu())
								{
									SettingMenu.I.SimpleMenu = this.SimpleMenu;
									SettingMenu.I.Perform();
								}
								this.DrawWall.TopMenuLeaved = false; // restore
							}
							break;

						case 4:
							goto endMenu;

						default:
							throw new DDError();
					}
				}
				if (DDInput.B.GetInput() == 1) // ? キャンセルボタン押下
				{
					if (this.TopMenu.SelectIndex == TopMenuTask.ITEM_NUM - 1)
						break;

					this.TopMenu.SelectIndex = TopMenuTask.ITEM_NUM - 1;
				}

				this.DrawWall.Execute();
				this.TopMenu.Execute();

				DDEngine.EachFrame();
			}
		endMenu:
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.WallDrawer();
				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();
		}

		private void CheatMainMenu()
		{
			Action<string> a_gameStart = startMapName =>
			{
				this.LeaveTitleMenu();

				using (new WorldGameMaster())
				{
					WorldGameMaster.I.World = new World(startMapName);
					WorldGameMaster.I.Status = new GameStatus();
					WorldGameMaster.I.Perform();
				}
				this.ReturnTitleMenu();
			};

			for (; ; )
			{
				int selectIndex = this.SimpleMenu.Perform(20, 20, 30, 24, "開発デバッグ用メニュー", new string[]
				{
					"ノベルパートテスト",
					"Stage1", // 左下
					"Stage2", // 下
					"Stage3", // 右下
					"Stage4", // 左
					"Stage5", // 中央
					"Stage6", // 右
					"Stage7", // 左上
					"Stage8", // 上
					"Stage9", // 右上
					"Stage5-2", // 中央(2面)
					"Stage5-3", // 中央(3面)
					"Stage5-4", // 中央(4面)
					"Game用テストメニュー",
					"Game用テストメニュー.2",
					"戻る",
				},
				0
				);

				switch (selectIndex)
				{
					case 0:
						{
							this.LeaveTitleMenu();

							using (new Novel())
							{
								Novel.I.Status.Scenario = new Scenario("Tests/テスト0001");
								Novel.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 1: a_gameStart("Stage1/t1001"); break;
					case 2: a_gameStart("Stage2/t1001"); break;
					case 3: a_gameStart("Stage3/t1001"); break;
					case 4: a_gameStart("Stage4/t1001"); break;
					case 5: a_gameStart("Stage5/t1001"); break;
					case 6: a_gameStart("Stage6/t1001"); break;
					case 7: a_gameStart("Stage7/t1001"); break;
					case 8: a_gameStart("Stage8/t1001"); break;
					case 9: a_gameStart("Stage9/t1001"); break;
					case 10: a_gameStart("Stage5/t2001"); break;
					case 11: a_gameStart("Stage5/t3001"); break;
					case 12: a_gameStart("Stage5/t4001"); break;
					case 13:
						{
							this.LeaveTitleMenu();

							using (new GameTestMenu())
							{
								//GameTestMenu.I.SimpleMenu = this.SimpleMenu; // 不用
								GameTestMenu.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 14:
						{
							this.LeaveTitleMenu();

							using (new GameTestMenu2())
							{
								GameTestMenu2.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 15:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			;
		}

		private void LeaveTitleMenu()
		{
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.WallDrawer();
				DDEngine.EachFrame();
			}

			GC.Collect();
		}

		private void ReturnTitleMenu()
		{
			DDTouch.Touch(); // 曲再生の前に -- .Play() で Touch した曲を解放してしまわないように
			Ground.I.Music.Title.Play();

			//DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			GC.Collect();
		}
	}
}
