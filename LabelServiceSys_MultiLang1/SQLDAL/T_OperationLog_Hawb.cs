using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_OperationLog_Hawb
    {
        public bool addOperationLog_Hawb(Model.M_OperationLog_Hawb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_OperationLog_Hawb]
                                   ([hawbop_urID]
                                   ,[hawbopJobNameIdLv1]
                                   ,[hawbopJobNameIdLv2]
                                   ,[hawbopContent]
                                   ,[hawbopDateTime]
                                   ,[hawbopIsDvir])
                             VALUES
                                   (@hawbop_urID
                                   ,@hawbopJobNameIdLv1
                                   ,@hawbopJobNameIdLv2
                                   ,@hawbopContent
                                   ,@hawbopDateTime
                                   ,@hawbopIsDvir)");

            SqlParameter[] parameters = {
                    new SqlParameter("@hawbop_urID",SqlDbType.Int),
                    new SqlParameter("@hawbopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@hawbopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@hawbopContent",SqlDbType.NVarChar),
                    new SqlParameter("@hawbopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@hawbopIsDvir",SqlDbType.Int)
            };
            parameters[0].Value = model.hawbop_urID;
            parameters[1].Value = model.hawbopJobNameIdLv1;
            parameters[2].Value = model.hawbopJobNameIdLv2;
            parameters[3].Value = model.hawbopContent;
            parameters[4].Value = model.hawbopDateTime;
            parameters[5].Value = model.hawbopIsDvir;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateOperationLog_Hawb(Model.M_OperationLog_Hawb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_OperationLog_Hawb]
                               SET [hawbop_urID] =@hawbop_urID
                                  ,[hawbopJobNameIdLv1] =@hawbopJobNameIdLv1
                                  ,[hawbopJobNameIdLv2] =@hawbopJobNameIdLv2
                                  ,[hawbopContent] =@hawbopContent
                                  ,[hawbopDateTime] =@hawbopDateTime
                                  ,[hawbopIsDvir] =@hawbopIsDvir
                             WHERE hawbopID=@hawbopID");

            SqlParameter[] parameters = {
                     new SqlParameter("@hawbop_urID",SqlDbType.Int),
                    new SqlParameter("@hawbopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@hawbopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@hawbopContent",SqlDbType.NVarChar),
                    new SqlParameter("@hawbopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@hawbopIsDvir",SqlDbType.Int),
                    new SqlParameter("@hawbopID",SqlDbType.Int)
            };
            parameters[0].Value = model.hawbop_urID;
            parameters[1].Value = model.hawbopJobNameIdLv1;
            parameters[2].Value = model.hawbopJobNameIdLv2;
            parameters[3].Value = model.hawbopContent;
            parameters[4].Value = model.hawbopDateTime;
            parameters[5].Value = model.hawbopIsDvir;
            parameters[6].Value = model.hawbopID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool deleteOperationLog_Hawb(string opIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_OperationLog_Hawb] where hawbopID in (" + opIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool insertOperationLog_HawbFromUsernum(string strUserNum, string data, string isDVIR)
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
                            strSql.AppendFormat(@"INSERT  INTO Better_OperationLog_Hawb
                                    ( hawbop_urID ,
                                      hawbopJobNameIdLv1 ,
                                      hawbopJobNameIdLv2 ,
                                      hawbopContent ,
                                      hawbopDateTime ,
                                      hawbopIsDvir
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
