using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDMain
	{
		public static List<Action> Finalizers = new List<Action>();

		private static int LogCount = 0;

		public static void GameStart()
		{
			foreach (string dllFile in "DxLib.dll:DxLibDotNet.dll".Split(':')) // DxLibDotNet.dll 等の存在確認 (1)
				if (!File.Exists(dllFile))
					throw new DDError();

			DX.GetColor(0, 0, 0); // DxLibDotNet.dll 等の存在確認 (2)

			DDConfig.Load(); // LogFile, LOG_ENABLED 等を含むので真っ先に

			// Log >

			File.WriteAllBytes(DDConfig.LogFile, SCommon.EMPTY_BYTES);

			ProcMain.WriteLog = message =>
			{
				if (LogCount < DDConfig.LogCountMax)
				{
					using (StreamWriter writer = new StreamWriter(DDConfig.LogFile, true, Encoding.UTF8))
					{
						writer.WriteLine("[" + DateTime.Now + "] " + message);
					}
					LogCount++;
				}
			};

			// < Log

			// *.INIT
			{
				DDGround.INIT();
				DDResource.INIT();
				DDDatStrings.INIT();
				DDFontRegister.INIT();
				DDKey.INIT();
			}

			DDSaveData.Load();

			Action showStartupMessage = () => LiteStatusDlg.StartDisplay("ゲームを起動しています...");

			if (DDConfig.DisplayIndex == -2) // *** DDGround.MonitorRect 初期化 (DisplayIndex == -2 の場合)
			{
				I2Point mousePt = DDWin32.GetMousePosition();
				I4Rect[] monitors = DDWin32.GetAllMonitor();
				I4Rect activeMonitor = monitors[0]; // マウス位置のモニタを特定できない場合のモニタ

				foreach (I4Rect monitor in monitors)
				{
					if (
						monitor.L <= mousePt.X && mousePt.X < monitor.R &&
						monitor.T <= mousePt.Y && mousePt.Y < monitor.B
						)
					{
						activeMonitor = monitor;
						break;
					}
				}
				DDGround.MonitorRect = activeMonitor;

				showStartupMessage();
			}

			// DxLib >

			if (DDConfig.LOG_ENABLED)
				DX.SetApplicationLogSaveDirectory(SCommon.MakeFullPath(DDConfig.ApplicationLogSaveDirectory));

			DX.SetOutApplicationLogValidFlag(DDConfig.LOG_ENABLED ? 1 : 0); // DxLib のログを出力 1: する 0: しない

			DX.SetAlwaysRunFlag(1); // ? 非アクティブ時に 1: 動く 0: 止まる

			SetMainWindowTitle();

			//DX.SetGraphMode(DDConsts.Screen_W, DDConsts.Screen_H, 32);
			DX.SetGraphMode(DDGround.RealScreen_W, DDGround.RealScreen_H, 32);
			DX.ChangeWindowMode(1); // 1: ウィンドウ 0: フルスクリーン

			//DX.SetFullSceneAntiAliasingMode(4, 2); // フルスクリーンを廃止したので不要

			DX.SetWindowIconHandle(GetAppIcon()); // ウィンドウ左上のアイコン

			if (0 <= DDConfig.DisplayIndex)
				DX.SetUseDirectDrawDeviceIndex(DDConfig.DisplayIndex);

			if (DX.DxLib_Init() != 0) // ? 失敗
				throw new DDError();

			Finalizers.Add(() =>
			{
				if (DX.DxLib_End() != 0) // ? 失敗
					throw new DDError();
			});

			DDUtils.SetMouseDispMode(DDGround.RO_MouseDispMode); // ? マウスを表示する。
			DX.SetWindowSizeChangeEnableFlag(0); // ウィンドウの右下をドラッグで伸縮 1: する 0: しない

			//DX.SetDrawScreen(DX.DX_SCREEN_BACK);
			DX.SetDrawMode(DDConsts.DEFAULT_DX_DRAWMODE); // これをデフォルトとする。

			// < DxLib

			DDGround.MainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
			DDGround.MainScreen.ChangeDrawScreen();
			DDGround.LastMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
			DDGround.KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

			if (DDConfig.DisplayIndex != -2) // *** DDGround.MonitorRect 初期化 (DisplayIndex != -2 の場合)
			{
				int l;
				int t;
				int w;
				int h;
				int p1;
				int p2;
				int p3;
				int p4;
				int p5;
				int p6;

				DX.GetDefaultState(out w, out h, out p1, out p2, out l, out t, out p3, out p4, out p5, out p6);

				if (
					w < 1 || SCommon.IMAX < w ||
					h < 1 || SCommon.IMAX < h ||
					l < -SCommon.IMAX || SCommon.IMAX < l ||
					t < -SCommon.IMAX || SCommon.IMAX < t
					)
					throw new DDError();

				DDGround.MonitorRect = new I4Rect(l, t, w, h);

				showStartupMessage();
			}

			PostSetScreenSize(DDGround.RealScreen_W, DDGround.RealScreen_H);

			// Font
			{
				//DDFontRegister.Add(@"dat\Font\Genkai-Mincho-font\genkai-mincho.ttf");
				//DDFontRegister.Add(@"dat\Font\riitf\RiiT_F.otf");
				DDFontRegister.Add(@"dat\Font\K Gothic\K Gothic.ttf");
				DDFontRegister.Add(@"dat\Font\木漏れ日ゴシック\komorebi-gothic.ttf");
			}

			Ground.I = new Ground();
			Ground.I.Picture2 = new ResourcePicture2(); // Ground.I を参照しているので Ground のコンストラクタには書けない。

			MainWin.I.PostGameStart_G3();

			DDSaveData.Load_Delay();

			Finalizers.Add(() =>
			{
				DDSaveData.Save();
			});

			LiteStatusDlg.EndDisplayDelay();
		}

		public static void GameEnd(List<Exception> errors)
		{
			while (1 <= Finalizers.Count)
			{
				try
				{
					SCommon.UnaddElement(Finalizers)();
				}
				catch (Exception e)
				{
					errors.Add(e);
				}
			}
		}

		public static void SetMainWindowTitle()
		{
			string revision = GetRevision(
				new SCommon.SimpleDateTime(SCommon.TimeStampToSec.ToSec(19700101090000) + ProcMain.GetPETimeDateStamp())
				);

			DX.SetMainWindowText(DDDatStrings.Title + " / " + revision);
		}

		/// <summary>
		/// 指定日時をリビジョン文字列に変換する。
		/// 但し、区切り文字に半角ハイフンを使用する。
		/// 元の実装：
		/// -- https://github.com/stackprobe/Factory/blob/master/DevTools/rev.c#L28-L71
		/// </summary>
		/// <param name="dateTime">指定日時</param>
		/// <returns>リビジョン文字列</returns>
		private static string GetRevision(SCommon.SimpleDateTime dateTime)
		{
			SCommon.SimpleDateTime firstDateTimeOfYear =
				SCommon.SimpleDateTime.FromTimeStamp(dateTime.Year * 10000000000L + 101000000);

			int revY = dateTime.Year;
			int revD = (int)((dateTime - firstDateTimeOfYear) / 86400 + 1);
			int revT = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;

			if (revT < 10000)
			{
				revD--;
				revT += 86400;
			}
			if (revD < 100)
			{
				SCommon.SimpleDateTime firstDateTimeOfLastYear =
					SCommon.SimpleDateTime.FromTimeStamp((dateTime.Year - 1) * 10000000000L + 101000000);

				int daysOfLastYear = (int)((firstDateTimeOfYear - firstDateTimeOfLastYear) / 86400);

				revY--;
				revD += daysOfLastYear;
			}
			return string.Format("{0}-{1}-{2}", revY, revD, revT);
		}

		private static IntPtr GetAppIcon()
		{
			using (MemoryStream mem = new MemoryStream(DDResource.Load(@"dat\General\game_app.ico")))
			{
				return new Icon(mem).Handle;
			}
		}

		public static void SetFullScreen()
		{
			BeforeSetScreenSize();
			P_SetFullScreen();
		}

		public static void SetScreenSize(int w, int h)
		{
			BeforeSetScreenSize();
			P_SetScreenSize(w, h);
		}

		private static void BeforeSetScreenSize()
		{
			if (DDConfig.DisplayIndex == -2)
			{
				UpdateActiveScreen();
			}
		}

		private static void UpdateActiveScreen()
		{
			I2Point screenCenter;

			{
				DDWin32.POINT p;

				p.X = 0;
				p.Y = 0;

				DDWin32.W_ClientToScreen(DDWin32.GetMainWindowHandle(), out p);

				int l = p.X;
				int t = p.Y;
				int w = DDGround.RealScreen_W;
				int h = DDGround.RealScreen_H;

				screenCenter = new I2Point(l + w / 2, t + h / 2);
			}

			foreach (I4Rect monitor in DDWin32.GetAllMonitor())
			{
				if (
					monitor.L <= screenCenter.X && screenCenter.X < monitor.R &&
					monitor.T <= screenCenter.Y && screenCenter.Y < monitor.B
					)
				{
					DDGround.MonitorRect = monitor;
					break;
				}
			}
		}

		private static void P_SetFullScreen()
		{
			int w = DDGround.MonitorRect.W;
			int h = (DDConsts.Screen_H * DDGround.MonitorRect.W) / DDConsts.Screen_W;

			if (DDGround.MonitorRect.H < h)
			{
				h = DDGround.MonitorRect.H;
				w = (DDConsts.Screen_W * DDGround.MonitorRect.H) / DDConsts.Screen_H;

				if (DDGround.MonitorRect.W < w)
					throw new DDError();
			}
			P_SetScreenSize(DDGround.MonitorRect.W, DDGround.MonitorRect.H);

			DDGround.RealScreenDraw_L = (DDGround.MonitorRect.W - w) / 2;
			DDGround.RealScreenDraw_T = (DDGround.MonitorRect.H - h) / 2;
			DDGround.RealScreenDraw_W = w;
			DDGround.RealScreenDraw_H = h;
		}

		private static void P_SetScreenSize(int w, int h)
		{
			if (
				w < DDConsts.Screen_W_Min || DDConsts.Screen_W_Max < w ||
				h < DDConsts.Screen_H_Min || DDConsts.Screen_H_Max < h
				)
				throw new DDError();

			DDGround.RealScreenDraw_W = -1; // 無効化

			if (DDGround.RealScreen_W != w || DDGround.RealScreen_H != h)
			{
				DDGround.RealScreen_W = w;
				DDGround.RealScreen_H = h;

				P2_SetScreenSize(w, h);
				PostSetScreenSize(w, h);
			}
		}

		private static void P2_SetScreenSize(int w, int h)
		{
			LiteStatusDlg.StartDisplay("ゲーム画面の位置とサイズを調整しています...");

			bool mdm = DDUtils.GetMouseDispMode();

			foreach (DDSubScreen subScreen in DDSubScreenUtils.SubScreens)
				subScreen.WasLoaded = subScreen.IsLoaded();

			//DDDerivationUtils.UnloadAll(); // moved -> DDPictureUtils.UnloadAll
			DDPictureUtils.UnloadAll();
			DDSubScreenUtils.UnloadAll();
			DDFontUtils.UnloadAll();
			//DDSoundUtils.UnloadAll(); // 不要

			if (DX.SetGraphMode(w, h, 32) != DX.DX_CHANGESCREEN_OK)
				throw new DDError();

			DX.SetDrawScreen(DX.DX_SCREEN_BACK); // DDSubScreenUtils.CurrDrawScreenHandle にするべきだが、このフレームだけの問題なので、無難なところで DX_SCREEN_BACK にしておく。
			DX.SetDrawMode(DDConsts.DEFAULT_DX_DRAWMODE);

			DDUtils.SetMouseDispMode(mdm);

			DDGround.SystemTasks.Delay(1, DDPictureUtils.TouchGlobally); // ウィンドウ位置調整・初回描画を優先するため、遅延する。
			//DDPictureUtils.TouchGlobally(); // old
			//DDTouch.Touch(); // old
			DDSubScreenUtils.DrawDummyScreenAll();

			LiteStatusDlg.EndDisplayDelay();
		}

		private static void PostSetScreenSize(int w, int h)
		{
			if (DDConfig.DisplayIndex == -2)
			{
				// 注意：DDGround.MonitorRect.L_T は -1 以下の場合もある。

				int l = (DDGround.MonitorRect.W - w) / 2;
				int t = (DDGround.MonitorRect.H - h) / 2;

				l = Math.Max(0, l);
				t = Math.Max(0, t);

				SetScreenPosition(DDGround.MonitorRect.L + l, DDGround.MonitorRect.T + t);
			}
			else
			{
				if (DDGround.MonitorRect.W == w && DDGround.MonitorRect.H == h)
				{
					SetScreenPosition(DDGround.MonitorRect.L, DDGround.MonitorRect.T);
				}
			}
		}

		private static void SetScreenPosition(int l, int t)
		{
			DX.SetWindowPosition(l, t);

			DDWin32.POINT p;

			p.X = 0;
			p.Y = 0;

			DDWin32.W_ClientToScreen(DDWin32.GetMainWindowHandle(), out p);

			int pToTrgX = l - (int)p.X;
			int pToTrgY = t - (int)p.Y;

			DX.SetWindowPosition(l + pToTrgX, t + pToTrgY);
		}

		private static int LastKeepMainScreenFrame = -1;

		public static void KeepMainScreen()
		{
			// once per frame
			{
				if (DDEngine.ProcFrame <= LastKeepMainScreenFrame)
					return;

				LastKeepMainScreenFrame = DDEngine.ProcFrame;
			}

			DDSubScreen tmp = DDGround.LastMainScreen;
			DDGround.LastMainScreen = DDGround.KeptMainScreen;
			DDGround.KeptMainScreen = tmp;
		}
	}
}
