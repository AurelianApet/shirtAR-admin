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
    public partial class ChangeConfig : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            int connect_cycle = int.Parse(Server.UrlDecode(Request.Params["connect_cycle"]));
            int count_per_page = int.Parse(Server.UrlDecode(Request.Params["count_per_page"]));
            int lang = int.Parse(Server.UrlDecode(Request.Params["lang"]));

            if (CurrentPage.AuthUser.ULevel != 1)
            {
                //최고관리자만 접근가능합니다.
                Alert(Resources.Lang.MSG_ADMIN_ACEPT_ONLY, "/Manager/Logout.aspx");
                return;
            }
            try
            {
                DBConn.RunUpdateQuery("UPDATE tbl_config SET connect_cycle = {0}, lang={1}, count_per_page={2}",
                    new string[] { "@connect_cycle", "@lang", "@count_per_page" },
                    new object[] { connect_cycle, lang, count_per_page });

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