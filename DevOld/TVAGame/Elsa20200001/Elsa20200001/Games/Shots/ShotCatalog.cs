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
			WAVE,
			SPREAD,
			BOUNCE,
		}

		public static int 武器_e_Length = Enum.GetValues(typeof(武器_e)).Length;
	}
}
