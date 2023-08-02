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
    public partial class ConfirmAuthKey : MobilePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string responseText = "";
            string qrcode_number = Server.UrlDecode(Request.Params["qrcode_number"]).ToUpper();
            string device_id = Server.UrlDecode(Request.Params["device_id"]);
            string device_name = Server.UrlDecode(Request.Params["device_name"]);
            string device_version = Server.UrlDecode(Request.Params["device_version"]);

            StringBuilder responseXml = new StringBuilder();

            try
            {
                if (string.IsNullOrEmpty(qrcode_number) || string.IsNullOrEmpty(device_id) || string.IsNullOrEmpty(device_name) || string.IsNullOrEmpty(device_version))
                {
                    responseText = "예상치 못한 오류!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"4\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_qrcode WHERE qrcode_number={0} and qrcode_type = 1", new string[] { "@qrcode_number" }, new object[] { qrcode_number });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//입력하신 코드는 존재하지 않는 코드 번호입니다. 다시 확인 후 코드를 재입력해 주시기 바랍니다.
                    responseText = "인증코드 불일치 오류!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"2\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                int qrcode_id = DataSetUtil.RowIntValue(dsContent, "id", 0);
                int apply_count = DataSetUtil.RowIntValue(dsContent, "apply_count", 0);

                dsContent = DBConn.RunSelectQuery("SELECT COUNT(*) AS activated_count FROM tbl_device WHERE qrcode_id = {0} and is_activated = 1", new string[] { "@qrcode_id" }, new object[] { qrcode_id });
                int activated_count = DataSetUtil.RowIntValue(dsContent, "activated_count", 0);

                if (apply_count <= activated_count)
                {//적용가능한 기기수 초과.
                    responseText = "적용가능한 기기수 초과 오류!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"1\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_device WHERE device_id = {0}", new string[] { "@device_id" }, new object[] { CryptSHA256.Encrypt(device_id) });
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    if (DataSetUtil.RowIntValue(dsContent, "qrcode_id", 0) != qrcode_id)
                    {
                        DBConn.RunUpdateQuery("UPDATE tbl_device SET qrcode_id = {0}, is_activated = 1 WHERE device_id = {1}",
                            new string[] { "@qrcode_id", "@device_id" },
                            new object[] { qrcode_id, CryptSHA256.Encrypt(device_id) });

                        DBConn.RunUpdateQuery("UPDATE tbl_qrcode SET is_use = 1 WHERE id = {0}", new string[] {"@id"}, new object[]{qrcode_id});
                    }
                    else
                    {
                        if (DataSetUtil.RowIntValue(dsContent, "is_activated", 0) == 0)
                        {//비활성화
                            //해당 디바이스에 관해서 비활성화 된 인증키를 입력한 경우
                            responseText = "비활성화된 인증키 입력 오류!";
                            responseXml.Clear();
                            responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            responseXml.Append("<response code=\"3\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                            responseXml.Append("</response>");

                            Response.Write(responseXml.ToString());
                            return;
                        }
                        else
                        {//활성화
                        }
                    }
                }
                else
                {
                    DBConn.RunInsertQuery("INSERT INTO tbl_device (qrcode_id, device_id, device_name, device_version, is_activated, reg_date) VALUES({0}, {1}, {2}, {3}, 1, GETDATE())",
                        new string[] { "@qrcode_id", "@device_id", "@device_name", "@device_version" },
                        new object[] { qrcode_id, CryptSHA256.Encrypt(device_id), device_name, device_version });

                    DBConn.RunUpdateQuery("UPDATE tbl_qrcode SET is_use = 1 WHERE id = {0}", new string[] { "@id" }, new object[] { qrcode_id });
                }

                responseText = "성공!";
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response code=\"0\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
            catch (Exception) 
            {
                responseText = "예상치 못한 오류!";
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response code=\"4\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
        }
    }
}