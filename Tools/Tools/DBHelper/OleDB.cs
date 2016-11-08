using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Tools.DBHelper
{
    public class OleDB:IDBHelper
    {
        private string logpath;
        private string _connstr;

        public string LogPath
        {
            get
            {
                string logpath = ConfigurationManager.AppSettings["logpath"];
                if (string.IsNullOrEmpty(logpath))
                {
                    logpath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToLower();
                    return logpath.Replace(@".dll", "") + "\\errorlog";
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

        public string DBName
        {
            get
            {
                return this._connstr;
            }
            set
            {
                this._connstr = ConfigurationManager.AppSettings[value];
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
            Tools.FileHelper.FilesOperat.DirectoryPathCreate(this.LogPath);

            StreamWriter fsoWrite = new StreamWriter(string.Format("{0}\\{1}.txt", this.LogPath, DateTime.Now.ToString("yyyy-MM-dd"))
                ,true,System.Text.Encoding.Default);
            try
            {
                fsoWrite.WriteLine("时间:{0}\r\n自定义信息:{1}\r\n错误信息:{2}\r\n----------------------------------------------------------------------------------------\r\n\r\n"
                    , DateTime.Now.ToString(), message, error);
            }
            catch (Exception)
            {
            }
            finally
            {
                fsoWrite.Close();
            }
        }

        public int Exec(string sql)
        {
            int n = 0;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
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

        public int Exec(List<DBParam> dbparamlist, string sql)
        {
            int n = 0;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
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

        public System.Data.DataTable Dt(string sql)
        {
            DataTable dt;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    DataSet ds = new DataSet();
                    OleDbDataAdapter sda = new OleDbDataAdapter(sql, Conn);
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

        public System.Data.DataTable Dt(List<DBParam> dbparamlist, string sql)
        {
            DataTable dt;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    OleDbDataAdapter sda = new OleDbDataAdapter(Commd);
                    DataSet ds = new DataSet();
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

        public System.Data.DataSet Ds(string sql)
        {
            DataSet ds;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    ds = new DataSet();
                    OleDbDataAdapter sda = new OleDbDataAdapter(sql, Conn);
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

        public System.Data.DataSet Ds(List<DBParam> dbparamlist, string sql)
        {
            DataSet ds;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    OleDbDataAdapter sda = new OleDbDataAdapter(Commd);
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

        public System.Data.DataSet Ds(List<DBParam> dbparamlist, string sql, string tablename)
        {
            DataSet ds;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    OleDbDataAdapter sda = new OleDbDataAdapter(Commd);
                    ds = new DataSet();
                    sda.Fill(ds, tablename);

                }
                catch (Exception ex)
                {
                    ds = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return ds;
        }

        public System.Data.DataSet Ds(string sql, string tablename)
        {
            DataSet ds;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    ds = new DataSet();
                    OleDbDataAdapter sda = new OleDbDataAdapter(sql, Conn);
                    sda.Fill(ds, tablename);

                }
                catch (Exception ex)
                {
                    ds = null;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return ds;
        }

        public bool Insert(List<DBParam> dbparamlist, string sql)
        {
            int n = 0;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    Conn.Open();
                    n = Commd.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    SqlErrorLog(sql, ex.Message);
                }
            }

            if (n == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string Insert(List<DBParam> dbparamlist, string sql, string getid)
        {
            bool teb = false;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    Conn.Open();
                    Commd.ExecuteNonQuery().ToString();
                    Conn.Close();
                    teb = true;
                }
                catch (Exception ex)
                {

                    SqlErrorLog(sql, ex.Message);
                }
            }
            if (teb)
            {
                return this.Value(getid);
            }
            else
            {
                return "0";
            }

        }

        public bool UpDate(List<DBParam> dbparamlist, string sql)
        {
            bool teb;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    Conn.Open();
                    if (Commd.ExecuteNonQuery() > 0)
                    {
                        teb = true;
                    }
                    else
                    {
                        teb = false;
                    }
                    Conn.Close();

                }
                catch (Exception ex)
                {
                    teb = false;
                    SqlErrorLog(sql, ex.Message);
                }
            }

            return teb;
        }

        public string Value(string sql)
        {
            string n;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Conn.Open();
                    n = Commd.ExecuteScalar().ToString();
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

        public string Value(List<DBParam> dbparamlist, string sql)
        {
            string n;
            using (OleDbConnection Conn = new OleDbConnection(this._connstr))
            {
                try
                {
                    OleDbCommand Commd = new OleDbCommand(sql, Conn);
                    Commd.Parameters.Clear();
                    Commd = GetSqlParameter(dbparamlist, Commd);
                    Conn.Open();
                    n = Commd.ExecuteScalar().ToString();
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

        public System.Data.SqlClient.SqlDataReader SqlDr(List<DBParam> dbparamlist, string sql)
        {
            return null;
        }
        public OleDbDataReader OleDr(List<DBParam> dbparamlist, string sql)
        {
            OleDbConnection Conn = new OleDbConnection(this._connstr);

            try
            {
                OleDbCommand Commd = new OleDbCommand(sql, Conn);
                Commd.Parameters.Clear();
                Commd = GetSqlParameter(dbparamlist, Commd);
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

        private OleDbCommand GetSqlParameter(List<DBParam> dbparamlist, OleDbCommand Commd)
        {
            foreach (DBParam p in dbparamlist)
            {
                OleDbParameter olep = new OleDbParameter();
                olep.ParameterName = p.FieldName;
                olep.OleDbType = TypeToSqlType(p.DbType);
                if (p.DbValue == null)
                {
                    olep.Value = Convert.DBNull;
                }
                else
                {
                    olep.Value = p.DbValue;
                }

                Commd.Parameters.Add(olep);
            }

            return Commd;
        }

        private OleDbType TypeToSqlType(DataType dtype)
        {
            switch (dtype)
            {
                case DataType.DBBit:
                    return OleDbType.Boolean;
                case DataType.DBByte:
                    return OleDbType.Binary;
                case DataType.DBChar:
                    return OleDbType.Char;
                case DataType.DBDate:
                    return OleDbType.Date;
                case DataType.DBDateTime:
                    return OleDbType.Date;
                case DataType.DBDecimal:
                    return OleDbType.Decimal;
                case DataType.DBFloat:
                    return OleDbType.Double;
                case DataType.DBGuid:
                    return OleDbType.Guid;
                case DataType.DBImage:
                    return OleDbType.LongVarBinary;
                case DataType.DBInt:
                    return OleDbType.Integer;
                case DataType.DBMoney:
                    return OleDbType.Currency;
                case DataType.DBSmallDateTime:
                    return OleDbType.Date;
                case DataType.DBStr:
                    return OleDbType.VarWChar;
                case DataType.DBText:
                    return OleDbType.LongVarChar;
                case DataType.DBNText:
                    return OleDbType.LongVarWChar;
                case DataType.DBTime:
                    return OleDbType.DBTime;
                case DataType.DBVarBinary:
                    return OleDbType.VarBinary;
                case DataType.DBVarChar:
                    return OleDbType.VarChar;
                default:
                    return OleDbType.VarWChar;

            }
        }

        private void SqlErrorLog(string sql, string message)
        {
            ErrorLog(string.Format("执行SQL:{0}语句出错", sql), message);
        }
    }
}
