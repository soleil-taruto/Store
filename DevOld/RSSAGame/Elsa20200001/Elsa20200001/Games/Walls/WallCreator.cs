using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Walls
{
	public static class WallCreator
	{
		public static Wall Create(string name)
		{
			if (name == GameConsts.NAME_DEFAULT)
				return new Wall_Dark();

			Wall wall;

			switch (name)
			{
				//case Consts.WALL_DEFAULT: wall = new Wall_Dark(); break; // 難読化のため、ここに書けない。
				//case "B0001": wall = new Wall_Simple(Ground.I.Picture.Wall_B0001); break;
				//case "B0002": wall = new Wall_Simple(Ground.I.Picture.Wall_B0002); break;
				//case "B0003": wall = new Wall_Simple(Ground.I.Picture.Wall_B0003); break;
				case "Dark": wall = new Wall_Dark(); break;
				case "None": wall = new Wall_None(); break;
				case "東方風": wall = new Wall_東方風(); break;
				case "ロックマン風": wall = new Wall_ロックマン風(); break;

				// 新しい壁紙をここへ追加..

				default:
					throw new DDError("name: " + name);
			}
			return wall;
		}
	}
}
