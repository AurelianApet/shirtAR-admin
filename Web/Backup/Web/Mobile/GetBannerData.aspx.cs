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
    public partial class GetBannerData : MobilePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string query = "";
            StringBuilder responseXml = new StringBuilder();
            try
            {
                query = string.Format("SELECT * FROM tbl_banner WHERE is_activate = 1 ORDER BY reg_date DESC");
                DataSet dsContent = DBConn.RunSelectQuery(query);
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//배너가 존재하지 않는다면
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response>");
                    responseXml.Append("</response>");
                    Response.Write(responseXml.ToString());
                    return;
                }

                string path = "";
                string link = "";

                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response>");
                for (int i = 0; i < DataSetUtil.RowCount(dsContent); i++)
                {
                    path = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "path", i));
                    link = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "link", i));
                    responseXml.Append("<banner code=\"0\" path=\"" + path + "\" link=\"" + link + "\"/>");
                }

                responseXml.Append("</response>");
                Response.Write(responseXml.ToString());
                return;
            }
            catch (Exception) 
            {
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