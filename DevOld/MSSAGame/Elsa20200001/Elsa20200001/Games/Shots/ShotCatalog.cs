using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Shots
{
	public static class ShotCatalog
	{
		public enum 武器_e
		{
			NORMAL,
			FIRE_BALL,
			LASER,
			WAVE_BEAM,
		}

		public static string[] 武器_e_Names = new string[]
		{
			"NORMAL",
			"FIRE-BALL",
			"LASER",
			"WAVE-BEAM",
		};
	}
}
