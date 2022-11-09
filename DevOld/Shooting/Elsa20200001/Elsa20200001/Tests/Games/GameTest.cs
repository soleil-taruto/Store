using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Games.Scripts;
using Charlotte.Games.Scripts.Tests;

namespace Charlotte.Tests.Games
{
	public class GameTest
	{
		public void Test01()
		{
			using (new Game())
			{
				Game.I.Perform();
			}
		}

		public void Test02()
		{
			using (new Game())
			{
				Game.I.Script = new Script_Test壁紙0001();
				Game.I.Perform();
			}
		}

		public void Test03()
		{
			Script script;

			// ---- choose one ----

			//script = new Script_Test壁紙0001();
			//script = new Script_Test0001();
			//script = new Script_Test0002();
			//script = new Script_Test0003();
			//script = new Script_Test0004();
			//script = new Script_Testボス0001テスト();
			//script = new Script_Testステージ0001();
			//script = new Script_Testステージ0002();
			script = new Script_Testステージ0003();

			// ----

			using (new Game())
			{
				Game.I.Script = script;
				Game.I.Perform();
			}
		}
	}
}
