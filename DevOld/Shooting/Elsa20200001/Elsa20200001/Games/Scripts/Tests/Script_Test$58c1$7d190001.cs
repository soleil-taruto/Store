using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Walls.Tests;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_Test壁紙0001 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());

			for (; ; )
			{
				Game.I.Walls.Add(new Wall_Test0001());

				for (int c = 0; c < 300; c++)
					yield return true;

				Game.I.Walls.Add(new Wall_Test0002());

				for (int c = 0; c < 300; c++)
					yield return true;

				Game.I.Walls.Add(new Wall_Test0003());

				for (int c = 0; c < 300; c++)
					yield return true;

				Game.I.Walls.Add(new Wall_Test0004());

				for (int c = 0; c < 300; c++)
					yield return true;
			}
		}
	}
}
