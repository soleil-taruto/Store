using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 即死トラップ
	/// いわゆる「針」
	/// </summary>
	public class Enemy_即死トラップThe針 : Enemy
	{
		private DDPicture Picture;

		public Enemy_即死トラップThe針(double x, double y, DDPicture picture)
			: base(x, y, 0, SCommon.IMAX / 2, false)
		{
			this.Picture = picture;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				DDDraw.DrawCenter(this.Picture, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 16.0);

				yield return true;
			}
		}
	}
}
