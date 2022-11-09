using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Scripts
{
	public static class ScriptCreator
	{
		public static Script Create(string name)
		{
			Script script;

			switch (name)
			{
				case "B0001": script = new Script_B0001(); break;
				//case "B0002": script = new Script_B0002(); break;
				//case "B0003": script = new Script_B0003(); break;
				case "入口": script = new Script_入口(); break;
				case "出口": script = new Script_出口(); break;

				// 新しいスクリプトをここへ追加..

				default:
					throw new DDError("name: " + name);
			}
			return script;
		}
	}
}
