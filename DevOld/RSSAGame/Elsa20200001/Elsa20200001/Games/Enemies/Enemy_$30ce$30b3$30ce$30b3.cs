using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	public class Enemy_ノコノコ : Enemy
	{
		private bool 端から落ちない;
		private bool FacingLeft;

		public Enemy_ノコノコ(double x, double y, bool 端から落ちない, bool facingLeft)
			: base(x, y, 5, 3, false)
		{
			this.端から落ちない = 端から落ちない;
			this.FacingLeft = facingLeft;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			const double CHARA_R = 20.0;

			double ySpeed = 0.0;

			for (; ; )
			{
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X - CHARA_R, this.Y))).Tile.IsWall()) // ? 左側面接触
				{
					this.FacingLeft = false;
				}
				if (Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X + CHARA_R, this.Y))).Tile.IsWall()) // ? 右側面接触
				{
					this.FacingLeft = true;
				}

				if (this.端から落ちない)
				{
					const double 判定_X = 10.0;
					const double 判定_Y = CHARA_R + 1.0;

					if (!Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X - 判定_X, this.Y + 判定_Y))).Tile.IsWall()) // ? 左下足場無し -> 引き返す。
					{
						this.FacingLeft = false;
					}
					if (!Game.I.Map.GetCell(GameCommon.ToTablePoint(new D2Point(this.X + 判定_X, this.Y + 判定_Y))).Tile.IsWall()) // ? 右下足場なし -> 引き返す。
					{
						this.FacingLeft = true;
					}
				}

				const double X_SPEED = 1.0;
				const double GRAVITY = 0.5;
				const double Y_SPEED_MAX = 10.0;

				this.X += X_SPEED * (this.FacingLeft ? -1 : 1);
				this.Y += ySpeed;

				ySpeed += GRAVITY;
				ySpeed = Math.Min(ySpeed, Y_SPEED_MAX);

				if (GameCommon.壁処理(ref this.X, ref this.Y, new D2Point[] { new D2Point(0.0, CHARA_R) }))
				{
					ySpeed = 0.0;
				}

				if (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), CHARA_R * 1.2))
				{
					// 暫定_描画
					{
						DDDraw.SetBright(new I3Color(64, 64, 255));
						DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
						DDDraw.DrawSetSize(CHARA_R * 2, CHARA_R * 2);
						DDDraw.DrawEnd();
						DDDraw.Reset();

						DDPrint.SetDebug((int)this.X - DDGround.ICamera.X, (int)this.Y - DDGround.ICamera.Y);
						DDPrint.SetBorder(new I3Color(0, 0, 0));
						DDPrint.Print("[ノコ◆コ_" + this.HP + "_" + (this.端から落ちない ? 1 : 0) + "]");
						DDPrint.Reset();
					}

					this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), CHARA_R);
				}
				yield return true;
			}
		}
	}
}
