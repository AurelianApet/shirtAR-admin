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
    public partial class Statistics : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            string query = "";
            string sub_query = "";

            DataSet dsContent = null;

            if (!IsPostBack)
            {
                select_login_id.Items.Clear();
                select_login_id.Items.Add(new ListItem(Resources.Lang.STR_ALL, "0"));
                dsContent = DBConn.RunSelectQuery("SELECT * FROM tbl_admin");
                if (!DataSetUtil.IsNullOrEmpty(dsContent))
                {
                    for (int i = 0; i < DataSetUtil.RowCount(dsContent); i++)
                    {
                        select_login_id.Items.Add(new ListItem(DataSetUtil.RowStringValue(dsContent, "login_id", i), DataSetUtil.RowStringValue(dsContent, "id", i)));
                    }
                }
                select_login_id.Items[0].Selected = true;

                select_product_type.Items.Clear();
                select_product_type.Items.Add(new ListItem(Resources.Lang.STR_ALL, "0"));
                select_product_type.Items.Add(new ListItem(Resources.Lang.STR_REAL_PRODUCT, "1"));
                select_product_type.Items.Add(new ListItem(Resources.Lang.STR_TEST_PRODUCT, "2"));
                select_product_type.Items[0].Selected = true;

                hd_curdate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                hd_type.Value = "1";
            }

            string login_id = select_login_id.SelectedItem.Value;
            string product_type = select_product_type.SelectedItem.Value;

            lblStatTable.Text = "";
            lblStatTable.Text += "<table id=\"tblStatData\" width=\"1000\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\">";
            lblStatTable.Text += "<tr height=\"30\" class=\"clsGridHeader\">";
            lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_MONTH_DAY_TIME + "</td>";
            lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_CREATE_NUMBER + "</td>";
            lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_AUTH_NUMBER + "</td>";
            lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_USED_QRCODE_NUMBER + "</td>";
            lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_ALL_REG_DEVICE_NUMBER + "</td>";
            lblStatTable.Text += "</tr>";

            if (login_id != "0")
            {//발급인이 전체가 아닌 경우
                sub_query = " AND tbl_qrcode.admin_id=" + login_id;
            }

            if (product_type != "0")
            {//제품타입이 전체가 아닌 경우
                sub_query += " AND tbl_qrcode.product_type=" + product_type; 
            }

            if (hd_type.Value == "1")
            {//년간통계
                int year = DateTime.Parse(hd_curdate.Value).Year;
                int month = DateTime.Parse(hd_curdate.Value).Month;
                DateTime dt = DateTime.Parse(year + "-01-01");

                int create_number_sum = 0;
                int auth_number_sum = 0;
                int used_qrcode_number_sum = 0;
                int all_reg_device_number_sum = 0;

                for (int i = 0; i < 12; i++)
                {
                    string cur_month = dt.AddMonths(i).ToString("yyyy-MM");

                    //생성개수
                    int create_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), reg_date, 20) LIKE '%{0}%')", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        create_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    create_number_sum += create_number;
                    
                    //발급개수
                    int auth_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), auth_date, 20) LIKE '%{0}%') AND qrcode_type=1", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        auth_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    auth_number_sum += auth_number;

                    //사용된QR코드수
                    int used_qrcode_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), auth_date, 20) LIKE '%{0}%') AND qrcode_type=1 AND is_use=1", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        used_qrcode_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    used_qrcode_number_sum += used_qrcode_number;

                    //전체기기등록수
                    int all_reg_device_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_device WHERE (CONVERT(VARCHAR(19), reg_date, 20) LIKE '%{0}%')", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        all_reg_device_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    all_reg_device_number_sum += all_reg_device_number;

                    lblStatTable.Text += "<tr height=\"25\">";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + (i+1) + Resources.Lang.STR_MONTH + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + create_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + auth_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + used_qrcode_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + all_reg_device_number + "</td>";
                    lblStatTable.Text += "</tr>";
                }

                lblStatTable.Text += "<tr height=\"30\" class=\"clsGridHeader\">";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_SUM + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + create_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + auth_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + used_qrcode_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + all_reg_device_number_sum + "</td>";
                lblStatTable.Text += "</tr>";

                lblStatTable.Text += "<table>";
            }
            else if (hd_type.Value == "2")
            {//월간통계 
                int year = DateTime.Parse(hd_curdate.Value).Year;
                int month = DateTime.Parse(hd_curdate.Value).Month;
                DateTime dt = DateTime.Parse(year + "-" + month + "-01");

                int create_number_sum = 0;
                int auth_number_sum = 0;
                int used_qrcode_number_sum = 0;
                int all_reg_device_number_sum = 0;

                int i = 0;
                while (dt.AddDays(i).Month == month)
                {
                    string cur_month = dt.AddDays(i).ToString("yyyy-MM-dd");

                    //생성개수
                    int create_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), reg_date, 20) LIKE '%{0}%')", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        create_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    create_number_sum += create_number;

                    //발급개수
                    int auth_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), auth_date, 20) LIKE '%{0}%') AND qrcode_type=1", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        auth_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    auth_number_sum += auth_number;

                    //사용된QR코드수
                    int used_qrcode_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), auth_date, 20) LIKE '%{0}%') AND qrcode_type=1 AND is_use=1", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        used_qrcode_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    used_qrcode_number_sum += used_qrcode_number;

                    //전체기기등록수
                    int all_reg_device_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_device WHERE (CONVERT(VARCHAR(19), reg_date, 20) LIKE '%{0}%')", cur_month);
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        all_reg_device_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    all_reg_device_number_sum += all_reg_device_number;

                    lblStatTable.Text += "<tr height=\"25\">";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + (i + 1) + Resources.Lang.STR_DAY + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + create_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + auth_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + used_qrcode_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + all_reg_device_number + "</td>";
                    lblStatTable.Text += "</tr>";

                    i++;
                }

                lblStatTable.Text += "<tr height=\"30\" class=\"clsGridHeader\">";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_SUM + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + create_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + auth_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + used_qrcode_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + all_reg_device_number_sum + "</td>";
                lblStatTable.Text += "</tr>";

                lblStatTable.Text += "<table>";
            }
            else if (hd_type.Value == "3")
            {//일간통계
                int year = DateTime.Parse(hd_curdate.Value).Year;
                int month = DateTime.Parse(hd_curdate.Value).Month;
                int day = DateTime.Parse(hd_curdate.Value).Day;
                DateTime dt = DateTime.Parse(year + "-" + month + "-" + day + " 00:00:00");

                int create_number_sum = 0;
                int auth_number_sum = 0;
                int used_qrcode_number_sum = 0;
                int all_reg_device_number_sum = 0;

                for (int i = 0; i < 24; i++)
                {
                    string cur_month = dt.AddHours(i).ToString("yyyy-MM-dd HH");

                    //생성개수
                    int create_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), reg_date, 20) LIKE '%{0}%')", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        create_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    create_number_sum += create_number;

                    //발급개수
                    int auth_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), auth_date, 20) LIKE '%{0}%') AND qrcode_type=1", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        auth_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    auth_number_sum += auth_number;

                    //사용된QR코드수
                    int used_qrcode_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_qrcode WHERE (CONVERT(VARCHAR(19), auth_date, 20) LIKE '%{0}%') AND qrcode_type=1 AND is_use=1", cur_month);
                    query += sub_query;
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        used_qrcode_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    used_qrcode_number_sum += used_qrcode_number;

                    //전체기기등록수
                    int all_reg_device_number = 0;
                    query = string.Format("SELECT COUNT(*) FROM tbl_device WHERE (CONVERT(VARCHAR(19), reg_date, 20) LIKE '%{0}%')", cur_month);
                    dsContent = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsContent))
                    {
                        all_reg_device_number = DataSetUtil.RowIntValue(dsContent, 0, 0);
                    }
                    all_reg_device_number_sum += all_reg_device_number;

                    lblStatTable.Text += "<tr height=\"25\">";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + (i) + Resources.Lang.STR_HOUR + "~" + (i + 1) + Resources.Lang.STR_HOUR + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + create_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + auth_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + used_qrcode_number + "</td>";
                    lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"font-size:10pt; border:1px solid #e5e5e5; border-top-style:none;\">" + all_reg_device_number + "</td>";
                    lblStatTable.Text += "</tr>";
                }

                lblStatTable.Text += "<tr height=\"30\" class=\"clsGridHeader\">";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + Resources.Lang.STR_SUM + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + create_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + auth_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + used_qrcode_number_sum + "</td>";
                lblStatTable.Text += "<td align=\"center\" width=\"200\" style=\"border:1px solid #e5e5e5;\">" + all_reg_device_number_sum + "</td>";
                lblStatTable.Text += "</tr>";

                lblStatTable.Text += "<table>";
            }
            base.Page_Load(sender, e);
        }

        protected override void LoadData()
        {
            base.LoadData();
        }

        protected override void BindData()
        {
            base.BindData();
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {
            PageDataSource = null;
            BindData();
            return;
        }
    }
}