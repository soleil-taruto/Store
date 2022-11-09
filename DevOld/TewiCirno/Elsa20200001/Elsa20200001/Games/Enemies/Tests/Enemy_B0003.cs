using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies.Tests
{
	/// <summary>
	/// テスト用_敵
	/// </summary>
	public class Enemy_B0003 : Enemy
	{
		public Enemy_B0003(double x, double y)
			: base(x, y, 1, SCommon.IMAX, false)
		{ }

		private int Frame = 0;
		private Enemy_B0003 Friend = null;

		protected override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.P_Draw();
				yield return true;
			}
		}

		private void P_Draw()
		{
			if (this.Friend == null)
			{
				this.Friend = this.GetFriend();
			}
			else
			{
				if (this.Friend.HP == -1)
				{
					Game.I.Enemies.Add(new Enemy_B0003_Egg(this.X, this.Y, this.Friend.X, this.Friend.Y));
					this.Friend = null;
				}
			}

			if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
			{
				double xBure = Math.Cos(this.Frame / 10.0) * 5.0;
				double yBure = Math.Sin(this.Frame / 10.0) * 5.0;

				DDDraw.DrawCenter(
					Ground.I.Picture.Enemy_B0003,
					this.X - DDGround.Camera.X + xBure,
					this.Y - DDGround.Camera.Y + yBure
					);

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 50.0);
			}

			// 最初の数フレーム間は無敵とする。
			// -- 再生後即死亡すると仲間に補足されず、次の再生が行われなくなってしまう。-- 1,2フレーム生き延びるだけで十分かもしれない。
			{
				if (this.Frame == 0)
					this.HP = 0; // 無敵

				if (this.Frame == 5)
					this.HP = 1; // 無敵解除
			}

			this.Frame++;
		}

		private Enemy_B0003 GetFriend() // ret: null == not found
		{
			Enemy_B0003 friend = null;
			double friendDistance = SCommon.IMAX;

			foreach (Enemy enemy in Game.I.Enemies.Iterate().Where(v => v != this && v is Enemy_B0003))
			{
				double distance = DDUtils.GetDistance(new D2Point(enemy.X, enemy.Y), new D2Point(this.X, this.Y));

				if (distance < friendDistance)
				{
					friend = (Enemy_B0003)enemy;
					friendDistance = distance;
				}
			}
			return friend;
		}
	}
}
