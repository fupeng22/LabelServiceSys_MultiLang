using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
   public class T_SysUsers
    {
       public bool addUser(M_SysUsers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_AdminUser]
                           ([auNum]
                           ,[auPSW]
                           ,[auUnitCodeIDs]
                           ,[auDelflag])
                     VALUES
                           (@auNum
                           ,@auPSW
                           ,@auUnitCodeIDs
                           ,@auDelflag)");

            SqlParameter[] parameters = {
                    new SqlParameter("@auNum",SqlDbType.NVarChar),
                    new SqlParameter("@auPSW",SqlDbType.NVarChar ),
                    new SqlParameter("@auUnitCodeIDs", SqlDbType.NVarChar),
                    new SqlParameter("@auDelflag", SqlDbType.Int)
            };
            parameters[0].Value = model.auNum;
            parameters[1].Value = model.auPSW;
            parameters[2].Value = model.auUnitCodeIDs;
            parameters[3].Value = model.auDelflag;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

       public bool updateUser(M_SysUsers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_AdminUser]
                           SET [auNum] =@auNum
                              ,[auPSW] =@auPSW
                              ,[auUnitCodeIDs] =@auUnitCodeIDs
                              ,[auDelflag]=@auDelflag
                         WHERE auID=@auID");

            SqlParameter[] parameters = {
                    new SqlParameter("@auNum",SqlDbType.NVarChar),
                    new SqlParameter("@auPSW",SqlDbType.NVarChar ),
                    new SqlParameter("@auUnitCodeIDs", SqlDbType.NVarChar),
                    new SqlParameter("@auDelflag", SqlDbType.Int),
                    new SqlParameter("@auID", SqlDbType.Int)
            };
            parameters[0].Value = model.auNum;
            parameters[1].Value = model.auPSW;
            parameters[2].Value = model.auUnitCodeIDs;
            parameters[3].Value = model.auDelflag;
            parameters[4].Value = model.auID;

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
            strSql.Append(" FROM [Better_AdminUser]");
            strSql.Append(" WHERE (auNum = '" + userNum + "')");

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
            strSql.Append(" FROM [Better_AdminUser]");
            strSql.Append(" WHERE (auNum = '" + userNum + "' and auID<>" + userID + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteUsers(string cIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_AdminUser] where auID in (" + cIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Login(string userNum, string userPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Better_AdminUser]");
            strSql.Append(" WHERE (auNum = '" + userNum + "' and auPSW='" + userPwd + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetUseByUsername(string strUserName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM Better_AdminUser where auNum='" + strUserName + "'");

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
