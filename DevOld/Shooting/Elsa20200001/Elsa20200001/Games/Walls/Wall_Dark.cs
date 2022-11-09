using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls
{
	public class Wall_Dark : Wall
	{
		public override IEnumerable<bool> E_Draw()
		{
			this.FilledFlag = true;

			for (; ; )
			{
				DDCurtain.DrawCurtain();
				yield return true;
			}
		}
	}
}
