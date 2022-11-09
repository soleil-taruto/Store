using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	/// <summary>
	/// コンストラクタに指定された当たり判定を、このフレームだけ適用する。
	/// 手持ち武器を振り回した時の当たり判定などを想定
	/// -- 自機からの攻撃の当たり判定を、自弾に設定する仕組みのため。Shotを経由する必要がある。
	/// </summary>
	public class Shot_OneTime : Shot
	{
		private DDCrash OneTimeCrash;

		public Shot_OneTime(int attackPoint, DDCrash crash)
			: base(DDConsts.Screen_W / 2, DDConsts.Screen_H / 2, attackPoint, true)
		{
			this.OneTimeCrash = crash;
		}

		protected override IEnumerable<bool> E_Draw()
		{
			this.Crash = this.OneTimeCrash;

			// このフレームで生き残るために１回だけ真を返す。
			// -- 自弾が死亡するとクラッシュが無視される。
			yield return true;
		}
	}
}
