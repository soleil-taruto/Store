using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Surfaces;

namespace Charlotte.Games
{
	public class ScenarioCommand
	{
		private string[] Tokens;

		public ScenarioCommand(string[] tokens)
		{
			this.Tokens = tokens;
		}

		public void Invoke()
		{
			try
			{
				if (this.Tokens[1] == "=")
				{
					string instanceName = this.Tokens[0];
					string typeName = this.Tokens[2];

					Game.I.Status.Surfaces.RemoveAll(v => v.InstanceName == instanceName);

					Surface surface = SurfaceCatalog.Create(typeName, instanceName);

					Game.I.Status.Surfaces.Add(surface);
				}
				else
				{
					string instanceName = this.Tokens[0];
					string command = this.Tokens[1];
					string[] arguments = this.Tokens.Skip(2).ToArray();

					Surface surface = Game.I.Status.GetSurface(instanceName);

					surface.Invoke(command, arguments);
				}
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog("コマンドの実行中にエラーが発生しました。エラーになったトークン列は以下のとおりです。");

				foreach (string token in this.Tokens)
					ProcMain.WriteLog(token);

				if (DDConfig.LOG_ENABLED)
					throw;

				ProcMain.WriteLog("ゲームを続行します。" + ex);
			}
		}
	}
}
