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
			public bool DeepConfigEntered = false;

			public override IEnumerable<bool> E_Task()
			{
				DDTaskList el = new DDTaskList();
				double dx = 0.0;
				double dy = 0.0;
				double ldx;
				double ldy;
				double shadow_a = 0.0;
				double shadow_x = 0.0;
				double title_a = 0.0;
				double shadow_targ_a;
				double shadow_targ_x;
				double title_targ_a;

				for (int frame = 0; ; frame++)
				{
					ldx = dx;
					ldy = dy;
					dx = Math.Cos(frame / 199.0) * 40.0;
					dy = Math.Cos(frame / 211.0) * 30.0;
					double dxa = dx - ldx;
					double dya = dy - ldy;

					DDDraw.DrawBegin(Ground.I.Picture.TitleWall, DDConsts.Screen_W / 2 + dx, DDConsts.Screen_H / 2 + dy);
					DDDraw.DrawZoom(1.3);
					DDDraw.DrawEnd();

					if (1 <= frame && DDUtils.Random.GetReal1() < 0.03 + Math.Sin(frame / 307.0) * 0.02)
					{
						el.Add(SCommon.Supplier(this.Effect_0001(dx, dy, dxa, dya)));
					}
					el.ExecuteAllTask_Reverse();

					double titleX = 720.0 + dx * 0.4;
					double titleY = 270.0 + dy * 0.4;

					double tba = 0.5 + Math.Sin(frame / 103.0) * 0.185 + Math.Sin(frame / 3.0) * 0.015 * Math.Sin(frame / 107.0);
					double tfa = 0.3;

					tba *= title_a;
					tfa *= title_a;

					{
						const int FRAME_MAX = 300;

						if (frame < FRAME_MAX)
						{
							DDDraw.SetBlendAdd(frame * tba / FRAME_MAX);
							DDDraw.DrawCenter(Ground.I.Picture.Title, titleX, titleY);
							DDDraw.Reset();
						}
						else
						{
							DDDraw.SetBlendAdd(tba);
							DDDraw.DrawCenter(Ground.I.Picture.Title, titleX, titleY);
							DDDraw.Reset();
						}
					}

					{
						const int FRAME_MAX = 300;

						if (frame < FRAME_MAX)
						{
							DDDraw.SetBlendAdd(frame * tfa / FRAME_MAX);
							DDDraw.DrawCenter(Ground.I.Picture.Title, titleX, titleY);
							DDDraw.Reset();
						}
						else
						{
							DDDraw.SetBlendAdd(tfa);
							DDDraw.DrawCenter(Ground.I.Picture.Title, titleX, titleY);
							DDDraw.Reset();
						}
					}

					if (this.DeepConfigEntered)
					{
						shadow_targ_a = 0.3;
						shadow_targ_x = DDConsts.Screen_W;
						title_targ_a = 0.0;
					}
					else if (this.TopMenuLeaved)
					{
						shadow_targ_a = 0.3;
						shadow_targ_x = 480.0;
						title_targ_a = 1.0;
					}
					else
					{
						shadow_targ_a = 0.0;
						shadow_targ_x = 30.0;
						title_targ_a = 1.0;
					}
					DDUtils.Approach(ref shadow_a, shadow_targ_a, 0.8);
					DDUtils.Approach(ref shadow_x, shadow_targ_x, 0.8);
					DDUtils.Approach(ref title_a, title_targ_a, 0.97);

					DDDraw.SetAlpha(shadow_a);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, shadow_x, DDConsts.Screen_H);
					DDDraw.Reset();

					yield return true;
				}
			}

			private IEnumerable<bool> Effect_0001(double dx, double dy, double dxa, double dya)
			{
				double x = DDConsts.Screen_W / 2;
				double y = DDConsts.Screen_H / 2;
				double a = 1.0;
				double z = 1.3;
				double r = 0.0;
				double xa = DDUtils.Random.GetReal1() * 0.01;
				double ya = DDUtils.Random.GetReal1() * 0.01;
				double aa = -0.007 - DDUtils.Random.GetReal1() * 0.003;
				double za = DDUtils.Random.GetReal1() * 0.00006 - 0.00002;
				double ra = DDUtils.Random.GetReal1() < 0.3 ? DDUtils.Random.GetReal2() * 0.0015 : 0.0;

				while (0.003 < a)
				{
					DDDraw.SetAlpha(a);
					DDDraw.DrawBegin(Ground.I.Picture.TitleWall, x, y);
					DDDraw.DrawZoom(z);
					DDDraw.DrawSlide(dx, dy);
					DDDraw.DrawRotate(r);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					x += xa + dxa;
					y += ya + dya;
					a += aa;
					z += za;
					r += ra;

					dxa *= 0.99;
					dya *= 0.99;

					yield return true;
				}
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

				const double ITEM_UNSEL_X = 120.0;
				const double ITEM_UNSEL_A = 0.5;
				const double ITEM_SEL_X = 140.0;
				const double ITEM_SEL_A = 1.0;
				const double ITEM_Y = 170.0;
				const double ITEM_Y_STEP = 50.0;

				double x = ITEM_SEL_X;
				double y = ITEM_Y + selfIndex * ITEM_Y_STEP;
				double a = ITEM_UNSEL_A;
				double realX = -100.0 - selfIndex * 200.0;
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
				BorderColor = new I3Color(0, 96, 0),
				WallDrawer = this.DrawWall.Execute,
			};

			this.TopMenu.SelectIndex = 0;

			for (; ; )
			{
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
							if (DDConfig.LOG_ENABLED && 1 <= DDInput.DIR_6.GetInput())
							{
								this.CheatMainMenu();
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
								this.DrawWall.DeepConfigEntered = true;
								Ground.P_SaveDataSlot saveDataSlot = LoadGame();
								this.DrawWall.DeepConfigEntered = false;

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
									OmakeMenu.I.SetDeepConfigEntered = flag => this.DrawWall.DeepConfigEntered = flag;
									OmakeMenu.I.Perform();
								}
								this.DrawWall.TopMenuLeaved = false;
							}
							break;

						case 3:
							{
								this.DrawWall.TopMenuLeaved = true;

								using (new SettingMenu())
								{
									SettingMenu.I.SimpleMenu = this.SimpleMenu;
									SettingMenu.I.SetDeepConfigEntered = flag => this.DrawWall.DeepConfigEntered = flag;
									SettingMenu.I.Perform();
								}
								this.DrawWall.TopMenuLeaved = false;
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

		private Ground.P_SaveDataSlot LoadGame() // ret: null == キャンセル, ret.GameStatus を使用する際は GetClone を忘れずに！
		{
			Ground.P_SaveDataSlot saveDataSlot = null;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			string[] items = Ground.I.SaveDataSlots.Select(v => v.GameStatus == null ?
				"----" :
				"[" + v.TimeStamp + "]　" + v.Description).Concat(new string[] { "戻る" }).ToArray();

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = this.SimpleMenu.Perform(selectIndex, 18, 18, 32, 24, "ロード", items);

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
