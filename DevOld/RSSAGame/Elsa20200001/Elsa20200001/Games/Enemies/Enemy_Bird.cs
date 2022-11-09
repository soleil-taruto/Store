using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Bird : Enemy
	{
		private DDRandom Random;

		public Enemy_Bird(double x, double y)
			: base(x, y, 1, 3, false)
		{
			this.Random = new DDRandom(((uint)x << 16) | (uint)y);
		}

		private bool FacingLeft = false;

		protected override IEnumerable<bool> E_Draw()
		{
			while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // 画面内に入るまで休止する。
				yield return true;

			this.FacingLeft = Game.I.Player.X < this.X;

			DDPicture[] pictures = new DDPicture[]
			{
				Ground.I.Picture.Teki_a01_Fly01,
				Ground.I.Picture.Teki_a01_Fly02,
				Ground.I.Picture.Teki_a01_Fly03,
				Ground.I.Picture.Teki_a01_Fly02,
			};

			Func<bool> f_attack = SCommon.Supplier(this.E_Attack());

			for (int frame = 0; ; frame++)
			{
				f_attack();

				const double SPEED = 4.0;

				this.X += this.FacingLeft ? -SPEED : SPEED;

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
				{
					DDDraw.DrawBegin(pictures[frame / 8 % 4], this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
					DDDraw.DrawZoom_X(this.FacingLeft ? 1.0 : -1.0);
					DDDraw.DrawEnd();

					this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 12.0);
				}
				yield return true;
			}
		}

		private IEnumerable<bool> E_Attack()
		{
			for (; ; )
			{
				for (; ; )
				{
					if (
						this.Random.Real() < 0.03 &&
						this.Y < Game.I.Player.Y &&
						Math.Abs(Game.I.Player.X - this.X) < 100.0
						)
					{
						Game.I.Enemies.Add(new Enemy_BirdShit(this.X, this.Y + 10.0));
						break;
					}
					yield return true;
				}
				for (int c = 0; c < 40; c++) // 休止
					yield return true;
			}
		}
	}
}
