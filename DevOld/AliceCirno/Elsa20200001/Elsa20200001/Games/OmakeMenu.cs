using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
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

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"何もないよ",
					"戻る",
				};

				selectIndex = this.SimpleMenu.Perform(selectIndex, 40, 40, 40, 18, "おまけ　（今のところ何もありません）", items);

				this.SetDeepConfigEntered(true);

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
					case 9:
						SetNaiyoFukidashi(selectIndex);
						break;

					case 10:
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
				DDPrint.SetPrint(190, 82 + iy * 40, 40, 16);
				DDPrint.PrintLine(line);
				DDPrint.Reset();
			});

			this.SNF_FreezeEndFrame = DDEngine.ProcFrame + 40;
		}
	}
}
