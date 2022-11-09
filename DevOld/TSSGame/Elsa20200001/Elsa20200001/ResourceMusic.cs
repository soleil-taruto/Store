using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceMusic
	{
		public DDMusic Dummy = new DDMusic(@"dat\General\muon.wav"); // ★サンプルとしてキープ

		// 例：ループ有り
		//public DDMusic AAA = new DDMusic(@"dat\AAA.mp3").SetLoopByStLength(123456, 789123); // ★サンプルとしてキープ

		public DDMusic Title = new DDMusic(@"dat\フリー素材\ajapa\TheLabyrinth.mp3");

		public ResourceMusic()
		{
			//this.Dummy.Volume = 0.1; // 非推奨 // ★サンプルとしてキープ
		}
	}
}
