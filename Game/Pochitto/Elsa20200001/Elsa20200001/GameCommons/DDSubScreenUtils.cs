using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDSubScreenUtils
	{
		public static List<DDSubScreen> SubScreens = new List<DDSubScreen>();

		public static void Add(DDSubScreen subScreen)
		{
			SubScreens.Add(subScreen);
		}

		public static void Remove(DDSubScreen subScreen)
		{
			if (DDUtils.FastDesertElement(SubScreens, i => i == subScreen) == null) // ? Already removed
				throw new DDError();
		}

		public static void UnloadAll()
		{
#if true
			UnloadAll(subScreen => true);
#else // old same
			foreach (DDSubScreen subScreen in SubScreens)
				subScreen.Unload();
#endif
		}

		public static void UnloadAll(Predicate<DDSubScreen> match)
		{
			foreach (DDSubScreen subScreen in SubScreens)
				if (match(subScreen))
					subScreen.Unload();
		}

		//public static int CurrDrawScreenHandle = DX.DX_SCREEN_BACK; // 廃止

		public static void ChangeDrawScreen(int handle)
		{
			if (DX.SetDrawScreen(handle) != 0) // ? 失敗
				throw new DDError();

			//CurrDrawScreenHandle = handle; // 廃止
		}

		public static DDSubScreen CurrDrawScreen; // DDMain.GameStart() で MainScreen に設定される。

		public static void ChangeDrawScreen(DDSubScreen subScreen)
		{
			ChangeDrawScreen(subScreen.GetHandle());
			CurrDrawScreen = subScreen;
		}

		public static void RestoreDrawScreen()
		{
			ChangeDrawScreen(DDGround.MainScreen);
		}

		public static void DrawDummyScreenAll()
		{
			DDPicture picture = Ground.I.Picture.DummyScreen;

			foreach (DDSubScreen subScreen in SubScreens)
			{
				if (subScreen.WasLoaded)
				{
					using (subScreen.Section())
					{
						DDDraw.DrawRect(
							picture,
							DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(new D2Point(0, 0), subScreen.GetSize().ToD2Size()))
							);
					}
				}
			}
		}
	}
}
