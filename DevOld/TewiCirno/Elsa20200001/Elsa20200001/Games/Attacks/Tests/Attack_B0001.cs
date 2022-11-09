using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Attacks.Tests
{
	public class Attack_B0001 : Attack
	{
		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				if (
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1
					)
					break;

				AttackCommon.ProcPlayer_移動();
				AttackCommon.ProcPlayer_Fall();

				AttackCommon.ProcPlayer_側面();
				AttackCommon.ProcPlayer_脳天();
				AttackCommon.ProcPlayer_接地();

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

				DDGround.EL.Add(() =>
				{
					DDPrint.SetDebug(
						(int)Game.I.Player.X - DDGround.Camera.X - 80,
						(int)Game.I.Player.Y - DDGround.Camera.Y - 60
						);
					DDPrint.SetBorder(new I3Color(0, 0, 192));
					DDPrint.Print("Attack_B0001 テスト");
					DDPrint.Reset();

					return false;
				});

				DDDraw.SetTaskList(Game.I.Player.Draw_EL);
				DDDraw.SetAlpha(plA);
				DDDraw.DrawCenter(
					Ground.I.Picture2.Tewi_立ち[0],
					Game.I.Player.X - DDGround.Camera.X,
					Game.I.Player.Y - DDGround.Camera.Y
					);
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
