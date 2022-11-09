using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceMusic
	{
		public DDMusic Muon = new DDMusic(@"dat\General\muon.wav"); // ★サンプルとしてキープ

		// 例：ループ有り
		//public DDMusic AAA = new DDMusic(@"dat\AAA.mp3").SetLoopByStLength(123456, 789123); // ★サンプルとしてキープ

		public DDMusic Title = new DDMusic(@"dat\hmix\n118.mp3");

		public DDMusic Stage_01 = new DDMusic(@"dat\hmix\n138.mp3");
		public DDMusic Stage_02 = new DDMusic(@"dat\hmix\n70.mp3");
		public DDMusic Stage_03 = new DDMusic(@"dat\hmix\n13.mp3");

		public DDMusic Boss_01 = new DDMusic(@"dat\ユーフルカ\Battle-Vampire_loop\Battle-Vampire_loop.ogg").SetLoopByStLength(241468, 4205876);
		public DDMusic Boss_02 = new DDMusic(@"dat\ユーフルカ\Battle-Conflict_loop\Battle-Conflict_loop.ogg").SetLoopByStLength(281888, 3704134);
		public DDMusic Boss_03 = new DDMusic(@"dat\ユーフルカ\Battle-rapier_loop\Battle-rapier_loop.ogg").SetLoopByStLength(422312, 2767055);

		public ResourceMusic()
		{
			// none
		}
	}
}
