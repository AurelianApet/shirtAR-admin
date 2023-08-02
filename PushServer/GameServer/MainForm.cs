#define POKER
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Timers;

namespace ArGateway
{
    using DataAccess;

    public enum SERVER_EVENT : int
    {
        ONGATEWAYDISCONNECT = 0,    // 게이트웨이서버소켓차단
        ONGATEWAYCONNECT,           // 게이트웨이서버와 소켓연결성공
        ONGATEWAYLOGIN,             // 게이트웨이서버로그인성공
        ONSERVICESTART,
        ONSERVICESTOP,
        ONSERVERMODE,               // 서버동작방식결정
        ONUSERLOGOUT,               // 유저가입탈퇴
        ONUSERLOGIN,                // 유저가입
        ONUSERCHANGE,               // 유저알방선택 및 기계점유상태변경
        ONCHATMSG,                  // 유저의 채팅로그사건
    }

    public enum LOG_EVENT : int
    {
        ONERRORMSG = 0,
        ONDEBUGMSG,
        ONNOTICEMSG,
        ONFILEMSG,
    }

    public delegate void ServerEvent(SERVER_EVENT evt, Object param);
    public delegate void LogEvent(LOG_EVENT evt, Object param);

    public partial class frmMain : Form
    {
        public delegate void ServerEventHandler(SERVER_EVENT evt, Object param);
        public delegate void LogEventHandler(LOG_EVENT evt, Object param);
        private const int MAXLOGLINE = 500;
        private FileStream m_fileStream = null;
        private string m_strFileName = "";

        private enum STATUS : byte
        {
            NOLOGIN_STATUS = 0,
            LOGIN_STATUS,
            SERVICE_STATUS,
        }

        private STATUS m_nStatus;

        private string m_strTitle = "푸시알람서버";

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = m_strTitle;
            m_nStatus = STATUS.NOLOGIN_STATUS;
            chkViewMode.Checked = true;
            UpdateMenu();
            cServerSettings.GetInstance().LoadSettings();
            PushAlarmServer.GetInstance().LogEventListener += new LogEvent(this.OnLogEvent);
            PushAlarmServer.GetInstance().ServerEventListener += new ServerEvent(this.OnServerEvent);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void mnuActionLogin_Click(object sender, EventArgs e)
        {
            mnuActionLogin.Enabled = false;
            Thread thread;
            thread = new Thread(new ThreadStart(LoginDB));
            thread.Start();
        }

        private void LoginDB()
        {
            this.UseWaitCursor = true;

            //디비서버에 접속해본다.
            OnLogEvent(LOG_EVENT.ONNOTICEMSG, "디비서버 접속중...");

            PushAlarmServer.GetInstance().TestDBConn = new MSSqlAccess();
            PushAlarmServer.GetInstance().TestDBConn.DBServer = cServerSettings.GetInstance().DBIP.ToString();
            PushAlarmServer.GetInstance().TestDBConn.DBPort = cServerSettings.GetInstance().DBPort.ToString();
            PushAlarmServer.GetInstance().TestDBConn.DBName = cServerSettings.GetInstance().DBName;
            PushAlarmServer.GetInstance().TestDBConn.DBID = cServerSettings.GetInstance().DBID;
            PushAlarmServer.GetInstance().TestDBConn.DBPwd = cServerSettings.GetInstance().DBPwd;
            PushAlarmServer.GetInstance().TestDBConn.Connect();

            if (!PushAlarmServer.GetInstance().TestDBConn.IsConnected)
            {
                mnuActionLogin.Enabled = true;
                this.UseWaitCursor = false;
                return;
            }

            PushAlarmServer.GetInstance().TestDBConn.Disconnect();

            OnLogEvent(LOG_EVENT.ONNOTICEMSG, "디비서버 접속이 성공되었습니다.");
            this.UseWaitCursor = false;
            OnServerEvent(SERVER_EVENT.ONSERVICESTOP, this);
        }

        public void OutputLog(string strMsg, bool bNewLine)
        {
            lock (listLog)
            {
                bool bViewMode = chkViewMode.Checked;
                int nLastRow = listLog.Items.Count;

                if (nLastRow >= MAXLOGLINE)
                {
                    listLog.Items.Clear();
                    //for (int i = 1; i < nLastRow; i++)
                    //{
                    //    object item = listLog.Items[i];
                    //    listLog.Items[i - 1] = item;
                    //}
                    //listLog.Items.RemoveAt(nLastRow - 1);
                }
                listLog.Items.Add(strMsg);
                if (bViewMode && nLastRow > 0)
                    listLog.TopIndex = listLog.Items.Count - 1;
            }
        }

        public void OutputLog(string strMsg)
        {
            OutputLog(strMsg, true);
        }

       private void UpdateCaption()
        {
            string strMode = "";
            if (m_nStatus == STATUS.SERVICE_STATUS)
            {
                strMode = "서비스중...";
            }
            strMode = string.Format("{0} {1}", m_strTitle, strMode);
            this.Text = strMode;
        }

