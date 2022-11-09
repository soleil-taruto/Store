using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Enemies.Tests;
using Charlotte.Games.Walls;
using Charlotte.Games.Walls.Tests;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_Test0004 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			DDRandom rand = new DDRandom(1);

			Ground.I.Music.Stage_01.Play();

			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_Test0003());

			foreach (var relay in Enumerable.Repeat(true, 100))
				yield return relay;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 1));
				foreach (var relay in Enumerable.Repeat(true, 120)) yield return relay;
			}

			Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 1));
			EnemyCommon_Tests.AddKillEvent(
				Game.I.Enemies[Game.I.Enemies.Count - 1],
				enemy => Game.I.Enemies.Add(new Enemy_TestItem(enemy.X, enemy.Y, Enemy_TestItem.効用_e.POWER_UP_WEAPON))
				);

			foreach (var relay in Enumerable.Repeat(true, 120))
				yield return relay;

			Game.I.Walls.Add(new Wall_Test0004());

			foreach (var relay in Enumerable.Repeat(true, 100))
				yield return relay;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 3));

				foreach (var relay in Enumerable.Repeat(true, 120))
					yield return relay;
			}
			for (int c = 0; c < 10; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, DDConsts.Screen_H / 2));

				foreach (var relay in Enumerable.Repeat(true, 30))
					yield return relay;
			}
			for (int c = 0; c < 10; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 1));
				Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 3));

				foreach (var relay in Enumerable.Repeat(true, 30))
					yield return relay;
			}
			for (int c = 0; c < 30; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, rand.GetInt(DDConsts.Screen_H)));

				foreach (var relay in Enumerable.Repeat(true, 10))
					yield return relay;
			}
			for (int c = 0; c < 60; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, rand.GetInt(DDConsts.Screen_H)));

				foreach (var relay in Enumerable.Repeat(true, 5))
					yield return relay;
			}
			for (int c = 0; c < 120; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, rand.GetInt(DDConsts.Screen_H)));

				foreach (var relay in Enumerable.Repeat(true, 2))
					yield return relay;
			}
			foreach (var relay in Enumerable.Repeat(true, 100))
				yield return relay;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 1));
				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 4 * 3));

				foreach (var relay in Enumerable.Repeat(true, 120))
					yield return relay;
			}
			foreach (var relay in Enumerable.Repeat(true, 200))
				yield return relay;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 6 * 1));

				foreach (var relay in Enumerable.Repeat(true, 30))
					yield return relay;

				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 6 * 3));

				foreach (var relay in Enumerable.Repeat(true, 30))
					yield return relay;

				Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, DDConsts.Screen_H / 6 * 5));

				foreach (var relay in Enumerable.Repeat(true, 60))
					yield return relay;
			}
			foreach (var relay in Enumerable.Repeat(true, 300))
				yield return relay;

			// ---- ここからボス ----

			Ground.I.Music.Boss_01.Play();

			Game.I.Enemies.Add(new Enemy_Testボス0001());

			for (; ; )
				yield return true; // 以降何もしない。
		}
	}
}
