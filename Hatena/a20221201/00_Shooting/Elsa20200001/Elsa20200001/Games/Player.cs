using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
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
		public double Reborn_X;
		public double Reborn_Y;
		public DDCrash Crash;
		public int DeadFrame = 0; // 0 == 無効, 1～ == 死亡中
		public int RebornFrame = 0; // 0 == 無効, 1～ == 登場中
		public int InvincibleFrame = 0; // 0 == 無効, 1～ == 無敵時間中

		public void Draw()
		{
			if (1 <= this.RebornFrame)
			{
				DDDraw.SetAlpha(0.5);
				DDDraw.DrawCenter(Ground.I.Picture.Player, this.Reborn_X, this.Reborn_Y);
				DDDraw.Reset();

				return;
			}
			if (1 <= this.DeadFrame)
			{
				// noop // 描画は Game.Perform で行う。

				return;
			}
			if (1 <= this.InvincibleFrame)
			{
				DDDraw.SetAlpha(0.5);
				DDDraw.DrawCenter(Ground.I.Picture.Player, this.X, this.Y);
				DDDraw.Reset();

				return;
			}
			DDDraw.DrawCenter(Ground.I.Picture.Player, this.X, this.Y);
		}

		/// <summary>
		/// 攻撃を行う。
		/// </summary>
		public void Fire()
		{
			// memo: 将来的に武器毎にコードが実装され、メソッドがでかくなると思われる。

			if (Game.I.Frame % 6 == 0)
			{
				switch (Game.I.Status.AttackLevel)
				{
					case 0:
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y));
						break;

					case 1:
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y - 16.0));
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y + 16.0));
						break;

					case 2:
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y - 32.0));
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y));
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y + 32.0));
						break;

					case 3:
						Game.I.Shots.Add(new Shot_Test0001(this.X + 20.0, this.Y - 48.0));
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y - 16.0));
						Game.I.Shots.Add(new Shot_Test0001(this.X + 38.0, this.Y + 16.0));
						Game.I.Shots.Add(new Shot_Test0001(this.X + 20.0, this.Y + 48.0));
						break;

					default:
						throw null; // never
				}
				Ground.I.SE.PlayerShoot.Play();
			}
		}

		public void Bomb()
		{
			if (!Isボム発動中() && DDUtils.CountDown(ref Game.I.Status.ZanBomb))
			{
				Game.I.Shots.Add(new Shot_Testボム0001());
			}
		}

		private static bool Isボム発動中()
		{
			return Game.I.Shots.Iterate().Any(shot => shot.Kind == Shot.Kind_e.ボム);
		}
	}
}
