using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	public class Shot_Normal : Shot
	{
		private int Level;

		public Shot_Normal(double x, double y, bool facingLeft, int level)
			: base(x, y, facingLeft, LevelToAttackPoint(level), true, LevelTo敵を貫通する(level))
		{
			this.Level = level;
		}

		private static int LevelToAttackPoint(int level)
		{
			switch (level)
			{
				case 1: return 1;
				case 2: return 5;
				case 3: return 1;
				case 4: return 5;

				default:
					throw null; // never
			}
		}

		private static bool LevelTo敵を貫通する(int level)
		{
			switch (level)
			{
				case 1: return false;
				case 2: return false;
				case 3: return true;
				case 4: return true;

				default:
					throw null; // never
			}
		}

		protected override IEnumerable<bool> E_Draw()
		{
			while (!DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)))
			{
				this.X += 12.0 * (this.FacingLeft ? -1 : 1);

				switch (this.Level)
				{
					case 1:
						{
							DDDraw.DrawCenter(Ground.I.Picture.Shot_Normal, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);

							this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), 10.0);
						}
						break;

					case 2:
						{
							const double R = 14.0;

							DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
							DDDraw.DrawSetSize(R, R);
							DDDraw.DrawEnd();

							this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);
						}
						break;

					case 3:
						{
							const double R = 24.0;

							DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
							DDDraw.DrawSetSize(R, R);
							DDDraw.DrawEnd();

							this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);
						}
						break;

					case 4:
						{
							const double R = 48.0;

							DDDraw.DrawBegin(Ground.I.Picture.WhiteCircle, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
							DDDraw.DrawSetSize(R, R);
							DDDraw.DrawEnd();

							this.Crash = DDCrashUtils.Circle(new D2Point(this.X, this.Y), R);
						}
						break;

					default:
						throw null; // never
				}
				yield return true;
			}
		}
	}
}
