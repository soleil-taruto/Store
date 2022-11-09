﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Tests
{
	/// <summary>
	/// Game用テストメニュー.2
	/// 仮(?)メニュー画面用のテンプレートとして残しておく
	/// </summary>
	public class GameTestMenu2 : IDisposable
	{
		public static GameTestMenu2 I;

		public GameTestMenu2()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

			Ground.I.Music.Title.Play();

			Action wallDrawer = () =>
			{
				DDDraw.DrawSimple(Ground.I.Picture.DummyScreen, 0, 0);
			};

			DDTableMenu tableMenu = new DDTableMenu(30, 30, 24, wallDrawer);

			DDEngine.FreezeInput();

			for (bool keepMenu = true; keepMenu; )
			{
				tableMenu.AddColumn(30);
				tableMenu.AddItem(true, "メニュー.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0));
				tableMenu.AddItem(false, "項目.1.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.1.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.1.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(true, "メニュー.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0));
				tableMenu.AddItem(false, "項目.2.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.2.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.2.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(true, "メニュー.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0));
				tableMenu.AddItem(false, "項目.3.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.3.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.3.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });

				tableMenu.AddColumn(230);
				tableMenu.AddItem(true, "メニュー.4", new I3Color(255, 255, 255), new I3Color(0, 0, 0));
				tableMenu.AddItem(false, "項目.4.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.4.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.4.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(true, "メニュー.5", new I3Color(255, 255, 255), new I3Color(0, 0, 0));
				tableMenu.AddItem(false, "項目.5.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.5.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.5.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });

				tableMenu.AddColumn(430);
				tableMenu.AddItem(true, "メニュー.6", new I3Color(255, 255, 255), new I3Color(0, 0, 0));
				tableMenu.AddItem(false, "項目.6.1", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.6.2", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "項目.6.3", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => { });
				tableMenu.AddItem(false, "サブメニューを開く", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () =>
				{
					using (new GameTestSubMenu2())
					{
						GameTestSubMenu2.I.WallDrawer = wallDrawer;
						GameTestSubMenu2.I.Perform();
					}
				});
				tableMenu.AddItem(false, "戻る", new I3Color(255, 255, 255), new I3Color(0, 0, 0), () => keepMenu = false);

				tableMenu.Perform();

				//DDEngine.EachFrame(); // 不要
			}
			DDMusicUtils.Fadeout();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				wallDrawer();
				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();
		}
	}
}
