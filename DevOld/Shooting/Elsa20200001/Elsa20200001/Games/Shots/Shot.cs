using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Shots
{
	/// <summary>
	/// 自弾(プレイヤーの弾)
	/// </summary>
	public abstract class Shot
	{
		public double X;
		public double Y;
		public int AttackPoint;
		public bool 敵を貫通する;

		public enum Kind_e
		{
			通常弾 = 1,
			ボム,
		}

		public Kind_e Kind;

		public Shot(double x, double y, int attackPoint, bool 敵を貫通する, Kind_e kind = Kind_e.通常弾)
		{
			this.X = x;
			this.Y = y;
			this.AttackPoint = attackPoint;
			this.敵を貫通する = 敵を貫通する;
			this.Kind = kind;
		}

		/// <summary>
		/// この自弾を消滅させるか
		/// 敵に当たった場合、画面外に出た場合などこの自弾を消滅させたい場合 true をセットすること。
		/// これにより「フレームの最後に」自弾リストから除去される。
		/// </summary>
		public bool DeadFlag = false;

		/// <summary>
		/// 現在のフレームにおける当たり判定を保持する。
		/// -- Draw によって設定される。
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
		/// -- Crash を設定する。-- 敵に当たらないなら設定しない。
		/// -- 必要に応じて Game.I.Shots.Add(shot); する。-- 自弾の追加
		/// -- 必要に応じて DeadFlag に true を設定する。(または false を返す) -- 自弾(自分自身)の削除
		/// ---- 呼び出し関係がややこしくなりそうなので Kill, Killed は呼び出さないこと。
		/// ---- 自弾(自分以外)を削除するには otherShot.DeadFlag = true; する。
		/// </summary>
		/// <returns>列挙：この自弾は生存しているか</returns>
		protected abstract IEnumerable<bool> E_Draw();

		/// <summary>
		/// Killed 複数回実行回避のため、DeadFlag をチェックして Killed を実行する。
		/// </summary>
		public void Kill()
		{
			if (!this.DeadFlag)
			{
				this.DeadFlag = true;
				this.Killed();
			}
		}

		/// <summary>
		/// 衝突して消滅した。
		/// </summary>
		protected virtual void Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.小爆発(this.X, this.Y)));

			// TODO: SE
		}
	}
}
