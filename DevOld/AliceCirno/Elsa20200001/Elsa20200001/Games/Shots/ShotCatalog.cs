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
			B_NORMAL,
			B_WAVE,
			B_SPREAD,
			B_BOUNCE,
			NORMAL,
		}

		public static string[] 武器_e_Names = new string[]
		{
			"テスト用 NORMAL",
			"テスト用 WAVE",
			"テスト用 SPREAD",
			"テスト用 BOUNCE",
			"NORMAL",
		};
	}
}
