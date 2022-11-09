using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

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

		public int MessageSpeed = GameConsts.MESSAGE_SPEED_DEF;
		public int MessageWindow_A_Pct = GameConsts.MESSAGE_WINDOW_A_PCT_DEF;
		public SaveDataSlot[] SaveDataSlots = Enumerable.Range(0, GameConsts.SAVE_DATA_SLOT_NUM).Select(v => new SaveDataSlot()).ToArray();
	}
}
