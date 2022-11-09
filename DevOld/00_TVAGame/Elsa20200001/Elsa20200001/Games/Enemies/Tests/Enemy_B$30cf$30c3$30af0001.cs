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
				if (DDUtils.GetDistanceLessThan(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y), 30.0))
				{
					foreach (var relay in this.E_ハック実行())
						yield return relay;

					break;
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
				{
					DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.Camera.X, this.Y - DDGround.Camera.Y);
					DDDraw.DrawRotate(DDEngine.ProcFrame / 100.0);
					DDDraw.DrawEnd();

					DDPrint.SetDebug((int)this.X - DDGround.Camera.X, (int)this.Y - DDGround.Camera.Y);
					DDPrint.SetBorder(new I3Color(0, 0, 0));
					DDPrint.PrintLine("ハック0001");
					DDPrint.Reset();

					// 当たり判定無し
				}
				yield return true;
			}
		}

		private IEnumerable<bool> E_ハック実行()
		{
			Game.I.UserInputDisabled = true;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.PlayerHacker.Fast = true;
			Game.I.PlayerHacker.DIR_2 = true;

			for (int c = 0; c < 50; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_6 = true;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_2 = false;

			for (int c = 0; c < 65; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_8 = true;

			for (int c = 0; c < 25; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_6 = false;

			for (int c = 0; c < 45; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_8 = false;
			Game.I.PlayerHacker.Fast = false;
			Game.I.PlayerHacker.DIR_4 = true;

			for (int c = 0; c < 110; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_4 = false;
			Game.I.PlayerHacker.DIR_8 = true;

			for (int c = 0; c < 110; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_8 = false;
			Game.I.PlayerHacker.DIR_6 = true;

			for (int c = 0; c < 50; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_6 = false;
			Game.I.PlayerHacker.DIR_2 = true;

			for (int c = 0; c < 60; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_2 = false;
			Game.I.PlayerHacker.DIR_6 = true;

			for (int c = 0; c < 55; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_6 = false;
			Game.I.PlayerHacker.Fast = true;
			Game.I.PlayerHacker.DIR_8 = true;

			for (int c = 0; c < 25; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_6 = true;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_6 = false;

			for (int c = 0; c < 80; c++)
				yield return true;

			Game.I.PlayerHacker.DIR_8 = false;
			Game.I.PlayerHacker.Fast = false;

			for (int c = 0; c < 30; c++)
				yield return true;

			Game.I.UserInputDisabled = false;
		}
	}
}
