using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;

namespace Web.Manager
{
    public partial class Logout : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            UserLogout();

            if (!string.IsNullOrEmpty(Request.Params["auto"]))
                Session.Add(Constants.SESSION_KEY_AUTOLOGOUT, Request.Params["auto"]);

            Response.Redirect(Defines.URL_PREFIX_MANAGE + "/Login.aspx");
        }
    }
}