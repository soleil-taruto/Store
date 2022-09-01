using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

namespace Charlotte.Games
{
	/// <summary>
	/// マップセル
	/// </summary>
	public class MapCell
	{
		public string TileName; // セーブ・ロード用
		public Tile Tile;
		public string EnemyName;

		// <---- prm

		/// <summary>
		/// このマップセルは「デフォルトのマップセル」か
		/// </summary>
		public bool IsDefault
		{
			get
			{
				return this == GameCommon.DefaultMapCell;
			}
		}
	}
}
