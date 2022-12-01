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
		// ProcPlayer_Status();
		// ProcPlayer_当たり判定();
		//
		// プレイヤーの描画 -> Game.I.Player.Draw_EL
		//

		// ======================
		// ==== プレイヤー動作 ====
		// ======================

		public static void ProcPlayer_移動()
		{
			if (CamSlide())
				return;

			double speed;

			if (1 <= DDInput.R.GetInput())
				speed = GameConsts.PLAYER_SLOW_SPEED;
			else
				speed = GameConsts.PLAYER_SPEED;

			// 攻撃中は左右の方向転換を抑止する。

			if (1 <= DDInput.DIR_4.GetInput())
			{
				Game.I.Player.X -= speed;
				//Game.I.Player.FacingLeft = true; // 抑止
			}
			if (1 <= DDInput.DIR_6.GetInput())
			{
				Game.I.Player.X += speed;
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

		public static int GetPlayer_側面()
		{
			bool touchSide_L =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_側面判定Pt_YT)).Tile.IsWall() ||
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y)).Tile.IsWall() ||
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_側面判定Pt_YB)).Tile.IsWall();

			bool touchSide_R =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_側面判定Pt_YT)).Tile.IsWall() ||
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y)).Tile.IsWall() ||
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_側面判定Pt_YB)).Tile.IsWall();

			return (touchSide_L ? 1 : 0) | (touchSide_R ? 2 : 0);
		}

		public static int GetPlayer_側面Sub()
		{
			bool touchSide_L =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y)).Tile.IsWall();

			bool touchSide_R =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X, Game.I.Player.Y)).Tile.IsWall();

			return (touchSide_L ? 1 : 0) | (touchSide_R ? 2 : 0);
		}

		public static bool IsPlayer_脳天()
		{
			bool touchCeiling_L =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_脳天判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

			bool touchCeiling_M =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X, Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

			bool touchCeiling_R =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_脳天判定Pt_X, Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

			return (touchCeiling_L && touchCeiling_R) || touchCeiling_M;
		}

		public static bool IsPlayer_接地()
		{
			bool touchGround =
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X - GameConsts.PLAYER_接地判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y)).Tile.IsWall() ||
				Game.I.Map.GetCell(GameCommon.ToTablePoint(Game.I.Player.X + GameConsts.PLAYER_接地判定Pt_X, Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y)).Tile.IsWall();

			return touchGround;
		}

		// ===============================
		// ==== プレイヤー動作・接地系処理 ====
		// ===============================

		public static bool ProcPlayer_側面()
		{
			int flag = GetPlayer_側面();

			if (flag == 3) // 左右両方 -> 壁抜け防止のため再チェック
			{
				flag = GetPlayer_側面Sub();
			}

			if (flag == 3) // 左右両方
			{
				// noop
			}
			else if (flag == 1) // 左側面
			{
				Game.I.Player.X = GameCommon.ToTileCenterX(Game.I.Player.X - GameConsts.PLAYER_側面判定Pt_X) + GameConsts.TILE_W / 2 + GameConsts.PLAYER_側面判定Pt_X;
			}
			else if (flag == 2) // 右側面
			{
				Game.I.Player.X = GameCommon.ToTileCenterX(Game.I.Player.X + GameConsts.PLAYER_側面判定Pt_X) - GameConsts.TILE_W / 2 - GameConsts.PLAYER_側面判定Pt_X;
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
				Game.I.Player.Y = GameCommon.ToTileCenterY(Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y) + GameConsts.TILE_H / 2 + GameConsts.PLAYER_脳天判定Pt_Y;
				Game.I.Player.YSpeed = Math.Max(0.0, Game.I.Player.YSpeed);
			}
			return ret;
		}

		public static bool ProcPlayer_接地()
		{
			bool ret = IsPlayer_接地();

			if (ret)
			{
				Game.I.Player.Y = GameCommon.ToTileCenterY(Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) - GameConsts.TILE_H / 2 - GameConsts.PLAYER_接地判定Pt_Y;
				Game.I.Player.YSpeed = Math.Min(0.0, Game.I.Player.YSpeed);
			}
			return ret;
		}

		// ============================
		// ==== プレイヤー動作・その他 ====
		// ============================

		public static void ProcPlayer_Status()
		{
			if (1 <= Game.I.Player.DamageFrame && GameConsts.PLAYER_DAMAGE_FRAME_MAX < ++Game.I.Player.DamageFrame)
			{
				Game.I.Player.DamageFrame = 0;
				Game.I.Player.InvincibleFrame = 1;
			}
			if (1 <= Game.I.Player.InvincibleFrame && GameConsts.PLAYER_INVINCIBLE_FRAME_MAX < ++Game.I.Player.InvincibleFrame)
			{
				Game.I.Player.InvincibleFrame = 0;
			}
		}

		public static void ProcPlayer_当たり判定()
		{
			Game.I.Player.Crash = DDCrashUtils.Point(new D2Point(
				Game.I.Player.X,
				Game.I.Player.Y + 10.0
				));
		}

		// ====================================
		// ==== プレイヤー動作・カメラ・スライド ====
		// ====================================

		private static bool CamSlideMode = false; // ? カメラ・スライド_モード中
		private static int CamSlideCount;

		/// <summary>
		/// カメラ・スライド
		/// </summary>
		/// <returns>カメラ・スライド_モード中か</returns>
		private static bool CamSlide()
		{
			if (1 <= DDInput.L.GetInput())
			{
				if (DDInput.DIR_4.IsPound())
				{
					CamSlideCount++;
					Game.I.CamSlideX--;
				}
				if (DDInput.DIR_6.IsPound())
				{
					CamSlideCount++;
					Game.I.CamSlideX++;
				}
				if (DDInput.DIR_8.IsPound())
				{
					CamSlideCount++;
					Game.I.CamSlideY--;
				}
				if (DDInput.DIR_2.IsPound())
				{
					CamSlideCount++;
					Game.I.CamSlideY++;
				}
				DDUtils.ToRange(ref Game.I.CamSlideX, -1, 1);
				DDUtils.ToRange(ref Game.I.CamSlideY, -1, 1);

				CamSlideMode = true;
			}
			else
			{
				if (CamSlideMode && CamSlideCount == 0)
				{
					Game.I.CamSlideX = 0;
					Game.I.CamSlideY = 0;
				}
				CamSlideMode = false;
				CamSlideCount = 0;
			}
			return CamSlideMode;
		}

		// =================================
		// ==== プレイヤー動作系 (ここまで) ====
		// =================================
	}
}
