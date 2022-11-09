using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Novels;

namespace Charlotte.GameProgressMasters
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

		public void Perform()
		{
			using (new Novel())
			{
				Novel.I.Status.Scenario = new Scenario("Start");
				Novel.I.Perform();
			}
			this.StageSelectLoop(new GameStatus());
		}

		public void Perform_コンテニュー()
		{
			GameStatus gameStatus;

			using (new PasswordInput())
			{
				PasswordInput.I.Perform();

				if (PasswordInput.I.LoadedGameStatus == null) // ? パスワード入力中止
					return;

				gameStatus = PasswordInput.I.LoadedGameStatus;
			}
			if (gameStatus.WilyStageIndex != 0)
			{
				this.WilyStageLoop(gameStatus);
				return;
			}
			this.StageSelectLoop(gameStatus);
		}

		private string[] StageNo2StartMapName = new string[]
		{
			null,
			"Stage1/Start", // Stage1
			"Stage2/Start", // Stage2
			"Stage3/Start", // Stage3
			"Stage4/Start", // Stage4
			"Stage5/Start", // Stage5
			"Stage6/Start", // Stage6
			"Stage7/Start", // Stage7
			"Stage8/Start", // Stage8
			"Stage9/Start", // Stage9
		};

		private string[] StageNo2AfterNovelScenarioName = new string[]
		{
			null,
			"Tests/テスト0001", // Stage1
			"Tests/テスト0001", // Stage2
			"Tests/テスト0001", // Stage3
			"Tests/テスト0001", // Stage4
			"Tests/テスト0001", // Stage5
			"Tests/テスト0001", // Stage6
			"Tests/テスト0001", // Stage7
			"Tests/テスト0001", // Stage8
			"Tests/テスト0001", // Stage9
		};

		private void StageSelectLoop(GameStatus gameStatus)
		{
			for (; ; )
			{
				int stageNo;

				using (new StageSelectMenu())
				{
					StageSelectMenu.I.GameStatus = gameStatus;
					StageSelectMenu.I.Perform();

					stageNo = StageSelectMenu.I.SelectedStageNo;
				}
				string startMapName = this.StageNo2StartMapName[stageNo];

				if (startMapName == null)
					throw null; // never

				Game.EndReason_e endReason;

				using (new WorldGameMaster())
				{
					WorldGameMaster.I.World = new World(startMapName);
					WorldGameMaster.I.Status = gameStatus;
					WorldGameMaster.I.Perform();

					endReason = WorldGameMaster.I.EndReason;

					// ステージクリアによるインベントリの変更(武器取得)は Game.I.Perform() 内で行うこと。
				}
				if (endReason == Game.EndReason_e.ReturnToTitleMenu)
					break;

				if (endReason != Game.EndReason_e.StageClear)
					throw null; // never

				string afterNovelScenarioName = this.StageNo2AfterNovelScenarioName[stageNo];

				if (afterNovelScenarioName == null)
					throw null; // never

				using (new Novel())
				{
					Novel.I.Status = new NovelStatus()
					{
						Scenario = new Scenario(afterNovelScenarioName),
					};

					Novel.I.Perform();
				}
				if (stageNo == 5) // ? Wily Stage
				{
					gameStatus.WilyStageIndex = 1;
					this.S_PasswordDisplay(gameStatus);
					this.WilyStageLoop(gameStatus);
					break;
				}
				this.S_PasswordDisplay(gameStatus);
			}
		}

		private void S_PasswordDisplay(GameStatus gameStatus)
		{
			using (new PasswordDisplay())
			{
				PasswordDisplay.I.GameStatus = gameStatus;
				PasswordDisplay.I.Perform();
			}
		}

		private string[] WilyStageIndex2StartMapName = new string[]
		{
			null, // ワイリーステージ.1 -- StageNo2StartMapName[5] と同じなので、ここには定義しない。
			"Stage5-2/Start", // ワイリーステージ.2
			"Stage5-3/Start", // ワイリーステージ.3
			"Stage5-4/Start", // ワイリーステージ.4(Final)
		};

		private void WilyStageLoop(GameStatus gameStatus)
		{
			for (; ; )
			{
				string startMapName = this.StageNo2StartMapName[gameStatus.WilyStageIndex];

				if (startMapName == null)
					throw null; // never

				Game.EndReason_e endReason;

				using (new Game())
				{
					Game.I.World = new World(startMapName);
					Game.I.Status = gameStatus;
					Game.I.Perform();

					endReason = Game.I.EndReason;
				}
				if (endReason == Game.EndReason_e.ReturnToTitleMenu)
					break;

				if (endReason != Game.EndReason_e.StageClear)
					throw null; // never

				gameStatus.WilyStageIndex++;
			}
		}
	}
}
