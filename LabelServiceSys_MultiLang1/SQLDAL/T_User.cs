using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_User
    {
        public bool addUser(Model.M_User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_User]
                               ([urNum]
                               ,[urPSW]
                               ,[urName]
                               ,[urSex]
                               ,[urAge]
                               ,[urStaffNum]
                               ,[urDept]
                               ,[urDuty]
                               ,[urUnitCode]
                               ,[urMemo]
                               ,[urDelflag])
                         VALUES
                               (@urNum
                               ,@urPSW
                               ,@urName
                               ,@urSex
                               ,@urAge
                               ,@urStaffNum
                               ,@urDept
                               ,@urDuty
                               ,@urUnitCode
                               ,@urMem
                               ,@urDelflag)");

            SqlParameter[] parameters = {
                    new SqlParameter("@urNum",SqlDbType.NVarChar),
                    new SqlParameter("@urPSW",SqlDbType.NVarChar ),
                    new SqlParameter("@urName", SqlDbType.NVarChar),
                    new SqlParameter("@urSex",SqlDbType.Int),
                    new SqlParameter("@urAge",SqlDbType.Int),
                    new SqlParameter("@urStaffNum",SqlDbType.NVarChar),
                    new SqlParameter("@urDept",SqlDbType.NVarChar),
                    new SqlParameter("@urDuty",SqlDbType.NVarChar),
                    new SqlParameter("@urUnitCode",SqlDbType.NVarChar),
                    new SqlParameter("@urMem",SqlDbType.NVarChar),
                    new SqlParameter("@urDelflag",SqlDbType.Int)
            };
            parameters[0].Value = model.urNum;
            parameters[1].Value = model.urPSW;
            parameters[2].Value = model.urName;
            parameters[3].Value = model.urSex;
            parameters[4].Value = model.urAge;
            parameters[5].Value = model.urStaffNum;
            parameters[6].Value = model.urDept;
            parameters[7].Value = model.urDuty;
            parameters[8].Value = model.urUnitCode;
            parameters[9].Value = model.urMemo;
            parameters[10].Value = model.urDelflag;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateUser(Model.M_User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_User]
                           SET [urNum] = @urNum
                              ,[urPSW] =@urPSW
                              ,[urName] =@urName
                              ,[urSex] =@urSex
                              ,[urAge] =@urAge
                              ,[urStaffNum] =@urStaffNum
                              ,[urDept] =@urDept
                              ,[urDuty] =@urDuty
                              ,[urUnitCode] =@urUnitCode
                              ,[urMemo] =@urMemo
                         WHERE urID=@urID");

            SqlParameter[] parameters = {
                    new SqlParameter("@urNum",SqlDbType.NVarChar),
                    new SqlParameter("@urPSW",SqlDbType.NVarChar ),
                    new SqlParameter("@urName", SqlDbType.NVarChar),
                    new SqlParameter("@urSex",SqlDbType.Int),
                    new SqlParameter("@urAge",SqlDbType.Int),
                    new SqlParameter("@urStaffNum",SqlDbType.NVarChar),
                    new SqlParameter("@urDept",SqlDbType.NVarChar),
                    new SqlParameter("@urDuty",SqlDbType.NVarChar),
                    new SqlParameter("@urUnitCode",SqlDbType.NVarChar),
                    new SqlParameter("@urMemo",SqlDbType.NVarChar),
                    new SqlParameter("@urID",SqlDbType.Int)
            };
            parameters[0].Value = model.urNum;
            parameters[1].Value = model.urPSW;
            parameters[2].Value = model.urName;
            parameters[3].Value = model.urSex;
            parameters[4].Value = model.urAge;
            parameters[5].Value = model.urStaffNum;
            parameters[6].Value = model.urDept;
            parameters[7].Value = model.urDuty;
            parameters[8].Value = model.urUnitCode;
            parameters[9].Value = model.urMemo;
            parameters[10].Value = model.urID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool UserExists(string userNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Better_User]");
            strSql.Append(" WHERE (urNum = '" + userNum + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UserExists(int userID, string userNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Better_User]");
            strSql.Append(" WHERE (urNum = '" + userNum + "' and urID<>" + userID + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetUserByurID(string urID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM [Better_User] where urID=" + urID);

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

        public DataSet GetUserByurIDs(string urIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM [Better_User] where urID in (" + urIDs + ")");

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

        public DataSet GetUserByurNumber(string urNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM [Better_User] where urNum='" + urNumber + "'");

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

        public bool deleteUsers(string urIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_User] where urID in (" + urIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="urIDs"></param>
        /// <returns></returns>
        public bool enableUsers(string urIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [Better_User] set urDelflag=0 where urID in (" + urIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="urIDs"></param>
        /// <returns></returns>
        public bool disableUsers(string urIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [Better_User] set urDelflag=1 where urID in (" + urIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetJobNameL1ByUserNumber(string userNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  l1.vchar_WorkProcess_L1_Code ,
                                        L1.int_id ,
                                        l1.vchar_WorkProcess_L1_Text
                                FROM    tb_Setting_WorkProcess_L1 L1
                                        INNER JOIN Better_User BU ON bu.urUnitCode = L1.vchar_gateway
                                WHERE   L1.bit_IsUse = 1
                                        AND bu.urNum = '{0}'
                                ORDER BY L1.vchar_WorkProcess_L1_Code ASC",
                                                                 userNumber);

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


        public DataSet GetJobNameL2ByJobNameL1Id(string JobNameL1Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  vchar_WorkProcess_L2_Code,int_id,vchar_WorkProcess_L2_Text
                                FROM    tb_Setting_WorkProcess_L2
                                WHERE   int_L1_id ={0}
                                ORDER BY vchar_WorkProcess_L2_Code ASC", JobNameL1Id);

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

        //        public DataSet GetJobNameL2ByJobNameL1Code(string JobName1Code)
        //        {
        //            StringBuilder strSql = new StringBuilder();
        //            strSql.AppendFormat(@"SELECT  vchar_WorkProcess_L2_Code ,
        //                                        int_id ,
        //                                        vchar_WorkProcess_L2_Text
        //                                FROM    tb_Setting_WorkProcess_L2
        //                                WHERE   int_L1_id = ( SELECT TOP 1
        //                                                                int_id
        //                                                      FROM      tb_Setting_WorkProcess_L1
        //                                                      WHERE     vchar_WorkProcess_L1_Code = '{0}'
        //                                                    )
        //                                ORDER BY vchar_WorkProcess_L2_Code ASC", JobName1Code);

        //            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                return ds;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }

        public DataSet GetJobNameL2ByJobNameL1Code(string JobNameL1Code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  vchar_WorkProcess_L2_Code ,
                                        SWL2.int_id ,
                                        vchar_WorkProcess_L2_Text
                                FROM    tb_Setting_WorkProcess_L2 SWL2
                                        INNER JOIN ( SELECT int_id
                                                     FROM   tb_Setting_WorkProcess_L1
                                                     WHERE  vchar_WorkProcess_L1_Code = '{0}'
                                                            AND bit_IsUse = 1
                                                   ) T ON T.int_id = SWL2.int_L1_id
                                                          AND SWL2.bit_IsUse = 1
                                ORDER BY vchar_WorkProcess_L2_Code ASC", JobNameL1Code);

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
