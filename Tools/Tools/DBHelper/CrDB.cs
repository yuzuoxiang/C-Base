using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Tools.DBHelper
{
    public class CrDB
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        static public string DBType
        {
            get
            {
                string dbtype = ConfigurationManager.AppSettings["DBTYPE"];
                if (string.IsNullOrEmpty(dbtype))
                {
                    return "sql";
                }
                else
                {
                    return dbtype;
                }
            }
        }

        static public IDBHelper Creator()
        {
            return new Tools.DBHelper.SqlDB();
        }

        static public IDBHelper Creator(string _dbtype)
        {
            switch (_dbtype.ToLower())
            {
                case "sql":
                    return new Tools.DBHelper.SqlDB();
                case "oledb":
                    return new Tools.DBHelper.OleDB();
                default:
                    return new Tools.DBHelper.SqlDB();
            }
        }
    }
}
