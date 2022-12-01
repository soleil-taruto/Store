using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;
using Charlotte.Commons;

namespace Charlotte.Games.Surfaces
{
	/// <summary>
	/// システム・ロジック的な処理に関わる命令群
	/// </summary>
	public class Surface_System : Surface
	{
		public Surface_System(string typeName, string instanceName)
			: base(typeName, instanceName)
		{ }

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// noop

				yield return true;
			}
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "Swap") // 即時
			{
				string name1 = arguments[c++];
				string name2 = arguments[c++];

				foreach (Surface surface in Game.I.Status.Surfaces)
				{
					if (surface.InstanceName == name1)
						surface.InstanceName = name2;
					else if (surface.InstanceName == name2)
						surface.InstanceName = name1;
				}
				return;
			}
			if (command == "WhileAct") // 即時
			{
				while (Game.I.Status.Surfaces.Any(v => v.Act.Count != 0))
				{
					Game.I.DrawSurfaces();
					DDEngine.EachFrame();
				}
				DDEngine.FreezeInput(GameConsts.NEXT_PAGE_INPUT_INTERVAL);
				return;
			}
			if (command == "WhileActOrInput") // 即時
			{
				while (Game.I.Status.Surfaces.Any(v => v.Act.Count != 0))
				{
					// 入力：シナリオを進める。
					if (
						DDMouse.Rot < 0 ||
						DDMouse.L.GetInput() == -1 ||
						DDInput.A.GetInput() == 1
						)
						break;

					Game.I.DrawSurfaces();
					DDEngine.EachFrame();
				}
				DDEngine.FreezeInput(GameConsts.NEXT_PAGE_INPUT_INTERVAL);
				return;
			}
			ProcMain.WriteLog(command);
			throw new DDError(); // Bad command
		}
	}
}
