using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Scripts;

namespace Charlotte.Games
{
	public class MapWall
	{
		public enum Kind_e
		{
			NONE = 1,
			WALL,
			GATE,
		}

		public Kind_e Kind = Kind_e.NONE;

		/// <summary>
		/// ここで発生するイベント・スクリプト
		/// この位置・方向を向いた時点で this.Script.Perform() を実行する。
		/// null == イベント無し
		/// </summary>
		public Script Script;
	}
}
