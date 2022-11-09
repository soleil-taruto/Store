using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Enemies;

namespace Charlotte.Games.Shots
{
	/// <summary>
	/// 自弾(プレイヤーの弾)
	/// </summary>
	public abstract class Shot
	{
		public double X;
		public double Y;
		public int Direction; // この自弾の進行方向 { 2, 4, 6, 8, 1, 3, 7, 9 } == { 下, 左, 右, 上, 左下, 右下, 左上, 右上 }
		public int AttackPoint;
		public bool 壁をすり抜ける;
		public bool 敵を貫通する;

		public Shot(double x, double y, int direction, int attackPoint, bool 壁をすり抜ける, bool 敵を貫通する)
		{
			this.X = x;
			this.Y = y;
			this.Direction = direction;
			this.AttackPoint = attackPoint;
			this.壁をすり抜ける = 壁をすり抜ける;
			this.敵を貫通する = 敵を貫通する;
		}

		/// <summary>
		/// この自弾を消滅させるか
		/// 敵に当たった場合、画面外に出た場合などこの自弾を消滅させたい場合 true をセットすること。
		/// これにより「フレームの最後に」自弾リストから除去される。
		/// </summary>
		public bool DeadFlag = false;

		/// <summary>
		/// 直前にクラッシュした敵
		/// 貫通武器について、貫通中に複数回ダメージを与えないように制御する。
		/// -- 複数の敵に同時に当たると意図通りにならないが、厳格に制御する必要は無いので、看過する。
		/// </summary>
		public Enemy LastCrashedEnemy = null;

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
		/// 何かに衝突して消滅した。
		/// </summary>
		protected virtual void Killed()
		{
			DDGround.EL.Add(SCommon.Supplier(Effects.小爆発(this.X, this.Y)));
		}
	}
}
