using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;
using Charlotte.Games;
using Charlotte.Novels.Surfaces;

namespace Charlotte.Novels
{
	public class Novel : IDisposable
	{
		public NovelStatus Status = new NovelStatus(); // 軽量な仮設オブジェクト

		// <---- prm

		public bool RequestReturnToTitleMenu = false;

		// <---- ret

		public static Novel I;

		public Novel()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public ScenarioPage CurrPage;
		public int SelectedSystemButtonIndex = -1; // -1 == システムボタン未選択, 0～ == システムボタン選択中(値とボタンの対応は実装で確認してね)
		public bool SkipMode;
		public bool AutoMode;
		public bool BacklogMode;

		public int DispSubtitleCharCount;
		public int DispCharCount;
		public int DispPageEndedCount;
		public bool DispFastMode;

		public void Perform()
		{
			DDUtils.SetMouseDispMode(true);

			// reset
			{
				this.SkipMode = false;
				this.AutoMode = false;
				this.BacklogMode = false;

				Surface_MessageWindow.Hide = false;
				Surface_SystemButtons.Hide = false;
			}

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain();

		restartCurrPage:
			this.CurrPage = this.Status.Scenario.Pages[this.Status.CurrPageIndex];

			foreach (ScenarioCommand command in this.CurrPage.Commands)
				command.Invoke();

			this.DispSubtitleCharCount = 0;
			this.DispCharCount = 0;
			this.DispPageEndedCount = 0;
			this.DispFastMode = false;

			DDEngine.FreezeInput();

			for (; ; )
			{
				bool nextPageFlag = false;

				// 入力：シナリオを進める。(マウスホイール)
				if (DDMouse.Rot < 0)
				{
					this.CancelSkipAutoMode();

					if (this.DispPageEndedCount < NovelConsts.NEXT_PAGE_INPUT_INTERVAL) // ? ページ表示_未完了 -> ページ表示_高速化
					{
						this.DispFastMode = true;
					}
					else // ? ページ表示_完了 -> 次ページ
					{
						if (!this.Status.HasSelect())
							nextPageFlag = true;
					}
					DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);
				}

				// 入力：シナリオを進める。(マウスホイール_以外)
				if (
					DDMouse.L.GetInput() == -1 && this.SelectedSystemButtonIndex == -1 || // システムボタン以外を左クリック
					DDInput.A.GetInput() == 1
					)
				{
					if (this.DispPageEndedCount < NovelConsts.NEXT_PAGE_INPUT_INTERVAL) // ? ページ表示_未完了 -> ページ表示_高速化
					{
						this.DispFastMode = true;
					}
					else // ? ページ表示_完了 -> 次ページ
					{
						if (!this.Status.HasSelect()) // ? 選択肢表示中ではない。
						{
							nextPageFlag = true;
						}
						else // ? 選択肢表示中
						{
							int index = this.Status.GetSelect().GetMouseFocusedIndex();

							if (index != -1) // 選択中の選択肢へ進む
							{
								string scenarioName = this.Status.GetSelect().Options[index].ScenarioName;

								this.Status.Scenario = new Scenario(scenarioName);
								this.Status.CurrPageIndex = 0;
								this.Status.RemoveSelect(); // 選択肢_終了

								goto restartCurrPage;
							}
						}
					}
				}

				if (this.SkipMode)
					if (!this.Status.HasSelect())
						if (1 <= this.DispPageEndedCount)
							nextPageFlag = true;

				if (this.AutoMode)
					if (!this.Status.HasSelect())
						if (NovelConsts.AUTO_NEXT_PAGE_INTERVAL <= this.DispPageEndedCount)
							nextPageFlag = true;

				if (nextPageFlag) // 次ページ
				{
					// スキップモード時はページを進める毎にエフェクトを強制終了する。
					if (this.SkipMode)
						foreach (Surface surface in this.Status.Surfaces)
							surface.Act.Flush();

					this.Status.CurrPageIndex++;

					if (this.Status.Scenario.Pages.Count <= this.Status.CurrPageIndex)
						break;

					goto restartCurrPage;
				}

				// 入力：過去ログ
				if (
					DDInput.DIR_4.GetInput() == 1 ||
					0 < DDMouse.Rot
					)
				{
					this.Backlog();
				}

				// 入力：鑑賞モード
				if (
					DDMouse.R.GetInput() == -1 ||
					DDInput.B.GetInput() == 1
					)
				{
					this.Appreciate();
				}

				// 入力：システムボタン
				if (DDMouse.L.GetInput() == -1 && this.SelectedSystemButtonIndex != -1) // システムボタンを左クリック
				{
					switch (this.SelectedSystemButtonIndex)
					{
						case 0: this.SkipMode = !this.SkipMode; break;
						case 1: this.AutoMode = !this.AutoMode; break;
						case 2: this.Backlog(); break;
						case 3: this.SystemMenu(); break;

						default:
							throw null; // never
					}
					if (this.RequestReturnToTitleMenu)
					{
						break;
					}
				}

				if (
					this.CurrPage.Subtitle.Length < this.DispSubtitleCharCount &&
					this.CurrPage.Text.Length < this.DispCharCount
					)
					this.DispPageEndedCount++;

				if (this.SkipMode)
				{
					this.DispSubtitleCharCount += 8;
					this.DispCharCount += 8;
				}
				else if (this.DispFastMode)
				{
					this.DispSubtitleCharCount += Ground.I.NovelMessageSpeed;
					this.DispCharCount += Ground.I.NovelMessageSpeed;
				}
				else
				{
					int speed = (NovelConsts.MESSAGE_SPEED_MAX + NovelConsts.MESSAGE_SPEED_MIN) - Ground.I.NovelMessageSpeed;

					if (DDEngine.ProcFrame % speed == 0)
						this.DispSubtitleCharCount++;

					if (DDEngine.ProcFrame % (speed + 1) == 0)
						this.DispCharCount++;
				}
				DDUtils.ToRange(ref this.DispSubtitleCharCount, 0, SCommon.IMAX);
				DDUtils.ToRange(ref this.DispCharCount, 0, SCommon.IMAX);

				// ====
				// 描画ここから
				// ====

				this.DrawSurfaces();

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();

				// ★★★ ゲームループの終わり ★★★
			}

