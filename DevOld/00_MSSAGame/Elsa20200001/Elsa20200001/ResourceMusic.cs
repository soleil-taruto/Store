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

		public DDMusic Title = new DDMusic(@"dat\ユーフルカ\Crazy-Halloween-Night_loop\Crazy-Halloween-Night_loop.ogg").SetLoopByStLength(184074, 4141574);

		public DDMusic Field_01 = new DDMusic(@"dat\ユーフルカ\Everlasting-Snow_loop\Everlasting-Snow_loop.ogg").SetLoopByStLength(551345, 4961249);
		public DDMusic Field_02 = new DDMusic(@"dat\ユーフルカ\Silent-Avalon_loop\Silent-Avalon_loop.ogg").SetLoopByStLength(875922, 7883346);

		public DDMusic 神さびた古戦場 = new DDMusic(@"dat\みるふぃ\nc200903.mp3");

		public ResourceMusic()
		{
			// none
		}
	}
}
