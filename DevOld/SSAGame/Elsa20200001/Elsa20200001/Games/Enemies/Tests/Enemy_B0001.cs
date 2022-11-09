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
	public class Enemy_B0001 : Enemy
	{
		public Enemy_B0001(double x, double y)
			: base(x, y, 0, 0, false)
		{ }

		private D2Point Speed = new D2Point();
		private int Frame = 0;

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
			double rot = DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y);
			rot += DDUtils.Random.Real() * 0.05;
			D2Point speedAdd = DDUtils.AngleToPoint(rot, 0.1);

			if (DDUtils.GetDistance(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y) < 50.0)
			{
				speedAdd *= -300.0;
			}
			this.Speed += speedAdd;
			this.Speed *= 0.93;

			this.X += this.Speed.X;
			this.Y += this.Speed.Y;

			if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
			{
				{
					double xZoom = this.Speed.X < 0.0 ? -1.0 : 1.0;

					DDDraw.DrawBegin(
						new DDPicture[]
						{
							Ground.I.Picture.Enemy_B0001_01,
							Ground.I.Picture.Enemy_B0001_02,
							Ground.I.Picture.Enemy_B0001_03,
							Ground.I.Picture.Enemy_B0001_04,
						}
						[this.Frame / 5 % 4],
						this.X - DDGround.ICamera.X,
						this.Y - DDGround.ICamera.Y
						);
					DDDraw.DrawZoom_X(xZoom);
					DDDraw.DrawEnd();
				}

				// 当たり判定ナシ
			}
			this.Frame++;
		}
	}
}
