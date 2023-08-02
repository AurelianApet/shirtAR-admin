using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace ArGateway
{
    public partial class frmServerSetting : Form
    {
        public frmServerSetting()
        {
            InitializeComponent();
        }

        private void frmServerSetting_Load(object sender, EventArgs e)
        {
            this.Text = "��������";

            numspinIPA.Minimum = 0;
            numspinIPA.Maximum = 255;
            numspinIPB.Minimum = 0;
            numspinIPB.Maximum = 255;
            numspinIPC.Minimum = 0;
            numspinIPC.Maximum = 255;
            numspinIPD.Minimum = 0;
            numspinIPD.Maximum = 255;
            cServerSettings.GetInstance().LoadSettings();
            byte[] ipbytes = cServerSettings.GetInstance().DBIP.GetAddressBytes();
            if (ipbytes != null)
            {
                numspinIPA.Value = ipbytes[0];
                numspinIPB.Value = ipbytes[1];
                numspinIPC.Value = ipbytes[2];
                numspinIPD.Value = ipbytes[3];
            }
            textDBName.Text = cServerSettings.GetInstance().DBName;
            textDBID.Text = cServerSettings.GetInstance().DBID;
            textDBPwd.Text = cServerSettings.GetInstance().DBPwd;
            textDBPort.Text = cServerSettings.GetInstance().DBPort.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string strIP;
                string strTemp;

                strTemp = textDBName.Text;
                if (strTemp.Trim() == "")
                {
                    MessageBox.Show("������ �Է��ϼ���.", "�Է¿���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textDBName.Focus();
                    textDBName_Enter(textDBName, new EventArgs());
                    this.DialogResult = DialogResult.None;
                    return;
                }
                strTemp = textDBID.Text;
                if (strTemp.Trim() == "")
                {
                    MessageBox.Show("������Ӿ��̵� �Է��ϼ���.", "�Է¿���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textDBID.Focus();
                    textDBID_Enter(textDBID, new EventArgs());
                    this.DialogResult = DialogResult.None;
                    return;
                }
                strTemp = textDBPwd.Text;
                if (strTemp.Length == 0)
                {
                    MessageBox.Show("������Ӿ�ȣ�� �Է��ϼ���.", "�Է¿���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textDBPwd.Focus();
                    textDBPwd_Enter(textDBPwd, new EventArgs());
                    this.DialogResult = DialogResult.None;
                    return;
                }
                UInt16 nPort = Convert.ToUInt16(textDBPort.Text);
                if (nPort < 1433 || nPort >= cServerSettings.GAMEPORT_MAX)
                {
                    MessageBox.Show("���������Ʈ�� �Է��ϼ���.", "�Է¿���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textDBPort.Focus();
                    textDBPort_Enter(textDBPort, new EventArgs());
                    this.DialogResult = DialogResult.None;
                    return;
                }
                strIP = string.Format("{0}.{1}.{2}.{3}",
                    numspinIPA.Value, numspinIPB.Value, numspinIPC.Value, numspinIPD.Value);
                cServerSettings.GetInstance().DBIP = IPAddress.Parse(strIP);
                cServerSettings.GetInstance().DBName = textDBName.Text;
                cServerSettings.GetInstance().DBID = textDBID.Text;
                cServerSettings.GetInstance().DBPwd = textDBPwd.Text;
                cServerSettings.GetInstance().DBPort = Convert.ToUInt16(textDBPort.Text);

                if (!cServerSettings.GetInstance().SaveSetting())
                {
                    MessageBox.Show("���������� �����Ҽ� �����ϴ�.", "�˸�",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            }
            catch(Exception)
            {
                MessageBox.Show("������������ �߸��ǿ����ϴ�.","�˸�",
                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                // DialogResult�� None���� �����Ͽ� ��ȭâ�� ������� �ʵ��� �Ѵ�.
                this.DialogResult = DialogResult.None;
            }
        }

        private void textDBName_Enter(object sender, EventArgs e)
        {
            textDBName.Select(0, textDBName.TextLength);
        }

        private void textDBID_Enter(object sender, EventArgs e)
        {
            textDBID.Select(0, textDBID.TextLength);
        }

        private void textDBPwd_Enter(object sender, EventArgs e)
        {
            textDBPwd.Select(0, textDBPwd.TextLength);
        }

        private void numspinipA_Enter(object sender, EventArgs e)
        {
            numspinIPA.Select(0, numspinIPA.Value.ToString().Length);
        }

        private void numspinipB_Enter(object sender, EventArgs e)
        {
            numspinIPB.Select(0, numspinIPB.Value.ToString().Length);
        }

        private void numspinipC_Enter(object sender, EventArgs e)
        {
            numspinIPC.Select(0, numspinIPC.Value.ToString().Length);
        }

        private void numspinipD_Enter(object sender, EventArgs e)
        {
            numspinIPD.Select(0, numspinIPD.Value.ToString().Length);
        }

        private void textDBPort_Enter(object sender, EventArgs e)
        {
            textDBPort.Select(0, textDBPort.ToString().Length);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
