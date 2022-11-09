using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public class MapCell
	{
		public MapWall Wall_2 = new MapWall(); // 南側
		public MapWall Wall_4 = new MapWall(); // 西側
		public MapWall Wall_6 = new MapWall(); // 東側
		public MapWall Wall_8 = new MapWall(); // 北側

		public MapWall GetWall(int direction)
		{
			switch (direction)
			{
				case 2: return this.Wall_2;
				case 4: return this.Wall_4;
				case 6: return this.Wall_6;
				case 8: return this.Wall_8;

				default:
					throw null; // never
			}
		}
	}
}
