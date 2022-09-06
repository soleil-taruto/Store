using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	/// <summary>
	/// 現在登場中のキャラクタやオブジェクトの状態を保持する。
	/// </summary>
	public abstract class Surface
	{
		public string TypeName; // ロード時に必要だった_今は不要
		public string InstanceName;

		public Surface(string typeName, string instanceName)
		{
			this.TypeName = typeName;
			this.InstanceName = instanceName;
		}

		/// <summary>
		/// アクションのリスト
		/// Act.Draw が false を返したとき this.Draw を実行しなければならない。
		/// セーブ・ロード時にこのフィールドは保存・再現されない。
		/// -- セーブ前に Flush しなければならない。
		/// </summary>
		public NovelAct Act = new NovelAct();

		public double X = DDConsts.Screen_W / 2;
		public double Y = DDConsts.Screen_H / 2;
		public int Z = 0;

		/// <summary>
		/// コマンドを実行する。
		/// ここでは共通のコマンドを処理し、個別のコマンドを処理するために Invoke_02 を呼び出す。
		/// ★コマンドの処理は原則的に Act へ追加すること。
		/// -- Act へ追加しない場合は if 行に「即時」とコメントする。
		/// -- 非即時コマンド名と区別するために、接頭辞 I- を付ける。(Immediate)
		/// </summary>
		/// <param name="command">コマンド名</param>
		/// <param name="arguments">コマンド引数列</param>
		public void Invoke(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "位置")
			{
				if (arguments.Length == 3)
				{
					this.Act.AddOnce(() =>
					{
						this.X = double.Parse(arguments[c++]);
						this.Y = double.Parse(arguments[c++]);
						this.Z = int.Parse(arguments[c++]);
					});
				}
				else if (arguments.Length == 2)
				{
					this.Act.AddOnce(() =>
					{
						this.X = double.Parse(arguments[c++]);
						this.Y = double.Parse(arguments[c++]);
					});
				}
				else
				{
					throw new DDError();
				}
			}
			else if (command == "X")
			{
				this.Act.AddOnce(() => this.X = double.Parse(arguments[c++]));
			}
			else if (command == "Y")
			{
				this.Act.AddOnce(() => this.Y = double.Parse(arguments[c++]));
			}
			else if (command == "Z")
			{
				this.Act.AddOnce(() => this.Z = int.Parse(arguments[c++]));
			}
			else if (command == "End")
			{
				this.Act.AddOnce(() => this.DeadFlag = true);
			}
			else if (command == "Flush") // 即時
			{
				this.Act.Flush();
			}
			else if (command == "Sleep") // 描画せずに待つ
			{
				int frame = int.Parse(arguments[c++]);

				if (frame < 1)
					throw new DDError("Bad (sleeping) frame: " + frame);

				int endFrame = DDEngine.ProcFrame + frame;

				this.Act.Add(() => DDEngine.ProcFrame < endFrame && !NovelAct.IsFlush);
			}
			else if (command == "Keep") // 描画しながら待つ
			{
				int frame = int.Parse(arguments[c++]);

				if (frame < 1)
					throw new DDError("Bad (keeping) frame: " + frame);

				int endFrame = DDEngine.ProcFrame + frame;

				this.Act.Add(() =>
				{
					this.Draw();
					return DDEngine.ProcFrame < endFrame && !NovelAct.IsFlush;
				});
			}
			else
			{
				this.Invoke_02(command, arguments);
			}
		}

		private Func<bool> _draw = null;
		public bool DeadFlag = false;

		public void Draw()
		{
			if (_draw == null)
				_draw = SCommon.Supplier(this.E_Draw());

			if (!_draw())
				this.DeadFlag = true;
		}

		/// <summary>
		/// 描画する。
		/// </summary>
		/// <returns>このサーフェスを継続するか</returns>
		public abstract IEnumerable<bool> E_Draw();

		/// <summary>
		/// 固有のコマンドを実行する。
		/// ★コマンドの処理は原則的に Act へ追加すること。
		/// -- Act へ追加しない場合は if 行に「即時」とコメントする。
		/// -- 非即時コマンド名と区別するために、接頭辞 I- を付ける。(Immediate)
		/// </summary>
		/// <param name="command">コマンド名</param>
		/// <param name="arguments">コマンド引数列</param>
		protected virtual void Invoke_02(string command, params string[] arguments)
		{
			throw new DDError();
		}
	}
}
