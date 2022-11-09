namespace Charlotte
{
	partial class ProcessingDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessingDlg));
			this.MainTimer = new System.Windows.Forms.Timer(this.components);
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.StartBtn = new System.Windows.Forms.Button();
			this.BlockAltF4Message = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// MainTimer
			// 
			this.MainTimer.Enabled = true;
			this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// ProgressBar
			// 
			this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProgressBar.Location = new System.Drawing.Point(12, 99);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(560, 50);
			this.ProgressBar.TabIndex = 2;
			// 
			// StartBtn
			// 
			this.StartBtn.Location = new System.Drawing.Point(12, 12);
			this.StartBtn.Name = "StartBtn";
			this.StartBtn.Size = new System.Drawing.Size(200, 50);
			this.StartBtn.TabIndex = 0;
			this.StartBtn.Text = "Start";
			this.StartBtn.UseVisualStyleBackColor = true;
			this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
			// 
			// BlockAltF4Message
			// 
			this.BlockAltF4Message.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BlockAltF4Message.AutoSize = true;
			this.BlockAltF4Message.BackColor = System.Drawing.Color.Blue;
			this.BlockAltF4Message.ForeColor = System.Drawing.Color.White;
			this.BlockAltF4Message.Location = new System.Drawing.Point(405, 9);
			this.BlockAltF4Message.Name = "BlockAltF4Message";
			this.BlockAltF4Message.Size = new System.Drawing.Size(167, 20);
			this.BlockAltF4Message.TabIndex = 1;
			this.BlockAltF4Message.Text = "Blocking [X] and ALT+F4";
			this.BlockAltF4Message.Visible = false;
			// 
			// ProcessingDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 161);
			this.Controls.Add(this.BlockAltF4Message);
			this.Controls.Add(this.StartBtn);
			this.Controls.Add(this.ProgressBar);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "ProcessingDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Processing Dialog";
			this.Load += new System.EventHandler(this.ProcessingDlg_Load);
			this.Shown += new System.EventHandler(this.ProcessingDlg_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer MainTimer;
		private System.Windows.Forms.ProgressBar ProgressBar;
		private System.Windows.Forms.Button StartBtn;
		private System.Windows.Forms.Label BlockAltF4Message;
	}
}