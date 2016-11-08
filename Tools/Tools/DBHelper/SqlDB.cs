using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Tools.DBHelper
{
    public class SqlDB:IDBHelper
    {
        private string logpath;
        private string _connstr;
        /// <summary>
        /// 数据库连接xml存放路径
        /// </summary>
        private readonly string xmlPath = @"E:\www\Inter\dell.xml";
        /// <summary>
        /// 默认链接的数据库名
        /// </summary>
        private readonly string defaultDataName = "efudb1";
        private readonly string key = "efuaaaaa";

        /// <summary>
        /// 错误日志路径
        /// </summary>
        public string LogPath
        {
            get 
            {
                string logpath = ConfigurationManager.AppSettings["logpath"];
                if (string.IsNullOrEmpty(logpath))
                {
                    return "";
                }
                else
                {
                    return logpath;
                }
            }
            set 
            {
                logpath = value;
            }
        }

        /// <summary>
        /// 从XML文件获取数据库链接
        /// </summary>
        public string DBName
        {
            get
            {
                return this._connstr;
            }
            set 
            {
                try
                {
                    string str;
                    if (string.IsNullOrEmpty(value))
                    {
                        str = defaultDataName;
                    }
                    else
                    {
                        str = value.Trim();
                    }
                    
                    //加载XML中的，根据数据库名称查找数据连接
                    XmlDocument xmldoc;
                    if (HttpRuntime.Cache["ConnXml"] == null)
                    {
                        string xmlPath2 = @"E:\www\intel\dell.xml";
                        xmldoc = new XmlDocument();
                        xmldoc.Load(xmlPath2);
                        System.Web.HttpRuntime.Cache.Add("ConnXml", xmldoc, null, DateTime.Now.AddHours(1)
                            , TimeSpan.Zero, CacheItemPriority.Default, null);
                    }
                    else
                    {
                        xmldoc = (XmlDocument)HttpRuntime.Cache["ConnXml"];
                    }

                    XmlNodeList elemList = xmldoc.GetElementsByTagName(str);
                    XmlNode node = elemList[0];
                    if (elemList.Count > 0)
                    {
                        string Temps = node.Attributes["ConnStr"].Value;
                        try
                        {
                            this._connstr = CryptHelper.DESEncrypt.Decrypt(Temps, key);
                        }
                        catch (Exception)
                        {
                            this._connstr = "";
                        }
                    }
                    else
                    {
                        this._connstr = "";
                    }
                }
                catch (Exception)
                {
                    this._connstr = "";   
                }
            }
        }

        public string ConnStr
        {
            get { return this._connstr; }
            set { this._connstr = value; }
        }

        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="message">自定义信息</param>
        /// <param name="error">错误信息</param>
        private void ErrorLog(string message, string error)
        {
            if (string.IsNullOrEmpty(this.LogPath))
                return;

            FileHelper.FilesOperat.DirectoryPathCreate(this.LogPath);
            StreamWriter fsoWrite = null;
            try
            {
                fsoWrite = new StreamWriter(string.Format("{0}\\{1}.txt", this.LogPath, DateTime.Now.ToString("yyyy-MM-dd").ToString())
                    , true, System.Text.Encoding.Default);
                fsoWrite.WriteLine(string.Format("时间:{0}\r\nIP:{1}\r\n自定义信息:{2}\r\n错误信息:{3}\r\n----------------------------------------------------------------------------------------\r\n\r\n"
                    , DateTime.Now.ToString(), GetClientIp(), message, error));
                fsoWrite.Close();
            }
            catch (Exception)
            {

            }
            finally
            {
                fsoWrite.Close();
            }
        }

        /// <summary>
        /// 更新SQL,返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Exec(string sql)
        {
            int n = 0;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Conn.Open();
                    n = Commd.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return n;
        }

        /// <summary>
        /// 更新SQL，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Exec(List<DBParam> dbparamList, string sql)
        {
            int n = 0;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    Conn.Open();
                    n = Commd.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    SqlErrorLog(sql, ex.Message);
                }
            }
            return n;
        }

        /// <summary>
        /// 查询SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public System.Data.DataTable Dt(string sql)
        {
            DataTable dt;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, Conn);
                    sda.Fill(ds);
                    dt = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    dt = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return dt;
        }

        /// <summary>
        /// 查询SQL，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public System.Data.DataTable Dt(List<DBParam> dbparamList, string sql)
        {
            DataTable dt;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    SqlDataAdapter sda = new SqlDataAdapter(Commd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    dt = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    dt = null;
                    StringBuilder sb = new StringBuilder();
                    foreach (DBParam d in dbparamList)
                    {
                        sb.Append(string.Format("{0}:{1}", d.FieldName,d.DbValue.ToString()));
                    }

                    SqlErrorLog(sql,ex.Message);
                }
            }

            return dt;
        }

        /// <summary>
        /// 查询SQL，返回一个数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public System.Data.DataSet Ds(string sql)
        {
            DataSet ds;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, Conn);
                    sda.Fill(ds);
                }
                catch (Exception ex)
                {
                    ds = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }
            return ds;
        }

        /// <summary>
        /// 查询SQL，返回一个数据集，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public System.Data.DataSet Ds(List<DBParam> dbparamList,string sql)
        {
            DataSet ds;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    SqlDataAdapter sda = new SqlDataAdapter(Commd);
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                catch (Exception ex)
                {
                    ds = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return ds;
        }

        /// <summary>
        /// 查询SQL，并指定表名，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public System.Data.DataSet Ds(List<DBParam> dbparamList, string sql, string tableName)
        {
            DataSet ds;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand commd = new SqlCommand(sql, Conn);
                    commd.Parameters.Clear();
                    commd = GetSqlParameter(dbparamList, commd);
                    SqlDataAdapter sda = new SqlDataAdapter(commd);
                    ds = new DataSet();
                    sda.Fill(ds, tableName);
                }
                catch (Exception ex)
                {
                    ds = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return ds;
        }

        public System.Data.DataSet Ds(string sql, string tableName)
        {
            DataSet ds;
            using (SqlConnection Conn = new SqlConnection(this._connstr))
            {
                try
                {
                    ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, Conn);
                    sda.Fill(ds, tableName);
                }
                catch (Exception ex)
                {
                    ds = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return ds;
        }

        /// <summary>
        /// 新增SQL，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Insert(List<DBParam> dbparamList, string sql)
        {
            int n = 0;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    Conn.Open();
                    n = Commd.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return n == 0 ? false : true;
        }

        /// <summary>
        /// 新增SQL并返回新增数据ID，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <param name="idName"></param>
        /// <returns></returns>
        public string Insert(List<DBParam> dbparamList, string sql, string idName)
        {
            string n;
            sql = string.Format("{0} SELECT @@IDENTITY as {1}", sql, idName);
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    Conn.Open();
                    n = Commd.ExecuteScalar().ToString();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    n = "0";
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return n;
        }

        /// <summary>
        /// 更新SQL，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool UpDate(List<DBParam> dbparamList, string sql)
        {
            bool teb;

            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    Conn.Open();
                    teb = Commd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    teb = false;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return teb;
        }

        /// <summary>
        /// 查询SQL，返回第一行第一列字段
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Value(string sql)
        {
            string n;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Conn.Open();
                    object o = Commd.ExecuteScalar();
                    Conn.Close();
                    n = o != null ? o.ToString() : null;
                }
                catch (Exception ex)
                {
                    n = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return n;
        }

        /// <summary>
        /// 查询SQL，返回第一行第一列字段，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Value(List<DBParam> dbparamList, string sql)
        {
            string n;
            using (SqlConnection Conn=new SqlConnection(this._connstr))
            {
                try
                {
                    SqlCommand Commd = new SqlCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamList, Commd);
                    Conn.Open();
                    object o = Commd.ExecuteScalar();
                    n = o != null ? o.ToString() : null;
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    n = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return n;
        }

        /// <summary>
        /// 查询SQL，返回DataReader，带参数
        /// </summary>
        /// <param name="dbparamList"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader SqlDr(List<DBParam> dbparamList, string sql)
        {
            SqlConnection Conn = new SqlConnection(this._connstr);
            try
            {
                SqlCommand Commd = new SqlCommand(sql, Conn);
                Commd.Parameters.Clear();
                Commd = GetSqlParameter(dbparamList, Commd);
                Conn.Open();
                return Commd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                SqlErrorLog(sql, ex.Message);
                Conn.Close();
                Conn.Dispose();
                return null;
            }
        }

        public System.Data.OleDb.OleDbDataReader OleDr(List<DBParam> dbparamlist, string sql)
        {
            return null;
        } 

        private SqlCommand GetSqlParameter(List<DBParam> dbparamList, SqlCommand Commd)
        {
            foreach (DBParam p in dbparamList)
            {
                SqlParameter sqlp = new SqlParameter();
                sqlp.ParameterName = p.FieldName;
                sqlp.SqlDbType = TypeToSqlType(p.DbType);

                if (p.DbType==null)
                {
                    sqlp.Value = Convert.DBNull;
                }
                else
                {
                    if (p.DbValue.GetType().ToString()=="System.DateTime")
                    {
                        if (DateTime.MinValue == (DateTime)p.DbValue)
                        {
                            sqlp.Value = Convert.DBNull;
                        }
                        else
                        {
                            sqlp.Value = p.DbValue;
                        }
                    }
                    else
                    {
                        sqlp.Value = p.DbValue;
                    }
                }

                Commd.Parameters.Add(sqlp);
            }

            return Commd;
        }

        private void SqlErrorLog(string sql,string message)
        {
            ErrorLog(string.Format("执行SQL:{0}语句出错", sql), message);
        }

        private SqlDbType TypeToSqlType(DataType dType)
        {
            switch (dType)
            {
                case DataType.DBBit:
                    return SqlDbType.Bit;
                case DataType.DBByte:
                    return SqlDbType.Binary;
                case DataType.DBChar:
                    return SqlDbType.Char;
                case DataType.DBDate:
                    return SqlDbType.Date;
                case DataType.DBDateTime:
                    return SqlDbType.DateTime;
                case DataType.DBDecimal:
                    return SqlDbType.Decimal;
                case DataType.DBFloat:
                    return SqlDbType.Float;
                case DataType.DBGuid:
                    return SqlDbType.UniqueIdentifier;
                case DataType.DBImage:
                    return SqlDbType.Image;
                case DataType.DBInt:
                    return SqlDbType.Int;
                case DataType.DBLong:
                    return SqlDbType.BigInt;
                case DataType.DBMoney:
                    return SqlDbType.Money;
                case DataType.DBSmallDateTime:
                    return SqlDbType.SmallDateTime;
                case DataType.DBStr:
                    return SqlDbType.NVarChar;
                case DataType.DBText:
                    return SqlDbType.Text;
                case DataType.DBNText:
                    return SqlDbType.NText;
                case DataType.DBTime:
                    return SqlDbType.Time;
                case DataType.DBVarBinary:
                    return SqlDbType.VarBinary;
                case DataType.DBVarChar:
                    return SqlDbType.VarChar;
                default:
                    return SqlDbType.NVarChar;
            }
        }

        private string GetClientIp()
        {
            try
            {
                string cip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(cip))
                {
                    cip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(cip))
                {
                    cip = HttpContext.Current.Request.UserHostAddress;
                }
                return cip;
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }
        }

    }
}
