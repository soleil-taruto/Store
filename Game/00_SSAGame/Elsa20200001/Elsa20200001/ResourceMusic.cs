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

		public DDMusic Title = new DDMusic(@"dat\ユーフルカ\Voyage_loop\Voyage_loop.ogg").SetLoopByStLength(655565, 4860197);

		public DDMusic Field_01 = new DDMusic(@"dat\ユーフルカ\The-sacred-place_loop\The-sacred-place_loop.ogg").SetLoopByStLength(800621, 4233349);

		public DDMusic 神さびた古戦場 = new DDMusic(@"dat\みるふぃ\nc200903.mp3");

		public ResourceMusic()
		{
			// none
		}
	}
}
