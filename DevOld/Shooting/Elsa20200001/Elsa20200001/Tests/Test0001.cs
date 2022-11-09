using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			for (; ; )
			{
				if (DDInput.A.GetInput() == 1)
				{
					DDGround.EL.Add(SCommon.Supplier(Effects.小爆発(300, DDConsts.Screen_H / 2)));
				}
				if (DDInput.B.GetInput() == 1)
				{
					DDGround.EL.Add(SCommon.Supplier(Effects.中爆発(500, DDConsts.Screen_H / 2)));
				}
				if (DDInput.C.GetInput() == 1)
				{
					DDGround.EL.Add(SCommon.Supplier(Effects.大爆発(700, DDConsts.Screen_H / 2)));
				}

				DDEngine.EachFrame();
			}
		}
	}
}
