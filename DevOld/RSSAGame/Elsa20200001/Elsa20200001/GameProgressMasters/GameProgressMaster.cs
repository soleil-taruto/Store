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
			"Stage1/t0001", // Stage1
			"Stage2/t0001", // Stage2
			"Stage3/t0001", // Stage3
			"Stage4/t0001", // Stage4
			"Stage5/t0001", // Stage5
			"Stage6/t0001", // Stage6
			"Stage7/t0001", // Stage7
			"Stage8/t0001", // Stage8
			"Stage9/t0001", // Stage9
		};

		private string[] StageNo2AfterNovelScenarioName = new string[]
		{
			null,
			"Stage1_Cleared", // Stage1
			"Stage2_Cleared", // Stage2
			"Stage3_Cleared", // Stage3
			"Stage4_Cleared", // Stage4
			"Stage5_Cleared", // Stage5
			"Stage6_Cleared", // Stage6
			"Stage7_Cleared", // Stage7
			"Stage8_Cleared", // Stage8
			"Stage9_Cleared", // Stage9
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

				using (new Game())
				{
					Game.I.World = new World(startMapName);
					Game.I.Status = gameStatus;
					Game.I.Perform();

					endReason = Game.I.EndReason;

					// ステージクリアによるインベントリの変更(武器取得)は Game.I.Perform() 内で行うこと。
				}
				if (endReason == Game.EndReason_e.ReturnToTitleMenu)
					break;

				// この時点で endReason == Game.EndReason_e.StageClear しか有り得ない。

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
			null, // ワイリーステージ.1 == StageNo2StartMapName[5]
			"Stage5_1/t0001", // ワイリーステージ.2
			"Stage5_2/t0001", // ワイリーステージ.3
			"Stage5_3/t0001", // ワイリーステージ.4(Final)
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

				// この時点で endReason == Game.EndReason_e.StageClear しか有り得ない。

				gameStatus.WilyStageIndex++;
			}
		}
	}
}
