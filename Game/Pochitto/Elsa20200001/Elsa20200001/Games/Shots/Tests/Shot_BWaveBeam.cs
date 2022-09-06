using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots.Tests
{
	/// <summary>
	/// 自弾_旧実装
	/// ★サンプルとしてキープ
	/// ----
	/// 壁を貫通する。
	/// </summary>
	public class Shot_BWaveBeam : Shot
	{
		private bool FacingLeft;

		public Shot_BWaveBeam(double x, double y, bool facingLeft)
			: base(x, y, 5, false)
		{
			this.FacingLeft = facingLeft;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double baseRad = DDUtils.Random.GetInt(2) == 0 ? 0.0 : Math.PI;

			for (int frame = 0; ; frame++)
			{
				this.X += 10.0 * (this.FacingLeft ? -1 : 1);

				double x = this.X;
				double y = this.Y + Math.Sin(baseRad + frame / 2.0) * 50.0;

				DDDraw.DrawBegin(Ground.I.Picture2.FireBall[14 + frame % 7], x - DDGround.Camera.X, y - DDGround.Camera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(x, y), 8.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(x, y)); // カメラから出たら消滅する。
			}
		}

		protected override void P_Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.BFireBall爆発(this.X, this.Y)));
		}
	}
}
