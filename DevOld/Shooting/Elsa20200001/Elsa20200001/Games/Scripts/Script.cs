using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Games.Scripts
{
	/// <summary>
	/// スクリプト
	/// 1つのステージを実行する。
	/// -- E_EachFrame の終了によって、このステージも終了する。
	/// </summary>
	public abstract class Script
	{
		private Func<bool> _eachFrame = null;

		public Func<bool> EachFrame
		{
			get
			{
				if (_eachFrame == null)
					_eachFrame = SCommon.Supplier(this.E_EachFrame());

				return _eachFrame;
			}
		}

		/// <summary>
		/// 真を返し続けること。
		/// 偽を返すと、このスクリプトを終了する。
		/// 処理すべきこと：
		/// -- 処理(内部状態を1フレーム分更新)
		/// -- 必要に応じて Game.I.Walls へ追加
		/// -- 必要に応じて Game.I.Enemies へ追加
		/// -- 必要に応じて 音楽の再生・停止 など
		/// スクリプトの差し替え：
		/// -- スクリプトの途中で Game.I.Script = scriptNew; として別のスクリプトに遷移して良い。
		/// -- 但し、スクリプトを差し替えた後で yield return true; を 1 回以上行うこと。
		/// ---- スクリプトを差し替えて列挙が終了すると Game.Perform() 側でスクリプトが終了したと判定され、
		/// ---- ゲームが終了してしまうため。
		/// </summary>
		/// <returns>列挙：このスクリプトを継続するか</returns>
		protected abstract IEnumerable<bool> E_EachFrame();
	}
}
