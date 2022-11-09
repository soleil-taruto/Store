using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.GameTools;
using Charlotte.Games.Enemies;
using Charlotte.Games.Shots;
using Charlotte.Games.Tiles;
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
			public bool Jump = false;
			public bool Attack = false;
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

		public DDTaskList Tasks = new DDTaskList();
		public DDActionList FrontActions = new DDActionList(true);

		public void Perform()
		{
			//Func<bool> f_ゴミ回収 = SCommon.Supplier(this.E_ゴミ回収()); // メソッド版_廃止
			this.Tasks.Add(SCommon.Supplier(this.E_ゴミ回収()));

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

			// ★★★★★ *****PSH (<-このパターンで検索できるようにしておく)
			// プレイヤー・ステータス反映(マップ入場時)
			// その他の反映箇所：
			// -- マップ退場時
			// -- セーブ時
			{
				// すべきこと：
				// -- ゲーム状態を this.Status から各方面に展開・反映する。

				// 例：
				//this.Player.HP = this.Status.StartHP;
				//this.Player.FacingLeft = this.Status.StartFacingLeft;
				//this.Player.武器 = this.Status.Start_武器;
				// --

				this.Player.HP = this.Status.StartHP;
				this.Player.FacingLeft = this.Status.StartFacingLeft;
				this.Player.武器 = this.Status.Start_武器;
			}

			this.Wall = WallCatalog.Create(this.Map.WallName);

			MusicCollection.Get(this.Map.MusicName).Play();

			DDGround.RealCamera.X = this.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.RealCamera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

			DDCurtain.SetCurtain(0, -1.0);
			DDCurtain.SetCurtain(10);

			DDEngine.FreezeInput();

			for (this.Frame = 0; ; this.Frame++)
			{
				if (!this.UserInputDisabled && DDInput.PAUSE.GetInput() == 1)
				{
					this.EquipmentMenu();
					//this.Pause(); // old
				}
				if (this.RequestReturnToTitleMenu)
				{
					this.Status.ExitDirection = 5;
					break;
				}
				if (DDConfig.LOG_ENABLED && DDInput.START.GetInput() == 1)
				//if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_RETURN) == 1)
				{
					this.DebugPause();
				}

				this.カメラ位置調整(false);

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1) // エディットモード(デバッグ用)
				{
					this.Edit();
					this.ReloadEnemies();
					this.Frame = 0;
				}

				bool camSlide = false;

				// プレイヤー入力
				{
					bool damageOrUID = 1 <= this.Player.DamageFrame || this.UserInputDisabled;
					bool move = false;
					bool slow = false;
					bool attack = false;
					bool shagami = false;
					bool uwamuki = false;
					bool shitamuki = false;
					int jump = 0;

					if (!damageOrUID && 1 <= DDInput.DIR_8.GetInput() || this.PlayerHacker.DIR_8)
					{
						uwamuki = true;
					}
					if (!damageOrUID && 1 <= DDInput.DIR_2.GetInput() || this.PlayerHacker.DIR_2)
					{
						shagami = true;
						shitamuki = true;
					}

					// 入力抑止中であるか否かに関わらず左右の入力は受け付けるようにする。
					int freezeInputFrameBackup = DDEngine.FreezeInputFrame;
					DDEngine.FreezeInputFrame = 0;

					if (!damageOrUID && 1 <= DDInput.DIR_4.GetInput() || this.PlayerHacker.DIR_4)
					{
						this.Player.FacingLeft = true;
						move = true;
					}
					if (!damageOrUID && 1 <= DDInput.DIR_6.GetInput() || this.PlayerHacker.DIR_6)
					{
						this.Player.FacingLeft = false;
						move = true;
					}

					DDEngine.FreezeInputFrame = freezeInputFrameBackup; // restore

					if (1 <= DDInput.L.GetInput())
					{
						move = false;
						shagami = false;
						uwamuki = false;
						camSlide = true;
					}
					if (!damageOrUID && 1 <= DDInput.R.GetInput() || this.PlayerHacker.Slow)
					{
						slow = true;
					}
					if (!damageOrUID && 1 <= DDInput.A.GetInput() || this.PlayerHacker.Jump)
					{
						jump = this.PlayerHacker.Jump ? 1 : DDInput.A.GetInput();
					}
					if (!damageOrUID && 1 <= DDInput.B.GetInput() || this.PlayerHacker.Attack)
					{
						attack = true;
					}

					if (move)
					{
						this.Player.MoveFrame++;
						shagami = false;
						//uwamuki = false;
						//shitamuki = false;
					}
					else
					{
						this.Player.MoveFrame = 0;
					}
					this.Player.MoveSlow = move && slow;

					if (jump == 0)
						this.Player.JumpLock = false;

					if (1 <= this.Player.JumpFrame) // ? ジャンプ中
					{
						const int JUMP_FRAME_MAX = 22;

						if (1 <= jump && this.Player.JumpFrame < JUMP_FRAME_MAX)
							this.Player.JumpFrame++;
						else
							this.Player.JumpFrame = 0;
					}
					else // ? 接地中 || 滞空中
					{
						// 事前入力 == 着地前の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。
						// 入力猶予 == 落下(地面から離れた)直後の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。

						const int 事前入力時間 = 5;
						const int 入力猶予時間 = 5;

						if (1 <= jump && jump < 事前入力時間 && this.Player.AirborneFrame < 入力猶予時間 && this.Player.JumpCount == 0 && !this.Player.JumpLock)
						{
							this.Player.JumpCount = 1;
							this.Player.JumpFrame = 1;
							this.Player.JumpLock = true;
						}
					}

					if (this.Player.JumpFrame == 1) // ? ジャンプ開始
					{
						Ground.I.SE.PlayerJump.Play();
					}

					if (1 <= this.Player.AirborneFrame)
					{
						shagami = false;
						//uwamuki = false;
						//shitamuki = false;
					}

					if (shagami)
						this.Player.ShagamiFrame++;
					else
						this.Player.ShagamiFrame = 0;

					if (uwamuki)
						this.Player.UwamukiFrame++;
					else
						this.Player.UwamukiFrame = 0;

					if (shitamuki)
						this.Player.ShitamukiFrame++;
					else
						this.Player.ShitamukiFrame = 0;

					if (attack)
						this.Player.AttackFrame++;
					else
						this.Player.AttackFrame = 0;
				}

				// カメラ位置スライド
				{
					if (camSlide)
					{
						if (DDInput.DIR_4.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideX--;
						}
						if (DDInput.DIR_6.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideX++;
						}
						if (DDInput.DIR_8.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideY--;
						}
						if (DDInput.DIR_2.IsPound())
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
				}

				if (1 <= this.Player.DamageFrame) // ? プレイヤー・ダメージ中
				{
					if (GameConsts.PLAYER_DAMAGE_FRAME_MAX < ++this.Player.DamageFrame)
					{
						this.Player.DamageFrame = 0;
						this.Player.InvincibleFrame = 1;
						goto endDamage;
					}
					int frame = this.Player.DamageFrame; // 値域 == 2 ～ GameConsts.PLAYER_DAMAGE_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_DAMAGE_FRAME_MAX, frame);

					// プレイヤー・ダメージ中の処理
					{
						if (frame == 2) // 初回のみ
						{
							Ground.I.SE.PlayerDamaged.Play();
						}

						this.Player.X -= (9.0 - 6.0 * rate) * (this.Player.FacingLeft ? -1 : 1);
					}
				}
			endDamage:

				if (1 <= this.Player.InvincibleFrame) // ? プレイヤー無敵時間中
				{
					if (GameConsts.PLAYER_INVINCIBLE_FRAME_MAX < ++this.Player.InvincibleFrame)
					{
						this.Player.InvincibleFrame = 0;
						goto endInvincible;
					}
					int frame = this.Player.InvincibleFrame; // 値域 == 2 ～ GameConsts.PLAYER_INVINCIBLE_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_INVINCIBLE_FRAME_MAX, frame);

					// プレイヤー無敵時間中の処理
					{
						// none
					}
				}
			endInvincible:

				// プレイヤー移動
				{
					if (1 <= this.Player.MoveFrame)
					{
						double speed;

						if (this.Player.MoveSlow)
						{
							speed = this.Player.MoveFrame * 0.2;
							DDUtils.Minim(ref speed, GameConsts.PLAYER_SLOW_SPEED);
						}
						else
						{
							speed = GameConsts.PLAYER_SPEED;
						}
						speed *= this.Player.FacingLeft ? -1 : 1;
						this.Player.X += speed;
					}
					else
					{
						this.Player.X = (double)SCommon.ToInt(this.Player.X);
					}

					if (1 <= this.Player.JumpFrame)
						this.Player.YSpeed = GameConsts.PLAYER_JUMP_SPEED;
					else
						this.Player.YSpeed += GameConsts.PLAYER_GRAVITY;

					DDUtils.Minim(ref this.Player.YSpeed, GameConsts.PLAYER_FALL_SPEED_MAX);

					this.Player.Y += this.Player.YSpeed; // 自由落下
				}

				// プレイヤー位置矯正
				{
					bool touchSide_L =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y - GameConsts.PLAYER_側面判定Pt_YT)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y + GameConsts.PLAYER_側面判定Pt_YB)).Tile.IsWall();

					bool touchSide_R =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y - GameConsts.PLAYER_側面判定Pt_YT)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y + GameConsts.PLAYER_側面判定Pt_YB)).Tile.IsWall();

					if (touchSide_L && touchSide_R) // -> 壁抜け防止のため再チェック
					{
						touchSide_L = this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y)).Tile.IsWall();
						touchSide_R = this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y)).Tile.IsWall();
					}

					if (touchSide_L && touchSide_R)
					{
						// noop
					}
					else if (touchSide_L)
					{
						this.Player.X = GameCommon.ToTileCenterX(this.Player.X - GameConsts.PLAYER_側面判定Pt_X) + GameConsts.TILE_W / 2 + GameConsts.PLAYER_側面判定Pt_X;
					}
					else if (touchSide_R)
					{
						this.Player.X = GameCommon.ToTileCenterX(this.Player.X + GameConsts.PLAYER_側面判定Pt_X) - GameConsts.TILE_W / 2 - GameConsts.PLAYER_側面判定Pt_X;
					}

					bool touchCeiling_L =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_脳天判定Pt_X, this.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

					bool touchCeiling_M =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X, this.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

					bool touchCeiling_R =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_脳天判定Pt_X, this.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

					if ((touchCeiling_L && touchCeiling_R) || touchCeiling_M)
					{
						if (this.Player.YSpeed < 0.0)
						{
							// プレイヤーと天井の反発係数
							//const double K = 1.0;
							const double K = 0.0;

							this.Player.Y = GameCommon.ToTileCenterY(Game.I.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y) + GameConsts.TILE_H / 2 + GameConsts.PLAYER_脳天判定Pt_Y;
							this.Player.YSpeed = Math.Abs(Player.YSpeed) * K;
							this.Player.JumpFrame = 0;
						}
					}
					else if (touchCeiling_L)
					{
						this.Player.X = GameCommon.ToTileCenterX(this.Player.X - GameConsts.PLAYER_脳天判定Pt_X) + GameConsts.TILE_W / 2 + GameConsts.PLAYER_脳天判定Pt_X;
					}
					else if (touchCeiling_R)
					{
						this.Player.X = GameCommon.ToTileCenterX(this.Player.X + GameConsts.PLAYER_脳天判定Pt_X) - GameConsts.TILE_W / 2 - GameConsts.PLAYER_脳天判定Pt_X;
					}

					bool touchGround =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_接地判定Pt_X, this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_接地判定Pt_X, this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y)).Tile.IsWall();

					// memo: @ 2022.7.11
					// 上昇中(ジャンプ中)に接地判定が発生することがある。
					// 接地中は重力により PlayerYSpeed がプラスに振れる。
					// -> 接地による位置等の調整は PlayerYSpeed がプラスに振れている場合のみ行う。

					if (touchGround && 0.0 < this.Player.YSpeed)
					{
						this.Player.Y = GameCommon.ToTileCenterY(Game.I.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) - GameConsts.TILE_H / 2 - GameConsts.PLAYER_接地判定Pt_Y;
						this.Player.YSpeed = 0.0;
						this.Player.JumpCount = 0;
						this.Player.AirborneFrame = 0;
					}
					else
					{
						this.Player.AirborneFrame++;
					}
				}

				// プレイヤー攻撃
				{
					if (1 <= this.Player.AttackFrame)
					{
						this.Player.Fire();
					}
				}

				// プレイヤー当たり判定をセットする。
				// -- プレイヤーのダメージ中・無敵時間中など、当たり判定無しの場合は DDCrashUtils.None をセットすること。
				{
					this.Player.Crash = DDCrashUtils.None(); // reset

					if (1 <= this.Player.DamageFrame) // ? プレイヤー・ダメージ中
					{
						// noop
					}
					else if (1 <= this.Player.InvincibleFrame) // ? プレイヤー無敵時間中
					{
						// noop
					}
					else if (1 <= this.Player.ShagamiFrame)
					{
						this.Player.Crash = DDCrashUtils.Point(new D2Point(this.Player.X, this.Player.Y + 16));
					}
					else
					{
						this.Player.Crash = DDCrashUtils.Point(new D2Point(this.Player.X, this.Player.Y));
					}
				}
				//endPlayer:

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
				// -- スタート地点(入場地点)が地面と接していると、最初のフレームでプレイヤーは上に押し出されてカメラの初期位置とズレてしまう。
				if (this.Frame == 0)
				{
					this.カメラ位置調整(true);
				}

				// ====
				// 描画ここから
				// ====

				this.DrawWall();
				this.DrawMap();
				this.Player.Draw();

				foreach (Enemy enemy in this.Enemies.Iterate())
				{
					if (enemy.DeadFlag) // ? 敵：既に死亡
						continue;

					enemy.Crash = DDCrashUtils.None(); // reset
					enemy.Draw();
				}
				foreach (Shot shot in this.Shots.Iterate())
				{
					if (shot.DeadFlag) // ? 自弾：既に死亡
						continue;

					shot.Crash = DDCrashUtils.None(); // reset
					shot.Draw();
				}

				this.Tasks.ExecuteAllTask();
				this.FrontActions.ExecuteAllAction();
				this.DrawFront();

				if (this.当たり判定表示)
				{
					// 最後に描画されるように DDGround.EL.Add() する。

					DDGround.EL.Add(() =>
					{
						DDCurtain.DrawCurtain(-0.7);

						double dPlX = this.Player.X - DDGround.Camera.X;
						double dPlY = this.Player.Y - DDGround.Camera.Y;

						DDDraw.SetBright(0.0, 1.0, 0.0);
						DDDraw.SetAlpha(0.3);
						DDDraw.DrawRect_LTRB(
							Ground.I.Picture.WhiteBox,
							dPlX - GameConsts.PLAYER_側面判定Pt_X,
							dPlY - GameConsts.PLAYER_側面判定Pt_YT,
							dPlX + GameConsts.PLAYER_側面判定Pt_X,
							dPlY + GameConsts.PLAYER_側面判定Pt_YB
							);
						DDDraw.DrawRect_LTRB(
							Ground.I.Picture.WhiteBox,
							dPlX - GameConsts.PLAYER_脳天判定Pt_X,
							dPlY - GameConsts.PLAYER_脳天判定Pt_Y,
							dPlX + GameConsts.PLAYER_脳天判定Pt_X,
							dPlY
							);
						DDDraw.DrawRect_LTRB(
							Ground.I.Picture.WhiteBox,
							dPlX - GameConsts.PLAYER_接地判定Pt_X,
							dPlY,
							dPlX + GameConsts.PLAYER_接地判定Pt_X,
							dPlY + GameConsts.PLAYER_接地判定Pt_Y
							);
						DDDraw.Reset();

						const double A = 0.7;

						DDCrashView.Draw(new DDCrash[] { this.Player.Crash }, new I3Color(255, 0, 0), 1.0);
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

								int damagePoint = Math.Min(enemy.HP, shot.AttackPoint);

								enemy.HP -= shot.AttackPoint;

								if (shot.敵を貫通する)
								{
									// noop
								}
								else // ? 敵を貫通しない -> 自弾の攻撃力と敵のHPを相殺
								{
									if (0 <= enemy.HP) // ? 丁度削りきった || 削りきれなかった -> 攻撃力を使い果たしたので、ショットは消滅
									{
										shot.AttackPoint = 0; // 攻撃力を使い果たした。
										shot.Kill();
									}
									else
									{
										shot.AttackPoint = -enemy.HP; // 過剰に削った分を残りの攻撃力として還元
									}
								}

								if (1 <= enemy.HP) // ? まだ生存している。
								{
									enemy.Damaged(shot, damagePoint);
								}
								else // ? 撃破した。
								{
									enemy.HP = 0; // 過剰に削られた分を正す。
									enemy.Kill(true);
									goto nextEnemy; // この敵は死亡したので、この敵について以降の当たり判定は不要
								}

								// ★ 敵_被弾ここまで
							}
						}
					}

					// 衝突判定：敵 x 自機
					if (
						!enemy.DeadFlag && // ? 敵：生存
						DDCrashUtils.IsCrashed(enemy.Crash, this.Player.Crash) // ? 衝突
						)
					{
						// ★ 自機_被弾ここから

						this.Player.HP -= enemy.AttackPoint;

						if (1 <= this.Player.HP) // ? まだ生存している。
						{
							this.Player.DamageFrame = 1;
						}
						else // ? 死亡した。
						{
							this.Player.HP = -1;
							this.Status.ExitDirection = 901;
							goto endGameLoop;
						}

						if (enemy.自機に当たると消滅する)
							enemy.Kill();

						// ★ 自機_被弾ここまで
					}

				nextEnemy:
					;
				}

				// ====
				// 当たり判定ここまで
				// ====

				//f_ゴミ回収(); // メソッド版_廃止

				this.Enemies.RemoveAll(v => v.DeadFlag);
				this.Shots.RemoveAll(v => v.DeadFlag);

				DDEngine.EachFrame();

				// ★★★ ゲームループの終わり ★★★
			}
		endGameLoop:
			DDEngine.FreezeInput();

			if (this.Status.ExitDirection == 901) // ? 死亡によりゲーム終了
			{
				Action drawPlayer_01 = () =>
				{
					DDDraw.DrawBegin(
						Ground.I.Picture.PlayerDamage[2],
						SCommon.ToInt(this.Player.X - DDGround.Camera.X) + 8 * (this.Player.FacingLeft ? -1 : 1),
						SCommon.ToInt(this.Player.Y - DDGround.Camera.Y) - 16
						);
					DDDraw.DrawZoom_X(this.Player.FacingLeft ? -1.0 : 1.0);
					DDDraw.DrawEnd();
				};

				Action drawPlayer_02 = () =>
				{
					DDDraw.DrawBegin(
						Ground.I.Picture.PlayerDamage[5],
						SCommon.ToInt(this.Player.X - DDGround.Camera.X) + 8 * (this.Player.FacingLeft ? -1 : 1),
						SCommon.ToInt(this.Player.Y - DDGround.Camera.Y) - 8
						);
					DDDraw.DrawZoom_X(this.Player.FacingLeft ? -1.0 : 1.0);
					DDDraw.DrawEnd();
				};

				// 1フレーム前の画面だと、プレイヤーと敵が当たっていないため不自然に見えるだろう。
				// 今回のフレームの描画を確定し、今回のフレームの画面を死亡確定画面として表示する必要がある。
				//
				DDEngine.EachFrame(); // 今回の描画を確定させてから...
				DDMain.KeepMainScreen(); // ...画面を保存する。

				DDMusicUtils.Fadeout();

				foreach (DDScene scene in DDSceneUtils.Create(40)) // 死亡確定画面
				{
					DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
					DDEngine.EachFrame();
				}
				foreach (DDScene scene in DDSceneUtils.Create(60))
				{
					this.DrawWall();
					this.DrawMap();

					DDCurtain.DrawCurtain(scene.Rate * -1.0);

					drawPlayer_01();

					DDEngine.EachFrame();
				}
				foreach (DDScene scene in DDSceneUtils.Create(30))
				{
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();

					drawPlayer_02();

					DDEngine.EachFrame();
				}
				//DDMusicUtils.Fadeout();
				DDCurtain.SetCurtain(30, -1.0);

				foreach (DDScene scene in DDSceneUtils.Create(40))
				{
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDDraw.Reset();

					drawPlayer_02();

					DDEngine.EachFrame();
				}
			}
			else if (this.Status.ExitDirection == 5) // ? メニュー操作によりゲーム終了
			{
				DDMusicUtils.Fadeout();
				DDCurtain.SetCurtain(30, -1.0);

				foreach (DDScene scene in DDSceneUtils.Create(40))
				{
					this.DrawWall();
					this.DrawMap();

					DDEngine.EachFrame();
				}
			}
			else // ? 部屋移動
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

			// ★★★★★ *****PSH (<-このパターンで検索できるようにしておく)
			// プレイヤー・ステータス反映(マップ退場時)
			// その他の反映箇所：
			// -- マップ入場時
			// -- セーブ時
			{
				// すべきこと：
				// -- 各方面に展開されているゲーム状態を this.Status に反映・格納する。

				// 例：
				//this.Status.StartHP = this.Player.HP;
				//this.Status.StartFacingLeft = this.Player.FacingLeft;
				//this.Status.Start_武器 = this.Player.武器;
				// --

				this.Status.StartHP = this.Player.HP;
				this.Status.StartFacingLeft = this.Player.FacingLeft;
				this.Status.Start_武器 = this.Player.武器;
			}

			// ★★★ end of Perform() ★★★
		}

		/// <summary>
		/// マップから離れすぎている敵・自弾の死亡フラグを立てる。
		/// </summary>
		/// <returns>タスク</returns>
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
				yield return true; // this.Enemies, this.Shots が空の場合、ループ内の yield return は実行されないので、ここにも yield return を設置しておく。
			}
		}

		/// <summary>
		/// マップから離れすぎているか判定する。
		/// </summary>
		/// <param name="x">X-座標</param>
		/// <param name="y">Y-座標</param>
		/// <returns>マップから離れすぎているか</returns>
		private bool IsProbablyEvacuated(double x, double y)
		{
			const int MGN_SCREEN_NUM = 3;

			return
				x < -DDConsts.Screen_W * MGN_SCREEN_NUM || this.Map.W * GameConsts.TILE_W + DDConsts.Screen_W * MGN_SCREEN_NUM < x ||
				y < -DDConsts.Screen_H * MGN_SCREEN_NUM || this.Map.H * GameConsts.TILE_H + DDConsts.Screen_H * MGN_SCREEN_NUM < y;
		}

		private void カメラ位置調整(bool 一瞬で)
		{
			double targCamX = this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3);
			double targCamY = this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3);

			DDUtils.ToRange(ref targCamX, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
			DDUtils.ToRange(ref targCamY, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

			if (this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W < GameConsts.TILE_W) // ? カメラの横の可動域が1タイルより狭い場合
				targCamX = (this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W) / 2; // 中心に合わせる。

			if (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H < GameConsts.TILE_H) // ? カメラの縦の可動域が1タイルより狭い場合
				targCamY = (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H) / 2; // 中心に合わせる。

			DDUtils.Approach(ref DDGround.RealCamera.X, targCamX, 一瞬で ? 0.0 : 0.8);
			DDUtils.Approach(ref DDGround.RealCamera.Y, targCamY, 一瞬で ? 0.0 : 0.8);

			DDGround.Camera.X = SCommon.ToInt(DDGround.RealCamera.X);
			DDGround.Camera.Y = SCommon.ToInt(DDGround.RealCamera.Y);
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
				//	break;

				I2Point cellPos = GameCommon.ToTablePoint(
					DDGround.RealCamera.X + DDMouse.X,
					DDGround.RealCamera.Y + DDMouse.Y
					);

				MapCell cell = Game.I.Map.GetCell(cellPos);

				if (cell.IsDefault)
				{
					// noop
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT) && 1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL)) // 左シフト・コントロール押下 -> 塗り潰し / none
				{
					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
						this.Map.Save(); // 失敗を想定して、セーブしておく

						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								{
									string tileName = LevelEditor.Dlg.GetTile();

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
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT)) // 左シフト押下 -> 移動 / none
				{
					if (1 <= DDMouse.L.GetInput())
					{
						DDGround.RealCamera.X -= DDMouse.X - lastMouseX;
						DDGround.RealCamera.Y -= DDMouse.Y - lastMouseY;

						DDUtils.ToRange(ref DDGround.RealCamera.X, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
						DDUtils.ToRange(ref DDGround.RealCamera.Y, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

						DDGround.Camera.X = SCommon.ToInt(DDGround.RealCamera.X);
						DDGround.Camera.Y = SCommon.ToInt(DDGround.RealCamera.Y);
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL)) // 左コントロール押下 -> スポイト / none
				{
					if (1 <= DDMouse.L.GetInput())
					{
						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								LevelEditor.Dlg.SetTile(cell.TileName);
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
						// none
					}
				}
				else if (1 <= DDKey.GetInput(DX.KEY_INPUT_LALT)) // 左 ALT 押下 -> 自機ワープ / none
				{
					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
						this.Player.X = cellPos.X * GameConsts.TILE_W + GameConsts.TILE_W / 2;
						this.Player.Y = cellPos.Y * GameConsts.TILE_H + GameConsts.TILE_H / 2;

						DDGround.EL.Add(SCommon.Supplier(Effects.B中爆発(this.Player.X, this.Player.Y))); // アクションが分かるように
					}
					else if (1 <= DDMouse.R.GetInput())
					{
						// none
					}
				}
				else // シフト系押下無し -> セット / クリア
				{
					if (1 <= DDMouse.L.GetInput())
					{
						switch (LevelEditor.Dlg.GetMode())
						{
							case LevelEditor.Mode_e.TILE:
								{
									string tileName = LevelEditor.Dlg.GetTile();

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
								cell.TileName = GameConsts.TILE_NONE;
								cell.Tile = new Tile_None();
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
						string tileName = LevelEditor.Dlg.GetTile();
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
			this.Wall.Draw();
		}

		private void DrawMap()
		{
			int w = this.Map.W;
			int h = this.Map.H;

			int cam_l = DDGround.Camera.X;
			int cam_t = DDGround.Camera.Y;
			int cam_r = cam_l + DDConsts.Screen_W;
			int cam_b = cam_t + DDConsts.Screen_H;

			I2Point lt = GameCommon.ToTablePoint(cam_l, cam_t);
			I2Point rb = GameCommon.ToTablePoint(cam_r, cam_b);

			// マージン付与
			// -- マップセルの範囲をはみ出て描画されるタイルのためにマージンを増やす。
			{
				const int MARGIN = 2; // マージン・マップセル数

				lt.X -= MARGIN;
				lt.Y -= MARGIN;
				rb.X += MARGIN;
				rb.Y += MARGIN;
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

					cell.Tile.Draw(tileX - cam_l, tileY - cam_t);
				}
			}
		}

		private void DrawFront()
		{
			// none -- ステータスなどを表示する。
		}

		public DDList<Enemy> Enemies = new DDList<Enemy>();
		public DDList<Shot> Shots = new DDList<Shot>();

		private void ReloadEnemies()
		{
			// 敵をクリアする前にちゃんと殺しておく。
			// -- 敵の死亡をモニタして終了(自滅)するタスクのため -- 例：Enemy_B1001.E_AttackTask
			foreach (Enemy enemy in this.Enemies.Iterate())
				enemy.DeadFlag = true;

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

		private static DDSubScreen EquipmentMenu_KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H); // 使用後は Unload すること。

		private void EquipmentMenu()
		{
			DDInput.DIR_2.FreezeInputUntilRelease();
			DDInput.DIR_4.FreezeInputUntilRelease();
			DDInput.DIR_6.FreezeInputUntilRelease();
			DDInput.DIR_8.FreezeInputUntilRelease();
			DDInput.A.FreezeInputUntilRelease();
			DDInput.B.FreezeInputUntilRelease();

			DDMain.KeepMainScreen();
			SCommon.Swap(ref DDGround.KeptMainScreen, ref EquipmentMenu_KeptMainScreen); // 使用後は Unload すること。

			TableMenu tableMenu = new TableMenu(130, 50, 24, () =>
			{
				DDDraw.DrawSimple(EquipmentMenu_KeptMainScreen.ToPicture(), 0, 0);

				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(0.0, 0.4, 0.0);
				DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, DDConsts.Screen_H / 8, DDConsts.Screen_W, DDConsts.Screen_H * 3 / 4);
				DDDraw.Reset();
			});

#if false // 条件によって初期選択をセットする。-- ★サンプルとしてキープ
			int cond = 1;
			switch (cond)
			{
				case 0: tableMenu.SetSelectedPosition(0, 0); break;
				case 1: tableMenu.SetSelectedPosition(0, 1); break;
				case 2: tableMenu.SetSelectedPosition(1, 1); break;

				default:
					break;
			}
#endif

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

						if (this.RequestReturnToTitleMenu)
							keepMenu = false;
					});
					tableMenu.AddItem(false, "戻る", color, borderColor, () => keepMenu = false);
				}

				tableMenu.Perform();

				//DDEngine.EachFrame(); // 不要
			}

			EquipmentMenu_KeptMainScreen.Unload();

			//DDInput.DIR_2.FreezeInputUntilRelease();
			//DDInput.DIR_4.FreezeInputUntilRelease();
			//DDInput.DIR_6.FreezeInputUntilRelease();
			//DDInput.DIR_8.FreezeInputUntilRelease();
			DDInput.A.FreezeInputUntilRelease();
			DDInput.B.FreezeInputUntilRelease();
		}

		private bool 当たり判定表示 = false;

		//private static DDSubScreen Pause_KeptMainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H); // 使用後は Unload すること。
		private static DDSubScreen Pause_KeptMainScreen { get { return EquipmentMenu_KeptMainScreen; } }

		/// <summary>
		/// ポーズメニュー
		/// </summary>
		private void Pause()
		{
			// old
			//DDMain.KeepMainScreen();
			//SCommon.Swap(ref DDGround.KeptMainScreen, ref Pause_KeptMainScreen); // 使用後は Unload すること。

			SimpleMenu simpleMenu = new SimpleMenu()
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
					selectIndex,
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
					true,
					false // 武器メニューの配下であるため Pound でも良いと判断
					);

				switch (selectIndex)
				{
					case 0:
						using (new SettingMenu()
						{
							SimpleMenu = new SimpleMenu()
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
			DDEngine.FreezeInput();
			//Pause_KeptMainScreen.Unload(); // old

			// old
			//DDInput.A.FreezeInputUntilRelease();
			//DDInput.B.FreezeInputUntilRelease();
		}

		/// <summary>
		/// ポーズメニュー(デバッグ用)
		/// </summary>
		private void DebugPause()
		{
			DDMain.KeepMainScreen();

			SimpleMenu simpleMenu = new SimpleMenu()
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
					selectIndex,
					40,
					40,
					40,
					24,
					"デバッグ用メニュー",
					new string[]
					{
						"装備している武器の切り替え [ 現在装備している武器：" + ShotCatalog.武器_e_Names[(int)this.Player.武器] + " ]",
						"強制遅延 [ 現在の設定：" + DDEngine.SlowdownLevel + " ]",
						"当たり判定表示 [ 現在の設定：" + this.当たり判定表示 + " ]",
						"ゲームに戻る",
					},
					true,
					true
					);

				switch (selectIndex)
				{
					case 0:
						this.Player.武器 = (ShotCatalog.武器_e)(((int)this.Player.武器 + 1) % ShotCatalog.武器_e_Names.Length);
						break;

					case 1:
						if (DDEngine.SlowdownLevel == 0)
							DDEngine.SlowdownLevel = 1;
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

			DDInput.A.FreezeInputUntilRelease();
			DDInput.B.FreezeInputUntilRelease();
		}
	}
}
