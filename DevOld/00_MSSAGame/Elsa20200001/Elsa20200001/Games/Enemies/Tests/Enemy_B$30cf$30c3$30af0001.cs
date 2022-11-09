using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	public class Enemy_Bハック0001 : Enemy
	{
		public Enemy_Bハック0001(double x, double y)
			: base(x, y, 0, 0, false)
		{ }

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				if (DDUtils.GetDistanceLessThan(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y), 30.0)) // 当たり判定
				{
					foreach (var relay in this.E_ハック実行())
						yield return relay;

					break;
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では描画しない。
				{
					DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
					DDDraw.DrawRotate(DDEngine.ProcFrame / 100.0);
					DDDraw.DrawEnd();

					DDPrint.SetDebug((int)this.X - DDGround.Camera.X, (int)this.Y - DDGround.Camera.Y);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.PrintLine("ハック0001");
					DDPrint.Reset();

					// アイテム系につき this.Crash への当たり判定セット無し
				}
				yield return true;
			}
		}

		private IEnumerable<bool> E_ハック実行()
		{
			Game.I.UserInputDisabled = true;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_4 = true;

			for (int c = 0; c < 65; c++)
				yield return true;

			Game.I.PlayerHacker.Jump = true;

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.PlayerHacker.Jump = false;
			yield return true;
			Game.I.PlayerHacker.Jump = true;

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.PlayerHacker.Jump = false;

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_4 = false;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.UserInputDisabled = false;
		}
	}
}
