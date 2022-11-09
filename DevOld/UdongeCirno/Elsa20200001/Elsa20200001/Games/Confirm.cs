using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;
using Charlotte.GameTools;

namespace Charlotte.Games
{
	public class Confirm
	{
		public I3Color BorderColor = new I3Color(100, 0, 200);
		public I4Rect BackBoardRect = new I4Rect(0, DDConsts.Screen_H / 3, DDConsts.Screen_W, DDConsts.Screen_H / 3);
		public int Text_L = 100;
		public int Text_T = DDConsts.Screen_H / 3 + 100;

		// <---- prm

		public int Perform(string prompt, params string[] options)
		{
			DDMain.KeepMainScreen();

			SimpleMenu simpleMenu = new SimpleMenu()
			{
				BorderColor = this.BorderColor,
				WallDrawer = () =>
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);

					DDDraw.SetAlpha(0.9);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, this.BackBoardRect.ToD4Rect());
					DDDraw.Reset();
				},
				X = this.Text_L,
				Y = this.Text_T,
			};

			return simpleMenu.Perform(0, prompt, options);
		}
	}
}
