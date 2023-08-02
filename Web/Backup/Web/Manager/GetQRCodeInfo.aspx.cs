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
    public partial class GetQRCodeInfo : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            try
            {
                string id = Request.Params["id"];

                DataSet dsContent = DBConn.RunSelectQuery("SELECT tbl_qrcode.*, tbl_admin.login_id, tbl_admin.nickname FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE tbl_qrcode.id = {0} and tbl_qrcode.qrcode_type = 0", new string[] { "@id" }, new object[] { id });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Response.Write("");
                    return;
                }

                string nickname = DataSetUtil.RowStringValue(dsContent, "nickname", 0);
                string loginid = DataSetUtil.RowStringValue(dsContent, "login_id", 0);
                string qrcodenumber = DataSetUtil.RowStringValue(dsContent, "qrcode_number", 0);
                string applycount = DataSetUtil.RowStringValue(dsContent, "apply_count", 0);
                string producttype = DataSetUtil.RowStringValue(dsContent, "product_type", 0);

                string data = nickname + "," + loginid + "," + qrcodenumber + "," + applycount + "," + producttype;
                Response.Write(Server.UrlEncode(data));
                return;
            }
            catch (Exception) 
            {
                Response.Write("");
                return;
            }
        }
    }
}