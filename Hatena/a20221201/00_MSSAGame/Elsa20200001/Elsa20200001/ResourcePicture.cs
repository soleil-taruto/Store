using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourcePicture
	{
		public DDPicture Dummy = DDPictureLoaders.Standard(@"dat\General\Dummy.png");
		public DDPicture WhiteBox = DDPictureLoaders.Standard(@"dat\General\WhiteBox.png");
		public DDPicture WhiteCircle = DDPictureLoaders.Standard(@"dat\General\WhiteCircle.png");
		public DDPicture DummyScreen = DDPictureLoaders.Standard(@"dat\General\DummyScreen.png");

		// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

		public DDPicture Copyright = DDPictureLoaders.Standard(@"dat\Logo\Copyright.png");

		public DDPicture Player = DDPictureLoaders.BgTrans(@"dat\pata2-magic\free_pochitto.png"); // プレイヤー画像 大元

		// ---- プレイヤー画像 ここから

		public DDPicture[][] PlayerStands = new DDPicture[5][];
		public DDPicture[][] PlayerTalks = new DDPicture[5][];
		public DDPicture PlayerShagami;
		public DDPicture[] PlayerWalk = new DDPicture[2];
		public DDPicture[] PlayerDash = new DDPicture[2];
		public DDPicture[] PlayerStop = new DDPicture[2];
		public DDPicture[] PlayerJump = new DDPicture[3];
		public DDPicture PlayerAttack;
		public DDPicture PlayerAttackShagami;
		public DDPicture[] PlayerAttackWalk = new DDPicture[2];
		public DDPicture[] PlayerAttackDash = new DDPicture[2];
		public DDPicture PlayerAttackJump;
		public DDPicture[] PlayerDamage = new DDPicture[8];

		// ---- プレイヤー画像 ここまで

		public ResourcePicture()
		{
			// ---- プレイヤー画像 ここから

			for (int x = 0; x < 5; x++)
			{
				PlayerStands[x] = new DDPicture[2];
				PlayerTalks[x] = new DDPicture[2];

				for (int y = 0; y < 2; y++)
					for (int k = 0; k < 2; k++)
						new[] { PlayerStands, PlayerTalks }[y][x][k] = DDDerivations.GetPicture(Player, x * 208 + k * 96, 16 + y * 144, 94, 112);
			}
			PlayerShagami = DDDerivations.GetPicture(Player, 0, 304, 94, 112);

			for (int x = 0; x < 3; x++)
				for (int k = 0; k < 2; k++)
					new[] { PlayerWalk, PlayerDash, PlayerStop }[x][k] = DDDerivations.GetPicture(Player, 112 + x * 208 + k * 96, 304, 94, 112);

			for (int x = 0; x < 3; x++)
				PlayerJump[x] = DDDerivations.GetPicture(Player, 736 + x * 112, 304, 94, 112);

			{
				List<DDPicture> buff = new List<DDPicture>();

				for (int x = 0; x < 2; x++)
					buff.Add(DDDerivations.GetPicture(Player, x * 112, 448, 94, 112));

				{
					int c = 0;

					PlayerAttack = buff[c++];
					PlayerAttackShagami = buff[c++];
				}
			}

			for (int x = 0; x < 2; x++)
				for (int k = 0; k < 2; k++)
					new[] { PlayerAttackWalk, PlayerAttackDash }[x][k] = DDDerivations.GetPicture(Player, 224 + x * 208 + k * 96, 448, 94, 112);

			PlayerAttackJump = DDDerivations.GetPicture(Player, 640, 448, 94, 112);

			for (int x = 0; x < 8; x++)
				PlayerDamage[x] = DDDerivations.GetPicture(Player, 0 + x * 112, 592, 94, 112);

			// ---- プレイヤー画像 ここまで
		}

		public DDPicture Tile_None = DDPictureLoaders.Standard(@"dat\Tile\Tile_None.png");
		public DDPicture Tile_B0001 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0001.png");
		public DDPicture Tile_B0002 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0002.png");
		public DDPicture Tile_B0003 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0003.png");
		public DDPicture Tile_B0004 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0004.png");

		public DDPicture Wall_B0001 = DDPictureLoaders.Standard(@"dat\素材Good\Wall_B0001.png");
		public DDPicture Wall_B0002 = DDPictureLoaders.Standard(@"dat\素材Good\Wall_B0002.png");
		public DDPicture Wall_B0003 = DDPictureLoaders.Standard(@"dat\素材Good\Wall_B0003.png");
		public DDPicture Wall_B0004 = DDPictureLoaders.Standard(@"dat\素材Good\Wall_B0004.jpg");
		public DDPicture Wall_B0005 = DDPictureLoaders.Standard(@"dat\素材Good\Wall_B0005.png");
		public DDPicture Wall_B0006 = DDPictureLoaders.Standard(@"dat\素材Good\Wall_B0006.png");

		public DDPicture Enemy_B0001_01 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_01.png");
		public DDPicture Enemy_B0001_02 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_02.png");
		public DDPicture Enemy_B0001_03 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_03.png");
		public DDPicture Enemy_B0001_04 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_04.png");

		public DDPicture Enemy_B0002_01 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0002_01.png");
		public DDPicture Enemy_B0002_02 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0002_02.png");

		public DDPicture Enemy_TrumpBack = DDPictureLoaders.Standard(@"dat\Picture\TrumpBack.png");
		public DDPicture Enemy_TrumpFrame = DDPictureLoaders.Standard(@"dat\Picture\TrumpFrame.png");
		public DDPicture Enemy_TrumpS01 = DDPictureLoaders.Standard(@"dat\Picture\TrumpS01.png");

		//public DDPicture Enemy_神奈子 = DDPictureLoaders.Reduct(@"dat\きつね仮\yukkuri-kanako.png", 4); // resize 4000x4000 -> 1000x1000
		public DDPicture Enemy_神奈子 = DDPictureLoaders.Standard(@"dat\きつね仮\yukkuri-kanako.png"); // use 1000x1000 resized png

		public DDPicture Title = DDPictureLoaders.Standard(@"dat\素材Good\Title.png");
		public DDPicture EnemyKilled = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-btleffect048.png");
		public DDPicture FireBall = DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-btleffect120g.png");

		// ノベルパート用システム画像
		public DDPicture MessageFrame_Message = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\01 message\message.png");
		public DDPicture MessageFrame_Button = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\02 button\button.png");
		public DDPicture MessageFrame_Button2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\02 button\button2.png");
		public DDPicture MessageFrame_Button3 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\02 button\button3.png");
		public DDPicture MessageFrame_Auto = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\auto.png");
		public DDPicture MessageFrame_Auto2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\auto2.png");
		public DDPicture MessageFrame_Load = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\load.png");
		public DDPicture MessageFrame_Load2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\load2.png");
		public DDPicture MessageFrame_Log = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\log.png");
		public DDPicture MessageFrame_Log2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\log2.png");
		public DDPicture MessageFrame_Menu = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\menu.png");
		public DDPicture MessageFrame_Menu2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\menu2.png");
		public DDPicture MessageFrame_Save = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\save.png");
		public DDPicture MessageFrame_Save2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\save2.png");
		public DDPicture MessageFrame_Skip = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\skip.png");
		public DDPicture MessageFrame_Skip2 = DDPictureLoaders.Standard(@"dat\Novel\空想曲線\Messageframe_29\material\03 system_button\skip2.png");
	}
}
