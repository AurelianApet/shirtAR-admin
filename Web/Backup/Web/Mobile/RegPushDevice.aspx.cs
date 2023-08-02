using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.IO;

using retroplus.Common;
using DataAccess;
using System.Data;
using System.Text;

namespace Web.Mobile
{
    public partial class RegPushDevice : MobilePageBase
    {
        private const string RESULT = "result";
        private const string SUCCESS = "success";
        private const string FAILED = "failed";

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/html";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            
            try
            {
                procRegDevice();
            }
            catch(Exception ex)
            {
                Response.Write(getResponse(1, RESULT, ex.Message));
                writeLog("getData() -> error: " + ex.Message);
            }
            Response.End();

        }
        // 장치푸시 아이디 등록
        protected void procRegDevice()
        {
            string strAPIKey = Request.Params["api_key"];
            string strUserId = Request.Params["user_id"];
            string strChannelId = Request.Params["channel_id"];
            int iDeviceType = int.Parse(Request.Params["device_type"]);

            if (string.IsNullOrEmpty(strAPIKey) || string.IsNullOrEmpty(strUserId) || string.IsNullOrEmpty(strChannelId) || iDeviceType < 0 || iDeviceType > 1)
                throw new Exception("Parameter Error!");

            if (iDeviceType == 0)
            {
                if (strAPIKey != Defines.PUSH_API_KEY)
                    throw new Exception("Register Error!");
            }
            else if (iDeviceType == 1)
            {
                if (strAPIKey != Defines.PUSH_API_KEY)
                    throw new Exception("Register Error!");
            }

            DataSet dsChk = DBConn.RunSelectQuery("SELECT id FROM tbl_pushdevice WHERE user_id={0} AND channel_id={1} AND device_type={2}",
                new string[] { "@user_id", "@channel_id", "@device_type" }, new object[] { strUserId, strChannelId, iDeviceType });
            if (!DataSetUtil.IsNullOrEmpty(dsChk))
                throw new Exception("Exist!");

            long lID = DBConn.RunInsertQuery("INSERT INTO tbl_pushdevice (user_id, channel_id, device_type) VALUES({0}, {1}, {2})",
                new string[] { "@user_id", "@channel_id", "@device_type" }, new object[] { strUserId, strChannelId, iDeviceType }, true);

            Response.Write(getResponse(1, RESULT, lID > 0 ? SUCCESS : FAILED));
        }

        protected string getResponse(int iDataCount, params object[] args)
        {
            return getResponse(iDataCount, true, args);
        }

        protected string getResponse(int iDataCount, bool bEncode, params object[] args)
        {
            if (bEncode)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].GetType() == typeof(string))
                        args[i] = HttpUtility.UrlEncode(args[i].ToString());
                }
            }
            return string.Format(getFormatString(iDataCount, args), args);
        }

        protected string getFormatString(int iDataCount, params object[] args)
        {
            string strRet = "{{";
            string strSpliter = "";
            for (int i = 0; i < iDataCount; i++)
            {
                if (args[i * 2 + 1].GetType() == typeof(string))
                    strRet += strSpliter + "\"{" + (i * 2) + "}\": \"{" + (i * 2 + 1) + "}\"";
                else
                    strRet += strSpliter + "\"{" + (i * 2) + "}\": {" + (i * 2 + 1) + "}";

                if (strSpliter == "") strSpliter = ", ";
            }
            strRet += "}}";
            return strRet;
        }

        protected void writeLog(string strFormat, params object[] args)
        {
            try
            {
                string strLogDir = Server.MapPath("/logs");
                if (!Directory.Exists(strLogDir))
                    Directory.CreateDirectory(strLogDir);

                using (StreamWriter sWriter = new StreamWriter(strLogDir + "\\output.log", true))
                {
                    sWriter.WriteLine("{0:yyyy-MM-dd HH:mm:ss} === {1}", DateTime.Now, string.Format(strFormat, args));
                    sWriter.Close();
                }
            }
            catch { }
        }
    }
}