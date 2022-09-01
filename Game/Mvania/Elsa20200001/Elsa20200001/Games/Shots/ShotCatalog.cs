using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public static class ShotCatalog
	{
		public enum 武器_e
		{
			B_NORMAL,
			B_FIRE_BALL,
			B_LASER,
			B_WAVE_BEAM,
			NORMAL,
		}

		public static string[] 武器_e_Names = new string[]
		{
			"テスト用 NORMAL",
			"テスト用 FIRE-BALL",
			"テスト用 LASER",
			"テスト用 WAVE-BEAM",
			"NORMAL",
		};
	}
}