			DDCurtain.SetCurtain(30, -1.0);
			DDMusicUtils.Fadeout();

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.DrawSurfaces();

				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();

			DDUtils.SetMouseDispMode(false);

			// ★★★ end of Perform() ★★★
		}

		/// <summary>
		/// スキップモード・オートモードを解除する。
		/// 両モード中、何か入力があれば解除されるのが自然だと思う。
		/// どこで解除しているか分かるようにメソッド化した。
		/// </summary>
		public void CancelSkipAutoMode()
		{
			this.SkipMode = false;
			this.AutoMode = false;
		}

		private Func<bool> SortSurfaces = null;

		/// <summary>
		/// 主たる画面描画
		/// 色々な場所(モード)から呼び出されるだろう。
		/// </summary>
		public void DrawSurfaces()
		{
			DDCurtain.DrawCurtain(); // 画面クリア

			if (this.SortSurfaces == null)
				this.SortSurfaces = SCommon.Supplier(this.E_SortSurfaces());

			for (int c = 0; c < 10; c++) // rough limit
				this.SortSurfaces();

			foreach (Surface surface in Novel.I.Status.Surfaces) // キャラクタ・オブジェクト・壁紙
				if (!surface.Act.Draw())
					surface.Draw();

			this.Status.Surfaces.RemoveAll(surface => surface.DeadFlag);
		}

		private IEnumerable<bool> E_SortSurfaces()
		{
			for (; ; )
			{
				for (int index = 1; index < this.Status.Surfaces.Count; index++)
				{
					if (0 < Comp_SurfaceZOrder(this.Status.Surfaces[index - 1], this.Status.Surfaces[index]))
					{
						SCommon.Swap(this.Status.Surfaces, index - 1, index);
					}
					yield return true;
				}
				yield return true; // ループ内で実行されない場合を想定
			}
		}

		/// <summary>
		/// サーフェスの比較
		/// Z-オーダー順
		/// </summary>
		/// <param name="a">左のサーフェス</param>
		/// <param name="b">右のサーフェス</param>
		/// <returns>比較結果</returns>
		private static int Comp_SurfaceZOrder(Surface a, Surface b)
		{
			int ret = a.Z - b.Z;
			if (ret != 0)
				return ret;

			ret = SCommon.Comp(a.X, b.X);
			if (ret != 0)
				return ret;

			ret = SCommon.Comp(a.Y, b.Y);
			return ret;
		}

