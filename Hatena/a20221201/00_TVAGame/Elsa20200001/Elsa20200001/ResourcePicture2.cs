using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourcePicture2
	{
		public DDPicture[,] Dummy = DDDerivations.GetAnimation(Ground.I.Picture.Dummy, 0, 0, 25, 25, 2, 2); // ★サンプルとしてキープ

		#region Players

		public const int Player_e_Length = 50; // Player_e の項目数と一致すること

		public static string[] Player_e_Names = new string[] // 並び方は Player_e と一致すること
		{
			"Alice",
			"高麗野あうん",
			"赤蛮奇_体",
			"Cirno",
			"戎珱花",
			"易者",
			"依神女苑",
			"今泉影狼",
			"今泉影狼_狼",
			"埴安神袿姫",
			"秦こころ",
			"秦こころ_面01",
			"秦こころ_面02",
			"秦こころ_面03",
			"秦こころ_面04",
			"秦こころ_面05",
			"秦こころ_面06",
			"秦こころ_面07",
			"秦こころ_面08",
			"秦こころ_面09",
			"秦こころ_面10",
			"庭渡久侘歌",
			"ラルバ",
			"丁礼田舞",
			"杖刀偶磨弓",
			"矢田寺成美",
			"矢田寺成美_地蔵",
			"矢田寺成美_NoHat",
			"坂田ネムノ",
			"摩多羅隠岐奈",
			"摩多羅隠岐奈_Chair",
			"面01",
			"面02",
			"面03",
			"面04",
			"面05",
			"面06",
			"面07",
			"面08",
			"面09",
			"面10",
			"驪駒早鬼",
			"爾子田里乃",
			"赤蛮奇_頭",
			"赤蛮奇",
			"依神紫苑_覚",
			"依神紫苑",
			"牛崎潤美",
			"わかさぎ姫",
			"吉弔八千慧",
		};

		public enum Player_e
		{
			Alice,
			高麗野あうん,
			赤蛮奇_体,
			Cirno,
			戎珱花,
			易者,
			依神女苑,
			今泉影狼,
			今泉影狼_狼,
			埴安神袿姫,
			秦こころ,
			秦こころ_面01,
			秦こころ_面02,
			秦こころ_面03,
			秦こころ_面04,
			秦こころ_面05,
			秦こころ_面06,
			秦こころ_面07,
			秦こころ_面08,
			秦こころ_面09,
			秦こころ_面10,
			庭渡久侘歌,
			ラルバ,
			丁礼田舞,
			杖刀偶磨弓,
			矢田寺成美,
			矢田寺成美_地蔵,
			矢田寺成美_NoHat,
			坂田ネムノ,
			摩多羅隠岐奈,
			摩多羅隠岐奈_Chair,
			面01,
			面02,
			面03,
			面04,
			面05,
			面06,
			面07,
			面08,
			面09,
			面10,
			驪駒早鬼,
			爾子田里乃,
			赤蛮奇_頭,
			赤蛮奇,
			依神紫苑_覚,
			依神紫苑,
			牛崎潤美,
			わかさぎ姫,
			吉弔八千慧,
		}

		private PlayerInfo[] Players = new PlayerInfo[] // 並び方は Player_e と一致すること
		{
			new PlayerInfo(Ground.I.Picture.Player_Alice),
			new PlayerInfo(Ground.I.Picture.Player_高麗野あうん),
			new PlayerInfo(Ground.I.Picture.Player_赤蛮奇_体),
			new PlayerInfo(Ground.I.Picture.Player_Cirno),
			new PlayerInfo(Ground.I.Picture.Player_戎珱花),
			new PlayerInfo(Ground.I.Picture.Player_易者),
			new PlayerInfo(Ground.I.Picture.Player_依神女苑),
			new PlayerInfo(Ground.I.Picture.Player_今泉影狼),
			new PlayerInfo(Ground.I.Picture.Player_今泉影狼_狼),
			new PlayerInfo(Ground.I.Picture.Player_埴安神袿姫),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面01),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面02),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面03),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面04),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面05),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面06),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面07),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面08),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面09),
			new PlayerInfo(Ground.I.Picture.Player_秦こころ_面10),
			new PlayerInfo(Ground.I.Picture.Player_庭渡久侘歌),
			new PlayerInfo(Ground.I.Picture.Player_ラルバ),
			new PlayerInfo(Ground.I.Picture.Player_丁礼田舞),
			new PlayerInfo(Ground.I.Picture.Player_杖刀偶磨弓),
			new PlayerInfo(Ground.I.Picture.Player_矢田寺成美),
			new PlayerInfo(Ground.I.Picture.Player_矢田寺成美_地蔵),
			new PlayerInfo(Ground.I.Picture.Player_矢田寺成美_NoHat),
			new PlayerInfo(Ground.I.Picture.Player_坂田ネムノ),
			new PlayerInfo(Ground.I.Picture.Player_摩多羅隠岐奈),
			new PlayerInfo(Ground.I.Picture.Player_摩多羅隠岐奈_Chair),
			new PlayerInfo(Ground.I.Picture.Player_面01),
			new PlayerInfo(Ground.I.Picture.Player_面02),
			new PlayerInfo(Ground.I.Picture.Player_面03),
			new PlayerInfo(Ground.I.Picture.Player_面04),
			new PlayerInfo(Ground.I.Picture.Player_面05),
			new PlayerInfo(Ground.I.Picture.Player_面06),
			new PlayerInfo(Ground.I.Picture.Player_面07),
			new PlayerInfo(Ground.I.Picture.Player_面08),
			new PlayerInfo(Ground.I.Picture.Player_面09),
			new PlayerInfo(Ground.I.Picture.Player_面10),
			new PlayerInfo(Ground.I.Picture.Player_驪駒早鬼),
			new PlayerInfo(Ground.I.Picture.Player_爾子田里乃),
			new PlayerInfo(Ground.I.Picture.Player_赤蛮奇_頭),
			new PlayerInfo(Ground.I.Picture.Player_赤蛮奇),
			new PlayerInfo(Ground.I.Picture.Player_依神紫苑_覚),
			new PlayerInfo(Ground.I.Picture.Player_依神紫苑),
			new PlayerInfo(Ground.I.Picture.Player_牛崎潤美),
			new PlayerInfo(Ground.I.Picture.Player_わかさぎ姫),
			new PlayerInfo(Ground.I.Picture.Player_吉弔八千慧),
		};

		public class PlayerInfo
		{
			private DDPicture Picture;
			private DDPicture[,] PictureTable;

			public PlayerInfo(DDPicture picture)
			{
				this.Picture = picture;
				this.PictureTable = DDDerivations.GetAnimation(picture, 0, 0, 24, 32);
			}

			public DDPicture GetPicture(int faceDirection, int koma)
			{
				int l;
				int t;

				switch (faceDirection)
				{
					case 2: l = 0; t = 0; break; // 下
					case 4: l = 0; t = 1; break; // 左
					case 6: l = 0; t = 2; break; // 右
					case 8: l = 0; t = 3; break; // 上

					case 1: l = 3; t = 0; break; // 左下
					case 3: l = 3; t = 1; break; // 右下
					case 7: l = 3; t = 2; break; // 左上
					case 9: l = 3; t = 3; break; // 右上

					default:
						throw null; // never
				}
				return this.PictureTable[l + koma, t];
			}
		}

		public PlayerInfo GetPlayer(Player_e index)
		{
			return this.Players[(int)index];
		}

		#endregion

		public DDPicture[,] Tile_A1 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A1, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_A2 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A2, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_A3 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A3, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_A4 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A4, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_A5 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A5, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_B = DDDerivations.GetAnimation(Ground.I.Picture.Tile_B, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_C = DDDerivations.GetAnimation(Ground.I.Picture.Tile_C, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_D = DDDerivations.GetAnimation(Ground.I.Picture.Tile_D, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);
		public DDPicture[,] Tile_E = DDDerivations.GetAnimation(Ground.I.Picture.Tile_E, 0, 0, GameConsts.TILE_W, GameConsts.TILE_H);

		public DDPicture[,] MiniTile_A1 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A1, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_A2 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A2, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_A3 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A3, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_A4 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A4, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_A5 = DDDerivations.GetAnimation(Ground.I.Picture.Tile_A5, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_B = DDDerivations.GetAnimation(Ground.I.Picture.Tile_B, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_C = DDDerivations.GetAnimation(Ground.I.Picture.Tile_C, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_D = DDDerivations.GetAnimation(Ground.I.Picture.Tile_D, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);
		public DDPicture[,] MiniTile_E = DDDerivations.GetAnimation(Ground.I.Picture.Tile_E, 0, 0, GameConsts.MINI_TILE_W, GameConsts.MINI_TILE_H);

		public DDPicture[] Enemy_神奈子 = DDDerivations.GetAnimation_YX(Ground.I.Picture.Enemy_神奈子, 0, 0, 250, 250).ToArray();

		public ResourcePicture2()
		{
			// none
		}
	}
}
