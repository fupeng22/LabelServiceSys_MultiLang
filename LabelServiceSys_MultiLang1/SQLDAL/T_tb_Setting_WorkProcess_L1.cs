using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_tb_Setting_WorkProcess_L1
    {
        public DataSet GetJobNameL1()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM tb_Setting_WorkProcess_L1 ORDER BY vchar_WorkProcess_L1_Code ASC");

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

        public bool deleteJobName1(string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [tb_Setting_WorkProcess_L1] where int_id in (" + ids + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool addJobName1(Model.M_tb_Setting_WorkProcess_L1 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [tb_Setting_WorkProcess_L1]
                               ([vchar_gateway]
                               ,[vchar_WorkProcess_L1_Code]
                               ,[vchar_WorkProcess_L1_Text]
                               ,[bit_IsUse]
                               ,[vchar_created_user]
                               ,[dttm_created_dttm])
                         VALUES
                               (@vchar_gateway
                               ,@vchar_WorkProcess_L1_Code
                               ,@vchar_WorkProcess_L1_Text
                               ,@bit_IsUse
                               ,@vchar_created_user
                               ,GetDate())");

            SqlParameter[] parameters = {
                    new SqlParameter("@vchar_gateway",SqlDbType.NVarChar),
                    new SqlParameter("@vchar_WorkProcess_L1_Code",SqlDbType.NVarChar ),
                    new SqlParameter("@vchar_WorkProcess_L1_Text", SqlDbType.NVarChar),
                    new SqlParameter("@bit_IsUse",SqlDbType.Bit),
                    new SqlParameter("@vchar_created_user",SqlDbType.NVarChar)
            };
            parameters[0].Value = model.vchar_gateway;
            parameters[1].Value = model.vchar_WorkProcess_L1_Code;
            parameters[2].Value = model.vchar_WorkProcess_L1_Text;
            parameters[3].Value = model.bit_IsUse;
            parameters[4].Value = model.vchar_created_user;
          
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateJobName1(Model.M_tb_Setting_WorkProcess_L1 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [tb_Setting_WorkProcess_L1]
                           SET [vchar_gateway] = @vchar_gateway
                              ,[vchar_WorkProcess_L1_Code] = @vchar_WorkProcess_L1_Code
                              ,[vchar_WorkProcess_L1_Text] = @vchar_WorkProcess_L1_Text
                              ,[bit_IsUse] = @bit_IsUse WHERE int_id=@int_id");

            SqlParameter[] parameters = {
                    new SqlParameter("@vchar_gateway",SqlDbType.NVarChar),
                    new SqlParameter("@vchar_WorkProcess_L1_Code",SqlDbType.NVarChar ),
                    new SqlParameter("@vchar_WorkProcess_L1_Text", SqlDbType.NVarChar),
                    new SqlParameter("@bit_IsUse",SqlDbType.Bit),
                    new SqlParameter("@int_id",SqlDbType.Int)
            };
            parameters[0].Value = model.vchar_gateway;
            parameters[1].Value = model.vchar_WorkProcess_L1_Code;
            parameters[2].Value = model.vchar_WorkProcess_L1_Text;
            parameters[3].Value = model.bit_IsUse;
            parameters[4].Value = model.int_id;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool ExistJobName1Code(string JobName1Code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [tb_Setting_WorkProcess_L1]");
            strSql.Append(" WHERE (vchar_WorkProcess_L1_Code = '" + JobName1Code + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExistJobName1Code(string JobName1Code,string int_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [tb_Setting_WorkProcess_L1]");
            strSql.Append(" WHERE (vchar_WorkProcess_L1_Code = '" + JobName1Code + "') and int_id<>"+int_Id);

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EnableOrDisableJobName1(string IsUse, string Ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"UPDATE [tb_Setting_WorkProcess_L1]
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
    }
}
