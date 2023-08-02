using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;
using DataAccess;
using System.Data;

using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Net.Security;
using System.Collections.Specialized;
using System.Net.Sockets;

namespace Web.Manager
{
    public partial class PushAlarm : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.AuthUser.ULevel == 0)
            {
                isAdmin.Value = "0";
            }
            else if (CurrentPage.AuthUser.ULevel == 1)
            {
                isAdmin.Value = "1";
            }
            base.Page_Load(sender, e);
        }

        protected override void InitControls()
        {
            base.InitControls();

            ddlHour.Items.Clear();
            for (int i = 0; i < 24; i++)
                ddlHour.Items.Add(new ListItem(string.Format("{0:D2}", i), string.Format("{0:D2}", i)));
            ddlMinute.Items.Clear();
            for (int i = 0; i < 60; i++)
                ddlMinute.Items.Add(new ListItem(string.Format("{0:D2}", i), string.Format("{0:D2}", i)));

        }


        protected override GridView getGridControl()
        {
            return gvContent;
        }

        protected override void LoadData()
        {
            base.LoadData();
            string query = "";

            query = string.Format("SELECT * FROM tbl_pushinfo ORDER BY reg_date DESC");

            PageDataSource = DBConn.RunSelectQuery(query);
            BindData();
        }

        protected override void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            base.gvContent_RowDataBound(sender, e);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //메시지ID
                string id = dr["id"].ToString();
                //메시지타이틀
                string msgtitle = dr["msg_title"].ToString();
                //메시지내용
                string msgcontent = dr["msg_content"].ToString();
                //상태
                string status = dr["status"].ToString();
                //전송한 수
                string sendcount = dr["send_count"].ToString();
                //총디바이스수
                string totalcount = dr["total_count"].ToString();
                //등록날짜
                string reg_date = dr["reg_date"].ToString();

                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrMsgTitle = (Literal)e.Row.FindControl("ltrMsgTitle");
                Literal ltrMsgContent = (Literal)e.Row.FindControl("ltrMsgContent");
                Literal ltrTotalCount = (Literal)e.Row.FindControl("ltrTotalCount");
                Literal ltrSendCount = (Literal)e.Row.FindControl("ltrSendCount");
                Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");

                ltrRegDate.Text = reg_date;
                ltrMsgTitle.Text = msgtitle;
                ltrMsgContent.Text = msgcontent;
                ltrTotalCount.Text = totalcount;
                ltrSendCount.Text = sendcount;
                
                if (status == "1"){
                    ltrStatus.Text = Resources.Lang.STR_SEND_END;
                }else if(status == "2"){
                    ltrStatus.Text = Resources.Lang.STR_SEND_WAITING;
                }

            }
        }

        public bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public byte[] HexStringToByteArray(String s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }

        protected bool SendToIOS(string strDeviceID, string strMsg)
        {
            try
            {
                //X509Certificate2 clientCertificate = new X509Certificate2(mIOSCertPath, mIOSCertPassword);
                X509Certificate2 clientCertificate = new X509Certificate2(Defines.PUSH_IOS_CERTIFICATE_PATH, Defines.PUSH_IOS_CERTIFICATE_PASSWORD, X509KeyStorageFlags.MachineKeySet);
                X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);
                TcpClient client = new TcpClient(Defines.PUSH_IOS_HOSTNAME, Defines.PUSH_IOS_PORT);
                SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                try
                {
                    sslStream.AuthenticateAsClient(Defines.PUSH_IOS_HOSTNAME, certificatesCollection, SslProtocols.Tls, true);
                }
                catch (Exception e)
                {
                    client.Close();
                    throw (e);
                }
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);  //The command
                writer.Write((byte)0);  //The first byte of the deviceId length (big-endian first byte)
                writer.Write((byte)32); //The deviceId length (big-endian second byte)
                writer.Write(HexStringToByteArray(strDeviceID.ToUpper()));
                String strPayLoad = "{\"aps\":{\"alert\":\"" + strMsg + "\",\"badge\":0,\"sound\":\"default\"}}";

                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(strPayLoad);
                byte[] payloadSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(b1.Length)));
                writer.Write(payloadSize);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string strMsgTitle = tbxMsgTitle.Text;
            string strMsgContent = tbxMsgContent.Text;
            if (string.IsNullOrEmpty(strMsgTitle))
            {
                Alert(Resources.Lang.MSG_INPUT_MESSAGE_TITLE, "/Manager/PushAlarm.aspx");
                return;
            }
            if (strMsgTitle.Length > 50)
            {
                Alert(Resources.Lang.STR_MSG_TITLE_50_LIMIT, "/Manager/PushAlarm.aspx");
                return;
            }
            if (string.IsNullOrEmpty(strMsgContent))
            {
                Alert(Resources.Lang.MSG_INPUT_MESSAGE_CONTENT, "/Manager/PushAlarm.aspx");
                return;
            }
            if (strMsgContent.Length > 50)
            {
                Alert(Resources.Lang.STR_MSG_TITLE_50_LIMIT, "/Manager/PushAlarm.aspx");
                return;
            }

            string strType = Request.Form["optType"];

            string sk = Defines.PUSH_SECRET_KEY;
            string ak = Defines.PUSH_API_KEY;

            BaiduPush Bpush = new BaiduPush("POST", sk);
            String apiKey = ak;
            String messages = "";
            String method = "push_msg";
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            uint device_type = 3;
            uint unixTime = (uint)ts.TotalSeconds;

            uint message_type = 1;  //0: Message 1: Notification
            string messageksy = "xxxxxx";


            long lTotalCount = 0;
            long lSendCount = 0;
            int iStatus = 0;
            DateTime dtDate = DateTime.Now;
            if (strType == "1")
            {//즉시전송
                DataSet dsDevice = DBConn.RunSelectQuery("SELECT * FROM tbl_pushdevice");
                for (int i = 0; i < DataSetUtil.RowCount(dsDevice); i++)
                {
                    lTotalCount++;
                    if (DataSetUtil.RowIntValue(dsDevice, "device_type", i) == 0)
                    {//안드로이드
                        BaiduPushNotification notification = new BaiduPushNotification();
                        notification.title = strMsgTitle;
                        notification.description = strMsgContent;

                        //알림스타일 0:기본값 1:알림삭제가능 2:진동 4:알림음성
                        notification.notification_basic_style = 1;

                        messages = notification.getJsonString();

                        string UserId = DataSetUtil.RowStringValue(dsDevice, "user_id", i);
                        string ChannelId = DataSetUtil.RowStringValue(dsDevice, "channel_id", i);

                        PushOptions pOpts = new PushOptions(method, apiKey, UserId, ChannelId, device_type, messages, messageksy, unixTime);
                        pOpts.message_type = message_type;
                        //pOpts.deploy_status = 2;    //1: ios개발자모드 2: ios제품모드
                        string response = Bpush.PushMessage(pOpts);

                        if (response.IndexOf("error_code") < 0)
                            lSendCount++;
                    }
                    else
                    {//IOS
                        string UserId = DataSetUtil.RowStringValue(dsDevice, "user_id", i);
                        if (SendToIOS(UserId, strMsgContent))
                            lSendCount++;
                    }
                }
                iStatus = 1;
                DBConn.RunInsertQuery("INSERT INTO tbl_pushinfo (msg_title, msg_content, status, send_count, total_count, reg_date) VALUES({0}, {1}, {2}, {3}, {4}, {5})",
                    new string[] {
                        "@msg_title",
                        "@msg_content",
                        "@status",
                        "@send_count",
                        "@total_count",
                        "@reg_date"
                        },
                    new object[] {
                        strMsgTitle,
                        strMsgContent,
                        iStatus,
                        lSendCount,
                        lTotalCount,
                        dtDate
                        });
                Alert(string.Format(Resources.Lang.MSG_PUSH_SEND_CONFIRM, lTotalCount, lSendCount), "/Manager/PushAlarm.aspx");
                return;
            }
            else if (strType == "2")
            {//예약전송
                if (string.IsNullOrEmpty(tbxDate.Text) ||
                    !DateTime.TryParse(tbxDate.Text + " " + ddlHour.SelectedValue + ":" + ddlMinute.SelectedValue + ":00", out dtDate))
                {
                    Alert(Resources.Lang.MSG_INPUT_SEND_DATE, "/Manager/PushAlarm.aspx");
                    return;
                }
                iStatus = 2;
                DBConn.RunInsertQuery("INSERT INTO tbl_pushinfo (msg_title, msg_content, status, send_count, total_count, reg_date) VALUES({0}, {1}, {2}, {3}, {4}, {5})",
                    new string[] {
                        "@msg_title",
                        "@msg_content",
                        "@status",
                        "@send_count",
                        "@total_count",
                        "@reg_date"
                        },
                    new object[] {
                        strMsgTitle,
                        strMsgContent,
                        iStatus,
                        lSendCount,
                        lTotalCount,
                        dtDate
                    });
                Alert(Resources.Lang.STR_RESERVATION, "/Manager/PushAlarm.aspx");
                return;
            }
        }
    }
}