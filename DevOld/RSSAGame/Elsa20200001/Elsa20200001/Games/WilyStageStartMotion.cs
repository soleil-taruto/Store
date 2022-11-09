using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class WilyStageStartMotion : IDisposable
	{
		public static WilyStageStartMotion I;

		public WilyStageStartMotion()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform(int stageIndex)
		{
			// TODO
		}
	}
}
