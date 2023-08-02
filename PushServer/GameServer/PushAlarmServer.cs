using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.Data;
using System.Runtime.InteropServices;
using System.Net.Json;

namespace ArGateway
{
    using DataAccess;
    using KSITC.Net;

    class PushAlarmServer 
    {
        private static PushAlarmServer m_Instance = null;

        protected bool m_bIsStart = false;

        public event LogEvent LogEventListener;
        public event ServerEvent ServerEventListener;

        protected MSSqlAccess _testdbconn = null;
        public MSSqlAccess TestDBConn
        {
            get{return _testdbconn;}
            set{_testdbconn = value;}
        }

        /// <summary>
        /// ������ �������¸� ��Ÿ����.
        /// </summary>
        public bool IsStart
        {
            get{return m_bIsStart;}
        }

        //2015.09.12
        private Thread m_threadPushAlarm = null;
        //2015.09.12

        public PushAlarmServer()
        {
            m_bIsStart = false;
        }

        public static PushAlarmServer GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new PushAlarmServer();
            }
            return m_Instance;
        }

        public void StartServer()
        {
            try
            {
                if (!this.IsStart)
                {
                    OnStart();
                }

                SendLog("���񽺸� �����մϴ�.", null);

                ServerEventListener(SERVER_EVENT.ONSERVICESTART, this);
            }
            catch(Exception ex)
            {
                SendLog("���ϴ����·� ���� �����ϴ�.", ex);
                Close();
            }
        }

        // ���ϴ����¿� �� �� ȣ��
        public void OnStart()
        {
            m_bIsStart = true;
            if (m_threadPushAlarm == null)
            {
                m_threadPushAlarm = new Thread(new ThreadStart(MonitoringPushAlarm));
                m_threadPushAlarm.Start();
            }
        }

        MSSqlAccess MakingDBConn()
        {
            MSSqlAccess dbconn = null;
            try
            {
                dbconn = new MSSqlAccess();
                dbconn.DBServer = TestDBConn.DBServer;
                dbconn.DBPort = TestDBConn.DBPort;
                dbconn.DBName = TestDBConn.DBName;
                dbconn.DBID = TestDBConn.DBID;
                dbconn.DBPwd = TestDBConn.DBPwd;
                dbconn.Connect();
            }
            catch (Exception ex)
            {
                SendLog("��� ���� ����!", ex);
                return null;
            }

            return dbconn;
        }

        //2015.09.12
        void MonitoringPushAlarm()
        {
            MSSqlAccess dbconn = MakingDBConn();
            DateTime dtDBDate = DateTime.Now;

            string PUSH_SECRET_KEY = "kMAeFQKxY9jn43U2GjOPvnnjbH3qfSRK";
            string PUSH_API_KEY = "LZEqOKGr721GBqWs2eMUw75t";

            BaiduPush Bpush = new BaiduPush("POST", PUSH_SECRET_KEY);

            while (m_bIsStart)
            {
                try
                {
                    // ��������� �ȵ��ִ� ��� �ٽ� ���ӽõ�
                    if (dbconn == null || !dbconn.IsConnected)
                    {
                        dbconn = MakingDBConn();
                    }

                    string query = string.Format("SELECT * FROM tbl_pushinfo where CONVERT(nvarchar(20), reg_date, 20) like '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    DataSet dsTemp = dbconn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsTemp))
                    {
                        String apiKey = PUSH_API_KEY;
                        String messages = "";
                        String method = "push_msg";
                        TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                        uint device_type = 3;
                        uint unixTime = (uint)ts.TotalSeconds;

                        uint message_type = 1;  //0: Message 1: Notification
                        string messageksy = "xxxxxx";

                        long lTotalCount = 0;
                        long lSendCount = 0;
                        DateTime dtDate = DateTime.Now;

                        string strMsgTitle = DataSetUtil.RowStringValue(dsTemp, "msg_title", 0);
                        string strMsgContent = DataSetUtil.RowStringValue(dsTemp, "msg_content", 0);

                        DataSet dsDevice = dbconn.RunSelectQuery("SELECT * FROM tbl_pushdevice");
                        for (int i = 0; i < DataSetUtil.RowCount(dsDevice); i++)
                        {
                            lTotalCount++;
                            if (DataSetUtil.RowIntValue(dsDevice, "device_type", i) == 0)
                            {//�ȵ���̵�
                                BaiduPushNotification notification = new BaiduPushNotification();
                                notification.title = strMsgTitle;
                                notification.description = strMsgContent;
                                messages = notification.getJsonString();
                            }
                            else
                            {//IOS
                                device_type = 4;
                                IOSNotification notification = new IOSNotification();
                                notification.title = strMsgTitle;
                                notification.description = strMsgContent;
                                messages = notification.getJsonString();
                            }

                            string UserId = DataSetUtil.RowStringValue(dsDevice, "user_id", i);
                            string ChannelId = DataSetUtil.RowStringValue(dsDevice, "channel_id", i);

                            PushOptions pOpts = new PushOptions(method, apiKey, UserId, ChannelId, device_type, messages, messageksy, unixTime);
                            pOpts.message_type = message_type;
                            pOpts.deploy_status = 2;    //1: ios�����ڸ�� 2: ios��ǰ���
                            string response = Bpush.PushMessage(pOpts);

                            if (response.IndexOf("error_code") < 0)
                                lSendCount++;
                        }
                        query = string.Format("UPDATE tbl_pushinfo SET status = 1, send_count = {0}, total_count = {1} WHERE id = {2}", lSendCount, lTotalCount, DataSetUtil.RowStringValue(dsTemp, "id", 0));
                        dbconn.RunUpdateQuery(query);
                        SendLog(DataSetUtil.RowStringValue(dsTemp, "reg_date", 0) + " ����� Ǫ�þ˶��� ��Ȯ�� ���۵Ǿ����ϴ�.");
                        SendOnlyFileLog(DataSetUtil.RowStringValue(dsTemp, "reg_date", 0) + " ����� Ǫ�þ˶��� ��Ȯ�� ���۵Ǿ����ϴ�.");
                    }

                    // �Ϸ翡 �ѹ��� ��� ���� �缳��
                    if ((DateTime.Now - dtDBDate).TotalHours > 24)
                    {
                        dtDBDate = DateTime.Now;
                        dbconn.Disconnect();
                        dbconn = null;
                    }
                }
                catch (Exception ex)
                {
                    SendLog("����� Ǫ�þ˸� �߼��� ���ܰ� �߻��Ͽ����ϴ�.", ex);
                }
                Thread.Sleep(1000);
            }
        }
        //2015.09.12

        public void Close()
        {
            if (m_bIsStart)
            {
                m_bIsStart = false;
                SendLog("���񽺸� �����մϴ�.");
                ServerEventListener(SERVER_EVENT.ONSERVICESTOP, this);
            }
        }
        
        /// <summary>
        /// �����α׸� �����.
        /// </summary>
        /// <param name="strMsg">�α׹��ڿ�</param>
        public void SendLog(string strMsg, Exception ex)
        {
            if (ex != null)
            {
                strMsg = string.Format("{0} ��������: {1} �󼼼���: {2}", strMsg, ex.Message, ex.StackTrace);
            }
            SendLog(strMsg);
        }

        /// <summary>
        /// �����α׸� �����.
        /// </summary>
        /// <param name="strMsg">�α׹��ڿ�</param>
        public void SendLog(string strMsg)
        {
            LogEventListener(LOG_EVENT.ONNOTICEMSG, strMsg);
        }

        /// <summary>
        /// ���Ͽ��� �α׸� ���⵵�� �Ѵ�.
        /// </summary>
        /// <param name="strMsg">�α׹��ڷ�</param>
        public void SendOnlyFileLog(string strMsg)
        {
            LogEventListener(LOG_EVENT.ONFILEMSG, strMsg);
        }

        /// <summary>
        /// ���Ͽ��� �α׸� ���⵵�� �Ѵ�.
        /// </summary>
        /// <param name="strMsg">�α׹��ڷ�</param>
        public void SendOnlyFileLog(string strMsg, Exception ex)
        {
            if (ex != null)
            {
                strMsg = string.Format("{0} ��������: {1} �󼼼���: {2}", strMsg, ex.Message, ex.StackTrace);
            }
            SendOnlyFileLog(strMsg);
        }

    }
}
