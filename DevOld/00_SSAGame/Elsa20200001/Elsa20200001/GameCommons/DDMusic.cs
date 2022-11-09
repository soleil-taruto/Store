using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace Charlotte.GameCommons
{
	public class DDMusic
	{
		public bool Globally = false; // 廃止予定
		public bool Locally { get { return !this.Globally; } } // 廃止予定
		public DDSound Sound;
		public double Volume = 0.5; // 廃止予定

		public DDMusic(string file)
			: this(new DDSound(file))
		{ }

		public DDMusic(Func<byte[]> getFileData)
			: this(new DDSound(getFileData))
		{ }

		public DDMusic(DDSound sound_binding)
		{
			this.Sound = sound_binding;
			this.Sound.PostLoadeds.Add(handle => DDSoundUtils.SetVolume(handle, 0.0)); // ロードしたらミュートしておく。

			DDMusicUtils.Add(this);
		}

		/// <summary>
		/// ループを設定する。
		/// 初期化時に呼び出すこと。
		/// ハンドルのロード前に呼び出すこと。
		/// -- 例：DDMusic xxx = new DDMusic("xxx.mp3").SetLoopByStEnd(st, ed);
		/// </summary>
		/// <param name="loopStart">ループ開始位置(サンプル位置)</param>
		/// <param name="loopLength">ループの長さ(サンプル位置)</param>
		/// <returns>このインスタンス</returns>
		public DDMusic SetLoopByStEnd(int loopStart, int loopEnd)
		{
			this.Sound.PostLoadeds.Add(handle =>
			{
				DX.SetLoopSamplePosSoundMem(loopStart, handle); // ループ開始位置
				DX.SetLoopStartSamplePosSoundMem(loopEnd, handle); // ループ終了位置
			});

			return this;
		}

		public DDMusic SetLoopByStLength(int loopStart, int loopLength)
		{
			return this.SetLoopByStEnd(loopStart, loopStart + loopLength);
		}

		public void Play(bool once = false, bool resume = false, double volume = 1.0, int fadeFrameMax = 30)
		{
			this.Touch(); // 再生までタイムラグがある。再生時にラグらないよう、ここでロードしておく
			DDMusicUtils.Play(this, once, resume, volume, fadeFrameMax);
		}

		public void Touch()
		{
			this.Sound.GetHandle(0);
		}
	}
}
