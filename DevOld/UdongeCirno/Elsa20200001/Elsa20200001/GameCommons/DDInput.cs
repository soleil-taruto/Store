using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons
{
	public static class DDInput
	{
		public class Button
		{
			public int[] BtnIds = new int[0]; // { } == 未割り当て
			public int[] KeyIds = new int[0]; // { } == 未割り当て

			// <---- prm

			public bool FreezeInputUntilReleaseFlag = false;
			public int Status = 0;

			public int GetInput()
			{
				if (this.FreezeInputUntilReleaseFlag)
				{
					if (1 <= this.Status)
						return 0;

					this.FreezeInputUntilReleaseFlag = false;
				}
				return 1 <= DDEngine.FreezeInputFrame ? 0 : this.Status;
			}

			public bool IsPound()
			{
				return DDUtils.IsPound(this.GetInput());
			}

			private int[][] BackupData = null;

			public void Backup()
			{
				this.BackupData = new int[][] { this.BtnIds, this.KeyIds };
			}

			public void Restore()
			{
				int c = 0;

				this.BtnIds = this.BackupData[c++];
				this.KeyIds = this.BackupData[c++];

				this.BackupData = null;
			}
		}

		// ボタンの初期化 --> DDGround.INIT()
		// ボタンの保存・読み込み --> DDSaveData.Save(), DDSaveData.Load()
		// ボタンの設定 --> SimpleMenu.PadConfig()

		public static Button DIR_2 = new Button();
		public static Button DIR_4 = new Button();
		public static Button DIR_6 = new Button();
		public static Button DIR_8 = new Button();
		public static Button A = new Button();
		public static Button B = new Button();
		public static Button C = new Button();
		public static Button D = new Button();
		public static Button E = new Button();
		public static Button F = new Button();
		public static Button L = new Button();
		public static Button R = new Button();
		public static Button PAUSE = new Button();
		public static Button START = new Button();

		private static void MixInput(Button button, bool oppositeInput = false)
		{
			bool keyDown = button.KeyIds.Any(keyId => keyId != -1 && 1 <= DDKey.GetInput(keyId));
			bool btnDown = button.BtnIds.Any(btnId => btnId != -1 && 1 <= DDPad.GetInput(DDGround.PrimaryPadId, btnId));

			DDUtils.UpdateInput(ref button.Status, !oppositeInput && (keyDown || btnDown));
		}

		public static void EachFrame()
		{
			int freezeInputFrame_BKUP = DDEngine.FreezeInputFrame;
			DDEngine.FreezeInputFrame = 0;

			MixInput(DIR_2);
			MixInput(DIR_4);
			MixInput(DIR_6, 1 <= DIR_4.Status);
			MixInput(DIR_8, 1 <= DIR_2.Status);
			MixInput(A);
			MixInput(B);
			MixInput(C);
			MixInput(D);
			MixInput(E);
			MixInput(F);
			MixInput(L);
			MixInput(R);
			MixInput(PAUSE);
			MixInput(START);

			DDEngine.FreezeInputFrame = freezeInputFrame_BKUP;
		}
	}
}
