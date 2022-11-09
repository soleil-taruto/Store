using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;

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
		// ProcPlayer_壁キャラ処理();
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
			ProcPlayer_移動(GameConsts.PLAYER_SPEED);
		}

		public static void ProcPlayer_移動(double speed)
		{
			if (CamSlide())
				return;

			bool dir2 = 1 <= DDInput.DIR_2.GetInput();
			bool dir4 = 1 <= DDInput.DIR_4.GetInput();
			bool dir6 = 1 <= DDInput.DIR_6.GetInput();
			bool dir8 = 1 <= DDInput.DIR_8.GetInput();

			int dir; // 1～9 == { 左下, 下, 右下, 左, 動かない, 右, 左上, 上, 右上 }

			if (dir2 && dir4)
				dir = 1;
			else if (dir2 && dir6)
				dir = 3;
			else if (dir4 && dir8)
				dir = 7;
			else if (dir6 && dir8)
				dir = 9;
			else if (dir2)
				dir = 2;
			else if (dir4)
				dir = 4;
			else if (dir6)
				dir = 6;
			else if (dir8)
				dir = 8;
			else
				dir = 5;

			//double speed = GameConsts.PLAYER_SPEED;
			double nanameSpeed = speed / Consts.ROOT_OF_2;

			switch (dir)
			{
				case 2:
					Game.I.Player.Y += speed;
					break;

				case 4:
					Game.I.Player.X -= speed;
					break;

				case 6:
					Game.I.Player.X += speed;
					break;

				case 8:
					Game.I.Player.Y -= speed;
					break;

				case 1:
					Game.I.Player.X -= nanameSpeed;
					Game.I.Player.Y += nanameSpeed;
					break;

				case 3:
					Game.I.Player.X += nanameSpeed;
					Game.I.Player.Y += nanameSpeed;
					break;

				case 7:
					Game.I.Player.X -= nanameSpeed;
					Game.I.Player.Y -= nanameSpeed;
					break;

				case 9:
					Game.I.Player.X += nanameSpeed;
					Game.I.Player.Y -= nanameSpeed;
					break;

				case 5:
					// 立ち止まったら座標を整数に矯正
					Game.I.Player.X = SCommon.ToInt(Game.I.Player.X);
					Game.I.Player.Y = SCommon.ToInt(Game.I.Player.Y);
					break;

				default:
					throw null; // never
			}
		}

		public static void ProcPlayer_壁キャラ処理()
		{
			壁キャラ処理.Perform(ref Game.I.Player.X, ref Game.I.Player.Y, v => v.GetKind() != Tile.Kind_e.SPACE);
		}

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
