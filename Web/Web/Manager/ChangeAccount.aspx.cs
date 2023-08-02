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
    public partial class ChangeAccount : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            string login_id = Server.UrlDecode(Request.Params["login_id"]);
            string login_pwd = Server.UrlDecode(Request.Params["login_pwd"]);
            string level = Request.Params["level"];
            string qrcode_count = Request.Params["qrcode_count"];

            if (CurrentPage.AuthUser.ULevel != 1)
            {
                //최고관리자만 접근가능합니다.
                Alert(Resources.Lang.MSG_ADMIN_ACEPT_ONLY, "/Manager/Logout.aspx");
                return;
            }
            try
            {
                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin WHERE login_id={0}", new string[] { "@login_id" }, new object[] { login_id });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Alert(Resources.Lang.MSG_WRONG_ACCESS, "/Manager/Logout.aspx");
                    return;
                }

                DBConn.RunUpdateQuery("UPDATE tbl_admin SET login_pwd = {0}, level={1}, qrcode_count={2} WHERE login_id={3}",
                    new string[] { "@login_pwd", "@level", "@qrcode_count", "@login_id" },
                    new object[] { CryptSHA256.Encrypt(Server.UrlDecode(login_pwd)), level, qrcode_count, login_id });

                Response.Write("0");
                return;
            }
            catch (Exception) 
            {
                return;
            }
        }
    }
}