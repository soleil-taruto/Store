using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls
{
	public class Wall_Dark : Wall
	{
		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				DDCurtain.DrawCurtain();
				yield return true;
			}
		}
	}
}
