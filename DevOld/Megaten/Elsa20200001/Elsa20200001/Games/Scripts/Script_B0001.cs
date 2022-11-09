using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Scripts
{
	public class Script_B0001 : Script
	{
		public override void Perform()
		{
			DDMain.KeepMainScreen();

			DDEngine.FreezeInput();

			for (; ; )
			{
				if (DDInput.A.GetInput() == 1)
					break;

				DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);

				DDPrint.SetPrint(30, 400, 30);
				DDPrint.PrintLine("B0001 -- event test");
				DDPrint.PrintLine("イベントから抜けるには A ボタンを押して下さい。");

				DDEngine.EachFrame();
			}
		}
	}
}
