using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;

namespace Web.Manager
{
    public partial class Manager : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            div_lbl_Nickname.Text = CurrentPage.AuthUser.Nickname;
            div_lbl_LoginID.Text = CurrentPage.AuthUser.LoginID;
        }
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }
    }
}