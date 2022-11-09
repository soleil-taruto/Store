using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;
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
		public enum 武器_e
		{
			NORMAL,
			FIRE_BALL,
			LASER,
			WAVE_BEAM,
		}

		public static string[] 武器_e_Names = new string[]
		{
			"NORMAL",
			"FIRE-BALL",
			"LASER",
			"WAVE-BEAM",
		};

		public double X;
		public double Y;
		public double YSpeed;
		public bool FacingLeft;
		public int MoveFrame;
		public bool MoveSlow; // ? 低速移動
		public int JumpCount;
		public int JumpFrame;
		public int AirborneFrame; // 0 == 接地状態, 1～ == 滞空状態
		public int ShagamiFrame;
		public int AttackFrame;
		public int DeadFrame = 0; // 0 == 無効, 1～ == 死亡中
		public int DamageFrame = 0; // 0 == 無効, 1～ == ダメージ中
		public int InvincibleFrame = 0; // 0 == 無効, 1～ == 無敵時間中
		public int HP = 1; // -1 == 死亡, 1～ == 生存

		public 武器_e 武器 = 武器_e.NORMAL;

		private int PlayerLookLeftFrame = 0;

		public void Draw()
		{
			if (PlayerLookLeftFrame == 0 && DDUtils.Random.Real() < 0.002) // キョロキョロするレート
				PlayerLookLeftFrame = 150 + (int)(DDUtils.Random.Real() * 90.0);

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

			if (1 <= this.AttackFrame)
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

			if (1 <= this.DeadFrame)
			{
				int koma = SCommon.ToRange(this.DeadFrame / 20, 0, 1);

				if (this.AirborneFrame == 0)
					koma *= 2;

				koma *= 2;
				koma++;

				picture = Ground.I.Picture.PlayerDamage[koma];

				DDDraw.SetTaskList(DDGround.EL);
			}
			if (1 <= this.DamageFrame)
			{
				picture = Ground.I.Picture.PlayerDamage[0];
				xZoom *= -1;
			}

			if (1 <= this.DamageFrame || 1 <= this.InvincibleFrame)
			{
				DDDraw.SetTaskList(DDGround.EL);
				DDDraw.SetAlpha(0.5);
			}
			DDDraw.DrawBegin(
				picture,
				SCommon.ToInt(this.X - DDGround.ICamera.X),
				SCommon.ToInt(this.Y - DDGround.ICamera.Y) - 16
				);
			DDDraw.DrawZoom_X(xZoom);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		public void Attack()
		{
			// 将来的に武器毎にコードが実装され、メソッドがでかくなると思われる。

			switch (this.武器)
			{
				case 武器_e.NORMAL:
					if (this.AttackFrame % 6 == 1)
					{
						double x = this.X;
						double y = this.Y;

						x += 30.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += 10.0;
						else
							y -= 4.0;

						Game.I.Shots.Add(new Shot_Normal(x, y, this.FacingLeft));
					}
					break;

				case 武器_e.FIRE_BALL:
					if (this.AttackFrame % 12 == 1)
					{
						double x = this.X;
						double y = this.Y;

						x += 50.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += 10.0;
						else
							y -= 4.0;

						Game.I.Shots.Add(new Shot_FireBall(x, y, this.FacingLeft));
					}
					break;

				case 武器_e.LASER:
					// 毎フレーム
					{
						double x = this.X;
						double y = this.Y;

						x += 38.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += 10.0;
						else
							y -= 4.0;

						Game.I.Shots.Add(new Shot_Laser(x, y, this.FacingLeft));
					}
					break;

				case 武器_e.WAVE_BEAM:
					if (this.AttackFrame % 12 == 1)
					{
						double x = this.X;
						double y = this.Y;

						x += 32.0 * (this.FacingLeft ? -1 : 1);

						if (1 <= this.ShagamiFrame)
							y += 10.0;
						else
							y -= 4.0;

						Game.I.Shots.Add(new Shot_WaveBeam(x, y, this.FacingLeft));
					}
					break;

				default:
					throw null; // never
			}
		}
	}
}
