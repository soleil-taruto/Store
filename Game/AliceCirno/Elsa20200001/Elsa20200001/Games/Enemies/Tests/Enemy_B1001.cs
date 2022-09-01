using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_敵
	/// </summary>
	public class Enemy_B1001 : Enemy
	{
		public Enemy_B1001(double x, double y)
			: base(x, y, 50, 1, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			D2Point center = new D2Point(this.X, this.Y);

			Game.I.Tasks.Add(SCommon.Supplier(this.E_AttackTask()));

			for (; ; )
			{
				double angle = center.X + center.Y + DDEngine.ProcFrame / 108.0;
				D2Point pt = DDUtils.AngleToPoint(angle, 25.0);

				this.X = center.X + pt.X;
				this.Y = center.Y + pt.Y;

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では描画しない。
				{
					DDDraw.SetBright(0.5, 1.0, 0.0);
					DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
					DDDraw.DrawSetSize(100.0, 100.0);
					DDDraw.DrawEnd();
					DDDraw.Reset();

					DDPrint.SetBorder(new I3Color(128, 64, 0));
					DDPrint.SetDebug(
						(int)this.X - DDGround.Camera.X - 46,
						(int)this.Y - DDGround.Camera.Y - 46,
						20
						);
					DDPrint.PrintLine("敵1001");
					DDPrint.PrintLine("HP:" + this.HP);
					DDPrint.Reset();

					this.Crash = DDCrashUtils.Rect(D4Rect.XYWH(this.X, this.Y, 100.0, 100.0));
				}

				yield return true;
			}
		}

		private IEnumerable<bool> E_AttackTask()
		{
			for (; ; )
			{
				foreach (int waitFrm in new int[] { 180, 60, 60 })
				{
					foreach (var relay in Enumerable.Repeat(true, waitFrm)) // 待ち
						yield return relay;

					if (this.DeadFlag) // この敵が死亡したら、即終了
						break;

					if (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では攻撃しない。
						continue;

					for (int c = 0; c < 4; c++)
					{
						D2Point speed = DDUtils.AngleToPoint(DDUtils.Random.GetReal1() * Math.PI * 2.0, 5.0);

						Game.I.Enemies.Add(new Enemy_B1002(this.X, this.Y, speed.X, speed.Y));
					}
				}
			}
		}
	}
}
