using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Attacks
{
	public class Attack_Cirno_ジャンプ強攻撃 : Attack
	{
		public override bool IsInvincibleMode()
		{
			return false;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			for (int frame = 0; ; frame++)
			{
				if (DDInput.A.GetInput() == 1) // ? ジャンプ押下
					break;

				//int FRAME_PER_KOMA = 1;
				//int FRAME_PER_KOMA = 2;
				int FRAME_PER_KOMA = 3;

				int koma = frame / FRAME_PER_KOMA;

				if (Ground.I.Picture2.Cirno_ジャンプ攻撃.Length <= koma)
					break;

				double x = Game.I.Player.X + 16 * (Game.I.Player.FacingLeft ? -1.0 : 1.0);
				double y = Game.I.Player.Y;
				double xZoom = Game.I.Player.FacingLeft ? -1.0 : 1.0;
				bool facingLeft = Game.I.Player.FacingLeft;

				if (frame == 4 * FRAME_PER_KOMA)
				{
					Game.I.Shots.Add(new Shot_OneTime(
						30,
						DDCrashUtils.Circle(
							new D2Point(
								Game.I.Player.X + 10.0 * (Game.I.Player.FacingLeft ? -1.0 : 1.0),
								Game.I.Player.Y
								),
							70.0
							)
						));
				}

				AttackCommon.ProcPlayer_移動();
				AttackCommon.ProcPlayer_Fall();

				AttackCommon.ProcPlayer_側面();
				AttackCommon.ProcPlayer_脳天();

				if (AttackCommon.ProcPlayer_接地())
					break;

				DDDraw.SetTaskList(Game.I.Player.Draw_EL);
				DDDraw.DrawBegin(
					Ground.I.Picture2.Cirno_ジャンプ攻撃[koma],
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
