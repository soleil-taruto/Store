using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public static class GameConsts
	{
		/// <summary>
		/// セーブデータ・スロット数
		/// </summary>
		public const int SAVE_DATA_SLOT_NUM = 14;

		/// <summary>
		/// 何もない空間のタイル名
		/// </summary>
		public const string TILE_NONE = "None";

		/// <summary>
		/// 何も配置しない場合の敵の名前
		/// </summary>
		public const string ENEMY_NONE = "None";

		/// <summary>
		/// マップデータの終端にあるパラメータのデフォルト値
		/// </summary>
		public const string MAPPRM_DEFAULT_VALUE = "Default";

		// マップセルのサイズ(ドット単位)
		//
		public const int TILE_W = 32;
		public const int TILE_H = 32;

		public const int MINI_TILE_W = 16;
		public const int MINI_TILE_H = 16;

		public const int PLAYER_DAMAGE_FRAME_MAX = 5;
		public const int PLAYER_INVINCIBLE_FRAME_MAX = 60;

		public const double PLAYER_SPEED = 3.0;
		public const double PLAYER_FAST_SPEED_RATE = 2.0;
		public const double PLAYER_SLOW_SPEED_RATE = 0.75;
	}
}
