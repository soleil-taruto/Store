using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Games
{
	public class SaveOrLoadMenu : IDisposable
	{
		public static SaveOrLoadMenu I;

		public SaveOrLoadMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Save(Action drawWall)
		{
			this.Perform(drawWall, true);
		}

		public SaveDataSlot Load(Action drawWall)
		{
			return this.Perform(drawWall, false);
		}

		private const int THUMB_W = 290;
		private const int THUMB_H = 200;

		private const int PAGE_NUM = 10;

		private Action DrawWall;
		private int PageIndex = 0; // 0 ～ (PAGE_NUM - 1)
		private int SelectedSaveDataSlotIndex = -1; // -1 == 未選択

		private SaveDataSlot Perform(Action a_drawWall, bool saveMode) // ret: null == セーブモード || (ロードモード && キャンセルした)
		{
			this.DrawWall = a_drawWall;

			DDHashedData thumbnail = saveMode ? this.MakeThumbnail() : null;
			SaveDataSlot ret = null;

			DDEngine.FreezeInput();

			for (; ; )
			{
				// ====
				// 入力判定ここから
				// ====

				if (
					DDInput.A.GetInput() == 1 ||
					DDInput.B.GetInput() == 1 ||
					DDMouse.R.GetInput() == -1
					)
					break;

				if (DDMouse.L.GetInput() == -1)
				{
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_前へ)
					{
						this.PageIndex--;
					}
					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_次へ)
					{
						this.PageIndex++;
					}

					DDUtils.ToRange(ref this.PageIndex, 0, PAGE_NUM - 1);

					if (this.LastHoveringButton == Ground.I.Picture.SettingButton_戻る)
					{
						break;
					}
					if (this.SelectedSaveDataSlotIndex != -1)
					{
						SaveDataSlot sdSlot = Ground.I.SaveDataSlots[this.SelectedSaveDataSlotIndex];

						if (saveMode) // ? セーブモード
						{
							if (new Confirm()
							{
								BorderColor =
									sdSlot.SerializedGameStatus != null ?
									new I3Color(255, 0, 0) :
									new I3Color(150, 150, 50)
							}
							.Perform(
								sdSlot.SerializedGameStatus != null ?
								"スロット " + (this.SelectedSaveDataSlotIndex + 1) + " のデータを上書きします。" :
								"スロット " + (this.SelectedSaveDataSlotIndex + 1) + " にセーブします。", "はい", "いいえ") == 0)
							{
								sdSlot.SerializedGameStatus = Game.I.Status.Serialize();
								sdSlot.SavedTime = new SCommon.SimpleDateTime(SCommon.TimeStampToSec.ToSec(DateTime.Now));
								sdSlot.Thumbnail = thumbnail;
							}
						}
						else // ? ロードモード
						{
							if (sdSlot.SerializedGameStatus != null) // ロードする。
							{
								if (new Confirm() { BorderColor = new I3Color(50, 100, 200) }
									.Perform("スロット " + (this.SelectedSaveDataSlotIndex + 1) + " のデータをロードします。", "はい", "いいえ") == 0)
								{
									ret = sdSlot;
									break;
								}
							}
						}
					}
				}

				// ====
				// 入力判定ここまで
				// ====

				// ====
				// 描画ここから
				// ====

				this.LastHoveringButton = null; // 不使用
				this.DrawWall();

				DDDraw.DrawSimple(Ground.I.Picture.詳細設定枠, 0, 0);

				DrawTabTitle(855, 70, saveMode ? "セーブ" : "ロード", true);

				int selSDSlotIndex = -1;
				int sdSlotIndex = this.PageIndex * 10;

				for (int y = 0; y < 2; y++)
				{
					for (int x = 0; x < 5; x++)
					{
						bool selected = this.SelectedSaveDataSlotIndex == sdSlotIndex;
						SaveDataSlot sdSlot = Ground.I.SaveDataSlots[sdSlotIndex];

						int slotX = 260 + x * 350;
						int slotY = 360 + y * 350;

						DDDraw.SetAlpha(selected ? 1.0 : 0.3);
						DDDraw.DrawBegin(Ground.I.Picture.SaveDataSlot, slotX, slotY);
						DDCrash drawedCrash = DDDraw.DrawGetCrash();
						DDDraw.DrawEnd();
						DDDraw.Reset();

						if (drawedCrash.IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
							selSDSlotIndex = sdSlotIndex;

						DDDraw.DrawCenter(DDHashedResource.GetPicture(sdSlot.Thumbnail), slotX, slotY);

						DDPrint.SetBorder(new I3Color(0, 0, 150));
						DDPrint.SetPrint(slotX - 140, slotY - 145, 40);
						DDPrint.Print("" + (sdSlotIndex + 1));

						DDPrint.SetBorder(new I3Color(0, 0, 150));
						DDPrint.SetPrint(slotX - 150, slotY + 120, 40);
						DDPrint.Print(sdSlot.SavedTime.Year == 1 ?
							"----/--/--(--)--:--"
							//"----/--/-- --:--:--"
							: sdSlot.SavedTime.ToString(
							"{0:D4}/{1:D2}/{2:D2}({3}){4:D2}:{5:D2}"
							//"{0:D4}/{1:D2}/{2:D2} {4:D2}:{5:D2}:{6:D2}"
							));

						sdSlotIndex++;
					}
				}
				this.SelectedSaveDataSlotIndex = selSDSlotIndex;

				this.DrawButton(800, 950, Ground.I.Picture.SettingButton_前へ, 0 < this.PageIndex);
				this.DrawButton(1120, 950, Ground.I.Picture.SettingButton_次へ, this.PageIndex < PAGE_NUM - 1);
				this.DrawButton(1630, 950, Ground.I.Picture.SettingButton_戻る, true);

				// ====
				// 描画ここまで
				// ====

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			DDHashedResource.ClearPicture();

			return ret;
		}

		// ◆◆◆
		// ◆◆◆ コンテンツ描画系メソッドここから
		// ◆◆◆

		private void DrawTabTitle(int x, int y, string line, bool activeFlag)
		{
			DDFontUtils.DrawString(
				x,
				y,
				line,
				DDFontUtils.GetFont("Kゴシック", 70),
				false,
				activeFlag ? new I3Color(100, 255, 255) : new I3Color(150, 150, 150),
				activeFlag ? new I3Color(50, 100, 100) : new I3Color(100, 100, 100)
				);
		}

		private DDPicture LastHoveringButton;
		//private bool LastButtonHoveringFlag;

		private void DrawButton(int x, int y, DDPicture picture, bool activeFlag)
		{
			DDDraw.SetAlpha(activeFlag ? 1.0 : 0.5);
			DDDraw.DrawBegin(picture, x, y);
			DDCrash drawedCrash = DDDraw.DrawGetCrash(); // 不使用
			DDDraw.DrawEnd();
			DDDraw.Reset();

			if (drawedCrash.IsCrashed(DDCrashUtils.Point(new D2Point(DDMouse.X, DDMouse.Y))))
			{
				this.LastHoveringButton = picture;
				//this.LastButtonHoveringFlag = true;
			}
			else
			{
				//this.LastButtonHoveringFlag = false;
			}
		}

		private DDHashedData MakeThumbnail()
		{
			DDMain.KeepMainScreen();

			using (DDSubScreen screen = new DDSubScreen(THUMB_W, THUMB_H))
			using (screen.Section())
			{
				DDDraw.DrawRect(DDGround.KeptMainScreen.ToPicture(), 0, 0, THUMB_W, THUMB_H);

				using (WorkingDir wd = new WorkingDir())
				{
					string bmpFile = wd.MakePath();
					string pngFile = wd.MakePath();

					DX.SaveDrawScreenToBMP(0, 0, THUMB_W, THUMB_H, bmpFile);

					using (
						Bitmap // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
							bmp =
								(Bitmap)Bitmap.FromFile( // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
									bmpFile
									))
					{
						bmp
							.Save( // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
								pngFile,
								ImageFormat.Png // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa
								);
					}
					return new DDHashedData(File.ReadAllBytes(pngFile));
				}
			}
		}
	}
}
