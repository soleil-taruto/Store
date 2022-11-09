namespace Charlotte
{
	partial class MainWin
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
			this.日付 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.金額 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.支払先 = new System.Windows.Forms.TextBox();
			this.支払先候補 = new System.Windows.Forms.ListBox();
			this.但し書き = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.但し書き候補 = new System.Windows.Forms.ListBox();
			this.金額Preview = new System.Windows.Forms.Label();
			this.日付Preview = new System.Windows.Forms.Label();
			this.MainTimer = new System.Windows.Forms.Timer(this.components);
			this.ErrorMessage = new System.Windows.Forms.Label();
			this.Known但し書きText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// 日付
			// 
			this.日付.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.日付.Location = new System.Drawing.Point(101, 12);
			this.日付.MaxLength = 8;
			this.日付.Name = "日付";
			this.日付.Size = new System.Drawing.Size(200, 27);
			this.日付.TabIndex = 1;
			this.日付.Text = "99991231";
			this.日付.TextChanged += new System.EventHandler(this.日付_TextChanged);
			this.日付.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.日付_KeyPress);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "YYYYMMDD";
			// 
			// 金額
			// 
			this.金額.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.金額.Location = new System.Drawing.Point(101, 45);
			this.金額.MaxLength = 8;
			this.金額.Name = "金額";
			this.金額.Size = new System.Drawing.Size(200, 27);
			this.金額.TabIndex = 4;
			this.金額.Text = "12345678";
			this.金額.TextChanged += new System.EventHandler(this.金額_TextChanged);
			this.金額.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.金額_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "金額";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 81);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 20);
			this.label3.TabIndex = 6;
			this.label3.Text = "支払先";
			// 
			// 支払先
			// 
			this.支払先.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.支払先.ImeMode = System.Windows.Forms.ImeMode.On;
			this.支払先.Location = new System.Drawing.Point(101, 78);
			this.支払先.MaxLength = 100;
			this.支払先.Name = "支払先";
			this.支払先.Size = new System.Drawing.Size(531, 27);
			this.支払先.TabIndex = 7;
			this.支払先.Text = "支払先";
			this.支払先.TextChanged += new System.EventHandler(this.支払先_TextChanged);
			this.支払先.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.支払先_KeyPress);
			this.支払先.KeyUp += new System.Windows.Forms.KeyEventHandler(this.支払先_KeyUp);
			// 
			// 支払先候補
			// 
			this.支払先候補.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.支払先候補.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.支払先候補.FormattingEnabled = true;
			this.支払先候補.ItemHeight = 20;
			this.支払先候補.Items.AddRange(new object[] {
            "支払先1",
            "支払先2",
            "支払先3",
            "支払先4",
            "支払先5"});
			this.支払先候補.Location = new System.Drawing.Point(101, 111);
			this.支払先候補.Name = "支払先候補";
			this.支払先候補.Size = new System.Drawing.Size(531, 104);
			this.支払先候補.TabIndex = 8;
			this.支払先候補.SelectedIndexChanged += new System.EventHandler(this.支払先候補_SelectedIndexChanged);
			this.支払先候補.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.支払先候補_KeyPress);
			// 
			// 但し書き
			// 
			this.但し書き.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.但し書き.ImeMode = System.Windows.Forms.ImeMode.On;
			this.但し書き.Location = new System.Drawing.Point(101, 221);
			this.但し書き.MaxLength = 100;
			this.但し書き.Name = "但し書き";
			this.但し書き.Size = new System.Drawing.Size(531, 27);
			this.但し書き.TabIndex = 10;
			this.但し書き.Text = "但し書き";
			this.但し書き.TextChanged += new System.EventHandler(this.但し書き_TextChanged);
			this.但し書き.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.但し書き_KeyPress);
			this.但し書き.KeyUp += new System.Windows.Forms.KeyEventHandler(this.但し書き_KeyUp);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 224);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(61, 20);
			this.label4.TabIndex = 9;
			this.label4.Text = "但し書き";
			// 
			// 但し書き候補
			// 
			this.但し書き候補.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.但し書き候補.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.但し書き候補.FormattingEnabled = true;
			this.但し書き候補.ItemHeight = 20;
			this.但し書き候補.Items.AddRange(new object[] {
            "但し書き1",
            "但し書き2",
            "但し書き3",
            "但し書き4",
            "但し書き5"});
			this.但し書き候補.Location = new System.Drawing.Point(101, 254);
			this.但し書き候補.Name = "但し書き候補";
			this.但し書き候補.Size = new System.Drawing.Size(531, 104);
			this.但し書き候補.TabIndex = 11;
			this.但し書き候補.SelectedIndexChanged += new System.EventHandler(this.但し書き候補_SelectedIndexChanged);
			this.但し書き候補.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.但し書き候補_KeyPress);
			// 
			// 金額Preview
			// 
			this.金額Preview.AutoSize = true;
			this.金額Preview.Location = new System.Drawing.Point(307, 48);
			this.金額Preview.Name = "金額Preview";
			this.金額Preview.Size = new System.Drawing.Size(223, 20);
			this.金額Preview.TabIndex = 5;
			this.金額Preview.Text = "--->　１２　万　３　， ４５６　円";
			// 
			// 日付Preview
			// 
			this.日付Preview.AutoSize = true;
			this.日付Preview.Location = new System.Drawing.Point(307, 15);
			this.日付Preview.Name = "日付Preview";
			this.日付Preview.Size = new System.Drawing.Size(258, 20);
			this.日付Preview.TabIndex = 2;
			this.日付Preview.Text = "--->　２０１８　年　０１　月　３１　日";
			// 
			// MainTimer
			// 
			this.MainTimer.Interval = 30;
			this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// ErrorMessage
			// 
			this.ErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ErrorMessage.AutoSize = true;
			this.ErrorMessage.BackColor = System.Drawing.Color.Red;
			this.ErrorMessage.ForeColor = System.Drawing.Color.White;
			this.ErrorMessage.Location = new System.Drawing.Point(12, 392);
			this.ErrorMessage.Name = "ErrorMessage";
			this.ErrorMessage.Size = new System.Drawing.Size(113, 20);
			this.ErrorMessage.TabIndex = 13;
			this.ErrorMessage.Text = "エラーメッセージ";
			// 
			// Known但し書きText
			// 
			this.Known但し書きText.AutoSize = true;
			this.Known但し書きText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
			this.Known但し書きText.ForeColor = System.Drawing.Color.White;
			this.Known但し書きText.Location = new System.Drawing.Point(12, 361);
			this.Known但し書きText.Name = "Known但し書きText";
			this.Known但し書きText.Size = new System.Drawing.Size(100, 20);
			this.Known但し書きText.TabIndex = 12;
			this.Known但し書きText.Text = "既知の但し書き";
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(644, 421);
			this.Controls.Add(this.Known但し書きText);
			this.Controls.Add(this.ErrorMessage);
			this.Controls.Add(this.日付Preview);
			this.Controls.Add(this.金額Preview);
			this.Controls.Add(this.但し書き候補);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.但し書き);
			this.Controls.Add(this.支払先候補);
			this.Controls.Add(this.支払先);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.金額);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.日付);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimizeBox = false;
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "領収書入力";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox 日付;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox 金額;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox 支払先;
		private System.Windows.Forms.ListBox 支払先候補;
		private System.Windows.Forms.TextBox 但し書き;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox 但し書き候補;
		private System.Windows.Forms.Label 金額Preview;
		private System.Windows.Forms.Label 日付Preview;
		private System.Windows.Forms.Timer MainTimer;
		private System.Windows.Forms.Label ErrorMessage;
		private System.Windows.Forms.Label Known但し書きText;
	}
}

