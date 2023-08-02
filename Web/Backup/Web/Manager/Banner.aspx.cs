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
    public partial class Banner : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.AuthUser.ULevel == 0)
            {
                isAdmin.Value = "0";
            }
            else if (CurrentPage.AuthUser.ULevel == 1)
            {
                isAdmin.Value = "1";
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
            PageDataSource = DBConn.RunSelectQuery("SELECT * FROM tbl_banner");
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
                //배너명
                string name = dr["name"].ToString();
                //배너이미지
                string path = dr["path"].ToString();
                //배너링크
                string link = dr["link"].ToString();
                //배너노출
                string is_activate = dr["is_activate"].ToString();
                //배너등록날짜
                string reg_date = dr["reg_date"].ToString();

                Literal ltrBannerName = (Literal)e.Row.FindControl("ltrBannerName");
                Literal ltrThumbnail = (Literal)e.Row.FindControl("ltrThumbnail");
                Literal ltrBannerLink = (Literal)e.Row.FindControl("ltrBannerLink");
                Literal ltrIsActivity = (Literal)e.Row.FindControl("ltrIsActivity");
                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrDelete = (Literal)e.Row.FindControl("ltrDelete");

                ltrBannerName.Text = name;
                ltrThumbnail.Text = "<img src='" + path + "' width='144' height='81' style='box-shadow:0px 1px 6px rgba(0,0,0,0.4); margin:auto;'/>";
                ltrBannerLink.Text = link;

                if (is_activate == "0")
                {
                    ltrIsActivity.Text = "<select id=\"ddl_is_activate_" + id + "\" onchange=\"setActivate('" + id + "')\"><option value=\"0\" selected>" + Resources.Lang.STR_NO_ACT + "</option><option value=\"1\">" + Resources.Lang.STR_ACT + "</option></select>";
                }
                else if (is_activate == "1")
                {
                    ltrIsActivity.Text = "<select id=\"ddl_is_activate_" + id + "\" onchange=\"setActivate('" + id + "')\"><option value=\"0\">" + Resources.Lang.STR_NO_ACT + "</option><option value=\"1\" selected>" + Resources.Lang.STR_ACT + "</option></select>";
                }

                ltrRegDate.Text = reg_date;
                ltrDelete.Text = "<input type=\"button\" class=\"btnSmallGray\" onclick=\"showDelPopup('" + id + "')\" value=\"" + Resources.Lang.STR_DELETE + "\" style=\"width:58px; height:22px;\"/>";
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxBannerName.Text))
            {
                //배너명을 입력해주십시오.
                Alert(Resources.Lang.MSG_INPUT_BANNER_NAME, "/Manager/Banner.aspx");
                return;
            }

            if (string.IsNullOrEmpty(tbxBannerPath.Text))
            {
                //배너이미지를 입력해주십시오.
                Alert(Resources.Lang.MSG_INPUT_BANNER_IMAGE, "/Manager/Banner.aspx");
                return;
            }

            if (string.IsNullOrEmpty(tbxBannerLink.Text))
            {
                //배너링크를 입력해주십시오.
                Alert(Resources.Lang.MSG_INPUT_BANNER_LINK, "/Manager/Banner.aspx");
                return;
            }

            string sourceFile = tbxBannerPath.Text;
            string destinationFile = sourceFile.Replace("/temp/", "/bannerImg/");
            System.IO.File.Move(Server.MapPath(sourceFile), Server.MapPath(destinationFile));
            tbxBannerPath.Text = destinationFile;

            DataSet dsTemp = DBConn.RunSelectQuery("SELECT COUNT(*) as count FROM tbl_banner");
            if (DataSetUtil.IsNullOrEmpty(dsTemp))
            {
                //등록된 배너 조회중 오류가 발생하였습니다.
                Alert(Resources.Lang.MSG_BANNER_INFO_ERROR, "/Manager/Banner.aspx");
                return;
            }

            if (DataSetUtil.RowIntValue(dsTemp, "count", 0) >= 3)
            {
                //배너는 최대 3개까지만 등록가능합니다.
                Alert(Resources.Lang.MSG_BANNER_ADD_WARNING, "/Manager/Banner.aspx");
                return;
            }

            DBConn.RunInsertQuery("INSERT INTO tbl_banner (name, path, link, is_activate) VALUES({0}, {1}, {2}, {3})",
                    new string[] {
                        "@name",
                        "@path",
                        "@link",
                        "@is_activate"
                        },
                    new object[] {
                        tbxBannerName.Text,
                        tbxBannerPath.Text,
                        tbxBannerLink.Text,
                        hd_optActivity.Value
                    });

            Alert(Resources.Lang.MSG_BANNER_ADD_SUCCESS, "/Manager/Banner.aspx");
            return;
        }

    }
}