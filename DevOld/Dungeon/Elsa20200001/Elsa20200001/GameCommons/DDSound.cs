using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public class DDSound
	{
		private Func<byte[]> Func_GetFileData;
		private int[] Handles = null; // null == Unloaded

		public int HandleCount
		{
			get
			{
				return this.Handles == null ? 1 : this.Handles.Length;
			}
		}

		public List<Action<int>> PostLoadeds = new List<Action<int>>();

		public DDSound(string file)
			: this(() => DDResource.Load(file))
		{ }

		public DDSound(Func<byte[]> getFileData)
		{
			this.Func_GetFileData = getFileData;

			DDSoundUtils.Add(this);
		}

		public void Unload()
		{
			if (this.Handles != null)
			{
				foreach (int handle in this.Handles.Reverse()) // DuplicateSoundMem したハンドルから削除する。
					if (DX.DeleteSoundMem(handle) != 0) // ? 失敗
						throw new DDError();

				this.Handles = null;
			}
		}

		public bool IsLoaded()
		{
			return this.Handles != null;
		}

		public int GetHandle(int handleIndex)
		{
			if (this.Handles == null)
			{
				this.Handles = new int[1];

				{
					byte[] fileData = this.Func_GetFileData();
					int handle = -1;

					DDSystem.PinOn(fileData, p => handle = DX.LoadSoundMemByMemImage(p, (ulong)fileData.Length));

					if (handle == -1) // ? 失敗
						throw new DDError("Sound File SHA-512: " + SCommon.Hex.ToString(SCommon.GetSHA512(fileData)));

					this.Handles[0] = handle;
				}

				foreach (Action<int> routine in this.PostLoadeds)
					routine(this.Handles[0]);
			}
			return this.Handles[handleIndex];
		}

		public void Extend()
		{
			int handle = DX.DuplicateSoundMem(this.GetHandle(0));

			if (handle == -1) // ? 失敗
				throw new DDError();

			foreach (Action<int> routine in this.PostLoadeds)
				routine(handle);

			this.Handles = this.Handles.Concat(new int[] { handle }).ToArray();
		}

		public bool IsPlaying()
		{
			if (this.Handles != null)
				for (int index = 0; index < this.HandleCount; index++)
					if (DDSoundUtils.IsPlaying(this.Handles[index]))
						return true;

			return false;
		}

		public int[] GetHandles()
		{
			return this.Handles == null ? new int[0] : this.Handles;
		}
	}
}
