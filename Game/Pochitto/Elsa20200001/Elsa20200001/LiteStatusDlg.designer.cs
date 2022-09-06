namespace Charlotte
{
	partial class LiteStatusDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiteStatusDlg));
			this.StatusMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// StatusMessage
			// 
			this.StatusMessage.AutoSize = true;
			this.StatusMessage.Location = new System.Drawing.Point(11, 9);
			this.StatusMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StatusMessage.Name = "StatusMessage";
			this.StatusMessage.Size = new System.Drawing.Size(23, 12);
			this.StatusMessage.TabIndex = 0;
			this.StatusMessage.Text = "---";
			// 
			// LiteStatusDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.StatusMessage);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(-400, -400);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LiteStatusDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Elsa20200001 - LiteStatusDlg";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LiteStatusDlg_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LiteStatusDlg_FormClosed);
			this.Load += new System.EventHandler(this.LiteStatusDlg_Load);
			this.Shown += new System.EventHandler(this.LiteStatusDlg_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label StatusMessage;
	}
}