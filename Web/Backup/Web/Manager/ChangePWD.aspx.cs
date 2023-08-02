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
    public partial class ChangePWD : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            string pwd = Request.Params["pwd"];

            if (CurrentPage.AuthUser.ULevel != 1)
            {
                Alert(Resources.Lang.MSG_ADMIN_ACCESSABLE, "/Manager/Logout.aspx");
                return;
            }
            try
            {
                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin WHERE id={0}", new string[] { "@id" }, new object[] { AuthUser.ID });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Alert(Resources.Lang.MSG_WRONG_ACCESS, "/Manager/Logout.aspx");
                    return;
                }

                DBConn.RunUpdateQuery("UPDATE tbl_admin SET login_pwd = {0} WHERE id={1}",
                    new string[] { "@login_pwd", "@id" },
                    new object[] { CryptSHA256.Encrypt(Server.UrlDecode(pwd)), AuthUser.ID });


                //로그
                DBConn.RunInsertQuery("INSERT INTO tbl_log (admin_id, event, qrcode_count, product_type, reg_date) VALUES({0}, {1}, '', '', GETDATE())",
                    new string[] { "@admin_id", "@event" },
                    new object[] { AuthUser.ID, Resources.Lang.STR_PASSWORD_CHANGE });
                //로그

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