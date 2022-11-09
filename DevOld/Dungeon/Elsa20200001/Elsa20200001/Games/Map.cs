using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	/// <summary>
	/// マップ
	/// マップファイルからロードするには MapLoader.Load() を呼び出すこと。
	/// </summary>
	public class Map
	{
		private MapCell[] Cells;

		public int W { get; private set; }
		public int H { get; private set; }

		public Map(int w, int h)
		{
			if (
				w < 1 || SCommon.IMAX < w ||
				h < 1 || SCommon.IMAX < h
				)
				throw new DDError();

			this.Cells = new MapCell[w * h];

			for (int index = 0; index < this.Cells.Length; index++)
				this.Cells[index] = new MapCell();

			this.W = w;
			this.H = h;
		}

		public MapCell this[int x, int y]
		{
			get
			{
				if (
					x < 0 || this.W <= x ||
					y < 0 || this.H <= y
					)
				{
					if (0 <= y && y < this.H)
					{
						if (x == -1)
							return GameCommon.DefaultCell_6_Wall;

						if (x == this.W)
							return GameCommon.DefaultCell_4_Wall;
					}
					if (0 <= x && x < this.W)
					{
						if (y == -1)
							return GameCommon.DefaultCell_2_Wall;

						if (y == this.H)
							return GameCommon.DefaultCell_8_Wall;
					}
					return GameCommon.DefaultCell;
				}
				return this.Cells[x + y * this.W];
			}
		}

		public bool Find(out int x, out int y, Predicate<MapCell> match)
		{
			for (x = 0; x < this.W; x++)
				for (y = 0; y < this.H; y++)
					if (match(this[x, y]))
						return true;

			x = -1;
			y = -1;

			return false;
		}

		public bool Find(out int x, out int y, out int direction, Predicate<MapWall> match)
		{
			for (x = 0; x < this.W; x++)
				for (y = 0; y < this.H; y++)
					for (direction = 2; direction <= 8; direction += 2)
						if (match(this[x, y].GetWall(direction)))
							return true;

			x = -1;
			y = -1;
			direction = -1;

			return false;
		}

		// プロパティ >

		public DDPicture BackgroundPicture;
		public DDPicture WallPicture;
		public DDPicture GatePicture;
		public DDMusic Music;

		// < プロパティ
	}
}
