using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_AirShooter : Shot
	{
		private int Level;

		/// <summary>
		/// 3発一緒に発射される。
		/// プレイヤーに近い方から this.Order == { 0, 1, 2 } とする。
		/// </summary>
		private int Order;

		public Shot_AirShooter(double x, double y, bool facingLeft, int order, int level)
			: base(x, y, facingLeft, LevelToAttackPoint(level), true, false)
		{
			this.Level = level;
			this.Order = order;
		}

		private static int LevelToAttackPoint(int level)
		{
			switch (level)
			{
				case 1: return 1;
				case 2: return 2;
				case 3: return 4;
				case 4: return 8;

				default:
					throw null; // never
			}
		}

		private static double LevelToScale(int level)
		{
			switch (level)
			{
				case 1: return 1.0;
				case 2: return 1.25;
				case 3: return 1.5;
				case 4: return 2.0;

				default:
					throw null; // never
			}
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double SCALE = LevelToScale(this.Level);
			double R = SCommon.ToInt(24.0 * SCALE);

			double yAdd = 0.0;

			// 初期位置調整
			{
				this.X += (36.0 + R * (1 + 2 * this.Order)) * (this.FacingLeft ? -1 : 1);
				//this.Y += 0.0;
			}

			for (; ; )
			{
				yAdd -= (0.2 + 0.05 * this.Order) * SCALE;

				this.X += (4.0 + 0.5 * this.Order) * SCALE * (this.FacingLeft ? -1 : 1);
				this.Y += yAdd;

				DDDraw.SetBright(new I3Color(0, 192, 192));
				DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawSetSize(R * 2, R * 2);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				DDPrint.SetDebug(
					(int)this.X - DDGround.ICamera.X - 12,
					(int)this.Y - DDGround.ICamera.Y - 8
					);
				DDPrint.SetBorder(new I3Color(0, 0, 0));
				DDPrint.Print("AS" + this.Level);
				DDPrint.Reset();

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);

				yield return !DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), R); // カメラから出たら消滅する。
			}
		}
	}
}
