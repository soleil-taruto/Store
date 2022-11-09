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

		public DDPicture Copyright = DDPictureLoaders.Standard(@"dat\Logo\Copyright.png");

		//public DDPicture Title = DDPictureLoaders.Standard(@"dat\Title.png");
		public DDPicture[] TitleMenuItems = new DDPicture[]
		{
			DDPictureLoaders.Standard(@"dat\Title\ゲームスタート.png"),
			DDPictureLoaders.Standard(@"dat\Title\コンテニュー.png"),
			DDPictureLoaders.Standard(@"dat\Title\おまけ.png"),
			DDPictureLoaders.Standard(@"dat\Title\設定.png"),
			DDPictureLoaders.Standard(@"dat\Title\終了.png"),
		};

		private const int CHARA_TIP_EXPNUM = 2;

		// -- プレイヤー・キャラチップ --

		public DDPicture Chara_A01_Jump = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_jump01.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Jump_Attack = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_jump02attack.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Jump_Damage = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_jump03damage.png", CHARA_TIP_EXPNUM);
		public DDPicture[] Chara_A01_Run = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_run01.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_run02.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_run03.png", CHARA_TIP_EXPNUM),
		};
		public DDPicture[] Chara_A01_Run_Attack = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_run04_attack.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_run05_attack.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_run06_attack.png", CHARA_TIP_EXPNUM),
		};
		public DDPicture Chara_A01_Sliding = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_sliding01.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Telepo_01 = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_telepo01.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Telepo_02 = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_telepo02.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Telepo_03 = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_telepo03.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Wait = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_wait01.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Wait_Mabataki = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_wait02.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Wait_Start = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_wait03start.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Wait_Attack = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_wait04attack.png", CHARA_TIP_EXPNUM);
		public DDPicture[] Chara_A01_Climb = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_climb01.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_climb02.png", CHARA_TIP_EXPNUM),
		};
		public DDPicture Chara_A01_Climb_Attack = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_climb03_attack.png", CHARA_TIP_EXPNUM);
		public DDPicture Chara_A01_Climb_Top = DDPictureLoaders.Expand(@"dat\chara_a01\chara_a01_climb04.png", CHARA_TIP_EXPNUM);

		// -- エフェクト・キャラチップ --

		public DDPicture[] Effect_A01_A_Explosion = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_a01.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_a02.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_a03.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_a04.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_a05.png", CHARA_TIP_EXPNUM),
		};
		public DDPicture[] Effect_A01_B_Explosion = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b01.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b02.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b03.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b04.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b05.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b06.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b07.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b08.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b09.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b10.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b11.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b12.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b13.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b14.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b15.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_explosion_b16.png", CHARA_TIP_EXPNUM),
		};
		public DDPicture Effect_A01_Shock_A = DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_shock01.png", CHARA_TIP_EXPNUM);
		public DDPicture[] Effect_A01_Shock_B = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_shock02.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_shock03.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_shock04.png", CHARA_TIP_EXPNUM),
		};
		public DDPicture[] Effect_A01_Sliding = new DDPicture[]
		{
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_sliding01.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_sliding02.png", CHARA_TIP_EXPNUM),
			DDPictureLoaders.Expand(@"dat\chara_a01\effect_a01_sliding03.png", CHARA_TIP_EXPNUM),
		};

		// -- ショット --

		public DDPicture Shot_Normal = DDPictureLoaders.Expand(@"dat\chara_a01\game_shot_a01.png", CHARA_TIP_EXPNUM);

		// --

		public DDPicture Tile_None = DDPictureLoaders.Expand(@"dat\Tile\Tile_None.png", CHARA_TIP_EXPNUM);
		public DDPicture Tile_B0001 = DDPictureLoaders.Expand(@"dat\Tile\Tile_B0001.png", CHARA_TIP_EXPNUM);
		public DDPicture Tile_B0002 = DDPictureLoaders.Expand(@"dat\Tile\Tile_B0002.png", CHARA_TIP_EXPNUM);
		public DDPicture Tile_B0003 = DDPictureLoaders.Expand(@"dat\Tile\Tile_B0003.png", CHARA_TIP_EXPNUM);
		public DDPicture Tile_B0004 = DDPictureLoaders.Expand(@"dat\Tile\Tile_B0004.png", CHARA_TIP_EXPNUM);

		public DDPicture Space_B0001 = DDPictureLoaders.Expand(@"dat\Tile\Space_B0001.png", CHARA_TIP_EXPNUM);

		//public DDPicture Wall_B0001 = DDPictureLoaders.Standard(@"dat\Wall_B0001.png");
		//public DDPicture Wall_B0002 = DDPictureLoaders.Standard(@"dat\Wall_B0002.png");
		//public DDPicture Wall_B0003 = DDPictureLoaders.Standard(@"dat\Wall_B0003.png");

		public DDPicture Enemy_B0001_01 = DDPictureLoaders.Standard(@"dat\Enemy_B0001_01.png");
		public DDPicture Enemy_B0001_02 = DDPictureLoaders.Standard(@"dat\Enemy_B0001_02.png");
		public DDPicture Enemy_B0001_03 = DDPictureLoaders.Standard(@"dat\Enemy_B0001_03.png");
		public DDPicture Enemy_B0001_04 = DDPictureLoaders.Standard(@"dat\Enemy_B0001_04.png");

		public DDPicture Enemy_B0002_01 = DDPictureLoaders.Standard(@"dat\Enemy_B0002_01.png");
		public DDPicture Enemy_B0002_02 = DDPictureLoaders.Standard(@"dat\Enemy_B0002_02.png");

		public DDPicture Enemy_B0003 = DDPictureLoaders.Standard(@"dat\Enemy_B0003.png");

		public DDPicture MessageFrame_Message = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\01 message\message.png");
		public DDPicture MessageFrame_Button = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\02 button\button.png");
		public DDPicture MessageFrame_Button2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\02 button\button2.png");
		public DDPicture MessageFrame_Button3 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\02 button\button3.png");
		public DDPicture MessageFrame_Auto = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\auto.png");
		public DDPicture MessageFrame_Auto2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\auto2.png");
		public DDPicture MessageFrame_Load = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\load.png");
		public DDPicture MessageFrame_Load2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\load2.png");
		public DDPicture MessageFrame_Log = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\log.png");
		public DDPicture MessageFrame_Log2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\log2.png");
		public DDPicture MessageFrame_Menu = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\menu.png");
		public DDPicture MessageFrame_Menu2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\menu2.png");
		public DDPicture MessageFrame_Save = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\save.png");
		public DDPicture MessageFrame_Save2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\save2.png");
		public DDPicture MessageFrame_Skip = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\skip.png");
		public DDPicture MessageFrame_Skip2 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\空想曲線\Messageframe_29\material\03 system_button\skip2.png");

		public DDPicture 結月ゆかり02 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\からい\ゆかマキ制服\yukari02.png"); // 仮
		public DDPicture 結月ゆかり03 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\からい\ゆかマキ制服\yukari03.png"); // 仮

		public DDPicture 弦巻マキ01 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\からい\ゆかマキ制服\maki01.png"); // 仮
		public DDPicture 弦巻マキ02 = DDPictureLoaders.Standard(@"dat\Novel\フリー素材\からい\ゆかマキ制服\maki02.png"); // 仮

		// -- [チップ] --

		public DDPicture Stage01_Bg_a01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_bg_a01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Bg_a02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_bg_a02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Bg_a03 = DDPictureLoaders.Expand(@"dat\チップ\stage01_bg_a03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Bg_Item01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_bg_item01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Bg_Item02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_bg_item02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Bg_Item03 = DDPictureLoaders.Expand(@"dat\チップ\stage01_bg_item03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_a01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_a01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_a02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_a02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_b01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_b01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_b02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_b02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c03 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c04 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c04.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c05 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c05.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c06 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c06.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c07 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c07.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c08 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c08.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c09 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c09.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c10 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c10.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c11 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c11.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c12 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c12.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_c13 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_c13.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_d01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_d01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_d02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_d02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_d03 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_d03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_e01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_e01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_e02 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_e02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_e03 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_e03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage01_Chip_f01 = DDPictureLoaders.Expand(@"dat\チップ\stage01_chip_f01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a01_Fly01 = DDPictureLoaders.Expand(@"dat\チップ\teki_a01_fly01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a01_Fly02 = DDPictureLoaders.Expand(@"dat\チップ\teki_a01_fly02.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a01_Fly03 = DDPictureLoaders.Expand(@"dat\チップ\teki_a01_fly03.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a01_Gliding01 = DDPictureLoaders.Expand(@"dat\チップ\teki_a01_gliding01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a01_Shit01 = DDPictureLoaders.Expand(@"dat\チップ\teki_a01_shit01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a02_Run01 = DDPictureLoaders.Expand(@"dat\チップ\teki_a02_run01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a02_Run02 = DDPictureLoaders.Expand(@"dat\チップ\teki_a02_run02.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a02_Run03 = DDPictureLoaders.Expand(@"dat\チップ\teki_a02_run03.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a02_Wait01 = DDPictureLoaders.Expand(@"dat\チップ\teki_a02_wait01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a03_Jump01 = DDPictureLoaders.Expand(@"dat\チップ\teki_a03_jump01.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a03_Jump02 = DDPictureLoaders.Expand(@"dat\チップ\teki_a03_jump02.png", CHARA_TIP_EXPNUM);
		public DDPicture Teki_a03_Jump03 = DDPictureLoaders.Expand(@"dat\チップ\teki_a03_jump03.png", CHARA_TIP_EXPNUM);

		// -- [ロックマン風] --

		public DDPicture Stage02_Bg_Chip_a01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_a01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_a02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_a02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_a03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_a03.png", CHARA_TIP_EXPNUM);
		public DDPicture[][] Stage02_Bg_Chip_b0x = new DDPicture[][]
		{
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b01_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b01_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b01_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b01_04.png", CHARA_TIP_EXPNUM),
			},
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b02_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b02_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b02_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b02_04.png", CHARA_TIP_EXPNUM),
			},
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b03_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b03_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b03_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b03_04.png", CHARA_TIP_EXPNUM),
			},
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b04_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b04_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b04_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_b04_04.png", CHARA_TIP_EXPNUM),
			},
		};
		public DDPicture Stage02_Bg_Chip_c01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c04 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c04.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c05 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c05.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c06 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c06.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c07 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c07.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c08 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c08.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c09 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c09.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c10 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c10.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c11 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c11.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c12 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c12.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c13 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c13.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Bg_Chip_c14 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_bg_chip_c14.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a04 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a04.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a05 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a05.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a06 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a06.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a07 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a07.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a08 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a08.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a09 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a09.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a10 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a10.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a11 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a11.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a12 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a12.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a13 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a13.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a14 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a14.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a15 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a15.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a16 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a16.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a17 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a17.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a18 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a18.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a19 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a19.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a20 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a20.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a21 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a21.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a22 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a22.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a23 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a23.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a24 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a24.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a25 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a25.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a26 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a26.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a27 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a27.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_a28 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_a28.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b04 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b04.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b05 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b05.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b06 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b06.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b07 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b07.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b08 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b08.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_b09 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_b09.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_c01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_c01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_c02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_c02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_c03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_c03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_d01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_d01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_e01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_e01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_e02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_e02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_e03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_e03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_e04 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_e04.png", CHARA_TIP_EXPNUM);
		public DDPicture[][] Stage02_Chip_f0x = new DDPicture[][]
		{
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f01_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f01_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f01_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f01_04.png", CHARA_TIP_EXPNUM),
			},
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f02_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f02_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f02_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f02_04.png", CHARA_TIP_EXPNUM),
			},
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f03_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f03_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f03_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f03_04.png", CHARA_TIP_EXPNUM),
			},
			new DDPicture[]
			{
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f04_01.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f04_02.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f04_03.png", CHARA_TIP_EXPNUM),
				DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_f04_04.png", CHARA_TIP_EXPNUM),
			},
		};
		public DDPicture Stage02_Chip_g04_01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_g04_01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_g04_02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_g04_02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_g04_03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_g04_03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_g04_04 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_g04_04.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h01 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h01.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h02 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h02.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h03 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h03.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h04 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h04.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h05 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h05.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h06 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h06.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h07 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h07.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h08 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h08.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h09 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h09.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h10 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h10.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h11 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h11.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h12 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h12.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h13 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h13.png", CHARA_TIP_EXPNUM);
		public DDPicture Stage02_Chip_h14 = DDPictureLoaders.Expand(@"dat\ロックマン風\stage02_chip_h14.png", CHARA_TIP_EXPNUM);

		// --

		public DDPicture 陰陽玉 = DDPictureLoaders.Standard(@"dat\blueberry\nc116914.png");
		//public DDPicture 陰陽玉 = DDPictureLoaders.Expand(@"dat\blueberry\nc116914.png", CHARA_TIP_EXPNUM);
		public DDPicture Laser = DDPictureLoaders.Standard(@"dat\Laser.png");

		public DDPicture[] Crystals = new DDPicture[]
		{
			DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-etcchara002.png"), // 青
			DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-etcchara002a.png"), // 赤
			DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-etcchara002b.png"), // 緑
			DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-etcchara002c.png"), // 黄色
			DDPictureLoaders.Standard(@"dat\ぴぽや倉庫\pipo-etcchara002d.png"), // 白黒
		};
	}
}
