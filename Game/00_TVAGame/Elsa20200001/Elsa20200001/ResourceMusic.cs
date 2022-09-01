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

		public DDMusic Title = new DDMusic(@"dat\hmix\n51.mp3");

		public DDMusic Field_01 = new DDMusic(@"dat\hmix\m2.mp3");
		public DDMusic Field_02 = new DDMusic(@"dat\hmix\n19.mp3");

		public DDMusic 神さびた古戦場 = new DDMusic(@"dat\みるふぃ\nc200903.mp3");

		public ResourceMusic()
		{
			// none
		}
	}
}
