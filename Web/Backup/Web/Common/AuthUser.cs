using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace retroplus.Common
{
    /// <summary>
    /// Summary description for AuthUser
    /// </summary>
    public class AuthUser
    {
        protected long _id = 0;
        public long ID
        {
            get { return _id; }
        }

        protected string _nickname = null;
        public string Nickname
        {
            get { return _nickname; }
        }

        protected string _loginid = null;
        public string LoginID
        {
            get { return _loginid; }
        }

        protected string _loginpwd = null;
        public string LoginPwd
        {
            get { return _loginpwd; }
        }

        protected int _ulevel = 0;
        public int ULevel
        {
            get { return _ulevel; }
            set { _ulevel = value; }
        }
        public bool IsAdmin
        {
            get { return _ulevel >= Constants.SITE_MAX_USERLEVEL; }
        }
        public AuthUser()
        {
            _id = 0;
            _nickname = null;
            _loginid = null;
            _loginpwd = null;            
        }

        public AuthUser(
            long lID,
            string strNickname,
            string strLoginID,
            string strLoginPwd,            
            int iULevel)
        {
            _id = lID;
            _nickname = strNickname;
            _loginid = strLoginID;
            _loginpwd = strLoginPwd;            
            _ulevel = iULevel;
        }
    }
}