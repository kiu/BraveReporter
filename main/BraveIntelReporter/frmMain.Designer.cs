﻿namespace BraveIntelReporter
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReported = new System.Windows.Forms.Label();
            this.lblFailed = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lblMonitoringFiles = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuViewMap = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOutputMinimal = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOutputVerbose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetEveToBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbIntel = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Reported: ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Failed: ";
            // 
            // lblReported
            // 
            this.lblReported.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblReported.AutoSize = true;
            this.lblReported.Location = new System.Drawing.Point(75, 141);
            this.lblReported.Name = "lblReported";
            this.lblReported.Size = new System.Drawing.Size(13, 13);
            this.lblReported.TabIndex = 3;
            this.lblReported.Text = "0";
            // 
            // lblFailed
            // 
            this.lblFailed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFailed.AutoSize = true;
            this.lblFailed.Location = new System.Drawing.Point(75, 166);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(13, 13);
            this.lblFailed.TabIndex = 4;
            this.lblFailed.Text = "0";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "BraveReporter is minimized (still reporting)";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Monitoring Files: ";
            // 
            // lblMonitoringFiles
            // 
            this.lblMonitoringFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMonitoringFiles.AutoSize = true;
            this.lblMonitoringFiles.Location = new System.Drawing.Point(268, 141);
            this.lblMonitoringFiles.Name = "lblMonitoringFiles";
            this.lblMonitoringFiles.Size = new System.Drawing.Size(10, 13);
            this.lblMonitoringFiles.TabIndex = 6;
            this.lblMonitoringFiles.Text = "-";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewMap,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(668, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuViewMap
            // 
            this.mnuViewMap.Name = "mnuViewMap";
            this.mnuViewMap.Size = new System.Drawing.Size(71, 20);
            this.mnuViewMap.Text = "View Map";
            this.mnuViewMap.Click += new System.EventHandler(this.mnuViewMap_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputToolStripMenuItem,
            this.mnuSetEveToBackground,
            this.optionsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOutputMinimal,
            this.mnuOutputVerbose});
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.outputToolStripMenuItem.Text = "Output";
            // 
            // mnuOutputMinimal
            // 
            this.mnuOutputMinimal.Name = "mnuOutputMinimal";
            this.mnuOutputMinimal.Size = new System.Drawing.Size(118, 22);
            this.mnuOutputMinimal.Text = "Minimal";
            this.mnuOutputMinimal.Click += new System.EventHandler(this.mnuOutputMinimal_Click);
            // 
            // mnuOutputVerbose
            // 
            this.mnuOutputVerbose.Checked = true;
            this.mnuOutputVerbose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuOutputVerbose.Name = "mnuOutputVerbose";
            this.mnuOutputVerbose.Size = new System.Drawing.Size(118, 22);
            this.mnuOutputVerbose.Text = "Verbose";
            this.mnuOutputVerbose.Click += new System.EventHandler(this.mnuOutputVerbose_Click);
            // 
            // mnuSetEveToBackground
            // 
            this.mnuSetEveToBackground.Name = "mnuSetEveToBackground";
            this.mnuSetEveToBackground.Size = new System.Drawing.Size(202, 22);
            this.mnuSetEveToBackground.Text = "Keep EVE in Background";
            this.mnuSetEveToBackground.Click += new System.EventHandler(this.mnuSetEveToBackground_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // rtbIntel
            // 
            this.rtbIntel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbIntel.BackColor = System.Drawing.SystemColors.Control;
            this.rtbIntel.Location = new System.Drawing.Point(12, 27);
            this.rtbIntel.Name = "rtbIntel";
            this.rtbIntel.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbIntel.Size = new System.Drawing.Size(644, 111);
            this.rtbIntel.TabIndex = 8;
            this.rtbIntel.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 187);
            this.Controls.Add(this.rtbIntel);
            this.Controls.Add(this.lblMonitoringFiles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFailed);
            this.Controls.Add(this.lblReported);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Brave Intel Reporter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReported;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMonitoringFiles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuViewMap;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOutputMinimal;
        private System.Windows.Forms.ToolStripMenuItem mnuOutputVerbose;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuSetEveToBackground;
        private System.Windows.Forms.RichTextBox rtbIntel;
    }
}

