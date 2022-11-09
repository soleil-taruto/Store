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
		/// <param name="xRate">マップにおける画面の位置(X位置_レート)</param>
		/// <param name="yRate">マップにおける画面の位置(Y位置_レート)</param>
		public void Draw(double xRate, double yRate)
		{
			if (_draw == null)
				_draw = SCommon.Supplier(this.E_Draw());

			this.DrawXRate = xRate;
			this.DrawYRate = yRate;

			if (!_draw())
				throw null; // never
		}

		/// <summary>
		/// マップにおける画面の位置(X位置_レート)
		/// -- 0.0 == 画面の左側がマップの左側と重なっている。
		/// -- 1.0 == 画面の右側がマップの右側と重なっている。
		/// </summary>
		protected double DrawXRate;

		/// <summary>
		/// マップにおける画面の位置(Y位置_レート)
		/// -- 0.0 == 画面の上側がマップの上側と重なっている。
		/// -- 1.0 == 画面の下側がマップの下側と重なっている。
		/// </summary>
		protected double DrawYRate;

		/// <summary>
		/// 壁紙を描画する。
		/// 以下のフィールドの値が保証される。
		/// -- this.DrawXRate
		/// -- this.DrawYRate
		/// </summary>
		/// <returns>列挙：真を返し続けること</returns>
		protected abstract IEnumerable<bool> E_Draw();
	}
}
