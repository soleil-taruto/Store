using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons
{
	public static class DDSEUtils
	{
		public static List<DDSE> SEList = new List<DDSE>();

		public static void Add(DDSE se)
		{
			SEList.Add(se);
		}

		private class PlayInfo
		{
			public enum AlterCommand_e
			{
				NORMAL = 1,
				STOP,
				LOOP,
			}

			public DDSE SE;
			public AlterCommand_e AlterCommand;

			public PlayInfo(DDSE se, AlterCommand_e alterCommand = AlterCommand_e.NORMAL)
			{
				this.SE = se;
				this.AlterCommand = alterCommand;
			}
		}

		private static Queue<PlayInfo> PlayInfos = new Queue<PlayInfo>();

		public static bool EachFrame() // ret: ? 処理した。
		{
			if (1 <= PlayInfos.Count)
			{
				PlayInfo info = PlayInfos.Dequeue();

				if (info != null)
				{
					switch (info.AlterCommand)
					{
						case PlayInfo.AlterCommand_e.NORMAL:
							info.SE.HandleIndex++;
							info.SE.HandleIndex %= info.SE.Sound.HandleCount;

							if (DDSoundUtils.IsPlaying(info.SE.Sound.GetHandle(info.SE.HandleIndex)))
							{
								for (info.SE.HandleIndex = 0; info.SE.HandleIndex < info.SE.Sound.HandleCount; info.SE.HandleIndex++)
									if (!DDSoundUtils.IsPlaying(info.SE.Sound.GetHandle(info.SE.HandleIndex)))
										goto foundNotPlaying;

								//info.SE.HandleIndex = info.SE.Sound.HandleCount;
								info.SE.Sound.Extend();

								//ProcMain.WriteLog("音を拡張しました。" + info.SE.Sound.HandleCount);
							}
						foundNotPlaying:
							DDSoundUtils.Play(info.SE.Sound.GetHandle(info.SE.HandleIndex));
							break;

						case PlayInfo.AlterCommand_e.STOP:
							for (int index = 0; index < info.SE.Sound.HandleCount; index++)
							{
								DDSoundUtils.Stop(info.SE.Sound.GetHandle(index));
							}
							break;

						case PlayInfo.AlterCommand_e.LOOP:
							DDSoundUtils.Play(info.SE.Sound.GetHandle(0), false);
							break;

						default:
							throw new DDError();
					}
					return true;
				}
			}
			return false;
		}

		public static void Play(DDSE se)
		{
			int count = 0;

			foreach (PlayInfo info in PlayInfos)
				if (info != null && info.SE == se && 2 <= ++count)
					return;

			PlayInfos.Enqueue(new PlayInfo(se));
			PlayInfos.Enqueue(null);
		}

		public static void Stop(DDSE se)
		{
			PlayInfos.Enqueue(new PlayInfo(se, PlayInfo.AlterCommand_e.STOP));
			PlayInfos.Enqueue(null);
		}

		public static void PlayLoop(DDSE se)
		{
			PlayInfos.Enqueue(new PlayInfo(se, PlayInfo.AlterCommand_e.LOOP));
			PlayInfos.Enqueue(null);
		}

		public static void UpdateVolume()
		{
			foreach (DDSE se in SEList)
				se.UpdateVolume();
		}

		/// <summary>
		/// クリア対象の効果音は停止していること。
		/// -- 再生中に Unload したらマズいのかどうかは不明。多分マズいだろう。
		/// </summary>
		public static void UnloadAll()
		{
			foreach (DDSE se in SEList)
				se.Sound.Unload();
		}

		/// <summary>
		/// クリア対象の効果音は停止していること。
		/// -- 再生中に Unload したらマズいのかどうかは不明。多分マズいだろう。
		/// </summary>
		public static void UnloadLocally()
		{
			UnloadLocally(se => true);
		}

		public static void UnloadLocally(Predicate<DDSE> match)
		{
			foreach (DDSE se in SEList)
				if (se.Locally && match(se))
					se.Sound.Unload();
		}

		public static void TouchGlobally()
		{
			foreach (DDSE se in SEList)
				if (se.Globally)
					se.Sound.GetHandle(0);
		}
	}
}
