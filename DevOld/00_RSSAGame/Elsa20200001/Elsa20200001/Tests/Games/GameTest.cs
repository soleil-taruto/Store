using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

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
				Game.I.World = new World("Tests/t1001");
				Game.I.Status = new GameStatus();
				Game.I.Perform();
			}
		}

		public void Test03()
		{
			string startMapName;

			// ---- choose one ----

			startMapName = "Tests/t1001";
			//startMapName = "Tests/t1002";
			//startMapName = "Tests/t1003";
			//startMapName = "Tests/t1004";

			// ----

			using (new Game())
			{
				Game.I.World = new World(startMapName);
				Game.I.Status = new GameStatus();
				Game.I.Perform();
			}
		}
	}
}
