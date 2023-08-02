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
    public partial class AddAccount : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            string nickname = Server.UrlDecode(Request.Params["nickname"]);
            string login_id = Server.UrlDecode(Request.Params["login_id"]);
            string login_pwd = Server.UrlDecode(Request.Params["login_pwd"]);
            string level = Request.Params["level"];
            string qrcode_count = Request.Params["qrcode_count"];

            if (CurrentPage.AuthUser.ULevel != 1)
            {
                Alert(Resources.Lang.MSG_ADMIN_ACCESSABLE, "/Manager/Logout.aspx");
                return;
            }
            try
            {
                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin WHERE login_id={0}", new string[] { "@login_id" }, new object[] { login_id });
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Response.Write("1");
                    return;
                }

                dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin WHERE nickname={0}", new string[] { "@nickname" }, new object[] { nickname });
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Response.Write("2");
                    return;
                }

                DBConn.RunInsertQuery("INSERT INTO tbl_admin (login_id, login_pwd, nickname, qrcode_count, level, reg_date) VALUES({0}, {1}, {2}, {3}, {4}, GETDATE())",
                    new string[] { "@login_id", "@login_pwd", "@nickname", "@qrcode_count", "@level" },
                    new object[] { login_id, CryptSHA256.Encrypt(login_pwd), nickname, qrcode_count, level });
                Response.Write("0");
                return;
            }
            catch (Exception ex) 
            {
                return;
            }
        }
    }
}