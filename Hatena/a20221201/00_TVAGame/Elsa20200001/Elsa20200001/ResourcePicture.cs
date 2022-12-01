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

		public DDPicture TitleWall = DDPictureLoaders.Standard(@"dat\k-after\BG23a_80.jpg");
		public DDPicture Title = DDPictureLoaders.Standard(@"dat\Picture\Actor83_1.png");
		public DDPicture[] TitleMenuItems = new DDPicture[]
		{
			DDPictureLoaders.Standard(@"dat\Title\ゲームスタート.png"),
			DDPictureLoaders.Standard(@"dat\Title\コンテニュー.png"),
			DDPictureLoaders.Standard(@"dat\Title\おまけ.png"),
			DDPictureLoaders.Standard(@"dat\Title\設定.png"),
			DDPictureLoaders.Standard(@"dat\Title\終了.png"),
		};

		public DDPicture Player_Alice = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_alice2_2.png");
		public DDPicture Player_高麗野あうん = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_aunn.png");
		public DDPicture Player_赤蛮奇_体 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_banki.png");
		public DDPicture Player_Cirno = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_cirno2.png");
		public DDPicture Player_戎珱花 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_eika.png");
		public DDPicture Player_易者 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_ekisha.png");
		public DDPicture Player_依神女苑 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_jyoon.png");
		public DDPicture Player_今泉影狼 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kagerou0.png");
		public DDPicture Player_今泉影狼_狼 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kagerou1.png");
		public DDPicture Player_埴安神袿姫 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_keiki_3.png");
		public DDPicture Player_秦こころ = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoro.png");
		public DDPicture Player_秦こころ_面01 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen01.png");
		public DDPicture Player_秦こころ_面02 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen02.png");
		public DDPicture Player_秦こころ_面03 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen03.png");
		public DDPicture Player_秦こころ_面04 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen04.png");
		public DDPicture Player_秦こころ_面05 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen05.png");
		public DDPicture Player_秦こころ_面06 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen06.png");
		public DDPicture Player_秦こころ_面07 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen07.png");
		public DDPicture Player_秦こころ_面08 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen08.png");
		public DDPicture Player_秦こころ_面09 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen09.png");
		public DDPicture Player_秦こころ_面10 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kokoroomen10.png");
		public DDPicture Player_庭渡久侘歌 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_kutaka.png");
		public DDPicture Player_ラルバ = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_larva.png");
		public DDPicture Player_丁礼田舞 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_mait.png");
		public DDPicture Player_杖刀偶磨弓 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_mayumi.png");
		public DDPicture Player_矢田寺成美 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_narumi.png");
		public DDPicture Player_矢田寺成美_地蔵 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_narumi_jizo.png");
		public DDPicture Player_矢田寺成美_NoHat = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_narumi_nohat.png");
		public DDPicture Player_坂田ネムノ = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_nemuno.png");
		public DDPicture Player_摩多羅隠岐奈 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_okina.png");
		public DDPicture Player_摩多羅隠岐奈_Chair = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_okinachair.png");
		public DDPicture Player_面01 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen01.png");
		public DDPicture Player_面02 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen02.png");
		public DDPicture Player_面03 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen03.png");
		public DDPicture Player_面04 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen04.png");
		public DDPicture Player_面05 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen05.png");
		public DDPicture Player_面06 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen06.png");
		public DDPicture Player_面07 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen07.png");
		public DDPicture Player_面08 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen08.png");
		public DDPicture Player_面09 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen09.png");
		public DDPicture Player_面10 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_omen10.png");
		public DDPicture Player_驪駒早鬼 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_saki_2.png");
		public DDPicture Player_爾子田里乃 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_satono.png");
		public DDPicture Player_赤蛮奇_頭 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_seki.png");
		public DDPicture Player_赤蛮奇 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_sekibanki.png");
		public DDPicture Player_依神紫苑_覚 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_shion_wake.png");
		public DDPicture Player_依神紫苑 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_shion0.png");
		public DDPicture Player_牛崎潤美 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_urumi1.png");
		public DDPicture Player_わかさぎ姫 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_wakasagi.png");
		public DDPicture Player_吉弔八千慧 = DDPictureLoaders.Standard(@"dat\点睛集積\thv8_yachie.png");

		public DDPicture Tile_A1 = DDPictureLoaders.Standard(@"dat\FSM\TileA1.png");
		public DDPicture Tile_A2 = DDPictureLoaders.Standard(@"dat\FSM\TileA2.png");
		public DDPicture Tile_A3 = DDPictureLoaders.Standard(@"dat\FSM\TileA3.png");
		public DDPicture Tile_A4 = DDPictureLoaders.Standard(@"dat\FSM\TileA4.png");
		public DDPicture Tile_A5 = DDPictureLoaders.Standard(@"dat\FSM\TileA5.png");
		public DDPicture Tile_B = DDPictureLoaders.Standard(@"dat\FSM\TileB.png");
		public DDPicture Tile_C = DDPictureLoaders.Standard(@"dat\FSM\TileC.png");
		public DDPicture Tile_D = DDPictureLoaders.Standard(@"dat\FSM\TileD.png");
		public DDPicture Tile_E = DDPictureLoaders.Standard(@"dat\FSM\TileE.png");
		public DDPicture Tile_Dummy = DDPictureLoaders.Standard(@"dat\Tile\DummyTile.png");

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
