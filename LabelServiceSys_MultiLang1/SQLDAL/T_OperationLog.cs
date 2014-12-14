using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_OperationLog
    {
        public bool addOperationLog(Model.M_OperationLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_OperationLog]
                               ([op_urID]
                               ,[op_urStaffNum]
                               ,[op_urUnitCode]
                               ,[opJobNameIdLv1]
                               ,[opJobNameIdLv2]
                               ,[opType]
                               ,[opContent]
                               ,[opDateTime])
                         VALUES
                               (@op_urID
                               ,@op_urStaffNum
                               ,@op_urUnitCode
                               ,@opJobNameIdLv1
                               ,@opJobNameIdLv2
                               ,@opType
                               ,@opContent
                               ,@opDateTime)");

            SqlParameter[] parameters = {
                    new SqlParameter("@op_urID",SqlDbType.Int),
                    new SqlParameter("@op_urStaffNum",SqlDbType.NVarChar ),
                    new SqlParameter("@op_urUnitCode", SqlDbType.NVarChar),
                    new SqlParameter("@opJobNameIdLv1",SqlDbType.NVarChar),
                    new SqlParameter("@opJobNameIdLv2",SqlDbType.NVarChar),
                    new SqlParameter("@opType",SqlDbType.Int),
                    new SqlParameter("@opContent",SqlDbType.NVarChar),
                    new SqlParameter("@opDateTime",SqlDbType.DateTime)
            };
            parameters[0].Value = model.op_urID;
            parameters[1].Value = model.op_urStaffNum;
            parameters[2].Value = model.op_urUnitCode;
            parameters[3].Value = model.opJobNameIdLv1;
            parameters[4].Value = model.opJobNameIdLv2;
            parameters[5].Value = model.opContent;
            parameters[6].Value = model.opDateTime;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateOperationLog(Model.M_OperationLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_OperationLog]
                               SET [op_urID] = @op_urID
                                  ,[op_urStaffNum] =@op_urStaffNum
                                  ,[op_urUnitCode] =@op_urUnitCode
                                  ,[opJobNameIdLv1] =@opJobNameIdLv1
                                  ,[opJobNameIdLv2] =@opJobNameIdLv2
                                  ,[opType] =@opType
                                  ,[opContent] =@opContent
                                  ,[opDateTime] =@opDateTime
                             WHERE opID=@opID");

            SqlParameter[] parameters = {
                    new SqlParameter("@op_urID",SqlDbType.Int),
                    new SqlParameter("@op_urStaffNum",SqlDbType.NVarChar ),
                    new SqlParameter("@op_urUnitCode", SqlDbType.NVarChar),
                    new SqlParameter("@opJobNameIdLv1",SqlDbType.NVarChar),
                    new SqlParameter("@opJobNameIdLv2",SqlDbType.NVarChar),
                    new SqlParameter("@opType",SqlDbType.Int),
                    new SqlParameter("@opContent",SqlDbType.NVarChar),
                    new SqlParameter("@opDateTime",SqlDbType.DateTime)
            };
            parameters[0].Value = model.op_urID;
            parameters[1].Value = model.op_urStaffNum;
            parameters[2].Value = model.op_urUnitCode;
            parameters[3].Value = model.opJobNameIdLv1;
            parameters[4].Value = model.opJobNameIdLv2;
            parameters[5].Value = model.opContent;
            parameters[6].Value = model.opDateTime;
            parameters[7].Value = model.opID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool deleteOperationLog(string opIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_OperationLog] where opID in (" + opIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
