using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

namespace Charlotte.Games
{
	public class Map
	{
		public string MapFile;

		public Map(string mapFile)
		{
			this.MapFile = mapFile;
			this.Load();
		}

		public MapCell[,] Table; // 添字：[x,y]
		public int W;
		public int H;
		public string WallName;
		public string MusicName;
		public bool 穴に落ちたら死亡;

		public void Load()
		{
			string[] lines = SCommon.TextToLines(SCommon.ENCODING_SJIS.GetString(DDResource.Load(this.MapFile)));
			int c = 0;

			lines = lines.Where(line => line != "" && line[0] != ';').ToArray(); // 空行とコメント行を除去

			int w = int.Parse(lines[c++]);
			int h = int.Parse(lines[c++]);

			if (w < 1 || SCommon.IMAX < w) throw new DDError();
			if (h < 1 || SCommon.IMAX < h) throw new DDError();

			this.Table = new MapCell[w, h];

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					MapCell cell = new MapCell();

					if (c < lines.Length)
					{
						string[] tokens = SCommon.Tokenize(lines[c++], "\t");
						int d = 0;

						d++; // Skip

						cell.TileName = Common.GetElement(tokens, d++, GameConsts.TILE_NONE);
						cell.Tile = TileCatalog.Create(cell.TileName);
						cell.EnemyName = Common.GetElement(tokens, d++, GameConsts.ENEMY_NONE);

						// 新しい項目をここへ追加..

						this.Table[x, y] = cell;
					}
					else
					{
						cell.TileName = GameConsts.TILE_NONE;
						cell.Tile = new Tile_None();
						cell.EnemyName = GameConsts.ENEMY_NONE;
					}
					this.Table[x, y] = cell;
				}
			}
			this.W = w;
			this.H = h;
			this.WallName = Common.GetElement(lines, c++, GameConsts.NAME_DEFAULT);
			this.MusicName = Common.GetElement(lines, c++, GameConsts.NAME_DEFAULT);
			this.穴に落ちたら死亡 = int.Parse(Common.GetElement(lines, c++, "1")) != 0;
		}

		public void Save()
		{
			List<string> lines = new List<string>();
			int w = this.W;
			int h = this.H;

			lines.Add(w.ToString());
			lines.Add(h.ToString());

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					MapCell cell = this.Table[x, y];

					lines.Add(string.Join("\t", cell.TileName == GameConsts.TILE_NONE ? 0 : 1, cell.TileName, cell.EnemyName));
				}
			}
			lines.Add("");
			lines.Add("; #---------------------------------------------------------------------#");
			lines.Add("; | 注意：このファイルにコメントを書いても次のマップ保存時に除去される。|");
			lines.Add("; #---------------------------------------------------------------------#");
			lines.Add("");
			lines.Add("; WallName");
			lines.Add(this.WallName);
			lines.Add("");
			lines.Add("; MusicName");
			lines.Add(this.MusicName);
			lines.Add("");
			lines.Add("; 穴に落ちたら死亡 (1=有効, 0=無効)");
			lines.Add("" + (this.穴に落ちたら死亡 ? 1 : 0));

			DDResource.Save(this.MapFile, SCommon.ENCODING_SJIS.GetBytes(SCommon.LinesToText(lines.ToArray())));
		}

		public MapCell GetCell(I2Point pt)
		{
			return this.GetCell(pt.X, pt.Y);
		}

		/// <summary>
		/// マップ上のマップセルを取得する。
		/// </summary>
		/// <param name="x">マップセルの位置(X-位置_テーブル座標)</param>
		/// <param name="y">マップセルの位置(Y-位置_テーブル座標)</param>
		/// <returns>マップセル</returns>
		public MapCell GetCell(int x, int y)
		{
			if (x < 0 || this.W <= x)
				return GameCommon.DefaultMapCell;

			if (y < 0)
			{
				// マップ最上段の梯子の上は梯子
				if (this.Table[x, 0].Tile.GetKind() == Tile.Kind_e.LADDER)
					return GameCommon.DefaultMapCell_Ladder;

				return GameCommon.DefaultMapCell;
			}
			if (this.H <= y)
			{
				// マップ最下段の梯子の下は梯子
				if (this.Table[x, this.H - 1].Tile.GetKind() == Tile.Kind_e.LADDER)
					return GameCommon.DefaultMapCell_Ladder;

				return GameCommon.DefaultMapCell;
			}
			return this.Table[x, y];
		}

		public bool FindCell(out int x, out int y, Predicate<MapCell> match)
		{
			for (x = 0; x < this.W; x++)
				for (y = 0; y < this.H; y++)
					if (match(this.Table[x, y]))
						return true;

			x = -1;
			y = -1;

			return false;
		}

		/// <summary>
		/// 指定されたマップ上のポイントに接地判定があるか判定する。
		/// </summary>
		/// <param name="x">マップ上のポイント(X-座標_ドット単位)</param>
		/// <param name="y">マップ上のポイント(Y-座標_ドット単位)</param>
		/// <returns>接地判定があるか</returns>
		public bool IsGroundPoint(double x, double y)
		{
			int ix = (int)x;
			int iy = (int)y;

			int mx = ix / GameConsts.TILE_W;
			int my = iy / GameConsts.TILE_H;
			//int sx = ix % Consts.TILE_W;
			int sy = iy % GameConsts.TILE_H;

			if (this.GetCell(mx, my).Tile.IsWall())
				return true;

			if (
				this.GetCell(mx, my - 1).Tile.GetKind() != Tile.Kind_e.LADDER &&
				this.GetCell(mx, my - 0).Tile.GetKind() == Tile.Kind_e.LADDER &&
				sy < GameConsts.LADDER_TOP_GROUND_Y_SIZE
				)
				return true;

			if (
				this.GetCell(mx, my).Tile.GetKind() == Tile.Kind_e.CLOUD &&
				sy < GameConsts.LADDER_TOP_GROUND_Y_SIZE
				)
				return true;

			return false;
		}
	}
}
