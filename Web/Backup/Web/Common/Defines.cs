using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace retroplus.Common
{
    /// <summary>
    /// web.config에 설정된 정보들을 읽어들이는 기능을 수행한다.
    /// </summary>
    public class Defines
    {
        public Defines()
        {
        }

        #region Database Connection 정보
        public static string DB_HOST
        {
            get { return ConfigurationManager.AppSettings["DB_HOST"]; }
        }
        public static string DB_PORT
        {
            get { return ConfigurationManager.AppSettings["DB_PORT"]; }
        }
        public static string DB_NAME
        {
            get { return ConfigurationManager.AppSettings["DB_NAME"]; }
        }
        public static string DB_USER
        {
            get { return ConfigurationManager.AppSettings["DB_USER"]; }
        }
        public static string DB_PASS
        {
            get { return ConfigurationManager.AppSettings["DB_PASS"]; }
        }
        public static string DB_BACKUP_PATH
        {
            get { return ConfigurationManager.AppSettings["DB_BACKUP_PATH"]; }
        }
        #endregion

        #region 페이지 관련 정보
        public static string URL_DOMAIN
        {
            get { return ConfigurationManager.AppSettings["URL_DOMAIN"]; }
        }
        public static string URL_LOGIN
        {
            get { return ConfigurationManager.AppSettings["URL_LOGIN"]; }
        }
        public static string URL_LOGOUT
        {
            get { return ConfigurationManager.AppSettings["URL_LOGOUT"]; }
        }
        public static string URL_DEFAULT
        {
            get { return ConfigurationManager.AppSettings["URL_DEFAULT"]; }
        }
        public static string URL_PREFIX_MEMBER
        {
            get { return ConfigurationManager.AppSettings["URL_PREFIX_MEMBER"]; }
        }
        public static string URL_PREFIX_MANAGE
        {
            get { return ConfigurationManager.AppSettings["URL_PREFIX_MANAGE"]; }
        }
        public static string URL_MOBILE
        {
            get { return ConfigurationManager.AppSettings["URL_MOBILE"]; }
        }
        public static string MAIL_ADMIN
        {
            get { return ConfigurationManager.AppSettings["MAIL_ADMIN"]; }
        }

        #endregion

        #region 쿠키 관련 정보
        public static bool COOKIE_INUSED
        {
            get { return (ConfigurationManager.AppSettings["COOKIE_INUSED"].ToLower() == "true"); }
        }
        public static int COOKIE_TIMEOUT
        {
            get
            {
                int iTimeOut = 0;
                return (int.TryParse(ConfigurationManager.AppSettings["COOKIE_TIMEOUT"], out iTimeOut) && iTimeOut > 0 ? iTimeOut : 24);
            }
        }
        #endregion

        #region 세션 관련 정보
        public static int SESSION_TIMEOUT
        {
            get
            {
                int iTimeOut = 0;
                return (int.TryParse(ConfigurationManager.AppSettings["SESSION_TIMEOUT"], out iTimeOut) && iTimeOut > 0 ? iTimeOut : 60);
            }
        }
        #endregion

        #region 푸시설정
        //안드로이드
        public static string PUSH_SECRET_KEY
        {
            get { return ConfigurationManager.AppSettings["PUSH_SECRET_KEY"]; }
        }
        public static string PUSH_API_KEY
        {
            get { return ConfigurationManager.AppSettings["PUSH_API_KEY"]; }
        }

        //IOS
        public static string PUSH_IOS_HOSTNAME
        {
            get { return ConfigurationManager.AppSettings["PUSH_IOS_HOSTNAME"]; }
        }
        public static int PUSH_IOS_PORT
        {
            get
            {
                int iPort = 0;
                return int.TryParse(ConfigurationManager.AppSettings["PUSH_IOS_PORT"], out iPort) ? iPort : 2195;
            }
        }
        public static string PUSH_IOS_CERTIFICATE_PATH
        {
            get { return ConfigurationManager.AppSettings["PUSH_IOS_CERTIFICATE_PATH"]; }
        }
        public static string PUSH_IOS_CERTIFICATE_PASSWORD
        {
            get { return ConfigurationManager.AppSettings["PUSH_IOS_CERTIFICATE_PASSWORD"]; }
        }
        #endregion
    }
}