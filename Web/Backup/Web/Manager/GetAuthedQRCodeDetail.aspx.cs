using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;
using DataAccess;
using System.Data;

namespace Web.Manager
{
    public partial class GetAuthedQRCodeDetail : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            Response.Clear();
            Response.ContentType = "text/html";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            DataSet dsContent = null;
            string strData = null;

            string id = Request.Params["id"];

            dsContent = DBConn.RunSelectQuery("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE tbl_qrcode.id = {0} and tbl_qrcode.qrcode_type = 1", new string[] { "@id" }, new object[] { id });
            if (DataSetUtil.IsNullOrEmpty(dsContent))
            {
                Response.Write(strData);
                Response.End();
                return;
            }
            string regdate = Server.UrlEncode(DateTime.Parse(DataSetUtil.RowStringValue(dsContent, "reg_date", 0)).ToString("yyyy-MM-dd"));
            string authdate = Server.UrlEncode(DateTime.Parse(DataSetUtil.RowStringValue(dsContent, "auth_date", 0)).ToString("yyyy-MM-dd"));
            string loginid = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "login_id", 0));
            string qrcodenumber = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "qrcode_number", 0));
            string applycount = DataSetUtil.RowStringValue(dsContent, "apply_count", 0);
            string producttype = DataSetUtil.RowStringValue(dsContent, "product_type", 0);
            string memo = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "memo", 0));
            string strItemFormat = "{{\"reg_date\":\"{0}\", " +
                                "\"auth_date\":\"{1}\", " +
                                "\"login_id\":\"{2}\", " +
                                "\"qrcode_number\":\"{3}\", " +
                                "\"apply_count\":\"{4}\", " +
                                "\"product_type\":\"{5}\", " +
                                "\"device_data\":\"{6}\", " +
                                "\"memo\":\"{7}\" " +
                                "}}";

            string device_data = "";
            dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_device WHERE qrcode_id = {0} ORDER BY reg_date ASC", new string[] { "@qrcode_id" }, new object[] { id });
            if (!DataSetUtil.IsNullOrEmpty(dsContent))
            {
                for (int i = 0; i < DataSetUtil.RowCount(dsContent); i++)
                {
                    if (i != 0) device_data += ";";

                    device_data += string.Format("{{\"id\":\"{0}\", \"device_id\":\"{1}\", \"device_name\":\"{2}\", \"device_version\":\"{3}\", \"is_activated\":\"{4}\", \"reg_date\":\"{5}\"}}",
                        DataSetUtil.RowStringValue(dsContent, "id", i),
                        Server.UrlEncode(CryptSHA256.Decrypt(DataSetUtil.RowStringValue(dsContent, "device_id", i))),
                        Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "device_name", i)),
                        Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "device_version", i)),
                        DataSetUtil.RowStringValue(dsContent, "is_activated", i),
                        Server.UrlEncode(DateTime.Parse(DataSetUtil.RowStringValue(dsContent, "reg_date", i)).ToString("yyyy-MM-dd")));
                }
            }

            strData = string.Format(strItemFormat, regdate, authdate, loginid, qrcodenumber, applycount, producttype, Server.UrlEncode(device_data), memo);
            Response.Write(strData);
            Response.End();
            return;
        }
    }
}