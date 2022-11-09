using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;
using Charlotte.Games;
using Charlotte.GameTools;

namespace Charlotte.GameCommons
{
	public static class DDSaveData
	{
		public static void Save()
		{
			List<byte[]> blocks = new List<byte[]>();

			// Donut3 用のセーブデータ
			{
				List<string> lines = new List<string>();

				lines.Add(ProcMain.APP_IDENT);
				lines.Add(ProcMain.APP_TITLE);

				lines.Add("" + DDGround.RealScreen_W);
				lines.Add("" + DDGround.RealScreen_H);

				lines.Add("" + DDGround.RealScreenDraw_L);
				lines.Add("" + DDGround.RealScreenDraw_T);
				lines.Add("" + DDGround.RealScreenDraw_W);
				lines.Add("" + DDGround.RealScreenDraw_H);

				lines.Add("" + DDGround.UnfullScreen_W);
				lines.Add("" + DDGround.UnfullScreen_H);

				lines.Add("" + SCommon.ToLong(DDGround.MusicVolume * SCommon.IMAX));
				lines.Add("" + SCommon.ToLong(DDGround.SEVolume * SCommon.IMAX));

				lines.Add("" + P_Join(DDInput.DIR_2.BtnIds));
				lines.Add("" + P_Join(DDInput.DIR_4.BtnIds));
				lines.Add("" + P_Join(DDInput.DIR_6.BtnIds));
				lines.Add("" + P_Join(DDInput.DIR_8.BtnIds));
				lines.Add("" + P_Join(DDInput.A.BtnIds));
				lines.Add("" + P_Join(DDInput.B.BtnIds));
				lines.Add("" + P_Join(DDInput.C.BtnIds));
				lines.Add("" + P_Join(DDInput.D.BtnIds));
				lines.Add("" + P_Join(DDInput.E.BtnIds));
				lines.Add("" + P_Join(DDInput.F.BtnIds));
				lines.Add("" + P_Join(DDInput.L.BtnIds));
				lines.Add("" + P_Join(DDInput.R.BtnIds));
				lines.Add("" + P_Join(DDInput.PAUSE.BtnIds));
				lines.Add("" + P_Join(DDInput.START.BtnIds));

				lines.Add("" + P_Join(DDInput.DIR_2.KeyIds));
				lines.Add("" + P_Join(DDInput.DIR_4.KeyIds));
				lines.Add("" + P_Join(DDInput.DIR_6.KeyIds));
				lines.Add("" + P_Join(DDInput.DIR_8.KeyIds));
				lines.Add("" + P_Join(DDInput.A.KeyIds));
				lines.Add("" + P_Join(DDInput.B.KeyIds));
				lines.Add("" + P_Join(DDInput.C.KeyIds));
				lines.Add("" + P_Join(DDInput.D.KeyIds));
				lines.Add("" + P_Join(DDInput.E.KeyIds));
				lines.Add("" + P_Join(DDInput.F.KeyIds));
				lines.Add("" + P_Join(DDInput.L.KeyIds));
				lines.Add("" + P_Join(DDInput.R.KeyIds));
				lines.Add("" + P_Join(DDInput.PAUSE.KeyIds));
				lines.Add("" + P_Join(DDInput.START.KeyIds));

				lines.Add("" + (DDGround.RO_MouseDispMode ? 1 : 0));

				// D3SaveData_新しい項目をここへ追加...

				blocks.Add(DDUtils.SplitableJoin(lines.ToArray()));
			}

			// アプリ固有のセーブデータ
			{
				List<string> lines = new List<string>();

				//lines.Add("Donut3-SaveData"); // Dummy
				//lines.Add("Donut3-SaveData"); // Dummy
				//lines.Add("Donut3-SaveData"); // Dummy

				lines = SCommon.E_AddRange(lines, AppSaveDataUtils.GetAppLines()).ToList();

				blocks.Add(DDUtils.SplitableJoin(lines.ToArray()));
			}

			File.WriteAllBytes(DDConsts.SaveDataFile, DDJammer.Encode(SCommon.SplittableJoin(blocks.ToArray())));
		}

