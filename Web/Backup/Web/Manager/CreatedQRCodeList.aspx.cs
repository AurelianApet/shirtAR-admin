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
    public partial class CreatedQRCodeList : PageBase
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
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=0 ORDER BY tbl_qrcode.reg_date DESC", AuthUser.ID);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[0].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=0 ORDER BY tbl_qrcode.reg_date DESC");
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[0].Selected = true;
                }
            }
            else if (search_type == "1")
            {//발급인
                query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=0 and tbl_admin.login_id like '%{0}%' ORDER BY tbl_qrcode.reg_date DESC", txt_searchkey.Text);
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                select_search_type.Items[0].Selected = true;
            }
            else if (search_type == "2")
            {//QR코드 번호
                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=0 and qrcode_number like '%{1}%' ORDER BY tbl_qrcode.reg_date DESC", AuthUser.ID, txt_searchkey.Text);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[0].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=0 and qrcode_number like '%{0}%' ORDER BY tbl_qrcode.reg_date DESC", txt_searchkey.Text);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[1].Selected = true;
                }
            }
            else if (search_type == "3")
            {//생성날자
                String dateStart = DateTime.Parse(txt_dateStart.Text).ToString("yyyy-MM-dd 00:00:00");
                String dateEnd = DateTime.Parse(txt_dateEnd.Text).ToString("yyyy-MM-dd 23:59:59");

                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=0 and CONVERT(varchar,tbl_qrcode.reg_date,120) >= '{1}' and CONVERT(varchar,tbl_qrcode.reg_date,120) <= '{2}' ORDER BY tbl_qrcode.reg_date DESC", AuthUser.ID, dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[1].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=0 and CONVERT(varchar,tbl_qrcode.reg_date,120) >= '{0}' and CONVERT(varchar,tbl_qrcode.reg_date,120) <= '{1}' ORDER BY tbl_qrcode.reg_date DESC", dateStart, dateEnd);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[2].Selected = true;
                }           
            }
            else if (search_type == "4")
            {//제품타입
                if (AuthUser.ULevel == 0)
                {//라이센스관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE admin_id={0} and qrcode_type=0 and product_type = {1} ORDER BY auth_date DESC", AuthUser.ID, ddl_productType.SelectedItem.Value);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[2].Selected = true;
                }
                else if (AuthUser.ULevel == 1)
                {//최고관리자
                    query = string.Format("SELECT tbl_qrcode.*, tbl_admin.login_id FROM tbl_qrcode INNER JOIN tbl_admin ON tbl_qrcode.admin_id = tbl_admin.id WHERE qrcode_type=0 and product_type = {0} ORDER BY auth_date DESC", ddl_productType.SelectedItem.Value);
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATOR, "1"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_QRCODE_NUMBER, "2"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_CREATE_DATE, "3"));
                    select_search_type.Items.Add(new ListItem(Resources.Lang.STR_PRODUCT_TYPE, "4"));
                    select_search_type.Items[3].Selected = true;
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
                //발급인
                string loginid = dr["login_id"].ToString();
                //시리얼번호
                string qrcodenumber = dr["qrcode_number"].ToString();
                //적용가능한 기기수량
                string applycount = dr["apply_count"].ToString();
                //생성날자
                string regdate = dr["reg_date"].ToString();
                //제품타입
                string producttype = dr["product_type"].ToString();

                Literal ltrCheckbox = (Literal)e.Row.FindControl("ltrCheckbox");
                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrLoginID = (Literal)e.Row.FindControl("ltrLoginID");
                Literal ltrQRCodeNumber = (Literal)e.Row.FindControl("ltrQRCodeNumber");
                Literal ltrApplyCount = (Literal)e.Row.FindControl("ltrApplyCount");
                Literal ltrProductType = (Literal)e.Row.FindControl("ltrProductType");
                Literal ltrModify = (Literal)e.Row.FindControl("ltrModify");

                ltrCheckbox.Text = "<input type=\"checkbox\" style=\"height: auto;\" name=\"chkNo\" value='" + id + "' />";
                ltrRegDate.Text = DateTime.Parse(regdate).ToString("yyyy-MM-dd");
                ltrLoginID.Text = loginid;
                ltrQRCodeNumber.Text = "<a class=\"popper\" data-popbox=\"divQRCodePop\">" + qrcodenumber + "</a>";
                ltrApplyCount.Text = applycount;

                if (producttype == "1")
                {
                    ltrProductType.Text = Resources.Lang.STR_REAL_PRODUCT;
                }
                else if (producttype == "2")
                {
                    ltrProductType.Text = Resources.Lang.STR_TEST_PRODUCT;
                }

                ltrModify.Text = "<input type=\"button\" class=\"btnSmallGray\" onclick=\"showModifyQRCodePopup('" + id + "')\" value=\"" + Resources.Lang.STR_CHANGE + "\" style=\"width:58px; height:22px;\"/>";
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
            
            int pos1 = 0;
            int pos2 = 0;
            if (hd_saveExcelType.Value == "1")
            {//현재 표시된 리스트만 저장
                pos1 = PageNumber * PAGE_ROWS;
                pos2 = PageNumber * PAGE_ROWS + PAGE_ROWS;
                if (pos2 > DataSetUtil.RowCount(PageDataSource))
                    pos2 = DataSetUtil.RowCount(PageDataSource);
            }
            else if (hd_saveExcelType.Value == "2")
            {//전체 리스트 저장
                pos1 = 0;
                pos2 = DataSetUtil.RowCount(PageDataSource);
            }

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            workSheet.Cells[1, 1].Value = "No";
            workSheet.Cells[1, 2].Value = Resources.Lang.STR_CREATE_DATE;
            workSheet.Cells[1, 3].Value = Resources.Lang.STR_CREATOR;
            workSheet.Cells[1, 4].Value = Resources.Lang.STR_QRCODE_NUMBER;
            workSheet.Cells[1, 5].Value = Resources.Lang.STR_APPLY_COUNT;
            workSheet.Cells[1, 6].Value = Resources.Lang.STR_PRODUCT_TYPE;

            if (!DataSetUtil.IsNullOrEmpty(PageDataSource))
            {
                int row_num = 2;
                for (int i = pos1; i < pos2; i++)
                {
                    string product_type = "";
                    if (DataSetUtil.RowIntValue(PageDataSource, "product_type", i) == 1)
                        product_type = Resources.Lang.STR_REAL_PRODUCT;
                    else if (DataSetUtil.RowIntValue(PageDataSource, "product_type", i) == 2)
                        product_type = Resources.Lang.STR_TEST_PRODUCT;

                    string query = string.Format("SELECT COUNT(*) AS count FROM tbl_device WHERE qrcode_id = {0}", DataSetUtil.RowIntValue(PageDataSource, "id", i));
                    DataSet dsTemp = DBConn.RunSelectQuery(query);
                    int device_count = 0;
                    if (!DataSetUtil.IsNullOrEmpty(dsTemp))
                    {
                        device_count = DataSetUtil.RowIntValue(dsTemp, "count", 0);
                    }

                    //기기의 최초등록날짜얻기
                    query = string.Format("SELECT reg_date FROM tbl_device WHERE qrcode_id = {0} ORDER BY reg_date ASC", DataSetUtil.RowIntValue(PageDataSource, "id", i));
                    dsTemp = DBConn.RunSelectQuery(query);
                    string device_reg_date = "";
                    if (!DataSetUtil.IsNullOrEmpty(dsTemp))
                    {
                        device_reg_date = DateTime.Parse(DataSetUtil.RowStringValue(dsTemp, "reg_date", 0)).ToString("yyyy-MM-dd");
                    }

                    workSheet.Cells[row_num, 1].Value = (i + 1);
                    workSheet.Cells[row_num, 2].Value = DateTime.Parse(DataSetUtil.RowStringValue(PageDataSource, "reg_date", i)).ToString("yyyy-MM-dd");
                    workSheet.Cells[row_num, 3].Value = DataSetUtil.RowStringValue(PageDataSource, "login_id", i);
                    workSheet.Cells[row_num, 4].Value = DataSetUtil.RowStringValue(PageDataSource, "qrcode_number", i);
                    workSheet.Cells[row_num, 5].Value = DataSetUtil.RowIntValue(PageDataSource, "apply_count", i);
                    workSheet.Cells[row_num, 6].Value = product_type;

                    row_num++;
                }
            }

            workSheet.Cells.AutoFitColumns();

            using (var memoryStream = new MemoryStream())
            {
                Response.Clear();
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment;  filename=CreatedQRCodeList_" + CurrentDate + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            return;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string query = "";

            //로그
            string str_qrcodecount = "";
            string str_product = "";
            //로그

            string str_ids = hd_ids.Value;

            if (string.IsNullOrEmpty(str_ids))
            {
                Alert("비정상적인 요청입니다.", "/Manager/Logout.aspx");
                return;
            }

            if (str_ids == "all")
            {//전체 리스트 판매

                //로그
                str_qrcodecount = DataSetUtil.RowCount(PageDataSource) + Resources.Lang.STR_GAE;
                for (int i = 0; i < DataSetUtil.RowCount(PageDataSource); i++)
                {
                    query = string.Format("SELECT * FROM tbl_qrcode WHERE id={0} and qrcode_type = 0", DataSetUtil.RowIntValue(PageDataSource, "id", i));
                    DataSet dsTemp = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsTemp))
                    {
                        if (DataSetUtil.RowIntValue(dsTemp, "product_type", 0) == 1)
                        {
                            if (str_product == "")
                            {
                                str_product = Resources.Lang.STR_TEST_PRODUCT;
                                continue;
                            }
                            if (str_product == Resources.Lang.STR_REAL_PRODUCT)
                                continue;
                            else
                                str_product = Resources.Lang.STR_REAL_PRODUCT + "," + Resources.Lang.STR_TEST_PRODUCT;
                        }
                        else if (DataSetUtil.RowIntValue(dsTemp, "product_type", 0) == 2)
                        {
                            if (str_product == "")
                            {
                                str_product = Resources.Lang.STR_REAL_PRODUCT;
                                continue;
                            }
                            if (str_product == Resources.Lang.STR_TEST_PRODUCT)
                                continue;
                            else
                                str_product = Resources.Lang.STR_REAL_PRODUCT + "," + Resources.Lang.STR_TEST_PRODUCT;
                        }
                    }
                }
                //로그

                for (int i = 0; i < DataSetUtil.RowCount(PageDataSource); i++)
                {
                    query = string.Format("UPDATE tbl_qrcode SET qrcode_type = 1, auth_date = GETDATE() WHERE id={0}", DataSetUtil.RowIntValue(PageDataSource, "id", i));
                    DBConn.RunUpdateQuery(query);
                }
            }
            else
            {//현재 체크된 리스트만 판매
                string[] ids = str_ids.Split(',');

                //로그
                str_qrcodecount = ids.Length + Resources.Lang.STR_GAE;
                for (int i = 0; i < ids.Length; i++)
                {
                    query = string.Format("SELECT * FROM tbl_qrcode WHERE id={0} and qrcode_type = 0", Int32.Parse(ids[i]));
                    DataSet dsTemp = DBConn.RunSelectQuery(query);
                    if (!DataSetUtil.IsNullOrEmpty(dsTemp))
                    {
                        if (DataSetUtil.RowIntValue(dsTemp, "product_type", 0) == 1)
                        {
                            if (str_product == "")
                            {
                                str_product = Resources.Lang.STR_REAL_PRODUCT;
                                continue;
                            }
                            if (str_product == Resources.Lang.STR_REAL_PRODUCT)
                                continue;
                            else
                                str_product = Resources.Lang.STR_REAL_PRODUCT + "," + Resources.Lang.STR_TEST_PRODUCT;
                        }
                        else if (DataSetUtil.RowIntValue(dsTemp, "product_type", 0) == 2)
                        {
                            if (str_product == "")
                            {
                                str_product = Resources.Lang.STR_TEST_PRODUCT;
                                continue;
                            }
                            if (str_product == Resources.Lang.STR_TEST_PRODUCT)
                                continue;
                            else
                                str_product = Resources.Lang.STR_REAL_PRODUCT + "," + Resources.Lang.STR_TEST_PRODUCT;
                        }
                    }
                }
                //로그

                for (int i = 0; i < ids.Length; i++)
                {
                    query = string.Format("UPDATE tbl_qrcode SET qrcode_type = 1, auth_date = GETDATE() WHERE id={0}", Int32.Parse(ids[i]));
                    DBConn.RunUpdateQuery(query);
                }
            }

            //로그
            DBConn.RunInsertQuery("INSERT INTO tbl_log (admin_id, event, qrcode_count, product_type, reg_date) VALUES({0}, {1}, {2}, {3}, GETDATE())",
                    new string[] { "@admin_id", "@event", "@qrcode_count", "@product_type" },
                    new object[] { AuthUser.ID, Resources.Lang.STR_QRCODE_AUTH, str_qrcodecount, str_product });
            //로그

            Alert(Resources.Lang.MSG_QRCODE_AUTH_SUCCESS, "/Manager/AuthedQRCodeList.aspx");
            return;
        }

    }
}