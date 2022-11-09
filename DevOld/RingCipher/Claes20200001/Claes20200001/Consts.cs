using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// ファイル処理をメモリ上で行うファイルサイズの最大値
		/// 単位：バイト
		/// </summary>
		public const long ON_MEMORY_FILE_SIZE_MAX = 202000000; // 192 * 1024 ^ 2 == 201326592
	}
}
