using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Enemies.Bosses.ゆドレミーs
{
	public class Enemy_Boss_ゆドレミー : Enemy
	{
		public Enemy_Boss_ゆドレミー(double x, double y)
			: base(x, y, 0, 0, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// TODO

				yield return true;
			}
		}
	}
}
