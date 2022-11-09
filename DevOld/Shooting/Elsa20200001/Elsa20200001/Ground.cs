using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Novels;

namespace Charlotte
{
	public class Ground
	{
		public static Ground I;

		public ResourceMusic Music = new ResourceMusic();
		public ResourcePicture Picture = new ResourcePicture();
		public ResourcePicture2 Picture2;
		public ResourceSE SE = new ResourceSE();

		// DDSaveData.Save/Load でセーブ・ロードする情報はここに保持する。

		public int NovelMessageSpeed = NovelConsts.MESSAGE_SPEED_DEF;
	}
}
