namespace Charlotte.LevelEditors
{
	partial class LevelEditorDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelEditorDlg));
			this.GroupTile = new System.Windows.Forms.GroupBox();
			this.TileMember_R = new System.Windows.Forms.ComboBox();
			this.TileMember_L = new System.Windows.Forms.ComboBox();
			this.TileGroup_R = new System.Windows.Forms.ComboBox();
			this.TileGroup_L = new System.Windows.Forms.ComboBox();
			this.GroupEnemy = new System.Windows.Forms.GroupBox();
			this.EnemyMember = new System.Windows.Forms.ComboBox();
			this.EnemyGroup = new System.Windows.Forms.ComboBox();
			this.ShowTile = new System.Windows.Forms.CheckBox();
			this.ShowEnemy = new System.Windows.Forms.CheckBox();
			this.TileEnemySw = new System.Windows.Forms.Button();
			this.GroupTile.SuspendLayout();
			this.GroupEnemy.SuspendLayout();
			this.SuspendLayout();
			// 
			// GroupTile
			// 
			this.GroupTile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupTile.Controls.Add(this.TileMember_R);
			this.GroupTile.Controls.Add(this.TileMember_L);
			this.GroupTile.Controls.Add(this.TileGroup_R);
			this.GroupTile.Controls.Add(this.TileGroup_L);
			this.GroupTile.Location = new System.Drawing.Point(12, 12);
			this.GroupTile.Name = "GroupTile";
			this.GroupTile.Size = new System.Drawing.Size(360, 200);
			this.GroupTile.TabIndex = 2;
			this.GroupTile.TabStop = false;
			this.GroupTile.Text = "タイル (左ボタン / 右ボタン)";
			this.GroupTile.Enter += new System.EventHandler(this.GroupTile_Enter);
			// 
			// TileMember_R
			// 
			this.TileMember_R.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileMember_R.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TileMember_R.FormattingEnabled = true;
			this.TileMember_R.Location = new System.Drawing.Point(6, 144);
			this.TileMember_R.Name = "TileMember_R";
			this.TileMember_R.Size = new System.Drawing.Size(348, 28);
			this.TileMember_R.TabIndex = 3;
			this.TileMember_R.SelectedIndexChanged += new System.EventHandler(this.TileMember_R_SelectedIndexChanged);
			this.TileMember_R.Click += new System.EventHandler(this.TileClick);
			// 
			// TileMember_L
			// 
			this.TileMember_L.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileMember_L.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TileMember_L.FormattingEnabled = true;
			this.TileMember_L.Location = new System.Drawing.Point(6, 60);
			this.TileMember_L.Name = "TileMember_L";
			this.TileMember_L.Size = new System.Drawing.Size(348, 28);
			this.TileMember_L.TabIndex = 2;
			this.TileMember_L.SelectedIndexChanged += new System.EventHandler(this.TileMember_L_SelectedIndexChanged);
			this.TileMember_L.Click += new System.EventHandler(this.TileClick);
			// 
			// TileGroup_R
			// 
			this.TileGroup_R.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileGroup_R.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TileGroup_R.FormattingEnabled = true;
			this.TileGroup_R.Location = new System.Drawing.Point(6, 110);
			this.TileGroup_R.Name = "TileGroup_R";
			this.TileGroup_R.Size = new System.Drawing.Size(348, 28);
			this.TileGroup_R.TabIndex = 1;
			this.TileGroup_R.SelectedIndexChanged += new System.EventHandler(this.TileGroup_R_SelectedIndexChanged);
			this.TileGroup_R.Click += new System.EventHandler(this.TileClick);
			// 
			// TileGroup_L
			// 
			this.TileGroup_L.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileGroup_L.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TileGroup_L.FormattingEnabled = true;
			this.TileGroup_L.Location = new System.Drawing.Point(6, 26);
			this.TileGroup_L.Name = "TileGroup_L";
			this.TileGroup_L.Size = new System.Drawing.Size(348, 28);
			this.TileGroup_L.TabIndex = 0;
			this.TileGroup_L.SelectedIndexChanged += new System.EventHandler(this.TileGroup_L_SelectedIndexChanged);
			this.TileGroup_L.Click += new System.EventHandler(this.TileClick);
			// 
			// GroupEnemy
			// 
			this.GroupEnemy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupEnemy.Controls.Add(this.EnemyMember);
			this.GroupEnemy.Controls.Add(this.EnemyGroup);
			this.GroupEnemy.Location = new System.Drawing.Point(12, 218);
			this.GroupEnemy.Name = "GroupEnemy";
			this.GroupEnemy.Size = new System.Drawing.Size(360, 120);
			this.GroupEnemy.TabIndex = 3;
			this.GroupEnemy.TabStop = false;
			this.GroupEnemy.Text = "敵 / イベントオブジェクト";
			this.GroupEnemy.Enter += new System.EventHandler(this.GroupEnemy_Enter);
			// 
			// EnemyMember
			// 
			this.EnemyMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EnemyMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnemyMember.FormattingEnabled = true;
			this.EnemyMember.Location = new System.Drawing.Point(6, 60);
			this.EnemyMember.Name = "EnemyMember";
			this.EnemyMember.Size = new System.Drawing.Size(348, 28);
			this.EnemyMember.TabIndex = 1;
			this.EnemyMember.SelectedIndexChanged += new System.EventHandler(this.EnemyMember_SelectedIndexChanged);
			this.EnemyMember.Click += new System.EventHandler(this.EnemyClick);
			// 
			// EnemyGroup
			// 
			this.EnemyGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EnemyGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnemyGroup.FormattingEnabled = true;
			this.EnemyGroup.Location = new System.Drawing.Point(6, 26);
			this.EnemyGroup.Name = "EnemyGroup";
			this.EnemyGroup.Size = new System.Drawing.Size(348, 28);
			this.EnemyGroup.TabIndex = 0;
			this.EnemyGroup.SelectedIndexChanged += new System.EventHandler(this.EnemyGroup_SelectedIndexChanged);
			this.EnemyGroup.Click += new System.EventHandler(this.EnemyClick);
			// 
			// ShowTile
			// 
			this.ShowTile.AutoSize = true;
			this.ShowTile.Checked = true;
			this.ShowTile.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowTile.Location = new System.Drawing.Point(18, 360);
			this.ShowTile.Name = "ShowTile";
			this.ShowTile.Size = new System.Drawing.Size(132, 24);
			this.ShowTile.TabIndex = 4;
			this.ShowTile.Text = "タイルを表示する";
			this.ShowTile.UseVisualStyleBackColor = true;
			this.ShowTile.CheckedChanged += new System.EventHandler(this.ShowTile_CheckedChanged);
			// 
			// ShowEnemy
			// 
			this.ShowEnemy.AutoSize = true;
			this.ShowEnemy.Checked = true;
			this.ShowEnemy.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowEnemy.Location = new System.Drawing.Point(18, 400);
			this.ShowEnemy.Name = "ShowEnemy";
			this.ShowEnemy.Size = new System.Drawing.Size(250, 24);
			this.ShowEnemy.TabIndex = 5;
			this.ShowEnemy.Text = "敵 / イベントオブジェクトを表示する";
			this.ShowEnemy.UseVisualStyleBackColor = true;
			this.ShowEnemy.CheckedChanged += new System.EventHandler(this.ShowEnemy_CheckedChanged);
			// 
			// TileEnemySw
			// 
			this.TileEnemySw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileEnemySw.Location = new System.Drawing.Point(12, 449);
			this.TileEnemySw.Name = "TileEnemySw";
			this.TileEnemySw.Size = new System.Drawing.Size(360, 50);
			this.TileEnemySw.TabIndex = 6;
			this.TileEnemySw.Text = "準備しています...";
			this.TileEnemySw.UseVisualStyleBackColor = true;
			this.TileEnemySw.Click += new System.EventHandler(this.TileEnemySw_Click);
			// 
			// LevelEditorDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 511);
			this.Controls.Add(this.TileEnemySw);
			this.Controls.Add(this.ShowEnemy);
			this.Controls.Add(this.ShowTile);
			this.Controls.Add(this.GroupEnemy);
			this.Controls.Add(this.GroupTile);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LevelEditorDlg";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Editor";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.LevelEditorDlg_Load);
			this.Shown += new System.EventHandler(this.LevelEditorDlg_Shown);
			this.GroupTile.ResumeLayout(false);
			this.GroupEnemy.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GroupTile;
		private System.Windows.Forms.ComboBox TileGroup_L;
		private System.Windows.Forms.GroupBox GroupEnemy;
		private System.Windows.Forms.ComboBox EnemyGroup;
		private System.Windows.Forms.CheckBox ShowTile;
		private System.Windows.Forms.CheckBox ShowEnemy;
		private System.Windows.Forms.Button TileEnemySw;
		private System.Windows.Forms.ComboBox TileGroup_R;
		private System.Windows.Forms.ComboBox TileMember_R;
		private System.Windows.Forms.ComboBox TileMember_L;
		private System.Windows.Forms.ComboBox EnemyMember;
	}
}