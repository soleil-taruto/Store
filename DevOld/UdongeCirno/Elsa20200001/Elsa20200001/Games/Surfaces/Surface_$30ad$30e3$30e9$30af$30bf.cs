using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Surfaces
{
	public class Surface_キャラクタ : Surface
	{
		public double Draw_Rnd = DDUtils.Random.GetReal1() * Math.PI * 2.0;

		public string ImageFile = @"dat\General\Dummy.png";
		public double A = 1.0;
		public double Zoom = 1.0;

		public Surface_キャラクタ(string typeName, string instanceName)
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
			const double BASIC_ZOOM = 1.0;

			DDDraw.SetAlpha(this.A);
			DDDraw.DrawBegin(DDCCResource.GetPicture(this.ImageFile), this.X, this.Y + Math.Sin(DDEngine.ProcFrame / 67.0 + this.Draw_Rnd) * 2.0);
			DDDraw.DrawZoom(BASIC_ZOOM * this.Zoom);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "Image")
			{
				this.Act.AddOnce(() => this.ImageFile = arguments[c++]);
				return;
			}
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
			ProcMain.WriteLog(command);
			throw new DDError(); // Bad command
		}

		private IEnumerable<bool> 待ち(int frame)
		{
			foreach (DDScene scene in DDSceneUtils.Create(frame))
			{
				if (Act.IsFlush)
					yield break;

				this.P_Draw();
				yield return true;
			}
		}

		private IEnumerable<bool> フェードイン()
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				if (Act.IsFlush)
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
				if (Act.IsFlush)
				{
					this.A = 0.0;
					yield break;
				}
				this.A = 1.0 - scene.Rate;
				this.P_Draw();

				yield return true;
			}
		}

		private IEnumerable<bool> モード変更(string imageFile)
		{
			string currImageFile = this.ImageFile;
			string destImageFile = imageFile;

			foreach (DDScene scene in DDSceneUtils.Create(30))
			{
				if (Act.IsFlush)
				{
					this.A = 1.0;
					this.ImageFile = destImageFile;

					yield break;
				}
				this.A = DDUtils.Parabola(scene.Rate * 0.5 + 0.5);
				this.ImageFile = currImageFile;
				this.P_Draw();

				this.A = DDUtils.Parabola(scene.Rate * 0.5 + 0.0);
				this.ImageFile = destImageFile;
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
				if (Act.IsFlush)
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

		protected override string[] Serialize_02()
		{
			return new string[]
			{
				this.ImageFile,
				this.A.ToString("F9"),
				this.Zoom.ToString("F9"),
			};
		}

		protected override void Deserialize_02(string[] lines)
		{
			int c = 0;

			this.ImageFile = lines[c++];
			this.A = double.Parse(lines[c++]);
			this.Zoom = double.Parse(lines[c++]);
		}
	}
}
