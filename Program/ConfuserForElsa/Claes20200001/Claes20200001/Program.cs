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
				Main4(ar);
			}
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			new Test0001().Test01(); // 難読化テスト
			//new Test0002().Test01(); // 似非英語名の長さの頻度分布
			//new Test0002().Test02(); // 使われない単語がどれくらいあるか

			// --

			//SCommon.Pause();
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
			string solutionFile = ar.NextArg();
			string workDir = ar.NextArg();

			ConfuserForElsa.Perform(solutionFile, workDir);
		}
	}
}
