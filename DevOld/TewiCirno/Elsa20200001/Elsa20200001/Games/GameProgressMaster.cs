using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Novels;

namespace Charlotte.Games
{
	public class GameProgressMaster : IDisposable
	{
		public static GameProgressMaster I;

		public GameProgressMaster()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void StartGame()
		{
			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("Start");
				Novel.I.Perform();

				if (Novel.I.RequestReturnToTitleMenu)
					return;
			}
			using (new WorldGameMaster())
			{
				WorldGameMaster.I.World = new World("Start");
				WorldGameMaster.I.Status = new GameStatus();
				WorldGameMaster.I.Perform();
			}
		}

		public void ContinueGame(Ground.P_SaveDataSlot saveDataSlot)
		{
			using (new WorldGameMaster())
			{
				WorldGameMaster.I.World = new World(saveDataSlot.MapName);
				WorldGameMaster.I.Status = saveDataSlot.GameStatus.GetClone();
				WorldGameMaster.I.Status.StartPointDirection = 101; // スタート地点を「ロード地点」にする。
				WorldGameMaster.I.Perform();
			}
		}
	}
}
