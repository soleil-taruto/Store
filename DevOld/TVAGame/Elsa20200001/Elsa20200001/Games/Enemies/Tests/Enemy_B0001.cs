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
			for (; ; )
			{
				double rot = DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y);
				rot += DDUtils.Random.Real() * 0.05;
				D2Point speedAdd = DDUtils.AngleToPoint(rot, 0.1);
				double distance = DDUtils.GetDistance(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y);

				if (distance < 50.0)
				{
					speedAdd *= -300.0;
				}
				this.Speed += speedAdd;
				this.Speed *= 0.93;

				this.X += this.Speed.X;
				this.Y += this.Speed.Y;

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0))
				{
					DDDraw.DrawBegin(Ground.I.Picture.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
					DDDraw.DrawRotate(DDEngine.ProcFrame / 10.0);
					DDDraw.DrawEnd();

					DDPrint.SetBorder(new I3Color(64, 64, 0));
					DDPrint.SetDebug(
						(int)this.X - DDGround.ICamera.X - 10,
						(int)this.Y - DDGround.ICamera.Y - 10,
						20
						);
					DDPrint.PrintLine("敵(仮)");
					DDPrint.PrintLine("[無害]");
					DDPrint.PrintLine("DIST=" + distance.ToString("F3"));
					DDPrint.Reset();

					// 当たり判定ナシ
				}
				yield return true;
			}
		}
	}
}
