using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Dog : Enemy
	{
		private DDRandom Random;

		public Enemy_Dog(double x, double y)
			: base(x, y, 5, 5, false)
		{
			this.Random = new DDRandom(((uint)x << 16) | (uint)y);
		}

		private bool FacingLeft = false;
		private int WaitFrame;

		private Func<bool> Run = () => false;

		protected override IEnumerable<bool> E_Draw()
		{
			for (this.WaitFrame = 0; ; this.WaitFrame++)
			{
				while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // 画面外に居るときは休止する。
					yield return true;

				if (!this.Run())
				{
					this.FacingLeft = Game.I.Player.X < this.X;

					if (
						30 < this.WaitFrame &&
						(
							DDUtils.GetDistance(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y)) < 200.0 ||
							Math.Abs(Game.I.Player.Y - this.Y) < 32.0
						))
						this.Run = SCommon.Supplier(this.E_Run());

					DDDraw.DrawBegin(Ground.I.Picture.Teki_a02_Wait01, this.X - DDGround.ICamera.X, this.Y - 32 - DDGround.ICamera.Y);
					DDDraw.DrawZoom_X(this.FacingLeft ? 1.0 : -1.0);
					DDDraw.DrawEnd();
				}

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y - 14.0), 30.0);

				yield return true;
			}
		}

		private IEnumerable<bool> E_Run()
		{
			DDPicture[] pictures = new DDPicture[]
			{
				Ground.I.Picture.Teki_a02_Run01,
				Ground.I.Picture.Teki_a02_Run02,
				Ground.I.Picture.Teki_a02_Run03,
			};

			for (int frame = 0; ; frame++)
			{
				if (
					30 < frame &&
					this.Random.Real() <
					(
						DDUtils.GetDistance(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y)) < 400.0 ?
						0.0001 :
						0.03
					))
					break;

				if (this.Random.Real() < 0.05)
					this.FacingLeft = Game.I.Player.X < this.X;

				if (this.FacingLeft)
				{
					if (!Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - 30.0, this.Y + 20.0)).Tile.IsGround())
						this.FacingLeft = false;
				}
				else
				{
					if (!Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + 30.0, this.Y + 20.0)).Tile.IsGround())
						this.FacingLeft = true;
				}

				const double SPEED = 6.0;

				this.X += this.FacingLeft ? -SPEED : SPEED;

				DDDraw.DrawBegin(pictures[frame / 6 % 3], this.X - DDGround.ICamera.X, this.Y - 32 - DDGround.ICamera.Y);
				DDDraw.DrawZoom_X(this.FacingLeft ? 1.0 : -1.0);
				DDDraw.DrawEnd();

				yield return true;
			}
			this.WaitFrame = 0;
		}
	}
}
