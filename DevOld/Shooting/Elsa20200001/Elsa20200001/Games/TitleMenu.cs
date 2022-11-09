using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;
using Charlotte.Games.Scripts;
using Charlotte.Games.Scripts.Tests;
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

		#region 背景

		private DDTask 背景 = new 背景Task();

		private class 背景Task : DDTask
		{
			private DDTask SnowPanel_01 = new SnowPanelTask() { RMin = 1.0, RMax = 2.0 };
			private DDTask SnowPanel_02 = new SnowPanelTask() { RMin = 2.0, RMax = 3.0 };
			private DDTask SnowPanel_03 = new SnowPanelTask() { RMin = 3.0, RMax = 4.0 };
			private DDTask SnowPanel_04 = new SnowPanelTask() { RMin = 4.0, RMax = 5.0 };

			private DDTask Panel_01 = new PanelTask_01();
			private DDTask Panel_02 = new PanelTask_02();
			private DDTask Panel_03 = new PanelTask_03();

			public override IEnumerable<bool> E_Task()
			{
				for (; ; )
				{
					DDCurtain.DrawCurtain();

					this.SnowPanel_01.Execute();
					this.Panel_01.Execute();
					this.SnowPanel_02.Execute();
					this.Panel_02.Execute();
					this.SnowPanel_03.Execute();
					this.Panel_03.Execute();
					this.SnowPanel_04.Execute();

					yield return true;
				}
			}

			private class SnowPanelTask : DDTask
			{
				public double RMin;
				public double RMax;

				// <---- prm

				private DDTaskList Snows = new DDTaskList();
				private double PutRate = 0.0;

				public override IEnumerable<bool> E_Task()
				{
					for (; ; )
					{
						DDUtils.Approach(ref this.PutRate, 0.6, 0.999);

						if (DDUtils.Random.GetReal1() < this.PutRate)
						{
							double r = DDUtils.AToBRate(this.RMin, this.RMax, DDUtils.Random.GetReal1());

							this.Snows.Add(new SnowInfo()
							{
								X = DDUtils.Random.GetReal1() * DDConsts.Screen_W,
								Y = -10.0,
								R = r,
								XAdd = DDUtils.Random.GetReal1() * -3.0,
								YAdd = r * 1.5,
							}
							.Task
							);
						}

						this.Snows.ExecuteAllTask();

						yield return true;
					}
				}

				private class SnowInfo : DDTask
				{
					public double X;
					public double Y;
					public double R;
					public double XAdd;
					public double YAdd;

					// <---- prm

					public override IEnumerable<bool> E_Task()
					{
						for (; ; )
						{
							this.X += this.XAdd;
							this.Y += this.YAdd;

							if (this.X < 0.0)
								this.X += DDConsts.Screen_W;

							DDDraw.SetAlpha(0.2);
							DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X, this.Y);
							DDDraw.DrawZoom(this.R / (Ground.I.Picture.WhiteCircle.Get_W() / 2.0));
							DDDraw.DrawEnd();
							DDDraw.Reset();

							yield return this.Y < DDConsts.Screen_H + 10.0;
						}
					}
				}
			}

			private class PanelTask_01 : DDTask
			{
				private DDPicture PIC = Ground.I.Picture.Wall0001;
				private const int SPEED = 3;
				private const double TARGET_PUT_RATE = 0.666;
				private const double PR_APPROACHING_RATE = 0.91;

				private DDTaskList Tiles = new DDTaskList();
				private double PutRate = 0.0;

				public override IEnumerable<bool> E_Task()
				{
					for (int slide = 0; ; slide += SPEED)
					{
						if (PIC.Get_W() < slide)
						{
							slide -= PIC.Get_W();
							DDUtils.Approach(ref this.PutRate, TARGET_PUT_RATE, PR_APPROACHING_RATE);

							for (int y = 0; y < DDConsts.Screen_H; y += PIC.Get_H())
								if (DDUtils.Random.GetReal1() < this.PutRate)
									this.Tiles.Add(new TileTask() { PIC = PIC, X = DDConsts.Screen_W - slide, Y = y }.Task);
						}
						this.Tiles.ExecuteAllTask();
						yield return true;
					}
				}

				private class TileTask : DDTask
				{
					public DDPicture PIC;
					public int X;
					public int Y;

					// <---- prm

					public override IEnumerable<bool> E_Task()
					{
						while (-PIC.Get_W() < this.X)
						{
							DDDraw.DrawSimple(PIC, this.X, this.Y);
							this.X -= SPEED;
							yield return true;
						}
					}
				}
			}

			private class PanelTask_02 : DDTask
			{
				private DDPicture PIC = Ground.I.Picture.Wall0002;
				private const int SPEED = 5;
				private const double TARGET_PUT_RATE = 0.666;
				private const double PR_APPROACHING_RATE = 0.99;

				private DDTaskList Tiles = new DDTaskList();
				private double PutRate = 0.0;

				public override IEnumerable<bool> E_Task()
				{
					for (int slide = 0; ; slide += SPEED)
					{
						if (PIC.Get_W() < slide)
						{
							slide -= PIC.Get_W();
							DDUtils.Approach(ref this.PutRate, TARGET_PUT_RATE, PR_APPROACHING_RATE);

							for (int y = 0; y < DDConsts.Screen_H; y += PIC.Get_H())
								if (DDUtils.Random.GetReal1() < this.PutRate)
									this.Tiles.Add(new TileTask() { PIC = PIC, X = DDConsts.Screen_W - slide, Y = y }.Task);
						}
						this.Tiles.ExecuteAllTask();
						yield return true;
					}
				}

				private class TileTask : DDTask
				{
					public DDPicture PIC;
					public int X;
					public int Y;

					// <---- prm

					public override IEnumerable<bool> E_Task()
					{
						while (-PIC.Get_W() < this.X)
						{
							DDDraw.DrawSimple(PIC, this.X, this.Y);
							this.X -= SPEED;
							yield return true;
						}
					}
				}
			}

			private class PanelTask_03 : DDTask
			{
				private DDPicture PIC = Ground.I.Picture.Wall0003;
				private const int SPEED = 7;
				private const double TARGET_PUT_RATE = 0.333;
				private const double PR_APPROACHING_RATE = 0.99;

				private DDTaskList Tiles = new DDTaskList();
				private double PutRate = 0.0;

				public override IEnumerable<bool> E_Task()
				{
					for (int slide = 0; ; slide += SPEED)
					{
						if (PIC.Get_W() < slide)
						{
							slide -= PIC.Get_W();
							DDUtils.Approach(ref this.PutRate, TARGET_PUT_RATE, PR_APPROACHING_RATE);

							for (int y = 0; y < DDConsts.Screen_H; y += PIC.Get_H())
								if (DDUtils.Random.GetReal1() < this.PutRate)
									this.Tiles.Add(new TileTask() { PIC = PIC, X = DDConsts.Screen_W - slide, Y = y }.Task);
						}
						this.Tiles.ExecuteAllTask();
						yield return true;
					}
				}

				private class TileTask : DDTask
				{
					public DDPicture PIC;
					public int X;
					public int Y;

					// <---- prm

					public override IEnumerable<bool> E_Task()
					{
						while (-PIC.Get_W() < this.X)
						{
							DDDraw.DrawSimple(PIC, this.X, this.Y);
							this.X -= SPEED;
							yield return true;
						}
					}
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

			string[] items = new string[]
			{
				"ゲームスタート",
				"コンテニュー",
				"設定",
				"終了",
			};

			int selectIndex = 0;

			this.SimpleMenu = new SimpleMenu();

			this.SimpleMenu.BorderColor = new I3Color(0, 64, 128);
			this.SimpleMenu.WallDrawer = this.背景.Execute;

			for (; ; )
			{
				selectIndex = this.SimpleMenu.Perform(selectIndex, 40, 40, 40, 24, "シューティング ゲーム(仮)", items);

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
							string[] continueMenuItems = Enumerable
								.Range(1, Ground.I.CanContinueStageNumber)
								.Select(v => "ステージ " + v)
								.Concat(new string[] { "戻る" })
								.ToArray();

							selectIndex = this.SimpleMenu.Perform(0, 40, 40, 40, 24, "コンテニュー", continueMenuItems);

							if (selectIndex == continueMenuItems.Length - 1) // ? 戻る
								break;

							Func<Script> getScript;

							switch (selectIndex)
							{
								case 0: getScript = () => new Script_Testステージ0001(); break;
								case 1: getScript = () => new Script_Testステージ0002(); break;
								case 2: getScript = () => new Script_Testステージ0003(); break;

								default:
									throw null; // never
							}
							this.LeaveTitleMenu();

							using (new GameProgressMaster())
							{
								GameProgressMaster.I.ContinueGame(getScript);
							}
							this.ReturnTitleMenu();
						}
						break;

					case 2:
						using (new SettingMenu()
						{
							SimpleMenu = this.SimpleMenu,
						})
						{
							SettingMenu.I.Perform();
						}
						break;

					case 3:
						goto endMenu;

					default:
						throw new DDError();
				}
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

		#region CheatMainMenu

		private void CheatMainMenu()
		{
			for (; ; )
			{
				int selectIndex = this.SimpleMenu.Perform(0, 40, 40, 40, 24, "開発デバッグ用メニュー", new string[]
				{
					"スタート_Script_Testステージ0001",
					"ノベルパート_テスト0001",
					"ノベルパート_Start",
					"ノベルパート_Ending",
					"戻る",
				});

				switch (selectIndex)
				{
					case 0:
						{
							this.LeaveTitleMenu();

							using (new Game())
							{
								Game.I.Script = new Script_Testステージ0001();
								Game.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 1:
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

					case 2:
						{
							this.LeaveTitleMenu();

							using (new Novel())
							{
								Novel.I.Status.Scenario = new Scenario("Start");
								Novel.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 3:
						{
							this.LeaveTitleMenu();

							using (new Novel())
							{
								Novel.I.Status.Scenario = new Scenario("Ending");
								Novel.I.Perform();
							}
							this.ReturnTitleMenu();
						}
						break;

					case 4:
						goto endMenu;

					default:
						throw new DDError();
				}
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
			DDTouch.Touch(); // 曲再生の前に -- .Play() で Touch した曲を解放してしまわないように
			Ground.I.Music.Title.Play();

			//DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			GC.Collect();
		}
	}
}
