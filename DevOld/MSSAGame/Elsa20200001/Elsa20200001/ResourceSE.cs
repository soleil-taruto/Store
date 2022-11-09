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

		public DDSE PlayerJump = new DDSE(@"dat\小森平\sfxrse\01pickup\coin01.mp3"); // 仮

		public DDSE EnemyDamaged = new DDSE(@"dat\小森平\sfxrse\05hit\hit04.mp3");
		public DDSE EnemyKilled = new DDSE(@"dat\小森平\sfxrse\03explosion\explosion06.mp3");

		public ResourceSE()
		{
			//this.Dummy.Volume = 0.1; // 非推奨
		}
	}
}
