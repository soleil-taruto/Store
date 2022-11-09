using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Attacks;
using Charlotte.Games.Enemies;
using Charlotte.Games.Shots;

namespace Charlotte.Games
{
	/// <summary>
	/// プレイヤーに関する情報と機能
	/// 唯一のインスタンスを Game.I.Player に保持する。
	/// </summary>
	public class Player
	{
		public double X;
		public double Y;
		public double YSpeed;
		public bool FacingLeft;
		public int MoveFrame;
		public int JumpFrame;
		public int JumpCount;
		public int ShootingFrame; // 0 == 無効, 1～ == 射撃モーション維持 (カウントダウン)
		public int AirborneFrame; // 0 == 接地状態, 1～ == 滞空状態
		public int DeadFrame = 0; // 0 == 無効, 1～ == 死亡中
		public int DamageFrame = 0; // 0 == 無効, 1～ == ダメージ中
		public int InvincibleFrame = 0; // 0 == 無効, 1～ == 無敵時間中
		public int HP = 1; // -1 == 死亡, 1～ == 生存
		public int ShotChargePCT = 0; // チャージショット溜めPCT, 連携：GameCommon.ShotChargePCTToLevel()

		/// <summary>
		/// プレイヤーの攻撃モーション
		/// null の場合は無効
		/// null ではない場合 Attack.EachFrame() が実行される代わりに、プレイヤーの入力・被弾処理などは実行されない。
		/// </summary>
		public Attack Attack = null;

		/// <summary>
		/// プレイヤー描画の代替タスクリスト
		/// 空の場合は無効
		/// 空ではない場合 Draw_EL.ExecuteAllTask() が実行される代わりに Draw() の主たる処理は実行されない。
		/// --- プレイヤーの攻撃モーションから使用されることを想定する。
		/// </summary>
		public DDTaskList Draw_EL = new DDTaskList();

		public void Draw()
		{
			if (1 <= this.Draw_EL.Count)
			{
				this.Draw_EL.ExecuteAllTask();
				return;
			}

			double xZoom = this.FacingLeft ? -1 : 1;
			double yZoom = 1.0;
			DDPicture picture = null;
			double xa = 0.0;
			double ya = 0.0;

			// 各種モーション
			{
				// ? 滞空状態
				if (
					1 <= Game.I.Player.AirborneFrame ||
					Game.I.Player.YSpeed < -SCommon.MICRO // ジャンプ中に梯子の一番上あたりに来たとき、接地状態になってしまわないように、この判定が要る。
					)
				{
					if (1 <= this.ShootingFrame)
					{
						picture = Ground.I.Picture.Chara_A01_Jump_Attack;
					}
					else
					{
						picture = Ground.I.Picture.Chara_A01_Jump;
					}
				}
				else if (1 <= this.MoveFrame)
				{
					if (1 <= this.ShootingFrame)
					{
						picture = Ground.I.Picture.Chara_A01_Run_Attack[(Game.I.Frame / 5) % Ground.I.Picture.Chara_A01_Run_Attack.Length];
					}
					else
					{
						int koma = Game.I.Frame / 5;

						if (koma == 0)
							picture = Ground.I.Picture.Chara_A01_Wait_Start;
						else
							picture = Ground.I.Picture.Chara_A01_Run[koma % Ground.I.Picture.Chara_A01_Run.Length];
					}
				}
				else
				{
					if (1 <= this.ShootingFrame)
					{
						picture = Ground.I.Picture.Chara_A01_Wait_Attack;
					}
					else
					{
						if (DDEngine.ProcFrame / 10 % 20 == 0)
							picture = Ground.I.Picture.Chara_A01_Wait_Mabataki;
						else
							picture = Ground.I.Picture.Chara_A01_Wait;
					}
				}
			}

			if (1 <= this.DeadFrame) // 死亡モーション
			{
				// 注意：this.DeadFrame == 0 ～ Consts.PLAYER_DEAD_FRAME_MAX + 2

				goto endDraw; // ティウンティウンになったので、描画しない。
			}
			if (1 <= this.DamageFrame) // 被弾モーション
			{
				// 注意：this.DamageFrame == 0 ～ Consts.PLAYER_DAMAGE_FRAME_MAX + 2

				if (this.DamageFrame / 6 % 2 == 0)
					picture = Ground.I.Picture.Effect_A01_Shock_A;
				else
					picture = Ground.I.Picture.Chara_A01_Jump_Damage;
			}
			if (1 <= this.DamageFrame || 1 <= this.InvincibleFrame)
			{
				DDDraw.SetTaskList(DDGround.EL);
				//DDDraw.SetTaskList(DDGround.EL_先行); //　old
			}
			if (1 <= this.InvincibleFrame)
			{
				DDDraw.SetAlpha(0.5);
			}

			double x = this.X;
			double y = this.Y;

			// 整数化
			x = (int)x;
			y = (int)y;

			DDDraw.DrawBegin(
				picture,
				x - DDGround.ICamera.X + (xa * xZoom),
				y - DDGround.ICamera.Y + ya
				);
			DDDraw.DrawZoom_X(xZoom);
			DDDraw.DrawZoom_Y(yZoom);
			DDDraw.DrawEnd();
			DDDraw.Reset();

		endDraw:
			;
		}

