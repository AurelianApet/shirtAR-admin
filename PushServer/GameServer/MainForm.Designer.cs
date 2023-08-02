namespace ArGateway
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuAction = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuActionLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuActionStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuActionStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuActionClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSettingServ = new System.Windows.Forms.ToolStripMenuItem();
            this.chkViewMode = new System.Windows.Forms.CheckBox();
            this.listLog = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAction,
            this.mnuSetting});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // mnuAction
            // 
            this.mnuAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuActionLogin,
            this.mnuActionStart,
            this.mnuActionStop,
            this.mnuActionClose});
            this.mnuAction.Name = "mnuAction";
            resources.ApplyResources(this.mnuAction, "mnuAction");
            // 
            // mnuActionLogin
            // 
            this.mnuActionLogin.Name = "mnuActionLogin";
            resources.ApplyResources(this.mnuActionLogin, "mnuActionLogin");
            this.mnuActionLogin.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.mnuActionLogin.Click += new System.EventHandler(this.mnuActionLogin_Click);
            // 
            // mnuActionStart
            // 
            this.mnuActionStart.Name = "mnuActionStart";
            resources.ApplyResources(this.mnuActionStart, "mnuActionStart");
            this.mnuActionStart.Click += new System.EventHandler(this.mnuActionStart_Click);
            // 
            // mnuActionStop
            // 
            this.mnuActionStop.Name = "mnuActionStop";
            resources.ApplyResources(this.mnuActionStop, "mnuActionStop");
            this.mnuActionStop.Click += new System.EventHandler(this.mnuActionStop_Click);
            // 
            // mnuActionClose
            // 
            this.mnuActionClose.Name = "mnuActionClose";
            resources.ApplyResources(this.mnuActionClose, "mnuActionClose");
            this.mnuActionClose.Click += new System.EventHandler(this.mnuActionClose_Click);
            // 
            // mnuSetting
            // 
            this.mnuSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettingServ});
            this.mnuSetting.Name = "mnuSetting";
            resources.ApplyResources(this.mnuSetting, "mnuSetting");
            // 
            // mnuSettingServ
            // 
            this.mnuSettingServ.Name = "mnuSettingServ";
            resources.ApplyResources(this.mnuSettingServ, "mnuSettingServ");
            this.mnuSettingServ.Click += new System.EventHandler(this.mnuSettingServ_Click);
            // 
            // chkViewMode
            // 
            resources.ApplyResources(this.chkViewMode, "chkViewMode");
            this.chkViewMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkViewMode.Name = "chkViewMode";
            this.chkViewMode.UseVisualStyleBackColor = true;
            // 
            // listLog
            // 
            resources.ApplyResources(this.listLog, "listLog");
            this.listLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listLog.FormattingEnabled = true;
            this.listLog.Name = "listLog";
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.chkViewMode);
            this.Controls.Add(this.menuStrip1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuAction;
        private System.Windows.Forms.ToolStripMenuItem mnuActionLogin;
        private System.Windows.Forms.ToolStripMenuItem mnuActionStart;
        private System.Windows.Forms.ToolStripMenuItem mnuActionStop;
        private System.Windows.Forms.ToolStripMenuItem mnuActionClose;
        private System.Windows.Forms.ToolStripMenuItem mnuSetting;
        private System.Windows.Forms.CheckBox chkViewMode;
        private System.Windows.Forms.ToolStripMenuItem mnuSettingServ;
        private System.Windows.Forms.ListBox listLog;
    }
}

