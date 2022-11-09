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
	public class Script_Bテスト0004 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			return ScriptCommon.Wrapper(SCommon.Supplier(this.E_EachFrame2()));
		}

		private IEnumerable<int> E_EachFrame2()
		{
			DDRandom rand = new DDRandom(1);

			Ground.I.Music.Stage_01.Play();

			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_B0003());
			yield return 100;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 1));
				yield return 120;
			}
			Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 1)
				.AddKilled(enemy => Game.I.Enemies.Add(new Enemy_BItem(enemy.X, enemy.Y, Enemy_BItem.効用_e.POWER_UP_WEAPON)))
				);
			yield return 120;
			Game.I.Walls.Add(new Wall_B0004());
			yield return 100;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 3));
				yield return 120;
			}
			for (int c = 0; c < 10; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 2));
				yield return 30;
			}
			for (int c = 0; c < 10; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 1));
				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 3));
				yield return 30;
			}
			for (int c = 0; c < 30; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, rand.GetInt(GameConsts.FIELD_H)));
				yield return 10;
			}
			for (int c = 0; c < 60; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, rand.GetInt(GameConsts.FIELD_H)));
				yield return 5;
			}
			for (int c = 0; c < 120; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50, rand.GetInt(GameConsts.FIELD_H)));
				yield return 2;
			}
			yield return 100;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 1));
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 4 * 3));
				yield return 120;
			}
			yield return 200;

			for (int c = 0; c < 3; c++)
			{
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 6 * 1));
				yield return 30;
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 6 * 3));
				yield return 30;
				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50, GameConsts.FIELD_H / 6 * 5));
				yield return 60;
			}
			yield return 300;

			// ---- ここからボス ----

			Ground.I.Music.Boss_01.Play();

			Game.I.Enemies.Add(new Enemy_Bボス0001());

			for (; ; )
				yield return 1; // 以降何もしない。
		}
	}
}
