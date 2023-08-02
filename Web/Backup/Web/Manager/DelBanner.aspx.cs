using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using retroplus.Common;
using DataAccess;
using System.Data;

using System.IO;

namespace Web.Manager
{
    public partial class DelBanner : PageBase
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

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_banner WHERE id={0}", new string[] { "@id" }, new object[] { Int32.Parse(id) });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    Response.Write("1");
                    return;
                }
                //thumbnail 삭제
                string strPath = Server.MapPath(DataSetUtil.RowStringValue(dsContent, "path", 0));
                if (File.Exists(strPath))
                    File.Delete(strPath);
                DBConn.RunDeleteQuery("DELETE FROM tbl_banner WHERE id = {0}", new string[] { "@id" }, new object[] { Int32.Parse(id) });
                Response.Write("0");
                return;
            }
            catch (Exception) 
            {
                Response.Write("1");
                return;
            }
        }
    }
}