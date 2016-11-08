using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Tools.DBHelper
{
    public interface IDBHelper
    {
        /// <summary>
        /// 日志路径
        /// </summary>
        string LogPath { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        string DBName { get; set; }

        /// <summary>
        /// 数据库链接
        /// </summary>
        string ConnStr { get; set; }

        /// <summary>
        /// 更新SQL,返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int Exec(string sql);
        /// <summary>
        /// 更新SQL，带参数
        /// </summary>
        /// <param name="dbparmlist"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        int Exec(List<DBParam> dbparmlist, string sql);
        /// <summary>
        /// 查询SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable Dt(string sql);
        /// <summary>
        /// 查询SQL（带参数）
        /// </summary>
        /// <param name="dbparmlist"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable Dt(List<DBParam> dbparmlist, string sql);
        /// <summary>
        /// 查询SQL返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataSet Ds(string sql);
        /// <summary>
        /// 根据表名查询语句返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataSet Ds(string sql, string tableName);
        /// <summary>
        /// 查询SQL返回DataSet（带参数）
        /// </summary>
        /// <param name="dbparmlist"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataSet Ds(List<DBParam> dbparmList, string sql);
        /// <summary>
        /// 根据表名查询语句返回DataSet（带参数）
        /// </summary>
        /// <param name="dbparmlist"></param>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataSet Ds(List<DBParam> dbparmList, string sql,string tableName);
        /// <summary>
        /// 新增SQL，返回是否成功
        /// </summary>
        /// <param name="dbparmlist"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool Insert(List<DBParam> dbparmList, string sql);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbparmlist"></param>
        /// <param name="sql"></param>
        /// <param name="idName"></param>
        /// <returns></returns>
        string Insert(List<DBParam> dbparmList, string sql, string getId);

        bool UpDate(List<DBParam> dbparmList, string sql);

        string Value(string sql);

        string Value(List<DBParam> dbparmList, string sql);

        System.Data.SqlClient.SqlDataReader SqlDr(List<DBParam> dbparmList, string sql);

        System.Data.OleDb.OleDbDataReader OleDr(List<DBParam> dbparmList, string sql);
    }
}
