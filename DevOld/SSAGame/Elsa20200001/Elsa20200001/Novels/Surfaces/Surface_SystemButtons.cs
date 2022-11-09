using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Novels.Surfaces
{
	public class Surface_SystemButtons : Surface
	{
		public static bool Hide = false; // Novel から制御される。

		public Surface_SystemButtons(string typeName, string instanceName)
			: base(typeName, instanceName)
		{
			this.Z = 60000;
		}

		private double A = 1.0;
		private bool ControlSkipMode = false;

		public override IEnumerable<bool> E_Draw()
		{
			for (; ; )
			{
				// 入力：スキップモード・オートモード_解除
				if (
					DDMouse.Rot != 0 ||
					DDMouse.L.GetInput() == -1 && Novel.I.SelectedSystemButtonIndex == -1 || // システムボタン以外を左クリック
					DDMouse.R.GetInput() == -1 ||
					DDInput.A.GetInput() == 1
					)
					Novel.I.CancelSkipAutoMode();

				// 入力：スキップモード
				if (1 <= DDInput.L.GetInput())
				{
					Novel.I.SkipMode = true;
					this.ControlSkipMode = true;
				}
				else
				{
					if (this.ControlSkipMode)
					{
						Novel.I.SkipMode = false;
						this.ControlSkipMode = false;
					}
				}

				// ---- ここから描画

				DDUtils.Approach(ref this.A, Hide ? 0.0 : 1.0, 0.9);

				var buttons = new[]
				{
					// p: 画像, fp: フォーカス時の画像, oh: オプショナル・ハイライト
					new { p = Ground.I.Picture.MessageFrame_Skip, fp = Ground.I.Picture.MessageFrame_Skip2, oh = Novel.I.SkipMode },
					new { p = Ground.I.Picture.MessageFrame_Auto, fp = Ground.I.Picture.MessageFrame_Auto2, oh = Novel.I.AutoMode },
					new { p = Ground.I.Picture.MessageFrame_Log, fp = Ground.I.Picture.MessageFrame_Log2, oh = false },
					new { p = Ground.I.Picture.MessageFrame_Menu, fp = Ground.I.Picture.MessageFrame_Menu2, oh = false },
				};

				int selSysBtnIdx = -1;

				for (int index = 0; index < buttons.Length; index++)
				{
					bool focused = index == Novel.I.SelectedSystemButtonIndex;

					DDDraw.SetAlpha(this.A * (focused ? 1.0 : 0.8));
					DDDraw.DrawBegin(
						focused || buttons[index].oh ? buttons[index].fp : buttons[index].p,
						NovelConsts.SYSTEM_BUTTON_X + index * NovelConsts.SYSTEM_BUTTON_X_STEP,
						NovelConsts.SYSTEM_BUTTON_Y
						);
					DDCrash drawedCrash = DDDraw.DrawGetCrash();
					DDDraw.DrawEnd();
					DDDraw.Reset();

					if (drawedCrash.IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
					{
						selSysBtnIdx = index;
					}
				}
				Novel.I.SelectedSystemButtonIndex = selSysBtnIdx;

				// 隠しているなら選択できない。
				if (Hide)
					Novel.I.SelectedSystemButtonIndex = -1;

				yield return true;
			}
		}
	}
}
