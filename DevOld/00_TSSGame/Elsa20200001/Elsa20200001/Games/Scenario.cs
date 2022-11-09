using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public class Scenario
	{
		public const string SCENARIO_FILE_PREFIX = "res\\Scenario\\";
		public const string SCENARIO_FILE_SUFFIX = ".txt";

		public string Name;
		public List<ScenarioPage> Pages = new List<ScenarioPage>();

		public Scenario(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new DDError();

			this.Name = name;
			this.Pages.Clear();

			string[] lines = ReadScenarioLines(name);
			ScenarioPage page = null;

			// シナリオデータに共通部を差し込み
			{
				int index = SCommon.IndexOf(lines, line => line.Trim().StartsWith("/"));

				if (index == -1)
					throw new DDError("シナリオデータの最初のページが見つかりません。");

				lines = SCommon.E_InsertRange(lines, index + 1, new string[] { "#Include/Startup" }).ToArray();
			}

			// memo: lines タブスペース除去済み

			for (int index = 0; index < lines.Length; index++)
			{
				string line = lines[index].Trim();

				if (line == "")
					continue;

				if (line[0] == '#') // ? 外部ファイル参照
				{
					line = line.Substring(1); // # 除去

					string includeName = line.Trim();
					string[] includeLines = ReadScenarioLines(includeName);

					lines = SCommon.E_RemoveRange(lines, index, 1).ToArray();
					lines = SCommon.E_InsertRange(lines, index, includeLines).ToArray();
				}
			}

			{
				Dictionary<string, string> def_dic = SCommon.CreateDictionary<string>();

				for (int index = 0; index < lines.Length; index++)
				{
					string line = lines[index].Trim();

					if (line == "")
						continue;

					if (line[0] == '^') // ? 定義
					{
						line = line.Substring(1); // ^ 除去

						string[] tokens = SCommon.Tokenize(line, " ", false, true, 2);
						string def_name = tokens[0];
						string def_value = tokens[1];

						def_dic.Add(def_name, def_value);

						lines[index] = "";
					}
				}
				for (int index = 0; index < lines.Length; index++)
				{
					string line = lines[index];

					foreach (KeyValuePair<string, string> pair in def_dic)
						line = line.Replace(pair.Key, pair.Value);

					lines[index] = line;
				}
			}

			bool 読み込み抑止中 = false;

			foreach (string f_line in lines)
			{
				string line = f_line.Trim();

				if (line == "")
					continue;

				if (line[0] == ';') // ? コメント行
					continue;

				if (line[0] == '/')
				{
					page = new ScenarioPage()
					{
						Subtitle = line.Substring(1)
					};

					this.Pages.Add(page);
				}
				else if (page == null)
				{
					throw new DDError("シナリオの先頭は /xxx でなければなりません。");
				}
				else if (line[0] == '!') // ? コマンド
				{
					string[] tokens = line.Substring(1).Split(' ').Where(v => v != "").ToArray();

					if (tokens[0] == "_ifndef")
					{
						読み込み抑止中 =
							Game.I != null &&
							Game.I.Status.Surfaces.Any(surface => surface.InstanceName == tokens[1]);
					}
					else if (tokens[0] == "_endif")
					{
						読み込み抑止中 = false;
					}
					else if (読み込み抑止中)
					{ }
					else
					{
						page.Commands.Add(new ScenarioCommand(tokens));
					}
				}
				else if (読み込み抑止中)
				{ }
				else
				{
					page.Lines.Add(line);
				}
			}
		}

		private static string[] ReadScenarioLines(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new DDError();

			byte[] fileData;

			{
				const string DEVENV_SCENARIO_DIR = "シナリオデータ";
				const string DEVENV_SCENARIO_SUFFIX = ".txt";

				if (Directory.Exists(DEVENV_SCENARIO_DIR))
				{
					string file = Path.Combine(DEVENV_SCENARIO_DIR, name + DEVENV_SCENARIO_SUFFIX);

					fileData = File.ReadAllBytes(file);
				}
				else
				{
					string file = SCENARIO_FILE_PREFIX + name + SCENARIO_FILE_SUFFIX;

					fileData = DDResource.Load(file);
				}
			}

			string text = SCommon.ToJString(fileData, true, true, true, true);

			text = text.Replace('\t', ' '); // タブスペースと空白 -> 空白に統一

			string[] lines = SCommon.TextToLines(text);
			return lines;
		}
	}
}
