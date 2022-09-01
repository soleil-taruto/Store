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

		protected override IEnumerable<bool> E_Draw()
		{
			for (int frame = 0; ; frame++)
			{
				//while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では行動しない。
				//    yield return true;

				double rot = DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y);
				rot += DDUtils.Random.GetReal1() * 0.05;
				D2Point speedAdd = DDUtils.AngleToPoint(rot, 0.1);

				if (DDUtils.GetDistanceLessThan(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y, 50.0))
					speedAdd *= -300.0;

				this.Speed += speedAdd;
				this.Speed *= 0.93;

				this.X += this.Speed.X;
				this.Y += this.Speed.Y;

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // カメラ外では描画しない。
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
						[frame / 5 % 4],
						this.X - DDGround.Camera.X,
						this.Y - DDGround.Camera.Y
						);
					DDDraw.DrawZoom_X(xZoom);
					DDDraw.DrawEnd();

					// 当たり判定無し
				}
				yield return true;
			}
		}
	}
}
