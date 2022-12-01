using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;

namespace Charlotte.Games
{
	public class SettingMenu : IDisposable
	{
		public static SettingMenu I;

		public SettingMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		private enum Mode_e
		{
			基本設定 = 1,
			拡張設定,
			画面サイズ設定,
			ボタン設定,
			キー設定,
			END,
		}

		private Action DrawWall;
		private Mode_e Mode;

		public void Perform(Action a_drawWall)
		{
			this.DrawWall = a_drawWall;
			this.Mode = Mode_e.基本設定;

			DDEngine.FreezeInput();

			do
			{
				switch (this.Mode)
				{
					case Mode_e.基本設定: this.基本設定(); break;
					case Mode_e.拡張設定: this.拡張設定(); break;
					case Mode_e.画面サイズ設定: this.画面サイズ設定(); break;
					case Mode_e.ボタン設定: this.ボタンまたはキー設定(false); break;
					case Mode_e.キー設定: this.ボタンまたはキー設定(true); break;

					default:
						throw null; // never
				}
			}
			while (this.Mode != Mode_e.END);

			DDEngine.FreezeInput();
		}

		private void 基本設定()
		{
			DDSE[] seSamples = Ground.I.SE.テスト用s;

			DDEngine.FreezeInput();

			for (; ; )
			{
				// ====
				// 入力判定ここから
				// ====

				if (
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1 ||
					DDMouse.R.GetInput() == -1
					)
				{
					this.Mode = Mode_e.END;
					break;
				}

				if (DDMouse.L.GetInput() == -1)
				{
					if (this.GetTabTitleCrash_拡張設定().IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
					{
						this.Mode = Mode_e.拡張設定;
						break;
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_フルスクリーン)
					{
						DDMain.SetFullScreen();
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_ウィンドウ)
					{
						const int def_screen_w = DDConsts.Screen_W / 2;
						const int def_screen_h = DDConsts.Screen_H / 2;

						DDMain.SetScreenSize(def_screen_w, def_screen_h);
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_デフォルトに戻す)
					{
						this.デフォルトに戻す();
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_戻る)
					{
						this.Mode = Mode_e.END;
						break;
					}
				}

				// ====
				// 入力判定ここまで
				// ====

				// ====
				// 描画ここから
				// ====

				this.BeforeDrawContents();

				DDDraw.DrawSimple(Ground.I.Picture.基本設定枠, 0, 0);

				this.DrawTabTitles(false);

				this.DrawPrompt(100, 250, "画面モード");
				this.DrawPrompt(100, 380, "ＢＧＭ音量");
				this.DrawPrompt(100, 510, "ＳＥ音量");
				this.DrawPrompt(100, 640, "メッセージ表示速度");
				this.DrawPrompt(100, 770, "メッセージウィンドウ透明度");

				bool fullScreenFlag =
					DDGround.RealScreen_W == DDGround.MonitorRect.W &&
					DDGround.RealScreen_H == DDGround.MonitorRect.H;

				this.DrawButton(1100, 280, Ground.I.Picture.SettingButton_フルスクリーン, fullScreenFlag);
				this.DrawButton(1550, 280, Ground.I.Picture.SettingButton_ウィンドウ, !fullScreenFlag);
				this.DrawTrackBar(1325, 410, "小", "大", DDGround.MusicVolume, volume =>
				{
					DDGround.MusicVolume = volume;
					DDMusicUtils.UpdateVolume();
				});
				this.DrawTrackBar(1325, 540, "小", "大", DDGround.SEVolume, volume =>
				{
					DDGround.SEVolume = volume;
					//DDSEUtils.UpdateVolume(); // v_20210215 -- メソッド終了時に全て更新する。

					foreach (DDSE se in seSamples)
						se.UpdateVolume();
				},
				() =>
				{
					DDUtils.Random.ChooseOne(seSamples).Play();
				});
				this.DrawTrackBar(1325, 670, "遅い", "速い",
					DDUtils.RateAToB(GameConsts.MESSAGE_SPEED_MIN, GameConsts.MESSAGE_SPEED_MAX, Ground.I.MessageSpeed),
					value => Ground.I.MessageSpeed = SCommon.ToInt(
						DDUtils.AToBRate(GameConsts.MESSAGE_SPEED_MIN, GameConsts.MESSAGE_SPEED_MAX, value)
						)
					);
				this.DrawTrackBar(1325, 800, "透明", "不透明",
					DDUtils.RateAToB(0, 100, Ground.I.MessageWindow_A_Pct),
					value => Ground.I.MessageWindow_A_Pct = SCommon.ToInt(
						DDUtils.AToBRate(0, 100, value)
						)
					);

				this.DrawUnderButtons();

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			DDSEUtils.UpdateVolume(); // v_20210215
		}

		private void 拡張設定()
		{
			DDEngine.FreezeInput();

			for (; ; )
			{
				// ====
				// 入力判定ここから
				// ====

				if (
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1 ||
					DDMouse.R.GetInput() == -1
					)
				{
					this.Mode = Mode_e.END;
					break;
				}

				if (DDMouse.L.GetInput() == -1)
				{
					if (this.GetTabTitleCrash_基本設定().IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
					{
						this.Mode = Mode_e.基本設定;
						break;
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_ウィンドウサイズ設定)
					{
						this.Mode = Mode_e.画面サイズ設定;
						break;
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_ゲームパッドのボタン設定 && 1 <= DDPad.GetPadCount())
					{
						this.Mode = Mode_e.ボタン設定;
						break;
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_キーボードのキー設定)
					{
						this.Mode = Mode_e.キー設定;
						break;
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_デフォルトに戻す)
					{
						this.デフォルトに戻す();
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_戻る)
					{
						this.Mode = Mode_e.END;
						break;
					}
				}

				// ====
				// 入力判定ここまで
				// ====

				// ====
				// 描画ここから
				// ====

				this.BeforeDrawContents();

				DDDraw.DrawSimple(Ground.I.Picture.拡張設定枠, 0, 0);

				this.DrawTabTitles(true);

				this.DrawButton(960, 330, Ground.I.Picture.SettingButton_ウィンドウサイズ設定, true);
				this.DrawButton(960, 530, Ground.I.Picture.SettingButton_ゲームパッドのボタン設定, 1 <= DDPad.GetPadCount());
				this.DrawButton(960, 730, Ground.I.Picture.SettingButton_キーボードのキー設定, true);

				this.DrawUnderButtons();

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
		}

		private void 画面サイズ設定()
		{
			DDEngine.FreezeInput();

			for (; ; )
			{
				// ====
				// 入力判定ここから
				// ====

				if (
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1 ||
					DDMouse.R.GetInput() == -1
					)
				{
					this.Mode = Mode_e.拡張設定;
					break;
				}

				if (DDMouse.L.GetInput() == -1)
				{
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_960x540) DDMain.SetScreenSize(960, 540);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1080x607) DDMain.SetScreenSize(1080, 607);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1200x675) DDMain.SetScreenSize(1200, 675);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1320x742) DDMain.SetScreenSize(1320, 742);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1440x810) DDMain.SetScreenSize(1440, 810);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1560x877) DDMain.SetScreenSize(1560, 877);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1680x945) DDMain.SetScreenSize(1680, 945);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1800x1012) DDMain.SetScreenSize(1800, 1012);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_1920x1080) DDMain.SetScreenSize(1920, 1080);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_2040x1147) DDMain.SetScreenSize(2040, 1147);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_2160x1215) DDMain.SetScreenSize(2160, 1215);
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_2280x1282) DDMain.SetScreenSize(2280, 1282);

					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_フルスクリーン画面に合わせる)
					{
						DDMain.SetScreenSize(DDGround.MonitorRect.W, DDGround.MonitorRect.H);
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_フルスクリーン縦横比を維持する)
					{
						DDMain.SetFullScreen();
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_デフォルトに戻す)
					{
						this.デフォルトに戻す();
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_戻る)
					{
						this.Mode = Mode_e.拡張設定;
						break;
					}
				}

				// ====
				// 入力判定ここまで
				// ====

				// ====
				// 描画ここから
				// ====

				this.BeforeDrawContents();

				DDDraw.DrawSimple(Ground.I.Picture.詳細設定枠, 0, 0);

				DrawTabTitle(610, 70, "ウィンドウサイズ設定", true);

				this.DrawButton(300, 300, Ground.I.Picture.SettingButton_960x540, true);
				this.DrawButton(740, 300, Ground.I.Picture.SettingButton_1080x607, true);
				this.DrawButton(1180, 300, Ground.I.Picture.SettingButton_1200x675, true);
				this.DrawButton(1620, 300, Ground.I.Picture.SettingButton_1320x742, true);

				this.DrawButton(300, 420, Ground.I.Picture.SettingButton_1440x810, true);
				this.DrawButton(740, 420, Ground.I.Picture.SettingButton_1560x877, true);
				this.DrawButton(1180, 420, Ground.I.Picture.SettingButton_1680x945, true);
				this.DrawButton(1620, 420, Ground.I.Picture.SettingButton_1800x1012, true);

				this.DrawButton(300, 540, Ground.I.Picture.SettingButton_1920x1080, true);
				this.DrawButton(740, 540, Ground.I.Picture.SettingButton_2040x1147, true);
				this.DrawButton(1180, 540, Ground.I.Picture.SettingButton_2160x1215, true);
				this.DrawButton(1620, 540, Ground.I.Picture.SettingButton_2280x1282, true);

				this.DrawButton(960, 660, Ground.I.Picture.SettingButton_フルスクリーン画面に合わせる, true);
				this.DrawButton(960, 780, Ground.I.Picture.SettingButton_フルスクリーン縦横比を維持する, true);

				this.DrawUnderButtons();

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
		}

		private void ボタンまたはキー設定(bool キー設定Flag)
		{
			DDEngine.FreezeInput();

			for (; ; )
			{
				// ====
				// 入力判定ここから
				// ====

				if (
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1 ||
					DDMouse.R.GetInput() == -1
					)
				{
					this.Mode = Mode_e.拡張設定;
					break;
				}

				if (DDMouse.L.GetInput() == -1)
				{
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_デフォルトに戻す)
					{
						this.デフォルトに戻す();
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_戻る)
					{
						this.Mode = Mode_e.拡張設定;
						break;
					}
				}

				// ====
				// 入力判定ここまで
				// ====

				// ====
				// 描画ここから
				// ====

				this.BeforeDrawContents();

				DDDraw.DrawSimple(Ground.I.Picture.詳細設定枠, 0, 0);

				if (キー設定Flag)
					DrawTabTitle(610, 70, "キーボードのキー設定", true);
				else
					DrawTabTitle(500, 70, "ゲームパッドのボタン設定", true);

				{
					int c = 0;

					PrintPadButtonKeySetting(キー設定Flag, c++, "上", DDInput.DIR_8);
					PrintPadButtonKeySetting(キー設定Flag, c++, "下", DDInput.DIR_2);
					PrintPadButtonKeySetting(キー設定Flag, c++, "左", DDInput.DIR_4);
					PrintPadButtonKeySetting(キー設定Flag, c++, "右", DDInput.DIR_6);
					PrintPadButtonKeySetting(キー設定Flag, c++, "決定", DDInput.A);
					PrintPadButtonKeySetting(キー設定Flag, c++, "キャンセル", DDInput.B);
					PrintPadButtonKeySetting(キー設定Flag, c++, "メッセージ・スキップ", DDInput.L);
				}

				this.DrawUnderButtons();

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
		}

		// ◆◆◆
		// ◆◆◆ コンテンツ描画系メソッドここから
		// ◆◆◆

		private DDPicture LastHoveringButton;
		private DDCrash? LastHoveringTrackBar;
		private DDCrash? LastActiveTrackBar;

		private void BeforeDrawContents()
		{
			if (DDMouse.L.GetInput() == 1) // ? 左ボタン押下開始
				this.LastActiveTrackBar = this.LastHoveringTrackBar;

			if (DDMouse.L.GetInput() == 0) // ? 左ボタン押下終了
				this.LastActiveTrackBar = null;

			this.LastHoveringButton = null;
			this.LastHoveringTrackBar = null;

			this.DrawWall();
		}

		private void DrawTabTitles(bool 拡張設定Flag)
		{
			DrawTabTitle(155, 70, "基本設定", !拡張設定Flag);
			DrawTabTitle(660, 70, "拡張設定", 拡張設定Flag);
		}

		private void DrawTabTitle(int x, int y, string line, bool activeFlag)
		{
			DDFontUtils.DrawString(
				x,
				y,
				line,
				DDFontUtils.GetFont("Kゴシック", 70),
				false,
				activeFlag ? new I3Color(100, 255, 255) : new I3Color(150, 150, 150),
				activeFlag ? new I3Color(50, 100, 100) : new I3Color(100, 100, 100)
				);
		}

		private DDCrash GetTabTitleCrash_基本設定()
		{
			return DDCrashUtils.Rect(D4Rect.LTRB(50, 50, 550, 150));
		}

		private DDCrash GetTabTitleCrash_拡張設定()
		{
			return DDCrashUtils.Rect(D4Rect.LTRB(550, 50, 1050, 150));
		}

		private void DrawUnderButtons()
		{
			this.DrawButton(320, 950, Ground.I.Picture.SettingButton_デフォルトに戻す, true);
			this.DrawButton(1630, 950, Ground.I.Picture.SettingButton_戻る, true);
		}

		private void DrawPrompt(int x, int y, string line)
		{
			DDFontUtils.DrawString(
				x,
				y,
				line,
				DDFontUtils.GetFont("Kゴシック", 50),
				false,
				new I3Color(255, 255, 255),
				new I3Color(0, 0, 100)
				);
		}

		private bool LastButtonHoveringFlag;

		private void DrawButton(int x, int y, DDPicture picture, bool activeFlag)
		{
			DDDraw.SetAlpha(activeFlag ? 1.0 : 0.5);
			DDDraw.DrawBegin(picture, x, y);
			DDCrash drawedCrash = DDDraw.DrawGetCrash();
			DDDraw.DrawEnd();
			DDDraw.Reset();

			if (drawedCrash.IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
			{
				this.LastHoveringButton = picture;
				this.LastButtonHoveringFlag = true;
			}
			else
			{
				this.LastButtonHoveringFlag = false;
			}
		}

		private void DrawTrackBar(int x, int y, string lwLabel, string hiLabel, double rate, Action<double> changed, Action pulse = null)
		{
			DDDraw.DrawBegin(Ground.I.Picture.TrackBar, x, y);
			DDCrash drawedCrash = DDDraw.DrawGetCrash();
			DDDraw.DrawEnd();

			DDPrint.SetPrint(x - Ground.I.Picture.TrackBar.Get_W() / 2 - lwLabel.Length * 32, y - 15, 40);
			DDPrint.Print(lwLabel);

			DDPrint.SetPrint(x + Ground.I.Picture.TrackBar.Get_W() / 2, y - 15, 40);
			DDPrint.Print(hiLabel);

			double span = Ground.I.Picture.TrackBar.Get_W() - Ground.I.Picture.TrackBar_つまみ.Get_W();
			span /= 2;
			double xMin = x - span;
			double xMax = x + span;

			double xつまみ = DDUtils.AToBRate(xMin, xMax, rate);

			DDDraw.DrawCenter(Ground.I.Picture.TrackBar_つまみ, xつまみ, y);

			if (drawedCrash.IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
			{
				this.LastHoveringTrackBar = drawedCrash;
			}
			if (
				this.LastActiveTrackBar != null &&
				this.LastActiveTrackBar.Value.IsCrashed(DDCrashUtils.Point(new D2Point(x, y)))
				)
			{
				double rateNew = DDUtils.RateAToB(xMin, xMax, DDMouse.X);
				DDUtils.ToRange(ref rateNew, 0.0, 1.0);

				if (SCommon.MICRO < Math.Abs(rate - rateNew))
					changed(rateNew);

				if (pulse != null)
				{
					const int PULSE_FRM = 60;

					if (DDEngine.ProcFrame % PULSE_FRM == 0)
						pulse();
				}
			}
		}

		private void デフォルトに戻す()
		{
			{
				const int def_screen_w = DDConsts.Screen_W / 2;
				const int def_screen_h = DDConsts.Screen_H / 2;

				DDMain.SetScreenSize(def_screen_w, def_screen_h);
			}

			DDGround.MusicVolume = 0.45;
			DDGround.SEVolume = 0.45;
			Ground.I.MessageSpeed = GameConsts.MESSAGE_SPEED_DEF;
			Ground.I.MessageWindow_A_Pct = GameConsts.MESSAGE_WINDOW_A_PCT_DEF;

			// 設定値変更を反映
			DDMusicUtils.UpdateVolume();
			DDSEUtils.UpdateVolume();

			// ボタン設定
			{
				DDInput.DIR_2.BtnIds = new int[] { 0 };
				DDInput.DIR_4.BtnIds = new int[] { 1 };
				DDInput.DIR_6.BtnIds = new int[] { 2 };
				DDInput.DIR_8.BtnIds = new int[] { 3 };
				DDInput.A.BtnIds = new int[] { 4 };
				DDInput.B.BtnIds = new int[] { 7 };
				DDInput.L.BtnIds = new int[] { 10 };
			}

			// キー設定
			{
				DDInput.DIR_2.KeyIds = new int[] { DX.KEY_INPUT_DOWN };
				DDInput.DIR_4.KeyIds = new int[] { DX.KEY_INPUT_LEFT };
				DDInput.DIR_6.KeyIds = new int[] { DX.KEY_INPUT_RIGHT };
				DDInput.DIR_8.KeyIds = new int[] { DX.KEY_INPUT_UP };
				DDInput.A.KeyIds = new int[] { DX.KEY_INPUT_RETURN, DX.KEY_INPUT_Z };
				DDInput.B.KeyIds = new int[] { DX.KEY_INPUT_DELETE, DX.KEY_INPUT_X };
				DDInput.L.KeyIds = new int[] { DX.KEY_INPUT_LCONTROL, DX.KEY_INPUT_RCONTROL };
			}
		}

		private void PrintPadButtonKeySetting(bool キー設定Flag, int padButtonIndex, string title, DDInput.Button button)
		{
			int y = 230 + padButtonIndex * 90;

			Func<DDInput.Button, string> getSetting;

			{
				Func<string, string> w = s => !string.IsNullOrEmpty(s) ? s : "割り当てナシ";

				if (キー設定Flag)
					getSetting = btn => w(string.Join(" , ", btn.KeyIds.Select(keyId => SimpleMenu.GetKeyName(keyId))));
				else
					getSetting = btn => w(string.Join(" , ", btn.BtnIds.Select(btnId => SimpleMenu.GetPadButtonName(btnId))));
			}

			this.DrawButton(300, y + 25, Ground.I.Picture.SettingButton_変更, true);

			if (this.LastButtonHoveringFlag && DDMouse.L.GetInput() == -1)
			{
				InputPadButtonKeySetting(キー設定Flag, title, button, getSetting);
			}

			DDFontUtils.DrawString(
				550,
				y,
				"「" + title + "」　＝　" + getSetting(button),
				DDFontUtils.GetFont("Kゴシック", 50),
				false,
				キー設定Flag ? new I3Color(192, 255, 128) : new I3Color(255, 192, 128),
				キー設定Flag ? new I3Color(50, 100, 0) : new I3Color(100, 50, 0)
				);
		}

		private void InputPadButtonKeySetting(bool キー設定Flag, string title, DDInput.Button button, Func<DDInput.Button, string> getSetting)
		{
			DDEngine.FreezeInput();

			button.Backup();

			if (キー設定Flag)
				button.KeyIds = new int[0];
			else
				button.BtnIds = new int[0];

			for (; ; )
			{
				CheckInputPadButtonKey(キー設定Flag, button);

				this.DrawWall();

				DDDraw.DrawSimple(Ground.I.Picture.詳細設定枠, 0, 0);

				if (キー設定Flag)
					DrawTabTitle(480, 70, "キーボードのキー設定 / 変更", true);
				else
					DrawTabTitle(400, 70, "ゲームパッドのボタン設定 / 変更", true);

				DDFontUtils.DrawString(
					100,
					400,
					"「" + title + "」に割り当てる" + (キー設定Flag ? "キー" : "ボタン") + "を押して下さい。(複数可)",
					DDFontUtils.GetFont("Kゴシック", 50),
					false,
					new I3Color(255, 255, 255),
					new I3Color(100, 100, 0)
					);
				DDFontUtils.DrawString(
					100,
					475,
					"入力が終わったら「決定」をクリックして下さい。",
					DDFontUtils.GetFont("Kゴシック", 50),
					false,
					new I3Color(255, 255, 255),
					new I3Color(100, 100, 0)
					);
				DDFontUtils.DrawString(
					100,
					600,
					"現在の割り当て：" + getSetting(button),
					DDFontUtils.GetFont("Kゴシック", 50),
					false,
					new I3Color(255, 255, 255),
					new I3Color(100, 0, 100)
					);

				bool inputDone;

				if (キー設定Flag)
					inputDone = 1 <= button.KeyIds.Length;
				else
					inputDone = 1 <= button.BtnIds.Length;

				this.DrawButton(1200, 950, Ground.I.Picture.SettingButton_決定, inputDone);

				if (this.LastButtonHoveringFlag && DDMouse.L.GetInput() == -1 && inputDone)
					break;

				this.DrawButton(1630, 950, Ground.I.Picture.SettingButton_キャンセル, true);

				if (this.LastButtonHoveringFlag && DDMouse.L.GetInput() == -1 || DDMouse.R.GetInput() == -1)
				{
					button.Restore();
					break;
				}
				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
		}

		private void CheckInputPadButtonKey(bool キー設定Flag, DDInput.Button targetButton)
		{
			DDInput.Button[] buttons = new DDInput.Button[]
			{
				DDInput.DIR_2,
				DDInput.DIR_4,
				DDInput.DIR_6,
				DDInput.DIR_8,
				DDInput.A,
				DDInput.B,
				DDInput.L,
			};

			if (キー設定Flag)
			{
				int pressKeyId = -1;

				foreach (int keyId in SimpleMenu.GetAllKeyId())
					if (DDKey.GetInput(keyId) == 1)
						pressKeyId = keyId;

				if (pressKeyId != -1)
				{
					// 他ボタンとの重複回避
					foreach (DDInput.Button button in buttons)
						button.KeyIds = button.KeyIds.Where(keyId => keyId != pressKeyId).ToArray();

					targetButton.KeyIds = targetButton.KeyIds.Concat(new int[] { pressKeyId }).ToArray();
				}
			}
			else
			{
				int pressBtnId = -1;

				for (int padId = 0; padId < DDPad.GetPadCount(); padId++)
					for (int btnId = 0; btnId < DDPad.PAD_BUTTON_MAX; btnId++)
						if (DDPad.GetInput(padId, btnId) == 1)
							pressBtnId = btnId;

				if (pressBtnId != -1)
				{
					// 他ボタンとの重複回避
					foreach (DDInput.Button button in buttons)
						button.BtnIds = button.BtnIds.Where(btnId => btnId != pressBtnId).ToArray();

					targetButton.BtnIds = targetButton.BtnIds.Concat(new int[] { pressBtnId }).ToArray();
				}
			}
		}
	}
}
