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
    public partial class Create : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected string getRandomSerialKey()
        {
            int rnum = 0;
            string serial_key = null;
            System.Random ranNum = new System.Random();
            //for (int i = 1; i < 20; i++)
            for (int i = 1; i < 15; i++)
            {
                if (i % 5 == 0)
                {
                    serial_key += '-';
                    continue;
                }
                for (int j = 48; j <= 122; j++)
                {
                    rnum = ranNum.Next(48, 123);
                    //if (rnum >= 48 && rnum <= 122 && (rnum <= 57 || rnum >= 65) && (rnum <= 90 || rnum >= 97))
                    if (rnum >= 48 && rnum <= 122 && (rnum <= 57 || rnum >= 65) && (rnum <= 90 || rnum >= 123))
                    {
                        break;
                    }
                }
                serial_key += Convert.ToChar(rnum);
            }
            return serial_key;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            try
            {
                string login_id = Request.Params["id"];

                int apply_count = 0;
                if (Int32.Parse(Request.Params["apply_count"]) > 0)
                    apply_count = Int32.Parse(Request.Params["apply_count"]);

                int product_type = 0;
                if (Int32.Parse(Request.Params["product_type"]) > 0)
                    product_type = Int32.Parse(Request.Params["product_type"]);

                int qrcode_count = 0;
                int id = 0;

                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin WHERE login_id={0}", new string[] { "@login_id" }, new object[] { login_id });
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    qrcode_count = DataSetUtil.RowIntValue(dsContent, "qrcode_count", 0);
                    id = DataSetUtil.RowIntValue(dsContent, "id", 0);
                }

                for (int i = 0; i < Int32.Parse(Request.Params["process_count"]); i++) 
                {
                    int j = 0;
                    while (j < qrcode_count)
                    {
                        string qrcode_number = getRandomSerialKey();

                        dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_qrcode WHERE qrcode_number={0}", new string[] { "@qrcode_number" }, new object[] { qrcode_number });
                        if (!DataSetUtil.IsNullOrEmpty(dsContent))
                        {
                            continue;
                        }

                        DBConn.RunInsertQuery("INSERT INTO tbl_qrcode (admin_id, qrcode_number, apply_count, product_type, qrcode_type, auth_date, reg_date) VALUES({0}, {1}, {2}, {3}, 0, GETDATE(), GETDATE())",
                            new string[] { "@admin_id", "@qrcode_number", "@apply_count", "@product_type" },
                            new object[] { id, qrcode_number, apply_count, product_type });

                        j++;
                    }
                }

                //로그
                string str_serialcount = qrcode_count + Resources.Lang.STR_GAE;
                string str_product = "";
                if(product_type == 1){
                    str_product = Resources.Lang.STR_REAL_PRODUCT;
                }else if(product_type == 2){
                    str_product = Resources.Lang.STR_TEST_PRODUCT;
                }

                DBConn.RunInsertQuery("INSERT INTO tbl_log (admin_id, event, qrcode_count, product_type, reg_date) VALUES({0}, {1}, {2}, {3}, GETDATE())",
                    new string[] { "@admin_id", "@event", "@qrcode_count", "@product_type" },
                    new object[] { AuthUser.ID, Resources.Lang.STR_QRCODE_CREATE, str_serialcount, str_product });

                //로그

                Response.Write("0");
                return;
            }
            catch (Exception ex) 
            {
                return;
            }
        }
    }
}