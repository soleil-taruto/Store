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

		private SimpleMenu SimpleMenu;

		public void Perform()
		{
			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			DDEngine.FreezeInput();

			Ground.I.Music.Title.Play();

			string[] items = new string[]
			{
				"ゲームスタート",
				"コンテニュー",
				"おまけ",
				"設定",
				"終了",
			};

			int selectIndex = 0;
			double stress = 0.0;
			double bureStress = 0.0;
			bool deepMenuEntered = false;

			this.SimpleMenu = new SimpleMenu()
			{
				BorderColor = new I3Color(64, 0, 0),
				WallDrawer = () =>
				{
					DDPicture picture = Ground.I.Picture.Title;

					DDDraw.DrawRect(
						picture,
						DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
						);

					DDDraw.SetMosaic();
					DDDraw.DrawBegin(
						Ground.I.Picture.PlayerStands[DDEngine.ProcFrame / 120 % 2][DDEngine.ProcFrame / 30 % 2],
						610,
						392
						);
					DDDraw.DrawZoom_X(-1.0);
					DDDraw.DrawZoom(14.0);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					if (deepMenuEntered)
					{
						DDCurtain.DrawCurtain(-0.4);
					}
					else
					{
						DDDraw.SetAlpha(0.4 * stress);
						DDDraw.SetBright(0, 0, 0);
						DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, 295 * stress, DDConsts.Screen_H);
						DDDraw.Reset();

						picture = Ground.I.Picture.TitleString;

						double titleStringX = 270;
						double titleStringY = 60;

						titleStringX += 20.0 * DDUtils.Random.GetReal2() * bureStress;
						titleStringY += 20.0 * DDUtils.Random.GetReal2() * bureStress;

						DDDraw.SetBright(1.0, 0.7, 0.3);
						DDDraw.DrawCenter(picture,
							titleStringX + 10 - 40.0 * (1.0 - stress),
							titleStringY + 10
							);
						DDDraw.Reset();
						DDDraw.DrawCenter(picture,
							titleStringX - 20.0 * (1.0 - stress),
							titleStringY
							);

						if (DDUtils.Random.GetReal1() < 0.001)
							bureStress = 1.0;

						DDUtils.Approach(ref stress, 1.0, 0.92);
						DDUtils.Approach(ref bureStress, 0.0, 0.9);
					}
				},
			};

			for (; ; )
			{
				selectIndex = this.SimpleMenu.Perform(selectIndex, 40, 280, 40, 24, "", items);

				bool cheatFlag;

				{
					int bk_freezeInputFrame = DDEngine.FreezeInputFrame;
					DDEngine.FreezeInputFrame = 0;
					cheatFlag = 1 <= DDInput.DIR_6.GetInput();
					DDEngine.FreezeInputFrame = bk_freezeInputFrame;
				}

				switch (selectIndex)
				{
					case 0:
						if (DDConfig.LOG_ENABLED && cheatFlag)
						{
							deepMenuEntered = true;
							this.CheatMainMenu();
							deepMenuEntered = false;
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
						using (new OmakeMenu())
						{
							deepMenuEntered = true;
							OmakeMenu.I.Perform();
							deepMenuEntered = false;
						}
						break;

					case 3:
						using (new SettingMenu())
						{
							SettingMenu.I.SimpleMenu = this.SimpleMenu;

							deepMenuEntered = true;
							SettingMenu.I.Perform();
							deepMenuEntered = false;
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
				BorderColor = new I3Color(0, 0, 64),
				WallDrawer = () =>
				{
					DDPicture picture = Ground.I.Picture.LoadBack;

					DDDraw.DrawRect(
						picture,
						DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
						);

					DDCurtain.DrawCurtain(-0.4);
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
					"スタート",
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

							using (new GameTestMenu())
							{
								//GameTestMenu.I.SimpleMenu = this.SimpleMenu; // 不用
								GameTestMenu.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 2:
						{
							this.LeaveTitleMenu();

							using (new GameTestMenu2())
							{
								GameTestMenu2.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 3:
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
