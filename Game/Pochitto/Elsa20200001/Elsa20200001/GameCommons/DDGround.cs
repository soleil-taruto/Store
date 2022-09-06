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

			DDInput.DIR_2.BtnId = 0;
			DDInput.DIR_4.BtnId = 1;
			DDInput.DIR_6.BtnId = 2;
			DDInput.DIR_8.BtnId = 3;
			DDInput.A.BtnId = 4;
			DDInput.B.BtnId = 7;
			DDInput.C.BtnId = 5;
			DDInput.D.BtnId = 8;
			DDInput.E.BtnId = 6;
			DDInput.F.BtnId = 9;
			DDInput.L.BtnId = 10;
			DDInput.R.BtnId = 11;
			DDInput.PAUSE.BtnId = 13;
			DDInput.START.BtnId = 12;

			DDInput.DIR_2.KeyId = DX.KEY_INPUT_DOWN;
			DDInput.DIR_4.KeyId = DX.KEY_INPUT_LEFT;
			DDInput.DIR_6.KeyId = DX.KEY_INPUT_RIGHT;
			DDInput.DIR_8.KeyId = DX.KEY_INPUT_UP;
			DDInput.A.KeyId = DX.KEY_INPUT_Z;
			DDInput.B.KeyId = DX.KEY_INPUT_X;
			DDInput.C.KeyId = DX.KEY_INPUT_C;
			DDInput.D.KeyId = DX.KEY_INPUT_V;
			DDInput.E.KeyId = DX.KEY_INPUT_B;
			DDInput.F.KeyId = DX.KEY_INPUT_N;
			DDInput.L.KeyId = DX.KEY_INPUT_LCONTROL;
			DDInput.R.KeyId = DX.KEY_INPUT_LSHIFT;
			DDInput.PAUSE.KeyId = DX.KEY_INPUT_SPACE;
			DDInput.START.KeyId = DX.KEY_INPUT_RETURN;

			// -- 以下アプリ固有の設定 ...

			//RO_MouseDispMode = true;
		}
	}
}
