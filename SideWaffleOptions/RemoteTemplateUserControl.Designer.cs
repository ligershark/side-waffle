namespace LigerShark.SideWaffleOptions
{
    partial class RemoteTemplateUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.remoteSourcesLabel = new System.Windows.Forms.Label();
            this.sourceNameLabel = new System.Windows.Forms.Label();
            this.sourceUrlLabel = new System.Windows.Forms.Label();
            this.sourceBranchLabel = new System.Windows.Forms.Label();
            this.rebuildTemplatesLabel = new System.Windows.Forms.Label();
            this.rebuildTemplatesDescLabel = new System.Windows.Forms.Label();
            this.configureScheduleLabel = new System.Windows.Forms.Label();
            this.configureScheduleDescLabel = new System.Windows.Forms.Label();
            this.sourcesListBox = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.newSourceBtn = new System.Windows.Forms.Button();
            this.rebuildTemplatesBtn = new System.Windows.Forms.Button();
            this.scheduleOnceADayCheckBox = new System.Windows.Forms.CheckBox();
            this.scheduleOnceAWeekCheckBox = new System.Windows.Forms.CheckBox();
            this.scheduleOnceAMonthCheckBox = new System.Windows.Forms.CheckBox();
            this.scheduleNeverCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // remoteSourcesLabel
            // 
            this.remoteSourcesLabel.AutoSize = true;
            this.remoteSourcesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remoteSourcesLabel.Location = new System.Drawing.Point(24, 11);
            this.remoteSourcesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.remoteSourcesLabel.Name = "remoteSourcesLabel";
            this.remoteSourcesLabel.Size = new System.Drawing.Size(123, 16);
            this.remoteSourcesLabel.TabIndex = 0;
            this.remoteSourcesLabel.Text = "Remote Sources";
            // 
            // sourceNameLabel
            // 
            this.sourceNameLabel.AutoSize = true;
            this.sourceNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceNameLabel.Location = new System.Drawing.Point(31, 255);
            this.sourceNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sourceNameLabel.Name = "sourceNameLabel";
            this.sourceNameLabel.Size = new System.Drawing.Size(48, 16);
            this.sourceNameLabel.TabIndex = 1;
            this.sourceNameLabel.Text = "Name:";
            // 
            // sourceUrlLabel
            // 
            this.sourceUrlLabel.AutoSize = true;
            this.sourceUrlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceUrlLabel.Location = new System.Drawing.Point(31, 294);
            this.sourceUrlLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sourceUrlLabel.Name = "sourceUrlLabel";
            this.sourceUrlLabel.Size = new System.Drawing.Size(38, 16);
            this.sourceUrlLabel.TabIndex = 2;
            this.sourceUrlLabel.Text = "URL:";
            // 
            // sourceBranchLabel
            // 
            this.sourceBranchLabel.AutoSize = true;
            this.sourceBranchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceBranchLabel.Location = new System.Drawing.Point(31, 337);
            this.sourceBranchLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sourceBranchLabel.Name = "sourceBranchLabel";
            this.sourceBranchLabel.Size = new System.Drawing.Size(53, 16);
            this.sourceBranchLabel.TabIndex = 3;
            this.sourceBranchLabel.Text = "Branch:";
            // 
            // rebuildTemplatesLabel
            // 
            this.rebuildTemplatesLabel.AutoSize = true;
            this.rebuildTemplatesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rebuildTemplatesLabel.Location = new System.Drawing.Point(31, 391);
            this.rebuildTemplatesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rebuildTemplatesLabel.Name = "rebuildTemplatesLabel";
            this.rebuildTemplatesLabel.Size = new System.Drawing.Size(140, 16);
            this.rebuildTemplatesLabel.TabIndex = 4;
            this.rebuildTemplatesLabel.Text = "Rebuild Templates";
            // 
            // rebuildTemplatesDescLabel
            // 
            this.rebuildTemplatesDescLabel.AutoSize = true;
            this.rebuildTemplatesDescLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rebuildTemplatesDescLabel.Location = new System.Drawing.Point(31, 414);
            this.rebuildTemplatesDescLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rebuildTemplatesDescLabel.Name = "rebuildTemplatesDescLabel";
            this.rebuildTemplatesDescLabel.Size = new System.Drawing.Size(500, 16);
            this.rebuildTemplatesDescLabel.TabIndex = 5;
            this.rebuildTemplatesDescLabel.Text = "To rebuild all templates (get latest from source and rebuild all) use the button " +
    "below:";
            // 
            // configureScheduleLabel
            // 
            this.configureScheduleLabel.AutoSize = true;
            this.configureScheduleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configureScheduleLabel.Location = new System.Drawing.Point(32, 493);
            this.configureScheduleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.configureScheduleLabel.Name = "configureScheduleLabel";
            this.configureScheduleLabel.Size = new System.Drawing.Size(143, 16);
            this.configureScheduleLabel.TabIndex = 6;
            this.configureScheduleLabel.Text = "Configure Schedule";
            // 
            // configureScheduleDescLabel
            // 
            this.configureScheduleDescLabel.AutoSize = true;
            this.configureScheduleDescLabel.Location = new System.Drawing.Point(31, 517);
            this.configureScheduleDescLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.configureScheduleDescLabel.Name = "configureScheduleDescLabel";
            this.configureScheduleDescLabel.Size = new System.Drawing.Size(350, 16);
            this.configureScheduleDescLabel.TabIndex = 7;
            this.configureScheduleDescLabel.Text = "How often should SideWaffle check for update templates?";
            // 
            // sourcesListBox
            // 
            this.sourcesListBox.FormattingEnabled = true;
            this.sourcesListBox.ItemHeight = 16;
            this.sourcesListBox.Location = new System.Drawing.Point(28, 36);
            this.sourcesListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sourcesListBox.Name = "sourcesListBox";
            this.sourcesListBox.Size = new System.Drawing.Size(712, 196);
            this.sourcesListBox.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(109, 255);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(353, 22);
            this.textBox1.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(109, 294);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(353, 22);
            this.textBox2.TabIndex = 10;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(109, 337);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(353, 22);
            this.textBox3.TabIndex = 11;
            // 
            // newSourceBtn
            // 
            this.newSourceBtn.Location = new System.Drawing.Point(640, 252);
            this.newSourceBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.newSourceBtn.Name = "newSourceBtn";
            this.newSourceBtn.Size = new System.Drawing.Size(100, 28);
            this.newSourceBtn.TabIndex = 12;
            this.newSourceBtn.Text = "New Source";
            this.newSourceBtn.UseVisualStyleBackColor = true;
            // 
            // rebuildTemplatesBtn
            // 
            this.rebuildTemplatesBtn.Location = new System.Drawing.Point(35, 438);
            this.rebuildTemplatesBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rebuildTemplatesBtn.Name = "rebuildTemplatesBtn";
            this.rebuildTemplatesBtn.Size = new System.Drawing.Size(140, 28);
            this.rebuildTemplatesBtn.TabIndex = 13;
            this.rebuildTemplatesBtn.Text = "Rebuild Templates";
            this.rebuildTemplatesBtn.UseVisualStyleBackColor = true;
            // 
            // scheduleOnceADayCheckBox
            // 
            this.scheduleOnceADayCheckBox.AutoSize = true;
            this.scheduleOnceADayCheckBox.Location = new System.Drawing.Point(35, 541);
            this.scheduleOnceADayCheckBox.Name = "scheduleOnceADayCheckBox";
            this.scheduleOnceADayCheckBox.Size = new System.Drawing.Size(96, 20);
            this.scheduleOnceADayCheckBox.TabIndex = 14;
            this.scheduleOnceADayCheckBox.Text = "Once a day";
            this.scheduleOnceADayCheckBox.UseVisualStyleBackColor = true;
            // 
            // scheduleOnceAWeekCheckBox
            // 
            this.scheduleOnceAWeekCheckBox.AutoSize = true;
            this.scheduleOnceAWeekCheckBox.Location = new System.Drawing.Point(137, 541);
            this.scheduleOnceAWeekCheckBox.Name = "scheduleOnceAWeekCheckBox";
            this.scheduleOnceAWeekCheckBox.Size = new System.Drawing.Size(105, 20);
            this.scheduleOnceAWeekCheckBox.TabIndex = 15;
            this.scheduleOnceAWeekCheckBox.Text = "Once a week";
            this.scheduleOnceAWeekCheckBox.UseVisualStyleBackColor = true;
            // 
            // scheduleOnceAMonthCheckBox
            // 
            this.scheduleOnceAMonthCheckBox.AutoSize = true;
            this.scheduleOnceAMonthCheckBox.Location = new System.Drawing.Point(248, 541);
            this.scheduleOnceAMonthCheckBox.Name = "scheduleOnceAMonthCheckBox";
            this.scheduleOnceAMonthCheckBox.Size = new System.Drawing.Size(109, 20);
            this.scheduleOnceAMonthCheckBox.TabIndex = 16;
            this.scheduleOnceAMonthCheckBox.Text = "Once a month";
            this.scheduleOnceAMonthCheckBox.UseVisualStyleBackColor = true;
            // 
            // scheduleNeverCheckBox
            // 
            this.scheduleNeverCheckBox.AutoSize = true;
            this.scheduleNeverCheckBox.Location = new System.Drawing.Point(363, 541);
            this.scheduleNeverCheckBox.Name = "scheduleNeverCheckBox";
            this.scheduleNeverCheckBox.Size = new System.Drawing.Size(64, 20);
            this.scheduleNeverCheckBox.TabIndex = 17;
            this.scheduleNeverCheckBox.Text = "Never";
            this.scheduleNeverCheckBox.UseVisualStyleBackColor = true;
            // 
            // RemoteTemplateUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.scheduleNeverCheckBox);
            this.Controls.Add(this.scheduleOnceAMonthCheckBox);
            this.Controls.Add(this.scheduleOnceAWeekCheckBox);
            this.Controls.Add(this.scheduleOnceADayCheckBox);
            this.Controls.Add(this.rebuildTemplatesBtn);
            this.Controls.Add(this.newSourceBtn);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.sourcesListBox);
            this.Controls.Add(this.configureScheduleDescLabel);
            this.Controls.Add(this.configureScheduleLabel);
            this.Controls.Add(this.rebuildTemplatesDescLabel);
            this.Controls.Add(this.rebuildTemplatesLabel);
            this.Controls.Add(this.sourceBranchLabel);
            this.Controls.Add(this.sourceUrlLabel);
            this.Controls.Add(this.sourceNameLabel);
            this.Controls.Add(this.remoteSourcesLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RemoteTemplateUserControl";
            this.Size = new System.Drawing.Size(771, 581);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label remoteSourcesLabel;
        private System.Windows.Forms.Label sourceNameLabel;
        private System.Windows.Forms.Label sourceUrlLabel;
        private System.Windows.Forms.Label sourceBranchLabel;
        private System.Windows.Forms.Label rebuildTemplatesLabel;
        private System.Windows.Forms.Label rebuildTemplatesDescLabel;
        private System.Windows.Forms.Label configureScheduleLabel;
        private System.Windows.Forms.Label configureScheduleDescLabel;
        private System.Windows.Forms.ListBox sourcesListBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button newSourceBtn;
        private System.Windows.Forms.Button rebuildTemplatesBtn;
        private System.Windows.Forms.CheckBox scheduleOnceADayCheckBox;
        private System.Windows.Forms.CheckBox scheduleOnceAWeekCheckBox;
        private System.Windows.Forms.CheckBox scheduleOnceAMonthCheckBox;
        private System.Windows.Forms.CheckBox scheduleNeverCheckBox;
    }
}
