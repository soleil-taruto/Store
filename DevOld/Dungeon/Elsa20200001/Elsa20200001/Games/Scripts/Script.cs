using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games.Scripts
{
	/// <summary>
	/// ダンジョンの特定の位置・方向において発生するイベント・スクリプト
	/// マップロード時にインスタンス化されるので、インスタンス化時に重い処理が走らないようにすること。
	/// </summary>
	public abstract class Script
	{
		/// <summary>
		/// イベント発生時に呼び出される。
		/// メソッドの終了によってイベントの終了となる。
		/// </summary>
		public abstract void Perform();
	}
}
