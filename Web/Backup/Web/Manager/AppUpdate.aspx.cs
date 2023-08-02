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
    public partial class AppUpdate : PageBase
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
            PageDataSource = DBConn.RunSelectQuery("SELECT * FROM tbl_update ORDER BY version DESC");
            BindData();
        }

        protected override void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            base.gvContent_RowDataBound(sender, e);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //ID
                string id = dr["id"].ToString();
                //업데이트버전
                string version = dr["version"].ToString();
                //업데이트메모
                string memo = dr["memo"].ToString();
                //등록날짜
                string reg_date = dr["reg_date"].ToString();

                Literal ltrUpdateVersion = (Literal)e.Row.FindControl("ltrUpdateVersion");
                Literal ltrUpdateMemo = (Literal)e.Row.FindControl("ltrUpdateMemo");
                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrDelete = (Literal)e.Row.FindControl("ltrDelete");

                ltrUpdateVersion.Text = version;
                ltrUpdateMemo.Text = memo;
                ltrRegDate.Text = reg_date;
                ltrDelete.Text = "<input type=\"button\" class=\"btnSmallGray\" onclick=\"showDelPopup('" + id + "')\" value=\"" + Resources.Lang.STR_DELETE + "\" style=\"width:58px; height:22px;\"/>";
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUpdateVersion.Text))
            {
                //업데이트버전을 입력해주십시오.
                Alert(Resources.Lang.MSG_INPUT_UPDATE_VERSION, "/Manager/AppUpdate.aspx");
                return;
            }

            if (string.IsNullOrEmpty(txtUpdateMemo.Text))
            {
                //업데이트내용를 입력해주십시오.
                Alert(Resources.Lang.MSG_INPUT_UPDATE_MEMO, "/Manager/AppUpdate.aspx");
                return;
            }

            DataSet dsTemp = DBConn.RunSelectQuery("SELECT * FROM tbl_update WHERE version={0}", new string[] { "@version" }, new object[] { txtUpdateVersion.Text });
            if (!DataSetUtil.IsNullOrEmpty(dsTemp))
            {
                //입력한 업데이트버전이 이미 존재합니다.
                Alert(Resources.Lang.MSG_UPDATE_INFO_EXIST, "/Manager/AppUpdate.aspx");
                return;
            }

            DBConn.RunInsertQuery("INSERT INTO tbl_update (version, memo, reg_date) VALUES({0}, {1}, {2})",
                    new string[] {
                        "@version",
                        "@memo",
                        "@reg_date"
                        },
                    new object[] {
                        txtUpdateVersion.Text,
                        txtUpdateMemo.Text,
                        DateTime.Now
                    });

            Alert(Resources.Lang.MSG_UPDATE_INFO_ADD_SUCCESS, "/Manager/AppUpdate.aspx");
            return;
        }

    }
}