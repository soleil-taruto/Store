using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Shots;
using Charlotte.Games.Shots.Tests;

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
		public DDCrash Crash;
		public int MoveFrame;
		public bool MoveSlow; // ? 低速移動
		public bool JumpLock; // ? ジャンプ・ロック -- ジャンプしたらボタンを離すまでロックする。
		public int JumpCount;
		public int JumpFrame;
		public int AirborneFrame; // 0 == 接地状態, 1～ == 滞空状態
		public int ShagamiFrame; // 0 == 無効, 1～ == しゃがみ中
		public int UwamukiFrame; // 0 == 無効, 1～ == 上向き中
		public int ShitamukiFrame; // 0 == 無効, 1～ == 下向き中
		public int AttackFrame; // 0 == 無効, 1～ == 攻撃中
		public int DamageFrame; // 0 == 無効, 1～ == ダメージ中
		public int InvincibleFrame; // 0 == 無効, 1～ == 無敵時間中

		/// <summary>
		/// 体力
		/// -1 == 死亡
		/// 0 == (不使用・予約)
		/// 1～ == 残り体力
		/// </summary>
		public int HP = 1;

		public bool FacingTop
		{
			get { return 1 <= this.UwamukiFrame; }
		}

		public ShotCatalog.武器_e 武器 = ShotCatalog.武器_e.NORMAL;

		private int PlayerLookLeftFrame = 0;

		public void Draw()
		{
			if (PlayerLookLeftFrame == 0 && DDUtils.Random.GetReal1() < 0.002) // キョロキョロするレート
				PlayerLookLeftFrame = 150 + (int)(DDUtils.Random.GetReal1() * 90.0);

			DDUtils.CountDown(ref PlayerLookLeftFrame);

			double xZoom = this.FacingLeft ? -1 : 1;

			// 立ち >

			DDPicture picture = Ground.I.Picture.PlayerStands[120 < PlayerLookLeftFrame ? 1 : 0][(DDEngine.ProcFrame / 20) % 2];

			if (1 <= this.MoveFrame)
			{
				if (this.MoveSlow)
				{
					picture = Ground.I.Picture.PlayerWalk[(DDEngine.ProcFrame / 10) % 2];
				}
				else
				{
					picture = Ground.I.Picture.PlayerDash[(DDEngine.ProcFrame / 5) % 2];
				}
			}
			if (1 <= this.AirborneFrame)
			{
				picture = Ground.I.Picture.PlayerJump[0];
			}
			if (1 <= this.ShagamiFrame)
			{
				picture = Ground.I.Picture.PlayerShagami;
			}

			// < 立ち

			// 攻撃中 >

			if (1 <= this.AttackFrame && this.UwamukiFrame == 0)
			{
				picture = Ground.I.Picture.PlayerAttack;

				if (1 <= this.MoveFrame)
				{
					if (this.MoveSlow)
					{
						picture = Ground.I.Picture.PlayerAttackWalk[(DDEngine.ProcFrame / 10) % 2];
					}
					else
					{
						picture = Ground.I.Picture.PlayerAttackDash[(DDEngine.ProcFrame / 5) % 2];
					}
				}
				if (1 <= this.AirborneFrame)
				{
					picture = Ground.I.Picture.PlayerAttackJump;
				}
				if (1 <= this.ShagamiFrame)
				{
					picture = Ground.I.Picture.PlayerAttackShagami;
				}
			}

			// < 攻撃中

			if (1 <= this.DamageFrame)
			{
				picture = Ground.I.Picture.PlayerDamage[0];
				xZoom *= -1;
			}
			if (1 <= this.DamageFrame || 1 <= this.InvincibleFrame)
			{
				DDDraw.SetTaskList(DDGround.EL); // 敵より前面に描画する。
			}
			if (1 <= this.InvincibleFrame)
			{
				DDDraw.SetAlpha(0.5);
			}
			DDDraw.DrawBegin(
				picture,
				this.X - DDGround.Camera.X,
				this.Y - DDGround.Camera.Y - 16
				);
			DDDraw.DrawZoom_X(xZoom);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		/// <summary>
		/// 攻撃を行う。
		/// </summary>
		public void Fire()
		{
			// memo: 将来的に武器毎にコードが実装され、メソッドがでかくなると思われる。

			const double Y_ADD_SHAGAMI = 9.0;
			const double Y_ADD_STAND = -7.0;

			switch (this.武器)
			{
				#region テスト用

				case ShotCatalog.武器_e.B_NORMAL:
					if (this.AttackFrame % 6 == 1)
					{
						double x = this.X;
						double y = this.Y;

						x += 30.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += Y_ADD_SHAGAMI;
						else
							y += Y_ADD_STAND;

						Game.I.Shots.Add(new Shot_BNormal(x, y, this.FacingLeft));
					}
					break;

				case ShotCatalog.武器_e.B_FIRE_BALL:
					if (this.AttackFrame % 12 == 1)
					{
						double x = this.X;
						double y = this.Y;

						x += 50.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += Y_ADD_SHAGAMI;
						else
							y += Y_ADD_STAND;

						Game.I.Shots.Add(new Shot_BFireBall(x, y, this.FacingLeft));
					}
					break;

				case ShotCatalog.武器_e.B_LASER:
					// 毎フレーム
					{
						double x = this.X;
						double y = this.Y;

						x += 38.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += Y_ADD_SHAGAMI;
						else
							y += Y_ADD_STAND;

						Game.I.Shots.Add(new Shot_BLaser(x, y, this.FacingLeft));
					}
					break;

				case ShotCatalog.武器_e.B_WAVE_BEAM:
					if (this.AttackFrame % 12 == 1)
					{
						double x = this.X;
						double y = this.Y;

						x += 32.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += Y_ADD_SHAGAMI;
						else
							y += Y_ADD_STAND;

						Game.I.Shots.Add(new Shot_BWaveBeam(x, y, this.FacingLeft));
					}
					break;

				#endregion

				case ShotCatalog.武器_e.NORMAL:
					if (this.AttackFrame % 6 == 1)
					{
						double x = this.X;
						double y = this.Y;

						if (1 <= this.UwamukiFrame)
							y -= 35.0;
						else
							x += 30.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += Y_ADD_SHAGAMI;
						else
							y += Y_ADD_STAND;

						Game.I.Shots.Add(new Shot_Normal(x, y, this.FacingLeft, 1 <= this.UwamukiFrame));
					}
					break;

				default:
					throw null; // never
			}
		}
	}
}
