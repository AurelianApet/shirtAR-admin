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
    public partial class GetCustomerInfo : PageBase
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

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_customer WHERE id = {0}", new string[] { "@id" }, new object[] { id });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Response.Write("");
                    return;
                }

                string writer = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "writer", 0));
                string recv_contents = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "recv_contents", 0));
                string reg_date = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "reg_date", 0));

                string strItemFormat = "{{\"writer\":\"{0}\", " +
                                "\"recv_contents\":\"{1}\", " +
                                "\"reg_date\":\"{2}\" " +
                                "}}";

                string strData = string.Format(strItemFormat, writer, recv_contents, reg_date);

                Response.Write(strData);
                return;
            }
            catch (Exception) 
            {
                Response.Write("");
                return;
            }
        }
    }
}