using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_ボス敵
	/// </summary>
	public class Enemy_Bボス0003 : Enemy
	{
		public Enemy_Bボス0003()
			: base(GameConsts.FIELD_W + 96.0, GameConsts.FIELD_H / 2.0, 1500, Kind_e.ボス)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			Func<bool> f_移動 = SCommon.Supplier(this.移動());
			Func<bool> f_攻撃 = SCommon.Supplier(this.攻撃());

			for (; ; )
			{
				f_移動();
				f_攻撃();

				DDDraw.DrawCenter(Ground.I.Picture.Boss0003, this.X, this.Y);

				{
					const double WH = 192.0;
					const double CORNER_R = 30.0;

					this.Crash = DDCrashUtils.Multi(
						DDCrashUtils.Rect(new D4Rect(
							this.X - WH / 2.0 + CORNER_R,
							this.Y - WH / 2.0,
							WH - CORNER_R * 2.0,
							WH
							)),
						DDCrashUtils.Rect(new D4Rect(
							this.X - WH / 2.0,
							this.Y - WH / 2.0 + CORNER_R,
							WH,
							WH - CORNER_R * 2.0
							)),
						DDCrashUtils.Circle(new D2Point(this.X - (WH / 2.0 - CORNER_R), this.Y - (WH / 2.0 - CORNER_R)), CORNER_R),
						DDCrashUtils.Circle(new D2Point(this.X + (WH / 2.0 - CORNER_R), this.Y - (WH / 2.0 - CORNER_R)), CORNER_R),
						DDCrashUtils.Circle(new D2Point(this.X + (WH / 2.0 - CORNER_R), this.Y + (WH / 2.0 - CORNER_R)), CORNER_R),
						DDCrashUtils.Circle(new D2Point(this.X - (WH / 2.0 - CORNER_R), this.Y + (WH / 2.0 - CORNER_R)), CORNER_R)
						);
				}

				yield return true;
			}
		}

		private IEnumerable<bool> 移動()
		{
			for (int c = 0; c < 40; c++)
			{
				this.X -= 5.0;
				yield return true;
			}
			for (; ; )
			{
				for (int c = 0; c < 30; c++)
				{
					this.Y += 3.0;
					yield return true;
				}
				for (int c = 0; c < 40; c++)
				{
					this.X -= 3.0;
					yield return true;
				}
				for (int c = 0; c < 60; c++)
				{
					this.Y -= 3.0;
					yield return true;
				}
				for (int c = 0; c < 40; c++)
				{
					this.X += 3.0;
					yield return true;
				}
				for (int c = 0; c < 30; c++)
				{
					this.Y += 3.0;
					yield return true;
				}
			}
		}

		private IEnumerable<bool> 攻撃()
		{
			const double R = 50.0;

			for (; ; )
			{
				for (int c = 0; c < 5; c++)
					yield return true;

				Game.I.Enemies.Add(new Enemy_BTama0001(this.X - R, this.Y - R));

				for (int c = 0; c < 5; c++)
					yield return true;

				Game.I.Enemies.Add(new Enemy_BTama0001(this.X - R, this.Y + R));

				for (int c = 0; c < 5; c++)
					yield return true;

				Game.I.Enemies.Add(new Enemy_BTama0001(this.X + R, this.Y - R));

				for (int c = 0; c < 5; c++)
					yield return true;

				Game.I.Enemies.Add(new Enemy_BTama0001(this.X + R, this.Y + R));
			}
		}
	}
}
