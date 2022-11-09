using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4(ar);
			}
			Common.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// -- choose one --

			Main4(new ArgsReader(new string[] { @"C:\temp\Ricecake" }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --

			Common.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				//MessageBox.Show("" + ex, ProcMain.APP_TITLE + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			string targDir = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(targDir))
				throw new Exception("no targDir: " + targDir);

			foreach (string file in Directory.GetFiles(targDir, "*", SearchOption.AllDirectories))
			{
				string ext = Path.GetExtension(file).ToLower();

				if (
					ext == ".js" ||
					ext == ".jsp"
					)
					ProcJSPFile(file);
			}
		}

		private class TextData_t
		{
			public string FilePath;
			public byte[] Text;

			// <---- prm

			public int this[int index]
			{
				get
				{
					if (index < 0 || this.Text.Length <= index)
						return ' ';

					return this.Text[index];
				}
			}

			public int First(string ptn, int index)
			{
				byte[] bPtn = Encoding.ASCII.GetBytes(ptn);

				for (; index + bPtn.Length <= this.Text.Length; index++)
					if (IsSamePart(bPtn, index))
						return index;

				return -1;
			}

			private bool IsSamePart(byte[] bPtn, int index)
			{
				for (int i = 0; i < bPtn.Length; i++)
					if (bPtn[i] != this.Text[index + i])
						return false;

				return true;
			}

			public int First(Predicate<int> match, int index)
			{
				for (; index < this.Text.Length; index++)
					if (match((int)this.Text[index]))
						return index;

				return -1;
			}

			public string GetPartString(int start, int end)
			{
				return Encoding.ASCII.GetString(this.GetPart(start, end));
			}

			private byte[] GetPart(int start, int end)
			{
				if (
					start < 0 ||
					end < start ||
					this.Text.Length < end
					)
					throw new ArgumentException(start + ", " + end + ", " + this.Text.Length);

				int count = end - start;
				byte[] buff = new byte[count];

				for (int index = 0; index < count; index++)
					buff[index] = this.Text[start + index];

				return buff;
			}

			public int InsertLOGPOS(string lead, string trail, int index)
			{
				if (index < 0 || this.Text.Length < index)
					throw new ArgumentException(index + ", " + this.Text.Length);

				byte[] bLead = this.GetPart(0, index);
				int lineNumb = bLead.Where(v => v == '\n').Count() + 1;
				byte[] bMid = Encoding.ASCII.GetBytes(lead + Path.GetFileName(this.FilePath) + " (" + lineNumb + ") " + trail);
				byte[] bTrail = this.GetPart(index, this.Text.Length);
				this.Text = SCommon.Join(new byte[][] { bLead, bMid, bTrail });
				return bLead.Length + bMid.Length;
			}
		}

		private TextData_t _t;

		private void ProcJSPFile(string file)
		{
			ProcMain.WriteLog("file: " + file);

			_t = new TextData_t() { FilePath = file, Text = File.ReadAllBytes(file) };

			for (int index = 0; ; )
			{
				int p = _t.First("function", index);

				if (p == -1)
					break;

				index = p + 8; // (暫定)次回検索位置

				if (' ' < _t[p - 1] && _t[p - 1] != '(' && _t[p - 1] != ':') // (function()... とか name:function() とか許す。
					continue;

				p += 8;

				if (' ' < _t[p] && _t[p] != '(')
					continue;

				p = _t.First(chr => ' ' < chr, p);

				if (p == -1)
					continue;

				string name;

				if (_t[p] == '(')
				{
					name = "NO_NAME";
				}
				else
				{
					{
						int q = _t.First(chr => !(SCommon.DECIMAL + SCommon.ALPHA + SCommon.alpha + "_").Contains((char)chr), p);

						if (q == -1)
							continue;

						name = _t.GetPartString(p, q);
						p = q;
					}

					p = _t.First(chr => ' ' < chr, p);

					if (p == -1 || _t[p] != '(')
						continue;
				}
				p++;
				p = _t.First(chr => chr == ')', p);

				if (p == -1)
					continue;

				p++;
				p = _t.First(chr => ' ' < chr, p);

				if (p == -1 || _t[p] != '{')
					continue;

				p++;
				p = _t.InsertLOGPOS(" console.log('", name + "');", p);

				index = p; // 次回検索位置
			}
			for (int index = 0; ; )
			{
				int p = _t.First("=>", index);

				if (p == -1)
					break;

				index = p + 2; // (暫定)次回検索位置

				p += 2;
				p = _t.First(chr => ' ' < chr, p);

				if (p == -1 || _t[p] != '{')
					continue;

				p++;
				p = _t.InsertLOGPOS(" console.log('", "LAMBDA');", p);

				index = p; // 次回検索位置
			}
			for (int index = 0; ; )
			{
				int p = _t.First("$.ajax", index);

				if (p == -1)
					break;

				index = p + 6; // (暫定)次回検索位置

				p += 2;
				p = _t.First(chr => chr == '{', p);

				if (p == -1)
					continue;

				p++;
				p = _t.InsertLOGPOS(" LOGPOS_dummy: function() { console.log('", "AJAX'); }(),", p);

				index = p; // 次回検索位置
			}
			File.WriteAllBytes(file, _t.Text);
		}
	}
}
