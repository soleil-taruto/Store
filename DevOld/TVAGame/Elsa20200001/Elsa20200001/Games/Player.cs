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
			WAVE,
			SPREAD,
			BOUNCE,
		}

		public static int 武器_e_Length = Enum.GetValues(typeof(武器_e)).Length;

		public double X;
		public double Y;
		public int FaceDirection; // プレイヤーが向いている方向 { 2, 4, 6, 8 } == { 下, 左, 右, 上 }
		public int MoveFrame;
		public int AttackFrame;
		public int DeadFrame = 0; // 0 == 無効, 1～ == 死亡中
		public int DamageFrame = 0; // 0 == 無効, 1～ == ダメージ中
		public int InvincibleFrame = 0; // 0 == 無効, 1～ == 無敵時間中
		public int HP = 1; // -1 == 死亡, 1～ == 生存
		public 武器_e 選択武器 = 武器_e.NORMAL;

		public void Draw()
		{
			int koma = 1;

			if (1 <= this.MoveFrame)
			{
				koma = (Game.I.Frame / 5) % 4;

				if (koma == 3)
					koma = 1;
			}

			DDPicture picture = Ground.I.Picture2.GetPlayer(Game.I.Status.Chara).GetPicture(this.FaceDirection, koma);

			// ---- ダメージ中等差し替え ----

			if (1 <= this.DeadFrame)
			{
				DDDraw.SetTaskList(DDGround.EL);
				DDDraw.SetBright(1.0, 0.3, 0.3);
				DDDraw.SetAlpha(0.7);
			}
			else if (1 <= this.DamageFrame || 1 <= this.InvincibleFrame)
			{
				DDDraw.SetTaskList(DDGround.EL);
				DDDraw.SetAlpha(0.5);
			}

			// ----

			DDDraw.SetMosaic();
			DDDraw.DrawBegin(
				picture,
				(int)this.X - DDGround.ICamera.X,
				(int)this.Y - DDGround.ICamera.Y - 12.0
				);
			DDDraw.DrawZoom(2.0);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		private bool Attack_Wave_左回転Sw = false;

		public void Attack()
		{
			// 将来的に武器毎にコードが実装され、メソッドがでかくなると思われる。

			switch (this.選択武器)
			{
				case 武器_e.NORMAL:
					if (this.AttackFrame % 10 == 1)
					{
						Game.I.Shots.Add(new Shot_Normal(this.X, this.Y, this.FaceDirection));
					}
					break;

				case 武器_e.WAVE:
					if (this.AttackFrame % 20 == 1)
					{
						Game.I.Shots.Add(new Shot_Wave(this.X, this.Y, this.FaceDirection, this.Attack_Wave_左回転Sw));
						this.Attack_Wave_左回転Sw = !this.Attack_Wave_左回転Sw;
					}
					break;

				case 武器_e.SPREAD:
					if (this.AttackFrame % 10 == 1)
					{
						for (int c = -2; c <= 2; c++)
						{
							Game.I.Shots.Add(new Shot_Spread(this.X, this.Y, this.FaceDirection, 0.3 * c));
						}
					}
					break;

				case 武器_e.BOUNCE:
					if (this.AttackFrame % 25 == 1)
					{
						for (int c = -1; c <= 1; c++)
						{
							Game.I.Shots.Add(new Shot_Bounce(this.X, this.Y, GameCommon.Rotate(this.FaceDirection, c)));
						}
					}
					break;

				default:
					throw null; // never
			}
		}
	}
}
