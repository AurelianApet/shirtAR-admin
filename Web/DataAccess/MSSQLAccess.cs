using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class MSSQLAccess
    {
        protected const int DBCONN_TIMEOUT = 30;            // 디비접속응답 대기시간
        private string m_strDBServer = "127.0.0.1";         // MSSQL서버의 주소
        private string m_strDBName = "master";              // 접근할 디비명
        private string m_strDBID = "sa";                    // 디비접속아이디
        private string m_strDBPwd = "sa";                   // 디비접속암호
        private string m_strDBPort = "1433";                // 디비접속포트
        private bool m_bIsConnected = false;                // TRUE: 디비접속, FALSE: 디비차단        
        protected SqlConnection m_dbConnection = null;      // 디비접속자

        protected SqlTransaction m_trans = null;

        protected static MSSQLAccess s_instance = null;

        public static MSSQLAccess getInstance()
        {
            if (s_instance == null)
            {
                s_instance = new MSSQLAccess();
                s_instance.Connect();
            }
            return s_instance;
        }

        public string DBServer
        {
            get
            {
                return m_strDBServer;
            }
            set
            {
                m_strDBServer = value;
            }
        }

        public string DBName
        {
            get
            {
                return m_strDBName;
            }
            set
            {
                m_strDBName = value;
            }
        }

        public string DBID
        {
            get
            {
                return m_strDBID;
            }
            set
            {
                m_strDBID = value;
            }
        }

        public string DBPwd
        {
            get
            {
                return m_strDBPwd;
            }
            set
            {
                m_strDBPwd = value;
            }
        }

        public string DBPort
        {
            get
            {
                return m_strDBPort;
            }
            set
            {
                m_strDBPort = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return m_bIsConnected;
            }
            set
            {
                m_bIsConnected = value;
            }
        }

        /// <summary>
        /// MSSQL서버에 접속한다.
        /// </summary>
        /// <returns>TRUE: 성공, FALSE: 실패</returns>
        public virtual void Connect()
        {
            if (IsConnected)
            {
                throw new Exception("자료기지에 이미 접속되어있습니다.");
            }
            else
            {
                string strTemp;

                strTemp = "Connection Timeout=" + DBCONN_TIMEOUT +
                    ";Password=" + this.DBPwd +
                    ";Persist Security Info=True;User ID=" + this.DBID +
                    ";Initial Catalog=" + this.DBName +
                    ";Data Source=" + this.DBServer + "," + this.DBPort;
                try
                {
                    if (m_dbConnection != null)
                    {
                        m_dbConnection.Close();
                        m_dbConnection.Dispose();
                    }
                    m_dbConnection = new SqlConnection(strTemp);
                    m_dbConnection.Open();
                }
                catch (Exception e)
                {
                    string strErrMsg = string.Format("디비서버에 접속할수 없습니다.");
                    m_dbConnection = null;
                    throw new Exception(strErrMsg);
                }
                this.IsConnected = true;
            }
        }

        public virtual void Disconnect()
        {
            if (this.IsConnected)
            {
                m_dbConnection.Close();
                m_dbConnection.Dispose();
                m_dbConnection = null;
                this.IsConnected = false;
            }
        }

        /// <summary>
        /// 파라메터화된 SELECT질문을 수행한다.
        /// </summary>
        /// <param name="strParamQuery">파라메터로 구성되는 질의문(예: "SELECT * FROM tblusrlist WHERE name={0}" )</param>
        /// <param name="strParamNames">파라메터이름(주의: 앞에 @기호가 꼭 붙어야 한다.)</param>
        /// <param name="strParamValues">파라메터값</param>
        /// <returns>데이터셋</returns>
        public virtual DataSet RunSelectQuery(string strParamQuery, string[] strParamNames, object[] paramValues)
        {
            if (strParamNames == null && paramValues == null)
                return RunSelectQuery(strParamQuery);

            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }

            if (strParamNames.Length != paramValues.Length)
            {
                // 파라메터수가 맞지 않음.
                throw new Exception("SelectQuery에서 파라메터이름과 값이 대응되어있지 않습니다.");
            }

            string strQuery = "";
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    sqlCommand.Transaction = m_trans;

                    for (int i = 0; i < strParamNames.Length; i++)
                    {
                        //sqlCommand.Parameters.Add(new SqlParameter(strParamNames[i], paramValues[i]));
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException e)
            {
                string strMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strMsg);
            }
            catch (Exception e)
            {
                string strMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strMsg);
            }
        }

        /// <summary>
        /// SELECT SQL질의를 수행하고 결과를 문자렬배렬형태로 돌려준다.
        /// </summary>
        /// <param name="strQuery">SQL 질의</param>
        /// <returns>데이타셋</returns>
        public virtual DataSet RunSelectQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    adapter.SelectCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.SelectCommand.Transaction = m_trans;
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException ex)
            {
                string strMsg = string.Format("질의오류:{0} {1}", strQuery, ex.Message);
                throw new Exception(strMsg);
            }
            catch (Exception ex)
            {
                string strMsg = string.Format("오류:{0}", ex.Message);
                throw new Exception(strMsg);
            }
        }

        /// <summary>
        /// 파라메터화된 UPDATE질문을 수행한다.
        /// </summary>
        /// <param name="strParamQuery">파라메터로 구성되는 질의문(예: "UPDATE tblusrlist SET name={0}" )</param>
        /// <param name="strParamNames">파라메터이름(주의: 앞에 @기호가 꼭 붙어야 한다.)</param>
        /// <param name="strParamValues">파라메터값</param>        
        public virtual void RunUpdateQuery(string strParamQuery, string[] strParamNames, object[] paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            if (strParamNames.Length != paramValues.Length)
            {
                // 파라메터수가 맞지 않음.
                throw new Exception("UpdateQuery에서 파라메터이름과 값이 대응되어있지 않습니다.");
            }
            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);

                    for (int i = 0; i < strParamNames.Length; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    sqlCommand.Transaction = m_trans;
                    adapter.UpdateCommand = sqlCommand;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        /// <summary>
        /// UPDATE 질의를 수행한다.
        /// </summary>
        /// <param name="strQuery">질의문</param>        
        public virtual void RunUpdateQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.UpdateCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.UpdateCommand.Transaction = m_trans;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        /// <summary>
        /// 파라메터화된 DELETE질문을 수행한다.
        /// </summary>
        /// <param name="strParamQuery">파라메터로 구성되는 질의문(예: "DELETE FROM tblusrlist WHERE name={0}" )</param>
        /// <param name="strParamNames">파라메터이름(주의: 앞에 @기호가 꼭 붙어야 한다.)</param>
        /// <param name="strParamValues">파라메터값</param>        
        public virtual void RunDeleteQuery(string strParamQuery, string[] strParamNames, object[] paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            if (strParamNames.Length != paramValues.Length)
            {
                // 파라메터수가 맞지 않음.
                throw new Exception("DeleteQuery에서 파라메터이름과 값이 대응되어있지 않습니다.");
            }
            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    for (int i = 0; i < strParamNames.Length; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    sqlCommand.Transaction = m_trans;
                    adapter.DeleteCommand = sqlCommand;
                    adapter.DeleteCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        /// <summary>
        /// DELETE 질의를 수행한다.
        /// </summary>
        /// <param name="strQuery">질의문</param>        
        public virtual void RunDeleteQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.DeleteCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.DeleteCommand.Transaction = m_trans;
                    adapter.DeleteCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RunNonQuery(string strParamQuery, string[] strParamNames, object[] paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }

            if (strParamNames.Length != paramValues.Length)
            {
                // 파라메터수가 맞지 않음.
                throw new Exception("NonQuery에서 파라메터이름과 값이 대응되어있지 않습니다.");
            }

            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames);
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    for (int i = 0; i < strParamNames.Length; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    sqlCommand.Transaction = m_trans;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunNonQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                lock (this)
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    sqlCommand.Transaction = m_trans;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }
        public virtual void RunNonQueryNoLimitTime(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                lock (this)
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    sqlCommand.Transaction = m_trans;
                    sqlCommand.CommandTimeout = 0;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual DataSet RunStoreProcedure(string strSPName, Dictionary<string, object> dicParams)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    SqlCommand selectCommand = new SqlCommand(strSPName, m_dbConnection);
                    selectCommand.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in dicParams)
                    {
                        object objValue = kvp.Value;
                        if (kvp.Value != null && Type.GetTypeCode(kvp.Value.GetType()) == TypeCode.String)
                            objValue = getUnicodeString(kvp.Value);

                        selectCommand.Parameters.AddWithValue(kvp.Key, objValue);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strSPName, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual DataSet RunStoreProcedure(string strSPName, string[] strParamNames, object[] paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    SqlCommand selectCommand = new SqlCommand(strSPName, m_dbConnection);
                    selectCommand.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < strParamNames.Length; i++)
                    {
                        if (paramValues[i] != null && Type.GetTypeCode(paramValues[i].GetType()) == TypeCode.String)
                            paramValues[i] = getUnicodeString(paramValues[i]);

                        selectCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strSPName, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual DataSet RunStoreProcedure(string strName, object[] objParams)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                DataSet ds = new DataSet();

                lock (this)
                {
                    SqlCommand selectCommand = new SqlCommand(strName, m_dbConnection);
                    selectCommand.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < objParams.Length; i++)
                    {
                        SqlParameter param = objParams[i] as SqlParameter;
                        if (param != null && Type.GetTypeCode(param.Value.GetType()) == TypeCode.String)
                            param.Value = getUnicodeString(param.Value);

                        selectCommand.Parameters.Add(objParams[i] as SqlParameter);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(ds);
                }
                return ds;
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strName, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual DataSet RunStoreProcedure(string strSPName)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand selectCommand = new SqlCommand(strSPName, m_dbConnection);
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.Transaction = m_trans;

                    adapter.SelectCommand = selectCommand;
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strSPName, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        /// <summary>
        /// 파라메터화된 INSERT질문을 수행한다.
        /// </summary>
        /// <param name="strParamQuery">파라메터로 구성되는 질의문(예: "INSERT INTO tblusrlist({0},{1}) VALUES({3},{4})" )</param>
        /// <param name="strParamNames">파라메터이름(주의: 앞에 @기호가 꼭 붙어야 한다.)</param>
        /// <param name="strParamValues">파라메터값</param>
        /// <param name="bReturnID">TRUE : 삽입한 레코드의 ID를 얻는다. FALSE : 삽입한 레코드 수를 얻는다.</param>
        /// <returns>bReturnID값에 따라서 ID혹은 레코드수를 돌려준다.</returns>
        public virtual long RunInsertQuery(string strParamQuery, string[] strParamNames, object[] paramValues, bool bReturnID)
        {
            string strQuery = "";
            try
            {
                if (bReturnID == true)
                {
                    strParamQuery = strParamQuery.Trim();
                    if (strParamQuery[strParamQuery.Length - 1] != ';')
                        strQuery += ";";
                    strParamQuery = string.Format("{0} SELECT @@IDENTITY", strParamQuery);

                    DataSet dsRet = this.RunSelectQuery(strParamQuery, strParamNames, paramValues);
                    return DataSetUtil.GetID(dsRet);
                }
                else
                {
                    if (!this.IsConnected)
                    {
                        throw new Exception("자료기지에 접속할수 없습니다.");
                    }

                    if (strParamNames.Length != paramValues.Length)
                    {
                        // 파라메터수가 맞지 않음.
                        string strErrMsg = "InsertQuery에서 파라메터이름과 값이 대응되어있지 않습니다.";
                        throw new Exception(strErrMsg);
                    }

                    long nRows = 0;
                    lock (this)
                    {
                        strQuery = string.Format(strParamQuery, strParamNames);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                        for (int i = 0; i < strParamNames.Length; i++)
                        {
                            sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                        }
                        sqlCommand.Transaction = m_trans;
                        adapter.InsertCommand = sqlCommand;
                        nRows = adapter.InsertCommand.ExecuteNonQuery();
                    }
                    return nRows;
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual long RunInsertQuery(string strParamQuery, string[] strParamNames, object[] paramValues)
        {
            return RunInsertQuery(strParamQuery, strParamNames, paramValues, false);
        }

        /// <summary>
        /// INSERT질의를 수행한다.
        /// </summary>
        /// <param name="strQuery">질의문</param>
        /// <param name="bReturnID">TRUE : 삽입한 레코드의 ID를 얻는다. FALSE : 삽입한 레코드 수를 얻는다.</param>
        /// <returns>bReturnID값에 따라서 ID혹은 레코드수를 돌려준다.</returns>
        public virtual long RunInsertQuery(string strQuery, bool bReturnID)
        {
            try
            {
                if (bReturnID == true)
                {
                    strQuery = strQuery.Trim();
                    if (strQuery[strQuery.Length - 1] != ';')
                        strQuery += ";";
                    strQuery = string.Format("{0} SELECT @@IDENTITY", strQuery);
                    DataSet dsRet = this.RunSelectQuery(strQuery);
                    return DataSetUtil.GetID(dsRet);
                }
                else
                {
                    if (!this.IsConnected)
                    {
                        throw new Exception("자료기지에 접속할수 없습니다.");
                    }

                    long nRows = 0;
                    lock (this)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = new SqlCommand(strQuery, m_dbConnection);
                        adapter.InsertCommand.Transaction = m_trans;
                        nRows = adapter.InsertCommand.ExecuteNonQuery();
                    }
                    return nRows;
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("질의오류:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("오류:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual long RunInsertQuery(string strQuery)
        {
            return RunInsertQuery(strQuery, false);
        }

        public virtual void BeginTrans()
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }

            if (m_trans != null)
                EndTrans();

            try
            {
                m_trans = m_dbConnection.BeginTransaction();
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("오류:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void BeginTrans(IsolationLevel iso)
        {
            if (!this.IsConnected)
            {
                throw new Exception("자료기지에 접속할수 없습니다.");
            }

            if (m_trans != null)
                EndTrans();

            try
            {
                m_trans = m_dbConnection.BeginTransaction(iso);
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("오류:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void EndTrans()
        {
            if (m_trans == null)
                return;

            try
            {
                m_trans.Commit();
                m_trans = null;
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("오류:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RollbackTrans()
        {
            if (m_trans == null)
                return;

            try
            {
                m_trans.Rollback();
                m_trans = null;
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("오류:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual SqlParameter MakeParam(string strParamName, ParameterDirection pdDirection, object objParamValue)
        {
            SqlParameter retParam = new SqlParameter(strParamName, objParamValue);
            retParam.Direction = pdDirection;

            return retParam;
        }

        public virtual SqlParameter MakeParam(string strParamName, SqlDbType paramType, ParameterDirection pdDirection, int iLen, object objParamValue)
        {
            SqlParameter retParam = new SqlParameter(strParamName, objParamValue);
            retParam.SqlDbType = paramType;
            retParam.Size = iLen;
            retParam.Direction = pdDirection;

            return retParam;
        }

        public string getUnicodeString(object objSrc)
        {
            if (objSrc == null)
                return "";

            if (string.IsNullOrEmpty(objSrc.ToString()))
                return "";

            return new string(Encoding.Unicode.GetChars(Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.UTF8.GetBytes(objSrc.ToString()))));
        }
    }
}
