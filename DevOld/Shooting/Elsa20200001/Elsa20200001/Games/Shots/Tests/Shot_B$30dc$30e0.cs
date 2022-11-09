using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;

namespace Charlotte.Games.Shots.Tests
{
	public class Shot_Bボム : Shot
	{
		public Shot_Bボム()
			: base(0.0, 0.0, SCommon.IMAX / 2, true, Shot.Kind_e.ボム)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			foreach (DDScene scene in DDSceneUtils.Create(90))
			{
				無敵の通常敵を全消し();

				DDGround.EL.Add(() =>
				{
					DDCurtain.DrawCurtain(0.5 * (1.0 - scene.Rate));
					return false;
				});

				this.Crash = DDCrashUtils.Rect(new D4Rect(0, 0, GameConsts.FIELD_W, GameConsts.FIELD_H));

				yield return true;
			}
		}

		private static void 無敵の通常敵を全消し()
		{
			foreach (Enemy enemy in Game.I.Enemies.Iterate())
			{
				if (
					enemy.Kind == Enemy.Kind_e.通常敵 &&
					enemy.HP == 0 // ? 無敵
					)
					enemy.DeadFlag = true;
			}
		}
	}
}
