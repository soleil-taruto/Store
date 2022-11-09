using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public static class EnemyCommon
	{
		/// <summary>
		/// 汎用・被弾イベント
		/// </summary>
		/// <param name="enemy"></param>
		public static void Damaged(Enemy enemy)
		{
			// TODO: SE
		}

		/// <summary>
		/// 汎用・死亡イベント
		/// </summary>
		public static void Killed(Enemy enemy)
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.中爆発(enemy.X, enemy.Y)));
		}
	}
}
