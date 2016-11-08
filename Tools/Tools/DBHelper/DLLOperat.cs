using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

namespace Tools.DBHelper
{
    /// <summary>
    /// 基本操作数据库类
    /// </summary>
    public class DLLOperat
    {
        static readonly string conString = ConfigurationManager.AppSettings["conn"];

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecSQL(string sql)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(conString);
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                connection.Open();

                int i = command.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql">sql语句</param>        
        /// <param name="count">总记录数量</param>
        /// <param name="orderby">排序字段和方向,如id asc,name desc</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public static ArrayList SelectPager(string sql, ref long count, string where = null, string orderby = null,
            int pageNum = 1, int pageSize = 20, params IDataParameter[] parameter)
        {
            sql = "select * from " + sql;
            if (!string.IsNullOrEmpty(where))
            {
                sql = sql += " where " + where;
            }
            string countSQL = ("SELECT COUNT(*) FROM (" + sql + ") AAA ").ToUpper(CultureInfo.CurrentCulture);
            count = Convert.ToInt32(ExecuteScalar(countSQL, parameter), CultureInfo.CurrentCulture);
            sql = sql + (string.IsNullOrEmpty(orderby) ? "" : ("order by " + orderby));
            return DBHelper.DBOperat.DataTable2Array(SelectPagerMSSqlAccess(sql, orderby, pageNum, pageSize, parameter));
        }

