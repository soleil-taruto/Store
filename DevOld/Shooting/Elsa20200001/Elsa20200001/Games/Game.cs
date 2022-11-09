using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Enemies.Tests;
using Charlotte.Games.Scripts;
using Charlotte.Games.Scripts.Tests;
using Charlotte.Games.Shots;
using Charlotte.Games.Walls;

namespace Charlotte.Games
{
	public class Game : IDisposable
	{
		public Script Script = new Script_Bダミー0001(); // 軽量なダミー初期オブジェクト
		public GameStatus Status = new GameStatus();   // 軽量なダミー初期オブジェクト

		// <---- prm

		public static Game I;

		public DDSubScreen Field;
		public DDSubScreen Field_Last;

		public Game()
		{
			I = this;

			this.Field = new DDSubScreen(GameConsts.FIELD_W, GameConsts.FIELD_H);
			this.Field_Last = new DDSubScreen(GameConsts.FIELD_W, GameConsts.FIELD_H);
		}

		public void Dispose()
		{
			this.Field.Dispose();
			this.Field = null;
			this.Field_Last.Dispose();
			this.Field_Last = null;

			I = null;
		}

		public Player Player = new Player();
		public int Frame;
		public bool UserInputDisabled = false;

		public bool RequestReturnToTitleMenu = false;

		public void Perform()
		{
			Func<bool> f_ゴミ回収 = SCommon.Supplier(this.E_ゴミ回収());

			DDUtils.Random = new DDRandom(1u); // 電源パターン確保のため

			this.Player.X = GameConsts.FIELD_W / 4;
			this.Player.Y = GameConsts.FIELD_H / 2;

			// ★★★★★
			// プレイヤー・ステータス反映(ステージ開始時)
			{
				// none
			}

			this.Player.RebornFrame = 1;

			Game.I.Walls.Add(new Wall_Dark());

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain(20);

			DDEngine.FreezeInput();

			for (this.Frame = 0; ; this.Frame++)
			{
				if (!this.Script.EachFrame())
					break;

				if (!this.UserInputDisabled && DDInput.PAUSE.GetInput() == 1) // ポーズ
				{
					DDMusicUtils.Pause();
					this.Pause();

					if (this.Pause_ReturnToTitleMenu)
					{
						break;
					}
					DDMusicUtils.Resume();
				}
				if (this.RequestReturnToTitleMenu)
				{
					break;
				}
				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_RETURN) == 1)
				{
					this.DebugPause();
				}

				// プレイヤー行動
				{
					bool deadOrRebornOrUID = 1 <= this.Player.DeadFrame || 1 <= this.Player.RebornFrame || this.UserInputDisabled;
					bool deadOrUID = 1 <= this.Player.DeadFrame || this.UserInputDisabled;
					double xa = 0.0;
					double ya = 0.0;

					if (!deadOrUID && 1 <= DDInput.DIR_4.GetInput()) // 左移動
					{
						xa = -1.0;
					}
					if (!deadOrUID && 1 <= DDInput.DIR_6.GetInput()) // 右移動
					{
						xa = 1.0;
					}
					if (!deadOrUID && 1 <= DDInput.DIR_8.GetInput()) // 上移動
					{
						ya = -1.0;
					}
					if (!deadOrUID && 1 <= DDInput.DIR_2.GetInput()) // 下移動
					{
						ya = 1.0;
					}

					double speed;

					if (1 <= DDInput.A.GetInput()) // 低速ボタン押下中
					{
						speed = (double)this.Player.SpeedLevel;
					}
					else
					{
						speed = (double)(this.Player.SpeedLevel * 2);
					}

					this.Player.X += xa * speed;
					this.Player.Y += ya * speed;

					DDUtils.ToRange(ref this.Player.X, 0.0, GameConsts.FIELD_W);
					DDUtils.ToRange(ref this.Player.Y, 0.0, GameConsts.FIELD_H);

					if (!deadOrRebornOrUID && 1 <= DDInput.B.GetInput()) // 攻撃ボタン押下中
					{
						this.Player.Shoot();
					}
					if (!deadOrRebornOrUID && DDInput.E.GetInput() == 1) // ボム_ボタン押下
					{
						this.Player.Bomb();
					}

					if (DDInput.C.GetInput() == 1)
					{
						this.Player.SpeedLevel--;
					}
					if (DDInput.D.GetInput() == 1)
					{
						this.Player.SpeedLevel++;
					}
					DDUtils.ToRange(ref this.Player.SpeedLevel, Player.SPEED_LEVEL_MIN, Player.SPEED_LEVEL_MAX);
				}

