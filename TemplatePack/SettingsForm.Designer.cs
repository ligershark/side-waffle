namespace TemplatePack
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.remoteSourceListView = new System.Windows.Forms.ListView();
            this.enabledColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UrlColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.branchColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sourceBranchTextBox = new System.Windows.Forms.TextBox();
            this.sourceUrlTextBox = new System.Windows.Forms.TextBox();
            this.sourceNameTextBox = new System.Windows.Forms.TextBox();
            this.newSourceBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rebuildTemplatesBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.onceADayCheckbox = new System.Windows.Forms.CheckBox();
            this.onceAWeekCheckbox = new System.Windows.Forms.CheckBox();
            this.onceAMonthCheckbox = new System.Windows.Forms.CheckBox();
            this.neverCheckBox = new System.Windows.Forms.CheckBox();
            this.OkBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.LoadingImage = new System.Windows.Forms.PictureBox();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.editBtn = new System.Windows.Forms.Button();
            this.applyBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remote Sources";
            // 
            // remoteSourceListView
            // 
            this.remoteSourceListView.CheckBoxes = true;
            this.remoteSourceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.enabledColumn,
            this.nameColumn,
            this.UrlColumn,
            this.branchColumn});
            this.remoteSourceListView.FullRowSelect = true;
            this.remoteSourceListView.Location = new System.Drawing.Point(29, 48);
            this.remoteSourceListView.MultiSelect = false;
            this.remoteSourceListView.Name = "remoteSourceListView";
            this.remoteSourceListView.Size = new System.Drawing.Size(659, 173);
            this.remoteSourceListView.TabIndex = 1;
            this.remoteSourceListView.UseCompatibleStateImageBehavior = false;
            this.remoteSourceListView.View = System.Windows.Forms.View.Details;
            // 
            // enabledColumn
            // 
            this.enabledColumn.Text = "Enabled";
            this.enabledColumn.Width = 58;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 128;
            // 
            // UrlColumn
            // 
            this.UrlColumn.Text = "URL";
            this.UrlColumn.Width = 307;
            // 
            // branchColumn
            // 
            this.branchColumn.Text = "Branch";
            this.branchColumn.Width = 121;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 200;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(29, 282);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 202;
            this.label3.Text = "Branch:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(29, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 16);
            this.label4.TabIndex = 201;
            this.label4.Text = "URL:";
            // 
            // sourceBranchTextBox
            // 
            this.sourceBranchTextBox.Enabled = false;
            this.sourceBranchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceBranchTextBox.Location = new System.Drawing.Point(88, 282);
            this.sourceBranchTextBox.Name = "sourceBranchTextBox";
            this.sourceBranchTextBox.Size = new System.Drawing.Size(411, 22);
            this.sourceBranchTextBox.TabIndex = 4;
            this.sourceBranchTextBox.Text = "origin/master";
            this.sourceBranchTextBox.Validated += new System.EventHandler(this.BranchTextbox_Validated);
            // 
            // sourceUrlTextBox
            // 
            this.sourceUrlTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceUrlTextBox.Location = new System.Drawing.Point(88, 252);
            this.sourceUrlTextBox.Name = "sourceUrlTextBox";
            this.sourceUrlTextBox.Size = new System.Drawing.Size(411, 22);
            this.sourceUrlTextBox.TabIndex = 3;
            this.sourceUrlTextBox.TextChanged += new System.EventHandler(this.SourceURL_TextChanged);
            // 
            // sourceNameTextBox
            // 
            this.sourceNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceNameTextBox.Location = new System.Drawing.Point(88, 224);
            this.sourceNameTextBox.Name = "sourceNameTextBox";
            this.sourceNameTextBox.Size = new System.Drawing.Size(411, 22);
            this.sourceNameTextBox.TabIndex = 2;
            // 
            // newSourceBtn
            // 
            this.newSourceBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newSourceBtn.Location = new System.Drawing.Point(596, 227);
            this.newSourceBtn.Name = "newSourceBtn";
            this.newSourceBtn.Size = new System.Drawing.Size(92, 23);
            this.newSourceBtn.TabIndex = 5;
            this.newSourceBtn.Text = "New Source";
            this.newSourceBtn.UseVisualStyleBackColor = true;
            this.newSourceBtn.Click += new System.EventHandler(this.newSourceBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(26, 329);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 16);
            this.label5.TabIndex = 203;
            this.label5.Text = "Rebuild Templates";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(26, 351);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(500, 16);
            this.label6.TabIndex = 204;
            this.label6.Text = "To rebuild all templates (get latest from source and rebuild all) use the button " +
    "below:";
            // 
            // rebuildTemplatesBtn
            // 
            this.rebuildTemplatesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rebuildTemplatesBtn.Location = new System.Drawing.Point(32, 370);
            this.rebuildTemplatesBtn.Name = "rebuildTemplatesBtn";
            this.rebuildTemplatesBtn.Size = new System.Drawing.Size(134, 25);
            this.rebuildTemplatesBtn.TabIndex = 11;
            this.rebuildTemplatesBtn.Text = "Rebuild Templates";
            this.rebuildTemplatesBtn.UseVisualStyleBackColor = true;
            this.rebuildTemplatesBtn.Click += new System.EventHandler(this.rebuildTemplatesBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 411);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 16);
            this.label7.TabIndex = 206;
            this.label7.Text = "Configure Schedule";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(26, 432);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(350, 16);
            this.label8.TabIndex = 207;
            this.label8.Text = "How often should SideWaffle check for update templates?";
            // 
            // onceADayCheckbox
            // 
            this.onceADayCheckbox.AutoSize = true;
            this.onceADayCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onceADayCheckbox.Location = new System.Drawing.Point(29, 451);
            this.onceADayCheckbox.Name = "onceADayCheckbox";
            this.onceADayCheckbox.Size = new System.Drawing.Size(99, 20);
            this.onceADayCheckbox.TabIndex = 14;
            this.onceADayCheckbox.Text = "Once A Day";
            this.onceADayCheckbox.UseVisualStyleBackColor = true;
            // 
            // onceAWeekCheckbox
            // 
            this.onceAWeekCheckbox.AutoSize = true;
            this.onceAWeekCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onceAWeekCheckbox.Location = new System.Drawing.Point(134, 451);
            this.onceAWeekCheckbox.Name = "onceAWeekCheckbox";
            this.onceAWeekCheckbox.Size = new System.Drawing.Size(110, 20);
            this.onceAWeekCheckbox.TabIndex = 15;
            this.onceAWeekCheckbox.Text = "Once A Week";
            this.onceAWeekCheckbox.UseVisualStyleBackColor = true;
            // 
            // onceAMonthCheckbox
            // 
            this.onceAMonthCheckbox.AutoSize = true;
            this.onceAMonthCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onceAMonthCheckbox.Location = new System.Drawing.Point(250, 451);
            this.onceAMonthCheckbox.Name = "onceAMonthCheckbox";
            this.onceAMonthCheckbox.Size = new System.Drawing.Size(110, 20);
            this.onceAMonthCheckbox.TabIndex = 16;
            this.onceAMonthCheckbox.Text = "Once A Month";
            this.onceAMonthCheckbox.UseVisualStyleBackColor = true;
            // 
            // neverCheckBox
            // 
            this.neverCheckBox.AutoSize = true;
            this.neverCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.neverCheckBox.Location = new System.Drawing.Point(366, 451);
            this.neverCheckBox.Name = "neverCheckBox";
            this.neverCheckBox.Size = new System.Drawing.Size(64, 20);
            this.neverCheckBox.TabIndex = 17;
            this.neverCheckBox.Text = "Never";
            this.neverCheckBox.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(532, 492);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 18;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(613, 492);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 19;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // LoadingImage
            // 
            this.LoadingImage.Image = ((System.Drawing.Image)(resources.GetObject("LoadingImage.Image")));
            this.LoadingImage.Location = new System.Drawing.Point(172, 374);
            this.LoadingImage.Name = "LoadingImage";
            this.LoadingImage.Size = new System.Drawing.Size(20, 17);
            this.LoadingImage.TabIndex = 20;
            this.LoadingImage.TabStop = false;
            this.LoadingImage.Visible = false;
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.AutoSize = true;
            this.LoadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadingLabel.Location = new System.Drawing.Point(195, 374);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(198, 16);
            this.LoadingLabel.TabIndex = 205;
            this.LoadingLabel.Text = "Please wait building templates...";
            this.LoadingLabel.Visible = false;
            // 
            // editBtn
            // 
            this.editBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editBtn.Location = new System.Drawing.Point(515, 227);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(75, 23);
            this.editBtn.TabIndex = 6;
            this.editBtn.Text = "Edit";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new System.EventHandler(this.editBtn_click);
            // 
            // applyBtn
            // 
            this.applyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyBtn.Location = new System.Drawing.Point(515, 256);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(110, 25);
            this.applyBtn.TabIndex = 7;
            this.applyBtn.Text = "Apply Changes";
            this.applyBtn.UseVisualStyleBackColor = true;
            this.applyBtn.Visible = false;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 527);
            this.Controls.Add(this.applyBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.LoadingLabel);
            this.Controls.Add(this.LoadingImage);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.neverCheckBox);
            this.Controls.Add(this.onceAMonthCheckbox);
            this.Controls.Add(this.onceAWeekCheckbox);
            this.Controls.Add(this.onceADayCheckbox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.rebuildTemplatesBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.newSourceBtn);
            this.Controls.Add(this.sourceNameTextBox);
            this.Controls.Add(this.sourceUrlTextBox);
            this.Controls.Add(this.sourceBranchTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.remoteSourceListView);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "SideWaffle Remote Template Settings";
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView remoteSourceListView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox sourceBranchTextBox;
        private System.Windows.Forms.TextBox sourceUrlTextBox;
        private System.Windows.Forms.TextBox sourceNameTextBox;
        private System.Windows.Forms.Button newSourceBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button rebuildTemplatesBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox onceADayCheckbox;
        private System.Windows.Forms.CheckBox onceAWeekCheckbox;
        private System.Windows.Forms.CheckBox onceAMonthCheckbox;
        private System.Windows.Forms.CheckBox neverCheckBox;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ColumnHeader enabledColumn;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader UrlColumn;
        private System.Windows.Forms.ColumnHeader branchColumn;
        private System.Windows.Forms.PictureBox LoadingImage;
        private System.Windows.Forms.Label LoadingLabel;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Button applyBtn;
    }
}