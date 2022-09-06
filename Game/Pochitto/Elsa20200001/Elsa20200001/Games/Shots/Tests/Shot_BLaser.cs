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
	/// 壁・敵_両方貫通
	/// </summary>
	public class Shot_BLaser : Shot
	{
		private bool FacingLeft;

		public Shot_BLaser(double x, double y, bool facingLeft)
			: base(x, y, 1, true)
		{
			this.FacingLeft = facingLeft;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			double x1;
			double x2;

			if (this.FacingLeft)
			{
				x1 = DDGround.Camera.X;
				x2 = this.X;
			}
			else
			{
				x1 = this.X;
				x2 = DDGround.Camera.X + DDConsts.Screen_W;
			}
			double y1 = this.Y - 2.0;
			double y2 = this.Y + 2.0;

			if (x2 < x1 + 2.0)
				goto endFunc;

			DDDraw.SetAlpha(0.2 + 0.1 * Math.Sin(DDEngine.ProcFrame / 2.0));
			DDDraw.SetBright(0.0, 1.0, 1.0);
			DDDraw.DrawRect_LTRB(
				Ground.I.Picture.WhiteBox,
				x1 - DDGround.Camera.X,
				y1 - DDGround.Camera.Y,
				x2 - DDGround.Camera.X,
				y2 - DDGround.Camera.Y
				);
			DDDraw.Reset();

			if (DDEngine.ProcFrame % 5 == 0) // 間隔適当
				this.Crash = DDCrashUtils.Rect(D4Rect.LTRB(x1, y1, x2, y2));

			// 1回だけ true を返して1フレームだけ生存する必要がある。
			// -- 当たり判定時に、自弾リストに this が無いと this.Crash は参照されない。
			//
			yield return true; // 1フレームで消滅する。

		endFunc:
			;

			//yield break;
		}
	}
}
