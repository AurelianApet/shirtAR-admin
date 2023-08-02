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
    public partial class AccountList : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.AuthUser.ULevel != 1)
            {
                //최고관리자만 접근가능합니다.
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
            PageDataSource = DBConn.RunSelectQuery("SELECT * FROM tbl_admin");
            BindData();
        }

        protected override void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            base.gvContent_RowDataBound(sender, e);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //회사명
                string nickname = dr["nickname"].ToString();
                //로그인id
                string loginid = dr["login_id"].ToString();
                //로그인pwd
                string loginpwd = CryptSHA256.Decrypt(dr["login_pwd"].ToString());
                //계정권한
                int level = int.Parse(dr["level"].ToString());
                //QR코드생성수량
                string qrcodecount = dr["qrcode_count"].ToString();

                Literal ltrNickname = (Literal)e.Row.FindControl("ltrNickname");
                Literal ltrLoginID = (Literal)e.Row.FindControl("ltrLoginID");
                Literal ltrLevel = (Literal)e.Row.FindControl("ltrLevel");
                Literal ltrModify = (Literal)e.Row.FindControl("ltrModify");
                Literal ltrSerialCount = (Literal)e.Row.FindControl("ltrSerialCount");

                ltrNickname.Text = nickname;
                ltrLoginID.Text = loginid;

                if (level == 0)
                {//License관리자
                    ltrLevel.Text = Resources.Lang.STR_LICENSER;
                }
                else if (level == 1)
                {//최고관리자
                    ltrLevel.Text = Resources.Lang.STR_ADMINISTRATOR;
                }

                ltrSerialCount.Text = qrcodecount + Resources.Lang.STR_GAE;
                ltrModify.Text = "<input type=\"button\" class=\"btnSmallGray\" onclick=\"showModifyAccountPopup('" + nickname + "','" + loginid + "','" + loginpwd + "','" + level + "','" + qrcodecount + "')\" value=\"" + Resources.Lang.STR_CHANGE + "\" style=\"width:58px; height:22px;\"/>";
            }
        }

    }
}