using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataAccess;

namespace retroplus.Common
{
    public class AjaxPageBase:Common.PageBase
    {
        public AjaxPageBase()
        {
        }

        protected override void Page_PreInit(object sender, EventArgs e)
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

            Session.Timeout = Defines.SESSION_TIMEOUT;
            _isInited = true;
        }

        protected override void Page_Init(object sender, EventArgs e)
        {
            checkAuth();
        }

        protected override void Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsInited)
                Response.End();
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected override void Page_Unload(object sender, EventArgs e)
        {
            base.Page_Unload(sender, e);
        }
    }
}