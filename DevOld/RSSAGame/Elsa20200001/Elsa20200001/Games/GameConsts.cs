using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public static class GameConsts
	{
		/// <summary>
		/// 何もない空間のタイル名
		/// </summary>
		public const string TILE_NONE = "None";

		/// <summary>
		/// 何も配置しない場合の敵の名前
		/// </summary>
		public const string ENEMY_NONE = "None";

		/// <summary>
		/// デフォルトの壁紙の名前
		/// デフォルトの音楽の名前
		/// </summary>
		public const string NAME_DEFAULT = "Default";

		public const int TILE_W = 32;
		public const int TILE_H = 32;

		public const int PLAYER_DEAD_FRAME_MAX = 180;
		public const int PLAYER_DAMAGE_FRAME_MAX = 40;
		public const int PLAYER_INVINCIBLE_FRAME_MAX = 60;

		public const int PLAYER_SHOOTING_FRAME_MAX = 10;

		/// <summary>
		/// プレイヤーの最大HP
		/// </summary>
		public const int PLAYER_HP_MAX = 30;

		/// <summary>
		/// ジャンプ回数の上限
		/// 1 == 通常
		/// 2 == 2-段ジャンプまで可能
		/// 3 == 3-段ジャンプまで可能
		/// ...
		/// </summary>
		public const int JUMP_MAX = 1;

		/// <summary>
		/// プレイヤーキャラクタの重力加速度
		/// </summary>
		public const double PLAYER_GRAVITY = 0.6;

		/// <summary>
		/// プレイヤーキャラクタの落下最高速度
		/// </summary>
		public const double PLAYER_FALL_SPEED_MAX = 10.0;
		//public const double PLAYER_FALL_SPEED_MAX = 9.0;
		//public const double PLAYER_FALL_SPEED_MAX = 8.0;

		/// <summary>
		/// プレイヤーキャラクタの(横移動)速度
		/// </summary>
		public const double PLAYER_SPEED = 4.0;

		//public const double PLAYER_ジャンプ初速度 = -12.0; // 4マス分の高さを登れる箇所と登れない箇所がある問題のため、廃止して -13.0 にした。@ 2021.7.10
		public const double PLAYER_ジャンプ初速度 = -13.0;
		//public const double PLAYER_ジャンプ初速度 = -16.0;

		// 上昇が速すぎると、脳天判定より先に側面判定に引っ掛かってしまう可能性がある。
		// -- ( -(PLAYER_ジャンプ初速度) < 脳天判定Pt_Y - 側面判定Pt_Y ) を維持すること。-- 左右同時接触時の再判定によりこの制約は緩和される。
		// 下降が速すぎると、接地判定より先に側面判定に引っ掛かってしまう可能性がある。
		// -- ( PLAYER_FALL_SPEED_MAX < 接地判定Pt_Y - 側面判定Pt_Y ) を維持すること。

		public const double PLAYER_側面判定Pt_X = 16.0 / 2;
		public const double PLAYER_側面判定Pt_Y = 20.0 / 2;
		public const double PLAYER_脳天判定Pt_X = 14.0 / 2;
		public const double PLAYER_脳天判定Pt_Y = 40.0 / 2;
		public const double PLAYER_接地判定Pt_X = 14.0 / 2;
		public const double PLAYER_接地判定Pt_Y = 48.0 / 2;

		// 2020.11.x
		// 崖っぷちギリギリまで行けるように、もう少し広げても良いかもしれないが -> 46 / 2
		// 1マスの穴に落ちられなくなるので妥協点としてこの幅 -> 30 / 2
		//
		public const double PLAYER_接地判定Pt_X_接地時のみ = 30.0 / 2;

		// 梯子の一番上のマップセルの上部の領域のみ接地判定アリとする。
		// この領域の高さ
		// 落下最高速度で落下中にすり抜けないように、落下最高速度より大きいこと
		// なお、この接地判定領域のおかげで梯子を登りきったとき梯子の上に立ってくれる。
		//
		public const int LADDER_TOP_GROUND_Y_SIZE = (int)PLAYER_FALL_SPEED_MAX + 2;

		/// <summary>
		/// パスワード(bool[,])の高さ・幅
		/// </summary>
		public const int PASSWORD_WH = 4;
	}
}
