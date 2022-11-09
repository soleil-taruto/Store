using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	/// <summary>
	/// プレイヤーキャラクタに関する情報と機能
	/// パーティの人数分生成し GameStatus.Players に保持する。
	/// </summary>
	public class Player
	{
		// (テスト時など)特にフィールドを設定せずにインスタンスを生成する使い方を想定して、
		// 全てのパラメータはデフォルト値で初期化すること。

		public enum Chara_e
		{
			HERO,
			HEROINE,
			LOW_HERO,
			CHAOS_HERO,
		}

		public Chara_e Chara = Chara_e.HERO;

		/// <summary>
		/// 体力
		/// -1 == 死亡
		/// 1～ == 生存
		/// </summary>
		public int HP = 100;
	}
}
