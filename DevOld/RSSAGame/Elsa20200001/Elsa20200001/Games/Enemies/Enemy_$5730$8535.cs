using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 地蔵
	/// ただの背景
	/// </summary>
	public class Enemy_地蔵 : Enemy
	{
		public Enemy_地蔵(double x, double y)
			: base(x, y, 0, 0, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 150.0))
				{
					Game.I.EL_AfterDrawMap.Add(() =>
					{
						DDDraw.DrawCenter(Ground.I.Picture.Stage01_Bg_Item03, this.X - DDGround.ICamera.X, this.Y - 48 - DDGround.ICamera.Y);
						return false;
					});

					// 当たり判定無し
				}
				yield return true;
			}
		}
	}
}
