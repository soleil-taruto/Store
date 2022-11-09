using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public DDPicture MessageFrame29_Message = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\01 message\message.png");
		public DDPicture MessageFrame29_Button = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\02 button\button.png");
		public DDPicture MessageFrame29_Button2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\02 button\button2.png");
		public DDPicture MessageFrame29_Button3 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\02 button\button3.png");
		public DDPicture MessageFrame29_Auto = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\auto.png");
		public DDPicture MessageFrame29_Auto2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\auto2.png");
		public DDPicture MessageFrame29_Load = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\load.png");
		public DDPicture MessageFrame29_Load2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\load2.png");
		public DDPicture MessageFrame29_Log = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\log.png");
		public DDPicture MessageFrame29_Log2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\log2.png");
		public DDPicture MessageFrame29_Menu = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\menu.png");
		public DDPicture MessageFrame29_Menu2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\menu2.png");
		public DDPicture MessageFrame29_Save = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\save.png");
		public DDPicture MessageFrame29_Save2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\save2.png");
		public DDPicture MessageFrame29_Skip = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\skip.png");
		public DDPicture MessageFrame29_Skip2 = DDPictureLoaders.Standard(@"dat\フリー素材\空想曲線\Messageframe_29\material\03 system_button\skip2.png");

		public DDPicture TitleMenuItem_はじめから = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\はじめから.png");
		public DDPicture TitleMenuItem_つづきから = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\つづきから.png");
		public DDPicture TitleMenuItem_設定 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\設定.png");
		public DDPicture TitleMenuItem_終了 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\終了.png");

		public DDPicture SettingButton_960x540 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\960 x 540.png");
		public DDPicture SettingButton_1080x607 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1080 x 607.png");
		public DDPicture SettingButton_1200x675 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1200 x 675.png");
		public DDPicture SettingButton_1320x742 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1320 x 742.png");
		public DDPicture SettingButton_1440x810 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1440 x 810.png");
		public DDPicture SettingButton_1560x877 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1560 x 877.png");
		public DDPicture SettingButton_1680x945 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1680 x 945.png");
		public DDPicture SettingButton_1800x1012 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1800 x 1012.png");
		public DDPicture SettingButton_1920x1080 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\1920 x 1080.png");
		public DDPicture SettingButton_2040x1147 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\2040 x 1147.png");
		public DDPicture SettingButton_2160x1215 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\2160 x 1215.png");
		public DDPicture SettingButton_2280x1282 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\2280 x 1282.png");
		public DDPicture SettingButton_ウィンドウ = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\ウィンドウ.png");
		public DDPicture SettingButton_フルスクリーン = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\フルスクリーン.png");
		public DDPicture SettingButton_フルスクリーン画面に合わせる = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\フルスクリーン 画面に合わせる (非推奨).png");
		public DDPicture SettingButton_フルスクリーン縦横比を維持する = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\フルスクリーン 縦横比を維持する (推奨).png");

		public DDPicture SettingButton_ウィンドウサイズ設定 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\ウィンドウサイズ設定.png");
		public DDPicture SettingButton_キーボードのキー設定 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\キーボードのキー設定.png");
		public DDPicture SettingButton_ゲームパッドのボタン設定 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\ゲームパッドのボタン設定.png");

		public DDPicture SettingButton_デフォルトに戻す = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\デフォルトに戻す.png");
		public DDPicture SettingButton_キャンセル = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\キャンセル.png");
		public DDPicture SettingButton_変更 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\変更.png");
		public DDPicture SettingButton_戻る = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\戻る.png");
		public DDPicture SettingButton_決定 = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\決定.png");

		public DDPicture SettingButton_前へ = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\前へ.png");
		public DDPicture SettingButton_次へ = DDPictureLoaders.Standard(@"dat\タイトル設定ボタン\次へ.png");

		public DDPicture 基本設定枠 = DDPictureLoaders.Standard(@"dat\タイトル設定枠\基本設定.png");
		public DDPicture 拡張設定枠 = DDPictureLoaders.Standard(@"dat\タイトル設定枠\拡張設定.png");
		public DDPicture 詳細設定枠 = DDPictureLoaders.Standard(@"dat\タイトル設定枠\詳細設定.png");
		public DDPicture TrackBar = DDPictureLoaders.Standard(@"dat\タイトル設定枠\TrackBar.png");
		public DDPicture TrackBar_つまみ = DDPictureLoaders.Standard(@"dat\タイトル設定枠\TrackBar_つまみ.png");
		public DDPicture SaveDataSlot = DDPictureLoaders.Standard(@"dat\タイトル設定枠\SaveDataSlot.png");

		public DDPicture Title = DDPictureLoaders.Standard(@"dat\フリー素材\かんなにらせ\nc78901.jpg");
	}
}
