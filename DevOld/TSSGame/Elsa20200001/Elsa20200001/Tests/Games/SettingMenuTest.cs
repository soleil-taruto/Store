using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.Games;
using Charlotte.GameCommons;

namespace Charlotte.Tests.Games
{
	public class SettingMenuTest
	{
		public void Test01()
		{
			using (new SettingMenu())
			{
				SettingMenu.I.Perform(() => { });
			}
		}
	}
}
