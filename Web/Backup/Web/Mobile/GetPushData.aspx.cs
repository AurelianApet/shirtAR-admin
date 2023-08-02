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
    public partial class GetPushData : MobilePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string query = "";
            string responseText = "";
            string push_id = Request.Params["push_id"];

            StringBuilder responseXml = new StringBuilder();

            try
            {
                if (string.IsNullOrEmpty(push_id))
                {
                    responseText = "예상치 못한 오류!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"2\" msg_content=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                query = string.Format("SELECT * FROM tbl_pushinfo WHERE id={0}", push_id);
                DataSet dsContent = DBConn.RunSelectQuery(query);
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//푸시메세지가 존재하지 않는다면
                    responseText = "존재하지 않음!";
                    responseXml.Clear();
                    responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    responseXml.Append("<response code=\"1\" msg_content=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                    responseXml.Append("</response>");

                    Response.Write(responseXml.ToString());
                    return;
                }

                string msg_content = Server.UrlEncode(DataSetUtil.RowStringValue(dsContent, "msg_content", 0));

                responseText = "푸시메세지가 존재함!";
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response code=\"0\" msg_content=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
            catch (Exception) 
            {
                responseText = "예상치 못한 오류!";
                responseXml.Clear();
                responseXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                responseXml.Append("<response code=\"2\" msg_content=\"\" text=\"" + Server.UrlEncode(responseText) + "\" >");
                responseXml.Append("</response>");

                Response.Write(responseXml.ToString());
                return;
            }
        }
    }
}