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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Web.Manager
{
    public partial class AuthedQRCodeList : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        protected override GridView getGridControl()
        {
            return gvContent;
        }

        protected override void LoadData()
        {
            base.LoadData();
            string query = "";

            select_search_type.Items.Clear();

            string search_type = hd_search_type.Value;

            if (search_type == "0")
            {//전체
                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=1 ORDER BY auth_date DESC", AuthUser.ID);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[0].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=1 ORDER BY auth_date DESC");
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[0].Selected = true;
                }
            }
            else if (search_type == "1")
            {//발급인
                query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=1 and tbl_admin.login_id like '%{0}%' ORDER BY auth_date DESC", txt_searchkey.Text);
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                select_search_type.Items[0].Selected = true;
            }
            else if (search_type == "2")
            {//QR코드 번호
                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=1 and qrcode_number like '%{1}%' ORDER BY auth_date DESC", AuthUser.ID, txt_searchkey.Text);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[0].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=1 and qrcode_number like '%{0}%' ORDER BY auth_date DESC", txt_searchkey.Text);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[1].Selected = true;
                }
            }
            else if (search_type == "3")
            {//발급날자
                String dateStart = DateTime.Parse(txt_dateStart.Text).ToString("yyyy-MM-dd 00:00:00");
                String dateEnd = DateTime.Parse(txt_dateEnd.Text).ToString("yyyy-MM-dd 23:59:59");

                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=1 and CONVERT(varchar,auth_date,120) >= '{1}' and CONVERT(varchar,auth_date,120) <= '{2}' ORDER BY auth_date ASC", AuthUser.ID, dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[1].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=1 and CONVERT(varchar,auth_date,120) >= '{0}' and CONVERT(varchar,auth_date,120) <= '{1}' ORDER BY auth_date ASC", dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6")); 
                    select_search_type.Items[2].Selected = true;
                }
            }
            else if (search_type == "4")
            {//생성날자
                String dateStart = DateTime.Parse(txt_dateStart.Text).ToString("yyyy-MM-dd 00:00:00");
                String dateEnd = DateTime.Parse(txt_dateEnd.Text).ToString("yyyy-MM-dd 23:59:59");

                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=1 and CONVERT(varchar, tbl_qrcode.reg_date,120) >= '{1}' and CONVERT(varchar, tbl_qrcode.reg_date,120) <= '{2}' ORDER BY auth_date ASC", AuthUser.ID, dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[2].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=1 and CONVERT(varchar,tbl_qrcode.reg_date,120) >= '{0}' and CONVERT(varchar,tbl_qrcode.reg_date,120) <= '{1}' ORDER BY auth_date ASC", dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[3].Selected = true;
                }
            }
            else if (search_type == "5")
            {//최초 기기 등록날자
                String dateStart = DateTime.Parse(txt_dateStart.Text).ToString("yyyy-MM-dd 00:00:00");
                String dateEnd = DateTime.Parse(txt_dateEnd.Text).ToString("yyyy-MM-dd 23:59:59");

                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN (SELECT DISTINCT qrcode_id FROM (SELECT * FROM tbl_device WHERE (CONVERT(varchar,reg_date,120) >= '{0}' and CONVERT(varchar,reg_date,120) <= '{1}')) AS a) AS b ON tbl_qrcode.id = b.qrcode_id INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE (tbl_qrcode.admin_id={2} and tbl_qrcode.qrcode_type = 1) ORDER BY tbl_qrcode.auth_date ASC", dateStart, dateEnd, AuthUser.ID);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[3].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN (SELECT DISTINCT qrcode_id FROM (SELECT * FROM tbl_device WHERE (CONVERT(varchar,reg_date,120) >= '{0}' and CONVERT(varchar,reg_date,120) <= '{1}')) AS a) AS b ON tbl_qrcode.id = b.qrcode_id INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE (tbl_qrcode.qrcode_type = 1) ORDER BY tbl_qrcode.auth_date ASC", dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[4].Selected = true;
                }
            }
            else if (search_type == "6")
            {//제품타입
                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=1 and product_type = {1} ORDER BY auth_date DESC", AuthUser.ID, ddl_productType.SelectedItem.Value);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[4].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=1 and product_type = {0} ORDER BY auth_date DESC", ddl_productType.SelectedItem.Value);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_AUTH_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "4"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_FIRST_DEVICE_REG_DATE, "5"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "6"));
                    select_search_type.Items[5].Selected = true;
                }
            }
            PageDataSource = DBConn.RunSelectQuery(query);
        }

        protected override void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            base.gvContent_RowDataBound(sender, e);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //QR코드ID
                string id = dr["id"].ToString();
                //QR코드발급날자
                string authdate = dr["auth_date"].ToString();
                //QR코드생성날자
                string regdate = dr["reg_date"].ToString();
                //발급인
                string loginid = dr["login_id"].ToString();
                //QR코드번호
                string qrcodenumber = dr["qrcode_number"].ToString();
                //적용가능한 기기수량
                string applycount = dr["apply_count"].ToString();
                //제품타입
                string producttype = dr["product_type"].ToString();
                //output
                int output = int.Parse(dr["output"].ToString());

                Literal ltrCheckbox = (Literal)e.Row.FindControl("ltrCheckbox");
                Literal ltrAuthDate = (Literal)e.Row.FindControl("ltrAuthDate");
                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrLoginID = (Literal)e.Row.FindControl("ltrLoginID");
                Literal ltrQRCodeNumber = (Literal)e.Row.FindControl("ltrQRCodeNumber");
                Literal ltrStoryName = (Literal)e.Row.FindControl("ltrStoryName");
                Literal ltrApplyCount = (Literal)e.Row.FindControl("ltrApplyCount");
                Literal ltrProductType = (Literal)e.Row.FindControl("ltrProductType");
                Literal ltrDeviceRegDate = (Literal)e.Row.FindControl("ltrDeviceRegDate");
                Literal ltrRegDeviceCount = (Literal)e.Row.FindControl("ltrRegDeviceCount");
                Literal ltrSerialStyle = (Literal)e.Row.FindControl("ltrSerialStyle");
                Literal ltrDetail = (Literal)e.Row.FindControl("ltrDetail");

                ltrCheckbox.Text = "<input type=\"checkbox\" style=\"height: auto;\" name=\"chkNo\" value='" + id + "' />";
                ltrAuthDate.Text = DateTime.Parse(authdate).ToString("yyyy-MM-dd");
                ltrRegDate.Text = DateTime.Parse(regdate).ToString("yyyy-MM-dd");
                ltrLoginID.Text = loginid;
                if (output > 0)
                {
                    ltrQRCodeNumber.Text = "<a class=\"popper\" data-popbox=\"divQRCodePop\" style=\"color:red;\">" + qrcodenumber + "</a>";
                }
                else
                {
                    ltrQRCodeNumber.Text = "<a class=\"popper\" data-popbox=\"divQRCodePop\">" + qrcodenumber + "</a>";
                }

                ltrApplyCount.Text = applycount;

                //제품타입
                if (producttype == "1")
                {
                    ltrProductType.Text = Resources.Lang.STR_REAL_PRODUCT;
                }
                else if (producttype == "2")
                {
                    ltrProductType.Text = Resources.Lang.STR_TEST_PRODUCT;
                }

                //등록된 기기수 얻기
                string query = string.Format("SELECT COUNT(*) AS count FROM tbl_device WHERE qrcode_id = {0}", id);
                DataSet dsTemp = DBConn.RunSelectQuery(query);
                if (DataSetUtil.IsNullOrEmpty(dsTemp))
                {
                    ltrRegDeviceCount.Text = "0";
                }
                else
                {
                    ltrRegDeviceCount.Text = DataSetUtil.RowStringValue(dsTemp, "count", 0);
                }

                //기기의 최초등록날자얻기
                query = string.Format("SELECT reg_date FROM tbl_device WHERE qrcode_id = {0} ORDER BY reg_date ASC", id);
                dsTemp = DBConn.RunSelectQuery(query);
                if (DataSetUtil.IsNullOrEmpty(dsTemp))
                {
                    ltrDeviceRegDate.Text = "";
                }
                else
                {
                    ltrDeviceRegDate.Text = DateTime.Parse(DataSetUtil.RowStringValue(dsTemp, "reg_date", 0)).ToString("yyyy-MM-dd");
                }

                //상세
                ltrDetail.Text = "<input type=\"button\" class=\"btnSmallGray\" onclick=\"showAuthedQRCodeDetailPopup('" + id + "')\" value=\"" + Resources.Lang.STR_DETAIL + "\" style=\"width:58px; height:22px;\"/>";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            return;
        }
        
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (DataSetUtil.RowCount(PageDataSource) <= 0)
            {
                return;
            }

            string str_ids = hd_ids.Value;

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            int row_num = 1;

            if (!DataSetUtil.IsNullOrEmpty(PageDataSource))
            {
                if (str_ids == "all")
                {//전체 리스트 저장
                    for (int i = 0; i < DataSetUtil.RowCount(PageDataSource); i++)
                    {
                        workSheet.Cells[row_num, 1].Value = DataSetUtil.RowStringValue(PageDataSource, "qrcode_number", i);

                        //파일로 출하한 코드로 변경
                        DBConn.RunUpdateQuery("UPDATE tbl_qrcode SET output = 1 WHERE id={0}", new string[] { "@id" }, new object[] { DataSetUtil.RowIntValue(PageDataSource, "id", i) });
                        row_num++;
                    }
                }
                else
                {//현재 체크된 리스트만 저장
                    string[] ids = str_ids.Split(',');
                    for (int i = 0; i < ids.Length; i++)
                    {
                        DataSet dsTemp = DBConn.RunSelectQuery("SELECT * FROM tbl_qrcode WHERE id={0}", new string[] { "@id" }, new object[] { ids[i] });
                        workSheet.Cells[row_num, 1].Value = DataSetUtil.RowStringValue(dsTemp, "qrcode_number", 0);

                        //파일로 출하한 코드로 변경
                        DBConn.RunUpdateQuery("UPDATE tbl_qrcode SET output = 1 WHERE id={0}", new string[] { "@id" }, new object[] { ids[i] });
                        row_num++;
                    }
                }
            }

            workSheet.Cells.AutoFitColumns();

            using (var memoryStream = new MemoryStream())
            {
                Response.Clear();
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment;  filename=AuthedQRCodeList_" + CurrentDate + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return;
        }

    }
}