using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Tests
{
	/// <summary>
	/// Game用テスト(サブ)メニュー
	/// 仮メニュー画面用のテンプレートとして残しておく
	/// </summary>
	public class GameTestSubMenu : IDisposable
	{
		public DDSimpleMenu SimpleMenu;

		// <---- prm

		public static GameTestSubMenu I;

		public GameTestSubMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"ダミー0001",
					"ダミー0002",
					"ダミー0003",
					"戻る",
				};

				selectIndex = this.SimpleMenu.Perform(40, 40, 40, 24, "Game用テスト(サブ)メニュー", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
						// none
						break;

					case 1:
						// none
						break;

					case 2:
						// none
						break;

					case 3:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			DDEngine.FreezeInput();
		}
	}
}
