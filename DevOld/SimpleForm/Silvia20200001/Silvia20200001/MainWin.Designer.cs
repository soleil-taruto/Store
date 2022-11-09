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
			this.MainTimer = new System.Windows.Forms.Timer(this.components);
			this.Counter = new System.Windows.Forms.Label();
			this.PauseBtn = new System.Windows.Forms.Button();
			this.CloseBtn = new System.Windows.Forms.Button();
			this.FiveSecBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// MainTimer
			// 
			this.MainTimer.Enabled = true;
			this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// Counter
			// 
			this.Counter.AutoSize = true;
			this.Counter.Location = new System.Drawing.Point(12, 9);
			this.Counter.Name = "Counter";
			this.Counter.Size = new System.Drawing.Size(17, 20);
			this.Counter.TabIndex = 0;
			this.Counter.Text = "0";
			// 
			// PauseBtn
			// 
			this.PauseBtn.Location = new System.Drawing.Point(12, 32);
			this.PauseBtn.Name = "PauseBtn";
			this.PauseBtn.Size = new System.Drawing.Size(200, 100);
			this.PauseBtn.TabIndex = 1;
			this.PauseBtn.Text = "Pause";
			this.PauseBtn.UseVisualStyleBackColor = true;
			this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
			// 
			// CloseBtn
			// 
			this.CloseBtn.Location = new System.Drawing.Point(12, 138);
			this.CloseBtn.Name = "CloseBtn";
			this.CloseBtn.Size = new System.Drawing.Size(100, 50);
			this.CloseBtn.TabIndex = 2;
			this.CloseBtn.Text = "Close";
			this.CloseBtn.UseVisualStyleBackColor = true;
			this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
			// 
			// FiveSecBtn
			// 
			this.FiveSecBtn.Location = new System.Drawing.Point(118, 138);
			this.FiveSecBtn.Name = "FiveSecBtn";
			this.FiveSecBtn.Size = new System.Drawing.Size(50, 50);
			this.FiveSecBtn.TabIndex = 3;
			this.FiveSecBtn.Text = "5s";
			this.FiveSecBtn.UseVisualStyleBackColor = true;
			this.FiveSecBtn.Click += new System.EventHandler(this.FiveSecBtn_Click);
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.FiveSecBtn);
			this.Controls.Add(this.CloseBtn);
			this.Controls.Add(this.PauseBtn);
			this.Controls.Add(this.Counter);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Silvia20200001";
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer MainTimer;
		private System.Windows.Forms.Label Counter;
		private System.Windows.Forms.Button PauseBtn;
		private System.Windows.Forms.Button CloseBtn;
		private System.Windows.Forms.Button FiveSecBtn;

	}
}

