using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Tiles;
using Charlotte.Games.Shots;
using Charlotte.Games.Walls;
using Charlotte.LevelEditors;

namespace Charlotte.Games
{
	public class Game : IDisposable
	{
		public World World;
		public GameStatus Status;

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

		public Player Player = new Player();

		public class PlayerHackerInfo
		{
			public bool DIR_2 = false;
			public bool DIR_4 = false;
			public bool DIR_6 = false;
			public bool DIR_8 = false;
			public bool Slow = false;
			public bool Fast = false;
			public bool Attack = false;
			//public bool 武器切り替え = false; // 直接 this.Player.選択武器 を変更した方が早い
		}

		public PlayerHackerInfo PlayerHacker = new PlayerHackerInfo();

		public Map Map;
		private Wall Wall;

		private bool CamSlideMode; // ? モード中
		private int CamSlideCount;
		private int CamSlideX; // -1 ～ 1
		private int CamSlideY; // -1 ～ 1

		public int Frame;
		public bool UserInputDisabled = false;

		public bool RequestReturnToTitleMenu = false;

		public void Perform()
		{
			Func<bool> f_ゴミ回収 = SCommon.Supplier(this.E_ゴミ回収());

			this.Map = new Map(GameCommon.GetMapFile(this.World.GetCurrMapName()));
			this.ReloadEnemies();

			// デフォルトの「プレイヤーのスタート地点」
			// -- マップの中央
			this.Player.X = this.Map.W * GameConsts.TILE_W / 2.0;
			this.Player.Y = this.Map.H * GameConsts.TILE_H / 2.0;

			{
				Enemy enemy = this.Enemies.Iterate().FirstOrDefault(v => v is Enemy_スタート地点 && ((Enemy_スタート地点)v).Direction == this.Status.StartPointDirection);

				if (enemy != null)
				{
					this.Player.X = enemy.X;
					this.Player.Y = enemy.Y;
				}
			}

			// ★★★★★
			// プレイヤー・ステータス反映(マップ入場時)
			// その他の反映箇所：
			// -- マップ退場時
			// -- セーブ時
			{
				this.Player.HP = this.Status.StartHP;
				this.Player.FaceDirection = this.Status.StartFaceDirection;
				this.Player.選択武器 = this.Status.Start選択武器;
			}

			this.Wall = WallCreator.Create(this.Map.WallName);

			MusicCollection.Get(this.Map.MusicName).Play();

			DDGround.Camera.X = this.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.Camera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain(10);

			DDEngine.FreezeInput();

			// TODO: 音楽

			for (this.Frame = 0; ; this.Frame++)
			{
				if (!this.UserInputDisabled && DDInput.PAUSE.GetInput() == 1)
				{
					this.EquipmentMenu();
					//this.Pause(); // old

					if (this.Pause_ReturnToTitleMenu)
					{
						this.Status.ExitDirection = 5;
						break;
					}
				}
				if (this.RequestReturnToTitleMenu)
				{
					this.Status.ExitDirection = 5;
					break;
				}
				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_RETURN) == 1)
				{
					this.DebugPause();
				}

				// 死亡時にカメラ移動を止める。
				//if (this.Player.DeadFrame == 0)
				//    this.カメラ位置調整(false);

				this.カメラ位置調整(false);

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1) // エディットモード(デバッグ用)
				{
					this.Edit();
					this.ReloadEnemies();
					this.Frame = 0;
				}

