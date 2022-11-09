using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4();
			}
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			SCommon.Pause();
		}

		private void Main4()
		{
			try
			{
				Main5();
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);
			}
		}

		private void Main5()
		{
			// -- choose one --

			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();
			//Main_20210421();
			//Main_20210422();
			//Main_20221001();
			MaskPictures_01();

			// --
		}

		private void MaskPictures_01()
		{
			EditPictures.MaskPictures(@"C:\temp");
		}

		private void Main_20221001()
		{
			int scale = 6;
			int w = 1800;
			int h = 600;
			I4Color color = new I4Color(255, 255, 64, 255);
			int fontSize = 250;

			MakePictures.MakeButton(scale, w, h, color, fontSize, "BET+", 80);
			MakePictures.MakeButton(scale, w, h, color, fontSize, "BET-", 160);
			MakePictures.MakeButton(scale, w, h, color, fontSize, "START", 0);

			color = new I4Color(255, 128, 64, 255);

			MakePictures.MakeButton(scale, w, h, color, fontSize, "STAND", -30);
			MakePictures.MakeButton(scale, w, h, color, fontSize, "HIT", 220);

			color = new I4Color(128, 255, 64, 255);

			MakePictures.MakeButton(scale, w, h, color, fontSize, "X", 440);
			MakePictures.MakeButton(scale, w, h, color, fontSize, "NEXT", 80);
			MakePictures.MakeButton(scale, w, h, color, fontSize, "C-100", 30);
		}

		/// <summary>
		/// ボタン作成テスト
		/// 2021.4.21 Doremy-用を再現した。
		/// </summary>
		private void Main_20210421()
		{
			I4Color color = new I4Color(255, 64, 64, 255);

			MakePictures.MakeButton(12, 2400, 480, color, 180, "ゲームスタート", 60);
			MakePictures.MakeButton(12, 2400, 480, color, 180, "コンテニュー", 180);
			MakePictures.MakeButton(12, 2400, 480, color, 180, "設定", 675);
			MakePictures.MakeButton(12, 2400, 480, color, 180, "終了", 675);
		}

		/// <summary>
		/// ボタン作成テスト
		/// 2021.4.21 SSAGame-用を再現した。
		/// </summary>
		private void Main_20210422()
		{
			I4Color color = new I4Color(233, 255, 33, 255);

			MakePictures.MakeButton(12, 2400, 480, color, 180, "ゲームスタート", 60);
			MakePictures.MakeButton(12, 2400, 480, color, 180, "コンテニュー", 180);
			MakePictures.MakeButton(12, 2400, 480, color, 180, "設定", 675);
			MakePictures.MakeButton(12, 2400, 480, color, 180, "終了", 675);
		}
	}
}
