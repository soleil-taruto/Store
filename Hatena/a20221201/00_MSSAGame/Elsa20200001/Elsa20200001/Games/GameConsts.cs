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

		public const int PLAYER_DAMAGE_FRAME_MAX = 20;
		public const int PLAYER_INVINCIBLE_FRAME_MAX = 60;

		/// <summary>
		/// プレイヤーキャラクタの重力加速度
		/// </summary>
		public const double PLAYER_GRAVITY = 1.0;

		/// <summary>
		/// プレイヤーキャラクタの落下最高速度
		/// </summary>
		public const double PLAYER_FALL_SPEED_MAX = 8.0;

		/// <summary>
		/// プレイヤーキャラクタの(横移動)速度
		/// </summary>
		public const double PLAYER_SPEED = 6.0;

		/// <summary>
		/// プレイヤーキャラクタの低速移動時の(横移動)速度
		/// </summary>
		public const double PLAYER_SLOW_SPEED = 2.0;

		/// <summary>
		/// プレイヤーキャラクタのジャンプによる上昇速度
		/// </summary>
		public const double PLAYER_JUMP_SPEED = -8.0;

		// 滞空中に壁に突進しても、脳天判定・接地判定に引っ掛からないように側面判定を先に行う。
		// -- ( 脳天判定Pt_X < 側面判定Pt_X && 接地判定Pt_X < 側面判定Pt_X ) を維持すること。
		// 上昇が速すぎると、脳天判定より先に側面判定に引っ掛かってしまう可能性がある。
		// -- ( -(PLAYER_JUMP_SPEED) < 脳天判定Pt_Y - 側面判定Pt_YT ) を維持すること。
		// 下降が速すぎると、接地判定より先に側面判定に引っ掛かってしまう可能性がある。
		// -- ( PLAYER_FALL_SPEED_MAX < 接地判定Pt_Y - 側面判定Pt_YB ) を維持すること。

		public const double PLAYER_側面判定Pt_X = 10.0;
		public const double PLAYER_側面判定Pt_YT = TILE_H / 2.0;
		public const double PLAYER_側面判定Pt_YB = TILE_H / 2.0;
		public const double PLAYER_脳天判定Pt_X = 9.0;
		public const double PLAYER_脳天判定Pt_Y = TILE_H;
		public const double PLAYER_接地判定Pt_X = 9.0;
		public const double PLAYER_接地判定Pt_Y = TILE_H;
	}
}
