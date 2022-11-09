using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	/// <summary>
	/// 共有データパス管理
	/// </summary>
	public class ProgramDataFolder
	{
		private static ProgramDataFolder _i = null;

		public static ProgramDataFolder I
		{
			get
			{
				if (_i == null)
					_i = new ProgramDataFolder();

				return _i;
			}
		}

		private ProgramDataFolder()
		{ }

		private static string RootDir = Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), "CCSP-" + ProcMain.APP_IDENT);

		/// <summary>
		/// 利用者：Action_AccessCounter
		/// </summary>
		/// <returns>パス</returns>
		public string GetAccessCounterFile()
		{
			SCommon.CreateDir(RootDir);
			string file = Path.Combine(RootDir, "AccessCounter.txt");

			if (!File.Exists(file))
				File.WriteAllBytes(file, SCommon.EMPTY_BYTES);

			return file;
		}
	}
}