				// プレイヤー入力・移動
				{
					bool deadOrUID = 1 <= this.Player.DeadFrame || this.UserInputDisabled;
					bool dir2 = !deadOrUID && 1 <= DDInput.DIR_2.GetInput() || this.PlayerHacker.DIR_2;
					bool dir4 = !deadOrUID && 1 <= DDInput.DIR_4.GetInput() || this.PlayerHacker.DIR_4;
					bool dir6 = !deadOrUID && 1 <= DDInput.DIR_6.GetInput() || this.PlayerHacker.DIR_6;
					bool dir8 = !deadOrUID && 1 <= DDInput.DIR_8.GetInput() || this.PlayerHacker.DIR_8;
					int dir; // 1～9 == { 左下, 下, 右下, 左, 動かない, 右, 左上, 上, 右上 }

					if (dir2 && dir4)
						dir = 1;
					else if (dir2 && dir6)
						dir = 3;
					else if (dir4 && dir8)
						dir = 7;
					else if (dir6 && dir8)
						dir = 9;
					else if (dir2)
						dir = 2;
					else if (dir4)
						dir = 4;
					else if (dir6)
						dir = 6;
					else if (dir8)
						dir = 8;
					else
						dir = 5;

					if (1 <= this.Player.DamageFrame) // ? プレイヤー・ダメージ中
						dir = 5;

					bool camSlide = !deadOrUID && 1 <= DDInput.L.GetInput();

					if (camSlide)
						dir = 5;

					bool slow = !deadOrUID && 1 <= DDInput.A.GetInput() || this.PlayerHacker.Slow;
					bool fast = !deadOrUID && 1 <= DDInput.R.GetInput() || this.PlayerHacker.Fast;

					if (Ground.I.FastReverseMode)
						fast = !fast;

					double speed = 3.0;

					if (slow)
						speed -= 1.0;

					if (fast)
						speed += 2.0;

					double nanameSpeed = speed / Consts.ROOT_OF_2;

					switch (dir)
					{
						case 2:
							this.Player.Y += speed;
							break;

						case 4:
							this.Player.X -= speed;
							break;

						case 6:
							this.Player.X += speed;
							break;

						case 8:
							this.Player.Y -= speed;
							break;

						case 1:
							this.Player.X -= nanameSpeed;
							this.Player.Y += nanameSpeed;
							break;

						case 3:
							this.Player.X += nanameSpeed;
							this.Player.Y += nanameSpeed;
							break;

						case 7:
							this.Player.X -= nanameSpeed;
							this.Player.Y -= nanameSpeed;
							break;

						case 9:
							this.Player.X += nanameSpeed;
							this.Player.Y -= nanameSpeed;
							break;

						case 5:
							break;

						default:
							throw null; // never
					}
					if (dir != 5 && !slow)
						this.Player.FaceDirection = dir;

					if (dir != 5)
						this.Player.MoveFrame++;
					else
						this.Player.MoveFrame = 0;

					if (this.Player.MoveFrame == 0) // 立ち止まったら座標を整数に矯正
					{
						this.Player.X = SCommon.ToInt(this.Player.X);
						this.Player.Y = SCommon.ToInt(this.Player.Y);
					}
					if (camSlide)
					{
						if (dir4)
						{
							this.CamSlideCount++;
							this.CamSlideX--;
						}
						if (dir6)
						{
							this.CamSlideCount++;
							this.CamSlideX++;
						}
						if (dir8)
						{
							this.CamSlideCount++;
							this.CamSlideY--;
						}
						if (dir2)
						{
							this.CamSlideCount++;
							this.CamSlideY++;
						}
						DDUtils.ToRange(ref this.CamSlideX, -1, 1);
						DDUtils.ToRange(ref this.CamSlideY, -1, 1);
					}
					else
					{
						if (this.CamSlideMode && this.CamSlideCount == 0)
						{
							this.CamSlideX = 0;
							this.CamSlideY = 0;
						}
						this.CamSlideCount = 0;
					}
					this.CamSlideMode = camSlide;

					bool attack = !deadOrUID && 1 <= DDInput.B.GetInput() || this.PlayerHacker.Attack;

					if (attack)
						this.Player.AttackFrame++;
					else
						this.Player.AttackFrame = 0;

					bool 武器切り替え = !deadOrUID && DDInput.C.GetInput() == 1;

					if (武器切り替え)
						this.Player.選択武器 = (Player.武器_e)(((int)this.Player.選択武器 + 1) % Player.武器_e_Length);
				}

				//startDead:
				if (1 <= this.Player.DeadFrame) // プレイヤー死亡中の処理
				{
					if (GameConsts.PLAYER_DEAD_FRAME_MAX < ++this.Player.DeadFrame)
					{
						this.Player.DeadFrame = 0;
						this.Status.ExitDirection = 5;
						break;
					}
					int frame = this.Player.DeadFrame; // 値域 == 2 ～ GameConsts.PLAYER_DEAD_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_DEAD_FRAME_MAX, frame);

					// ----

					const int HIT_BACK_FRAME_MAX = 30;
					double hitBackRate = DDUtils.RateAToB(2, HIT_BACK_FRAME_MAX, frame);

					if (hitBackRate < 1.0)
					{
						double invHitBackRate = 1.0 - hitBackRate;

						D2Point speed = GameCommon.GetXYSpeed(this.Player.FaceDirection, 10.0 * invHitBackRate);

						this.Player.X -= speed.X;
						this.Player.Y -= speed.Y;
					}
				}
				//endDead:

