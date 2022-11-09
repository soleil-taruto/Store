using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies
{
	public static class EnemyCommon
	{
		public static void Damaged(Enemy enemy, Shot shot)
		{
			// none
		}

		public static void Killed(Enemy enemy)
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.B中爆発(enemy.X, enemy.Y)));
		}
	}
}
