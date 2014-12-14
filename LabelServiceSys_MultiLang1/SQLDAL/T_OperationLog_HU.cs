using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_OperationLog_HU
    {
        public bool addOperationLog_HU(Model.M_OperationLog_HU model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_OperationLog_HU]
                                   ([huop_urID]
                                   ,[huopJobNameIdLv1]
                                   ,[huopJobNameIdLv2]
                                   ,[huopContent]
                                   ,[huopDateTime]
                                   ,[huopIsDvir])
                             VALUES
                                   (@huop_urID
                                   ,@huopJobNameIdLv1
                                   ,@huopJobNameIdLv2
                                   ,@huopContent
                                   ,@huopDateTime
                                   ,@huopIsDvir)");

            SqlParameter[] parameters = {
                    new SqlParameter("@huop_urID",SqlDbType.Int),
                    new SqlParameter("@huopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@huopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@huopContent",SqlDbType.NVarChar),
                    new SqlParameter("@huopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@huopIsDvir",SqlDbType.Int)
            };
            parameters[0].Value = model.huop_urID;
            parameters[1].Value = model.huopJobNameIdLv1;
            parameters[2].Value = model.huopJobNameIdLv2;
            parameters[3].Value = model.huopContent;
            parameters[4].Value = model.huopDateTime;
            parameters[5].Value = model.huopIsDvir;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateOperationLog_HU(Model.M_OperationLog_HU model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_OperationLog_HU]
                               SET [huop_urID] =@huop_urID
                                  ,[huopJobNameIdLv1] =@huopJobNameIdLv1
                                  ,[huopJobNameIdLv2] =@huopJobNameIdLv2
                                  ,[huopContent] =@huopContent
                                  ,[huopDateTime] =@huopDateTime
                                  ,[huopIsDvir] =@huopIsDvir
                             WHERE huopID=@huopID");

            SqlParameter[] parameters = {
                     new SqlParameter("@huop_urID",SqlDbType.Int),
                    new SqlParameter("@huopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@huopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@huopContent",SqlDbType.NVarChar),
                    new SqlParameter("@huopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@huopIsDvir",SqlDbType.Int),
                     new SqlParameter("@huopID",SqlDbType.Int)
            };
            parameters[0].Value = model.huop_urID;
            parameters[1].Value = model.huopJobNameIdLv1;
            parameters[2].Value = model.huopJobNameIdLv2;
            parameters[3].Value = model.huopContent;
            parameters[4].Value = model.huopDateTime;
            parameters[5].Value = model.huopIsDvir;
            parameters[6].Value = model.huopID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool deleteOperationLog_HU(string opIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_OperationLog_HU] where huopID in (" + opIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool insertOperationLog_HUFromUsernum(string strUserNum, string data, string isDVIR)
        {
            StringBuilder strSql = new StringBuilder();

            DataSet ds = null;
            DataTable dt = null;
            ds = DBUtility.SqlServerHelper.Query(string.Format(@"SELECT TOP 1
                                                        *
                                                FROM    Better_OperationLog_Login BOL
                                                        INNER JOIN Better_User BU ON BU.urID = BOL.lgop_urID
                                                WHERE   BU.urNum = '{0}'
                                                ORDER BY BOL.lgopDateTime desc", strUserNum));
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["lgopContent"].ToString().ToUpper())
                    {
                        case "LOGIN":
                            strSql.AppendFormat(@"INSERT INTO Better_OperationLog_HU
                                    ( huop_urID ,
                                      huopJobNameIdLv1 ,
                                      huopJobNameIdLv2 ,
                                      huopContent ,
                                      huopDateTime ,
                                      huopIsDvir
                                    )
                                    SELECT TOP 1
                                            BOL.lgop_urID ,
                                            BOL.lgopJobNameIdLv1 ,
                                            BOL.lgopJobNameIdLv2 ,
                                            '{0}' ,
                                            GETDATE() ,
                                            '{1}'
                                    FROM    Better_OperationLog_Login BOL
                                            INNER
                            JOIN ( SELECT TOP 1
                                            urID
                                   FROM     Better_User
                                   WHERE    urNum = '{2}'
                                 ) T ON T.urID = BOL.lgop_urID
                                    ORDER BY lgopDateTime DESC", data, isDVIR, strUserNum);
                            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case "LOGOUT":
                            return false;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }
    }
}
