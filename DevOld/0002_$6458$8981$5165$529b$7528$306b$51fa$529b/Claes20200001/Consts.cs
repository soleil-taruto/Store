using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte
{
	public static class Consts
	{
		public const string R_MASTER_FILE = @"..\..\..\..\dat\Receipt.csv";

		public const string W_DIR = @"C:\temp\領収書";
		public static readonly string W_FILE = Path.Combine(W_DIR, "領収書.csv");
	}
}
