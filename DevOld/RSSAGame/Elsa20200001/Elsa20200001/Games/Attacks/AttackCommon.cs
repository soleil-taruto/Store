using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Attacks
{
	/// <summary>
	/// Attack 共通
	/// Game.cs に実装されている(非アタック中と)共通する処理も実装する。
	/// -- 共通化は難しそうなので Game.cs と重複して実装する。
	/// </summary>
	public static class AttackCommon
	{
		// プレイヤー動作セット
		// -- この辺やっとけば良いんじゃないか的な
		//
		// ProcPlayer_移動();
		// ProcPlayer_Fall();
		//
		// ProcPlayer_側面();
		// ProcPlayer_脳天();
		// ProcPlayer_接地();
		//

		// ======================
		// ==== プレイヤー動作 ====
		// ======================

		public static void ProcPlayer_移動()
		{
			// 攻撃中は左右の方向転換を抑止する。

			double SPEED = GameConsts.PLAYER_SPEED;

			if (1 <= DDInput.DIR_4.GetInput())
			{
				Game.I.Player.X -= SPEED;
				//Game.I.Player.FacingLeft = true; // 抑止
			}
			if (1 <= DDInput.DIR_6.GetInput())
			{
				Game.I.Player.X += SPEED;
				//Game.I.Player.FacingLeft = false; // 抑止
			}
		}

		public static void ProcPlayer_Fall()
		{
			if (1 <= Game.I.Player.JumpFrame) // ? ジャンプ中(だった)
			{
				if (DDInput.A.GetInput() <= 0) // ? ジャンプを中断・終了した。
				{
					Game.I.Player.JumpFrame = 0;

					if (Game.I.Player.YSpeed < 0.0)
						Game.I.Player.YSpeed /= 2.0;
				}
			}

			// 重力による加速
			Game.I.Player.YSpeed += GameConsts.PLAYER_GRAVITY;

			// 自由落下の最高速度を超えないように矯正
			DDUtils.Minim(ref Game.I.Player.YSpeed, GameConsts.PLAYER_FALL_SPEED_MAX);

			// 自由落下
			Game.I.Player.Y += Game.I.Player.YSpeed;
		}

		// ===============================
		// ==== プレイヤー動作・接地系判定 ====
		// ===============================

		public static bool IsPlayer_側面()
		{
			return GetPlayer_側面() != 0;
		}

		/// <summary>
		/// 側面の接地判定を行う。
		/// 側面モード：
		/// -- 1 == 下段のみ
		/// -- 2 == 中段のみ
		/// -- 3 == 中段と下段
		/// -- 4 == 上段のみ
		/// -- 5 == 上段と下段
		/// -- 6 == 上段と中段
		/// -- 7 == 全て
		/// 判定値：
		/// -- 0 == 接地していない。
		/// -- 1 == 左側に接地している。
		/// -- 2 == 右側に接地している。
		/// -- 3 == 左右両方接地している。
		/// </summary>
		/// <param name="側面モード">側面モード</param>
		/// <returns>判定値</returns>
		public static int GetPlayer_側面(int 側面モード = 7)
		{
			bool touchSide_L =
				(側面モード & 4) != 0 && Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall() ||
				(側面モード & 2) != 0 && Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y)).Tile.IsWall() ||
				(側面モード & 1) != 0 && Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();

			bool touchSide_R =
				(側面モード & 4) != 0 && Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall() ||
				(側面モード & 2) != 0 && Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y)).Tile.IsWall() ||
				(側面モード & 1) != 0 && Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();

			return (touchSide_L ? 1 : 0) | (touchSide_R ? 2 : 0);
		}

		public static bool IsPlayer_脳天()
		{
			bool touchCeiling =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_脳天判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall() ||
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_脳天判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

			return touchCeiling;
		}

		public static bool IsPlayer_接地()
		{
			bool touchGround =
				Game.I.Map.IsGroundPoint(Game.I.Player.X - GameConsts.PLAYER_接地判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) ||
				Game.I.Map.IsGroundPoint(Game.I.Player.X + GameConsts.PLAYER_接地判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y);

			return touchGround;
		}

		// ===============================
		// ==== プレイヤー動作・接地系処理 ====
		// ===============================

		public static bool ProcPlayer_側面(int 側面モード = 7)
		{
			int flag;

			if (側面モード == 7)
			{
				flag = GetPlayer_側面();

				if (flag == 3) // 左右両方 -> 壁抜け防止のため再チェック
				{
					flag = GetPlayer_側面(2);
				}
			}
			else
			{
				flag = GetPlayer_側面(側面モード);
			}

			if (flag == 3) // 左右両方
			{
				// noop
			}
			else if (flag == 1) // 左側面
			{
				Game.I.Player.X = (double)SCommon.ToInt(Game.I.Player.X / GameConsts.TILE_W) * GameConsts.TILE_W + GameConsts.PLAYER_側面判定Pt_X;
			}
			else if (flag == 2) // 右側面
			{
				Game.I.Player.X = (double)SCommon.ToInt(Game.I.Player.X / GameConsts.TILE_W) * GameConsts.TILE_W - GameConsts.PLAYER_側面判定Pt_X;
			}
			else if (flag == 0) // なし
			{
				// noop
			}
			else
			{
				throw null; // never
			}
			return flag != 0;
		}

		public static bool ProcPlayer_脳天()
		{
			bool ret = IsPlayer_脳天();

			if (ret)
			{
				//if (Game.I.Player.YSpeed < 0.0) // 攻撃中につき判定を抑止
				{
					double plY = ((int)((Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y) / GameConsts.TILE_H) + 1) * GameConsts.TILE_H + GameConsts.PLAYER_脳天判定Pt_Y;

					Game.I.Player.Y = plY;
					//Game.I.Player.YSpeed = 0.0;
					Game.I.Player.YSpeed = Math.Max(0.0, Game.I.Player.YSpeed); // YSpeed をチェックしないため
				}
			}
			return ret;
		}

		public static bool ProcPlayer_接地()
		{
			bool ret = IsPlayer_接地();

			if (ret)
			{
				//if (0.0 < Game.I.Player.YSpeed) // 攻撃中につき判定を抑止
				{
					double plY = (int)((Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) / GameConsts.TILE_H) * GameConsts.TILE_H - GameConsts.PLAYER_接地判定Pt_Y;

					Game.I.Player.Y = plY;
					//Game.I.Player.YSpeed = 0.0;
					Game.I.Player.YSpeed = Math.Min(0.0, Game.I.Player.YSpeed); // YSpeed をチェックしないため
				}
			}
			return ret;
		}

		// =================================
		// ==== プレイヤー動作系 (ここまで) ====
		// =================================

		private static bool CamSlideMode = false; // ? カメラ・スライド_モード中
		private static bool CamSlided = false;

		public static bool CamSlide() // ? カメラ・スライド_モード中
		{
			if (1 <= DDInput.L.GetInput())
			{
				if (DDInput.DIR_4.IsPound())
				{
					CamSlided = true;
					Game.I.CamSlideX--;
				}
				if (DDInput.DIR_6.IsPound())
				{
					CamSlided = true;
					Game.I.CamSlideX++;
				}
				if (DDInput.DIR_8.IsPound())
				{
					CamSlided = true;
					Game.I.CamSlideY--;
				}
				if (DDInput.DIR_2.IsPound())
				{
					CamSlided = true;
					Game.I.CamSlideY++;
				}
				DDUtils.ToRange(ref Game.I.CamSlideX, -1, 1);
				DDUtils.ToRange(ref Game.I.CamSlideY, -1, 1);

				CamSlideMode = true;
			}
			else
			{
				if (CamSlideMode && !CamSlided)
				{
					Game.I.CamSlideX = 0;
					Game.I.CamSlideY = 0;
				}
				CamSlideMode = false;
				CamSlided = false;
			}
			return CamSlideMode;
		}
	}
}
