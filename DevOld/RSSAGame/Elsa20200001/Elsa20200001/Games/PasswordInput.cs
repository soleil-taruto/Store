using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class PasswordInput : IDisposable
	{
		public GameStatus LoadedGameStatus = null; // null == 入力キャンセル

		// <---- ret

		public static PasswordInput I;

		public PasswordInput()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			// TODO
			// TODO
			// TODO

			bool[,] password = null;

			UInt16 gameStatusValue = GameStatus.PasswordConv.GetValue(password);
			GameStatus gameStatus = GameStatus.Deserialize(gameStatusValue);

			this.LoadedGameStatus = gameStatus;
		}
	}
}
