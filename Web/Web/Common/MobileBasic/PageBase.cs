using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataAccess;

namespace retroplus.Common.MobileBasic
{
    public class PageBase : System.Web.UI.Page
    {
        public const string DEVICE_ANDROID = "Android";
        public const string DEVICE_IOS = "iOS";

        protected MSSQLAccess _dbconn = null;
        public MSSQLAccess DBConn
        {
            get
            {
                return _dbconn;
            }
        }

        protected virtual void Page_PreInit(object sender, EventArgs e)
        {
            _dbconn = new MSSQLAccess();
            _dbconn.DBServer = Defines.DB_HOST;
            _dbconn.DBPort = Defines.DB_PORT;
            _dbconn.DBName = Defines.DB_NAME;
            _dbconn.DBID = Defines.DB_USER;
            _dbconn.DBPwd = Defines.DB_PASS;
            _dbconn.Connect();
        }
        protected virtual void Page_UnLoad(object sender, EventArgs e)
        {
            // 디비 연결 닫기
            if (_dbconn != null)
            {
                _dbconn.Disconnect();
                _dbconn = null;
            }
        }
    }
}