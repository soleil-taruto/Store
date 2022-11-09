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
		{
			SCommon.CreateDir(RootDir);
		}

		private static string RootDir = Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), "CCSP-{c4323a86-2bf7-4f92-8262-daeae5ca0aba}");

		/// <summary>
		/// 利用者：Action_AccessCounter
		/// </summary>
		/// <returns>パス</returns>
		public string GetAccessCounterFile()
		{
			string file = Path.Combine(RootDir, "AccessCounter.txt");

			if (!File.Exists(file))
				File.WriteAllBytes(file, SCommon.EMPTY_BYTES);

			return file;
		}
	}
}
