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
    public partial class QRCodeCreate : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            ddl_LoginID.Items.Clear();
            if (CurrentPage.AuthUser.ULevel == 1)
            {//최고관리자
                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin");
                for (int i = 0; i < DataSetUtil.RowCount(dsContent); i++)
                {
                    ddl_LoginID.Items.Add(new ListItem(DataSetUtil.RowStringValue(dsContent, "login_id", i), DataSetUtil.RowStringValue(dsContent, "qrcode_count", i)));
                }
            }
            else
            {//라이센스관리자
                DataSet dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin WHERE id={0}", new string[] { "@id" }, new object[] { CurrentPage.AuthUser.ID });
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    ddl_LoginID.Items.Add(new ListItem(DataSetUtil.RowStringValue(dsContent, "login_id", 0), DataSetUtil.RowStringValue(dsContent, "qrcode_count", 0)));
                }
            }

        }
    }
}