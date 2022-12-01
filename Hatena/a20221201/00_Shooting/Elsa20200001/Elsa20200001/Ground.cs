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

		/// <summary>
		/// コンテニュー可能な最大ステージ番号
		/// 値：
		/// -- 1 == 1 ～ 1 ステージからコンテニュー可能
		/// -- 2 == 1 ～ 2 ステージからコンテニュー可能
		/// -- 3 == 1 ～ 3 ステージからコンテニュー可能
		/// -- ...
		/// </summary>
		public int CanContinueStageNumber = 1;
	}
}
