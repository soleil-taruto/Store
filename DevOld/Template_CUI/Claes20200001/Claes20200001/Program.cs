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

		// 以下様式統一のため用途別に好きな方を使ってね -- ★要削除

#if true // 主にデバッガで実行するテスト用プログラム -- ★不要なら要削除
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
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			Main4();
			//Common.Pause();
		}

		private void Main4()
		{
			// -- choose one --

			Main5();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}

		// Main5 -- テスト用なら Test01 不要なら削除 -- ★要削除

		private void Main5()
		{
			// none
		}
#else // 主に実行ファイルにして使う/コマンド引数有り -- ★不要なら要削除
		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4(ar);
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			//Common.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main5(ArgsReader ar)
		{
			// TODO
		}
#endif
	}
}
