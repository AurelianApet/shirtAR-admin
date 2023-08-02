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
    public partial class GetDataInfo : MobilePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string query = "";

            StringBuilder responseXml = new StringBuilder();

            try
            {
                string device_type = Request.Params["device_type"];

                query = string.Format("SELECT * FROM tbl_data WHERE data_type={0} ORDER BY reg_date DESC", new string[] { "@data_type" }, new object[]{ device_type });
                DataSet dsContent = DBConn.RunSelectQuery(query);
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
                for (int i = 0; i < DataSetUtil.RowCount(dsContent); i++)
                {
                    string data_id = DataSetUtil.RowStringValue(dsContent, "data_id", i);
                    string data_description = DataSetUtil.RowStringValue(dsContent, "data_description", i);
                    string data_type = DataSetUtil.RowStringValue(dsContent, "data_type", i);
                    string data_version = DataSetUtil.RowStringValue(dsContent, "data_version", i);
                    string data_size =  DataSetUtil.RowStringValue(dsContent, "data_size", i);
                    string data_link1 = Defines.URL_DOMAIN + DataSetUtil.RowStringValue(dsContent, "data_link1", i);
                    string data_link2 = DataSetUtil.RowStringValue(dsContent, "data_link2", i);
                    string data_link3 = DataSetUtil.RowStringValue(dsContent, "data_link3", i);
                    string data_link4 = DataSetUtil.RowStringValue(dsContent, "data_link4", i);
                    string data_link5 = DataSetUtil.RowStringValue(dsContent, "data_link5", i);

                    responseXml.Append(string.Format("<data data_id=\"{0}\" data_description=\"{1}\" data_type=\"{2}\" data_type=\"{3}\" data_version=\"{4}\" data_size=\"{5}\" data_link1=\"{6}\" data_link2=\"{7}\" data_link3=\"{8}\" data_link4=\"{9}\" data_link5=\"{10}\" />",
                            Server.UrlEncode(data_id),
                            Server.UrlEncode(data_description),
                            Server.UrlEncode(data_type),
                            Server.UrlEncode(data_version),
                            Server.UrlEncode(data_size),
                            Server.UrlEncode(data_link1),
                            Server.UrlEncode(data_link2),
                            Server.UrlEncode(data_link3),
                            Server.UrlEncode(data_link4),
                            Server.UrlEncode(data_link5)
                            ));
                }
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