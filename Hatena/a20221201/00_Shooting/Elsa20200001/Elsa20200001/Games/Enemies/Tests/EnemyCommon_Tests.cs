using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	public static class EnemyCommon_Tests
	{
		/// <summary>
		/// 敵の死亡時のイベントを追加する。
		/// </summary>
		/// <param name="enemy">敵</param>
		/// <param name="reaction">イベント</param>
		public static void AddKillEvent(Enemy enemy, Action<Enemy> reaction)
		{
			Game.I.Tasks.Add(SCommon.Supplier(E_KillMonitor(enemy, reaction)));
		}

		private static IEnumerable<bool> E_KillMonitor(Enemy enemy, Action<Enemy> reaction)
		{
			for (; ; )
			{
				if (enemy.DeadFlag) // ? 死亡した -> 待ち終了
					break;

				yield return true;
			}

			if (DDUtils.IsOutOfScreen(new D2Point(enemy.X, enemy.Y), 100.0)) // ? 画面外 -> 退場と見なし何もしない。
				yield break;

			reaction(enemy); // イベント実行
		}
	}
}
