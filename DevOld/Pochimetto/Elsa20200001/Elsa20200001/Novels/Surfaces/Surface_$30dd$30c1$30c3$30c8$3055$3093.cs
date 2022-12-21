using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_ポチットさん : Surface
	{
		public double Draw_Rnd = DDUtils.Random.GetReal1() * Math.PI * 2.0;

		public DDPicture Image = Ground.I.Picture.PlayerStands[0][0];
		public double A = 1.0;
		public double Zoom = 1.0;
		public bool FacingLeft = false;

		public Surface_ポチットさん(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 20000;
		}

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				this.P_Draw();

				yield return true;
			}
		}

		private void P_Draw()
		{
			const double BASIC_ZOOM = 8.0;

			DDDraw.SetAlpha(this.A);
			DDDraw.SetMosaic();
			DDDraw.DrawBegin(
				this.Image,
				this.X,
				this.Y + 40.0 + (DDEngine.ProcFrame / 50 % 2 * BASIC_ZOOM * -1)
				);
			DDDraw.DrawZoom(BASIC_ZOOM * this.Zoom);

			if (this.FacingLeft)
				DDDraw.DrawZoom_X(-1.0);

			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "A")
			{
				this.Act.AddOnce(() => this.A = double.Parse(arguments[c++]));
				return;
			}
			if (command == "Zoom")
			{
				this.Act.AddOnce(() => this.Zoom = double.Parse(arguments[c++]));
				return;
			}
			if (command == "待ち")
			{
				this.Act.Add(SCommon.Supplier(this.待ち(int.Parse(arguments[c++]))));
				return;
			}
			if (command == "フェードイン")
			{
				this.Act.Add(SCommon.Supplier(this.フェードイン()));
				return;
			}
			if (command == "フェードアウト")
			{
				this.Act.Add(SCommon.Supplier(this.フェードアウト()));
				return;
			}
			if (command == "モード変更")
			{
				this.Act.Add(SCommon.Supplier(this.モード変更(arguments[c++])));
				return;
			}
			if (command == "スライド")
			{
				double x = double.Parse(arguments[c++]);
				double y = double.Parse(arguments[c++]);

				this.Act.Add(SCommon.Supplier(this.スライド(x, y)));
				return;
			}
			if (command == "キョロキョロ")
			{
				this.Act.Add(SCommon.Supplier(this.キョロキョロ()));
				return;
			}
			ProcMain.WriteLog(command);
			throw new DDError(); // Bad command
		}

		private IEnumerable<bool> 待ち(int frame)
		{
			foreach (DDScene scene in DDSceneUtils.Create(frame))
			{
				if (NovelAct.IsFlush)
					yield break;

				this.P_Draw();
				yield return true;
			}
		}

		private IEnumerable<bool> フェードイン()
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				if (NovelAct.IsFlush)
				{
					this.A = 1.0;
					yield break;
				}
				this.A = scene.Rate;
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> フェードアウト()
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				if (NovelAct.IsFlush)
				{
					this.A = 0.0;
					yield break;
				}
				this.A = 1.0 - scene.Rate;
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> モード変更(string destImageName)
		{
			DDPicture currImage = this.Image;
			DDPicture destImage;

			switch (destImageName)
			{
				case "通常": destImage = Ground.I.Picture.PlayerStands[0][0]; break;

				// 新しいイメージ名をここへ追加..

				default:
					throw new DDError(); // never
			}

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				if (NovelAct.IsFlush)
				{
					this.A = 1.0;
					this.Image = destImage;

					yield break;
				}
				this.A = DDUtils.Parabola(scene.Rate * 0.5 + 0.5);
				this.Image = currImage;
				this.P_Draw();

				this.A = DDUtils.Parabola(scene.Rate * 0.5 + 0.0);
				this.Image = destImage;
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> スライド(double x, double y)
		{
			double currX = this.X;
			double destX = x;
			double currY = this.Y;
			double destY = y;

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				if (NovelAct.IsFlush)
				{
					this.X = destX;
					this.Y = destY;

					yield break;
				}
				this.X = DDUtils.AToBRate(currX, destX, DDUtils.SCurve(scene.Rate));
				this.Y = DDUtils.AToBRate(currY, destY, DDUtils.SCurve(scene.Rate));
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> キョロキョロ()
		{
			bool facingLeftOrig = this.FacingLeft;

			foreach (DDScene scene in DDSceneUtils.Create(45))
			{
				if (NovelAct.IsFlush)
				{
					this.FacingLeft = facingLeftOrig;
					yield break;
				}
				this.FacingLeft = facingLeftOrig ^ (scene.Numer / 15 % 2 == 0);
				this.P_Draw();

				yield return true;
			}
		}
	}
}
