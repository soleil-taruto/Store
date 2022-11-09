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
				AttackCommon.ProcPlayer_壁キャラ処理();
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

				DDPicture picture = Ground.I.Picture2.GetPlayer(Game.I.Status.Chara).GetPicture(Game.I.Player.FaceDirection, 0);

				DDDraw.SetTaskList(Game.I.Player.Draw_EL);
				DDDraw.SetMosaic();
				DDDraw.SetAlpha(plA);
				DDDraw.DrawBegin(
					picture,
					Game.I.Player.X - DDGround.Camera.X,
					Game.I.Player.Y - DDGround.Camera.Y - 12.0
					);
				DDDraw.DrawZoom(2.0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
