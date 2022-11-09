using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	/// <summary>
	/// 選択肢
	/// </summary>
	public class Surface_Select : Surface
	{
		public static bool Hide = false; // Novel から制御される。

		public Surface_Select(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 80000;
		}

		public class OptionInfo
		{
			public string Title = "ここに選択肢に表示する文字列を設定します。";
			public string ScenarioName = NovelConsts.DUMMY_SCENARIO_NAME;

			// <---- prm

			public bool MouseFocused = false;
		}

		public List<OptionInfo> Options = new List<OptionInfo>();

		public int GetMouseFocusedIndex()
		{
			for (int index = 0; index < this.Options.Count; index++)
				if (this.Options[index].MouseFocused)
					return index;

			return -1; // フォーカス無し
		}

		public override IEnumerable<bool> E_Draw()
		{
			Novel.I.SkipMode = false;

			for (; ; )
			{
				//Novel.I.CancelSkipAutoMode();

				if (
					this.Options.Count < NovelConsts.SELECT_OPTION_MIN ||
					this.Options.Count > NovelConsts.SELECT_OPTION_MAX
					)
					throw new DDError("選択肢の個数に問題があります。");

				// ---- 入力ここから

				if (!Novel.I.BacklogMode)
				{
					int moving = 0;

					if (
						DDKey.IsPound(DX.KEY_INPUT_UP) ||
						DDInput.DIR_8.IsPound()
						)
						moving = -1;

					if (
						DDKey.IsPound(DX.KEY_INPUT_DOWN) ||
						DDInput.DIR_2.IsPound()
						)
						moving = 1;

					if (moving != 0)
					{
						int optIndex = this.GetMouseFocusedIndex();

						if (optIndex == -1)
						{
							optIndex = 0;
						}
						else
						{
							optIndex += this.Options.Count + moving;
							optIndex %= this.Options.Count;
						}

						DDMouse.X =
							NovelConsts.SELECT_FRAME_L +
							Ground.I.Picture.MessageFrame_Button2.Get_W() -
							10;
						DDMouse.Y =
							NovelConsts.SELECT_FRAME_T + NovelConsts.SELECT_FRAME_T_STEP * optIndex +
							Ground.I.Picture.MessageFrame_Button2.Get_H() -
							10;

						DDMouse.PosChanged();
					}
				}

				// ---- ここから描画

				if (!Hide)
				{
					for (int index = 0; index < NovelConsts.SELECT_FRAME_NUM; index++)
					{
						DDPicture picture = Ground.I.Picture.MessageFrame_Button;

						if (index < this.Options.Count)
						{
							picture = Ground.I.Picture.MessageFrame_Button2;

							if (this.Options[index].MouseFocused)
								picture = Ground.I.Picture.MessageFrame_Button3;
						}

						DDDraw.DrawBeginRect(
							picture,
							NovelConsts.SELECT_FRAME_L,
							NovelConsts.SELECT_FRAME_T + NovelConsts.SELECT_FRAME_T_STEP * index,
							picture.Get_W(),
							picture.Get_H()
							);
						DDCrash drawedCrash = DDDraw.DrawGetCrash();
						DDDraw.DrawEnd();

						// フォーカスしている選択項目を再設定
						{
							if (index < this.Options.Count)
							{
								bool mouseIn = drawedCrash.IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y)));

								this.Options[index].MouseFocused = mouseIn;
							}
						}
					}
					for (int index = 0; index < this.Options.Count; index++)
					{
						const int title_x = 80;
						const int title_y = 28;

						DDFontUtils.DrawString(
							 NovelConsts.SELECT_FRAME_L + title_x,
							 NovelConsts.SELECT_FRAME_T + NovelConsts.SELECT_FRAME_T_STEP * index + title_y,
							 this.Options[index].Title,
							 DDFontUtils.GetFont("Kゴシック", 16),
							 false,
							 new I3Color(110, 100, 90)
							 );
					}
				}

				// 隠しているなら選択できない。
				if (Hide)
					foreach (OptionInfo option in this.Options)
						option.MouseFocused = false;

				yield return true;
			}
		}

		protected override void Invoke_02(string command, params string[] arguments)
		{
			int c = 0;

			if (command == "選択肢") // 即時
			{
				string title = arguments[c++];

				this.Options.Add(new OptionInfo()
				{
					Title = title,
				});

				return;
			}
			if (command == "分岐先") // 即時
			{
				this.Options[this.Options.Count - 1].ScenarioName = arguments[c++];
				return;
			}
			ProcMain.WriteLog(command);
			throw new DDError(); // Bad command
		}
	}
}
