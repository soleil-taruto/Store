using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class SaveOrLoadMenuTest
	{
		public void Test01()
		{
			using (new SaveOrLoadMenu())
			{
				SaveOrLoadMenu.I.Save(() => { });
			}
		}
	}
}
