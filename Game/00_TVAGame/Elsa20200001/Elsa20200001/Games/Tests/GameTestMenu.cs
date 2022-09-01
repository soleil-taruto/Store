using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;

namespace Charlotte.Games.Tests
{
	/// <summary>
	/// Game用テストメニュー
	/// 仮メニュー画面用のテンプレートとして残しておく
	/// </summary>
	public class GameTestMenu : IDisposable
	{
		public static GameTestMenu I;

		public GameTestMenu()
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

			Ground.I.Music.Title.Play();

			this.SimpleMenu = new SimpleMenu()
			{
				BorderColor = new I3Color(32, 64, 96),
				WallDrawer = () => DDDraw.DrawSimple(Ground.I.Picture.DummyScreen, 0, 0),
			};

			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"サブメニューを開く",
					"ダミー0001",
					"ダミー0002",
					"ダミー0003",
					"戻る",
				};

				selectIndex = this.SimpleMenu.Perform(selectIndex, 40, 40, 40, 24, "Game用テストメニュー", items);

				switch (selectIndex)
				{
					case 0:
						using (new GameTestSubMenu())
						{
							GameTestSubMenu.I.SimpleMenu = this.SimpleMenu;
							GameTestSubMenu.I.Perform();
						}
						break;

					case 1:
						// none
						break;

					case 2:
						// none
						break;

					case 3:
						// none
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
	}
}
