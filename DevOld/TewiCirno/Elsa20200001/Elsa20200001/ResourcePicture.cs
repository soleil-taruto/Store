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

		public DDPicture Title = DDPictureLoaders.Standard(@"dat\run\22350006_big_p12.jpg");
		public DDPicture[] TitleMenuItems = new DDPicture[]
		{
			DDPictureLoaders.Standard(@"dat\Title\ゲームスタート.png"),
			DDPictureLoaders.Standard(@"dat\Title\コンテニュー.png"),
			DDPictureLoaders.Standard(@"dat\Title\おまけ.png"),
			DDPictureLoaders.Standard(@"dat\Title\設定.png"),
			DDPictureLoaders.Standard(@"dat\Title\終了.png"),
		};

		public DDPicture Tewi_01 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material01.png");
		public DDPicture Tewi_02 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material02.png");
		public DDPicture Tewi_03 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material03.png");
		public DDPicture Tewi_04 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material04.png");
		public DDPicture Tewi_05 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material05.png");
		public DDPicture Tewi_06 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material06.png");
		public DDPicture Tewi_07 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material07.png");
		public DDPicture Tewi_08 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material08.png");
		public DDPicture Tewi_09 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material09.png");
		public DDPicture Tewi_10 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material10.png");
		public DDPicture Tewi_11 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material11.png");
		public DDPicture Tewi_12 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material12.png");
		public DDPicture Tewi_13 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material13.png");
		public DDPicture Tewi_14 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material14.png");
		public DDPicture Tewi_15 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material15.png");
		public DDPicture Tewi_16 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material16.png");
		public DDPicture Tewi_17 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material17.png");
		public DDPicture Tewi_18 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material18.png");
		public DDPicture Tewi_19 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material19.png");
		public DDPicture Tewi_20 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material20.png");
		public DDPicture Tewi_21 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material21.png");
		public DDPicture Tewi_22 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material22.png");
		public DDPicture Tewi_23 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material23.png");
		public DDPicture Tewi_24 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material24.png");
		public DDPicture Tewi_25 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material25.png");
		public DDPicture Tewi_26 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material26.png");
		public DDPicture Tewi_27 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material27.png");
		public DDPicture Tewi_28 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material28.png");
		public DDPicture Tewi_29 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material29.png");
		public DDPicture Tewi_30 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material30.png");
		public DDPicture Tewi_31 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material31.png");
		public DDPicture Tewi_32 = DDPictureLoaders.BgTrans(@"dat\オオバコ\因幡てゐ\tewi_material32.png");

		public DDPicture Cirno_01 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno01.png");
		public DDPicture Cirno_02 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno02.png");
		public DDPicture Cirno_03 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno03.png");
		public DDPicture Cirno_04 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno04.png");
		public DDPicture Cirno_05 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno05.png");
		public DDPicture Cirno_06 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno06.png");
		public DDPicture Cirno_07 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno07.png");
		public DDPicture Cirno_08 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno08.png");
		public DDPicture Cirno_09 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno09.png");
		public DDPicture Cirno_10 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno10.png");
		public DDPicture Cirno_11 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno11.png");
		public DDPicture Cirno_12 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno12.png");
		public DDPicture Cirno_13 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno13.png");
		public DDPicture Cirno_14 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno14.png");
		public DDPicture Cirno_15 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno15.png");
		public DDPicture Cirno_16 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno16.png");
		public DDPicture Cirno_17 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno17.png");
		public DDPicture Cirno_18 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno18.png");
		public DDPicture Cirno_19 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno19.png");
		public DDPicture Cirno_20 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno20.png");
		public DDPicture Cirno_21 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno21.png");
		public DDPicture Cirno_22 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno22.png");
		public DDPicture Cirno_23 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno23.png");
		public DDPicture Cirno_24 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno24.png");
		public DDPicture Cirno_25 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno25.png");
		public DDPicture Cirno_26 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno26.png");
		public DDPicture Cirno_27 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno27.png");
		public DDPicture Cirno_28 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno28.png");
		public DDPicture Cirno_29 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno29.png");
		public DDPicture Cirno_30 = DDPictureLoaders.BgTrans(@"dat\オオバコ\チルノ\free_cirno30.png");

		public DDPicture Tile_None = DDPictureLoaders.Standard(@"dat\Tile\Tile_None.png");
		public DDPicture Tile_B0001 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0001.png");
		public DDPicture Tile_B0002 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0002.png");
		public DDPicture Tile_B0003 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0003.png");
		public DDPicture Tile_B0004 = DDPictureLoaders.Standard(@"dat\Tile\Tile_B0004.png");

		public DDPicture Wall_R0001 = DDPictureLoaders.Standard(@"dat\run\22350006_big_p30.jpg");
		public DDPicture Wall_R0002 = DDPictureLoaders.Standard(@"dat\run\22350006_big_p10.jpg");
		public DDPicture Wall_R0003 = DDPictureLoaders.Standard(@"dat\run\22350006_big_p31.jpg");

		public DDPicture Enemy_B0001_01 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_01.png");
		public DDPicture Enemy_B0001_02 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_02.png");
		public DDPicture Enemy_B0001_03 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_03.png");
		public DDPicture Enemy_B0001_04 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0001_04.png");

		public DDPicture Enemy_B0002_01 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0002_01.png");
		public DDPicture Enemy_B0002_02 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0002_02.png");

		public DDPicture Enemy_B0003 = DDPictureLoaders.Standard(@"dat\Picture\Enemy_B0003.png");

		public DDPicture Enemy_TrumpBack = DDPictureLoaders.Standard(@"dat\Picture\TrumpBack.png");
		public DDPicture Enemy_TrumpFrame = DDPictureLoaders.Standard(@"dat\Picture\TrumpFrame.png");
		public DDPicture Enemy_TrumpS01 = DDPictureLoaders.Standard(@"dat\Picture\TrumpS01.png");

		//public DDPicture Enemy_神奈子 = DDPictureLoaders.Reduct(@"dat\きつね仮\yukkuri-kanako.png", 4); // resize 4000x4000 -> 1000x1000
		public DDPicture Enemy_神奈子 = DDPictureLoaders.Standard(@"dat\きつね仮\yukkuri-kanako.png"); // use 1000x1000 resized png

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

		public ResourcePicture()
		{
			// none
		}
	}
}
