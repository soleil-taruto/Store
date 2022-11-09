using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Attacks
{
	public class Attack_Sliding : Attack
	{
		public override bool IsInvincibleMode()
		{
			return false;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.スライディング(Game.I.Player.X, Game.I.Player.Y + 20.0)));

			for (int frame = 0; ; frame++)
			{
				if (10 < frame) // ? 最低持続時間経過
					if (AttackCommon.GetPlayer_側面(4) != 3) // ? ここは立ち上がれる場所である。
						if (DDInput.A.GetInput() <= 0 || DDInput.DIR_2.GetInput() <= 0) // ? 下・ジャンプボタン少なくともどちらかを離している。
							break;

				if (AttackCommon.GetPlayer_側面(2) != 0) // ? 壁に当たっている。
					if (AttackCommon.GetPlayer_側面(4) != 3) // ? ここは立ち上がれる場所である。
						if (DDInput.A.GetInput() == 1) // ? ジャンプボタンを押した。
							break;

				if (1 <= DDInput.DIR_4.GetInput())
					Game.I.Player.FacingLeft = true;

				if (1 <= DDInput.DIR_6.GetInput())
					Game.I.Player.FacingLeft = false;

				// 移動
				{
					const double SPEED = 10.0;

					if (Game.I.Player.FacingLeft)
						Game.I.Player.X -= SPEED;
					else
						Game.I.Player.X += SPEED;
				}

				//AttackCommon.ProcPlayer_移動();
				AttackCommon.ProcPlayer_Fall();
				AttackCommon.ProcPlayer_側面(2);
				AttackCommon.ProcPlayer_脳天();

				if (!AttackCommon.ProcPlayer_接地())
				{
					//Game.I.Player.AirborneFrame = 1; // 崖の淵に立ってしまわないように滞空状態にする。
					Game.I.Player.AirborneFrame = SCommon.IMAX / 2; // ジャンプしてしまわないように十分大きな値を設定する。
					break;
				}

				double xZoom = Game.I.Player.FacingLeft ? -1.0 : 1.0;

				DDDraw.SetTaskList(Game.I.Player.Draw_EL);
				DDDraw.DrawBegin(
					Ground.I.Picture.Chara_A01_Sliding,
					Game.I.Player.X - DDGround.ICamera.X,
					Game.I.Player.Y - DDGround.ICamera.Y
					);
				DDDraw.DrawZoom_X(xZoom);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
