using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 敵
	/// </summary>
	public abstract class Enemy
	{
		// プレイヤーに撃破されない(自弾に当たらない)敵を作る場合 HP == 0 にすること。
		// -- アイテム・敵弾など
		// プレイヤーに当たらない敵を作る場合 E_Draw において Crash を設定しないこと。
		// -- アイテムなど

		public double X;
		public double Y;

		/// <summary>
		/// 体力
		/// 0 == 無敵
		/// -1 == 死亡
		/// </summary>
		public int HP;

		public enum Kind_e
		{
			通常敵 = 1,
			アイテム,
			ボス,
		}

		public Kind_e Kind;

		// 敵：
		// -- new Enemy(x, y, hp, Kind_e.通常敵)
		// 敵弾：
		// -- new Enemy(x, y, 0, Kind_e.通常敵)
		// アイテム：
		// -- new Enemy(x, y, 0, Kind_e.アイテム) -- Crash を更新しない。
		// ボス：
		// -- new Enemy(x, y, hp, Kind_e.ボス)

		public Enemy(double x, double y, int hp, Kind_e kind = Kind_e.通常敵)
		{
			this.X = x;
			this.Y = y;
			this.HP = hp;
			this.Kind = kind;
		}

		/// <summary>
		/// この敵を消滅させるか
		/// 撃破された場合、画面外に出た場合などこの敵を消滅させたい場合 true をセットすること。
		/// これにより「フレームの最後に」敵リストから除去される。
		/// </summary>
		public bool DeadFlag
		{
			set
			{
				if (value)
					this.HP = -1;
				else
					throw null; // never
			}

			get
			{
				return this.HP == -1;
			}
		}

		/// <summary>
		/// 現在のフレームにおける当たり判定を保持する。
		/// -- E_Draw によって設定される。
		/// </summary>
		public DDCrash Crash = DDCrashUtils.None();

		private Func<bool> _draw = null;

		public void Draw()
		{
			if (_draw == null)
				_draw = SCommon.Supplier(this.E_Draw());

			if (!_draw())
				this.DeadFlag = true;
		}

		/// <summary>
		/// 現在のフレームにおける描画を行う。
		/// するべきこと：
		/// -- 行動・移動
		/// -- 描画
		/// -- Crash を設定する。-- プレイヤーに当たらないなら設定しない。
		/// -- 必要に応じて Game.I.Enemies.Add(enemy); する。-- 敵の追加
		/// -- 必要に応じて DeadFlag に true を設定する。(または false を返す) -- 敵(自分自身)の削除
		/// ---- 呼び出し関係がややこしくなりそうなので Kill, Killed は呼び出さないこと。
		/// ---- 敵(自分以外)を削除するには otherEnemy.DeadFlag = true; する。
		/// </summary>
		/// <returns>列挙：この敵は生存しているか</returns>
		protected abstract IEnumerable<bool> E_Draw();

		/// <summary>
		/// 被弾した。
		/// </summary>
		public virtual void Damaged()
		{
			// TODO: SE
		}

		/// <summary>
		/// Killed 複数回実行回避のため、DeadFlag をチェックして Killed を実行する。
		/// 注意：HP を減らして -1 になったとき Kill を呼んでも(DeadFlag == true になるため) Killed は実行されない！
		/// -- HP == -1 の可能性がある場合は -- Kill(true);
		/// </summary>
		/// <param name="force">強制モード</param>
		public void Kill(bool force = false)
		{
			if (force || !this.DeadFlag)
			{
				this.DeadFlag = true;
				this.Killed();
			}
		}

		/// <summary>
		/// 撃破されて消滅した。
		/// 注意：本メソッドを複数回実行しないように注意すること！
		/// -- DeadFlag == true の敵を { DeadFlag = true; Killed(); } してしまわないように！
		/// </summary>
		protected virtual void Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.中爆発(this.X, this.Y)));

			foreach (Action<Enemy> a_killed in this.A_KilledList)
				a_killed(this);

			// TODO: SE
		}

		private List<Action<Enemy>> A_KilledList = new List<Action<Enemy>>();

		/// <summary>
		/// 死亡イベントの追加
		/// コンストラクタ呼び出しとの併用を考慮して、このインスタンスを返す。
		/// </summary>
		/// <param name="a_killed">死亡イベント</param>
		/// <returns>このインスタンス</returns>
		public Enemy AddKilled(Action<Enemy> a_killed)
		{
			this.A_KilledList.Add(a_killed);
			return this;
		}
	}
}
