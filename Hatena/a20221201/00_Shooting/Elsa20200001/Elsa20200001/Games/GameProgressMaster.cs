using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games;
using Charlotte.Games.Scripts.Tests;
using Charlotte.Games.Scripts;
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
			this.GameMain(() => new Script_Testステージ0001());
		}

		public void ContinueGame(Func<Script> getScript)
		{
			this.GameMain(getScript);
		}

		private void GameMain(Func<Script> getScript)
		{
			Game.EndReason_e endReason;

			do
			{
				using (new Game())
				{
					Game.I.Script = getScript();
					Game.I.Perform();
					endReason = Game.I.EndReason;
				}
			}
			while (endReason == Game.EndReason_e.RestartGame);

			if (endReason == Game.EndReason_e.AllStageCleared)
			{
				using (new Novel())
				{
					Novel.I.Status.Scenario = new Scenario("Ending");
					Novel.I.Perform();

					if (Novel.I.RequestReturnToTitleMenu)
						return;
				}
			}
		}
	}
}
