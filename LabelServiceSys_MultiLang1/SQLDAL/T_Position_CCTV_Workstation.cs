using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public  class T_Position_CCTV_Workstation
    {
        public bool addPosition_CCTV_Workstation(M_Position_CCTV_Workstation model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Better_Position_CCTV_Workstation]
                           ([PositionNO]
                           ,[CCTV_workstation_id])
                     VALUES
                           (@PositionNO
                           ,@CCTV_workstation_id)");

            SqlParameter[] parameters = {
                    new SqlParameter("@PositionNO",SqlDbType.Int),
                    new SqlParameter("@CCTV_workstation_id",SqlDbType.NVarChar )
            };
            parameters[0].Value = model.PositionNO;
            parameters[1].Value = model.CCTV_workstation_id;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updatePosition_CCTV_Workstation(M_Position_CCTV_Workstation model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Better_Position_CCTV_Workstation]
                           SET [PositionNO] =@PositionNO
                              ,[CCTV_workstation_id] =@CCTV_workstation_id
                         WHERE PId=@PId");

            SqlParameter[] parameters = {
                    new SqlParameter("@PositionNO",SqlDbType.Int),
                    new SqlParameter("@CCTV_workstation_id",SqlDbType.NVarChar ),
                    new SqlParameter("@PId", SqlDbType.Int)
            };
            parameters[0].Value = model.PositionNO;
            parameters[1].Value = model.CCTV_workstation_id;
            parameters[2].Value = model.PId;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool PositionNOExists(string positionNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Better_Position_CCTV_Workstation]");
            strSql.Append(" WHERE (PositionNO = " + positionNO + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PositionNOExists(int pID, string positionNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Better_Position_CCTV_Workstation]");
            strSql.Append(" WHERE (PositionNO = " + positionNO + " and PId<>" + pID + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deletePosition_CCTV_Workstation(string pIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Better_Position_CCTV_Workstation] where PId in (" + pIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet getAllPositionCCTVWorkstation()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM Better_Position_CCTV_Workstation order by PositionNO");

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
