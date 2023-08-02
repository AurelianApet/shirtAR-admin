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
    public partial class GetDataInfo : PageBase
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

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_data WHERE id = {0}", new string[] { "@id" }, new object[] { id });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Response.Write("");
                    return;
                }

                string data_id = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_id", 0));
                string data_description = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_description", 0));
                string data_type = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_type", 0));
                string data_version = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_version", 0));
                string data_size = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_size", 0));
                string data_link1 = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_link1", 0));
                string data_link2 = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_link2", 0));
                string data_link3 = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_link3", 0));
                string data_link4 = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_link4", 0));
                string data_link5 = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "data_link5", 0));

                string strItemFormat = "{{\"data_id\":\"{0}\", " +
                                "\"data_description\":\"{1}\", " +
                                "\"data_type\":\"{2}\", " +
                                "\"data_version\":\"{3}\", " +
                                "\"data_size\":\"{4}\", " +
                                "\"data_link1\":\"{5}\", " +
                                "\"data_link2\":\"{6}\", " +
                                "\"data_link3\":\"{7}\", " +
                                "\"data_link4\":\"{8}\", " +
                                "\"data_link5\":\"{9}\" " +
                                "}}";

                string strData = string.Format(strItemFormat, data_id, data_description, data_type, data_version, data_size, data_link1, data_link2, data_link3, data_link4, data_link5);

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