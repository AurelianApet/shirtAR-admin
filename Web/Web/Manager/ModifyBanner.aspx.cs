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
    public partial class ModifyBanner : PageBase
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
                string id = Server.UrlDecode(Request.Params["id"]);
                string is_activate = Server.UrlDecode(Request.Params["is_activate"]);

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(is_activate))
                {
                    Response.Write("1");
                    return;
                }

                DBConn.RunUpdateQuery("UPDATE tbl_banner SET is_activate={0} WHERE id={1}", new string[] { "@is_activate", "@id" }, new object[] { Int32.Parse(is_activate), Int32.Parse(id) });
                Response.Write("0");
                return;
            }
            catch (Exception ex)
            {
                Response.Write("1");
                return;
            }
        }
    }
}