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
    public partial class SetMemo : PageBase
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
                string memo = Server.UrlDecode(Request.Params["memo"]);

                DBConn.RunUpdateQuery("UPDATE tbl_qrcode SET memo = {0} WHERE id={1} and qrcode_type = 1", 
                    new string[] { "@memo", "@id" }, 
                    new object[] { memo, id }
                    );

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