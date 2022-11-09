using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;
using Charlotte.Games.Enemies;
using Charlotte.Games.Shots;
using Charlotte.Games.Attacks;

namespace Charlotte.Games
{
	/// <summary>
	/// プレイヤーに関する情報と機能
	/// 唯一のインスタンスを Game.I.Player に保持する。
	/// </summary>
	public class Player
	{
		public enum Chara_e
		{
			TEWI,
			CIRNO,
		}

		public static string[] Chara_e_Names = new string[]
		{
			"因幡てゐ",
			"チルノ",
		};

		public Chara_e Chara;
		public double X;
		public double Y;
		public double YSpeed;
		public bool FacingLeft;
		public DDCrash Crash;
		public int MoveFrame;
		public bool MoveSlow; // ? 低速移動
		public bool JumpLock; // ? ジャンプ・ロック -- ジャンプしたらボタンを離すまでロックする。
		public int JumpFrame;
		public int JumpCount;
		public int AirborneFrame; // 0 == 接地状態, 1～ == 滞空状態
		public int ShagamiFrame; // 0 == 無効, 1～ == しゃがみ中
		public int UwamukiFrame; // 0 == 無効, 1～ == 上向き中
		public int ShitamukiFrame; // 0 == 無効, 1～ == 下向き中
		public int AttackFrame; // 0 == 無効, 1～ == 攻撃中
		public int DamageFrame; // 0 == 無効, 1～ == ダメージ中
		public int InvincibleFrame; // 0 == 無効, 1～ == 無敵時間中

		/// <summary>
		/// 体力
		/// -1 == 死亡
		/// 0 == (不使用・予約)
		/// 1～ == 残り体力
		/// </summary>
		public int HP = 1;

		public bool FacingTop
		{
			get { return 1 <= this.UwamukiFrame; }
		}

		public int 上昇_Frame;
		public int 下降_Frame;
		public int StandFrame = SCommon.IMAX / 2; // 0 == 無効, 1～ == しゃがんでいない(立っている・跳んでいる)

		/// <summary>
		/// プレイヤーの攻撃モーション
		/// -- 攻撃(Attack)と言っても攻撃以外の利用(スライディング・梯子など)も想定する。
		/// null の場合は無効
		/// null ではない場合 Attack.EachFrame() が実行される代わりに、プレイヤーの入力・被弾処理などは実行されない。
		/// </summary>
		public Attack Attack = null;

		/// <summary>
		/// プレイヤー描画の代替タスクリスト
		/// 空の場合は無効
		/// 空ではない場合 Draw_EL.ExecuteAllTask() が実行される代わりに Draw() の主たる処理は実行されない。
		/// --- プレイヤーの攻撃モーションから使用されることを想定する。
		/// </summary>
		public DDTaskList Draw_EL = new DDTaskList();

