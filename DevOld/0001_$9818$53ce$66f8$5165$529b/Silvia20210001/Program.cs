using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Charlotte.Commons;

namespace Charlotte
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			ProcMain.GUIMain(() => new MainWin());
		}
	}
}
