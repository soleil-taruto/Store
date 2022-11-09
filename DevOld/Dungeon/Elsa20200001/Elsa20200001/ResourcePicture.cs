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

		public DDPicture Title = DDPictureLoaders.Standard(@"dat\run\22350006_big_p12.jpg");
		public DDPicture[] TitleMenuItems = new DDPicture[]
		{
			DDPictureLoaders.Standard(@"dat\Title\ゲームスタート.png"),
			DDPictureLoaders.Standard(@"dat\Title\コンテニュー.png"),
			DDPictureLoaders.Standard(@"dat\Title\おまけ.png"),
			DDPictureLoaders.Standard(@"dat\Title\設定.png"),
			DDPictureLoaders.Standard(@"dat\Title\終了.png"),
		};

		// ノベルパート用システム画像
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
	}
}
