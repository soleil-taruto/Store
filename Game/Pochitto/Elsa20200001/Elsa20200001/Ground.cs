using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
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

		public class P_SaveDataSlot
		{
			public string TimeStamp = "0001/01/01 (月) 00:00:00";
			public string Description = "none";
			public string MapName = "Tests/t0001";
			public GameStatus GameStatus = null; // null == セーブデータ無し

			#region Serialize / Deserialize

			public string Serialize()
			{
				List<string> dest = new List<string>();

				// ★★★ シリアライズ_ここから ★★★

				dest.Add(this.TimeStamp);
				dest.Add(this.Description);
				dest.Add(this.MapName);
				dest.Add("" + (this.GameStatus != null ? 1 : 0));

				if (GameStatus != null)
					dest.Add(this.GameStatus.Serialize());

				// ★★★ シリアライズ_ここまで ★★★

				return SCommon.Serializer.I.Join(dest.ToArray());
			}

			public void Deserialize(string value)
			{
				string[] lines = SCommon.Serializer.I.Split(value);
				int c = 0;

				// ★★★ デシリアライズ_ここから ★★★

				this.TimeStamp = lines[c++];
				this.Description = lines[c++];
				this.MapName = lines[c++];

				if (int.Parse(lines[c++]) != 0)
					this.GameStatus = GameStatus.Deserialize(lines[c++]);
				else
					this.GameStatus = null;

				// ★★★ デシリアライズ_ここまで ★★★
			}

			#endregion
		}

		public P_SaveDataSlot[] SaveDataSlots = Enumerable.Range(0, GameConsts.SAVE_DATA_SLOT_NUM).Select(v => new P_SaveDataSlot()).ToArray();
	}
}
