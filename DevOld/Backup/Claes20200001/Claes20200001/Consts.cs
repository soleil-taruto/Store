using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// コピー元フォルダパス
		/// </summary>
		public const string SRC_DIR = @"C:\";

		/// <summary>
		/// コピー先フォルダパス
		/// </summary>
		public const string DEST_DIR = @"P:\";

		private static string[] _srcIgnoreNames = null;

		/// <summary>
		/// コピー元で無視するフォルダのローカル名
		/// </summary>
		public static string[] SRC_IGNORE_NAMES
		{
			get
			{
				if (_srcIgnoreNames == null)
					_srcIgnoreNames = File.ReadAllLines(Path.Combine(ProcMain.SelfDir, "Ignore-Src.txt"), Encoding.UTF8)
						.Select(v => v.Trim())
						.Where(v => v != "")
						.ToArray();

				return _srcIgnoreNames;
			}
		}

		private static string[] _destIgnoreNames = null;

		/// <summary>
		/// コピー先で無視するフォルダのローカル名
		/// </summary>
		public static string[] DEST_IGNORE_NAMES
		{
			get
			{
				if (_destIgnoreNames == null)
					_destIgnoreNames = File.ReadAllLines(Path.Combine(ProcMain.SelfDir, "Ignore-Dest.txt"), Encoding.UTF8)
						.Select(v => v.Trim())
						.Where(v => v != "")
						.ToArray();

				return _destIgnoreNames;
			}
		}

		/// <summary>
		/// ログファイル出力先(1)
		/// </summary>
		public static string LOG_FILE_1
		{
			get
			{
				string dir = Environment.GetEnvironmentVariable("TMP");

				if (string.IsNullOrEmpty(dir))
					throw new Exception("Bad TMP");

				if (!Directory.Exists(dir))
					throw new Exception("no TMP");

				return Path.Combine(dir, "Backup_" + ProcMain.APP_IDENT + ".log");
			}
		}

		/// <summary>
		/// ログファイル出力先(2)
		/// </summary>
		public const string LOG_FILE_2 = @"P:\Backup.log";
	}
}
