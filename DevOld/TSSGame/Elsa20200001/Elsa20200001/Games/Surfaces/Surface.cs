using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games.Surfaces
{
	/// <summary>
	/// 現在登場中のキャラクタやオブジェクトの状態を保持する。
	/// GameStatus の一部であるため、セーブ・ロード時にこのクラスの内容を保存・再現する。
	/// </summary>
	public abstract class Surface
	{
		// memo:
		// 抽象クラス(abstract クラス)とそれを継承したクラスもコンストラクタ以外の初期化の方法を持つべきではない。
		// -- prm にするとフィールド初期化し忘れる。ややこしくなる。初期化方法は明瞭であるべき。
		// インスタンス開放時に特定の手順を持つべきではない。
		// -- 廃止した GameStatus.RemoveSurface() -- そうしなくても良いように設計するべき。

		public string TypeName; // ロード時に必要
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
		public Act Act = new Act();

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
						this.X = int.Parse(arguments[c++]);
						this.Y = int.Parse(arguments[c++]);
						this.Z = int.Parse(arguments[c++]);
					});
				}
				else if (arguments.Length == 2)
				{
					this.Act.AddOnce(() =>
					{
						this.X = int.Parse(arguments[c++]);
						this.Y = int.Parse(arguments[c++]);
					});
				}
				else
				{
					throw new DDError();
				}
			}
			else if (command == "X")
			{
				this.Act.AddOnce(() => this.X = int.Parse(arguments[c++]));
			}
			else if (command == "Y")
			{
				this.Act.AddOnce(() => this.Y = int.Parse(arguments[c++]));
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

				this.Act.Add(() => DDEngine.ProcFrame < endFrame && !Act.IsFlush);
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
					return DDEngine.ProcFrame < endFrame && !Act.IsFlush;
				});
			}
			else
			{
				this.Invoke_02(command, arguments);
			}
		}

		/// <summary>
		/// シリアライザ
		/// 現在の状態を再現可能な文字列を返す。
		/// </summary>
		/// <returns></returns>
		public string Serialize()
		{
			return SCommon.Serializer.I.Join(SCommon.Concat(new string[][]
			{
				new string[]
				{
					//this.TypeName, // 不要
					//this.InstanceName, // 不要
					this.X.ToString(),
					this.Y.ToString(),
					this.Z.ToString(),
				},
				this.Serialize_02(),
			})
			.ToArray());
		}

		/// <summary>
		/// シリアライザ実行時の状態を再現する。
		/// </summary>
		/// <param name="lines">シリアライザから取得した状態データ</param>
		public void Deserialize(string value)
		{
			string[] lines = SCommon.Serializer.I.Split(value);
			int c = 0;

			//this.TypeName = lines[c++];
			//this.InstanceName = lines[c++];
			this.X = int.Parse(lines[c++]);
			this.Y = int.Parse(lines[c++]);
			this.Z = int.Parse(lines[c++]);
			this.Deserialize_02(lines.Skip(c).ToArray());
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

		private static readonly string[] SERIALIZED_DUMMY = new string[] { "SERIALIZED_DUMMY" };
		//private static readonly string[] SERIALIZED_DUMMY = new string[] { "SERIALIZED_DUMMY_01", "SERIALIZED_DUMMY_02", "SERIALIZED_DUMMY_03" };
		//private static readonly string[] SERIALIZED_DUMMY = SCommon.EMPTY_STRINGS;

		/// <summary>
		/// シリアライザ
		/// 現在の「固有の状態」を再現可能な文字列の配列を返す。
		/// </summary>
		/// <returns>状態データ</returns>
		protected virtual string[] Serialize_02()
		{
			return SERIALIZED_DUMMY;
		}

		/// <summary>
		/// シリアライザ実行時の「固有の状態」を再現する。
		/// </summary>
		/// <param name="lines">シリアライザから取得した状態データ</param>
		protected virtual void Deserialize_02(string[] lines)
		{
			if (SCommon.Comp(lines, SERIALIZED_DUMMY, SCommon.Comp) != 0)
				throw new DDError();
		}
	}
}
