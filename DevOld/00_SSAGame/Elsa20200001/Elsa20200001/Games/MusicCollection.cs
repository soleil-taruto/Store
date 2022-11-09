using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class MusicCollection
	{
		public static DDMusic Get(string name)
		{
			if (name == GameConsts.MAPPRM_DEFAULT_VALUE)
				return Ground.I.Music.Muon;

			DDMusic music;

			switch (name)
			{
				case "Field_01": music = Ground.I.Music.Field_01; break;
				//case "Field_02": music = Ground.I.Music.Field_02; break;

				// 新しい曲をここへ追加..

				default:
					throw new DDError("name: " + name);
			}
			return music;
		}
	}
}
