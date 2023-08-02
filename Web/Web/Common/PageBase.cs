using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataAccess;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

using System.Drawing;
using System.Drawing.Imaging;

using Microsoft.VisualBasic.FileIO;

using System.Globalization;
using System.Threading;

namespace retroplus.Common
{
    /// <summary>
    /// Summary description for PageBase
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        #region 속성 및 상수정의부

        //페지당 표시수
        private static int page_rows = 100;
        public int PAGE_ROWS
        {
            get { return page_rows; }
        }

        // 접속기기 종류
        private string _mobiletype = null;
        public string MobileType
        {
            get { return _mobiletype; }
        }

        public string ActiveMobileType
        {
            get
            {
                if (Session[Constants.SESSION_KEY_MOBILETYPE] != null)
                    return Session[Constants.SESSION_KEY_MOBILETYPE].ToString();

                return _mobiletype;
            }
            set
            {
                Session[Constants.SESSION_KEY_MOBILETYPE] = value;
            }
        }

        // 관리자페이지목록
        private Dictionary<string, string> _adminpage = null;
        public Dictionary<string, string> AdminPage
        {
            get { return _adminpage; }
        }

        //public string ADMIN_MENU_HTML
        //{
        //    get
        //    {
        //        return "<a href='javascript:;' onclick=\\\"onPopupSendMsg('__loginid__')\\\">" + Resources.Str.STR_SENDMSG + "</a><br />" +
        //               "<a href='/Manage/MemberMng/MemberEdit.do?mid=__id__' target='_blank'>" + Resources.Str.STR_USERINFOCHANGE + "</a><br />" +
        //               "<a href='/Manage/MoneyMng/PointMng.do?loginid=__loginid__' target='_blank'>" + Resources.Str.STR_POINTHIST + "</a><br />" +
        //               "<a href='/Manage/MoneyMng/CashMng.do?loginid=__loginid__' target='_blank'>" + Resources.Str.STR_CASHHIST + "</a><br />" +
        //               "<a href='/Manage/BettingMng/BettingMng.do?loginid=__loginid__' target='_blank'>" + Resources.Str.STR_BETHIST + "</a><br />";
        //    }
        //}

        protected MSSQLAccess _dbconn = null;
        public MSSQLAccess DBConn
        {
            get
            {
                return _dbconn;
            }
        }

        private Config _config = null;
        public Config SiteConfig
        {
            get { return _config; }
        }

        //private Dictionary<int, MemberConfig> _mbconfig = null;
        //public Dictionary<int, MemberConfig> MemberConfig
        //{
        //    get { return _mbconfig; }
        //}

        protected bool _isInited = false;
        public bool IsInited
        {
            get { return _isInited; }
        }

        private string _adminName = "";
        public string AdminName
        {
            get { return _adminName; }
        }

        protected string strCurDate = null;
        public string CurrentDate
        {
            get { return strCurDate; }
        }

        protected string _userip = "";
        public string UserIP
        {
            get { return _userip; }
        }
      
        // 읽지 않은 새 메시지수
        private int _unreadnoticecount = 0;
        public int UnreadNoticeCount
        {
            get
            {
                return _unreadnoticecount;
            }
        }
        // 읽지 않은 답변수
        private int _unreadanscount = 0;
        public int UnreadAnsCount
        {
            get
            {
                return _unreadanscount;
            }
        }

        public AuthUser AuthUser
        {
            get
            {
                try
                {
                    // 세션에 유저정보가 등록되어있는 경우
                    if (Session[Constants.SESSION_KEY_USERINFO] != null &&
                        Session[Constants.SESSION_KEY_USERINFO] as AuthUser != null)
                    {
                        return Session[Constants.SESSION_KEY_USERINFO] as AuthUser;
                    }
                    else
                    {
                        // 세션에 없다면 쿠키 검사
                        // 쿠키에는 아이디;로그인아이디;암호화된패스워드;닉네임;레벨;로그인시간 형태로 보관됨
                        if (getCookie(Constants.COOKIE_KEY_USERINFO) != null)
                        {
                            string strCookie = getCookie(Constants.COOKIE_KEY_USERINFO);

                            strCookie = CryptSHA256.Decrypt(strCookie);
                            string[] arrTemp = strCookie.Split(';');

                            // 보관된 자료개수가 정확한 경우
                            if (arrTemp.Length == 6)
                            {
                                // 로그인한 시간을 검사
                                // 만일 하루이전에 로그인한것이면 다시 로그인정보를 세션에 기록함
                                DateTime dtLoginDate = DateTime.Parse(arrTemp[5]);
                                if ((DateTime.Now - dtLoginDate).TotalHours < 24)
                                {
                                    System.Data.DataSet dsUser = DBConn.RunStoreProcedure(
                                            Constants.SP_GETUSER,
                                                new string[] {
                                                "@login_id"
                                            },
                                            new object[] {
                                                arrTemp[1]
                                            });

                                    if (DataSetUtil.IsNullOrEmpty(dsUser))
                                        return null;

                                    // 쿠키에 저장된 암호와 디비의 암호가 맞지 않는 경우
                                    if (DataSetUtil.RowStringValue(dsUser, "login_pwd", 0) != arrTemp[2])
                                        return null;

                                    AuthUser _authUser = new AuthUser(
                                        DataSetUtil.RowLongValue(dsUser, "id", 0),
                                        DataSetUtil.RowStringValue(dsUser, "nickname", 0),
                                        DataSetUtil.RowStringValue(dsUser, "login_id", 0),
                                        DataSetUtil.RowStringValue(dsUser, "login_pwd", 0),                                        
                                        DataSetUtil.RowIntValue(dsUser, "level", 0));

                                    Session[Constants.SESSION_KEY_USERINFO] = _authUser;
                                    return _authUser;
                                }
                            }
                        }
                    }
                }
                catch { }

                return null;
            }
        }