				//startDamage:
				if (1 <= this.Player.DamageFrame) // プレイヤー・ダメージ中の処理
				{
					if (GameConsts.PLAYER_DAMAGE_FRAME_MAX < ++this.Player.DamageFrame)
					{
						this.Player.DamageFrame = 0;
						this.Player.InvincibleFrame = 1;
						goto endDamage;
					}
					int frame = this.Player.DamageFrame; // 値域 == 2 ～ GameConsts.PLAYER_DAMAGE_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_DAMAGE_FRAME_MAX, frame);

					// ----

					{
						D2Point speed = GameCommon.GetXYSpeed(this.Player.FaceDirection, 5.0);

						for (int c = 0; c < 5; c++)
						{
							{
								int x = SCommon.ToInt(this.Player.X) / GameConsts.TILE_W;
								int y = SCommon.ToInt(this.Player.Y) / GameConsts.TILE_H;

								if (this.Map.GetCell(x, y).Tile.GetKind() != Tile.Kind_e.SPACE) // ? 歩行可能な場所ではない -> これ以上ヒットバックさせない。
									break;
							}

							this.Player.X -= speed.X;
							this.Player.Y -= speed.Y;
						}
					}
				}
			endDamage:

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

					// ----

					// noop
				}
			endInvincible:

				// プレイヤー位置矯正
				{
					壁キャラ処理.Perform(ref this.Player.X, ref this.Player.Y, v => v.GetKind() != Tile.Kind_e.SPACE);
				}

				if (this.Player.X < 0.0) // ? マップの左側に出た。
				{
					this.Status.ExitDirection = 4;
					break;
				}
				if (this.Map.W * GameConsts.TILE_W < this.Player.X) // ? マップの右側に出た。
				{
					this.Status.ExitDirection = 6;
					break;
				}
				if (this.Player.Y < 0.0) // ? マップの上側に出た。
				{
					this.Status.ExitDirection = 8;
					break;
				}
				if (this.Map.H * GameConsts.TILE_H < this.Player.Y) // ? マップの下側に出た。
				{
					this.Status.ExitDirection = 2;
					break;
				}

				// 画面遷移時の微妙なカメラ位置ズレ解消
				// -- 必要無いかもしれないが、念の為実行しておく。
				if (this.Frame == 0)
				{
					this.カメラ位置調整(true);
				}

				if (1 <= this.Player.AttackFrame)
				{
					this.Player.Attack();
				}

				DDCrash plCrash = DDCrashUtils.Point(new D2Point(this.Player.X, this.Player.Y));

				// ====
				// 描画ここから
				// ====

				this.DrawWall();
				this.DrawMap();
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

				if (this.当たり判定表示)
				{
					// 最後に描画されるように DDGround.EL.Add() する。

					DDGround.EL.Add(() =>
					{
						DDCurtain.DrawCurtain(-0.8);

						const double A = 0.3;

						DDCrashView.Draw(new DDCrash[] { plCrash }, new I3Color(255, 0, 0), 1.0);
						DDCrashView.Draw(this.Enemies.Iterate().Select(v => v.Crash), new I3Color(255, 255, 255), A);
						DDCrashView.Draw(this.Shots.Iterate().Select(v => v.Crash), new I3Color(0, 255, 255), A);

						return false;
					});
				}

				// ====
				// 描画ここまで
				// ====

				// ====
				// 当たり判定ここから
				// ====

				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					if (1 <= enemy.HP) // ? 敵：生存 && 無敵ではない
					{
						foreach (Shot shot in this.Shots.Iterate())
						{
							// 衝突判定：敵 x 自弾
							if (
								!shot.DeadFlag && // ? 自弾：生存
								enemy.Crash.IsCrashed(shot.Crash) // ? 衝突
								)
							{
								// ★ 敵_被弾ここから

								// 貫通武器について、貫通中に複数回クラッシュしないように制御する。
								// -- 複数の敵に同時に当たると意図通りにならないが、厳格に制御する必要は無いので、看過する。

								if (shot.LastCrashedEnemy == enemy) // ? 直前にクラッシュした -> 複数回クラッシュしない。
									continue;

								enemy.HP -= shot.AttackPoint;

								if (shot.敵を貫通する)
								{
									shot.LastCrashedEnemy = enemy;
								}
								else // ? 敵を貫通しない -> 自弾の攻撃力と敵のHPを相殺
								{
									if (0 <= enemy.HP) // ? 丁度削りきった || 削りきれなかった -> 攻撃力を使い果たしたので、ショットは消滅
										shot.Kill();
									else
										shot.AttackPoint = -enemy.HP; // 過剰に削った分を残りの攻撃力として反映
								}

								if (1 <= enemy.HP) // ? まだ生存している。
								{
									enemy.Damaged(shot);
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
						this.Player.DeadFrame == 0 && // ? プレイヤー死亡中ではない。
						this.Player.DamageFrame == 0 && // ? プレイヤー・ダメージ中ではない。
						this.Player.InvincibleFrame == 0 && // ? プレイヤー無敵時間中ではない。
						!enemy.DeadFlag && // ? 敵：生存
						DDCrashUtils.IsCrashed(enemy.Crash, plCrash) // ? 衝突
						)
					{
						// ★ 自機_被弾ここから

						if (enemy.自機に当たると消滅する)
							enemy.Kill();

						this.Player.HP -= enemy.AttackPoint;

						if (1 <= this.Player.HP) // ? まだ生存している。
						{
							this.Player.DamageFrame = 1;
						}
						else // ? 死亡した。
						{
							this.Player.HP = -1;
							this.Player.DeadFrame = 1;
						}

						// ★ 自機_被弾ここまで
					}
				}

				foreach (Shot shot in this.Shots.Iterate())
				{
					// 壁への当たり判定は自弾の「中心座標のみ」であることに注意して下さい。

					if (
						!shot.DeadFlag && // ? 自弾：生存
						!shot.壁をすり抜ける && // ? この自弾は壁に当たる。
						this.Map.GetCell(GameCommon.ToTablePoint(shot.X, shot.Y)).Tile.GetKind() == Tile.Kind_e.WALL // ? 壁に当たった。
						)
						shot.Kill();
				}

				// ====
				// 当たり判定ここまで
				// ====

				f_ゴミ回収();

				this.Enemies.RemoveAll(v => v.DeadFlag);
				this.Shots.RemoveAll(v => v.DeadFlag);

				DDEngine.EachFrame();

				// ★★★ ゲームループの終わり ★★★
			}
			DDEngine.FreezeInput();

			if (this.Status.ExitDirection == 5)
			{
				DDMusicUtils.Fade();
				DDCurtain.SetCurtain(30, -1.0);

				foreach (DDScene scene in DDSceneUtils.Create(40))
				{
					this.DrawWall();
					this.DrawMap();

					DDEngine.EachFrame();
				}
			}
			else
			{
				double destSlide_X = 0.0;
				double destSlide_Y = 0.0;

				switch (this.Status.ExitDirection)
				{
					case 4:
						destSlide_X = DDConsts.Screen_W;
						break;

					case 6:
						destSlide_X = -DDConsts.Screen_W;
						break;

					case 8:
						destSlide_Y = DDConsts.Screen_H;
						break;

					case 2:
						destSlide_Y = -DDConsts.Screen_H;
						break;

					default:
						throw null; // never
				}
				using (DDSubScreen wallMapScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H))
				{
					using (wallMapScreen.Section())
					{
						this.DrawWall();
						this.DrawMap();
					}
					foreach (DDScene scene in DDSceneUtils.Create(30))
					{
						double slide_X = destSlide_X * scene.Rate;
						double slide_Y = destSlide_Y * scene.Rate;

						DDCurtain.DrawCurtain();
						DDDraw.DrawSimple(wallMapScreen.ToPicture(), slide_X, slide_Y);

						DDEngine.EachFrame();
					}
				}
				DDCurtain.SetCurtain(0, -1.0);
			}

			// ★★★★★
			// プレイヤー・ステータス反映(マップ退場時)
			// その他の反映箇所：
			// -- マップ入場時
			// -- セーブ時
			{
				this.Status.StartHP = this.Player.HP;
				this.Status.StartFaceDirection = this.Player.FaceDirection;
				this.Status.Start選択武器 = this.Player.選択武器;
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

		private void カメラ位置調整(bool 一瞬で)
		{
			double targCamX = this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3);
			double targCamY = this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3);

			DDUtils.ToRange(ref targCamX, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
			DDUtils.ToRange(ref targCamY, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

			// 不要
			//if (this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W < GameConsts.TILE_W) // ? カメラの横の可動域が1タイルより狭い場合
			//    targCamX = (this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W) / 2; // 中心に合わせる。

			if (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H < GameConsts.TILE_H) // ? カメラの縦の可動域が1タイルより狭い場合
				targCamY = (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H) / 2; // 中心に合わせる。

			DDUtils.Approach(ref DDGround.Camera.X, targCamX, 一瞬で ? 0.0 : 0.8);
			DDUtils.Approach(ref DDGround.Camera.Y, targCamY, 一瞬で ? 0.0 : 0.8);

			//DDUtils.ToRange(ref DDGround.Camera.X, 0.0, this.Map.W * Consts.TILE_W - DDConsts.Screen_W);
			//DDUtils.ToRange(ref DDGround.Camera.Y, 0.0, this.Map.H * Consts.TILE_H - DDConsts.Screen_H);

			DDGround.ICamera.X = SCommon.ToInt(DDGround.Camera.X);
			DDGround.ICamera.Y = SCommon.ToInt(DDGround.Camera.Y);
		}

		#region Edit

		private void Edit()
		{
			this.Map.Load(); // ゲーム中にマップを書き換える場合があるので、再ロードする。

			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(true);
			LevelEditor.ShowDialog();

			int lastMouseX = DDMouse.X;
			int lastMouseY = DDMouse.Y;

			for (; ; )
			{
				if (LevelEditor.Dlg.XPressed)
					break;

				// 廃止
				//if (DDKey.GetInput(DX.KEY_INPUT_E) == 1)
				//    break;

				I2Point cellPos = GameCommon.ToTablePoint(
					DDGround.Camera.X + DDMouse.X,
					DDGround.Camera.Y + DDMouse.Y
					);

				MapCell cell = Game.I.Map.GetCell(cellPos);

				if (cell.IsDefault)
				{
					// noop
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT) && 1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL)) // 左シフト・コントロール押下 -> 塗り潰し_L / 塗り潰し_R
				{
					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
						this.Map.Save(); // 失敗を想定して、セーブしておく

						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								{
									string tileName = LevelEditor.Dlg.GetTile_L();

									if (tileName != cell.TileName)
									{
										string targetTileName = cell.TileName; // cell.TileName は this.EditFill で変更される。

										this.EditFill(
											cellPos,
											v => v.TileName == targetTileName,
											v =>
											{
												v.TileName = tileName;
												v.Tile = TileCatalog.Create(tileName);
											}
											);
									}
								}
								break;

							case LevelEditor.Mode_e.ENEMY:
								{
									string enemyName = LevelEditor.Dlg.GetEnemy();

									if (enemyName != cell.EnemyName)
									{
										string targetEnemyName = cell.EnemyName; // cell.EnemyName は this.EditFill で変更される。

										this.EditFill(
											cellPos,
											v => v.EnemyName == targetEnemyName,
											v => v.EnemyName = enemyName
											);
									}
								}
								break;

							default:
								throw null; // never
						}
					}
					else if (DDMouse.R.GetInput() == -1) // クリックを検出
					{
						this.Map.Save(); // 失敗を想定して、セーブしておく

						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								{
									string tileName = LevelEditor.Dlg.GetTile_R();

									if (tileName != cell.TileName)
									{
										string targetTileName = cell.TileName; // cell.TileName は this.EditFill で変更される。

										this.EditFill(
											cellPos,
											v => v.TileName == targetTileName,
											v =>
											{
												v.TileName = tileName;
												v.Tile = TileCatalog.Create(tileName);
											}
											);
									}
								}
								break;

							case LevelEditor.Mode_e.ENEMY:
								// none
								break;

							default:
								throw null; // never
						}
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT)) // 左シフト押下 -> 移動 / none
				{
					if (1 <= DDMouse.L.GetInput())
					{
						DDGround.Camera.X -= DDMouse.X - lastMouseX;
						DDGround.Camera.Y -= DDMouse.Y - lastMouseY;

						DDUtils.ToRange(ref DDGround.Camera.X, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
						DDUtils.ToRange(ref DDGround.Camera.Y, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

						DDGround.ICamera.X = SCommon.ToInt(DDGround.Camera.X);
						DDGround.ICamera.Y = SCommon.ToInt(DDGround.Camera.Y);
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL)) // 左コントロール押下 -> スポイト_L / スポイト_R
				{
					if (1 <= DDMouse.L.GetInput())
					{
						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								LevelEditor.Dlg.SetTile_L(cell.TileName);
								break;

							case LevelEditor.Mode_e.ENEMY:
								LevelEditor.Dlg.SetEnemy(cell.EnemyName);
								break;

							default:
								throw null; // never
						}
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								LevelEditor.Dlg.SetTile_R(cell.TileName);
								break;

							case LevelEditor.Mode_e.ENEMY:
								// none
								break;

							default:
								throw null; // never
						}
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LALT)) // 左 ALT 押下 -> 自機ワープ / none
				{
					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
						this.Player.X = cellPos.X * GameConsts.TILE_W + GameConsts.TILE_W / 2;
						this.Player.Y = cellPos.Y * GameConsts.TILE_H + GameConsts.TILE_H / 2;

						DDGround.EL.Add(SCommon.Supplier(Effects.中爆発(this.Player.X, this.Player.Y))); // アクションが分かるように
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else // シフト系押下無し -> セット_L / セット_R (敵はクリア)
				{
					if (1 <= DDMouse.L.GetInput())
					{
						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								{
									string tileName = LevelEditor.Dlg.GetTile_L();

									cell.TileName = tileName;
									cell.Tile = TileCatalog.Create(tileName);
								}
								break;

							case LevelEditor.Mode_e.ENEMY:
								{
									string enemyName = LevelEditor.Dlg.GetEnemy();

									cell.EnemyName = enemyName;
								}
								break;

							default:
								throw null; // never
						}
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								{
									string tileName = LevelEditor.Dlg.GetTile_R();

									cell.TileName = tileName;
									cell.Tile = TileCatalog.Create(tileName);
								}
								break;

							case LevelEditor.Mode_e.ENEMY:
								cell.EnemyName = GameConsts.ENEMY_NONE;
								break;

							default:
								throw null; // never
						}
					}
				}

				if (DDKey.GetInput(DX.KEY_INPUT_S) == 1) // S キー --> Save
				{
					this.Map.Save();

					// 表示
					{
						int endFrame = DDEngine.ProcFrame + 60;

						DDGround.EL.Add(() =>
						{
							DDPrint.SetDebug(0, 16);
							DDPrint.SetBorder(new I3Color(0, 0, 0));
							DDPrint.Print("セーブしました...");
							DDPrint.Reset();

							return DDEngine.ProcFrame < endFrame;
						});
					}
				}
				if (DDKey.GetInput(DX.KEY_INPUT_L) == 1) // L キー --> Load
				{
					this.Map.Load();

					// 表示
					{
						int endFrame = DDEngine.ProcFrame + 60;

						DDGround.EL.Add(() =>
						{
							DDPrint.SetDebug(0, 16);
							DDPrint.SetBorder(new I3Color(0, 0, 0));
							DDPrint.Print("ロードしました...");
							DDPrint.Reset();

							return DDEngine.ProcFrame < endFrame;
						});
					}
				}
				if (DDKey.GetInput(DX.KEY_INPUT_H) == 1) // H キー --> セット_L(10x10)
				{
					this.Map.Save(); // 失敗を想定して、セーブしておく

					int firstTileIndex;

					{
						string tileName = LevelEditor.Dlg.GetTile_L();
						int index = SCommon.IndexOf(TileCatalog.GetNames(), name => name == tileName);

						if (index == -1)
							throw new DDError();

						firstTileIndex = index;
					}

					int offset = 0;

					for (int xc = 0; xc < 10; xc++)
					{
						for (int yc = 0; yc < 10; yc++)
						{
							string tileName = TileCatalog.GetNames()[(firstTileIndex + offset) % TileCatalog.GetNames().Length];
							offset++;

							I2Point subCellPos = new I2Point(cellPos.X + xc, cellPos.Y + yc);
							MapCell subCell = Game.I.Map.GetCell(subCellPos);

							subCell.TileName = tileName;
							subCell.Tile = TileCatalog.Create(tileName);
						}
					}
				}

				DDCurtain.DrawCurtain();

				if (LevelEditor.Dlg.IsShowTile())
					this.DrawMap();

				if (LevelEditor.Dlg.IsShowEnemy())
					LevelEditor.DrawEnemy();

				lastMouseX = DDMouse.X;
				lastMouseY = DDMouse.Y;

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(false);
			LevelEditor.CloseDialog();

			this.Map.Save(); // ★★★ マップをセーブする ★★★
		}

		private void EditFill(I2Point targetPos, Predicate<MapCell> isFillable, Action<MapCell> fill)
		{
			Queue<I2Point> q = new Queue<I2Point>();

			q.Enqueue(targetPos);

			while (1 <= q.Count)
			{
				I2Point cellPos = q.Dequeue();
				MapCell cell = this.Map.GetCell(cellPos);

				if (!cell.IsDefault && isFillable(cell))
				{
					fill(cell);

					q.Enqueue(new I2Point(cellPos.X - 1, cellPos.Y));
					q.Enqueue(new I2Point(cellPos.X + 1, cellPos.Y));
					q.Enqueue(new I2Point(cellPos.X, cellPos.Y - 1));
					q.Enqueue(new I2Point(cellPos.X, cellPos.Y + 1));
				}
			}
		}

		#endregion

		private void DrawWall()
		{
			double xRate = (double)DDGround.ICamera.X / (this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
			double yRate = (double)DDGround.ICamera.Y / (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

			this.Wall.Draw(xRate, yRate);
		}

		private void DrawMap()
		{
			int w = this.Map.W;
			int h = this.Map.H;

			int cam_l = DDGround.ICamera.X;
			int cam_t = DDGround.ICamera.Y;
			int cam_r = cam_l + DDConsts.Screen_W;
			int cam_b = cam_t + DDConsts.Screen_H;

			I2Point lt = GameCommon.ToTablePoint(cam_l, cam_t);
			I2Point rb = GameCommon.ToTablePoint(cam_r, cam_b);

			// マージン付与
			// -- マップセルの範囲をはみ出て描画されるタイルのためにマージンを増やす。
			{
				lt.X -= 2;
				lt.Y -= 2;
				rb.X += 2;
				rb.Y += 2;
			}

			lt.X = SCommon.ToRange(lt.X, 0, w - 1);
			lt.Y = SCommon.ToRange(lt.Y, 0, h - 1);
			rb.X = SCommon.ToRange(rb.X, 0, w - 1);
			rb.Y = SCommon.ToRange(rb.Y, 0, h - 1);

			for (int x = lt.X; x <= rb.X; x++)
			{
				for (int y = lt.Y; y <= rb.Y; y++)
				{
					MapCell cell = this.Map.Table[x, y];

					int tileX = x * GameConsts.TILE_W + GameConsts.TILE_W / 2;
					int tileY = y * GameConsts.TILE_H + GameConsts.TILE_H / 2;

					cell.Tile.Draw(tileX - cam_l, tileY - cam_t, x, y);
				}
			}
		}

		public DDList<Enemy> Enemies = new DDList<Enemy>();
		public DDList<Shot> Shots = new DDList<Shot>();

		private void ReloadEnemies()
		{
			this.Enemies.Clear();

			for (int x = 0; x < this.Map.W; x++)
			{
				for (int y = 0; y < this.Map.H; y++)
				{
					string enemyName = this.Map.Table[x, y].EnemyName;

					if (enemyName != GameConsts.ENEMY_NONE)
					{
						this.Enemies.Add(EnemyCatalog.Create(
							this.Map.Table[x, y].EnemyName,
							x * GameConsts.TILE_W + GameConsts.TILE_W / 2.0,
							y * GameConsts.TILE_H + GameConsts.TILE_H / 2.0
							));
					}
				}
			}
		}

		/// <summary>
		/// マップから離れすぎているか
		/// 退場と見なして良いか
		/// </summary>
		/// <param name="x">X_座標</param>
		/// <param name="y">Y_座標</param>
		/// <returns></returns>
		private bool IsProbablyEvacuated(double x, double y)
		{
			const int MGN_SCREEN_NUM = 3;

			return
				x < -DDConsts.Screen_W * MGN_SCREEN_NUM || this.Map.W * GameConsts.TILE_W + DDConsts.Screen_W * MGN_SCREEN_NUM < x ||
				y < -DDConsts.Screen_H * MGN_SCREEN_NUM || this.Map.H * GameConsts.TILE_H + DDConsts.Screen_H * MGN_SCREEN_NUM < y;
		}

		#region EquipmentMenu

		private static DDSubScreen EquipmentMenu_KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

		private void EquipmentMenu()
		{
			DDInput.DIR_2.FreezeInputUntilRelease = true;
			DDInput.DIR_4.FreezeInputUntilRelease = true;
			DDInput.DIR_6.FreezeInputUntilRelease = true;
			DDInput.DIR_8.FreezeInputUntilRelease = true;
			DDInput.A.FreezeInputUntilRelease = true;
			DDInput.B.FreezeInputUntilRelease = true;

			DDMain.KeepMainScreen();
			SCommon.Swap(ref DDGround.KeptMainScreen, ref EquipmentMenu_KeptMainScreen);

			DDTableMenu tableMenu = new DDTableMenu(130, 50, 24, () =>
			{
				DDDraw.DrawSimple(EquipmentMenu_KeptMainScreen.ToPicture(), 0, 0);

				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(0.0, 0.4, 0.0);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 8, DDConsts.Screen_W, DDConsts.Screen_H * 3 / 4);
				DDDraw.Reset();
			});

			//switch (this.Status.Equipment)
			//{
			//    case GameStatus.Equipment_e.Normal: tableMenu.SetSelectedPosition(0, 1); break;
			//    case GameStatus.Equipment_e.跳ねる陰陽玉: tableMenu.SetSelectedPosition(0, 2); break;
			//    case GameStatus.Equipment_e.ハンマー陰陽玉: tableMenu.SetSelectedPosition(0, 3); break;
			//    case GameStatus.Equipment_e.エアーシューター: tableMenu.SetSelectedPosition(0, 4); break;
			//    case GameStatus.Equipment_e.マグネットエアー: tableMenu.SetSelectedPosition(0, 5); break;

			//    default:
			//        break;
			//}

			for (bool keepMenu = true; keepMenu; )
			{
				{
					I3Color color = new I3Color(255, 255, 255);
					I3Color borderColor = new I3Color(0, 128, 0);
					I3Color 装備中Color = new I3Color(255, 255, 0);
					I3Color 装備中BorderColor = new I3Color(128, 128, 0);
					I3Color 未取得Color = new I3Color(128, 128, 200);
					I3Color 未取得BorderColor = new I3Color(0, 64, 0);

					tableMenu.AddColumn(130);
					tableMenu.AddItem(true, "装備・アイテム", color, borderColor);
					tableMenu.AddItem(false, "ダミー0001", color, borderColor, () => { });
					tableMenu.AddItem(false, "ダミー0002", color, borderColor, () => { });
					tableMenu.AddItem(false, "ダミー0003", color, borderColor, () => { });

					tableMenu.AddColumn(540);
					tableMenu.AddItem(true, "システム", color, borderColor);
					tableMenu.AddItem(false, "システムメニュー", color, borderColor, () =>
					{
						this.Pause();

						if (this.Pause_ReturnToTitleMenu)
							keepMenu = false;
					});
					tableMenu.AddItem(false, "戻る", color, borderColor, () => keepMenu = false);
				}

				tableMenu.Perform();

				//DDEngine.EachFrame(); // 不要
			}

			DDInput.DIR_2.FreezeInputUntilRelease = false;
			DDInput.DIR_4.FreezeInputUntilRelease = false;
			DDInput.DIR_6.FreezeInputUntilRelease = false;
			DDInput.DIR_8.FreezeInputUntilRelease = false;
			DDInput.A.FreezeInputUntilRelease = true;
			DDInput.B.FreezeInputUntilRelease = true;
		}

		#endregion

		private bool Pause_ReturnToTitleMenu = false;
		private bool 当たり判定表示 = false;

		//private static DDSubScreen Pause_KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);
		private static DDSubScreen Pause_KeptMainScreen { get { return EquipmentMenu_KeptMainScreen; } }

		/// <summary>
		/// ポーズメニュー
		/// </summary>
		private void Pause()
		{
			// old
			//DDMain.KeepMainScreen();
			//SCommon.Swap(ref DDGround.KeptMainScreen, ref Pause_KeptMainScreen);

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
					250,
					180,
					50,
					24,
					"システムメニュー",
					new string[]
					{
						"設定",
						"タイトルに戻る",
						"戻る",
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

			// old
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
				string 現在のキャラクタ = ResourcePicture2.Player_e_Names[(int)this.Status.Chara];

				selectIndex = simpleMenu.Perform(
					40,
					40,
					40,
					24,
					"デバッグ用メニュー",
					new string[]
					{
						"キャラクタ切り替え [ 現在のキャラクタ：" + 現在のキャラクタ + " ] (Lホールド=逆順)",
						"デバッグ強制遅延 [ 現在の設定：" + DDEngine.SlowdownLevel + " ]",
						"当たり判定表示 [ 現在の設定：" + this.当たり判定表示 + " ]",
						"ゲームに戻る",
					},
					selectIndex,
					true,
					true
					);

				switch (selectIndex)
				{
					case 0:
						{
							bool backMode;

							{
								int bk_freezeInputFrame = DDEngine.FreezeInputFrame;
								DDEngine.FreezeInputFrame = 0;
								backMode = 1 <= DDInput.L.GetInput();
								DDEngine.FreezeInputFrame = bk_freezeInputFrame;
							}

							int chara = (int)this.Status.Chara;

							if (backMode)
								chara--;
							else
								chara++;

							chara += ResourcePicture2.Player_e_Length;
							chara %= ResourcePicture2.Player_e_Length;

							this.Status.Chara = (ResourcePicture2.Player_e)chara;
						}
						break;

					case 1:
						if (DDEngine.SlowdownLevel == 0)
							DDEngine.SlowdownLevel++;
						else
							DDEngine.SlowdownLevel *= 2;

						DDEngine.SlowdownLevel %= 16;
						break;

					case 2:
						this.当たり判定表示 = !this.当たり判定表示;
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

			DDInput.A.FreezeInputUntilRelease = true;
			DDInput.B.FreezeInputUntilRelease = true;
		}
	}
}
