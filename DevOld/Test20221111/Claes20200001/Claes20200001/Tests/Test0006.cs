using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Tests
{
	public class Test0006
	{
		public void Test01()
		{
			long t = SCommon.TimeStampToSec.ToSec(19700101000000);
			t += ProcMain.GetPETimeDateStamp();
			t += 9 * 3600; // UTC -> JST
			long ts = SCommon.TimeStampToSec.ToTimeStamp(t);
			DateTime dt = SCommon.SimpleDateTime.FromTimeStamp(ts).ToDateTime();

			Console.WriteLine(dt);
		}
	}
}