		public DDCrash GetCrash()
		{
			if (this.Attack is Attack_Ladder)
			{
				return DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(20.0, 30.0));
			}
			else if (this.Attack is Attack_Sliding)
			{
				return DDCrashUtils.Circle(new D2Point(this.X, this.Y + 10.0), 10.0);
			}
			else if (1 <= this.AirborneFrame)
			{
				//return DDCrashUtils.Point(new D2Point(this.X, this.Y));
				return DDCrashUtils.Circle(new D2Point(this.X, this.Y), 10.0);
			}
			else
			{
				return DDCrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(20.0, 30.0));
			}
		}

		/// <summary>
		/// 攻撃
		/// ショットの強さレベル：
		/// -- 関連：GameCommon.ShotChargePCTToLevel()
		/// </summary>
		/// <param name="level">ショットの強さレベル</param>
		public void Shoot(int level)
		{
			switch (Game.I.Status.Equipment)
			{
				case GameStatus.Equipment_e.Normal:
					Game.I.Shots.Add(new Shot_Normal(
						Game.I.Player.X + 34.0 * (Game.I.Player.FacingLeft ? -1 : 1),
						Game.I.Player.Y - 2.0,
						Game.I.Player.FacingLeft,
						level
						));
					break;

				case GameStatus.Equipment_e.跳ねる陰陽玉:
					Game.I.Shots.Add(new Shot_跳ねる陰陽玉(
						// 初期位置調整は Shot 側で行う。
						Game.I.Player.X,
						Game.I.Player.Y,
						Game.I.Player.FacingLeft,
						level
						));
					break;

				case GameStatus.Equipment_e.ハンマー陰陽玉:
					if (level == 1 && Game.I.Shots.Iterate().Any(shot => shot is Shot_ハンマー陰陽玉)) // 既に打っている場合のみ level == 1 のみ抑止
						break;

					foreach (Shot shot in Game.I.Shots.Iterate().Where(shot => shot is Shot_ハンマー陰陽玉))
						shot.Kill();

					Game.I.Shots.Add(new Shot_ハンマー陰陽玉(
						// 初期位置はプレイヤー中央で良い。
						Game.I.Player.X,
						Game.I.Player.Y,
						Game.I.Player.FacingLeft,
						level
						));
					break;

				case GameStatus.Equipment_e.エアーシューター:
					for (int order = 0; order < 3; order++)
					{
						Game.I.Shots.Add(new Shot_AirShooter(
							// 初期位置調整は Shot 側で行う。
							Game.I.Player.X,
							Game.I.Player.Y,
							Game.I.Player.FacingLeft,
							order,
							level
							));
					}
					break;

				case GameStatus.Equipment_e.マグネットエアー:
					Game.I.Shots.Add(new Shot_MagnetAir(
						Game.I.Player.X + 34.0 * (Game.I.Player.FacingLeft ? -1 : 1),
						Game.I.Player.Y - 2.0,
						Game.I.Player.FacingLeft,
						level
						));
					break;

				default:
					throw null; // never
			}
		}
	}
}
