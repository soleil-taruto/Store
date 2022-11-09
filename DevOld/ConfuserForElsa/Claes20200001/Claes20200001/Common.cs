using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Common
	{
		/// <summary>
		/// リソースファイル(テキストファイル)を読み込む
		/// </summary>
		/// <param name="localName">リソースファイルのローカル名</param>
		/// <returns>リソースファイルの内容</returns>
		public static string ReadResTextFile(string localName)
		{
			string resFile = Path.Combine(ProcMain.SelfDir, localName);

			if (!File.Exists(resFile))
			{
				resFile = Path.Combine(@"..\..\..\..\doc", localName);

				if (!File.Exists(resFile))
					throw new Exception("リソースファイル \"" + localName + "\" が存在しません。");
			}
			return File.ReadAllText(resFile, SCommon.ENCODING_SJIS);
		}
	}
}
