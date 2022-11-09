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
				Game.I.World = new World("Stage1/t1001");
				Game.I.Status = new GameStatus();
				Game.I.Perform();
			}
		}

		public void Test03()
		{
			string startMapName;

			// ---- choose one ----

			startMapName = "Stage1/t1001";
			//startMapName = "Stage1/t1002";
			//startMapName = "Stage1/t1003";
			//startMapName = "Stage1/t1004";

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
