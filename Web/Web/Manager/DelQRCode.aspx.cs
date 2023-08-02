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
    public partial class DelQRCode : PageBase
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
                
                //로그
                string str_qrcodecount = ids.Length + Resources.Lang.STR_GAE;
                string str_product = "";
                for (int i = 0; i < ids.Length; i++)
                {
                    DataSet dsTemp = DBConn.RunSelectQuery("SELECT * FROM tbl_qrcode WHERE id = {0} and qrcode_type = 0", new string[] { "@id" }, new object[] { Int32.Parse(ids[i]) });
                    if (!DataSetUtil.IsNullOrEmpty(dsTemp))
                    {
                        if (DataSetUtil.RowIntValue(dsTemp, "product_type", 0) == 1)
                        {
                            if (str_product == "")
                            {
                                str_product = "실제품";
                                continue;
                            }
                            if (str_product == "실제품")
                                continue;
                            else
                                str_product = "실제품,테스트";
                        }else if(DataSetUtil.RowIntValue(dsTemp, "product_type", 0) == 2){
                            if (str_product == "")
                            {
                                str_product = "테스트";
                                continue;
                            }
                            if (str_product == "테스트")
                                continue;
                            else
                                str_product = "실제품,테스트";
                        }
                    }
                }
                //로그

                for (int i = 0; i < ids.Length; i++)
                {
                    DBConn.RunDeleteQuery("DELETE FROM tbl_qrcode WHERE id = {0} and qrcode_type = 0", new string[] { "@id" }, new object[] { Int32.Parse(ids[i]) });
                }

                //로그
                DBConn.RunInsertQuery("INSERT INTO tbl_log (admin_id, event, qrcode_count, product_type, reg_date) VALUES({0}, {1}, {2}, {3}, GETDATE())",
                    new string[] { "@admin_id", "@event", "@qrcode_count", "@product_type" },
                    new object[] { AuthUser.ID, Resources.Lang.STR_QRCODE_DELETE, str_qrcodecount, str_product });
                //로그

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