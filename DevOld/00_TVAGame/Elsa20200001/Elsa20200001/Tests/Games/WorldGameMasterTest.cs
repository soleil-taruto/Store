using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class WorldGameMasterTest
	{
		public void Test01()
		{
			using (new WorldGameMaster())
			{
				WorldGameMaster.I.Perform();
			}
		}

		public void Test02()
		{
			using (new WorldGameMaster())
			{
				WorldGameMaster.I.World = new World("Tests/t0001");
				WorldGameMaster.I.Status = new GameStatus();
				WorldGameMaster.I.Perform();
			}
		}

		public void Test03()
		{
			string startMapName;

			// ---- choose one ----

			startMapName = "Tests/t0001"; // 旧スタート地点
			//startMapName = "Tests/t0002";
			//startMapName = "Tests/t0003";
			//startMapName = "Tests/t0004";
			//startMapName = "Start"; // スタート地点

			// ----

			using (new WorldGameMaster())
			{
				WorldGameMaster.I.World = new World(startMapName);
				WorldGameMaster.I.Status = new GameStatus();
				WorldGameMaster.I.Perform();
			}
		}
	}
}
