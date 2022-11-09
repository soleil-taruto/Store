using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Attacks
{
	public class Attack_Tewi_ジャンプ弱攻撃 : Attack
	{
		protected override IEnumerable<bool> E_Draw()
		{
			for (int frame = 0; ; frame++)
			{
				if (DDInput.A.GetInput() == 1) // ? ジャンプ押下
					break;

				int FRAME_PER_KOMA = 1;
				//int FRAME_PER_KOMA = 2;
				//int FRAME_PER_KOMA = 3;

				int koma = frame / FRAME_PER_KOMA;

				if (Ground.I.Picture2.Tewi_ジャンプ弱攻撃.Length <= koma)
					break;

				double x = Game.I.Player.X;
				double y = Game.I.Player.Y;
				double xZoom = Game.I.Player.FacingLeft ? -1.0 : 1.0;
				bool facingLeft = Game.I.Player.FacingLeft;

				if (frame == 3 * FRAME_PER_KOMA)
				{
					Game.I.Shots.Add(new Shot_OneTime(
						10,
						DDCrashUtils.Rect(D4Rect.XYWH(
							Game.I.Player.X + 20.0 * (Game.I.Player.FacingLeft ? -1.0 : 1.0),
							Game.I.Player.Y,
							120.0,
							140.0
							))
						));
				}

				AttackCommon.ProcPlayer_移動();
				AttackCommon.ProcPlayer_Fall();

				AttackCommon.ProcPlayer_側面();
				AttackCommon.ProcPlayer_脳天();

				if (AttackCommon.ProcPlayer_接地())
					break;

				AttackCommon.ProcPlayer_Status();

				double plA = 1.0;

				if (1 <= Game.I.Player.InvincibleFrame)
				{
					plA = 0.5;
				}
				else
				{
					AttackCommon.ProcPlayer_当たり判定();
				}

				DDDraw.SetTaskList(Game.I.Player.Draw_EL);
				DDDraw.SetAlpha(plA);
				DDDraw.DrawBegin(
					Ground.I.Picture2.Tewi_ジャンプ弱攻撃[koma],
					x - DDGround.Camera.X,
					y - DDGround.Camera.Y
					);
				DDDraw.DrawZoom_X(xZoom);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
