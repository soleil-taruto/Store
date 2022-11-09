using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.Games.Walls
{
	/// <summary>
	/// 壁紙
	/// 視覚的な背景
	/// </summary>
	public abstract class Wall
	{
		private Func<bool> _draw = null;

		/// <summary>
		/// 壁紙を描画する。
		/// </summary>
		public void Draw()
		{
			if (_draw == null)
				_draw = SCommon.Supplier(this.E_Draw());

			if (!_draw())
				throw null; // never
		}

		/// <summary>
		/// 壁紙を描画する。
		/// </summary>
		/// <returns>列挙：真を返し続けること</returns>
		protected abstract IEnumerable<bool> E_Draw();
	}
}
