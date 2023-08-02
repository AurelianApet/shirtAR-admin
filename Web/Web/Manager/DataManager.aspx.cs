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
    public partial class DataManager : PageBase
    {
        public PageBase CurrentPage
        {
            get { return Page as PageBase; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPage.AuthUser.ULevel != 1)
            {
                Alert(Resources.Lang.MSG_ADMIN_ACCESSABLE, "/Manager/Logout.aspx");
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

            PageDataSource = DBConn.RunSelectQuery("SELECT * FROM tbl_data ORDER BY tbl_data.reg_date ASC");
        }

        protected override void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            base.gvContent_RowDataBound(sender, e);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //데이터ID
                string id = dr["id"].ToString();
                //데이터식별코드
                string dataid = dr["data_id"].ToString();
                //데이터타입
                string datatype = dr["data_type"].ToString();
                //데이터버전
                string dataversion = dr["data_version"].ToString();
                //데이터크기
                string datasize = dr["data_size"].ToString();
                //데이터설명
                string datadescription = dr["data_description"].ToString();
                //데이터링크
                string datalink = dr["data_link1"].ToString() + "<br>" + dr["data_link2"].ToString() + "<br>" + dr["data_link3"].ToString() + "<br>" + dr["data_link4"].ToString() + "<br>" + dr["data_link5"].ToString();
                //등록날자
                string regdate = dr["reg_date"].ToString();

                Literal ltrCheckbox = (Literal)e.Row.FindControl("ltrCheckbox");
                Literal ltrRegDate = (Literal)e.Row.FindControl("ltrRegDate");
                Literal ltrDataID = (Literal)e.Row.FindControl("ltrDataID");
                Literal ltrDataVersion = (Literal)e.Row.FindControl("ltrDataVersion");
                Literal ltrDataSize = (Literal)e.Row.FindControl("ltrDataSize");
                Literal ltrDataLink = (Literal)e.Row.FindControl("ltrDataLink");
                Literal ltrDataDescription = (Literal)e.Row.FindControl("ltrDataDescription");
                Literal ltrDataType = (Literal)e.Row.FindControl("ltrDataType");
                Literal ltrModify = (Literal)e.Row.FindControl("ltrModify");

                ltrCheckbox.Text = "<input type=\"checkbox\" style=\"height: auto;\" name=\"chkNo\" value='" + id + "' />";
                ltrRegDate.Text = DateTime.Parse(regdate).ToString("yyyy-MM-dd");
                ltrDataID.Text = dataid;
                ltrDataVersion.Text = dataversion;
                ltrDataSize.Text = datasize;
                ltrDataLink.Text = datalink;
                ltrDataDescription.Text = datadescription;
                
                if(datatype == "0")
                {
                    ltrDataType.Text = "Android";
                }
                else if(datatype == "1")
                {
                    ltrDataType.Text = "IOS";
                }

                ltrModify.Text = "<input type=\"button\" class=\"btnSmallGray\" onclick=\"showModifyPopup('" + id + "')\" value=\"" + Resources.Lang.STR_CHANGE + "\" style=\"width:58px; height:22px;\"/>";
            }
        }

    }
}