using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;
using Charlotte.Games.Surfaces;

namespace Charlotte.Games
{
	public class Game : IDisposable
	{
		public GameStatus Status = new GameStatus(); // 軽量な仮設オブジェクト

		// <---- prm

		public static Game I;

		public Game()
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

		public void Perform(bool continueByTitleMenuFlag = false)
		{
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

			if (!continueByTitleMenuFlag)
				foreach (ScenarioCommand command in this.CurrPage.Commands)
					command.Invoke();

			continueByTitleMenuFlag = false;

			this.DispSubtitleCharCount = 0;
			this.DispCharCount = 0;
			this.DispPageEndedCount = 0;
			this.DispFastMode = false;

			DDEngine.FreezeInput();

			for (; ; )
			{
				bool nextPageFlag = false;

				// ★★★ キー押下は 1 マウス押下は -1 で判定する。

				// 入力：シナリオを進める。(マウスホイール)
				if (DDMouse.Rot < 0)
				{
					this.CancelSkipAutoMode();

					if (this.DispPageEndedCount < GameConsts.NEXT_PAGE_INPUT_INTERVAL) // ? ページ表示_未完了 -> ページ表示_高速化
					{
						this.DispFastMode = true;
					}
					else // ? ページ表示_完了 -> 次ページ
					{
						if (!this.Status.HasSelect())
							nextPageFlag = true;
					}
					DDEngine.FreezeInput(GameConsts.SHORT_INPUT_SLEEP);
				}

				// 入力：シナリオを進める。(マウスホイール_以外)
				if (
					DDMouse.L.GetInput() == -1 && this.SelectedSystemButtonIndex == -1 || // システムボタン以外を左クリック
					DDInput.A.GetInput() == 1
					)
				{
					if (this.DispPageEndedCount < GameConsts.NEXT_PAGE_INPUT_INTERVAL) // ? ページ表示_未完了 -> ページ表示_高速化
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
								//this.SkipMode = false; // moved

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
						if (GameConsts.AUTO_NEXT_PAGE_INTERVAL <= this.DispPageEndedCount)
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
					//DDInput.DIR_8.GetInput() == 1 || // 選択肢の選択に反応してしまう。
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
						case 0:
							foreach (Surface surface in this.Status.Surfaces)
								surface.Act.Flush();

							this.SaveMenu();
							break;

						case 1: this.LoadMenu(); break;
						case 2: this.SkipMode = !this.SkipMode; break;
						case 3: this.AutoMode = !this.AutoMode; break;
						case 4: this.Backlog(); break;
						case 5: this.SystemMenu(); break;

						default:
							throw null; // never
					}
					if (this.SystemMenu_ReturnToTitleMenu)
						break;
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
					this.DispSubtitleCharCount += Ground.I.MessageSpeed;
					this.DispCharCount += Ground.I.MessageSpeed;
				}
				else
				{
					int speed = (GameConsts.MESSAGE_SPEED_MAX + GameConsts.MESSAGE_SPEED_MIN) - Ground.I.MessageSpeed;

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

			for (int c = 0; c < 10; c++)
				this.SortSurfaces();

			foreach (Surface surface in Game.I.Status.Surfaces) // キャラクタ・オブジェクト・壁紙
				if (!surface.Act.Draw())
					surface.Draw();

			Game.I.Status.Surfaces.RemoveAll(surface => surface.DeadFlag);
		}

		private IEnumerable<bool> E_SortSurfaces()
		{
			for (; ; )
			{
				for (int index = 1; index < Game.I.Status.Surfaces.Count; index++)
				{
					if (0 < SS_Comp(Game.I.Status.Surfaces[index - 1], Game.I.Status.Surfaces[index]))
					{
						SCommon.Swap(Game.I.Status.Surfaces, index - 1, index);

						if (1 < index)
							index--;
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
		private static int SS_Comp(Surface a, Surface b)
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

			DDEngine.FreezeInput(GameConsts.SHORT_INPUT_SLEEP);

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
						DDFontUtils.DrawString(16, DDConsts.Screen_H - c * 60 - 16, logLines[i], DDFontUtils.GetFont("Kゴシック", 32));
					}
				}
				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput(GameConsts.SHORT_INPUT_SLEEP);

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

			DDEngine.FreezeInput(GameConsts.SHORT_INPUT_SLEEP);

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
			DDEngine.FreezeInput(GameConsts.SHORT_INPUT_SLEEP);

			Surface_MessageWindow.Hide = false; // restore
			Surface_SystemButtons.Hide = false; // restore
			Surface_Select.Hide = false; // restore
		}

		/// <summary>
		/// ゲーム中のセーブ画面
		/// </summary>
		private void SaveMenu()
		{
			using (new SaveOrLoadMenu())
			{
				SaveOrLoadMenu.I.Save(() =>
				{
					DDPicture picture = Ground.I.Picture.Title;

					DDDraw.DrawRect(
						picture,
						DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
						);

					DDCurtain.DrawCurtain(-0.5);
				});
			}
			DDEngine.FreezeInput(GameConsts.LONG_INPUT_SLEEP);
		}

		/// <summary>
		/// ゲーム中のロード画面
		/// </summary>
		private void LoadMenu()
		{
			using (new SaveOrLoadMenu())
			{
				SaveDataSlot sdSlot = SaveOrLoadMenu.I.Load(() =>
				{
					DDPicture picture = Ground.I.Picture.Title;

					DDDraw.DrawRect(
						picture,
						DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
						);

					DDCurtain.DrawCurtain(-0.5);
				});

				if (sdSlot != null)
				{
					this.Status = GameStatus.Deserialize(sdSlot.SerializedGameStatus);
					this.CurrPage = this.Status.Scenario.Pages[this.Status.CurrPageIndex];
					this.DispSubtitleCharCount = 0;
					this.DispCharCount = 0;
					this.DispPageEndedCount = 0;
					this.DispFastMode = false;
				}
			}
			DDEngine.FreezeInput(GameConsts.LONG_INPUT_SLEEP);
		}

		private bool SystemMenu_ReturnToTitleMenu = false;

		/// <summary>
		/// システムメニュー画面
		/// </summary>
		private void SystemMenu()
		{
			this.SystemMenu_ReturnToTitleMenu = false; // reset

			SimpleMenu simpleMenu = new SimpleMenu()
			{
				Color = new I3Color(255, 255, 255),
				BorderColor = new I3Color(0, 64, 0),
				//WallPicture = Ground.I.Picture.Title,
				//WallCurtain = -0.5,
				WallDrawer = () =>
				{
					DDPicture picture = Ground.I.Picture.Title;

					DDDraw.DrawRect(
						picture,
						DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
						);

					DDDraw.SetAlpha(0.5);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 4, DDConsts.Screen_W, DDConsts.Screen_H / 2);
					DDDraw.Reset();
				},
				X = 150,
				Y = DDConsts.Screen_H / 4 + 150,
			};

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(
					selectIndex,
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
						using (new SettingMenu())
						{
							SettingMenu.I.Perform(() =>
							{
								DDPicture picture = Ground.I.Picture.Title;

								DDDraw.DrawRect(
									picture,
									DDUtils.AdjustRectExterior(picture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
									);

								DDCurtain.DrawCurtain(-0.5);
							});
						}
						break;

					case 1:
						if (new Confirm().Perform("タイトル画面に戻ります。", "はい", "いいえ") == 0)
						{
							this.SystemMenu_ReturnToTitleMenu = true;
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
			DDEngine.FreezeInput(GameConsts.LONG_INPUT_SLEEP);
		}
	}
}
