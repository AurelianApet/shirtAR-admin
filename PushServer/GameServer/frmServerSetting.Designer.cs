namespace ArGateway
{
    partial class frmServerSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServerSetting));
            this.textDBPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textDBPwd = new System.Windows.Forms.TextBox();
            this.textDBID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.numspinIPD = new System.Windows.Forms.NumericUpDown();
            this.numspinIPC = new System.Windows.Forms.NumericUpDown();
            this.numspinIPB = new System.Windows.Forms.NumericUpDown();
            this.numspinIPA = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textDBName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPA)).BeginInit();
            this.SuspendLayout();
            // 
            // textDBPort
            // 
            this.textDBPort.Location = new System.Drawing.Point(140, 157);
            this.textDBPort.MaxLength = 10;
            this.textDBPort.Name = "textDBPort";
            this.textDBPort.Size = new System.Drawing.Size(100, 20);
            this.textDBPort.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(46, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 56;
            this.label8.Text = "디비접속포트:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textDBPwd
            // 
            this.textDBPwd.Location = new System.Drawing.Point(139, 131);
            this.textDBPwd.MaxLength = 20;
            this.textDBPwd.Name = "textDBPwd";
            this.textDBPwd.PasswordChar = '*';
            this.textDBPwd.Size = new System.Drawing.Size(100, 20);
            this.textDBPwd.TabIndex = 7;
            // 
            // textDBID
            // 
            this.textDBID.Location = new System.Drawing.Point(140, 104);
            this.textDBID.MaxLength = 20;
            this.textDBID.Name = "textDBID";
            this.textDBID.Size = new System.Drawing.Size(100, 20);
            this.textDBID.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(68, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "디비암호:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "디비아이디:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(182, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "취소(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(60, 193);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "확인(&O)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // numspinIPD
            // 
            this.numspinIPD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numspinIPD.Location = new System.Drawing.Point(240, 44);
            this.numspinIPD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numspinIPD.Name = "numspinIPD";
            this.numspinIPD.Size = new System.Drawing.Size(44, 20);
            this.numspinIPD.TabIndex = 3;
            this.numspinIPD.Enter += new System.EventHandler(this.numspinipD_Enter);
            // 
            // numspinIPC
            // 
            this.numspinIPC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numspinIPC.Location = new System.Drawing.Point(195, 44);
            this.numspinIPC.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numspinIPC.Name = "numspinIPC";
            this.numspinIPC.Size = new System.Drawing.Size(44, 20);
            this.numspinIPC.TabIndex = 2;
            this.numspinIPC.Enter += new System.EventHandler(this.numspinipC_Enter);
            // 
            // numspinIPB
            // 
            this.numspinIPB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numspinIPB.Location = new System.Drawing.Point(151, 44);
            this.numspinIPB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numspinIPB.Name = "numspinIPB";
            this.numspinIPB.Size = new System.Drawing.Size(44, 20);
            this.numspinIPB.TabIndex = 1;
            this.numspinIPB.Enter += new System.EventHandler(this.numspinipB_Enter);
            // 
            // numspinIPA
            // 
            this.numspinIPA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numspinIPA.Location = new System.Drawing.Point(106, 44);
            this.numspinIPA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numspinIPA.Name = "numspinIPA";
            this.numspinIPA.Size = new System.Drawing.Size(44, 20);
            this.numspinIPA.TabIndex = 0;
            this.numspinIPA.Enter += new System.EventHandler(this.numspinipA_Enter);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(36, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(252, 18);
            this.label6.TabIndex = 49;
            this.label6.Text = "☞ 서버가 접속할 디비접속정보를 설정합니다.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "접속IP주소:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(80, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 52;
            this.label9.Text = "디비명:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // textDBName
            // 
            this.textDBName.Location = new System.Drawing.Point(140, 78);
            this.textDBName.MaxLength = 20;
            this.textDBName.Name = "textDBName";
            this.textDBName.Size = new System.Drawing.Size(100, 20);
            this.textDBName.TabIndex = 5;
            this.textDBName.Enter += new System.EventHandler(this.textDBName_Enter);
            // 
            // frmServerSetting
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(325, 226);
            this.Controls.Add(this.textDBPort);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textDBPwd);
            this.Controls.Add(this.textDBID);
            this.Controls.Add(this.textDBName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.numspinIPD);
            this.Controls.Add(this.numspinIPC);
            this.Controls.Add(this.numspinIPB);
            this.Controls.Add(this.numspinIPA);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmServerSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmServerSetting";
            this.Load += new System.EventHandler(this.frmServerSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspinIPA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textDBPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textDBPwd;
        private System.Windows.Forms.TextBox textDBID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown numspinIPD;
        private System.Windows.Forms.NumericUpDown numspinIPC;
        private System.Windows.Forms.NumericUpDown numspinIPB;
        private System.Windows.Forms.NumericUpDown numspinIPA;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textDBName;

    }
}