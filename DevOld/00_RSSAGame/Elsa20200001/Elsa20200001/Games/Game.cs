using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Attacks;
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

		public enum EndReason_e
		{
			ReturnToTitleMenu = 1,
			StageClear,
		}

		public EndReason_e EndReason = EndReason_e.ReturnToTitleMenu;

		// <---- ret

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

		public Map Map;
		private Wall Wall;

		private bool CamSlideMode; // ? カメラ・スライド_モード中
		private bool CamSlided;
		public int CamSlideX; // -1 ～ 1
		public int CamSlideY; // -1 ～ 1

		public int Frame;
		public bool UserInputDisabled = false;

		public DDTaskList EL_AfterDrawMap = new DDTaskList();

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

					this.Player.Y += GameConsts.TILE_H / 2;
					this.Player.Y -= GameConsts.PLAYER_接地判定Pt_Y;
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

				this.Player.HP = this.Status.StartHP;
				this.Player.FacingLeft = this.Status.StartFacingLeft;
				//plStatus = this.Status.StartPlayerStatus;

				// ★ここまでサンプルとしてキープ

				if (this.Status.StartPlayerStatus != null)
				{
					this.Player.X = this.Status.StartPlayerStatus.X;
					this.Player.Y = this.Status.StartPlayerStatus.Y;

					if (this.Status.StartPlayerStatus.Ladder)
						this.Player.Attack = new Attack_Ladder();

					this.Player.FacingLeft = this.Status.StartPlayerStatus.FacingLeft;
				}
			}

			this.Wall = WallCatalog.Create(this.Map.WallName);

			MusicCollection.Get(this.Map.MusicName).Play();

			DDGround.Camera.X = this.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.Camera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

			//DDCurtain.SetCurtain(0, -1.0); // del -- マップ移動.cs により明転するため
			DDCurtain.SetCurtain(10);

			if (this.Status.StartPointDirection == 5)
				this.プレイヤー登場();

			this.カメラ位置調整(true);

			DDEngine.FreezeInput();

			bool jumpLock = false; // ? ジャンプ・ロック // ジャンプしたらボタンを離すまでロックする。

			for (this.Frame = 0; ; this.Frame++)
			{
				if (
					//Game.I.Player.Attack == null && // ? プレイヤーの攻撃モーション中ではない。// モーション中でも構わないはず。
					!this.UserInputDisabled &&
					DDInput.PAUSE.GetInput() == 1
					)
				{
					this.EquipmentMenu();
					//this.Pause(); // old

					if (this.Pause_ReturnToTitleMenu)
					{
						this.Status.ExitDirection = 5;
						this.プレイヤー退場();
						break;
					}
				}
				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_RETURN) == 1)
				{
					this.DebugPause();
				}

				{
					//bool disabled = false;
					bool disabled = 1 <= this.Player.DeadFrame; // プレイヤー死亡中はカメラ移動抑止

					if (!disabled)
					{
						this.カメラ位置調整(false);
					}
				}

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1) // エディットモード(デバッグ用)
				{
					this.Edit();
					this.ReloadEnemies();
					this.Frame = 0;
				}

				if (this.Player.Attack != null) // プレイヤー攻撃中
				{
					if (this.Player.Attack.EachFrame()) // ? このプレイヤー攻撃を継続する。
						goto endPlayer;

					this.Player.Attack = null; // プレイヤー攻撃_終了
				}

				// プレイヤー入力
				{
					bool deadOrDamageOrUID = 1 <= this.Player.DeadFrame || 1 <= this.Player.DamageFrame || this.UserInputDisabled;
					bool move = false;
					bool camSlide = false;
					int jump = 0;
					bool under = false;
					int attack = 0;

					if (!deadOrDamageOrUID && 1 <= DDInput.DIR_2.GetInput())
					{
						I2Point pt1 = GameCommon.ToTablePoint(new D2Point(this.Player.X, this.Player.Y));
						I2Point pt2 = pt1;

						pt2.Y++;

						if (
							DDInput.A.GetInput() <= 0 && // ? ジャンプ押下していない。-- 梯子の頂上付近でジャンプ状態で固まらないように
							Map.GetCell(pt1).Tile.GetKind() != Tile.Kind_e.LADDER &&
							Map.GetCell(pt2).Tile.GetKind() == Tile.Kind_e.LADDER
							)
						{
							// ★ 梯子を降り始める。

							this.Player.Y = pt2.Y * GameConsts.TILE_H - 10; // 初期位置、要調整？

							this.Player.Attack = new Attack_Ladder();
							goto endPlayer;
						}

						under = true;
					}
					if (!deadOrDamageOrUID && 1 <= DDInput.DIR_8.GetInput())
					{
						if (Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.Player.X, this.Player.Y))).Tile.GetKind() == Tile.Kind_e.LADDER)
						{
							// ★ 梯子を登り始める。

							this.Player.Attack = new Attack_Ladder();
							goto endPlayer;
						}
					}

					// 入力抑止中であるか否かに関わらず左右の入力は受け付けるようにする。
					int freezeInputFrameBackup = DDEngine.FreezeInputFrame;
					DDEngine.FreezeInputFrame = 0;

					if (!deadOrDamageOrUID && 1 <= DDInput.DIR_4.GetInput())
					{
						this.Player.FacingLeft = true;
						move = true;
					}
					if (!deadOrDamageOrUID && 1 <= DDInput.DIR_6.GetInput())
					{
						this.Player.FacingLeft = false;
						move = true;
					}

					DDEngine.FreezeInputFrame = freezeInputFrameBackup; // restore

					if (1 <= DDInput.L.GetInput())
					{
						move = false;
						camSlide = true;
					}
					if (!deadOrDamageOrUID && 1 <= DDInput.A.GetInput())
					{
						jump = DDInput.A.GetInput();
					}
					if (!deadOrDamageOrUID && 1 <= DDInput.B.GetInput())
					{
						attack = DDInput.B.GetInput();
					}

					if (move)
						this.Player.MoveFrame++;
					else
						this.Player.MoveFrame = 0;

					if (jump == 0)
						jumpLock = false;

					if (1 <= this.Player.JumpFrame)
					{
						if (1 <= jump)
						{
							this.Player.JumpFrame++;
						}
						else
						{
							// ★ ジャンプを中断・終了した。

							this.Player.JumpFrame = 0;

							if (this.Player.YSpeed < 0.0)
								this.Player.YSpeed /= 2.0;
						}
					}
					else
					{
						// 事前入力 == 着地前の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。
						// 入力猶予 == 落下(地面から離れた)直後の数フレーム間にジャンプボタンを押し始めてもジャンプできるようにする。

						const int 事前入力時間 = 5;
						const int 入力猶予時間 = 5;

						if (this.Player.AirborneFrame < 入力猶予時間 && this.Player.JumpCount == 0) // ? 接地状態からのジャンプが可能な状態
						{
							if (1 <= jump && jump < 事前入力時間 && !jumpLock)
							{
								if (
									under &&
									this.Player.AirborneFrame == 0 // スライディングは入力猶予無し
									)
								{
									// ★ スライディング開始

									this.Player.Attack = new Attack_Sliding();
									goto endPlayer;
								}
								else
								{
									// ★ ジャンプを開始した。

									this.Player.JumpFrame = 1;
									this.Player.JumpCount = 1;

									this.Player.YSpeed = GameConsts.PLAYER_ジャンプ初速度;

									jumpLock = true;
								}
							}
							else
							{
								this.Player.JumpCount = 0;
							}
						}
						else // ? 接地状態からのジャンプが「可能ではない」状態
						{
							// 滞空状態に入ったら「通常ジャンプの状態」にする。
							if (this.Player.JumpCount < 1)
								this.Player.JumpCount = 1;

							if (1 <= jump && jump < 事前入力時間 && this.Player.JumpCount < GameConsts.JUMP_MAX && !jumpLock)
							{
								// ★ 空中(n-段)ジャンプを開始した。

								this.Player.JumpFrame = 1;
								this.Player.JumpCount++;

								this.Player.YSpeed = GameConsts.PLAYER_ジャンプ初速度;

								//DDGround.EL.Add(SCommon.Supplier(Effects.空中ジャンプの足場(this.Player.X, this.Player.Y + 48)));

								jumpLock = true;
							}
							else
							{
								// noop
							}
						}
					}

					if (this.Player.JumpFrame == 1) // ? ジャンプ開始
					{
						Ground.I.SE.Coin01.Play(); // test test test test test
					}

					if (camSlide)
					{
						if (DDInput.DIR_4.IsPound())
						{
							this.CamSlided = true;
							this.CamSlideX--;
						}
						if (DDInput.DIR_6.IsPound())
						{
							this.CamSlided = true;
							this.CamSlideX++;
						}
						if (DDInput.DIR_8.IsPound())
						{
							this.CamSlided = true;
							this.CamSlideY--;
						}
						if (DDInput.DIR_2.IsPound())
						{
							this.CamSlided = true;
							this.CamSlideY++;
						}
						DDUtils.ToRange(ref this.CamSlideX, -1, 1);
						DDUtils.ToRange(ref this.CamSlideY, -1, 1);
					}
					else
					{
						if (this.CamSlideMode && !this.CamSlided)
						{
							this.CamSlideX = 0;
							this.CamSlideY = 0;
						}
						this.CamSlided = false;
					}
					this.CamSlideMode = camSlide;

					bool 攻撃ボタンを押した瞬間撃つ = Ground.I.ショットのタイミング == Ground.ショットのタイミング_e.ショットボタンを押し下げた時;

					if (1 <= attack)
					{
						if (攻撃ボタンを押した瞬間撃つ && attack == 1)
						{
							if (!deadOrDamageOrUID) // 死亡中_攻撃_抑止
							{
								this.Player.Shoot(1);
							}
						}
						this.Player.ShootingFrame = GameConsts.PLAYER_SHOOTING_FRAME_MAX;
						this.Player.ShotChargePCT++;
						DDUtils.Minim(ref this.Player.ShotChargePCT, 100);
					}
					else
					{
						DDUtils.CountDown(ref this.Player.ShootingFrame);
						int level = GameCommon.ShotChargePCTToLevel(this.Player.ShotChargePCT);
						int chargePct = this.Player.ShotChargePCT;
						this.Player.ShotChargePCT = 0;

						if (攻撃ボタンを押した瞬間撃つ ? 2 <= level : 1 <= chargePct)
						{
							if (!deadOrDamageOrUID) // 死亡中_攻撃_抑止
							{
								this.Player.Shoot(level);
							}
						}
					}
				}

				//startDead:
				if (1 <= this.Player.DeadFrame) // ? プレイヤー死亡中
				{
					if (GameConsts.PLAYER_DEAD_FRAME_MAX < ++this.Player.DeadFrame)
					{
						this.Player.DeadFrame = 0;
						this.Status.ExitDirection = 5;
						break;
					}
					int frame = this.Player.DeadFrame; // 値域 == 2 ～ GameConsts.PLAYER_DEAD_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_DEAD_FRAME_MAX, frame);

					// プレイヤー死亡中の処理
					{
						if (frame == 2) // 初回
						{
							DDMain.KeepMainScreen();

							foreach (DDScene scene in DDSceneUtils.Create(30))
							{
								DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
								DDEngine.EachFrame();
							}
							if (!DDUtils.IsOutOfCamera(new D2Point(this.Player.X, this.Player.Y))) // 画面外で死亡したら視覚効果は無し
							{
								Effects.ティウンティウン_AddToEL(this.Player.X, this.Player.Y);
							}

							// TODO: SE
						}
					}
				}
				//endDead:

				//startDamage:
				if (1 <= this.Player.DamageFrame) // ? プレイヤー・ダメージ中
				{
					if (GameConsts.PLAYER_DAMAGE_FRAME_MAX < ++this.Player.DamageFrame)
					{
						this.Player.DamageFrame = 0;
						this.Player.InvincibleFrame = 1;
						//this.Player.YSpeed = 0.0;
						goto endDamage;
					}
					int frame = this.Player.DamageFrame; // 値域 == 2 ～ GameConsts.PLAYER_DAMAGE_FRAME_MAX
					double rate = DDUtils.RateAToB(2, GameConsts.PLAYER_DAMAGE_FRAME_MAX, frame);

					// プレイヤー・ダメージ中の処理
					{
						if (frame == 2) // init
							this.Player.YSpeed = 0.0;

						if (frame % 30 == 2)
							DDGround.EL.Add(SCommon.Supplier(Effects.ヒットバック(Game.I.Player.X, Game.I.Player.Y - 50.0)));

						this.Player.X -= 1.0 * (this.Player.FacingLeft ? -1 : 1);
					}
				}
			endDamage:

				//startInvincible:
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
				if (
					this.Player.DeadFrame == 0 //&&
					//this.Player.DamageFrame == 0 // ダメージ中も落下する。
					)
				{
					if (1 <= this.Player.MoveFrame)
					{
						double SPEED = GameConsts.PLAYER_SPEED;

						SPEED = Math.Min((this.Player.MoveFrame + 1) / 2, SPEED); // 走り出し時に加速する。
						SPEED *= this.Player.FacingLeft ? -1 : 1;

						this.Player.X += SPEED;
					}
					else
						this.Player.X = (double)SCommon.ToInt(this.Player.X);

					// 重力による加速
					this.Player.YSpeed += GameConsts.PLAYER_GRAVITY;

					// 自由落下の最高速度を超えないように矯正
					DDUtils.Minim(ref this.Player.YSpeed, GameConsts.PLAYER_FALL_SPEED_MAX);

					// 自由落下
					this.Player.Y += this.Player.YSpeed;
				}

				// プレイヤー位置矯正
				{
					bool touchSide_L =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X, this.Player.Y + GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();

					bool touchSide_R =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X, this.Player.Y + GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();

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
						this.Player.X = (double)SCommon.ToInt(this.Player.X / GameConsts.TILE_W) * GameConsts.TILE_W + GameConsts.PLAYER_側面判定Pt_X;
					}
					else if (touchSide_R)
					{
						this.Player.X = (double)SCommon.ToInt(this.Player.X / GameConsts.TILE_W) * GameConsts.TILE_W - GameConsts.PLAYER_側面判定Pt_X;
					}

					bool touchCeiling =
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_脳天判定Pt_X, this.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall() ||
						this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_脳天判定Pt_X, this.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y)).Tile.IsWall();

					if (touchCeiling)
					{
						if (this.Player.YSpeed < 0.0)
						{
							double plY = ((int)((this.Player.Y - GameConsts.PLAYER_脳天判定Pt_Y) / GameConsts.TILE_H) + 1) * GameConsts.TILE_H + GameConsts.PLAYER_脳天判定Pt_Y;

							this.Player.Y = plY;
							this.Player.YSpeed = 0.0;
						}
					}

					bool touchGround =
						this.Map.IsGroundPoint(this.Player.X - GameConsts.PLAYER_接地判定Pt_X, this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) ||
						this.Map.IsGroundPoint(this.Player.X + GameConsts.PLAYER_接地判定Pt_X, this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y);

					if (!touchGround && this.Player.AirborneFrame == 0) // ★ 接地時のみ接地判定を拡張する。
					{
						touchGround =
							this.Map.IsGroundPoint(this.Player.X - GameConsts.PLAYER_接地判定Pt_X_接地時のみ, this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) ||
							this.Map.IsGroundPoint(this.Player.X + GameConsts.PLAYER_接地判定Pt_X_接地時のみ, this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y);

						// 壁面に立ってしまわないように
						touchGround = touchGround &&
							!this.Map.IsGroundPoint(this.Player.X - GameConsts.PLAYER_接地判定Pt_X_接地時のみ, this.Player.Y) &&
							!this.Map.IsGroundPoint(this.Player.X + GameConsts.PLAYER_接地判定Pt_X_接地時のみ, this.Player.Y);
					}

					if (touchGround)
					{
						if (0.0 < this.Player.YSpeed)
						{
							double plY = (int)((this.Player.Y + GameConsts.PLAYER_接地判定Pt_Y) / GameConsts.TILE_H) * GameConsts.TILE_H - GameConsts.PLAYER_接地判定Pt_Y;

							this.Player.Y = plY;
							this.Player.YSpeed = 0.0;
						}
					}

					if (touchGround)
					{
						this.Player.JumpCount = 0;
						this.Player.AirborneFrame = 0;
					}
					else
						this.Player.AirborneFrame++;

					for (int c = 0; c < 20; c++) // ★ スライディング時のハマり防止
					{
						bool xL2 = this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X * 2, this.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();
						bool xL1 = this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X - GameConsts.PLAYER_側面判定Pt_X * 1, this.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();
						bool xR1 = this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X * 1, this.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();
						bool xR2 = this.Map.GetCell(GameCommon.ToTablePoint(this.Player.X + GameConsts.PLAYER_側面判定Pt_X * 2, this.Player.Y - GameConsts.PLAYER_側面判定Pt_Y)).Tile.IsWall();

						if (!xL1 || !xR1)
							break;

						if (!xL2)
							this.Player.X--;

						if (!xR2)
							this.Player.X++;
					}
				}
			endPlayer:

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
					if (this.Map.穴に落ちたら死亡)
					{
						// DeadFrame 終了したら即 break; するので、この判定で良いはず
						//
						if (this.Player.DeadFrame == 0)
							this.Player.DeadFrame = 1;
					}
					else
					{
						this.Status.ExitDirection = 2;
						break;
					}
				}

				// 画面遷移時の微妙なカメラ位置ズレ解消
				// -- スタート地点(入場地点)が地面と接していると、最初のフレームでプレイヤーは上に押し出されて(ゲームによっては)カメラの初期位置とズレてしまう。
				// ---- なのでこの場所で処理する。
				if (this.Frame == 0)
				{
					this.カメラ位置調整(true);
				}

				// プレイヤーの当たり判定を plCrash にセットする。

				DDCrash plCrash = this.Player.GetCrash();
				//DDCrash plCrash = DDCrashUtils.Circle(new D2Point(this.Player.X, this.Player.Y), 10.0);
				//DDCrash plCrash = DDCrashUtils.Point(new D2Point(this.Player.X, this.Player.Y));

				// ====
				// 描画ここから
				// ====

				this.DrawWall();
				this.DrawMap();
				this.EL_AfterDrawMap.ExecuteAllTask();
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

				{
					const int DRAW_L = 20;
					const int DRAW_T = 20;
					const int DRAW_W = 20;
					const int DRAW_H = 150;

					double rate = (double)this.Player.HP / GameConsts.PLAYER_HP_MAX;
					int h1 = SCommon.ToInt(rate * DRAW_H);
					int h2 = DRAW_H - h1;

					DDDraw.SetAlpha(0.8);

					if (1 <= h2)
					{
						DDDraw.SetBright(new I3Color(0, 0, 0));
						DDDraw.DrawRect(Ground.I.Picture.WhiteBox, DRAW_L, DRAW_T, DRAW_W, h2);
					}
					if (1 <= h1)
					{
						DDDraw.SetBright(new I3Color(255, 255, 0));
						DDDraw.DrawRect(Ground.I.Picture.WhiteBox, DRAW_L, DRAW_T + h2, DRAW_W, h1);
					}
					DDDraw.Reset();
				}

				if (this.当たり判定表示)
				{
					// 最後に描画されるように DDGround.EL.Add() する。

					DDGround.EL.Add(() =>
					{
						DDCurtain.DrawCurtain(-0.7);

						const double A = 0.7;

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

				// ? 無敵な攻撃中 -> 敵 x 自機 の衝突判定を行わない。
				bool attackInvincibleMode =
					Game.I.Player.Attack != null &&
					Game.I.Player.Attack.IsInvincibleMode();

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

								if (enemy.防御中)
								{
									if (shot.敵を貫通する)
									{
										// TODO: 攻撃が効かない音
									}
									else
									{
										// 自弾を跳ね返す。
										{
											shot.DeadFlag = true;
											DDGround.EL.Add(SCommon.Supplier(Effects.自弾跳ね返し(shot)));
										}
									}
									continue; // この自弾の攻撃は無効なので、次の弾の判定へ移る。
								}

								// 貫通武器について、貫通中に複数回クラッシュしないように制御する。
								// -- 複数の敵に同時に当たると意図通りにならないが、厳格に制御する必要は無いので、看過する。

								if (shot.LastCrashedEnemy == enemy) // ? 直前にクラッシュした -> 複数回クラッシュしない。
								{
									shot.CurrCrashedEnemy = enemy;
									continue;
								}
								enemy.HP -= shot.AttackPoint;

								if (shot.敵を貫通する)
								{
									shot.CurrCrashedEnemy = enemy;
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
									enemy.HP = 0; // 過剰に削った分を正す。
									enemy.Kill();
									goto nextEnemy; // この敵は死亡したので、この敵について以降の当たり判定は不要
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
						!attackInvincibleMode && // 無敵になる攻撃中ではない。
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

				nextEnemy:
					;
				}

				// ====
				// 当たり判定ここまで
				// ====

				foreach (Shot shot in this.Shots.Iterate()) // 自弾・フレーム事後処理
				{
					shot.CurrCrashedEnemy = shot.LastCrashedEnemy;
					shot.LastCrashedEnemy = null;

					// 壁への衝突
					// 壁への当たり判定は自弾の「中心座標のみ」であることに注意して下さい。
					{
						if (
							!shot.DeadFlag && // ? 自弾：生存
							!shot.壁をすり抜ける && // ? この自弾は壁に当たる。
							this.Map.GetCell(GameCommon.ToTablePoint(shot.X, shot.Y)).Tile.IsWall() // ? 壁に当たった。
							)
							shot.Kill();
					}
				}

				f_ゴミ回収();

				this.Enemies.RemoveAll(v => v.DeadFlag);
				this.Shots.RemoveAll(v => v.DeadFlag);

				DDEngine.EachFrame();

				// ★★★ ゲームループの終わり ★★★
			}
			DDEngine.FreezeInput();

			GameStatus.StartPlayerStatusInfo plStatus = null;

			if (this.Status.ExitDirection == 5)
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
			else
			{
				foreach (DDScene scene in DDSceneUtils.Create(5)) // なめらかなカメラ移動のため
				{
					this.カメラ位置調整(false);

					this.DrawWall();
					this.DrawMap();

					DDEngine.EachFrame();
				}
				this.カメラ位置調整(true);

				Map nextMap;

				{
					int xa = 0;
					int ya = 0;

					switch (this.Status.ExitDirection)
					{
						case 2: ya = 1; break;
						case 4: xa = -1; break;
						case 6: xa = 1; break;
						case 8: ya = -1; break;

						default:
							throw null; // never
					}
					this.World.Move(xa, ya); // 一時的に移動

					nextMap = new Map(GameCommon.GetMapFile(this.World.GetCurrMapName()));

					this.World.Move(-xa, -ya); // restore
				}

				plStatus = new GameStatus.StartPlayerStatusInfo()
				{
					X = this.Player.X,
					Y = this.Player.Y,
					Ladder = this.Player.Attack != null && this.Player.Attack is Attack_Ladder,
					FacingLeft = this.Player.FacingLeft,
				};

				マップ移動.Perform(this.Map, nextMap, DDGround.Camera, this.Status.ExitDirection, plStatus);
			}

			// ★★★★★ *****PSH (<-このパターンで検索できるようにしておく)
			// プレイヤー・ステータス反映(マップ退場時)
			// その他の反映箇所：
			// -- マップ入場時
			// -- セーブ時
			{
				// すべきこと：
				// -- 各方面に展開されているゲーム状態を this.Status に反映・格納する。

				this.Status.StartHP = this.Player.HP;
				this.Status.StartFacingLeft = this.Player.FacingLeft;
				this.Status.StartPlayerStatus = plStatus;

				// ★ここまでサンプルとしてキープ
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

		private void プレイヤー登場()
		{
			this.カメラ位置調整(true);

			foreach (DDScene scene in DDSceneUtils.Create(10)) // 画面暗転解除のため
			{
				this.DrawWall();
				this.DrawMap();

				DDEngine.EachFrame();
			}

			double destX = this.Player.X - DDGround.ICamera.X;
			double destY = this.Player.Y - DDGround.ICamera.Y - 8.0; // プレイヤー位置矯正の実行前であるため、手動で調整を入れる。

			double x = destX;
			double y = 0.0;
			double yAdd = 3.0; // 初速度

			for (; ; )
			{
				y += yAdd;
				yAdd += 1.0; // 加速

				if (destY < y)
					break;

				this.DrawWall();
				this.DrawMap();

				DDDraw.DrawCenter(Ground.I.Picture.Chara_A01_Telepo_01, x, y);

				DDEngine.EachFrame();
			}
			y = destY;

			for (int c = 0; c < 9; c++)
			{
				DDPicture picture;

				if (c / 3 % 2 == 0)
					picture = Ground.I.Picture.Chara_A01_Telepo_02;
				else
					picture = Ground.I.Picture.Chara_A01_Telepo_03;

				this.DrawWall();
				this.DrawMap();

				DDDraw.DrawCenter(picture, x, y);

				DDEngine.EachFrame();
			}
		}

		private void プレイヤー退場()
		{
			double x = this.Player.X - DDGround.ICamera.X;
			double y = this.Player.Y - DDGround.ICamera.Y;

			for (int c = 0; c < 9; c++)
			{
				DDPicture picture;

				if (c / 3 % 2 == 0)
					picture = Ground.I.Picture.Chara_A01_Telepo_02;
				else
					picture = Ground.I.Picture.Chara_A01_Telepo_03;

				this.DrawWall();
				this.DrawMap();

				DDDraw.DrawCenter(picture, x, y);

				DDEngine.EachFrame();
			}

			double destX = x;
			double destY = 0.0;
			double yAdd = -3.0; // 初速度

			for (; ; )
			{
				y += yAdd;
				yAdd -= 1.0; // 加速

				if (y < destY)
					break;

				this.DrawWall();
				this.DrawMap();

				DDDraw.DrawCenter(Ground.I.Picture.Chara_A01_Telepo_01, x, y);

				DDEngine.EachFrame();
			}
		}

		private void カメラ位置調整(bool 一瞬で)
		{
			double targCamX = this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3);
			double targCamY = this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3);

			DDUtils.ToRange(ref targCamX, 0.0, this.Map.W * GameConsts.TILE_W - DDConsts.Screen_W);
			DDUtils.ToRange(ref targCamY, 0.0, this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H);

			if (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H < GameConsts.TILE_H) // ? カメラの縦の可動域が1タイルより狭い場合
				//targCamY = (this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H) / 2; // 中心に合わせる。
				targCamY = this.Map.H * GameConsts.TILE_H - DDConsts.Screen_H; // 下に合わせる。

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
				//	break;

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
					this.Map.Save(); // 失敗を想定して、セーブしておく

					if (DDMouse.L.GetInput() == -1) // クリックを検出
					{
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
			this.Wall.Draw();
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

			switch (this.Status.Equipment)
			{
				case ShotCatalog.武器_e.Normal: tableMenu.SetSelectedPosition(0, 1); break;
				case ShotCatalog.武器_e.跳ねる陰陽玉: tableMenu.SetSelectedPosition(0, 2); break;
				case ShotCatalog.武器_e.ハンマー陰陽玉: tableMenu.SetSelectedPosition(0, 3); break;
				case ShotCatalog.武器_e.AirShooter: tableMenu.SetSelectedPosition(0, 4); break;
				case ShotCatalog.武器_e.MagnetAir: tableMenu.SetSelectedPosition(0, 5); break;

				default:
					break;
			}

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
					tableMenu.AddItem(true, "ＥＱＵＩＰＭＥＮＴ", color, borderColor);

					Action<string, ShotCatalog.武器_e, bool> a_addEquipment = (title, equipment, 取得済み) =>
					{
						Action a_desided = () =>
						{
							this.Status.Equipment = equipment;
							keepMenu = false;
						};

						if (!取得済み)
							tableMenu.AddItem(false, title, 未取得Color, 未取得BorderColor, () => { });
						else if (this.Status.Equipment == equipment)
							tableMenu.AddItem(false, title, 装備中Color, 装備中BorderColor, a_desided);
						else
							tableMenu.AddItem(false, title, color, borderColor, a_desided);
					};

					a_addEquipment("通常武器", ShotCatalog.武器_e.Normal, true);
					a_addEquipment("跳ねる陰陽玉", ShotCatalog.武器_e.跳ねる陰陽玉, this.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_跳ねる陰陽玉]);
					a_addEquipment("ハンマー陰陽玉", ShotCatalog.武器_e.ハンマー陰陽玉, this.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_ハンマー陰陽玉]);
					a_addEquipment("ＡｉｒＳｈｏｏｔｅｒ", ShotCatalog.武器_e.AirShooter, this.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_エアーシューター]);
					a_addEquipment("ＭａｇｎｅｔＡｉｒ", ShotCatalog.武器_e.MagnetAir, this.Status.InventoryFlags[GameStatus.Inventory_e.取得済み_マグネットエアー]);

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
						"ゲームに戻る",
					},
					selectIndex,
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
									DDCurtain.DrawCurtain(-0.5);
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

		private bool 当たり判定表示 = false;

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
					50,
					50,
					50,
					24,
					"デバッグ用メニュー",
					new string[]
					{
						"キャラクタ切り替え [ 現在のキャラクタ：---- ]",
						"デバッグ強制遅延 [ 現在の設定：" + DDEngine.SlowdownLevel + " ]",
						"当たり判定表示 [ 現在の設定：" + this.当たり判定表示 + " ]",
						"ゲームに戻る",
					},
					selectIndex,
					true
					);

				switch (selectIndex)
				{
					case 0:
						// none
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

			DDInput.A.FreezeInputUntilRelease = true;
			DDInput.B.FreezeInputUntilRelease = true;
		}
	}
}
