using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQLDAL
{
    public class T_UnitCode
    {
        public DataSet GetUnitCode()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                                    FROM    Better_UnitCode
                                    ORDER BY ucName");

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        public DataSet GetUnitCodeByIds(string Ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"SELECT distinct *
                                    FROM    Better_UnitCode
                                    where ucID in ({0})", Ids));

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }
    }
}
