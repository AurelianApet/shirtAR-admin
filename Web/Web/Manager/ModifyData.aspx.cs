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
    public partial class ModifyData : PageBase
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
                string data_version = Server.UrlDecode(Request.Params["data_version"]);
                long data_size = string.IsNullOrEmpty(Server.UrlDecode(Request.Params["data_size"])) ? 0 : long.Parse(Server.UrlDecode(Request.Params["data_size"]));
                string data_description = Server.UrlDecode(Request.Params["data_description"]);
                string data_link1 = Server.UrlDecode(Request.Params["data_link1"]);
                string data_link2 = Server.UrlDecode(Request.Params["data_link2"]);
                string data_link3 = Server.UrlDecode(Request.Params["data_link3"]);
                string data_link4 = Server.UrlDecode(Request.Params["data_link4"]);
                string data_link5 = Server.UrlDecode(Request.Params["data_link5"]);

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(data_version) || string.IsNullOrEmpty(data_link1))
                {
                    Response.Write("2");
                    return;
                }

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_data WHERE id={0}", new string[] { "@id" }, new object[] { id });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//수정할 데이터가 존재하지 않으면
                    Response.Write("1");
                    return;
                }

                int data_type = DataSetUtil.RowIntValue(dsContent, "data_type", 0);

                //업로드타입에 따르는 파일 변경
                string destinationFile = "";
                if (data_type == 0)
                {//안드로이드
                    destinationFile = data_link1.Replace("/temp/", "/contents/android/");
                }
                else if (data_type == 1)
                {//IOS
                    destinationFile = data_link1.Replace("/temp/", "/contents/ios/");
                }

                if (destinationFile != DataSetUtil.RowStringValue(dsContent, "data_link1", 0))
                {
                    System.IO.File.Delete(Server.MapPath(DataSetUtil.RowStringValue(dsContent, "data_link1", 0)));
                    System.IO.File.Move(Server.MapPath(data_link1), Server.MapPath(destinationFile));
                }
                data_link1 = destinationFile;

                DBConn.RunUpdateQuery("UPDATE tbl_data SET data_description={0}, data_version={1}, data_size={2}, data_link1={3}, data_link2={4}, data_link3={5}, data_link4={6}, data_link5={7}, reg_date=GETDATE() WHERE id = {8}",
                    new string[] { "@data_description", "@data_version", "@data_size", "@data_link1", "@data_link2", "@data_link3", "@data_link4", "@data_link5", "@id" },
                    new object[] { data_description, data_version, data_size, data_link1, data_link2, data_link3, data_link4, data_link5, id });

                Response.Write("0");
                return;
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                Response.Write("2");
                return;
            }
        }
    }
}