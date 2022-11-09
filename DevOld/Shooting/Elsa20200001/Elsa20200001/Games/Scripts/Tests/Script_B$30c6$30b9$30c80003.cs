using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Enemies.Tests;
using Charlotte.Games.Walls;
using Charlotte.Games.Walls.Tests;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_Bテスト0003 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());

			Ground.I.Music.Stage_01.Play();

			for (; ; )
			{
				Game.I.Walls.Add(new Wall_B0003());

				Game.I.Enemies.Add(new Enemy_B0002(GameConsts.FIELD_W + 50.0, GameConsts.FIELD_H / 2.0));

				for (int c = 0; c < 500; c++)
					yield return true;

				Game.I.Walls.Add(new Wall_B0004());

				Game.I.Enemies.Add(new Enemy_B0001(GameConsts.FIELD_W + 50.0, GameConsts.FIELD_H / 2.0));

				for (int c = 0; c < 500; c++)
					yield return true;

				Ground.I.Music.Boss_01.Play();

				Game.I.Enemies.Add(new Enemy_Bボス0001());

				for (; ; )
					yield return true; // 以降何もしない。
			}
		}
	}
}
