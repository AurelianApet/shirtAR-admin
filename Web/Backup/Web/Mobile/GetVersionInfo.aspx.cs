using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;
using DataAccess;
using System.Data;
using System.Text;

namespace Web.Mobile
{
    public partial class GetVersionInfo : MobilePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder responseXml = new StringBuilder();

            try
            {
                DataSet dsContent = DBConn.RunSelectQuery("SELECT TOP(1) * FROM tbl_update ORDER BY version DESC");
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//데이터가 존재하지 않는다면
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response>");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response>");
                responseXml.Append(string.Format("<version value=\"{0}\" />", Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "version", 0))));
                responseXml.Append("</response>");
                Response.Write(responseXml.ToString());
                return;

            }
            catch (Exception)
            {//예상치 못한 오류
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response>");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
        }
    }
}