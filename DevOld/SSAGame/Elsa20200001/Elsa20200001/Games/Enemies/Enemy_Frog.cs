using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_Frog : Enemy
	{
		private DDRandom Random;

		public Enemy_Frog(double x, double y)
			: base(x, y, 3, 3, false)
		{
			this.Random = new DDRandom(((uint)x << 16) | (uint)y);
		}

		private bool FacingLeft = false;
		private int WaitFrame;

		private Func<bool> Jump = () => false;

		private const double JUMP_SPEED = 6.0;
		private const double HI_JUMP_SPEED = 12.0;

		protected override IEnumerable<bool> E_Draw()
		{
			for (this.WaitFrame = 0; ; this.WaitFrame++)
			{
				while (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 50.0)) // 画面外に居るときは休止する。
					yield return true;

				if (!this.Jump())
				{
					this.FacingLeft = Game.I.Player.X < this.X;

					if (
						30 < this.WaitFrame &&
						(
							DDUtils.GetDistance(new D2Point(Game.I.Player.X, Game.I.Player.Y), new D2Point(this.X, this.Y)) < 300.0 ||
							Math.Abs(Game.I.Player.Y - this.Y) < 32.0
						))
						this.Jump = SCommon.Supplier(this.E_Jump(this.Random.Real() < 0.5 ? JUMP_SPEED : HI_JUMP_SPEED));

					DDDraw.DrawBegin(Ground.I.Picture.Teki_a03_Jump01, this.X - DDGround.ICamera.X, this.Y - 16 - DDGround.ICamera.Y);
					DDDraw.DrawZoom_X(this.FacingLeft ? 1.0 : -1.0);
					DDDraw.DrawEnd();
				}

				this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y + 2.0), 20.0);

				yield return true;
			}
		}

		private IEnumerable<bool> E_Jump(double jumpSpeed)
		{
			double ySpeed = -jumpSpeed;

			const double SPEED_X = 3.0;
			const double GRAVITY = 0.5;

			for (int frame = 0; frame < 5; frame++)
			{
				DDDraw.DrawBegin(Ground.I.Picture.Teki_a03_Jump02, this.X - DDGround.ICamera.X, this.Y - 16 - DDGround.ICamera.Y);
				DDDraw.DrawZoom_X(this.FacingLeft ? 1.0 : -1.0);
				DDDraw.DrawEnd();

				yield return true;
			}
			for (int frame = 0; ; frame++)
			{
				if (this.FacingLeft)
				{
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X - 20.0, this.Y)).Tile.IsWall())
						this.FacingLeft = false;
				}
				else
				{
					if (Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X + 20.0, this.Y)).Tile.IsWall())
						this.FacingLeft = true;
				}
				if (
					ySpeed < 0.0 && // ? 上昇中
					Game.I.Map.GetCell(GameCommon.ToTablePoint(this.X, this.Y - 20.0)).Tile.IsWall()
					)
					ySpeed = 0.0;

				if (0.0 < ySpeed) // ? 降下中
				{
					I2Point pt = GameCommon.ToTablePoint(this.X, this.Y + 30.0);

					if (Game.I.Map.GetCell(pt).Tile.IsWall())
					{
						this.Y = pt.Y * GameConsts.TILE_H - GameConsts.TILE_H / 2;
						break;
					}
				}
				if (Game.I.Map.H * GameConsts.TILE_H + 50.0 < this.Y) // ? 画面下に十分出た。
				{
					this.DeadFlag = true; // 消滅
				}

				this.X += this.FacingLeft ? -SPEED_X : SPEED_X;
				this.Y += ySpeed;

				ySpeed += GRAVITY;

				DDDraw.DrawBegin(Ground.I.Picture.Teki_a03_Jump03, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawZoom_X(this.FacingLeft ? 1.0 : -1.0);
				DDDraw.DrawEnd();

				yield return true;
			}
			this.WaitFrame = 0;
		}
	}
}
