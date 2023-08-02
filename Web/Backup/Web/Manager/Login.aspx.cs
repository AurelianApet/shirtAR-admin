using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using retroplus.Common;
using DataAccess;

namespace Web.Manager
{
    public partial class Login : PageBase
    {
        protected override void Page_Init(object sender, EventArgs e)
        {

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            tbxLoginID.Attributes.Add("placeholder", Resources.Lang.STR_LOGIN_ID);
            tbxLoginPWD.Attributes.Add("placeholder", Resources.Lang.STR_LOGIN_PWD);
            btnLogin.Text = Resources.Lang.STR_LOGIN;

            //아이디저장쿠키가 있다면
            if (!IsPostBack)
            {
                if (Request.Cookies[Constants.COOKIE_KEY_REMEBERID] != null)
                {
                    string strCookie = Request.Cookies[Constants.COOKIE_KEY_REMEBERID][Constants.COOKIE_KEY_REMEBERID];
                    strCookie = CryptSHA256.Decrypt(strCookie);
                    string[] arrTemp = strCookie.Split(';');
                    if (arrTemp.Length == 2)
                    {
                        tbxLoginID.Text = arrTemp[1].ToString();
                        chkRememberMe.Checked = true;
                    }

                }
            }

            //자동로그인쿠키가 있다면 
            if (Request.Cookies[Constants.COOKIE_KEY_AUTOLOGIN] != null)
            {
                if (AuthUser != null)
                    Response.Redirect(Defines.URL_PREFIX_MANAGE + "/MemberMng.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (chkRememberMe.Checked)
            {
                isRememberMe = true;
            }
            else
            {
                isRememberMe = false;
            }
            if (chkAutoLogin.Checked)
            {
                isAutoLogin = true;
            }
            else
            {
                isAutoLogin = false;
            }
            if (string.IsNullOrEmpty(tbxLoginID.Text))
            {
                ShowMessageBox(Resources.Err.ERR_LOGINID_INPUT);
                return;
            }
            if (string.IsNullOrEmpty(tbxLoginPWD.Text))
            {
                ShowMessageBox(Resources.Err.ERR_LOGINPWD_INPUT);
                return;
            }

            if (!UserLogin(tbxLoginID.Text, tbxLoginPWD.Text))
            {
                ShowMessageBox(Resources.Err.ERR_LOGIN_FAILED);
                return;
            }
            else
            {
                //로그
                DBConn.RunInsertQuery("INSERT INTO tbl_log (admin_id, event, qrcode_count, product_type, reg_date) VALUES({0}, {1}, '', '', GETDATE())",
                    new string[] { "@admin_id", "@event" },
                    new object[] { AuthUser.ID, Resources.Lang.STR_LOGIN });
                //로그
                Response.Redirect(Defines.URL_PREFIX_MANAGE + "/QRCodeCreate.aspx");
            }
        }
    }

}