        public int PageNumber
        {
            get
            {
                if (ViewState[Constants.VS_PAGENUMBER] != null)
                    return Convert.ToInt32(ViewState[Constants.VS_PAGENUMBER]);
                else
                    return 0;
            }
            set
            {
                ViewState[Constants.VS_PAGENUMBER] = value;
            }
        }
        private DataSet _dsPageData = null;
        public System.Data.DataSet PageDataSource
        {
            get
            {
                return _dsPageData;
            }
            set
            {
                _dsPageData = value;
            }
        }

        public string SortColumn
        {
            get
            {
                return ViewState[Constants.VS_SORTCOLUMN] as string;
            }
            set
            {
                ViewState[Constants.VS_SORTCOLUMN] = value;
            }
        }
        public SortDirection SortDirection
        {
            get
            {
                return (SortDirection)ViewState[Constants.VS_SORTDIRECTION];
            }
            set
            {
                ViewState[Constants.VS_SORTDIRECTION] = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtRet = DateTime.Now;

                if (dtNow.Hour < 15)
                    dtRet = DateTime.Parse(dtNow.AddDays(-1).ToString("yyyy-MM-dd 15:00:00"));
                else
                    dtRet = DateTime.Parse(dtNow.ToString("yyyy-MM-dd 15:00:00"));

                if (ViewState[Constants.VS_STARTDATE] != null)
                {
                    try
                    {
                        dtRet = Convert.ToDateTime(ViewState[Constants.VS_STARTDATE]);
                    }
                    catch { }
                }
                else
                {
                    ViewState[Constants.VS_STARTDATE] = dtRet;
                }
                return dtRet;
            }
            set
            {
                ViewState[Constants.VS_STARTDATE] = value;
            }
        }
        public DateTime EndDate
        {
            get
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtRet = DateTime.Now;

                if (dtNow.Hour < 15)
                    dtRet = DateTime.Parse(dtNow.ToString("yyyy-MM-dd 14:59:00"));
                else
                    dtRet = DateTime.Parse(dtNow.AddDays(1).ToString("yyyy-MM-dd 14:59:00"));

                if (ViewState[Constants.VS_ENDDATE] != null)
                {
                    try
                    {
                        dtRet = Convert.ToDateTime(ViewState[Constants.VS_ENDDATE]);
                    }
                    catch { }
                }
                else
                {
                    ViewState[Constants.VS_ENDDATE] = dtRet;
                }
                return dtRet;
            }
            set
            {
                ViewState[Constants.VS_ENDDATE] = value;
            }
        }
        public DateTime SearchDate
        {
            get
            {
                DateTime dtRet = DateTime.Now;

                if (Session[Constants.VS_SEARCHDATE] != null)
                {
                    try
                    {
                        dtRet = Convert.ToDateTime(Session[Constants.VS_SEARCHDATE]);
                    }
                    catch { }
                }
                else
                {
                    Session[Constants.VS_SEARCHDATE] = dtRet;
                }
                return dtRet;
            }
            set
            {
                Session[Constants.VS_SEARCHDATE] = value;
            }
        }
        
        // 로그인아이디 저장관련
        protected bool _isRemeberMe = false;        
        public bool isRememberMe
        {
            get
            {
                return _isRemeberMe;
            }
            set
            {
                _isRemeberMe = value;
            }
        }
        // 자동로그인 유무
        protected bool _isAutoLogin = false;
        public bool isAutoLogin
        {
            get
            {
                return _isAutoLogin;
            }
            set
            {
                _isAutoLogin = value;
            }
        }
        #endregion

        public PageBase()
        {
        }

        #region 유저관리부분
        protected bool UserLogin(string strLoginID, string strLoginPwd)
        {
            
            try
            {
                System.Data.DataSet dsUser = DBConn.RunStoreProcedure(
                    Constants.SP_GETUSER,
                    new string[] {
                    "@login_id"
                },
                    new object[] {
                    strLoginID
                });

                if (DataSetUtil.IsNullOrEmpty(dsUser))
                    return false;

                string sLoginPwd = DataSetUtil.RowValue(dsUser, "login_pwd", 0).ToString();                

                if (strLoginPwd != CryptSHA256.Decrypt(sLoginPwd))
                    return false;

                long lID = long.Parse(DataSetUtil.RowValue(dsUser, "id", 0).ToString());
                string sNickname = DataSetUtil.RowValue(dsUser, "nickname", 0).ToString();
                string sLoginID = DataSetUtil.RowValue(dsUser, "login_id", 0).ToString();               
                int iULevel = int.Parse(DataSetUtil.RowValue(dsUser, "level", 0).ToString());
                AuthUser _authUser = new AuthUser(lID, sNickname, sLoginID, sLoginPwd, iULevel);
                Session[Constants.SESSION_KEY_USERINFO] = _authUser;

                if (Defines.COOKIE_INUSED)
                {
                    string strCookieData = string.Format("{0};{1};{2};{3};{4}",
                        _authUser.ID,
                        _authUser.LoginID,
                        _authUser.LoginPwd,                        
                        _authUser.ULevel,
                        CurrentDate);

                    setCookie(Constants.COOKIE_KEY_USERINFO, CryptSHA256.Encrypt(strCookieData));
                }

                string strUserAgent = Request.ServerVariables["HTTP_USER_AGENT"];
                string strUserIP = Request.ServerVariables["REMOTE_ADDR"];
                string strUrl = Request.UrlReferrer.AbsolutePath;

            }
            catch(Exception ex)
            {
                return false;
            }
            //아디저장 및 자동로그인 쿠키 보관
            if (isRememberMe)
            {
                string strCookieData = string.Format("{0};{1}",
                    "REMEBERID",
                    AuthUser.LoginID);
                

                HttpCookie hc = null;

                hc = new HttpCookie(Constants.COOKIE_KEY_REMEBERID);

                hc.Values.Add(Constants.COOKIE_KEY_REMEBERID, CryptSHA256.Encrypt(strCookieData));

                hc.Expires = DateTime.Now.AddHours(Defines.COOKIE_TIMEOUT);

                Response.Cookies.Add(hc);
            }
            else
            {
                if (Request.Cookies[Constants.COOKIE_KEY_REMEBERID] != null)
                {
                    HttpCookie hcCookie = Request.Cookies[Constants.COOKIE_KEY_REMEBERID];
                    hcCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(hcCookie);
                }
                
            }

            if (isAutoLogin)
            {
                string strCookieData = string.Format("{0}",
                    "AUTOLOGIN");                

                HttpCookie hc = null;

                hc = new HttpCookie(Constants.COOKIE_KEY_AUTOLOGIN);

                hc.Values.Add(Constants.COOKIE_KEY_AUTOLOGIN, CryptSHA256.Encrypt(strCookieData));

                hc.Expires = DateTime.Now.AddHours(Defines.COOKIE_TIMEOUT);

                Response.Cookies.Add(hc);
            }
            else
            {
                if (Request.Cookies[Constants.COOKIE_KEY_AUTOLOGIN] != null)
                {
                    HttpCookie hcCookie = Request.Cookies[Constants.COOKIE_KEY_AUTOLOGIN];
                    hcCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(hcCookie);
                }

            }
            return true;
        }
        public void UserLogout()
        {
            //로그
            DBConn.RunInsertQuery("INSERT INTO tbl_log (admin_id, event, qrcode_count, product_type, reg_date) VALUES({0}, {1}, '', '', GETDATE())",
                new string[] { 
                    "@admin_id",
                    "@event" 
                },
                new object[] { 
                    AuthUser.ID,
                    Resources.Lang.STR_LOGOUT
                });
            //로그

            Session[Constants.SESSION_KEY_USERINFO] = null;
            Session.Remove(Constants.SESSION_KEY_USERINFO);

            if (Request.Cookies[Constants.COOKIE_KEY_SITE] != null)
            {
                if (Request.Cookies[Constants.COOKIE_KEY_AUTOLOGIN] == null)
                {
                    HttpCookie hcCookie = Request.Cookies[Constants.COOKIE_KEY_SITE];
                    hcCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(hcCookie);
                }
            }
            if (Request.Cookies[Constants.COOKIE_KEY_AUTOLOGIN] != null)
            {
                HttpCookie hcCookie = Request.Cookies[Constants.COOKIE_KEY_AUTOLOGIN];
                hcCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(hcCookie);
            }
        }                
        #endregion

        #region 기타 유틸 함수
        public string MD5(string strSrc)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strSrc, "MD5");
        }
        public void ShowMessageBox(string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg))
                return;

            ClientScript.RegisterStartupScript(GetType(),
                "MessageBox",
                "alert('" + strMsg.Replace("'", "\'") + "');",
                true);
        }
        public void ShowMessageBox(string strMsg, string strUrl)
        {
            if (string.IsNullOrEmpty(strMsg))
                return;

            ClientScript.RegisterStartupScript(GetType(),
                "MessageBox",
                "alert('" + strMsg.Replace("'", "\'") + "');" +
                "location.href='" + strUrl + "';",
                true);
        }
        public void ShowConfirm(string strMsg, string strUrl)
        {
            if (string.IsNullOrEmpty(strMsg))
                return;

            ClientScript.RegisterStartupScript(GetType(),
                "ShowConfirm",
                "if(confirm('" + strMsg.Replace("'", "\'") + "') == true) {" +
                "location.href='" + strUrl + "';" +
                "}",
                true);
        }
        public void Alert(string strMsg, string strUrl)
        {
            if (strMsg == null)
                return;

            Response.Write("<script lanuage=\"javascript\" type=\"text/javascript\">\n" +
                            "alert('" + strMsg.Replace("'", "\'") + "');\n" +
                            "location.href='" + strUrl + "';\n" +
                            "</script>");
            Response.End();
        }
        public void AlertAndClose(string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg))
                return;

            Response.Write("<script lanuage=\"javascript\" type=\"text/javascript\">\n" +
                            "alert('" + strMsg.Replace("'", "\'") + "');\n" +
                            "window.opener.location.href=window.opener.location.href;\n" +
                            "window.close();\n" +
                            "</script>");
            Response.End();
        }
        public void ShowError(string strMsg)
        {
            if (strMsg != null)
                Response.Write("<h4><font color=\"red\">" + strMsg + "</font></h4>");

            Response.End();
        }
        public bool checkAuth()
        {
            if (AuthUser == null)
            {
                Alert(Resources.Err.ERR_REQUIRED_AUTH, Defines.URL_LOGIN);
                return false;
            }

            return true;
        }

        //public Dictionary<string, string> checkAdminAuth()
        //{
        //    Dictionary<string, string> dicPerm = new Dictionary<string, string>();

        //    checkAuth();

        //    if (!AuthUser.IsAdmin)
        //    {
        //        System.Data.DataSet dsPerm = DBConn.RunStoreProcedure(Constants.SP_GETMANAGEPERM,
        //            new string[] { "@user_id" }, new object[] { AuthUser.ID });

        //        for (int i = 0; i < DataSetUtil.RowCount(dsPerm); i++)
        //        {
        //            string strPageNo = DataSetUtil.RowStringValue(dsPerm, "pageno", i);
        //            if (AdminPage.Keys.Contains(strPageNo) &&
        //                !dicPerm.Keys.Contains(strPageNo))
        //            {
        //                dicPerm.Add(strPageNo, DataSetUtil.RowStringValue(dsPerm, "perm", i));
        //            }
        //        }

        //        if (dicPerm.Count < 1)
        //        {
        //            UserLogout();
        //            Alert(Resources.Err.ERR_REQUIRED_AUTH, Defines.URL_LOGIN);
        //            return null;
        //        }
        //    }

        //    return dicPerm;
        //}

        public bool checkBlockOrLeave(out string strMsg)
        {
            strMsg = "";

            // 차단 또는 삭제되었는가 검사
            if (AuthUser != null)
            {
                System.Data.DataSet dsUser = DBConn.RunStoreProcedure(Constants.SP_GETUSER,
                    new string[] { "@id" }, new object[] { AuthUser.ID });

                if (DataSetUtil.IsNullOrEmpty(dsUser))
                {
                    strMsg = Resources.Err.ERR_LOGINID_INVALID;
                    return true;
                }

                
                //string strLeaveDate = DataSetUtil.RowDateTimeValue(dsUser, "leavedate", 0);
                //if (!string.IsNullOrEmpty(strLeaveDate))
                //{
                //    strMsg = string.Format(Resources.Err.ERR_USER_LEAVE, strLeaveDate);
                //    return true;
                //}
            }

            return false;
        }
        public string cutString(string strValue, int iLength)
        {
            return cutString(strValue, iLength, "...");
        }
        public string cutString(string strValue, int iLength, string strFooter)
        {
            if (string.IsNullOrEmpty(strValue))
                return "";

            if (iLength < 1 || strValue.Length <= iLength)
                return strValue;

            return strValue.Substring(0, iLength) + strFooter;
        }
        public string cutHTML(string strValue, int iLength)
        {
            return cutHTML(strValue, iLength, "...");
        }
        public string cutHTML(string strValue, int iLength, string strFooter)
        {
            if (string.IsNullOrEmpty(strValue))
                return "";

            string strRet = "";
            bool bIsTag = false;
            for (int i = 0; i < strValue.Length; i++)
            {
                string strChar = strValue.Substring(i, 1);
                if (strChar == "<") bIsTag = true;
                if (!bIsTag) strRet += strChar;
                if (strRet.Length >= iLength)
                {
                    strRet += strFooter;
                    break;
                }
                if (strChar == ">") bIsTag = false;
            }
            return strRet;
        }
        public int null2Zero(object objValue)
        {
            try
            {
                return int.Parse(objValue.ToString());
            }
            catch
            {
            }

            return 0;
        }
        public string text2Html(string strValue)
        {
            return strValue.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }
        public string html2Text(string strValue)
        {
            return strValue.Replace("<br />", "\r\n");
        }
        
        protected string getCookie(string strKey)
        {
            HttpCookie hcCookie = Request.Cookies[Constants.COOKIE_KEY_SITE];
            if (hcCookie == null || hcCookie[strKey] == null)
                return null;

            return hcCookie[strKey];
        }
        protected void setCookie(string strKey, string strValue)
        {
            HttpCookie hc = null;

            if (Request.Cookies[Constants.COOKIE_KEY_SITE] != null)
                hc = Request.Cookies[Constants.COOKIE_KEY_SITE];
            else
                hc = new HttpCookie(Constants.COOKIE_KEY_SITE);

            hc.Values.Add(strKey, strValue);

            hc.Expires = DateTime.Now.AddHours(Defines.COOKIE_TIMEOUT);

            Response.Cookies.Add(hc);
        }        
        protected virtual void visibleEmptyRow(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpCtrl = sender as Repeater;
            if (rpCtrl == null)
                return;

            if (rpCtrl.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    System.Web.UI.HtmlControls.HtmlTableRow tr = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("rowEmpty");
                    if (tr != null)
                        tr.Visible = true;
                }
            }
        }
        public void setLiteralValue(GridViewRow gvRow, string strID, string strValue)
        {
            Literal ltlTarget = (Literal)gvRow.FindControl(strID);
            if (ltlTarget != null)
                ltlTarget.Text = strValue;
        }
        public void setItemValue(RepeaterItem rpItem, string strID, string strValue)
        {
            Literal ltlTarget = (Literal)rpItem.FindControl(strID);
            if (ltlTarget != null)
                ltlTarget.Text = strValue;
        }

        public void setPageRows(DropDownList ddl, string dbField)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("20열", "20"));
            ddl.Items.Add(new ListItem("50열", "50"));
            ddl.Items.Add(new ListItem("100열", "100"));
            ddl.Items.Add(new ListItem("200열", "200"));
            ddl.Items[2].Selected = true;
            PageBase.page_rows = Int32.Parse(ddl.SelectedValue);

            /*
            string query = string.Format("SELECT * FROM tbl_admin WHERE id = {0}", AuthUser.ID);
            DataSet dsTemp = DBConn.RunSelectQuery(query);
            if (DataSetUtil.IsNullOrEmpty(dsTemp))
            {//default
                ddl.Items.Add(new ListItem("20열", "20"));
                ddl.Items.Add(new ListItem("50열", "50"));
                ddl.Items.Add(new ListItem("100열", "100"));
                ddl.Items.Add(new ListItem("200열", "200"));
                ddl.Items[2].Selected = true;
                PageBase.page_rows = Int32.Parse(ddl.SelectedValue);
            }
            else
            {
                ddl.Items.Add(new ListItem("20열", "20"));
                ddl.Items.Add(new ListItem("50열", "50"));
                ddl.Items.Add(new ListItem("100열", "100"));
                ddl.Items.Add(new ListItem("200열", "200"));
                ddl.Items[DataSetUtil.RowIntValue(dsTemp, dbField, 0)].Selected = true;
                PageBase.page_rows = Int32.Parse(ddl.SelectedValue);
            }*/
            return;
        }

        public void GenerateThumbnails(string sourcePath, string targetPath, string filename, int width, int height)
        {
            string strSourcePath = Server.MapPath(sourcePath);
            string strPath = Server.MapPath(targetPath);
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string strTargetPath = strPath + "\\" + filename;
            if (File.Exists(strSourcePath))
            {
                if (File.Exists(strTargetPath))
                    File.Delete(strTargetPath);
                using (System.Drawing.Image img1 = System.Drawing.Image.FromFile(strSourcePath))
                {
                    int i_width = img1.Width;
                    int i_height = img1.Height;

                    height = (int)(i_height * width / i_width);
                    //System.Drawing.Image img1 = System.Drawing.Image.FromFile(strSourcePath);
                    System.Drawing.Image bmp1 = new Bitmap(img1, width, (int)(i_height * width / i_width)); //img1.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    //bmp1.Save(MapPath(targetPath));
                    bmp1.Save(strTargetPath);
                }
            }
        }

        protected bool checkImageFile(HttpPostedFile srcFile)
        {
            if (srcFile == null || srcFile.ContentLength == 0)
                return false;

            string[] arrValidExt =
                new string[] { 
                    "gif",
                    "jpg",
                    "png"
                };

            string strExt = srcFile.FileName.Substring(srcFile.FileName.LastIndexOf(".") + 1);

            for (int i = 0; i < arrValidExt.Length; i++)
            {
                if (strExt.ToLower() == arrValidExt[i])
                    return true;
            }

            return false;
        }

        protected string uploadFile(HttpPostedFile srcFile, string strWebPath, string strFileName)
        {
            if (srcFile == null || srcFile.ContentLength == 0)
                return null;

            try
            {
                string strPath = Server.MapPath(strWebPath);
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                string strTargetPath = strPath + "\\" + strFileName;
                if (File.Exists(strTargetPath))
                    File.Delete(strTargetPath);

                srcFile.SaveAs(strTargetPath);
            }
            catch
            {
                return null;
            }

            return strWebPath + "/" + strFileName;
        }
        protected string uploadFile(HttpPostedFile srcFile, string strWebPath)
        {
            if (srcFile == null || srcFile.ContentLength == 0)
                return null;

            string strFileName = srcFile.FileName;
            return uploadFile(srcFile, strWebPath, strFileName.Substring(strFileName.LastIndexOf("\\") + 1));
        }
        protected string copyFile(string strFullPath, string strWebPath, string strFileName)
        {
            if (string.IsNullOrEmpty(strFullPath))
                return null;

            try
            {
                string strPath = Server.MapPath(strWebPath);
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                string strTargetPath = strPath + "\\" + strFileName;

                FileStream fs = File.Create(strTargetPath);
                FileStream fsSrc = File.Open(strFullPath, FileMode.Open, FileAccess.Read);
                byte[] buff = new byte[fsSrc.Length];
                fsSrc.Read(buff, 0, buff.Length);
                fs.Write(buff, 0, buff.Length);
                fsSrc.Close();
                fs.Close();
                //File.Copy(strFullPath, strTargetPath, true);
            }
            catch
            {
                return null;
            }

            return strWebPath + "/" + strFileName;
        }
        protected void deleteFile(string strWebPath)
        {
            if (string.IsNullOrEmpty(strWebPath))
                return;

            try
            {
                string strPath = Server.MapPath(strWebPath);
                if (File.Exists(strPath))
                    File.Delete(strPath);
            }
            catch { }
        }

        protected string getBrowserType(string strAgent)
        {
            string strRet = Resources.Str.STR_OTHER;

            if (Regex.Match(strAgent, "msie 5.0[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 5.0";
            else if (Regex.Match(strAgent, "msie 5.5[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 5.5";
            else if (Regex.Match(strAgent, "msie 6.0[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 6.0";
            else if (Regex.Match(strAgent, "msie 7.0[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 7.0";
            else if (Regex.Match(strAgent, "msie 8.0[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 8.0";
            else if (Regex.Match(strAgent, "msie 9.0[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 9.0";
            else if (Regex.Match(strAgent, "msie 4.[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "MSIE 4.x";
            else if (Regex.Match(strAgent, "firefox", RegexOptions.IgnoreCase).Success)
                strRet = "Firefox";
            else if (Regex.Match(strAgent, "x11", RegexOptions.IgnoreCase).Success)
                strRet = "Netscape";
            else if (Regex.Match(strAgent, "opera", RegexOptions.IgnoreCase).Success)
                strRet = "Opera";
            else if (Regex.Match(strAgent, "gec", RegexOptions.IgnoreCase).Success)
                strRet = "Gecko";
            else if (Regex.Match(strAgent, "bot|slurp", RegexOptions.IgnoreCase).Success)
                strRet = "Robot";
            else if (Regex.Match(strAgent, "internet explorer", RegexOptions.IgnoreCase).Success)
                strRet = "IE";
            else if (Regex.Match(strAgent, "mozilla", RegexOptions.IgnoreCase).Success)
                strRet = "Mozilla";

            return strRet;
        }
        protected string getDomain(string strUrl)
        {
            string strRet = "[DIRECT]";
            Regex rg = new Regex(@"://(?<host>([a-z\d][-a-z\d]*[a-z\d]\.)*[a-z][-a-z\d]+[a-z])");
            if (rg.IsMatch(strUrl))
                strRet = rg.Match(strUrl).Result("${host}");

            return strRet;
        }
        protected string getOS(string strAgent)
        {
            string strRet = Resources.Str.STR_OTHER;

            if (Regex.Match(strAgent, "windows 98", RegexOptions.IgnoreCase).Success)
                strRet = "98";
            else if (Regex.Match(strAgent, "windows 95", RegexOptions.IgnoreCase).Success)
                strRet = "95";
            else if (Regex.Match(strAgent, "windows nt 4.[0-9]*", RegexOptions.IgnoreCase).Success)
                strRet = "NT";
            else if (Regex.Match(strAgent, "windows nt 5.0", RegexOptions.IgnoreCase).Success)
                strRet = "2000";
            else if (Regex.Match(strAgent, "windows nt 5.1", RegexOptions.IgnoreCase).Success)
                strRet = "XP";
            else if (Regex.Match(strAgent, "windows nt 5.2", RegexOptions.IgnoreCase).Success)
                strRet = "2003";
            else if (Regex.Match(strAgent, "windows nt 6.0", RegexOptions.IgnoreCase).Success)
                strRet = "Vista";
            else if (Regex.Match(strAgent, "windows nt 6.1", RegexOptions.IgnoreCase).Success)
                strRet = "Win7";
            else if (Regex.Match(strAgent, "windows 9x", RegexOptions.IgnoreCase).Success)
                strRet = "ME";
            else if (Regex.Match(strAgent, "windows ce", RegexOptions.IgnoreCase).Success)
                strRet = "CE";
            else if (Regex.Match(strAgent, "mac", RegexOptions.IgnoreCase).Success)
                strRet = "MacOS";
            else if (Regex.Match(strAgent, "linux", RegexOptions.IgnoreCase).Success)
                strRet = "Linux";
            else if (Regex.Match(strAgent, "sunos", RegexOptions.IgnoreCase).Success)
                strRet = "SunOS";
            else if (Regex.Match(strAgent, "irix", RegexOptions.IgnoreCase).Success)
                strRet = "Irix";
            else if (Regex.Match(strAgent, "phone", RegexOptions.IgnoreCase).Success)
                strRet = "Phone";
            else if (Regex.Match(strAgent, "bot|slurp", RegexOptions.IgnoreCase).Success)
                strRet = "Robot";
            else if (Regex.Match(strAgent, "internet explorer", RegexOptions.IgnoreCase).Success)
                strRet = "IE";
            else if (Regex.Match(strAgent, "mozilla", RegexOptions.IgnoreCase).Success)
                strRet = "Mozilla";

            return strRet;
        }
        protected string getChkSum(long lGameID, int iBetType, int iTarget, string strIP, long lBetMoney, double fBetRatio)
        {
            return CryptSHA256.Encrypt(string.Format("{0};{1};{2};{3};{4};{5}",
                            lGameID,
                            iBetType,
                            iTarget,
                            strIP,
                            lBetMoney,
                            fBetRatio));
        }
        
        //public void readUnreadMsg()
        //{
        //    if (AuthUser != null)
        //    {
        //        System.Data.DataSet dsTemp = DBConn.RunStoreProcedure(
        //            Constants.SP_GETUNREADNOTICECOUNT,
        //                new string[] {
        //                        "@user_id"
        //                    },
        //                new object[] {
        //                        AuthUser.ID
        //                    });

        //        _unreadnoticecount = DataSetUtil.RowIntValue(dsTemp, 0, 0);
        //        _unreadanscount = DataSetUtil.RowIntValue(dsTemp, 1, 0);
        //    }
        //}
        //public void readSiteConfig()
        //{
        //    // 사이트 설정정보 조회
        //    System.Data.DataSet dsConfig = DBConn.RunStoreProcedure(Constants.SP_GETCONFIG);
        //    if (DataSetUtil.IsNullOrEmpty(dsConfig))
        //        throw new Exception();

        //    _config = new Config();
        //    _config.initConfig(
        //        DataSetUtil.RowStringValue(dsConfig, "cf_title", 0),
        //        DataSetUtil.RowIntValue(dsConfig, "cf_member_join", 0),
        //        DataSetUtil.RowIntValue(dsConfig, "cf_auto_delete", 0),
        //        DataSetUtil.RowIntValue(dsConfig, "cf_page_rows", 0),
        //        DataSetUtil.RowIntValue(dsConfig, "cf_login_minutes", 0),
        //        DataSetUtil.RowStringValue(dsConfig, "cf_intercept_ip", 0),
        //        DataSetUtil.RowStringValue(dsConfig, "cf_prohibit_id", 0),
        //        DataSetUtil.RowStringValue(dsConfig, "cf_stipulation", 0),
        //        DataSetUtil.RowStringValue(dsConfig, "cf_privacy", 0),
        //        DataSetUtil.RowStringValue(dsConfig, "cf_quicknotice", 0),
        //        DataSetUtil.RowStringValue(dsConfig, "cf_noticepad", 0),
        //        DataSetUtil.RowIntValue(dsConfig, "cf_logincount_time", 0)
        //    );
        //}
        //public void readMemberConfig()
        //{
        //    // 유저의 레벨별 설정값 조회
        //    System.Data.DataSet dsMbConfig = DBConn.RunStoreProcedure(Constants.SP_GETMEMBERCONFIG);
        //    if (DataSetUtil.IsNullOrEmpty(dsMbConfig))
        //        throw new Exception();

        //    _mbconfig = new Dictionary<int, MemberConfig>();
        //    for (int i = 0; i < dsMbConfig.Tables[0].Rows.Count; i++)
        //    {
        //        int iLvl = DataSetUtil.RowIntValue(dsMbConfig, "lvl", i);
        //        if (iLvl < 1 || iLvl > Constants.SITE_MAX_USERLEVEL || _mbconfig.Keys.Contains(iLvl))
        //            continue;

        //        _mbconfig.Add(iLvl, new MemberConfig(
        //            DataSetUtil.RowStringValue(dsMbConfig, "lvlname", i),
        //            DataSetUtil.RowStringValue(dsMbConfig, "icon", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "charge", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "lose", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "win", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "bet", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "bet34", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "bet56", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "bet78", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "bet910", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "moneytype", i),
        //            DataSetUtil.RowLongValue(dsMbConfig, "maxwinmoney", i),
        //            DataSetUtil.RowLongValue(dsMbConfig, "maxbetmoney", i),
        //            DataSetUtil.RowLongValue(dsMbConfig, "maxonebetmoney", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_charge", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_lose", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_win", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_bet", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_bet34", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_bet56", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_bet78", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_bet910", i),
        //            DataSetUtil.RowIntValue(dsMbConfig, "r_moneytype", i),
        //            DataSetUtil.RowStringValue(dsMbConfig, "bankname", i),
        //            DataSetUtil.RowStringValue(dsMbConfig, "bankinfo", i),
        //            DataSetUtil.RowStringValue(dsMbConfig, "ownername", i)
        //        ));
        //    }
        //}
      
        /// <summary>
        /// 유저가입 또는 정보수정시에 입력값 검사부분
        /// </summary>
        /// <returns></returns>
        protected bool validateInput(
            string strLoginID,
            string strLoginPwd,
            string strLoginPwdConfirm,
            string strNickName,
            string strName,
            string strTelNo,
            string strEmail,
            string strBankNum,
            string strOwnerName,
            bool bRegister)
        {
            if (bRegister)
            {
                // 아이디 검사
                if (string.IsNullOrEmpty(strLoginID))
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINID_INPUT);
                    return false;
                }
                if (!Regex.Match(strLoginID, Resources.RegEx.REGEX_LOGINID).Success)
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINID_FORMAT);
                    return false;
                }                
                if (!Regex.Match(strNickName, Resources.RegEx.REGEX_NICKNAME).Success)
                {
                    ShowMessageBox(Resources.Err.ERR_NICKNAME_FORMAT);
                    return false;
                }

                // 이름 검사
                /*if (string.IsNullOrEmpty(strName))
                {
                    ShowMessageBox(Resources.Err.ERR_NAME_INPUT);
                    return false;
                }
                if (!Regex.Match(strName, Resources.RegEx.REGEX_NAME).Success)
                {
                    ShowMessageBox(Resources.Err.ERR_NAME_FORMAT);
                    return false;
                }*/
               

                // 사용할수 없는 아이디,닉네임인가 검사
                string[] arrProhibitID = SiteConfig.ProhibitID.Split(',');
                if (arrProhibitID.Contains(strLoginID))
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINID_CANNOTUSE);
                    return false;
                }

                if (arrProhibitID.Contains(strNickName))
                {
                    ShowMessageBox(Resources.Err.ERR_NICKNAME_CANNOTUSE);
                    return false;
                }

                // 패스워드 검사
                if (string.IsNullOrEmpty(strLoginPwd))
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINPWD_INPUT);
                    return false;
                }
                if (!Regex.Match(strLoginPwd, Resources.RegEx.REGEX_LOGINPWD).Success)
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINPWD_FORMAT);
                    return false;
                }

                // 확인용 패스워드 검사
                if (string.IsNullOrEmpty(strLoginPwdConfirm))
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINPWDCONF_INPUT);
                    return false;
                }
                if (strLoginPwd != strLoginPwdConfirm)
                {
                    ShowMessageBox(Resources.Err.ERR_LOGINPWDCONF_FORMAT);
                    return false;
                }

                // 연락처 검사
                if (string.IsNullOrEmpty(strTelNo))
                {
                    ShowMessageBox(Resources.Err.ERR_TELNO_INPUT);
                    return false;
                }
                if (!Regex.Match(strTelNo, Resources.RegEx.REGEX_TELNO).Success)
                {
                    ShowMessageBox(Resources.Err.ERR_TELNO_FORMAT);
                    return false;
                }
            }
            else
            {
                // 패스워드 검사
                if (!string.IsNullOrEmpty(strLoginPwd))
                {
                    if (!Regex.Match(strLoginPwd, Resources.RegEx.REGEX_LOGINPWD).Success)
                    {
                        ShowMessageBox(Resources.Err.ERR_LOGINPWD_FORMAT);
                        return false;
                    }

                    // 확인용 패스워드 검사
                    if (string.IsNullOrEmpty(strLoginPwdConfirm))
                    {
                        ShowMessageBox(Resources.Err.ERR_LOGINPWDCONF_INPUT);
                        return false;
                    }
                    if (strLoginPwd != strLoginPwdConfirm)
                    {
                        ShowMessageBox(Resources.Err.ERR_LOGINPWDCONF_FORMAT);
                        return false;
                    }
                }
            }
            /*
            // 이메일 검사
            if (string.IsNullOrEmpty(strEmail))
            {
                ShowMessageBox(Resources.Err.ERR_EMAIL_INPUT);
                return false;
            }
            if (!Regex.Match(strEmail, Resources.RegEx.REGEX_EMAIL).Success)
            {
                ShowMessageBox(Resources.Err.ERR_EMAIL_FORMAT);
                return false;
            }*/

            return true;
        }
        protected bool validateInput(
            string strLoginPwd,
            string strLoginPwdConfirm,
            string strTelNo,
            string strEmail)
        {
            return validateInput(null, strLoginPwd, strLoginPwdConfirm, null, null, strTelNo, strEmail, null, null, false);
        }
        protected bool validateInput(
            string strLoginID,
            string strLoginPwd,
            string strLoginPwdConfirm,
            string strNickName,
            string strName,
            string strTelNo,
            string strEmail,
            string strBankNum,
            string strOwnerName)
        {
            return validateInput(strLoginID, strLoginPwd, strLoginPwdConfirm, strNickName, strName, strTelNo, strEmail, strBankNum, strOwnerName, true);
        }
        #endregion

        #region 사건처리부
        protected virtual void Page_PreInit(object sender, EventArgs e)
        {
            // 디비 초기 연결 작업 시작...
            _dbconn = new MSSQLAccess();
            _dbconn.DBServer = Defines.DB_HOST;
            _dbconn.DBPort = Defines.DB_PORT;
            _dbconn.DBName = Defines.DB_NAME;
            _dbconn.DBID = Defines.DB_USER;
            _dbconn.DBPwd = Defines.DB_PASS;
            _dbconn.Connect();
            // 디비 연결 끝

            // 페이지 설정 시작
            strCurDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _userip = Request.ServerVariables["REMOTE_ADDR"];

            Session.Timeout = Defines.SESSION_TIMEOUT;

            try
            {
                //readSiteConfig();                

                // 로그인한 유저의 개인정보 갱신
                if (AuthUser != null)
                {
                    System.Data.DataSet dsUser = DBConn.RunStoreProcedure(Constants.SP_GETUSER,
                        new string[] { "@id" }, new object[] { AuthUser.ID });

                    if (DataSetUtil.IsNullOrEmpty(dsUser))
                    {
                        Alert(Resources.Err.ERR_REQUIRED_AUTH, Defines.URL_LOGOUT);
                        return;
                    }                   
                }
            }
            catch
            {
                _isInited = false;
                return;
            }

            _isInited = true;
            // 페이지 설정 끝

            //환경설정
            int lang = 1;
            int count_per_page = 20;
            DataSet dsContent = DBConn.RunSelectQuery("SELECT TOP(1) * FROM tbl_config");
            if (!DataSetUtil.IsNullOrEmpty(dsContent))
            {
                lang = DataSetUtil.RowIntValue(dsContent, "lang", 0);
                count_per_page = DataSetUtil.RowIntValue(dsContent, "count_per_page", 0);
            }

            //언어설정 zh-CN
            if (lang == 2)
            {//중국어
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
            else
            {//조선어
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
            //페지당 표시수 설정
            PageBase.page_rows = count_per_page;

        }

        protected virtual void Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsInited)
            {
                ShowError(Resources.Err.ERR_CONFIG_INVALID);
            }
        }

        protected virtual void Page_Init(object sender, EventArgs e)
        {
            PageDataSource = null;
            checkAuth();
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            string strMsg = "";
            if (checkBlockOrLeave(out strMsg))
            {
                UserLogout();
                Alert(strMsg, Defines.URL_LOGIN);
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.Params["page"]))
                    PageNumber = Convert.ToInt32(Request.Params["page"]);

                InitControls();

                BindData();
            }
            else
                LoadData();
        }
        protected virtual void Page_PreRender(object sender, EventArgs e)
        {
        }
        protected virtual void LoadData()
        {
        }
        protected virtual void BindData()
        {
            if (PageDataSource == null)
                LoadData();

            if (getGridControl() != null)
            {
                int temp = getGridControl().PageSize;
                getGridControl().PageSize = page_rows;

                DataView dv = null;
                if (PageDataSource != null && PageDataSource.Tables.Count > 0)
                    dv = PageDataSource.Tables[0].DefaultView;

                if (temp != getGridControl().PageSize)
                {
                    PageNumber = 0;
                }
                else
                {
                    if (DataSetUtil.IsNullOrEmpty(PageDataSource))
                        PageNumber = 0;
                    else
                    {
                        int iPageCount = PageDataSource.Tables[0].Rows.Count / getGridControl().PageSize;
                        if (iPageCount < PageNumber)
                        {
                            PageNumber = iPageCount;
                        }

                        if (!string.IsNullOrEmpty(SortColumn))
                        {
                            string strSort = (SortDirection == SortDirection.Ascending) ? SortColumn : SortColumn + " DESC";
                            dv.Sort = strSort;
                        }
                    }
                }

                if (dv != null)
                {
                    getGridControl().PageIndex = PageNumber;
                    getGridControl().DataSource = dv;
                    getGridControl().DataBind();
                }
            }
        }
        
        protected virtual void InitControls()
        {
            //if (getGridControl() != null)
                //getGridControl().PageSize = PAGE_ROWS;
                //getGridControl().PageSize = SiteConfig.PageRows;
                //getGridControl().PageSize = Int32.Parse(Defines.PAGE_ROWS);
        }
        
        protected virtual GridView getGridControl()
        {
            return null;
        }

        protected virtual void Page_Unload(object sender, EventArgs e)
        {
            // 디비 연결 닫기
            if (_dbconn != null)
            {
                _dbconn.Disconnect();
                _dbconn = null;
            }
        }

        protected virtual void gvContent_PageIndexChange(object sender, GridViewPageEventArgs e)
        {
            PageNumber = e.NewPageIndex;
            BindData();
        }
        protected virtual void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected virtual void gvContent_Sorting(object sender, GridViewSortEventArgs e)
        {
            // 요청되는 정돈항이 이전과 같으면 정돈방향을 바꾼다
            if (e.SortExpression == SortColumn)
            {
                SortDirection = (SortDirection == SortDirection.Ascending) ?
                    SortDirection.Descending : SortDirection.Ascending;
            }
            else
            {
                // 정돈항에 새 정돈항을 넣어주고 정돈방향은 Descending 으로 설정한다.
                SortColumn = e.SortExpression;
                SortDirection = SortDirection.Descending;
            }

            BindData();

            SetSortHeaderText();
        }
        protected virtual void SetSortHeaderText()
        {
        }        
        #endregion

        #region 오버라이드함수
        protected override void InitializeCulture()
        {
            string strLang = getCookie(Constants.COOKIE_KEY_SITE_LANGUAGE);
            if (strLang == "en-US" ||
                strLang == "ko-KR" ||
                strLang == "ja-JP")
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(strLang);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(strLang);
            }

            base.InitializeCulture();
        }
        #endregion

    }
}