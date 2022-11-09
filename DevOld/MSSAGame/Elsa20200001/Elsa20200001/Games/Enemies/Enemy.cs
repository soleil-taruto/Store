using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Shots;

namespace Charlotte.Games.Enemies
{
	/// <summary>
	/// 敵
	/// </summary>
	public abstract class Enemy
	{
		// Game.I.ReloadEnemies() からロードされた場合、初期位置として「配置されたマップセルの中心座標」が与えられる。

		// this.X, this.Y はマップの座標(マップの左上を0,0とする)
		// -- 描画する際は DDGround.ICamera.X, DDGround.ICamera.Y をそれぞれ減じること。

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

		/// <summary>
		/// 攻撃力
		/// 1～
		/// </summary>
		public int AttackPoint;

		/// <summary>
		/// 自機に当たると消滅する。
		/// -- 敵弾を想定する。
		/// </summary>
		public bool 自機に当たると消滅する;

		// 敵：
		// -- new Enemy(x, y, hp, attackPoint, false)
		// 敵弾：
		// -- new Enemy(x, y, 0, attackPoint, 自機に当たると消滅する)
		// アイテム(イベント)：
		// -- new Enemy(x, y, 0, 0, false) -- Crash を更新しない。

		public Enemy(double x, double y, int hp, int attackPoint, bool 自機に当たると消滅する)
		{
			this.X = x;
			this.Y = y;
			this.HP = hp;
			this.AttackPoint = attackPoint;
			this.自機に当たると消滅する = 自機に当たると消滅する;
		}

		/// <summary>
		/// この敵を消滅させるか
		/// 撃破された場合などこの敵を消滅させたい場合 true をセットすること。
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
		/// 体力の減少などは呼び出し側でやっている。
		/// </summary>
		/// <param name="shot">この敵が被弾したプレイヤーの弾</param>
		public virtual void Damaged(Shot shot)
		{
#if true
			int count = shot.AttackPoint;

			DDGround.EL.Add(() =>
			{
				Ground.I.SE.EnemyDamaged.Play();
				return 0 < --count;
			});
#else // old
			Ground.I.SE.EnemyDamaged.Play();
#endif
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
		/// マップから離れすぎて消された場合・シナリオ的に消された場合などでは呼び出されない。
		/// 死亡フラグ立てなどは呼び出し側でやっている。
		/// 注意：本メソッドを複数回実行しないように注意すること！
		/// -- DeadFlag == true の敵を { DeadFlag = true; Killed(); } してしまわないように！
		/// </summary>
		protected virtual void Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.EnemyKilled(this.X, this.Y)));
			Ground.I.SE.EnemyKilled.Play();
		}
	}
}
