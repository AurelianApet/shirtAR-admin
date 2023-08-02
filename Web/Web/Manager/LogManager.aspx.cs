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
    public partial class LogManager : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.AuthUser.ULevel != 1)
            {
                Alert(Resources.Lang.MSG_ADMIN_ACEPT_ONLY, "/Manager/Logout.aspx");
                return;
            }
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
                query = string.Format("SELECT tbl_log.*, tbl_admin.login_id, tbl_admin.nickname FROM tbl_log INNER JOIN tbl_admin ON tbl_log.admin_id = tbl_admin.id ORDER BY tbl_log.reg_date DESC");
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_NICKNAME, "1"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_LOGINID, "2"));
                select_search_type.Items[0].Selected = true;
            }
            else if (search_type == "1")
            {//등록자명
                query = string.Format("SELECT tbl_log.*, tbl_admin.login_id, tbl_admin.nickname FROM tbl_log INNER JOIN tbl_admin ON tbl_log.admin_id = tbl_admin.id WHERE tbl_admin.nickname like '%{0}%' ORDER BY tbl_log.reg_date DESC", txt_searchkey.Text);
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_NICKNAME, "1"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_LOGINID, "2"));
                select_search_type.Items[0].Selected = true;
            }
            else if (search_type == "2")
            {//등록자ID
                query = string.Format("SELECT tbl_log.*, tbl_admin.login_id, tbl_admin.nickname FROM tbl_log INNER JOIN tbl_admin ON tbl_log.admin_id = tbl_admin.id WHERE tbl_admin.login_id like '%{0}%' ORDER BY tbl_log.reg_date DESC", txt_searchkey.Text);
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_NICKNAME, "1"));
                select_search_type.Items.Add(new ListItem(Resources.Lang.STR_LOGINID, "2"));
                select_search_type.Items[1].Selected = true;
            }

            PageDataSource = DBConn.RunSelectQuery(query);

            if (!IsPostBack)
            {
                BindData();
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

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.AddHeader("Content-Disposition", "attachment;filename=Log_" + CurrentDate + ".csv");

            Response.BinaryWrite(new byte[] { 0xFF, 0xFE });

            string[] arrHeader = { 
                                 "No", 
                                 Resources.Lang.STR_REG_DATE, 
                                 Resources.Lang.STR_NICKNAME,
                                 Resources.Lang.STR_LOGINID,
                                 Resources.Lang.STR_EVENT,
                                 Resources.Lang.STR_QRCODE_COUNT,
                                 Resources.Lang.STR_PRODUCT_TYPE
                             };
            Response.Write(string.Join("\t", arrHeader) + "\n");

            string[] arrRow = null;

            if (!DataSetUtil.IsNullOrEmpty(PageDataSource))
            {
                for (int i = pos1; i < pos2; i++)
                {
                    arrRow = new string[] {
                        (i+1).ToString(),
                        DataSetUtil.RowStringValue(PageDataSource, "reg_date", i),
                        DataSetUtil.RowStringValue(PageDataSource, "nickname", i),
                        DataSetUtil.RowStringValue(PageDataSource, "login_id", i),
                        DataSetUtil.RowStringValue(PageDataSource, "event", i),
                        DataSetUtil.RowStringValue(PageDataSource, "qrcode_count", i),
                        DataSetUtil.RowStringValue(PageDataSource, "product_type", i)
                    };
                    Response.Write(string.Join("\t", arrRow) + "\n");
                }
            }

            Response.End();
            return;
        }

        protected override void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            base.gvContent_RowDataBound(sender, e);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //일시
                string regdate = dr["reg_date"].ToString();
                //등록자명
                string nickname = dr["nickname"].ToString();
                //등록자id
                string loginid = dr["login_id"].ToString();
                //이벤트
                string str_event = dr["event"].ToString();
                //시리얼 수량
                string serialcount = dr["qrcode_count"].ToString();
                //해당 시리얼 성격
                string producttype = dr["product_type"].ToString();

                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrNickname = (Literal)e.Row.FindControl("ltrNickname");
                Literal ltrLoginID = (Literal)e.Row.FindControl("ltrLoginID");
                Literal ltrEvent = (Literal)e.Row.FindControl("ltrEvent");
                Literal ltrSerialCount = (Literal)e.Row.FindControl("ltrSerialCount");
                Literal ltrProductType = (Literal)e.Row.FindControl("ltrProductType");

                ltrRegDate.Text = regdate;
                ltrNickname.Text = nickname;
                ltrLoginID.Text = loginid;
                ltrEvent.Text = str_event;
                ltrSerialCount.Text = serialcount;
                ltrProductType.Text = producttype;
            }
        }

    }
}