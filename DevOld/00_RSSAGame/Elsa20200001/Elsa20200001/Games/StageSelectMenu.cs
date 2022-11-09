using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games
{
	public class StageSelectMenu : IDisposable
	{
		public GameStatus GameStatus;

		// <---- prm

		public int SelectedStageNo = 1; // 1～9 == 選択したステージ

		// <---- ret

		public static StageSelectMenu I;

		public StageSelectMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			DDSimpleMenu simpleMenu = new DDSimpleMenu()
			{
				BorderColor = new I3Color(0, 64, 128),
				WallDrawer = () =>
				{
					DDDraw.SetBright(0.2, 0.4, 0.6);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();

					DDDraw.SetAlpha(0.5);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 4, DDConsts.Screen_W, DDConsts.Screen_H / 2);
					DDDraw.Reset();
				},
			};

			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"Stage-1",
					"Stage-2",
					"Stage-3",
					"Stage-4",
					"Stage-5",
					"Stage-6",
					"Stage-7",
					"Stage-8",
					"Stage-9",
				};

				selectIndex = simpleMenu.Perform(40, 40, 40, 24, "StageSelectMenu", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
						this.SelectedStageNo = selectIndex + 1;
						goto endMenu;

					default:
						throw new DDError();
				}
				//yield return true; // 不要
			}
		endMenu:
			DDMusicUtils.Fadeout();
			DDCurtain.SetCurtain(30, -1.0);
			DDEngine.FreezeInput();
		}
	}
}
