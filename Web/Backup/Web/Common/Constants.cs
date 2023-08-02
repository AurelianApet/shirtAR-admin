using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace retroplus.Common
{
    public class Constants
    {
        #region 쿠키 관련 상수
        public const string COOKIE_KEY_SITE = "RETROPLUS::COOKIE::KEY";
        public const string COOKIE_KEY_SITE_LANGUAGE = "RETROPLUS::COOKIE::KEY::SITE::LANGUAGE";
        public const string COOKIE_KEY_USERINFO = "RETROPLUS::COOKIE::KEY::USERINFO";
        public const string COOKIE_KEY_REMEBERID = "RETROPLUS::COOKIE::LOGIN::REMEMBERID";
        public const string COOKIE_KEY_AUTOLOGIN = "RETROPLUS::COOKIE::LOGIN::AUTOLOGIN";

        public const string COOKIE_KEY_MOBILETYPE = "RETROPLUS::COOKIE::KEY::MOBILETYPE";
        #endregion

        #region 세션 관련 상수
        public const string SESSION_KEY_USERINFO = "RETROPLUS::SESSION::KEY::USERINFO";
        public const string SESSION_KEY_AUTOLOGOUT = "RETROPLUS::SESSION::KEY::AUTOLOGOUT";
        public const string SESSION_KEY_MOBILETYPE = "RETROPLUS::SESSION::KEY::MOBILETYPE";
        #endregion

        #region 모바일기기 형태
        public const string MOBILE_IPHONE = "iPhone";
        public const string MOBILE_ANDROID = "Android";
        public const string MOBILE_IPAD = "iPad";

        // Japan Mobile
        public const string MOBILE_DOCOMO = "DoCoMo";
        public const string MOBILE_KDDI = "KDDI";
        public const string MOBILE_SOFTBANK = "SoftBank";
        public const string MOBILE_EMOBILE = "eMobile";
        public const string MOBILE_PHS = "PHS";
        public const string MOBILE_UNKNOWN = "Unknown";
        #endregion

        #region Stored Procedure 이름 정의
        public static string SP_GETUSER = "sp_getUser";
        public static string SP_GETLOGINS = "sp_getLogins";
        
        
        #endregion

        #region 사이트관련상수
        public const string SITE_SEPERATOR = ":";
        public const char SITE_SEPERATOR_CHAR = ':';

        public const int SITE_MAX_USERLEVEL = 10;       // 유저의 최대레벨값
        public const int SITE_RECENT_NOTICECOUNT = 5;        // 최근 게시글 개수

        public const string SITE_SELECTALL = "All";    // 검색시 전체선택값

        public const string EVENT_LOGIN = "로그인";
        public const string EVENT_LOGOUT = "로그아웃";
        public const string EVENT_ADDUSER = "계정추가";
        public const string EVENT_DELUSER = "계정삭제";
        public const string EVENT_MODUSER = "계정수정";
        public const string EVENT_ADDPRODUCT = "상품등록";
        public const string EVENT_DELPRODUCT = "상품삭제";
        public const string EVENT_MODPRODUCT = "상품수정";
        public const string EVENT_ADDNOTICE = "공지사항등록";
        public const string EVENT_DELNOTICE = "공지사항삭제";
        public const string EVENT_MODNOTICE = "공지사항수정";
        public const string EVENT_CHANGEPWD = "메인관리자 PWD 변경";

        public const string EVENT_ADDPROJECT = "켐페인그룹생성";
        public const string EVENT_DELPROJECT = "켐페인그룹삭제";
        public const string EVENT_ADDCAMPAIGN = "켐페인생성";
        public const string EVENT_DELCAMPAIGN = "켐페인삭제";
        public const string EVENT_MODCAMPAIGN = "켐페인수정";        

        #endregion

        #region 뷰스테이트 관련 상수
        public const string VS_PAGENUMBER = "PageNumber";
        public const string VS_DATASOURCE = "DataSource";

        public const string VS_STARTDATE = "StartDate";
        public const string VS_ENDDATE = "EndDate";

        public const string VS_SORTCOLUMN = "SortColumn";
        public const string VS_SORTDIRECTION = "SortDirection";

        public const string VS_SEARCHDATE = "SearchDate";
        #endregion
    }
}