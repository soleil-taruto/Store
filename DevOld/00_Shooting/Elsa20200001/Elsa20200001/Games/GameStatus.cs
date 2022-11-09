using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	/// <summary>
	/// ゲームの状態を保持する。
	/// プレイヤーのレベルとか保有アイテムといった概念が入ってくることを想定して、独立したクラスとする。
	/// </summary>
	public class GameStatus
	{
		// (テスト時など)特にフィールドを設定せずにインスタンスを生成する使い方を想定して、
		// 全てのパラメータはデフォルト値で初期化すること。

		public int Zanki = GameConsts.DEFAULT_ZANKI;
		public int ZanBomb = GameConsts.DEFAULT_ZAN_BOMB;

		// 攻撃レベル
		// 初期値：0
		// 値域：0 ～ GameConsts.ATTACK_LEVEL_MAX
		//
		public int AttackLevel = 0;

		// <---- prm
	}
}
