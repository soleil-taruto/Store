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
			this.タイルGroup = new System.Windows.Forms.GroupBox();
			this.TileMember = new System.Windows.Forms.ComboBox();
			this.TileGroup = new System.Windows.Forms.ComboBox();
			this.敵Group = new System.Windows.Forms.GroupBox();
			this.EnemyMember = new System.Windows.Forms.ComboBox();
			this.EnemyGroup = new System.Windows.Forms.ComboBox();
			this.ShowTile = new System.Windows.Forms.CheckBox();
			this.ShowEnemy = new System.Windows.Forms.CheckBox();
			this.TileEnemySw = new System.Windows.Forms.Button();
			this.タイルGroup.SuspendLayout();
			this.敵Group.SuspendLayout();
			this.SuspendLayout();
			// 
			// タイルGroup
			// 
			this.タイルGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.タイルGroup.Controls.Add(this.TileMember);
			this.タイルGroup.Controls.Add(this.TileGroup);
			this.タイルGroup.Location = new System.Drawing.Point(12, 12);
			this.タイルGroup.Name = "タイルGroup";
			this.タイルGroup.Size = new System.Drawing.Size(360, 120);
			this.タイルGroup.TabIndex = 0;
			this.タイルGroup.TabStop = false;
			this.タイルGroup.Text = "タイル";
			this.タイルGroup.Enter += new System.EventHandler(this.タイルGroup_Enter);
			// 
			// TileMember
			// 
			this.TileMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TileMember.FormattingEnabled = true;
			this.TileMember.Location = new System.Drawing.Point(6, 60);
			this.TileMember.Name = "TileMember";
			this.TileMember.Size = new System.Drawing.Size(348, 28);
			this.TileMember.TabIndex = 1;
			this.TileMember.SelectedIndexChanged += new System.EventHandler(this.TileMember_SelectedIndexChanged);
			this.TileMember.Click += new System.EventHandler(this.TileClick);
			// 
			// TileGroup
			// 
			this.TileGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TileGroup.FormattingEnabled = true;
			this.TileGroup.Location = new System.Drawing.Point(6, 26);
			this.TileGroup.Name = "TileGroup";
			this.TileGroup.Size = new System.Drawing.Size(348, 28);
			this.TileGroup.TabIndex = 0;
			this.TileGroup.SelectedIndexChanged += new System.EventHandler(this.TileGroup_SelectedIndexChanged);
			this.TileGroup.Click += new System.EventHandler(this.TileClick);
			// 
			// 敵Group
			// 
			this.敵Group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.敵Group.Controls.Add(this.EnemyMember);
			this.敵Group.Controls.Add(this.EnemyGroup);
			this.敵Group.Location = new System.Drawing.Point(12, 138);
			this.敵Group.Name = "敵Group";
			this.敵Group.Size = new System.Drawing.Size(360, 120);
			this.敵Group.TabIndex = 1;
			this.敵Group.TabStop = false;
			this.敵Group.Text = "敵 / イベントオブジェクト";
			this.敵Group.Enter += new System.EventHandler(this.敵Group_Enter);
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
			this.ShowTile.Location = new System.Drawing.Point(18, 284);
			this.ShowTile.Name = "ShowTile";
			this.ShowTile.Size = new System.Drawing.Size(132, 24);
			this.ShowTile.TabIndex = 2;
			this.ShowTile.Text = "タイルを表示する";
			this.ShowTile.UseVisualStyleBackColor = true;
			this.ShowTile.CheckedChanged += new System.EventHandler(this.ShowTile_CheckedChanged);
			// 
			// ShowEnemy
			// 
			this.ShowEnemy.AutoSize = true;
			this.ShowEnemy.Checked = true;
			this.ShowEnemy.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowEnemy.Location = new System.Drawing.Point(18, 314);
			this.ShowEnemy.Name = "ShowEnemy";
			this.ShowEnemy.Size = new System.Drawing.Size(250, 24);
			this.ShowEnemy.TabIndex = 3;
			this.ShowEnemy.Text = "敵 / イベントオブジェクトを表示する";
			this.ShowEnemy.UseVisualStyleBackColor = true;
			this.ShowEnemy.CheckedChanged += new System.EventHandler(this.ShowEnemy_CheckedChanged);
			// 
			// TileEnemySw
			// 
			this.TileEnemySw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TileEnemySw.Location = new System.Drawing.Point(12, 344);
			this.TileEnemySw.Name = "TileEnemySw";
			this.TileEnemySw.Size = new System.Drawing.Size(360, 55);
			this.TileEnemySw.TabIndex = 4;
			this.TileEnemySw.Text = "準備しています...";
			this.TileEnemySw.UseVisualStyleBackColor = true;
			this.TileEnemySw.Click += new System.EventHandler(this.TileEnemySw_Click);
			// 
			// LevelEditorDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 411);
			this.Controls.Add(this.TileEnemySw);
			this.Controls.Add(this.ShowEnemy);
			this.Controls.Add(this.ShowTile);
			this.Controls.Add(this.敵Group);
			this.Controls.Add(this.タイルGroup);
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
			this.タイルGroup.ResumeLayout(false);
			this.敵Group.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox タイルGroup;
		private System.Windows.Forms.ComboBox TileGroup;
		private System.Windows.Forms.GroupBox 敵Group;
		private System.Windows.Forms.ComboBox EnemyGroup;
		private System.Windows.Forms.CheckBox ShowTile;
		private System.Windows.Forms.CheckBox ShowEnemy;
		private System.Windows.Forms.Button TileEnemySw;
		private System.Windows.Forms.ComboBox TileMember;
		private System.Windows.Forms.ComboBox EnemyMember;
	}
}