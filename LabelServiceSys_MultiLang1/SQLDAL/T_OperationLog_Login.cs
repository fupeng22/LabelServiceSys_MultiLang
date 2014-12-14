using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_OperationLog_Login
    {
        public bool addOperationLog_Login(Model.M_OperationLog_Login model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_OperationLog_Login]
                                   ([lgop_urID]
                                   ,[lgopJobNameIdLv1]
                                   ,[lgopJobNameIdLv2]
                                   ,[lgopContent]
                                   ,[lgopDateTime]
                                   ,[lgopIsDvir])
                             VALUES
                                   (@lgop_urID
                                   ,@lgopJobNameIdLv1
                                   ,@lgopJobNameIdLv2
                                   ,@lgopContent
                                   ,@lgopDateTime
                                   ,@lgopIsDvir)");

            SqlParameter[] parameters = {
                    new SqlParameter("@lgop_urID",SqlDbType.Int),
                    new SqlParameter("@lgopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@lgopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@lgopContent",SqlDbType.NVarChar),
                    new SqlParameter("@lgopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@lgopIsDvir",SqlDbType.Int)
            };
            parameters[0].Value = model.lgop_urID;
            parameters[1].Value = model.lgopJobNameIdLv1;
            parameters[2].Value = model.lgopJobNameIdLv2;
            parameters[3].Value = model.lgopContent;
            parameters[4].Value = model.lgopDateTime;
            parameters[5].Value = model.lgopIsDvir;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateOperationLog_Login(Model.M_OperationLog_Login model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_OperationLog_Login]
                               SET [lgop_urID] = @lgop_urID
                                  ,[lgopJobNameIdLv1] = @lgopJobNameIdLv1
                                  ,[lgopJobNameIdLv2] =@lgopJobNameIdLv2
                                  ,[lgopContent] =@lgopContent
                                  ,[lgopDateTime] =@lgopDateTime
                                  ,[lgopIsDvir] =@lgopIsDvir
                             WHERE lgopID=@lgopID");

            SqlParameter[] parameters = {
                     new SqlParameter("@lgop_urID",SqlDbType.Int),
                    new SqlParameter("@lgopJobNameIdLv1",SqlDbType.NVarChar ),
                    new SqlParameter("@lgopJobNameIdLv2", SqlDbType.NVarChar),
                    new SqlParameter("@lgopContent",SqlDbType.NVarChar),
                    new SqlParameter("@lgopDateTime",SqlDbType.DateTime),
                    new SqlParameter("@lgopIsDvir",SqlDbType.Int),
                    new SqlParameter("@lgopID",SqlDbType.Int)
            };
            parameters[0].Value = model.lgop_urID;
            parameters[1].Value = model.lgopJobNameIdLv1;
            parameters[2].Value = model.lgopJobNameIdLv2;
            parameters[3].Value = model.lgopContent;
            parameters[4].Value = model.lgopDateTime;
            parameters[5].Value = model.lgopIsDvir;
            parameters[6].Value = model.lgopID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool deletOperationLog_Login(string opIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_OperationLog_Login] where lgopID in (" + opIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetLogin(string userNum)
        {
            StringBuilder strSql = new StringBuilder();
//            strSql.AppendFormat(@"SELECT 
//                                    BOLL.lgopID ,
//                                    BOLL.lgop_urID ,
//                                    BOLL.lgopJobNameIdLv1 ,
//                                    BOLL.lgopJobNameIdLv2 ,
//                                    BOLL.lgopContent ,
//                                    BOLL.lgopDateTime ,
//                                    BOLL.lgopIsDvir ,
//                                    T.urID ,
//                                    T.urNum ,
//                                    T.urPSW ,
//                                    T.urName ,
//                                    T.urSex ,
//                                    T.urAge ,
//                                    T.urStaffNum ,
//                                    T.urDept ,
//                                    T.urDuty ,
//                                    T.urUnitCode ,
//                                    T.urDelflag
//                            FROM    Better_OperationLog_Login AS BOLL
//                                    INNER JOIN ( SELECT urID ,
//                                                        urNum ,
//                                                        urPSW ,
//                                                        urName ,
//                                                        urSex ,
//                                                        urAge ,
//                                                        urStaffNum ,
//                                                        urDept ,
//                                                        urDuty ,
//                                                        urUnitCode ,
//                                                        urMemo ,
//                                                        urDelflag
//                                                 FROM   dbo.Better_User
//                                                 WHERE  ( urNum = '{0}' )
//                                               ) AS T ON T.urID = BOLL.lgop_urID
//                            ORDER BY BOLL.lgopDateTime DESC", userNum);

            strSql.AppendFormat(@"SELECT  BOLL.lgopID ,
                                        BOLL.lgop_urID ,
                                        BOLL.lgopJobNameIdLv1 ,
                                        BOLL.lgopJobNameIdLv2 ,
                                        SWL1.vchar_WorkProcess_L1_Text ,
                                        SWL2.vchar_WorkProcess_L2_Text ,
                                        BOLL.lgopContent ,
                                        BOLL.lgopDateTime ,
                                        BOLL.lgopIsDvir ,
                                        T.urID ,
                                        T.urNum ,
                                        T.urPSW ,
                                        T.urName ,
                                        T.urSex ,
                                        T.urAge ,
                                        T.urStaffNum ,
                                        T.urDept ,
                                        T.urDuty ,
                                        T.urUnitCode ,
                                        T.urDelflag
                                FROM    Better_OperationLog_Login AS BOLL
                                        INNER JOIN ( SELECT urID ,
                                                            urNum ,
                                                            urPSW ,
                                                            urName ,
                                                            urSex ,
                                                            urAge ,
                                                            urStaffNum ,
                                                            urDept ,
                                                            urDuty ,
                                                            urUnitCode ,
                                                            urMemo ,
                                                            urDelflag
                                                     FROM   dbo.Better_User
                                                     WHERE  ( urNum = '{0}' )
                                                   ) AS T ON T.urID = BOLL.lgop_urID
                                        INNER JOIN tb_Setting_WorkProcess_L1 SWL1 ON BOLL.lgopJobNameIdLv1 = SWL1.vchar_WorkProcess_L1_Code
                                        INNER JOIN tb_Setting_WorkProcess_L2 SWL2 ON BOLL.lgopJobNameIdLv2 = SWL2.vchar_WorkProcess_L2_Code
                                WHERE   SWL2.int_L1_id = SWL1.int_id
                                        AND SWL1.bit_IsUse = 1
                                        AND SWL2.bit_IsUse = 1
                                ORDER BY BOLL.lgopDateTime DESC", userNum);

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public Boolean LoginOut(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT  INTO dbo.Better_OperationLog_Login
                                    ( lgop_urID ,
                                      lgopJobNameIdLv1 ,
                                      lgopJobNameIdLv2 ,
                                      lgopContent ,
                                      lgopDateTime ,
                                      lgopIsDvir
                                    )
                                    SELECT TOP 1
                                            BOLL.lgop_urID ,
                                            BOLL.lgopJobNameIdLv1 ,
                                            BOLL.lgopJobNameIdLv2 ,
                                            'LOGOUT' ,
                                            GETDATE() ,
                                            BOLL.lgopIsDvir
                                    FROM    dbo.Better_OperationLog_Login AS BOLL
                                            INNER JOIN ( SELECT urID ,
                                                                urNum ,
                                                                urPSW ,
                                                                urName ,
                                                                urSex ,
                                                                urAge ,
                                                                urStaffNum ,
                                                                urDept ,
                                                                urDuty ,
                                                                urUnitCode ,
                                                                urMemo ,
                                                                urDelflag
                                                         FROM   dbo.Better_User
                                                         WHERE  ( urNum = '{0}' )
                                                       ) AS T ON T.urID = BOLL.lgop_urID
                                    ORDER BY BOLL.lgopDateTime DESC",userName);
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Login(string userName,string JobNameL1Code,string JobNameL2Code,string IsDVIR)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT  INTO dbo.Better_OperationLog_Login
                                        ( lgop_urID ,
                                          lgopJobNameIdLv1 ,
                                          lgopJobNameIdLv2 ,
                                          lgopContent ,
                                          lgopDateTime ,
                                          lgopIsDvir
                                        )
                                VALUES  ( ( SELECT  urID
                                            FROM    Better_User
                                            WHERE   urNum = '{0}'
                                          ) ,
                                          '{1}' ,
                                          '{2}' ,
                                          'LOGIN' ,
                                          GETDATE() ,
                                          '{3}'
                                        )
                                ", userName, JobNameL1Code, JobNameL2Code, IsDVIR);
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
