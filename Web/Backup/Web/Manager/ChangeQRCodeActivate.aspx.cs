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
    public partial class ChangeQRCodeActivate : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            string id = Request.Params["id"];
            string qrcode_id = Request.Params["qrcode_id"];

            try
            {
                DataSet dsContent = DBConn.RunSelectQuery("SELECT COUNT(*) AS activated_count FROM tbl_device WHERE qrcode_id = {0} and is_activated = 1", new string[] { "@qrcode_id" }, new object[] { Int32.Parse(qrcode_id) });
                int activated_count = DataSetUtil.RowIntValue(dsContent, "activated_count", 0);

                dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_qrcode WHERE id = {0}", new string[] { "@id" }, new object[] { Int32.Parse(qrcode_id) });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//QR코드번호가 존재하지 않을때
                    Response.Write("2");
                    return;
                }
                int apply_count = DataSetUtil.RowIntValue(dsContent, "apply_count", 0);

                int is_activate = 0;
                dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_device WHERE id={0}", new string[] { "@id" }, new object[] { Int32.Parse(id) });
                if (DataSetUtil.IsNullOrEmpty(dsContent))
                {//디바이스가 존재하지 않을때
                    Response.Write("3");
                    return;
                }
                if (DataSetUtil.RowIntValue(dsContent, "is_activated", 0) == 0)
                {//비활성화 되였다면
                    if (apply_count <= activated_count)
                    {//적용가능한 기기수 초과했을때
                        Response.Write("4");
                        return;
                    }
                    //활성화 하기
                    is_activate = 1;
                    DBConn.RunUpdateQuery("UPDATE tbl_device SET is_activated = 1 WHERE id = {0}", new string[] { "@id" }, new object[] { Int32.Parse(id) });
                }
                else
                {//활성화 되였다면
                    //비활성화 하기
                    is_activate = 0;
                    DBConn.RunUpdateQuery("UPDATE tbl_device SET is_activated = 0 WHERE id = {0}", new string[] { "@id" }, new object[] { Int32.Parse(id) });
                }

                Response.Write(is_activate);
                return;
            }
            catch (Exception ex) 
            {
                return;
            }
        }
    }
}