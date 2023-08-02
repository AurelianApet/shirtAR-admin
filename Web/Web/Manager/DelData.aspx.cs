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
    public partial class DelData : PageBase
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
                string[] ids = Server.UrlDecode(Request.Params["ids"]).Split(',');

                for (int i = 0; i < ids.Length; i++)
                {
                    DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_data WHERE id={0}", new string[] { "@id" }, new object[] { Int32.Parse(ids[i]) });
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        //데이터파일 삭제
                        string strPath = Server.MapPath(DataSetUtil.RowStringValue(dsContent, "data_link1", 0));
                        if (File.Exists(strPath))
                            File.Delete(strPath);

                        DBConn.RunDeleteQuery("DELETE FROM tbl_data WHERE id = {0}", new string[] { "@id" }, new object[] { Int32.Parse(ids[i]) });
                    }
                }

                Response.Write("0");
                return;
            }
            catch (Exception) 
            {
                return;
            }
        }
    }
}