using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Walls
{
	public class Wall_None : Wall
	{
		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// noop

				yield return true;
			}
		}
	}
}