        private void UpdateMenu()
        {
            bool bMnuActionLogin = true;
            bool bMnuActionStartSrv = false;
            bool bMnuActionStopSrv = false;

            switch (m_nStatus)
            {
                case STATUS.NOLOGIN_STATUS:
                default:
                    bMnuActionStartSrv = false;
                    bMnuActionStopSrv = false;
                    break;
                case STATUS.LOGIN_STATUS:
                    bMnuActionLogin = false;
                    bMnuActionStartSrv = true;
                    bMnuActionStopSrv = false;
                    break;
                case STATUS.SERVICE_STATUS:
                    bMnuActionLogin = false;
                    bMnuActionStartSrv = false;
                    bMnuActionStopSrv = true;
                    break;
            }
            mnuActionLogin.Enabled = bMnuActionLogin;
            mnuActionStart.Enabled = bMnuActionStartSrv;
            mnuActionStop.Enabled = bMnuActionStopSrv;
        }

        public void OnServerEvent(SERVER_EVENT evt, Object param)
        {
            if (this.Visible)
            {
                this.BeginInvoke(new ServerEventHandler(OnServerEventHandler), evt, param);
            }
        }

        public void OnLogEvent(LOG_EVENT evt, Object param)
        {
            if (this.Visible)
            {
                this.BeginInvoke(new LogEventHandler(OnLogEventHandler), evt, param);
            }
        }

        public void OnServerEventHandler(SERVER_EVENT evt, Object param)
        {
            switch (evt)
            {
                case SERVER_EVENT.ONGATEWAYDISCONNECT:  // 게이트웨이접속차단
                    PushAlarmServer.GetInstance().Close();
                    m_nStatus = STATUS.NOLOGIN_STATUS;
                    UpdateMenu();
                    break;
                case SERVER_EVENT.ONGATEWAYCONNECT:
                    break;
                case SERVER_EVENT.ONGATEWAYLOGIN:
                    break;
                case SERVER_EVENT.ONSERVICESTART:       // 서비스시작
                    m_nStatus = STATUS.SERVICE_STATUS;
                    UpdateMenu();
                    break;
                case SERVER_EVENT.ONSERVICESTOP:        // 서비스중지
                    m_nStatus = STATUS.LOGIN_STATUS;
                    UpdateMenu();
                    break;
                case SERVER_EVENT.ONSERVERMODE:         // 주서버동작사건
                    break;
                case SERVER_EVENT.ONUSERCHANGE:         // 유저알방선택 & 기계점유변경
                    break;
            }
            UpdateCaption();
        }

        public void OnLogEventHandler(LOG_EVENT evt, Object param)
        {
            string strMsg;
            string strTime, strMsgType;

            DateTime now = DateTime.Now;

            strTime = string.Format("{0:D4}년{1:D2}월{2:D2}일 {3:D2}:{4:D2}:{5:D2} ->", now.Year, now.Month, now.Day,
                now.Hour, now.Minute, now.Second);
            strMsgType = " ";
            bool bShow = true;
            switch (evt)
            {
                case LOG_EVENT.ONERRORMSG:
                    strMsgType = " === 오류발생 === ";
                    break;

                case LOG_EVENT.ONDEBUGMSG:
                    strMsgType = "### DEBUG ### ";
                    break;

                case LOG_EVENT.ONNOTICEMSG:
                    break;
                case LOG_EVENT.ONFILEMSG:
                    bShow = false;
                    break;
            }
            strMsg = string.Format("{0}{1}{2}", strTime, strMsgType, param.ToString());
            if (bShow)
            {
                OutputLog(strMsg);
            }
            LogToFile(strMsg);
        }

        private void mnuActionClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_Closing(object sender, FormClosingEventArgs e)
        {
            string strMsg = "프로그램을 종료하시겠습니까?";

            DialogResult result = MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                if (PushAlarmServer.GetInstance().IsStart)
                {
                    PushAlarmServer.GetInstance().Close();
                }
                e.Cancel = false;
            }
        }

        private void frmMainTextlog_Enter(object sender, EventArgs e)
        {
            chkViewMode.Focus();
        }

        private void mnuActionStart_Click(object sender, EventArgs e)
        {
            //mnuActionLogin_Click(null, null);
            PushAlarmServer.GetInstance().StartServer();
        }

        private void mnuActionStop_Click(object sender, EventArgs e)
        {
            PushAlarmServer.GetInstance().Close();
        }

        private void mnuSettingServ_Click(object sender, EventArgs e)
        {
            frmServerSetting frmSetting = new frmServerSetting();
            frmSetting.ShowDialog(this);
        }

        private void LogToFile(string strMsg)
        {
            try
            {
                if (m_strFileName.Length == 0 || m_fileStream == null)
                {
                    DateTime now = DateTime.Now;
                    m_strFileName = string.Format("PushAlarmServer_{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.log",
                        now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                }
                lock (this)
                {
                    string strFilePath = Application.StartupPath;
                    strFilePath = string.Format("{0}\\log\\", Application.StartupPath);
                    Directory.CreateDirectory(strFilePath);
                    strFilePath += m_strFileName;
                    m_fileStream = File.Open(strFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                    if (m_fileStream.Length > 204800)
                    {
                        DateTime now = DateTime.Now;
                        m_strFileName = string.Format("PushAlarmServer_{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.log",
                            now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                    }
                    StreamWriter writer = new StreamWriter(m_fileStream, Encoding.UTF8);
                    writer.WriteLine(strMsg);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                string strErr = string.Format("파일로그남기기 실패: {0}", ex.Message);
                OutputLog(strErr);
            }
            finally
            {
                if (m_fileStream != null)
                {
                    m_fileStream.Close();
                }
            }
        }
    }
}
