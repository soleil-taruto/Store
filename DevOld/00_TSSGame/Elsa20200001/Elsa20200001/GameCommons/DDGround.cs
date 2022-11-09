using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDGround
	{
		public static DDTaskList EL = new DDTaskList();
		public static DDTaskList SystemTasks = new DDTaskList();
		public static int PrimaryPadId = -1; // -1 == 未設定
		public static DDSubScreen MainScreen;
		public static DDSubScreen LastMainScreen;
		public static DDSubScreen KeptMainScreen;
		public static I4Rect MonitorRect;

		// 実際の画面サイズ
		//
		public static int RealScreen_W = DDConsts.Screen_W;
		public static int RealScreen_H = DDConsts.Screen_H;

		// ゲーム画面を描画する位置とサイズ
		// RealScreenDraw_W == -1 の場合は { 0, 0, RealScreen_W, RealScreen_H } に描画する。
		//
		public static int RealScreenDraw_L;
		public static int RealScreenDraw_T;
		public static int RealScreenDraw_W = -1; // -1 == RealScreenDraw_LTWH 不使用
		public static int RealScreenDraw_H;

		// フルスクリーン解除時の画面サイズ
		//
		public static int UnfullScreen_W = DDConsts.Screen_W;
		public static int UnfullScreen_H = DDConsts.Screen_H;

		// 音楽の音量
		// 0.0 ～ 1.0
		// 変更を反映するには -> DDMusicUtils.UpdateVolume();
		// 実際の音量は DDSoundUtils.MixVolume(DDGround.MusicVolume, <DDMusic>.Volume) になる。
		//
		public static double MusicVolume = DDConsts.DefaultVolume;

		// 効果音の音量
		// 0.0 ～ 1.0
		// 変更を反映するには -> DDSEUtils.UpdateVolume();
		// 実際の音量は DDSoundUtils.MixVolume(DDGround.SEVolume, <DDSE>.Volume) になる。
		//
		public static double SEVolume = DDConsts.DefaultVolume;

		// マウスカーソルを表示するか
		// 変更を反映するには -> DDUtils.SetMouseDispMode(DDGround.RO_MouseDispMode);
		//
		public static bool RO_MouseDispMode = false;

		// カメラ位置
		//
		// カメラ位置を変更する場合、フレームループ内で描画を行う前に RealCamera を更新し、その直後に以下のとおり Camera を更新すること。
		// DDGround.Camera.X = DoubleTools.ToInt(DDGround.RealCamera.X);
		// DDGround.Camera.Y = DoubleTools.ToInt(DDGround.RealCamera.Y);
		//
		// カメラ位置に影響を受ける画像の描画には Camera の座標を使うこと。
		// 例：DDDraw.DrawCenter(picture, drawX - Camera.X, drawY - Camera.Y);
		//
		public static D2Point RealCamera = new D2Point();
		public static I2Point Camera = new I2Point();

		public static void INIT()
		{
			// -- 全アプリ共通の設定 ...

			DDInput.DIR_2.BtnIds = new int[] { 0 };
			DDInput.DIR_4.BtnIds = new int[] { 1 };
			DDInput.DIR_6.BtnIds = new int[] { 2 };
			DDInput.DIR_8.BtnIds = new int[] { 3 };
			DDInput.A.BtnIds = new int[] { 4 };
			DDInput.B.BtnIds = new int[] { 7 };
			DDInput.C.BtnIds = new int[] { 5 };
			DDInput.D.BtnIds = new int[] { 8 };
			DDInput.E.BtnIds = new int[] { 6 };
			DDInput.F.BtnIds = new int[] { 9 };
			DDInput.L.BtnIds = new int[] { 10 };
			DDInput.R.BtnIds = new int[] { 11 };
			DDInput.PAUSE.BtnIds = new int[] { 13 };
			DDInput.START.BtnIds = new int[] { 12 };

			DDInput.DIR_2.KeyIds = new int[] { DX.KEY_INPUT_DOWN };
			DDInput.DIR_4.KeyIds = new int[] { DX.KEY_INPUT_LEFT };
			DDInput.DIR_6.KeyIds = new int[] { DX.KEY_INPUT_RIGHT };
			DDInput.DIR_8.KeyIds = new int[] { DX.KEY_INPUT_UP };
			DDInput.A.KeyIds = new int[] { DX.KEY_INPUT_RETURN, DX.KEY_INPUT_Z };
			DDInput.B.KeyIds = new int[] { DX.KEY_INPUT_DELETE, DX.KEY_INPUT_X };
			DDInput.C.KeyIds = new int[] { DX.KEY_INPUT_C };
			DDInput.D.KeyIds = new int[] { DX.KEY_INPUT_V };
			DDInput.E.KeyIds = new int[] { DX.KEY_INPUT_A };
			DDInput.F.KeyIds = new int[] { DX.KEY_INPUT_S };
			DDInput.L.KeyIds = new int[] { DX.KEY_INPUT_LCONTROL, DX.KEY_INPUT_RCONTROL };
			DDInput.R.KeyIds = new int[] { DX.KEY_INPUT_F };
			DDInput.PAUSE.KeyIds = new int[] { DX.KEY_INPUT_SPACE };
			DDInput.START.KeyIds = new int[] { DX.KEY_INPUT_END };

			// -- 以下アプリ固有の設定 ...

			RealScreen_W /= 2;
			RealScreen_H /= 2;

			UnfullScreen_W /= 2;
			UnfullScreen_H /= 2;

			RO_MouseDispMode = true;
		}
	}
}
