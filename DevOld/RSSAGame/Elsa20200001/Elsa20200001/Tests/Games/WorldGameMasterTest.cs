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
				WorldGameMaster.I.World = new World("Stage1/t1001");
				WorldGameMaster.I.Status = new GameStatus();
				WorldGameMaster.I.Perform();
			}
		}

		public void Test03()
		{
			string startMapName;

			// ---- choose one ----

			//startMapName = "Stage1/t1001";
			//startMapName = "Stage1/t1002";
			//startMapName = "Stage1/t1003";
			//startMapName = "Stage1/t1004";

			startMapName = "Stage2/Start";
			//startMapName = "Stage2/Room_02";
			//startMapName = "Stage2/Room_03";
			//startMapName = "Stage2/Room_04";

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
