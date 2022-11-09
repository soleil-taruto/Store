using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public static class GameCommon
	{
		// ==================
		// ==== Map 関連 ====
		// ==================

		public static MapCell DefaultCell = new MapCell();
		public static MapCell DefaultCell_2_Wall = new MapCell();
		public static MapCell DefaultCell_4_Wall = new MapCell();
		public static MapCell DefaultCell_6_Wall = new MapCell();
		public static MapCell DefaultCell_8_Wall = new MapCell();

		public static void INIT()
		{
			DefaultCell_2_Wall.Wall_2.Kind = MapWall.Kind_e.WALL;
			DefaultCell_4_Wall.Wall_4.Kind = MapWall.Kind_e.WALL;
			DefaultCell_6_Wall.Wall_6.Kind = MapWall.Kind_e.WALL;
			DefaultCell_8_Wall.Wall_8.Kind = MapWall.Kind_e.WALL;
		}

		public static string MapNameToMapFile(string name)
		{
			return @"res\Map\" + name + ".txt";
		}

		// ===========================
		// ==== Map 関連 (ここまで) ====
		// ===========================

		public static T GetElement<T>(T[] arr, int index, T defval) // HACK: 不使用？
		{
			if (index < arr.Length)
			{
				return arr[index];
			}
			else
			{
				return defval;
			}
		}

		public static int RotL(int direction)
		{
			return direction / 2 + ((direction / 2) % 2) * 5;
		}

		public static int RotR(int direction)
		{
			return direction * 2 - (direction / 6) * 10;
		}
	}
}
