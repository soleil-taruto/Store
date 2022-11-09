using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class StageSelectMenu : IDisposable
	{
		public GameStatus GameStatus;

		// <---- prm

		public int SelectedStageNo = 1; // 1～9 == 選択したステージ

		// <---- ret

		public static StageSelectMenu I;

		public StageSelectMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			throw null; // TODO

			// ステージ選択後のモーションもここでやる予定
		}
	}
}
