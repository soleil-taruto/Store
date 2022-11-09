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
		/// <summary>
		/// 汎用・被弾イベント
		/// </summary>
		/// <param name="enemy">被弾した敵</param>
		/// <param name="shot">「被弾した敵」に当たった自弾</param>
		public static void Damaged(Enemy enemy, Shot shot)
		{
			// noop ???
		}

		/// <summary>
		/// 汎用・消滅イベント
		/// </summary>
		/// <param name="enemy">消滅する敵</param>
		public static void Killed(Enemy enemy)
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.小爆発(enemy.X, enemy.Y)));
		}
	}
}
