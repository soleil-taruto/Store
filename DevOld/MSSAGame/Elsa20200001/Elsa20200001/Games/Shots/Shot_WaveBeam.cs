using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_WaveBeam : Shot
	{
		public Shot_WaveBeam(double x, double y, bool facingLeft)
			: base(x, y, facingLeft, 5, true, false) // 壁を貫通する。
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			double baseRad = DDUtils.Random.GetInt(2) == 0 ? 0.0 : Math.PI;

			for (int frame = 0; ; frame++)
			{
				this.X += 10.0 * (this.FacingLeft ? -1 : 1);

				double x = this.X;
				double y = this.Y + Math.Sin(baseRad + frame / 2.0) * 50.0;

				DDDraw.DrawBegin(Ground.I.Picture2.FireBall[14 + frame % 7], x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawEnd();

				this.Crash = DDCrashUtils.Circle(new D2Point(x, y), 8.0);

				yield return !DDUtils.IsOutOfCamera(new D2Point(x, y)); // カメラから出たら消滅する。
			}
		}

		protected override void Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.FireBall爆発(this.X, this.Y)));
		}
	}
}