        /// <summary>
        /// 附加智能查询的分页查询功能的实现
        /// </summary>
        /// <param name="table"></param>
        /// <param name="queryConeten"></param>
        /// <param name="count"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ArrayList SelectPagerCleaver(string table, string queryConeten, ref long count, string where = null,
            string order = null, int pageNum = 1, int pageSize = 20, params IDataParameter[] parameters)
        {
            if (queryConeten != "")
            {
                if (string.IsNullOrEmpty(where))
                {
                    where += "1=1";
                }
                where += " and (";
                var addCount = 0;
                DataTable dt = SelectSQL("select top 1 * from " + table);
                foreach (DataColumn column in dt.Columns)
                {
                    addCount++;
                    if (addCount == 1)
                    {
                        string cloumnName = column.ColumnName;
                        where += cloumnName + " like '%" + queryConeten + "%'";
                    }
                    else
                    {
                        string cloumnName = column.ColumnName;
                        where += " or " + cloumnName + " like '%" + queryConeten + "%'";
                    }
                }
                where += ")";
            }
            return SelectPager(table, ref count, where, order, pageNum, pageSize, parameters);
        }

        /// <summary>
        /// MSSql或者Acces的分页查询，表名不能是AAA，BBB
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="orderby">排序字段和方向,如id asc,name desc</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        private static DataTable SelectPagerMSSqlAccess(string sql, string orderby, int pageNum = 1, int pageSize = 20,
            params IDataParameter[] parameters)
        {
            StringBuilder SB = new StringBuilder();
            try
            {
                string[] orders = GetOrderBy(orderby, ref sql, parameters);
                SB.Append("select * from (");
                SB.Append("select top ").Append(pageSize * pageNum).Append(" *, ROW_NUMBER() OVER (" + orders[0] + ") ROWNUMBER")
                    .Append(" from (").Append(sql).Append(") AAA ");
                SB.Append(") BBB").Append(" where BBB.ROWNUMBER>").Append(pageSize * (pageNum - 1));
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return SelectSQL(SB.ToString(), parameters);
        }

        /// <summary>
        /// 获取反向orderby语句
        /// 返回2维数组，第一个为原始排序，第二个是反向排序
        /// 如果orderby为空[不需要排序]，系统自动以SQL的结果的第一列ASC排序
        /// </summary>
        /// <param name="orderby"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string[] GetOrderBy(string orderby, ref string sql, params IDataParameter[] parameters)
        {
            string[] orderZF = new string[2];
            if (string.IsNullOrEmpty(orderby))
            {
                //需要获取表的第一列列名
                DataTable dt = SelectSQL("select top 1 * from (" + sql + ") AAA", parameters);
                orderZF[0] = "order by " + dt.Columns[0].ColumnName;
                orderZF[1] = "order by " + dt.Columns[0].ColumnName + " desc";
                return orderZF;
            }
            orderZF[0] = "order by " + orderby;
            orderZF[1] = "order by " + new OrderByOperat(orderby).GetReverseOrder();
            return orderZF;
        }

        /// <summary>
        /// SQL的查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static DataTable SelectSQL(string sql, params IDataParameter[] parameters)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(sql))
                return dt;

            using (IDbConnection conn = new SqlConnection(conString))
            {
                IDataReader reader = null;
                try
                {
                    conn.Open();
                    IDbCommand cmd = PrepareCommand(conn, null, sql, CommandType.Text, parameters);
                    reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Dispose();
                    reader.Close();
                }
                catch (Exception e)
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Dispose();
                        reader.Close();
                    }
                    reader = null;
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// 执行一条SQL，并返回结果集的第一条第一列的值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params IDataParameter[] parameters)
        {
            using (IDbConnection iConn = new SqlConnection(conString))
            {
                try
                {
                    iConn.Open();
                    IDbCommand cmd = PrepareCommand(iConn, null, sql, CommandType.Text, parameters);
                    return cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (iConn != null)
                        iConn.Close();
                }
            }
        }
        
        /// <summary>
        /// SQL的执行命令设置
        /// </summary>
        /// <param name="iConn">连接</param>
        /// <param name="iTrans">事务</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="cmdType">sql类型</param>
        /// <param name="iParms">参数</param>
        /// <returns></returns>
        private static IDbCommand PrepareCommand(IDbConnection iConn, System.Data.IDbTransaction iTrans,
            string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] iParms)
        {
            if (iConn.State != ConnectionState.Open)
                iConn.Open();
            IDbCommand iCmd = new SqlCommand();
            iCmd.Connection = iConn;
            iCmd.CommandText = cmdText;
            if (iTrans != null)
                iCmd.Transaction = null;
            iCmd.CommandText = cmdText;
            if (iParms != null)
            {
                foreach (IDataParameter parm in iParms)
                    if (parm != null)
                        iCmd.Parameters.Add((SqlParameter)((ICloneable)parm).Clone());
            }
            return iCmd;
        }

        /// <summary>
        /// 单表查询
        /// </summary>        
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="top">取前几条 如"top 10"</param>
        /// <returns></returns>
        public static ArrayList Select(string table, string where = null, string order = null, string top = null)
        {
            if (string.IsNullOrEmpty(table))
                return new ArrayList(0);

            if (!string.IsNullOrEmpty(where))
                where = "where " + where;

            DataTable dt = new DataTable();
            using (IDbConnection iConn = new SqlConnection(conString))
            {
                IDataReader reader = null;
                try
                {
                    iConn.Open();
                    string sql = string.Format("select {0} * from {1} {2}", top, table, where);

                    if (!string.IsNullOrEmpty(order))
                    {
                        sql += string.Format(" order by {0}", order.Trim());
                    }

                    IDbCommand iCmd = new SqlCommand(sql, (SqlConnection)iConn);
                    reader = iCmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Dispose();
                    reader.Close();
                }
                catch (Exception e)
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Dispose();
                        reader.Close();
                    }
                    reader = null;
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (iConn != null)
                        iConn.Close();
                }
            }
            return DBHelper.DBOperat.DataTable2Array(dt);
        }

        /// <summary>
        /// 单表查询
        /// </summary>        
        /// <param name="sql"></param>      
        /// <returns></returns>
        public static ArrayList Select(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return new ArrayList(0);
            }

            DataTable dt = new DataTable();
            using (IDbConnection iConn = new SqlConnection(conString))
            {
                IDataReader reader = null;
                try
                {
                    iConn.Open();
                    IDbCommand iCmd = new SqlCommand(sql, (SqlConnection)iConn);
                    reader = iCmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Dispose();
                    reader.Close();
                }
                catch (Exception e)
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Dispose();
                        reader.Close();
                    }
                    reader = null;
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (iConn != null)
                        iConn.Close();
                }
            }
            return DBHelper.DBOperat.DataTable2Array(dt);
        }
    }
}
