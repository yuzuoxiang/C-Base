using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace Tools.DBHelper
{
    /// <summary>
    /// 
    /// <remarks>功能描述：辅助数据库格式转化</remarks>
    /// </summary>
    public class DBOperat
    {
        /// <summary>
        /// 将DataRow转化成HashTable
        /// </summary>
        /// <param name="rows">DataRow类型对象</param>
        /// <returns>如果rows为null返回一个空值的Hashtable实例</returns>
        public static Hashtable DataRow2Hashtable(DataRow rows)
        {
            Hashtable record = new Hashtable();
            if (rows == null)
            {
                return record;
            }

            for (int i = 0; i < rows.Table.Columns.Count; i++)
            {
                object cellValue = rows[i];
                if (cellValue.GetType() == typeof(DBNull))
                {
                    cellValue = null;
                }
                if (cellValue != null && "System.DateTime".Equals(rows[i].GetType().FullName))
                {
                    DateTime dt = (DateTime)rows[i];
                    cellValue = dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                record[rows.Table.Columns[i].ColumnName] = cellValue;
            }
            return record;
        }

        /// <summary>
        /// 将DataTable里的所有DataRow转化成ArrayList(Hashtable)格式
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static ArrayList DataTable2Array(DataTable dataTable)
        {
            ArrayList rows = new ArrayList(dataTable.Rows.Count);
            foreach (DataRow row in dataTable.Rows)
            {
                rows.Add(DataRow2Hashtable(row));
            }
            return rows;
        }
    }
}
