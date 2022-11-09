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
	public class Script_Bステージ0003 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			return ScriptCommon.Wrapper(SCommon.Supplier(this.E_EachFrame2()));
		}

		private IEnumerable<int> E_EachFrame2()
		{
			DDRandom rand = new DDRandom(3);
			DDRandom rand_Sub = new DDRandom(103);

			Ground.I.Music.Stage_03.Play();
			Game.I.Walls.Add(new Wall_B0004());
			yield return 100;

			foreach (DDScene scene in DDSceneUtils.Create((1 * 60 + 35) * 60))
			{
				DDGround.EL.Add(() =>
				{
					DDPrint.SetDebug(GameConsts.FIELD_W - 180, 0);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.Print(scene.Numer + " / " + scene.Denom + " = " + scene.Rate.ToString("F3"));
					DDPrint.Reset();

					return false;
				});

				if (rand.Real() < scene.Rate * 0.1)
				{
					if (rand.Real() < 0.1)
					{
						Enemy_BItem.効用_e 効用;

						if (rand_Sub.Real() < 0.1)
							効用 = Enemy_BItem.効用_e.ZANKI_UP;
						else if (rand_Sub.Real() < 0.2)
							効用 = Enemy_BItem.効用_e.BOMB_ADD;
						else
							効用 = Enemy_BItem.効用_e.POWER_UP_WEAPON;

						Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, rand.Real() * GameConsts.FIELD_H).AddKilled(enemy =>
						{
							Game.I.Enemies.Add(new Enemy_BItem(enemy.X, enemy.Y, 効用));
						}
						));
					}
					else if (rand.Real() < 0.3)
					{
						Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, rand.Real() * GameConsts.FIELD_H));
					}
					else
					{
						Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, rand.Real() * GameConsts.FIELD_H));
					}
				}
				yield return 1;
			}

			Game.I.システム的な敵クリア();
			yield return 120;
			Ground.I.Music.Boss_03.Play();
			Game.I.Walls.Add(new Wall_B0003());
			yield return 120;

			{
				Enemy boss = new Enemy_Bボス0003();

				Game.I.Enemies.Add(boss);

				while (!boss.DeadFlag)
				{
					DDGround.EL.Add(() =>
					{
						DDPrint.SetDebug(GameConsts.FIELD_W - 140, 0);
						DDPrint.SetBorder(new I3Color(0, 0, 0));
						DDPrint.Print("BOSS_HP = " + boss.HP);
						DDPrint.Reset();

						return false;
					});

					yield return 1;
				}
			}

			yield return 120;
			DDMusicUtils.Fade();
			yield return 120;

			// ここは最終ステージ

			DDCurtain.SetCurtain(30, -1.0);
			yield return 40;

			yield return -1; // Script 終了するために -1 を返す。
		}
	}
}
