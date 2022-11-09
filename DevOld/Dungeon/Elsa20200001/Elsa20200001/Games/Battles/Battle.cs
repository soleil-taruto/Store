using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Battles
{
	/// <summary>
	/// エンカウントと戦闘
	/// </summary>
	public class Battle : IDisposable
	{
		// <---- prm

		// <---- ret

		public static Battle I;

		public Battle()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			DDEngine.FreezeInput();

			this.Wall_A = 0.0;
			this.Wall_TargetA = -0.5;

			// 登場エフェクト

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				this.DrawWall();
				DDEngine.EachFrame();
			}

			// TODO: ここで戦闘をやる予定

			this.Wall_TargetA = 0.0;

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				this.DrawWall();
				DDEngine.EachFrame();
			}

			// 退場エフェクト

			DDEngine.FreezeInput();
		}

		private double Wall_A;
		private double Wall_TargetA;

		private void DrawWall()
		{
			DDUtils.Approach(ref this.Wall_A, this.Wall_TargetA, 0.9);
			Game.I.Draw();
			DDCurtain.DrawCurtain(this.Wall_A);
		}
	}
}
