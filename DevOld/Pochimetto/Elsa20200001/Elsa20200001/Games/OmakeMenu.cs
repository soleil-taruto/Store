using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;

namespace Charlotte.Games
{
	public class OmakeMenu : IDisposable
	{
		public static OmakeMenu I;

		public OmakeMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		private SimpleMenu SimpleMenu = new SimpleMenu()
		{
			BorderColor = new I3Color(64, 0, 0),
			WallDrawer = () =>
			{
				DDPicture picture = Ground.I.Picture.Title;

				DDDraw.DrawRect(
					picture,
					DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
					);

				DDCurtain.DrawCurtain(-0.4);

				DDPrint.SetPrint(140, 410, 40, 40);
				DDPrint.SetBorder(new I3Color(0, 60, 64));
				DDPrint.PrintLine("今のところ何もありません！");
				DDPrint.Reset();
			},
		};

		public void Perform()
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"戻る",
				};

				selectIndex = this.SimpleMenu.Perform(selectIndex, 40, 140, 40, 24, "おまけ", items);

				switch (selectIndex)
				{
					case 0:
						SetNaiyoFukidashi(0);
						break;

					case 1:
						SetNaiyoFukidashi(1);
						break;

					case 2:
						SetNaiyoFukidashi(2);
						break;

					case 3:
						goto endMenu;

					default:
						throw new DDError();
				}
				//DDEngine.EachFrame(); // 不要
			}
		endMenu:
			DDEngine.FreezeInput();
		}

		private int SNF_FreezeEndFrame = -1;

		private void SetNaiyoFukidashi(int iy)
		{
			if (DDEngine.ProcFrame < this.SNF_FreezeEndFrame)
				return;

			string line = "< " + SCommon.CRandom.ChooseOne(new string[]
			{
				"ナイノヨ！",
				"ナイッテバヨ！",
				"ダカラ、ナイッテバヨ！",
				"ネーヨ！",
				"ネーッテバヨ！",
				"ダカラ、ネーヨ！",
			});

			DDGround.EL.Keep(30, () =>
			{
				DDPrint.SetPrint(240, 185 + iy * 40, 40, 16);
				DDPrint.PrintLine(line);
				DDPrint.Reset();
			});

			this.SNF_FreezeEndFrame = DDEngine.ProcFrame + 40;
		}
	}
}
