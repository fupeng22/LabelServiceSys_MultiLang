using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_OperationLog_Pcid
    {
        public bool addOperationLog_Pcid(Model.M_OperationLog_Pcid model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_OperationLog_Pcid]
                                   ([pcidop_urID]
                                   ,[pcidopJobNameIdLv1]
                                   ,[pcidopJobNameIdLv2]
                                   ,[pcidopContent]
                                   ,[pcidopDateTime]
                                   ,[pcidopIsDvir])
                             VALUES
                                   (@pcidop_urID
                                   ,@pcidopJobNameIdLv1
                                   ,@pcidopJobNameIdLv2
                                   ,@pcidopContent
                                   ,@pcidopDateTime
                                   ,@pcidopIsDvir)");

            SqlParameter[] parameters = {
                    new SqlParameter("@pcidop_urID",SqlDbType.Int),
                    new SqlParameter("@pcidopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@pcidopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@pcidopContent",SqlDbType.NVarChar),
                    new SqlParameter("@pcidopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@pcidopIsDvir",SqlDbType.Int)
            };
            parameters[0].Value = model.pcidop_urID;
            parameters[1].Value = model.pcidopJobNameIdLv1;
            parameters[2].Value = model.pcidopJobNameIdLv2;
            parameters[3].Value = model.pcidopContent;
            parameters[4].Value = model.pcidopDateTime;
            parameters[5].Value = model.pcidopIsDvir;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateOperationLog_Pcid(Model.M_OperationLog_Pcid model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_OperationLog_Pcid]
                                   SET [pcidop_urID] =@pcidop_urID
                                      ,[pcidopJobNameIdLv1] =@pcidopJobNameIdLv1
                                      ,[pcidopJobNameIdLv2] =@pcidopJobNameIdLv2
                                      ,[pcidopContent] =@pcidopContent
                                      ,[pcidopDateTime] =@pcidopDateTime
                                      ,[pcidopIsDvir] =@pcidopIsDvir
                                 WHERE pcidopID=@pcidopID");

            SqlParameter[] parameters = {
                    new SqlParameter("@pcidop_urID",SqlDbType.Int),
                    new SqlParameter("@pcidopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@pcidopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@pcidopContent",SqlDbType.NVarChar),
                    new SqlParameter("@pcidopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@pcidopIsDvir",SqlDbType.Int),
                    new SqlParameter("@pcidopID",SqlDbType.Int)
            };
            parameters[0].Value = model.pcidop_urID;
            parameters[1].Value = model.pcidopJobNameIdLv1;
            parameters[2].Value = model.pcidopJobNameIdLv2;
            parameters[3].Value = model.pcidopContent;
            parameters[4].Value = model.pcidopDateTime;
            parameters[5].Value = model.pcidopIsDvir;
            parameters[6].Value = model.pcidopID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool deleteOperationLog_Pcid(string opIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_OperationLog_Pcid] where pcidopID in (" + opIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool insertOperationLog_PcidFromUsernum(string strUserNum, string data, string isDVIR)
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
                            strSql.AppendFormat(@"INSERT INTO Better_OperationLog_Pcid
                                    ( pcidop_urID ,
                                      pcidopJobNameIdLv1 ,
                                      pcidopJobNameIdLv2 ,
                                      pcidopContent ,
                                      pcidopDateTime ,
                                      pcidopIsDvir
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
