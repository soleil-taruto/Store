using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceSE
	{
		public DDSE Dummy = new DDSE(@"dat\General\muon.wav");

		public DDSE Poka01 = new DDSE(@"dat\フリー素材\小森平\sfxrse\01pickup\poka01.mp3");
		public DDSE Poka02 = new DDSE(@"dat\フリー素材\小森平\sfxrse\01pickup\poka02.mp3");

		public ResourceSE()
		{
			//this.Dummy.Volume = 0.1; // 非推奨
		}
	}
}
