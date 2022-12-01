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
	public class Script_Test0001 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Game.I.Walls.Add(new Wall_Dark());

			for (; ; )
			{
				Game.I.Walls.Add(new Wall_Test0001());

				foreach (DDScene scene in DDSceneUtils.Create(10))
				{
					Game.I.Enemies.Add(new Enemy_Test0001(
						DDConsts.Screen_W + 50.0,
						100.0 + scene.Rate * 200.0
						));

					for (int c = 0; c < 20; c++)
						yield return true;
				}

				for (int c = 0; c < 60; c++)
					yield return true;

				foreach (DDScene scene in DDSceneUtils.Create(10))
				{
					Game.I.Enemies.Add(new Enemy_Test0001(
						DDConsts.Screen_W + 50.0,
						DDConsts.Screen_H - 100.0 - scene.Rate * 200.0
						));

					for (int c = 0; c < 20; c++)
						yield return true;
				}

				for (int c = 0; c < 60; c++)
					yield return true;

				Game.I.Walls.Add(new Wall_Test0002());

				foreach (DDScene scene in DDSceneUtils.Create(20))
				{
					Game.I.Enemies.Add(new Enemy_Test0001(
						DDConsts.Screen_W + 50.0,
						DDConsts.Screen_H * DDUtils.Random.GetReal1()
						));

					for (int c = 0; c < 10; c++)
						yield return true;
				}

				for (int c = 0; c < 60; c++)
					yield return true;

				foreach (DDScene scene in DDSceneUtils.Create(20))
				{
					Game.I.Enemies.Add(new Enemy_Test0001(
						DDConsts.Screen_W + 50.0,
						DDConsts.Screen_H * DDUtils.Random.GetReal1()
						));

					for (int c = 0; c < 10; c++)
						yield return true;
				}

				for (int c = 0; c < 60; c++)
					yield return true;
			}
		}
	}
}
