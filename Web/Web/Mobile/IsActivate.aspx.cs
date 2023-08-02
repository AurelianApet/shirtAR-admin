using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;
using DataAccess;
using System.Data;
using System.Text;

namespace Web.Mobile
{
    public partial class IsActivate : MobilePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string responseText = "";
            string device_id = Server.UrlDecode(Request.Params["device_id"]);

            StringBuilder responseXml = new StringBuilder();

            try
            {
                if (string.IsNullOrEmpty(device_id))
                {
                    responseText = "예상치 못한 오류!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"4\" qrcode_number=\"\" interval=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_device WHERE device_id = {0}", new string[] { "@device_id" }, new object[] { CryptSHA256.Encrypt(device_id) });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//존재하지 않는 디바이스
                    responseText = "존재하지 않음!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"2\" qrcode_number=\"\" interval=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                int qrcode_id = DataSetUtil.RowIntValue(dsContent, "qrcode_id", 0);
                if (DataSetUtil.RowIntValue(dsContent, "is_activated", 0) == 1)
                {
                    dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_qrcode WHERE id = {0}", new string[] { "@id" }, new object[] { qrcode_id });
                    if (DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        responseText = "QR코드번호 얻기 못함!";
                        responseXml.Clear();
                        responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        responseXml.Append("<response code=\"5\" qrcode_number=\"\" interval=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                        responseXml.Append("</response>");

                        Response.Write(responseXml.ToString());
                        return;
                    }

                    string qrcode_number = DataSetUtil.RowStringValue(dsContent, "qrcode_number", 0);

                    dsContent = DBConn.RunSelectQuery("SELECT TOP(1) * FROM tbl_config");
                    if (DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        responseText = "서버접속주기 얻기 못함!";
                        responseXml.Clear();
                        responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        responseXml.Append("<response code=\"3\" qrcode_number=\"\" interval=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                        responseXml.Append("</response>");

                        Response.Write(responseXml.ToString());
                        return;
                    }

                    responseText = "디바이스가 존재하면서 QR코드가 활성화 됨!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"0\" qrcode_number=\"" + Server.UrlEncode(qrcode_number) + "\" interval=\"" + DataSetUtil.RowIntValue(dsContent, "connect_cycle", 0) + "\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;

                }

                responseText = "디바이스가 존재하면서 QR코드가 비활성화 됨!";
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response code=\"1\" qrcode_number=\"\" interval=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
            catch (Exception)
            {
                responseText = "예상치 못한 오류!";
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response code=\"4\" qrcode_number=\"\" interval=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
        }
    }
}