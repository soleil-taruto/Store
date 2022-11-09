using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Walls
{
	public class WallCommon
	{
		private static DDSubScreen _screen = null;

		private static DDSubScreen GetScreen()
		{
			if (_screen == null)
				_screen = new DDSubScreen(GameConsts.FIELD_W, GameConsts.FIELD_H);

			return _screen;
		}

		public static IEnumerable<bool> E_フェードイン(IEnumerable<bool> e_draw, int frameMax = 150)
		{
			Func<bool> a_draw = SCommon.Supplier(e_draw);

			foreach (DDScene scene in DDSceneUtils.Create(frameMax))
			{
				bool ret;

				using (GetScreen().Section())
				{
					ret = a_draw();
				}
				DDDraw.SetAlpha(scene.Rate);
				DDDraw.DrawSimple(GetScreen().ToPicture(), 0, 0);
				DDDraw.Reset();

				yield return ret;
			}
			for (; ; )
			{
				yield return a_draw();
			}
		}
	}
}
