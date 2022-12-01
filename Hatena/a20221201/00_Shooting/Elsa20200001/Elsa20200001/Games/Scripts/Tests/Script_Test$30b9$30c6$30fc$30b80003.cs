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
using Charlotte.Novels;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_Testステージ0003 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			DDRandom rand = new DDRandom(3);
			DDRandom rand_Sub = new DDRandom(103);

			Ground.I.Music.Stage_03.Play();
			Game.I.Walls.Add(new Wall_Test0004());

			foreach (var relay in Enumerable.Repeat(true, 100))
				yield return relay;

			foreach (DDScene scene in DDSceneUtils.Create((1 * 60 + 35) * 60))
			{
				DDGround.EL.Add(() =>
				{
					DDPrint.SetDebug(DDConsts.Screen_W - 180, 0);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.Print(scene.Numer + " / " + scene.Denom + " = " + scene.Rate.ToString("F3"));
					DDPrint.Reset();

					return false;
				});

				if (rand.GetReal1() < scene.Rate * 0.1)
				{
					if (rand.GetReal1() < 0.1)
					{
						Enemy_TestItem.効用_e 効用;

						if (rand_Sub.GetReal1() < 0.1)
							効用 = Enemy_TestItem.効用_e.ZANKI_UP;
						else if (rand_Sub.GetReal1() < 0.2)
							効用 = Enemy_TestItem.効用_e.BOMB_ADD;
						else
							効用 = Enemy_TestItem.効用_e.POWER_UP_WEAPON;

						Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, rand.GetReal1() * DDConsts.Screen_H));
						EnemyCommon_Tests.AddKillEvent(
							Game.I.Enemies[Game.I.Enemies.Count - 1],
							enemy => Game.I.Enemies.Add(new Enemy_TestItem(enemy.X, enemy.Y, 効用))
							);
					}
					else if (rand.GetReal1() < 0.3)
					{
						Game.I.Enemies.Add(new Enemy_Test0002(DDConsts.Screen_W + 50, rand.GetReal1() * DDConsts.Screen_H));
					}
					else
					{
						Game.I.Enemies.Add(new Enemy_Test0001(DDConsts.Screen_W + 50, rand.GetReal1() * DDConsts.Screen_H));
					}
				}
				yield return true;
			}

			Game.I.システム的な敵クリア();

			foreach (var relay in Enumerable.Repeat(true, 120))
				yield return relay;

			Ground.I.Music.Boss_03.Play();
			Game.I.Walls.Add(new Wall_Test0003());

			foreach (var relay in Enumerable.Repeat(true, 120))
				yield return relay;

			{
				Enemy boss = new Enemy_Testボス0003();

				Game.I.Enemies.Add(boss);

				while (!boss.DeadFlag)
				{
					DDGround.EL.Add(() =>
					{
						DDPrint.SetDebug(DDConsts.Screen_W - 140, 0);
						DDPrint.SetBorder(new I3Color(0, 0, 0));
						DDPrint.Print("BOSS_HP = " + boss.HP);
						DDPrint.Reset();

						return false;
					});

					yield return true;
				}
			}

			foreach (var relay in Enumerable.Repeat(true, 120))
				yield return relay;

			DDMusicUtils.Fadeout();

			foreach (var relay in Enumerable.Repeat(true, 120))
				yield return relay;

			// ここは最終ステージ

			DDCurtain.SetCurtain(30, -1.0);

			foreach (var relay in Enumerable.Repeat(true, 40))
				yield return relay;

			Game.I.EndReason = Game.EndReason_e.AllStageCleared;
		}
	}
}
