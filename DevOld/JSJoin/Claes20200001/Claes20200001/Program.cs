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
using Charlotte.JSConfusers;

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
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			//Main4(new ArgsReader(new string[] { @"C:\Dev\GameJS\GameTemplate\Program", @"C:\Dev\GameJS\GameTemplate\res", @"C:\temp" }));
			Main4(new ArgsReader(new string[] { "/R", @"C:\Dev\GameJS\GameTemplate\Program", @"C:\Dev\GameJS\GameTemplate\res", @"C:\temp" }));
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

				Console.WriteLine("変換エラー(エンターキーを押して下さい)");
				Console.ReadLine();
			}
		}

		private class IdentifierInfo
		{
			public string Name;
			public int Index;
		}

		private string SourceDir;
		private string ResourceDir;
		private string OutputDir;

		private List<string> SourceFiles = new List<string>();
		private List<string> ResourceFiles = new List<string>();
		private List<string> JSLines = new List<string>();
		private List<string> JSFunctions;
		private List<string> JSVariables;
		private List<string> Tags = new List<string>();

		private void Main5(ArgsReader ar)
		{
			bool releaseMode = ar.ArgIs("/R");

			this.SourceDir = SCommon.MakeFullPath(ar.NextArg());
			this.ResourceDir = SCommon.MakeFullPath(ar.NextArg());
			this.OutputDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(this.SourceDir))
				throw new Exception("no SourceDir");

			if (!Directory.Exists(this.ResourceDir))
				throw new Exception("no ResourceDir");

			if (!Directory.Exists(this.OutputDir))
				throw new Exception("no OutputDir");

			foreach (string file in Directory.GetFiles(this.SourceDir, "*", SearchOption.AllDirectories))
				if (!file.Contains("\\_")) // ? アンダースコアで始まるローカル名を含まない。
					if (SCommon.EndsWithIgnoreCase(file, ".js")) // ? JSファイル
						this.SourceFiles.Add(file);

			foreach (string file in Directory.GetFiles(this.ResourceDir, "*", SearchOption.AllDirectories))
				if (!file.Contains("\\_")) // ? アンダースコアで始まるローカル名を含まない。
					this.ResourceFiles.Add(file);

			// ソース・ファイルは読み込む順序に副作用があるので注意すること。

			// ソース・ファイルのソート
			this.SourceFiles.Sort(Common.CompPath);
			// リソースのソート
			this.ResourceFiles.Sort(Common.CompPath);

			for (int index = 0; index < this.SourceFiles.Count; index++)
			{
				string file = this.SourceFiles[index];
				string text = File.ReadAllText(file, SCommon.ENCODING_SJIS);
				string uniqueString = string.Format("_P_{0:D4}_", index);

				text = text.Replace("@@", uniqueString);

				string[] lines = SCommon.TextToLines(text);

				lines = ReplaceSpecialCode(lines, file, releaseMode);

				this.JSLines.Add("// ここから " + file);
				this.JSLines.AddRange(lines);
				this.JSLines.Add("// ここまで " + file);
			}

			this.JSFunctions = this.CollectFunctions(this.JSLines).Select(v => v.Name).ToList();
			this.JSVariables = this.CollectVariables(this.JSLines).Select(v => v.Name).ToList();

			string[] identifiers = this.JSFunctions.Concat(this.JSVariables).ToArray();

			SCommon.ForEachPair(identifiers, (a, b) =>
			{
				if (a == b)
					throw new Exception("識別子の重複：" + a);
			});

			List<string> escapedJSLines = this.JSLines;
			this.JSLines = new List<string>();

			this.JSLines.Add("var Resources =");
			this.JSLines.Add("{");

			foreach (string file in this.ResourceFiles)
			{
				string name = file;
				name = SCommon.ChangeRoot(name, this.ResourceDir);
				name = name.Replace('\\', '/');

				string url;

				if (releaseMode) // ? リリース・モード -> 埋め込み
				{
					string ext = Path.GetExtension(file);
					string mediaType = GetMediaTypeByExt(ext);
					byte[] fileData = File.ReadAllBytes(file);
					string b64FileData = SCommon.Base64.I.Encode(fileData);
					string dataUrl = string.Format("data:{0};base64,{1}", mediaType, b64FileData);

					url = dataUrl;
				}
				else // ? デバッグ・モード -> ローカルファイルへのリンク
				{
					url = "file:" + file.Replace('\\', '/');
				}

				this.JSLines.Add(string.Format("\t\"{0}\": \"{1}\",", name, url));
			}

			this.JSLines.Add("};");
			this.JSLines.Add("");
			this.JSLines.Add("var Functions =");
			this.JSLines.Add("{");

			foreach (string functionName in this.JSFunctions)
			{
				this.JSLines.Add(string.Format("\t\"{0}\": function() {{ return {0}; }},", functionName));
			}

			this.JSLines.Add("};");
			this.JSLines.Add("");
			this.JSLines.Add("var Variables =");
			this.JSLines.Add("{");

			foreach (string variableName in this.JSVariables)
			{
				this.JSLines.Add(string.Format("\t\"{0}\": [ function() {{ return {0}; }}, function(value) {{ {0} = value; }} ],", variableName));
			}

			this.JSLines.Add("};");
			this.JSLines.Add("");
			this.JSLines.AddRange(escapedJSLines);
			escapedJSLines = null;

			if (releaseMode) // ? リリース・モード -> 難読化
			{
				this.JSLines = new JSConfuser().Confuse(this.JSLines);
			}

			IEnumerable<string> htmlLines = this.CreateHtmlLines();

			File.WriteAllLines(
				Path.Combine(this.OutputDir, "index.html"),
				htmlLines,
				SCommon.ENCODING_SJIS
				);

			this.CollectTags();

			File.WriteAllLines(
				Path.Combine(this.SourceDir, "tags"),
				this.Tags,
				SCommon.ENCODING_SJIS
				);
		}

		private int AutoCounter = 1;

		/// <summary>
		/// 特別なコードをJSコードに変換する。
		/// </summary>
		/// <param name="lines">コード行リスト</param>
		/// <param name="sourceFilePath">このソースファイルのパス</param>
		/// <returns>処理後のコード行リスト</returns>
		private string[] ReplaceSpecialCode(string[] lines, string sourceFilePath, bool releaseMode)
		{
			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index];
				int indentLen;

				for (indentLen = 0; indentLen < line.Length; indentLen++)
					if (' ' < line[indentLen])
						break;

				string indent = line.Substring(0, indentLen); // インデントを抽出
				line = line.Substring(indentLen); // インデントを除去

				string positionString =
					sourceFilePath.Replace('\\', '/') +
					" (" +
					(index + 1) +
					")";

				string sha512_64 = SCommon.Hex.ToString(SCommon.GetSHA512(Encoding.UTF8.GetBytes(positionString))).Substring(0, 16);

				if (releaseMode)
					positionString = sha512_64;
				else
					positionString += " " + sha512_64;

				if (line.StartsWith("LOGPOS;"))
				{
					string trailer = line.Substring(7);

					line = indent + "console.log(\"" + positionString + "\");" + trailer;
				}
				else if (line.StartsWith("error;"))
				{
					string trailer = line.Substring(6);

					line = indent + "throw \"" + positionString + "\";" + trailer;
				}
				else
				{
					continue; // 通常行なので何もせず次行へ
				}

				lines[index] = line; // 行を更新
			}

			string text = SCommon.LinesToText(lines);

			text = RSC_ProcessWord(text, "@(AUTO)", () => "" + (AutoCounter++));
			text = RSC_ProcessWord(text, "@(UUID)", () => Guid.NewGuid().ToString("B"));

			lines = SCommon.TextToLines(text);

			return lines;
		}

		private string RSC_ProcessWord(string text, string word, Func<string> getCode)
		{
			string dest = "";

			for (; ; )
			{
				string[] slnd = SCommon.ParseIsland(text, word);

				if (slnd == null)
					break;

				dest += slnd[0] + getCode();
				text = slnd[2];
			}
			return dest + text;
		}

		private void CollectTags()
		{
			foreach (string file in this.SourceFiles)
			{
				string[] lines = File.ReadAllLines(file, SCommon.ENCODING_SJIS);

				foreach (IdentifierInfo identifier in this.CollectFunctions(lines))
				{
					this.Tags.Add(file + "(" + (identifier.Index + 1) + ") : " + identifier.Name + " // 関数");
				}
				foreach (IdentifierInfo identifier in this.CollectVariables(lines))
				{
					this.Tags.Add(file + "(" + (identifier.Index + 1) + ") : " + identifier.Name + " // 変数");
				}
			}
		}

		private IEnumerable<IdentifierInfo> CollectFunctions(IList<string> lines)
		{
			for (int index = 0; index < lines.Count; index++)
			{
				string line = lines[index];

				if (line == "") // ? 空行
					continue;

				if (line[0] <= ' ') // ? 空白系文字で始まる == インデント有り
					continue;

				string[] tokens = SCommon.Tokenize(line, "\t *(", false, true);

				if (tokens.Length < 2)
					continue;

				if (tokens[0] != "function")
					continue;

				string name = tokens[1];

				if (name.StartsWith("@@_")) // ? ファイルスコープ
					continue;

				yield return new IdentifierInfo()
				{
					Name = name,
					Index = index,
				};
			}
		}

		private IEnumerable<IdentifierInfo> CollectVariables(IList<string> lines)
		{
			for (int index = 0; index < lines.Count; index++)
			{
				string line = lines[index];

				if (line == "") // ? 空行
					continue;

				if (line[0] <= ' ') // ? 空白系文字で始まる == インデント有り
					continue;

				string[] tokens = SCommon.Tokenize(line, "\t =;", false, true);

				if (tokens.Length < 2)
					continue;

				if (tokens[0] != "var")
					continue;

				string name = tokens[1];

				if (name.StartsWith("@@_")) // ? ファイルスコープ
					continue;

				yield return new IdentifierInfo()
				{
					Name = name,
					Index = index,
				};
			}
		}

		private IEnumerable<string> CreateHtmlLines()
		{
			yield return "<html>";
			yield return "<head>";
			yield return "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=Shift_JIS\" />";
			yield return "<script>";
			yield return "";

			foreach (string line in this.JSLines)
				yield return line;

			yield return "";
			yield return "</script>";
			yield return "</head>";
			yield return "<body>";
			yield return "</body>";
			yield return "</html>";
		}

		private static string GetMediaTypeByExt(string ext)
		{
			string[] EXT_MEDIA_TYPE_PAIRS = new string[]
			{
				".bmp", "image/bmp",
				".gif", "image/gif",
				".jpeg", "image/jpeg",
				".jpg", "image/jpeg",
				".png", "image/png",
				".mp3", "audio/mp3",
				".wav", "audio/wav",
			};

			for (int index = 0; index < EXT_MEDIA_TYPE_PAIRS.Length; index += 2)
				if (SCommon.EqualsIgnoreCase(EXT_MEDIA_TYPE_PAIRS[index], ext.ToLower()))
					return EXT_MEDIA_TYPE_PAIRS[index + 1];

			return "application/octet-stream";
		}
	}
}