		public static void Load()
		{
			if (!File.Exists(DDConsts.SaveDataFile))
				return;

			byte[][] blocks = SCommon.Split(DDJammer.Decode(File.ReadAllBytes(DDConsts.SaveDataFile)));
			int bc = 0;

			string[] lines = DDUtils.Split(blocks[bc++]);
			int c = 0;

			if (lines[c++] != ProcMain.APP_IDENT)
				throw new DDError();

			if (lines[c++] != ProcMain.APP_TITLE)
				throw new DDError();

			// アプリのアップデートによって項目の更新・増減があっても処理を続行するように try ～ catch しておく。

			try // Donut3 のセーブデータ
			{
				// TODO int.Parse -> IntTools.ToInt

				DDGround.RealScreen_W = int.Parse(lines[c++]);
				DDGround.RealScreen_H = int.Parse(lines[c++]);

				DDGround.RealScreenDraw_L = int.Parse(lines[c++]);
				DDGround.RealScreenDraw_T = int.Parse(lines[c++]);
				DDGround.RealScreenDraw_W = int.Parse(lines[c++]);
				DDGround.RealScreenDraw_H = int.Parse(lines[c++]);

				DDGround.UnfullScreen_W = int.Parse(lines[c++]);
				DDGround.UnfullScreen_H = int.Parse(lines[c++]);

				DDGround.MusicVolume = long.Parse(lines[c++]) / (double)SCommon.IMAX;
				DDGround.SEVolume = long.Parse(lines[c++]) / (double)SCommon.IMAX;

				DDInput.DIR_2.BtnIds = P_Split(lines[c++]);
				DDInput.DIR_4.BtnIds = P_Split(lines[c++]);
				DDInput.DIR_6.BtnIds = P_Split(lines[c++]);
				DDInput.DIR_8.BtnIds = P_Split(lines[c++]);
				DDInput.A.BtnIds = P_Split(lines[c++]);
				DDInput.B.BtnIds = P_Split(lines[c++]);
				DDInput.C.BtnIds = P_Split(lines[c++]);
				DDInput.D.BtnIds = P_Split(lines[c++]);
				DDInput.E.BtnIds = P_Split(lines[c++]);
				DDInput.F.BtnIds = P_Split(lines[c++]);
				DDInput.L.BtnIds = P_Split(lines[c++]);
				DDInput.R.BtnIds = P_Split(lines[c++]);
				DDInput.PAUSE.BtnIds = P_Split(lines[c++]);
				DDInput.START.BtnIds = P_Split(lines[c++]);

				DDInput.DIR_2.KeyIds = P_Split(lines[c++]);
				DDInput.DIR_4.KeyIds = P_Split(lines[c++]);
				DDInput.DIR_6.KeyIds = P_Split(lines[c++]);
				DDInput.DIR_8.KeyIds = P_Split(lines[c++]);
				DDInput.A.KeyIds = P_Split(lines[c++]);
				DDInput.B.KeyIds = P_Split(lines[c++]);
				DDInput.C.KeyIds = P_Split(lines[c++]);
				DDInput.D.KeyIds = P_Split(lines[c++]);
				DDInput.E.KeyIds = P_Split(lines[c++]);
				DDInput.F.KeyIds = P_Split(lines[c++]);
				DDInput.L.KeyIds = P_Split(lines[c++]);
				DDInput.R.KeyIds = P_Split(lines[c++]);
				DDInput.PAUSE.KeyIds = P_Split(lines[c++]);
				DDInput.START.KeyIds = P_Split(lines[c++]);

				DDGround.RO_MouseDispMode = int.Parse(lines[c++]) != 0;

				// D3SaveData_新しい項目をここへ追加...
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}

			Load_Delay = () =>
			{
				lines = DDUtils.Split(blocks[bc++]);

				try // アプリ固有のセーブデータ
				{
					//c = 0;
					//DDUtils.Noop(lines[c++]); // Dummy
					//DDUtils.Noop(lines[c++]); // Dummy
					//DDUtils.Noop(lines[c++]); // Dummy

					AppSaveDataUtils.UngetAppLines(lines);
				}
				catch (Exception e)
				{
					ProcMain.WriteLog(e);
				}

				Load_Delay = () => { }; // reset
			};
		}

		public static Action Load_Delay = () => { };

		private static string P_Join(int[] values)
		{
			return SCommon.Serializer.I.Join(values.Select(value => "" + value).ToArray());
		}

		private static int[] P_Split(string line)
		{
			return SCommon.Serializer.I.Split(line).Select(token => int.Parse(token)).ToArray();
		}
	}
}
