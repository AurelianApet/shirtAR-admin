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
    public partial class MyAccount : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            lbl_Nickname.Text   = CurrentPage.AuthUser.Nickname;
            lbl_LoginID.Text    = CurrentPage.AuthUser.LoginID;
            hd_LoginPWD.Value   = CurrentPage.AuthUser.LoginPwd;
            
            if (CurrentPage.AuthUser.ULevel == 0)
            {
                lbl_Level.Text = "라이센스 관리자";
                isAdmin.Value = "0";
            }
            else if (CurrentPage.AuthUser.ULevel == 1)
            {
                lbl_Level.Text = "최고 관리자";
                isAdmin.Value = "1";
            }
            base.Page_Load(sender, e);
        }
    }
}