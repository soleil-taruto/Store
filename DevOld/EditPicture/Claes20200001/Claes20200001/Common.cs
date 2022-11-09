using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		public static bool IsSame(I3Color a, I3Color b)
		{
			return
				a.R == b.R &&
				a.G == b.G &&
				a.B == b.B;
		}

		public static bool IsSame(I4Color a, I4Color b)
		{
			return
				a.R == b.R &&
				a.G == b.G &&
				a.B == b.B &&
				a.A == b.A;
		}
	}
}
