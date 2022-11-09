using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Surfaces
{
	public class Surface_音楽 : Surface
	{
		private string MusicFile = null; // null == 停止中

		public Surface_音楽(string typeName, string instanceName)
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

			if (command == "再生")
			{
				this.Act.AddOnce(() =>
				{
					this.MusicFile = arguments[c++];
					this.MusicFileChanged();
				});

				return;
			}
			if (command == "停止")
			{
				this.Act.AddOnce(() =>
				{
					this.MusicFile = null;
					this.MusicFileChanged();
				});

				return;
			}
			ProcMain.WriteLog(command);
			throw new DDError(); // Bad command
		}

		private void MusicFileChanged()
		{
			if (this.MusicFile == null)
				DDMusicUtils.Fadeout();
			else
				DDCCResource.GetMusic(this.MusicFile).Play();
		}

		protected override string[] Serialize_02()
		{
			return new string[]
			{
				GameCommon.WrapNullOrString(this.MusicFile),
			};
		}

		protected override void Deserialize_02(string[] lines)
		{
			int c = 0;

			this.MusicFile = GameCommon.UnwrapNullOrString(lines[c++]);
			this.MusicFileChanged();
		}
	}
}
