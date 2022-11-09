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
				Game.I.World = new World("Tests/t0001");
				Game.I.Status = new GameStatus();
				Game.I.Perform();
			}
		}

		public void Test03()
		{
			string startMapName;

			// ---- choose one ----

			//startMapName = "Tests/t0001";
			//startMapName = "Tests/t0002";
			//startMapName = "Tests/t0003";
			//startMapName = "Tests/t0004";
			startMapName = "Start";

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
