using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Scripts
{
	public static class ScriptCommon
	{
		public static IEnumerable<bool> Wrapper(Func<int> eachFrame)
		{
			for (; ; )
			{
				int waitCount = eachFrame();

				if (waitCount == -1)
					break;

				if (waitCount <= 0)
					throw null; // 想定外

				for (int c = 0; c < waitCount; c++)
					yield return true;
			}
		}
	}
}
