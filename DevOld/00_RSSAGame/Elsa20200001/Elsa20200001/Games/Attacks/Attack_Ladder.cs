using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Tiles;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Attacks
{
	public class Attack_Ladder : Attack
	{
		public override bool IsInvincibleMode()
		{
			return false;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			const int SHOOTING_FRAME_MAX = 15;
			//const int SHOOTING_FRAME_MAX = 10; // old

			int shootingFrame = 0; // 0 == 無効, 1～ == 射撃モーション (カウントダウン方式)

			// 梯子の中央に寄せる。
			// -- 複数箇所から new Attack_Ladder() されるので、ここでやるのが妥当
			Game.I.Player.X = GameCommon.ToTablePoint(new D2Point(Game.I.Player.X, 0.0)).X * GameConsts.TILE_W + GameConsts.TILE_W / 2;

			for (int frame = 0; ; frame++)
			{
				if (AttackCommon.CamSlide())
					goto endPlayer;

				if (DDInput.DIR_8.GetInput() == 0 && 1 <= DDInput.A.GetInput()) // ? 上ボタンを離して、ジャンプボタン押下
				{
					if (1 <= DDInput.DIR_2.GetInput()) // ? 下ボタン押下 -> ジャンプしない。
					{
						Game.I.Player.JumpFrame = 0;
						Game.I.Player.JumpCount = 0;

						Game.I.Player.YSpeed = 0.0;
					}
					else // ? 下ボタンを離している -> ジャンプする。
					{
						Game.I.Player.JumpFrame = 1;
						Game.I.Player.JumpCount = 1;

						Game.I.Player.YSpeed = GameConsts.PLAYER_ジャンプ初速度;
					}

					// スライディングさせないために滞空状態にする。
					// -- 入力猶予を考慮して、大きい値を設定する。
					Game.I.Player.AirborneFrame = SCommon.IMAX / 2;

					break;
				}

				if (1 <= DDInput.DIR_4.GetInput())
					Game.I.Player.FacingLeft = true;

				if (1 <= DDInput.DIR_6.GetInput())
					Game.I.Player.FacingLeft = false;

				// 移動
				if (shootingFrame == 0) // 射撃モーション時は移動できない。
				{
					const double SPEED = 2.5;
					const double 加速_RATE = 0.5;

					bool moved = false;

					if (1 <= DDInput.DIR_8.GetInput())
					{
						Game.I.Player.Y -= Math.Min(SPEED, DDInput.DIR_8.GetInput() * 加速_RATE);
						moved = true;
					}
					if (1 <= DDInput.DIR_2.GetInput())
					{
						Game.I.Player.Y += Math.Min(SPEED, DDInput.DIR_2.GetInput() * 加速_RATE);
						moved = true;
					}

					if (moved)
					{
						// none
					}
				}

				bool 攻撃ボタンを押した瞬間撃つ = Ground.I.ショットのタイミング == Ground.ショットのタイミング_e.ショットボタンを押し下げた時;
				bool shot = false;

				if (1 <= DDInput.B.GetInput())
				{
					if (攻撃ボタンを押した瞬間撃つ && DDInput.B.GetInput() == 1)
					{
						Game.I.Player.Shoot(1);
						shot = true;
					}
					Game.I.Player.ShotChargePCT++;
					DDUtils.Minim(ref Game.I.Player.ShotChargePCT, 100);
				}
				else
				{
					int level = GameCommon.ShotChargePCTToLevel(Game.I.Player.ShotChargePCT);
					int chargePct = Game.I.Player.ShotChargePCT;
					Game.I.Player.ShotChargePCT = 0;

					if (攻撃ボタンを押した瞬間撃つ ? 2 <= level : 1 <= chargePct)
					{
						Game.I.Player.Shoot(level);
						shot = true;
					}
				}
				if (shot)
					shootingFrame = SHOOTING_FRAME_MAX;
				else
					DDUtils.CountDown(ref shootingFrame);

				//AttackCommon.ProcPlayer_移動();
				//AttackCommon.ProcPlayer_Fall();
				//AttackCommon.ProcPlayer_側面();
				AttackCommon.ProcPlayer_脳天();

				if (AttackCommon.ProcPlayer_接地())
				{
					Game.I.Player.JumpFrame = 0;
					Game.I.Player.JumpCount = 0;

					Game.I.Player.YSpeed = 0.0;

					break;
				}

				// ? 梯子の下に出た。
				if (
					Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(Game.I.Player.X, Game.I.Player.Y + GameConsts.TILE_H * 0))).Tile.GetKind() != Tile.Kind_e.LADDER &&
					Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(Game.I.Player.X, Game.I.Player.Y + GameConsts.TILE_H * 1))).Tile.GetKind() != Tile.Kind_e.LADDER
					)
				{
					Game.I.Player.JumpFrame = 0;
					Game.I.Player.JumpCount = 0;

					Game.I.Player.YSpeed = 0.0;

					break;
				}
			endPlayer:

				DDPicture picture;
				double xZoom;

				if (1 <= shootingFrame)
				{
					picture = Ground.I.Picture.Chara_A01_Climb_Attack;
					xZoom = Game.I.Player.FacingLeft ? -1.0 : 1.0;
				}
				else
				{
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(Game.I.Player.X, Game.I.Player.Y))).Tile.GetKind() == Tile.Kind_e.LADDER)
						picture = Ground.I.Picture.Chara_A01_Climb[(int)Game.I.Player.Y / 20 % 2];
					else
						picture = Ground.I.Picture.Chara_A01_Climb_Top;

					xZoom = 1.0;
				}

				double x = Game.I.Player.X;
				double y = Game.I.Player.Y;

				// 整数化
				x = (int)x;
				y = (int)y;

				DDDraw.SetTaskList(Game.I.Player.Draw_EL);
				DDDraw.DrawBegin(
					picture,
					x - DDGround.ICamera.X,
					y - DDGround.ICamera.Y
					);
				DDDraw.DrawZoom_X(xZoom);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
