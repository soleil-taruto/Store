using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class GameTest
	{
		public void Test01()
		{
			using (new Game())
			{
				Game.I.Map = MapLoader.Load("t0001");
				Game.I.Status = new GameStatus();

				Game.I.Perform();
			}
		}

		public void Test02()
		{
			// none
		}

		public void Test03()
		{
			string mapName;

			// -- choose one --

			mapName = "t0001";
			//mapName = "t0002";
			//mapName = "t0003";

			// --

			using (new Game())
			{
				Game.I.Map = MapLoader.Load(mapName);
				Game.I.Status = new GameStatus();

				Game.I.Perform();
			}
		}
	}
}
