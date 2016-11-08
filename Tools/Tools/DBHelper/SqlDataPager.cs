using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Tools.DBHelper
{
    public class SqlDataPager
    {
        private List<DBHelper.DBParam> _dbplist;
        public SqlDataPager()
        {
            this.PageSize = 20;
            this.PageNum = 1;
        }

        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public string TableName { get; set; }

        public string Sort { get; set; }

        public string Fields { get; set; }

        public string Filter { get; set; }

        public string DBName { get; set; }

        public List<DBHelper.DBParam> DBParamList 
        {
            set { _dbplist = value; }
        }

        public string TestSqlStr { get; set; }

        public int Count()
        {
            string whstr = "";
            if (string.IsNullOrEmpty(Filter)==false)
            {
                whstr = " WHERE " + Filter;
            }

            IDBHelper db = CrDB.Creator();
            db.DBName = DBName;
            string s;
            if (_dbplist == null || _dbplist.Count == 0)
            {
                s = db.Value(" SELECT COUNT(0) AS V FROM " + TableName + whstr);
            }
            else
            {
                s = db.Value(_dbplist, " SELECT COUNT(0) AS V FROM " + TableName + whstr);
            }
            int n;
            if (int.TryParse(s, out n))
            {
                return n;
            }
            else
            {
                return 0;
            }
        }

        public DataTable ExecDt()
        {
            string whstr = "";
            if (string.IsNullOrEmpty(Filter) == false)
            {
                whstr = " WHERE " + Filter;
            }
            if (string.IsNullOrEmpty(Sort) == false)
            {
                Sort = " ORDER BY " + Sort;
            }
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            if (PageNum == 1)
            {
                sql.Append("SELECT TOP " + PageSize.ToString() + " " + Fields + " FROM " + TableName + whstr + Sort);

            }
            else
            {
                int snum = (PageNum - 1) * PageSize + 1;
                int endm = PageNum * PageSize;

                sql.Append("SELECT * ");
                sql.Append(" FROM ");
                sql.Append("(SELECT ROW_NUMBER() OVER (");
                sql.Append(Sort);
                sql.Append(") AS row_number,");
                sql.Append(Fields);
                sql.Append(" FROM ");
                sql.Append(TableName);
                sql.Append(whstr);
                sql.Append(") AS t WHERE t.row_number BETWEEN " + snum + " AND " + endm);
            }

            IDBHelper db = CrDB.Creator();
            db.DBName = DBName;

            this.TestSqlStr = sql.ToString();
            if (_dbplist == null)
            {
                return db.Dt(sql.ToString());
            }
            else
            {
                return db.Dt(_dbplist, sql.ToString());
            }  
        }

    }
}