		/// <summary>
		/// 過去ログ
		/// </summary>
		private void Backlog()
		{
			List<string> logLines = new List<string>();

			for (int index = 0; index < this.Status.CurrPageIndex; index++)
				foreach (string line in this.Status.Scenario.Pages[index].Lines)
					logLines.Add(line);

			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			this.BacklogMode = true;
			int backIndex = 0;

			for (; ; )
			{
				if (
					DDMouse.L.GetInput() == -1 ||
					DDInput.A.GetInput() == 1
					)
					break;

				if (
					DDInput.DIR_8.IsPound() ||
					0 < DDMouse.Rot
					)
					backIndex++;

				if (
					DDInput.DIR_2.IsPound() ||
					DDMouse.Rot < 0
					)
					backIndex--;

				if (
					DDInput.DIR_6.GetInput() == 1
					)
					backIndex = -1;

				DDUtils.ToRange(ref backIndex, -1, logLines.Count - 1);

				if (backIndex < 0)
					break;

				this.DrawSurfaces();
				DDCurtain.DrawCurtain(-0.8);

				for (int c = 1; c <= 17; c++)
				{
					int i = logLines.Count - backIndex - c;

					if (0 <= i)
					{
						DDFontUtils.DrawString(8, DDConsts.Screen_H - c * 30 - 8, logLines[i], DDFontUtils.GetFont("Kゴシック", 16));
					}
				}
				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			this.BacklogMode = false;
		}

		/// <summary>
		/// 鑑賞モード
		/// </summary>
		private void Appreciate()
		{
			Surface_MessageWindow.Hide = true;
			Surface_SystemButtons.Hide = true;
			Surface_Select.Hide = true;

			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			for (; ; )
			{
				if (
					DDMouse.L.GetInput() == -1 ||
					DDMouse.R.GetInput() == -1 ||
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1
					)
					break;

				this.DrawSurfaces();
				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput(NovelConsts.SHORT_INPUT_SLEEP);

			Surface_MessageWindow.Hide = false; // restore
			Surface_SystemButtons.Hide = false; // restore
			Surface_Select.Hide = false; // restore
		}

		private static DDSubScreen SystemMenu_KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H); // 使用後は Unload すること。

		/// <summary>
		/// システムメニュー画面
		/// </summary>
		private void SystemMenu()
		{
			DDMain.KeepMainScreen();
			SCommon.Swap(ref DDGround.KeptMainScreen, ref SystemMenu_KeptMainScreen); // 使用後は Unload すること。

			SimpleMenu simpleMenu = new SimpleMenu()
			{
				BorderColor = new I3Color(0, 64, 0),
				WallDrawer = () =>
				{
					DDDraw.DrawSimple(SystemMenu_KeptMainScreen.ToPicture(), 0, 0);

					DDDraw.SetAlpha(0.5);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 4, DDConsts.Screen_W, DDConsts.Screen_H / 2);
					DDDraw.Reset();
				},
			};

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(
					selectIndex,
					100,
					180,
					50,
					24,
					"システムメニュー",
					new string[]
					{
						"設定",
						"タイトルに戻る",
						"ゲームに戻る",
					});

				switch (selectIndex)
				{
					case 0:
						using (new SettingMenu()
						{
							SimpleMenu = new SimpleMenu()
							{
								BorderColor = new I3Color(0, 64, 0),
								WallDrawer = () =>
								{
									DDDraw.DrawSimple(SystemMenu_KeptMainScreen.ToPicture(), 0, 0);
									DDCurtain.DrawCurtain(-0.5);
								},
							},
						})
						{
							SettingMenu.I.Perform();
						}
						break;

					case 1:
						if (new Confirm().Perform("タイトル画面に戻ります。", "はい", "いいえ") == 0)
						{
							this.RequestReturnToTitleMenu = true;
							goto endLoop;
						}
						break;

					case 2:
						goto endLoop;

					default:
						throw null; // never
				}
				//DDEngine.EachFrame(); // 不要
			}
		endLoop:
			DDEngine.FreezeInput(NovelConsts.LONG_INPUT_SLEEP);
			SystemMenu_KeptMainScreen.Unload();
		}
	}
}
