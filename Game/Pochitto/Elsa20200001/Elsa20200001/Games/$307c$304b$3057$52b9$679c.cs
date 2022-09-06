using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public static class ぼかし効果
	{
		public static void Perform(double rate)
		{
			DX.GraphFilter(
				DDSubScreenUtils.CurrDrawScreen.GetHandle(),
				DX.DX_GRAPH_FILTER_GAUSS,
				16,
				SCommon.ToInt(5000.0 * rate)
				);
		}
	}
}
