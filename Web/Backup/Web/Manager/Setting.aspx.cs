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
    public partial class Setting : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            DataSet dsContent = DBConn.RunSelectQuery("SELECT TOP(1) * FROM tbl_config");
            if (!DataSetUtil.IsNullOrEmpty(dsContent))
            {
                connect_cycle.Value = DataSetUtil.RowStringValue(dsContent, "connect_cycle", 0);
                lang.Value = DataSetUtil.RowStringValue(dsContent, "lang", 0);
                count_per_page.Value = DataSetUtil.RowStringValue(dsContent, "count_per_page", 0);
            }
            base.Page_Load(sender, e);
        }
    }
}