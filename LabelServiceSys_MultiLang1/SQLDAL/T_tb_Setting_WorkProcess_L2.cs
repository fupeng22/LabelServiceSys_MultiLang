using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_tb_Setting_WorkProcess_L2
    {
        public bool HasL2(string L1_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [tb_Setting_WorkProcess_L2]");
            strSql.Append(" WHERE (int_L1_id = " + L1_id + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteJobName2(string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [tb_Setting_WorkProcess_L2] where int_id in (" + ids + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool addJobName2(Model.M_tb_Setting_WorkProcess_L2 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [tb_Setting_WorkProcess_L2]
                                   ([int_L1_id]
                                   ,[vchar_WorkProcess_L2_Code]
                                   ,[vchar_WorkProcess_L2_Text]
                                   ,[bit_IsUse]
                                   ,[vchar_CCTV_workstation_id]
                                   ,[vchar_created_user]
                                   ,[dttm_created_dttm])
                             VALUES
                                   (@int_L1_id
                                   ,@vchar_WorkProcess_L2_Code
                                   ,@vchar_WorkProcess_L2_Text
                                   ,@bit_IsUse
                                   ,@vchar_CCTV_workstation_id
                                   ,@vchar_created_user
                                   ,GetDate())");

            SqlParameter[] parameters = {
                    new SqlParameter("@int_L1_id",SqlDbType.Int),
                    new SqlParameter("@vchar_WorkProcess_L2_Code",SqlDbType.NVarChar ),
                    new SqlParameter("@vchar_WorkProcess_L2_Text", SqlDbType.NVarChar),
                    new SqlParameter("@bit_IsUse",SqlDbType.Bit),
                    new SqlParameter("@vchar_CCTV_workstation_id",SqlDbType.NVarChar),
                    new SqlParameter("@vchar_created_user",SqlDbType.NVarChar)
            };
            parameters[0].Value = model.int_L1_id;
            parameters[1].Value = model.vchar_WorkProcess_L2_Code;
            parameters[2].Value = model.vchar_WorkProcess_L2_Text;
            parameters[3].Value = model.bit_IsUse;
            parameters[4].Value = model.vchar_CCTV_workstation_id;
            parameters[5].Value = model.vchar_created_user;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateJobName2(Model.M_tb_Setting_WorkProcess_L2 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [tb_Setting_WorkProcess_L2]
                            SET [int_L1_id] =@int_L1_id
                                ,[vchar_WorkProcess_L2_Code] = @vchar_WorkProcess_L2_Code
                                ,[vchar_WorkProcess_L2_Text] = @vchar_WorkProcess_L2_Text
                                ,[bit_IsUse] = @bit_IsUse
                                ,[vchar_CCTV_workstation_id] = @vchar_CCTV_workstation_id where int_id=@int_id");

            SqlParameter[] parameters = {
                    new SqlParameter("@int_L1_id",SqlDbType.Int),
                    new SqlParameter("@vchar_WorkProcess_L2_Code",SqlDbType.NVarChar ),
                    new SqlParameter("@vchar_WorkProcess_L2_Text", SqlDbType.NVarChar),
                    new SqlParameter("@bit_IsUse",SqlDbType.Bit),
                    new SqlParameter("@vchar_CCTV_workstation_id",SqlDbType.NVarChar),
                    new SqlParameter("@int_id",SqlDbType.Int)
            };
            parameters[0].Value = model.int_L1_id;
            parameters[1].Value = model.vchar_WorkProcess_L2_Code;
            parameters[2].Value = model.vchar_WorkProcess_L2_Text;
            parameters[3].Value = model.bit_IsUse;
            parameters[4].Value = model.vchar_CCTV_workstation_id;
            parameters[5].Value = model.int_id;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public DataSet GetL1_L2InfoByPositionNO(string PositionNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT TOP 1
                                        SWL1.int_id ,
                                        SWL1.vchar_WorkProcess_L1_Code ,
                                        SWL1.vchar_WorkProcess_L1_Text ,
                                        SWL2.int_id AS L2_int_id ,
                                        SWL2.vchar_WorkProcess_L2_Code ,
                                        SWL2.vchar_WorkProcess_L2_Text
                                FROM    tb_Setting_WorkProcess_L1 SWL1
                                        INNER JOIN tb_Setting_WorkProcess_L2 SWL2 ON SWL1.int_id = SWL2.int_L1_id
                                WHERE   SWL1.bit_IsUse = 1
                                        AND SWL2.bit_IsUse = 1
                                        AND SWL2.vchar_CCTV_workstation_id = ( SELECT TOP 1
                                                                                        CCTV_workstation_id
                                                                               FROM     Better_Position_CCTV_Workstation
                                                                               WHERE    PositionNO = {0}
                                                                             )", PositionNO);

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

        public bool ExistJobName2Code(string JobName2Code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [tb_Setting_WorkProcess_L2]");
            strSql.Append(" WHERE (vchar_WorkProcess_L2_Code = '" + JobName2Code + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExistJobName2Code(string JobName2Code, string int_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [tb_Setting_WorkProcess_L2]");
            strSql.Append(" WHERE (vchar_WorkProcess_L2_Code = '" + JobName2Code + "') and int_id<>" + int_Id);

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetCCTVWorkstationId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  SWL2.*
                            FROM    tb_Setting_WorkProcess_L2 SWL2
                                    INNER JOIN dbo.tb_Setting_WorkProcess_L1 SWL1 ON SWL2.int_L1_id = SWL1.int_id
                            WHERE   SWL1.bit_IsUse = 1
                                    AND SWL2.bit_IsUse = 1
                            ORDER BY SWL2.vchar_CCTV_workstation_id");

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

        public DataSet GetCCTVWorkstationIdByGateWays(string UnitCodeIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  SWP2.* ,
                                        UC.ucID
                                FROM    tb_Setting_WorkProcess_L2 SWP2
                                        INNER JOIN tb_Setting_WorkProcess_L1 SWP1 ON SWP2.int_L1_id = SWP1.int_id
                                        INNER  JOIN Better_UnitCode UC ON UC.ucName = SWP1.vchar_gateway
                                WHERE   SWP1.bit_IsUse = 1
                                        AND SWP2.bit_IsUse = 1
                                        AND UC.ucID IN ( {0} )
                                ORDER BY SWP2.vchar_CCTV_workstation_id", UnitCodeIds);

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

        public bool EnableOrDisableJobName2(string IsUse, string Ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"UPDATE [tb_Setting_WorkProcess_L2]
                           SET [bit_IsUse] ={0} WHERE int_id in ({1})", IsUse, Ids));
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
        /// 根据工作内容1禁用工作内容2
        /// </summary>
        /// <param name="IsUse"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool EnableOrDisableJobName2ByJobId1(string IsUse, string Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"UPDATE [tb_Setting_WorkProcess_L2]
                           SET [bit_IsUse] ={0} WHERE int_L1_id in ({1})", IsUse, Id));
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
