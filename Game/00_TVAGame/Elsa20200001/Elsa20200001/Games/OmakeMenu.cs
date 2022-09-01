using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.GameTools;
using Charlotte.Novels;

namespace Charlotte.Games
{
	public class OmakeMenu : IDisposable
	{
		public SimpleMenu SimpleMenu;
		public Action<bool> SetDeepConfigEntered = flag => { };

		// <---- prm

		public static OmakeMenu I;

		public OmakeMenu()
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

			DDSE[] seSamples = Ground.I.SE.テスト用s;

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

				selectIndex = this.SimpleMenu.Perform(selectIndex, 40, 40, 40, 18, "おまけ", items);

				this.SetDeepConfigEntered(true);

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
				this.SetDeepConfigEntered(false);

				//DDEngine.EachFrame(); // 不要
			}
		endMenu:
			this.SetDeepConfigEntered(false);
			DDEngine.FreezeInput();
		}
	}
}
