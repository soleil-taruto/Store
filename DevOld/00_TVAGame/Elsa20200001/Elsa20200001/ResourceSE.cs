using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceSE
	{
		//public DDSE Muon = new DDSE(@"dat\General\muon.wav"); // ★サンプルとしてキープ

		public DDSE[] テスト用s = new DDSE[]
		{
			new DDSE(@"dat\小森平\coin01.mp3"),
			new DDSE(@"dat\小森平\coin02.mp3"),
			new DDSE(@"dat\小森平\coin04.mp3"),
		};

		public DDSE PlayerDamaged = new DDSE(@"dat\小森平\damage5.mp3");

		public DDSE EnemyDamaged = new DDSE(@"dat\小森平\hit04.mp3");
		public DDSE EnemyKilled = new DDSE(@"dat\小森平\explosion06.mp3");

		public ResourceSE()
		{
			// none
		}
	}
}
