using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourcePicture2
	{
		public DDPicture[,] Dummy = DDDerivations.GetAnimation(Ground.I.Picture.Dummy, 0, 0, 25, 25, 2, 2); // ★サンプルとしてキープ

		public DDPicture[] Enemy_神奈子 = DDDerivations.GetAnimation_YX(Ground.I.Picture.Enemy_神奈子, 0, 0, 250, 250).ToArray();

		public DDPicture[] EnemyKilled = DDDerivations.GetAnimation_YX(Ground.I.Picture.EnemyKilled, 0, 0, 240, 240).ToArray();
		public DDPicture[] FireBall = DDDerivations.GetAnimation_YX(Ground.I.Picture.FireBall, 0, 0, 240, 240).ToArray(); // 円になっているのは [14] から 7 個

		public ResourcePicture2()
		{
			// none
		}
	}
}
