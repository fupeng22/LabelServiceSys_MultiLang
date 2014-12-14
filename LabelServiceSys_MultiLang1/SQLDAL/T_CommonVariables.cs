using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQLDAL
{
    public class T_CommonVariables
    {
        public DataSet GetCommonVariable(string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  *
                                    FROM    Better_CommonVariables
                                    WHERE   vType = '{0}'
                                    ORDER BY vName",type);

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
