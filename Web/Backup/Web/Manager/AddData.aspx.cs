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
    public partial class AddData : PageBase
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
                string data_id = Server.UrlDecode(Request.Params["data_id"]);
                int data_type = string.IsNullOrEmpty(Server.UrlDecode(Request.Params["data_type"])) ? 0 : int.Parse(Server.UrlDecode(Request.Params["data_type"]));
                string data_version = Server.UrlDecode(Request.Params["data_version"]);
                long data_size = string.IsNullOrEmpty(Server.UrlDecode(Request.Params["data_size"])) ? 0 : long.Parse(Server.UrlDecode(Request.Params["data_size"]));
                string data_description = Server.UrlDecode(Request.Params["data_description"]);
                string data_link1 = Server.UrlDecode(Request.Params["data_link1"]);
                string data_link2 = Server.UrlDecode(Request.Params["data_link2"]);
                string data_link3 = Server.UrlDecode(Request.Params["data_link3"]);
                string data_link4 = Server.UrlDecode(Request.Params["data_link4"]);
                string data_link5 = Server.UrlDecode(Request.Params["data_link5"]);

                if (string.IsNullOrEmpty(data_id) || string.IsNullOrEmpty(data_version) || string.IsNullOrEmpty(data_link1))
                {
                    Response.Write("2");
                    return;
                }

                //데이터식별코드중복검사
                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_data WHERE data_id={0}", new string[] { "@data_id" }, new object[] { data_id });
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {//존재한다면
                    //템프에 림시보관된 zip파일 지우기
                    System.IO.File.Delete(Server.MapPath(data_link1));
                    Response.Write("1");
                    return;
                }

                string destinationFile = "";
                if (data_type == 0)
                {//안드로이드
                    destinationFile = data_link1.Replace("/temp/", "/contents/android/");
                    System.IO.File.Move(Server.MapPath(data_link1), Server.MapPath(destinationFile));
                    data_link1 = destinationFile;
                }
                else if (data_type == 1)
                {//IOS
                    destinationFile = data_link1.Replace("/temp/", "/contents/ios/");
                    System.IO.File.Move(Server.MapPath(data_link1), Server.MapPath(destinationFile));
                    data_link1 = destinationFile;                    
                }

                DBConn.RunInsertQuery("INSERT INTO tbl_data (data_id, data_description, data_type, data_version, data_size, data_link1, data_link2, data_link3, data_link4, data_link5, reg_date) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, GETDATE())",
                    new string[] { "@data_id", "@data_description", "@data_type", "@data_version", "@data_size", "@data_link1", "@data_link2", "@data_link3", "@data_link4", "@data_link5" },
                    new object[] { data_id, data_description, data_type, data_version, data_size, data_link1, data_link2, data_link3, data_link4, data_link5 });

                Response.Write("0");
                return;
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                //Response.Write("2");
                return;
            }
        }
    }
}