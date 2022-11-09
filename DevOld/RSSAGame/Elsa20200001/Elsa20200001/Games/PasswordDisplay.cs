using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class PasswordDisplay : IDisposable
	{
		public GameStatus GameStatus;

		// <---- prm

		public static PasswordDisplay I;

		public PasswordDisplay()
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
		}
	}
}
