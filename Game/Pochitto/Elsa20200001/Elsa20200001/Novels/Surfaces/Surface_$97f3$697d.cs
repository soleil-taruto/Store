using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
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
			}
			else if (command == "停止")
			{
				this.Act.AddOnce(() =>
				{
					this.MusicFile = null;
					this.MusicFileChanged();
				});
			}
			else
			{
				throw new DDError();
			}
		}

		private void MusicFileChanged()
		{
			if (this.MusicFile == null)
				DDMusicUtils.Fadeout();
			else
				DDCCResource.GetMusic(this.MusicFile).Play();
		}
	}
}
