using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class GameConsts
	{
		/// <summary>
		/// セーブデータ・スロット数
		/// </summary>
		public const int SAVE_DATA_SLOT_NUM = 100;

		// ====
		// 主要な待ち時間_ここから
		// ====

		/// <summary>
		/// 短めの入力抑止時間(フレーム数)
		/// 例：前のホイール操作が次のホイール入力受付に反応してしまわないように
		/// </summary>
		public const int SHORT_INPUT_SLEEP = 5;

		/// <summary>
		/// 長めの入力抑止時間(フレーム数)
		/// 例：メニューから戻ってきたとき
		/// </summary>
		public const int LONG_INPUT_SLEEP = 30;

		/// <summary>
		/// 現在のページのテキストを表示し終えてから次ページへ遷移させないための入力抑止時間(フレーム数)
		/// </summary>
		public const int NEXT_PAGE_INPUT_INTERVAL = 10;

		/// <summary>
		/// 自動モードで次ページへ遷移するまでの時間(フレーム数)
		/// </summary>
		public const int AUTO_NEXT_PAGE_INTERVAL = 180;

		// ====
		// 主要な待ち時間_ここまで
		// ====

		public const string DUMMY_SCENARIO_NAME = "Tests/ダミーシナリオ";
		public const string FIRST_SCENARIO_NAME = "0001_スタートシナリオ";

		public const int MESSAGE_SPEED_MIN = 1; // 遅い
		public const int MESSAGE_SPEED_DEF = 3;
		public const int MESSAGE_SPEED_MAX = 5; // 速い

		public const int MESSAGE_WINDOW_A_PCT_DEF = 90;

		public const double SYSTEM_BUTTON_X = 1060.0;
		public const double SYSTEM_BUTTON_Y = 824.0;
		public const double SYSTEM_BUTTON_X_STEP = 156.0;

		public const int SELECT_FRAME_L = 580;
		public const int SELECT_FRAME_T = 140;
		public const int SELECT_FRAME_T_STEP = 200;
		public const int SELECT_FRAME_NUM = 3;

		public const int SELECT_OPTION_MIN = 1;
		public const int SELECT_OPTION_MAX = SELECT_FRAME_NUM;
	}
}
