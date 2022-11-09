using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;
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
					DDPicture picture = Ground.I.Picture.Title;

					DDDraw.DrawRect(
						picture,
						DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
						);

					DDCurtain.DrawCurtain(-0.8);

					if (!this.TopMenuLeaved)
					{
						DDPrint.SetBorder(new I3Color(128, 0, 0));
						DDPrint.SetPrint(75, 110, 0, 60);
						DDPrint.Print("横スクロール アクション(仮)");
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

		private SimpleMenu SimpleMenu;

		public void Perform()
		{
			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			DDEngine.FreezeInput();

			Ground.I.Music.Title.Play();

			this.SimpleMenu = new SimpleMenu()
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
									GameProgressMaster.I.StartGame();
								}
								this.ReturnTitleMenu();
							}
							break;

						case 1:
							{
								Ground.P_SaveDataSlot saveDataSlot = LoadGame();

								if (saveDataSlot != null)
								{
									this.LeaveTitleMenu();

									using (new GameProgressMaster())
									{
										GameProgressMaster.I.ContinueGame(saveDataSlot);
									}
									this.ReturnTitleMenu();
								}
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
			DDMusicUtils.Fadeout();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.WallDrawer();
				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();
		}

		private static Ground.P_SaveDataSlot LoadGame() // ret: null == キャンセル, ret.GameStatus を使用する際は GetClone を忘れずに！
		{
			Ground.P_SaveDataSlot saveDataSlot = null;

			DDEngine.FreezeInput();

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			SimpleMenu simpleMenu = new SimpleMenu()
			{
				BorderColor = new I3Color(0, 128, 0),
				WallDrawer = () =>
				{
					DDDraw.SetBright(new I3Color(64, 64, 128));
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();
				},
			};

			string[] items = Ground.I.SaveDataSlots.Select(v => v.GameStatus == null ?
				"----" :
				"[" + v.TimeStamp + "]　" + v.Description).Concat(new string[] { "戻る" }).ToArray();

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(selectIndex, 18, 18, 32, 24, "ロード", items);

				if (selectIndex < GameConsts.SAVE_DATA_SLOT_NUM)
				{
					if (Ground.I.SaveDataSlots[selectIndex].GameStatus != null)
					{
						if (new Confirm() { BorderColor = new I3Color(50, 100, 200) }
							.Perform("スロット " + (selectIndex + 1) + " のデータをロードします。", "はい", "いいえ") == 0)
						{
							saveDataSlot = Ground.I.SaveDataSlots[selectIndex];
							break;
						}
					}
				}
				else // [戻る]
				{
					break;
				}
				//DDEngine.EachFrame(); // 不要
			}
			DDEngine.FreezeInput();

			return saveDataSlot;
		}

		#region CheatMainMenu

		private void CheatMainMenu()
		{
			for (; ; )
			{
				int selectIndex = this.SimpleMenu.Perform(0, 40, 40, 40, 24, "開発デバッグ用メニュー", new string[]
				{
					"スタート_因幡てゐ",
					"スタート_チルノ",
					"Game用テストメニュー",
					"Game用テストメニュー.2",
					"戻る",
				});

				switch (selectIndex)
				{
					case 0:
						{
							this.LeaveTitleMenu();

							using (new WorldGameMaster())
							{
								WorldGameMaster.I.World = new World("Start");
								WorldGameMaster.I.Status = new GameStatus();
								WorldGameMaster.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 1:
						{
							this.LeaveTitleMenu();

							using (new WorldGameMaster())
							{
								WorldGameMaster.I.World = new World("Start");
								WorldGameMaster.I.Status = new GameStatus()
								{
									StartChara = Player.Chara_e.CIRNO,
								};
								WorldGameMaster.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 2:
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

					case 3:
						{
							this.LeaveTitleMenu();

							using (new GameTestMenu2())
							{
								GameTestMenu2.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 4:
						goto endMenu;

					default:
						throw new DDError();
				}
				//DDEngine.EachFrame(); // 不要
			}
		endMenu:
			;
		}

		#endregion

		private void LeaveTitleMenu()
		{
			DDMusicUtils.Fadeout();
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
			DDTouch.Touch(); // 曲の再生前にタッチしておく -- .Play() で Touch した曲を解放してしまわないように
			Ground.I.Music.Title.Play();

			//DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			GC.Collect();
		}
	}
}
