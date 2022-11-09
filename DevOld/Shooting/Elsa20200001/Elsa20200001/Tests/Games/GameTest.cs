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
				Game.I.Script = new Script_B壁紙テスト0001();
				Game.I.Perform();
			}
		}

		public void Test03()
		{
			Script script;

			// ---- choose one ----

			//script = new Script_壁紙テスト0001();
			//script = new Script_テスト0001();
			//script = new Script_テスト0002();
			//script = new Script_テスト0003();
			//script = new Script_テスト0004();
			//script = new Script_Bボス0001テスト();
			//script = new Script_ステージ0001();
			//script = new Script_ステージ0002();
			script = new Script_Bステージ0003();

			// ----

			using (new Game())
			{
				Game.I.Script = script;
				Game.I.Perform();
			}
		}
	}
}