				//startDead:
				if (1 <= this.Player.DeadFrame) // プレイヤー死亡中の処理
				{
					if (GameConsts.PLAYER_DEAD_FRAME_MAX < ++this.Player.DeadFrame)
					{
						this.Player.DeadFrame = 0;

						if (this.Status.Zanki <= 0) // ? 残機不足
							break;

						this.システム的な敵クリア();

						this.Status.Zanki--;
						this.Player.AttackLevel = Math.Max(0, this.Player.AttackLevel - 1);
						this.Player.RebornFrame = 1;
						goto endDead;
					}
					int frame = this.Player.DeadFrame; // 値域 == 2 ～ GameConsts.PLAYER_DEAD_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_DEAD_FRAME_MAX, frame);

					// ---- Dead

					if (frame == 2) // init
					{
						DDMain.KeepMainScreen();

						foreach (DDScene scene in DDSceneUtils.Create(20))
						{
							DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);

							DDDraw.SetAlpha(0.3 + scene.Rate * 0.3);
							DDDraw.SetBright(1.0, 0.0, 0.0);
							DDDraw.DrawRect(Ground.I.Picture.WhiteBox, new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H));
							DDDraw.Reset();

							DDEngine.EachFrame();
						}
						DDGround.EL.Add(SCommon.Supplier(Effects.PlayerDead(this.Player.X, this.Player.Y)));
					}
				}
			endDead:

				//startReborn:
				if (1 <= this.Player.RebornFrame) // プレイヤー登場中の処理
				{
					if (GameConsts.PLAYER_REBORN_FRAME_MAX < ++this.Player.RebornFrame)
					{
						this.Player.RebornFrame = 0;
						this.Player.InvincibleFrame = 1;
						goto endReborn;
					}
					int frame = this.Player.RebornFrame; // 値域 == 2 ～ GameConsts.PLAYER_REBORN_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_REBORN_FRAME_MAX, frame);

					// ---- Reborn

					if (frame == 2) // init
					{
						this.Player.Reborn_X = -50.0;
						this.Player.Reborn_Y = GameConsts.FIELD_H / 2.0;
					}
					DDUtils.Approach(ref this.Player.Reborn_X, this.Player.X, 0.9 - 0.3 * rate);
					DDUtils.Approach(ref this.Player.Reborn_Y, this.Player.Y, 0.9 - 0.3 * rate);
				}
			endReborn:

				//startInvincible:
				if (1 <= this.Player.InvincibleFrame) // プレイヤー無敵時間中の処理
				{
					if (GameConsts.PLAYER_INVINCIBLE_FRAME_MAX < ++this.Player.InvincibleFrame)
					{
						this.Player.InvincibleFrame = 0;
						goto endInvincible;
					}
					int frame = this.Player.InvincibleFrame; // 値域 == 2 ～ GameConsts.PLAYER_INVINCIBLE_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_INVINCIBLE_FRAME_MAX, frame);

					// ---- Invincible

					// noop
				}
			endInvincible:

				DDCrash plCrash = DDCrashUtils.Point(new D2Point(this.Player.X, this.Player.Y));

				// ====
				// 描画ここから
				// ====

				// Swap
				{
					DDSubScreen tmp = this.Field;
					this.Field = this.Field_Last;
					this.Field_Last = tmp;
				}

				using (this.Field.Section())
				{
					foreach (Wall wall in this.Walls.Iterate())
						wall.Draw();

					this.Player.Draw();

					// memo: DeadFlag をチェックするのは「当たり判定」から

					foreach (Enemy enemy in this.Enemies.Iterate())
					{
						enemy.Crash = DDCrashUtils.None(); // reset
						enemy.Draw();
					}
					foreach (Shot shot in this.Shots.Iterate())
					{
						shot.Crash = DDCrashUtils.None(); // reset
						shot.Draw();
					}

					if (DDConfig.LOG_ENABLED && 1 <= DDInput.R.GetInput()) // 当たり判定表示(チート)
					{
						DDCurtain.DrawCurtain(-0.7);

						const double A = 0.7;

						DDCrashView.Draw(new DDCrash[] { plCrash }, new I3Color(255, 0, 0), 1.0);
						DDCrashView.Draw(this.Enemies.Iterate().Select(v => v.Crash), new I3Color(255, 255, 255), A);
						DDCrashView.Draw(this.Shots.Iterate().Select(v => v.Crash), new I3Color(0, 255, 255), A);
					}
				}
				DDDraw.DrawSimple(Ground.I.Picture.Background, 0, 0);
				DDDraw.DrawSimple(this.Field.ToPicture(), GameConsts.FIELD_L, GameConsts.FIELD_T);

				// ====
				// 描画ここまで
				// ====

				// ====
				// 当たり判定ここから
				// ====

				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					if (
						1 <= enemy.HP && // ? 敵：生存 && 無敵ではない
						!DDUtils.IsOutOfScreen(new D2Point(enemy.X, enemy.Y)) // ? 画面内の敵である。
						)
					{
						foreach (Shot shot in this.Shots.Iterate())
						{
							// memo: ボスにボムは効かない！

							// 衝突判定：敵 x 自弾
							if (
								!shot.DeadFlag && // ? 自弾：生存
								DDCrashUtils.IsCrashed(enemy.Crash, shot.Crash) && // ? 衝突
								!(enemy.Kind == Enemy.Kind_e.ボス && shot.Kind == Shot.Kind_e.ボム) // ? ボス x ボム ではない。
								)
							{
								// ★ 敵_被弾ここから

								enemy.HP -= shot.AttackPoint;

								if (!shot.敵を貫通する) // 自弾の攻撃力と敵のHPを相殺
								{
									if (0 <= enemy.HP) // ? 丁度削りきった || 削りきれなかった -> 攻撃力を使い果たしたので、ショットは消滅
										shot.Kill();
									else
										shot.AttackPoint = -enemy.HP; // 過剰に削った分を残りの攻撃力として反映
								}

								if (1 <= enemy.HP) // ? まだ生存している。
								{
									enemy.Damaged();
								}
								else // ? 撃破した。
								{
									enemy.Kill(true);
									break; // この敵は死亡したので、この敵について以降の当たり判定は不要
								}

								// ★ 敵_被弾ここまで
							}
						}
					}

					// 衝突判定：敵 x 自機
					if (
						this.Player.RebornFrame == 0 && // ? プレイヤー登場中ではない。
						this.Player.DeadFrame == 0 && // ? プレイヤー死亡中ではない。
						this.Player.InvincibleFrame == 0 && // ? プレイヤー無敵時間中ではない。
						!enemy.DeadFlag && // ? 敵：生存
						!DDUtils.IsOutOfScreen(new D2Point(enemy.X, enemy.Y)) && // ? 画面内の敵である。
						DDCrashUtils.IsCrashed(enemy.Crash, plCrash) // ? 衝突
						)
					{
						// ★ 自機_被弾ここから

						this.Player.DeadFrame = 1;

						// ★ 自機_被弾ここまで
					}
				}

				// ====
				// 当たり判定ここまで
				// ====

				// 不要な壁の死亡フラグを立てる。
				// -- FilledFlag == true な Wall より下の Wall は見えないので破棄して良い。
				{
					bool flag = false;

					for (int index = this.Walls.Count - 1; 0 <= index; index--)
					{
						this.Walls[index].DeadFlag |= flag;
						flag |= this.Walls[index].FilledFlag;
					}
				}

				f_ゴミ回収();

				this.Walls.RemoveAll(v => v.DeadFlag);
				this.Enemies.RemoveAll(v => v.DeadFlag);
				this.Shots.RemoveAll(v => v.DeadFlag);

				DDEngine.EachFrame();

				// ★★★ ゲームループの終わり ★★★
			}

			DDMain.KeepMainScreen();

			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
				DDEngine.EachFrame();
			}

			// ★★★★★
			// プレイヤー・ステータス反映(ステージ終了時)
			{
				// none
			}

			// ★★★ end of Perform() ★★★
		}

		/// <summary>
		/// あまりにもマップから離れすぎている敵・自弾の死亡フラグを立てる。
		/// </summary>
		/// <returns></returns>
		private IEnumerable<bool> E_ゴミ回収()
		{
			for (; ; )
			{
				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					if (this.IsProbablyEvacuated(enemy.X, enemy.Y))
						enemy.DeadFlag = true;

					yield return true;
				}
				foreach (Shot shot in this.Shots.Iterate())
				{
					if (this.IsProbablyEvacuated(shot.X, shot.Y))
						shot.DeadFlag = true;

					yield return true;
				}
				yield return true; // ループ内で1度も実行されない場合を想定
			}
		}

		// Walls:
		// 壁紙リスト
		// 壁紙の重ね合わせ, FilledFlag == true によって、それより下の(古い)壁紙が除去される方式

		public DDList<Wall> Walls = new DDList<Wall>();
		public DDList<Enemy> Enemies = new DDList<Enemy>();
		public DDList<Shot> Shots = new DDList<Shot>();

		private bool Pause_ReturnToTitleMenu = false;

		private static DDSubScreen Pause_KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		/// <summary>
		/// ポーズメニュー
		/// </summary>
		private void Pause()
		{
			DDMain.KeepMainScreen();
			SCommon.Swap(ref DDGround.KeptMainScreen, ref Pause_KeptMainScreen);

			DDSimpleMenu simpleMenu = new DDSimpleMenu()
			{
				BorderColor = new I3Color(0, 64, 128),
				WallDrawer = () =>
				{
					DDDraw.DrawSimple(Pause_KeptMainScreen.ToPicture(), 0, 0);

					DDDraw.SetAlpha(0.5);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 4, DDConsts.Screen_W, DDConsts.Screen_H / 2);
					DDDraw.Reset();
				},
			};

			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(
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
					},
					selectIndex,
					true,
					true
					);

				switch (selectIndex)
				{
					case 0:
						using (new SettingMenu()
						{
							SimpleMenu = new DDSimpleMenu()
							{
								BorderColor = new I3Color(0, 64, 128),
								WallDrawer = () =>
								{
									DDDraw.DrawSimple(Pause_KeptMainScreen.ToPicture(), 0, 0);
									DDCurtain.DrawCurtain(-0.7);
								},
							},
						})
						{
							SettingMenu.I.Perform();
						}
						break;

					case 1:
						if (new Confirm() { BorderColor = new I3Color(0, 0, 200), }.Perform("タイトル画面に戻ります。", "はい", "いいえ") == 0)
						{
							this.Pause_ReturnToTitleMenu = true;
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
			DDEngine.FreezeInput();

			// 寧ろやりにくい
			//DDInput.A.FreezeInputUntilRelease = true;
			//DDInput.B.FreezeInputUntilRelease = true;
		}

		/// <summary>
		/// ポーズメニュー(デバッグ用)
		/// </summary>
		private void DebugPause()
		{
			DDMain.KeepMainScreen();

			DDSimpleMenu simpleMenu = new DDSimpleMenu()
			{
				BorderColor = new I3Color(0, 128, 64),
				WallDrawer = () =>
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
					DDCurtain.DrawCurtain(-0.5);
				},
			};

			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = simpleMenu.Perform(
					40,
					40,
					40,
					24,
					"デバッグ用メニュー",
					new string[]
					{
						"----",
						"----",
						"----",
						"ゲームに戻る",
					},
					selectIndex,
					true,
					true
					);

				switch (selectIndex)
				{
					case 0:
						// none
						break;

					case 1:
						// none
						break;

					case 2:
						// none
						break;

					case 3:
						goto endLoop;

					default:
						throw null; // never
				}
				//DDEngine.EachFrame(); // 不要
			}
		endLoop:
			DDEngine.FreezeInput();

			// 寧ろやりにくい
			//DDInput.A.FreezeInputUntilRelease = true;
			//DDInput.B.FreezeInputUntilRelease = true;
		}

		/// <summary>
		/// フィールドから離れすぎているか
		/// 退場と見なして良いか
		/// </summary>
		/// <param name="x">X_座標</param>
		/// <param name="y">Y_座標</param>
		/// <returns></returns>
		private bool IsProbablyEvacuated(double x, double y)
		{
			const int MGN_FIELD_NUM = 3;

			return
				x < -GameConsts.FIELD_W * MGN_FIELD_NUM || GameConsts.FIELD_W * (1 + MGN_FIELD_NUM) < x ||
				y < -GameConsts.FIELD_H * MGN_FIELD_NUM || GameConsts.FIELD_H * (1 + MGN_FIELD_NUM) < y;
		}

		public void システム的な敵クリア()
		{
			foreach (Enemy enemy in this.Enemies.Iterate())
			{
				if (
					enemy.DeadFlag || // ? 敵：死亡
					enemy is Enemy_BItem ||
					enemy is Enemy_Bボス0001 ||
					enemy is Enemy_Bボス0002 ||
					enemy is Enemy_Bボス0003
					)
				{
					// noop
				}
				else
				{
					enemy.Kill();
				}
			}
		}
	}
}