		public void Draw()
		{
			if (1 <= this.Draw_EL.Count)
			{
				this.Draw_EL.ExecuteAllTask();
				return;
			}

			DDPicture picture = null;

			switch (Game.I.Player.Chara) // キャラクタ別_各種モーション
			{
				case Chara_e.TEWI:
					{
						if (1 <= Game.I.Player.ShagamiFrame) // てゐ_しゃがみ
						{
							picture = Ground.I.Picture2.Tewi_しゃがみ[Math.Min(Game.I.Player.ShagamiFrame / 3, Ground.I.Picture2.Tewi_しゃがみ.Length - 1)];
						}
						else if (Game.I.Player.AirborneFrame != 0) // てゐ_滞空状態
						{
							if (1 <= Game.I.Player.上昇_Frame) // てゐ_上昇
							{
								int koma = Game.I.Player.上昇_Frame;
								koma--;
								koma /= 3;
								koma = Math.Min(koma, Ground.I.Picture2.Tewi_ジャンプ_上昇.Length - 1);

								picture = Ground.I.Picture2.Tewi_ジャンプ_上昇[koma];
							}
							else // てゐ_下降
							{
								int koma = Game.I.Player.下降_Frame;
								koma--;
								koma /= 3;

								if (Ground.I.Picture2.Tewi_ジャンプ_下降.Length <= koma)
								{
									koma -= Ground.I.Picture2.Tewi_ジャンプ_下降.Length;
									koma %= 3;
									koma = Ground.I.Picture2.Tewi_ジャンプ_下降.Length - 3 + koma;
								}
								picture = Ground.I.Picture2.Tewi_ジャンプ_下降[koma];
							}
						}
						else if (1 <= this.MoveFrame) // てゐ_移動
						{
							if (this.MoveSlow)
							{
								picture = Ground.I.Picture2.Tewi_歩く[Game.I.Frame / 10 % Ground.I.Picture2.Tewi_歩く.Length];
							}
							else
							{
								picture = Ground.I.Picture2.Tewi_走る[Game.I.Frame / 5 % Ground.I.Picture2.Tewi_走る.Length];
							}
						}
						else // てゐ_立ち
						{
							int koma = this.StandFrame / 3;

							if (koma < Ground.I.Picture2.Tewi_しゃがみ解除.Length)
								picture = Ground.I.Picture2.Tewi_しゃがみ解除[koma];
							else
								picture = Ground.I.Picture2.Tewi_立ち[Game.I.Frame / 10 % Ground.I.Picture2.Tewi_立ち.Length];
						}
					}
					break;

				case Chara_e.CIRNO:
					{
						if (1 <= Game.I.Player.ShagamiFrame) // チルノ_しゃがみ
						{
							picture = Ground.I.Picture2.Cirno_しゃがみ[Math.Min(Game.I.Player.ShagamiFrame / 3, Ground.I.Picture2.Cirno_しゃがみ.Length - 1)];
						}
						else if (Game.I.Player.AirborneFrame != 0) // チルノ_滞空状態
						{
							if (1 <= Game.I.Player.上昇_Frame) // チルノ_上昇
							{
								int koma = Game.I.Player.上昇_Frame;
								koma--;
								koma /= 3;
								koma = Math.Min(koma, Ground.I.Picture2.Cirno_ジャンプ_上昇.Length - 1);

								picture = Ground.I.Picture2.Cirno_ジャンプ_上昇[koma];
							}
							else // チルノ_下降
							{
								int koma = Game.I.Player.下降_Frame;
								koma--;
								koma /= 5;
								koma %= 2;

								picture = Ground.I.Picture2.Cirno_ジャンプ_下降[koma];
							}
						}
						else if (1 <= this.MoveFrame) // チルノ_移動
						{
							if (this.MoveSlow)
							{
								picture = Ground.I.Picture2.Cirno_歩く[Game.I.Frame / 10 % Ground.I.Picture2.Cirno_歩く.Length];
							}
							else
							{
								int koma = this.MoveFrame;
								koma--;

								if (Ground.I.Picture2.Cirno_走る.Length <= koma)
								{
									koma -= Ground.I.Picture2.Cirno_走る.Length;
									koma /= 5;
									koma %= 2;
									koma = Ground.I.Picture2.Cirno_走る.Length - 2 + koma;
								}
								picture = Ground.I.Picture2.Cirno_走る[koma];
							}
						}
						else // チルノ_立ち
						{
							int koma = this.StandFrame / 3;

							if (koma < Ground.I.Picture2.Cirno_しゃがみ解除.Length)
								picture = Ground.I.Picture2.Cirno_しゃがみ解除[koma];
							else
								picture = Ground.I.Picture2.Cirno_立ち[Game.I.Frame / 10 % Ground.I.Picture2.Cirno_立ち.Length];
						}
					}
					break;

				default:
					throw null; // never
			}
			if (1 <= this.DamageFrame) // 被弾モーション
			{
				switch (Game.I.Player.Chara)
				{
					case Chara_e.TEWI:
						picture = Ground.I.Picture2.Tewi_大ダメージ[(this.DamageFrame * Ground.I.Picture2.Tewi_大ダメージ.Length) / (GameConsts.PLAYER_DAMAGE_FRAME_MAX + 1)];
						break;

					case Chara_e.CIRNO:
						picture = Ground.I.Picture2.Tewi_大ダメージ[(this.DamageFrame * Ground.I.Picture2.Tewi_大ダメージ.Length) / (GameConsts.PLAYER_DAMAGE_FRAME_MAX + 1)];
						break;

					default:
						throw null; // never
				}
			}
			if (1 <= this.DamageFrame || 1 <= this.InvincibleFrame)
			{
				DDDraw.SetTaskList(DDGround.EL); // 敵より前面に描画する。
			}
			if (1 <= this.InvincibleFrame)
			{
				DDDraw.SetAlpha(0.5);
			}
			DDDraw.DrawBegin(
				picture,
				this.X - DDGround.Camera.X,
				this.Y - DDGround.Camera.Y
				);
			DDDraw.DrawZoom_X(this.FacingLeft ? -1.0 : 1.0);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		/// <summary>
		/// 攻撃を行う。
		/// -- Attack から呼び出されるかもしれない。
		/// </summary>
		public void Fire()
		{
			// none
		}
	}
}
