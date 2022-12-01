using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_敵
	/// </summary>
	public class Enemy_B0002 : Enemy
	{
		public Enemy_B0002(double x, double y)
			: base(x, y, 10, 3, false)
		{ }

		private const int HIT_BACK_FRAME_MAX = 10;

		private int HitBackFrame = 0; // 0 == 無効, 1～ ヒットバック中

		protected override IEnumerable<bool> E_Draw()
		{
			Game.I.Tasks.Add(SCommon.Supplier(this.E_DummyTask()));

			for (int frame = 0; ; frame++)
			{
				//while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では行動しない。
				//    yield return true;

				double SPEED = 2.0;
				double xBuru = 0.0;
				double yBuru = 0.0;

				// memo: SpecialDraw-方式を検討のこと。

				if (1 <= this.HitBackFrame)
				{
					int frm = this.HitBackFrame - 1;

					if (HIT_BACK_FRAME_MAX < frm)
					{
						this.HitBackFrame = 0;
						goto endHitBack;
					}
					this.HitBackFrame++;

					// ----

					double rate = (double)frm / HIT_BACK_FRAME_MAX;

					SPEED = 0.0;
					xBuru = (1.0 - rate) * 30.0 * DDUtils.Random.GetReal2();
					yBuru = (1.0 - rate) * 30.0 * DDUtils.Random.GetReal2();
				}
			endHitBack:

				switch (frame / 60 % 4)
				{
					case 0: this.X += SPEED; break;
					case 1: this.Y += SPEED; break;
					case 2: this.X -= SPEED; break;
					case 3: this.Y -= SPEED; break;

					default:
						throw null; // never
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 100.0))
				{
					if (1 <= this.HitBackFrame)
						DDDraw.SetBright(1.0, 0.8, 1.0);
					else
						DDDraw.SetBright(1.0, 0.5, 0.0);

					DDDraw.DrawBegin(
						Ground.I.Picture.WhiteBox,
						this.X - DDGround.Camera.X + xBuru,
						this.Y - DDGround.Camera.Y + yBuru
						);
					DDDraw.DrawSetSize(100.0, 100.0);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					DDPrint.SetBorder(new I3Color(128, 64, 0));
					DDPrint.SetDebug(
						(int)this.X - DDGround.Camera.X - 46,
						(int)this.Y - DDGround.Camera.Y - 46,
						20
						);
					DDPrint.PrintLine("敵(仮)");
					DDPrint.PrintLine("HP:" + this.HP);
					DDPrint.Reset();

					//this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 50.0);
					this.Crash = DDCrashUtils.Rect(D4Rect.XYWH(this.X, this.Y, 100.0, 100.0));
				}
				yield return true;
			}
		}

		private IEnumerable<bool> E_DummyTask()
		{
			for (; ; )
			{
				if (this.DeadFlag)
				{
					break;
				}

				// noop

				yield return true;
			}
		}

		protected override void P_Damaged(Shot shot, int damagePoint)
		{
			// 難読化のため #if の入れ子ができない。
#if true
			// 後退は不要
#elif true
			// 被弾した武器の進行方向へ後退する。
			{
				D2Point speed = Common.GetSpeed(shot.Direction, 10.0);

				this.X += speed.X;
				this.Y += speed.Y;
			}
#else
			// 被弾した武器の進行方向へ後退する。
			{
				const double SPAN = 10.0;
				const double NANAME_SPAN = 7.0;

				switch (shot.Direction)
				{
					case 4: this.X -= SPAN; break;
					case 6: this.X += SPAN; break;
					case 8: this.Y -= SPAN; break;
					case 2: this.Y += SPAN; break;

					case 1:
						this.X -= NANAME_SPAN;
						this.Y += NANAME_SPAN;
						break;

					case 3:
						this.X += NANAME_SPAN;
						this.Y += NANAME_SPAN;
						break;

					case 7:
						this.X -= NANAME_SPAN;
						this.Y -= NANAME_SPAN;
						break;

					case 9:
						this.X += NANAME_SPAN;
						this.Y -= NANAME_SPAN;
						break;

					default:
						throw null; // never
				}
			}
#endif

			this.HitBackFrame = 1;
			EnemyCommon.Damaged(this, shot, damagePoint);
		}
	}
}
