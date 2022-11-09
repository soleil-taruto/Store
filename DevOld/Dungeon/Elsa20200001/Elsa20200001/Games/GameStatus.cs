using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

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

		public int Dummy_01 = 1;
		public int Dummy_02 = 2;
		public int Dummy_03 = 3;

		// <---- prm

		#region Serialize / Deserialize

		public string Serialize()
		{
			List<string> dest = new List<string>();

			// ★★★ シリアライズ_ここから ★★★

			dest.Add("" + this.Dummy_01);
			dest.Add("" + this.Dummy_02);
			dest.Add("" + this.Dummy_03);

			// ★★★ シリアライズ_ここまで ★★★

			return SCommon.Serializer.I.Join(dest.ToArray());
		}

		private void S_Deserialize(string value)
		{
			string[] lines = SCommon.Serializer.I.Split(value);
			int c = 0;

			// ★★★ デシリアライズ_ここから ★★★

			this.Dummy_01 = int.Parse(lines[c++]);
			this.Dummy_02 = int.Parse(lines[c++]);
			this.Dummy_03 = int.Parse(lines[c++]);

			// ★★★ デシリアライズ_ここまで ★★★
		}

		public static GameStatus Deserialize(string value)
		{
			GameStatus gameStatus = new GameStatus();
			gameStatus.S_Deserialize(value);
			return gameStatus;
		}

		#endregion

		public GameStatus GetClone()
		{
			return Deserialize(this.Serialize());
		}
	}
}
