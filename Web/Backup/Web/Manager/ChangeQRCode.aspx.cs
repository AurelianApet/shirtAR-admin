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
    public partial class ChangeQRCode : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            string id = Request.Params["id"];
            string apply_count = Request.Params["apply_count"];
            string product_type = Request.Params["product_type"];

            try
            {
                DBConn.RunUpdateQuery("UPDATE tbl_qrcode SET apply_count = {0}, product_type = {1} WHERE id = {2}",
                    new string[] { "@apply_count", "@product_type", "@id" },
                    new object[] { apply_count, product_type, id });
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