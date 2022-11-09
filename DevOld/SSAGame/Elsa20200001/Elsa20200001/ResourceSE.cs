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

		public DDSE[] テスト用s = new DDSE[]
		{
			new DDSE(@"dat\小森平\sfxrse\01pickup\coin01.mp3"),
			new DDSE(@"dat\小森平\sfxrse\01pickup\coin02.mp3"),
			new DDSE(@"dat\小森平\sfxrse\01pickup\coin04.mp3"),
		};

		public DDSE Coin01 = new DDSE(@"dat\小森平\sfxrse\01pickup\coin01.mp3"); // test test test test test

		public ResourceSE()
		{
			//this.Dummy.Volume = 0.1; // 非推奨
		}
	}
}
