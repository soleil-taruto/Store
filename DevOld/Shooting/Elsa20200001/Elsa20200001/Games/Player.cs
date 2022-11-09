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
		public const int SPEED_LEVEL_MIN = 1;
		public const int SPEED_LEVEL_DEF = 3;
		public const int SPEED_LEVEL_MAX = 5;

		// 攻撃レベル
		// 初期：0
		// 範囲：0 ～ ATTACK_LEVEL_MAX
		//
		public const int ATTACK_LEVEL_MAX = 3;

		public double X;
		public double Y;
		public double Reborn_X;
		public double Reborn_Y;

		public int SpeedLevel = SPEED_LEVEL_DEF;
		public int AttackLevel = 0;

		public int DeadFrame = 0; // 0 == 無効, 0< == 死亡中
		public int RebornFrame = 0; // 0 == 無効, 0< == 登場中
		public int InvincibleFrame = 0; // 0 == 無効, 0< == 無敵時間中

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

		public void Shoot()
		{
			// memo:
			// 武器の種類が増えた場合、このメソッド内で分岐してこのメソッド内で処理してしまって良いと思う。
			// 下手にクラスに分けると却って面倒臭いことになりそうなので、、
			// とは言え、状況次第で臨機応変に検討すること。

			if (Game.I.Frame % 6 == 0)
			{
				switch (this.AttackLevel)
				{
					case 0:
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y));
						break;

					case 1:
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y - 16.0));
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y + 16.0));
						break;

					case 2:
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y - 32.0));
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y));
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y + 32.0));
						break;

					case 3:
						Game.I.Shots.Add(new Shot_B0001(this.X + 20.0, this.Y - 48.0));
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y - 16.0));
						Game.I.Shots.Add(new Shot_B0001(this.X + 38.0, this.Y + 16.0));
						Game.I.Shots.Add(new Shot_B0001(this.X + 20.0, this.Y + 48.0));
						break;

					default:
						throw null; // never
				}
			}
		}

		public void Bomb()
		{
			if (!Isボム発動中() && DDUtils.CountDown(ref Game.I.Status.ZanBomb))
			{
				Game.I.Shots.Add(new Shot_Bボム());
			}
		}

		private static bool Isボム発動中()
		{
			return Game.I.Shots.Iterate().Any(shot => shot.Kind == Shot.Kind_e.ボム);
		}
	}
}
