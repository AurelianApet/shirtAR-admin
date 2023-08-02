using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace retroplus.Common
{
    /// <summary>
    /// Summary description for Config
    /// </summary>
    public class Config
    {
        #region 설정값속성
        // 사이트 제목
        private string _title = null;
        public string Title
        {
            get { return _title; }
        }

        // 회원가입 여부
        private int _member_join = 0;
        public bool MemberJoin
        {
            get
            {
                return (_member_join == 0) ? false : true;
            }
        }
                
        // 디비정리시에 삭제할 자료의 지난일수
        private int _auto_delete = 0;
        public int AutoDelete
        {
            get { return _auto_delete; }
        }

        // 한 페이지에 표시할 행수
        private int _page_rows = 0;
        public int PageRows
        {
            get { return (_page_rows > 0) ? _page_rows : 15; }
        }

        // 접속자로 판단하는 시간, 단위는 분
        // 이 시간이 지나면 접속자목록에서 삭제
        private int _login_minutes = 0;
        public int LoginMinutes
        {
            get { return _login_minutes; }
        }

        // 접속차단아이피목록, \n으로 구분
        private string _intercept_ip = null;
        public string InterceptIP
        {
            get { return _intercept_ip; }
        }
                
        // 아이디, 닉네임으로 이용할수 없는 단어목록: ,로 구분
        private string _prohibit_id = null;
        public string ProhibitID
        {
            get { return _prohibit_id; }
        }

        // 회원가입약관
        private string _stipulation = null;
        public string Stipulation
        {
            get { return _stipulation; }
        }

        // 개인정보취급방침
        private string _privacy = null;
        public string Privacy
        {
            get { return _privacy; }
        }

        // marquee로 전달되는 공지문장
        private string _quicknotice = null;
        public string QuickNotice
        {
            get { return _quicknotice; }
        }

        // 알림판에 현시할 내용
        private string _noticepad = null;
        public string NoticePad
        {
            get { return _noticepad; }
        }

        // 회원가입시 초기 레벨
        private int _join_level = 0;
        public int JoinLevel
        {
            get { return _join_level; }
        }

        // 로그인화면에서 로그인카운트 증가시간간격
        private long _logincounttime = 0;
        public long LoginCountTime
        {
            get { return _logincounttime; }
        }
        #endregion

        public Config()
        {
        }

        public void initConfig(
            string strTitle,
            int iMemberJoin,
            int iAutoDelete,
            int iPageRows,
            int iLoginMinutes,
            string strInterceptIP,
            string strProhibitID,
            string strStipulation,
            string strPrivacy,
            string strQuickNotice,
            string strNoticePad,
            long lLoginCountTime
            )
        {
            _title = strTitle;
            _member_join = iMemberJoin;
            _auto_delete = iAutoDelete;
            _page_rows = iPageRows;
            _login_minutes = iLoginMinutes;
            _intercept_ip = strInterceptIP;
            _prohibit_id = strProhibitID;
            _stipulation = strStipulation;
            _privacy = strPrivacy;
            _quicknotice = strQuickNotice;
            _noticepad = strNoticePad;
            _logincounttime = lLoginCountTime;
        }
    }
}