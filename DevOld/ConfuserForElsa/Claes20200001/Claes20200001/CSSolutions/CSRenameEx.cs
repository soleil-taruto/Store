using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using System.Text.RegularExpressions;

namespace Charlotte.CSSolutions
{
	public class CSRenameEx
	{
		private CSRenameVarsFilter Rvf;
		private CSFile[] CSFiles;
		private Func<bool> ExecBuild;

		public CSRenameEx(CSRenameVarsFilter rvf, CSFile[] csFiles, Func<bool> execBuild)
		{
			this.Rvf = rvf;
			this.CSFiles = csFiles;
			this.ExecBuild = execBuild;
		}

		public void RenameEx()
		{
			if (!this.ExecBuild())
				throw new Exception("ビルド試験失敗.1");

			string[] names = this.CollectNames()
				.Where(name => name != "" && !SCommon.DECIMAL.Contains(name[0])) // 空文字列と数値を除去
				.Where(name => !CSResources.予約語リスト.Contains(name)) // 予約語を除去
				.DistinctOrderBy(SCommon.Comp) // 重複を除去
				.ToArray();

			List<string> names_noTest = new List<string>();
			List<string> names_needTest = new List<string>();

			foreach (string name in names)
			{
				if (IsNoTestName(name))
					names_noTest.Add(name);
				else
					names_needTest.Add(name);
			}

			this.RenameAll(
				names_noTest.ToArray(),
				names_noTest.Select(name => this.Rvf.GetNameNew(name)).ToArray()
				);

			if (!this.ExecBuild())
				throw new Exception("ビルド試験失敗.2");

			int successfulCount = 0;

			for (int index = 0; index < names_needTest.Count; index++)
			{
				Console.WriteLine("処理中：" + index + " / " + names_needTest.Count + " 成功：" + successfulCount);

				string name = names[index];
				string nameNew = this.Rvf.GetNameNew(name);

				this.RenameAll(name, nameNew);

				if (this.ExecBuild()) // ? ビルド成功
				{
					successfulCount++;
				}
				else // ? ビルド失敗
				{
					this.RenameAll(nameNew, name);
				}
			}
		}

		private bool IsNoTestName(string name)
		{
			return
				name[0] == '_' ||
				SCommon.alpha.Contains(name[0]) ||
				Regex.IsMatch(name, "_[0-9]{20}_[0-9]{20}_"); // ? 本プログラム内で自動生成した名前特有のパターンを含む
		}

		private IEnumerable<string> CollectNames()
		{
			foreach (CSFile csFile in this.CSFiles)
			{
				string text = File.ReadAllText(csFile.GetFile(), Encoding.UTF8);
				List<string> names = new List<string>();

				Filter(text, name =>
				{
					names.Add(name);
					return name;
				});

				foreach (string name in names)
					yield return name;
			}
		}

		private void RenameAll(string renameTargetName, string nameNew)
		{
			this.RenameAll(new string[] { renameTargetName }, new string[] { nameNew });
		}

		private void RenameAll(string[] renameTargetNames, string[] namesNew)
		{
			Dictionary<string, string> renameTargetName2nameNew = SCommon.CreateDictionary<string>();

			for (int index = 0; index < renameTargetNames.Length; index++)
				renameTargetName2nameNew.Add(renameTargetNames[index], namesNew[index]);

			foreach (CSFile csFile in this.CSFiles)
			{
				string text = File.ReadAllText(csFile.GetFile(), Encoding.UTF8);

				text = Filter(text, name =>
				{
					if (renameTargetName2nameNew.ContainsKey(name))
						name = renameTargetName2nameNew[name];

					return name;
				});

				File.WriteAllText(csFile.GetFile(), text, Encoding.UTF8);
			}
		}

		private static string Filter(string text, Func<string, string> nameFilter)
		{
			text += " ";

			bool insideOfLiteralChar = false;
			bool insideOfLiteralString = false;

			StringBuilder dest = new StringBuilder();

			for (int index = 0; index < text.Length; )
			{
				if (text[index] == '\\') // ? エスケープ文字 -> スキップする。
				{
					if (text[index + 1] == 'u') // ? 文字コード(4桁) -- これしかないはず。
					{
						dest.Append(text.Substring(index, 6));
						index += 6;
						continue;
					}
					throw new Exception("不正なエスケープ文字");
				}
				insideOfLiteralChar ^= text[index] == '\'';
				insideOfLiteralString ^= text[index] == '"';

				if (
					!insideOfLiteralChar &&
					!insideOfLiteralString &&
					CSCommon.IsCSWordChar(text[index])
					)
				{
					int end = index + 1;

					while (CSCommon.IsCSWordChar(text[end]))
						end++;

					string name = text.Substring(index, end - index);
					string nameNew = nameFilter(name);

					dest.Append(nameNew);
					index = end;
					continue;
				}
				dest.Append(text[index]);
				index++;
			}
			text = dest.ToString();
			text = text.Substring(0, text.Length - 1); // 番兵除去
			return text;
		}
	}
}
