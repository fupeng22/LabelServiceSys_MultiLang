using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabelServiceSys.Filter;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using DBUtility;
using SQLDAL;
using Model;
using Util;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class JobNameManagementController : Controller
    {
        public const string STR_TOP_ID = "top";
        public const string strFileds = "WorkProcess_Code,WorkProcess_Text,gateway,CCTV_workstation_id,bit_IsUse,bit_IsUse_Des,hasChild,parentID,state,ID";

        [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData(string order, string page, string rows, string sort, string id)
        {
            string strId = id == null ? STR_TOP_ID : id;
            SqlParameter[] param = new SqlParameter[8];
            DataSet ds = null;
            DataTable dt = null;
            StringBuilder sb = null;
            switch (strId)
            {
                case STR_TOP_ID:
                    param[0] = new SqlParameter();
                    param[0].SqlDbType = SqlDbType.VarChar;
                    param[0].ParameterName = "@TableName";
                    param[0].Direction = ParameterDirection.Input;
                    param[0].Value = "V_Better_tb_Setting_WorkProcess_L1";

                    param[1] = new SqlParameter();
                    param[1].SqlDbType = SqlDbType.VarChar;
                    param[1].ParameterName = "@FieldKey";
                    param[1].Direction = ParameterDirection.Input;
                    param[1].Value = "int_id";

                    param[2] = new SqlParameter();
                    param[2].SqlDbType = SqlDbType.VarChar;
                    param[2].ParameterName = "@FieldShow";
                    param[2].Direction = ParameterDirection.Input;
                    param[2].Value = "*";

                    param[3] = new SqlParameter();
                    param[3].SqlDbType = SqlDbType.VarChar;
                    param[3].ParameterName = "@FieldOrder";
                    param[3].Direction = ParameterDirection.Input;
                    param[3].Value = sort + " " + order;

                    param[4] = new SqlParameter();
                    param[4].SqlDbType = SqlDbType.Int;
                    param[4].ParameterName = "@PageSize";
                    param[4].Direction = ParameterDirection.Input;
                    param[4].Value = 200;//Convert.ToInt32(rows);

                    param[5] = new SqlParameter();
                    param[5].SqlDbType = SqlDbType.Int;
                    param[5].ParameterName = "@PageCurrent";
                    param[5].Direction = ParameterDirection.Input;
                    param[5].Value = Convert.ToInt32(page);

                    param[6] = new SqlParameter();
                    param[6].SqlDbType = SqlDbType.VarChar;
                    param[6].ParameterName = "@Where";
                    param[6].Direction = ParameterDirection.Input;
                    param[6].Value = "";

                    string strWhereTemp = "";

                    //当前登录用户为系统超级管理员
                    if (PubVariables.SysUserNames.Contains(Session["Global_UserName"].ToString().ToLower()))
                    {

                    }
                    else//当前登录用户为非超级管理员
                    {
                        if (strWhereTemp != "")
                        {
                            strWhereTemp = strWhereTemp + string.Format(" and (ucID in ({0}) ) ", Session["Global_UnitCode"].ToString());
                        }
                        else
                        {
                            strWhereTemp = strWhereTemp + string.Format("  (ucID in ({0}) ) ", Session["Global_UnitCode"].ToString());
                        }
                    }

                    param[6].Value = strWhereTemp;

                    param[7] = new SqlParameter();
                    param[7].SqlDbType = SqlDbType.Int;
                    param[7].ParameterName = "@RecordCount";
                    param[7].Direction = ParameterDirection.Output;

                    ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
                    dt = ds.Tables["result"];

                    sb = new StringBuilder("");
                    sb.Append("{");
                    sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
                    sb.Append(",\"rows\":[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        //"WorkProcess_Code,WorkProcess_Text,gateway,parentID,state,ID"
                        string[] strFiledArray = strFileds.Split(',');
                        for (int j = 0; j < strFiledArray.Length; j++)
                        {
                            switch (strFiledArray[j])
                            {
                                case "WorkProcess_Code":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L1_Code"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L1_Code"].ToString());
                                    }
                                    break;
                                case "WorkProcess_Text":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L1_Text"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L1_Text"].ToString());
                                    }
                                    break;
                                case "gateway":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["vchar_gateway"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["vchar_gateway"].ToString());
                                    }
                                    break;
                                case "CCTV_workstation_id":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                                    }
                                    break;
                                case "hasChild":
                                    if (new T_tb_Setting_WorkProcess_L2().HasL2(dt.Rows[i]["int_id"].ToString()))
                                    {
                                        if (j != strFiledArray.Length - 1)
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "1");
                                        }
                                        else
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "1");
                                        }
                                    }
                                    else
                                    {
                                        if (j != strFiledArray.Length - 1)
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "0");
                                        }
                                        else
                                        {
                                            sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "0");
                                        }
                                    }

                                    break;
                                case "parentID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "parent_" + dt.Rows[i]["int_id"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "parent_" + dt.Rows[i]["int_id"].ToString());
                                    }
                                    break;
                                case "ID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "top_" + dt.Rows[i]["int_id"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "top_" + dt.Rows[i]["int_id"].ToString());
                                    }
                                    break;
                                case "state":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "closed");
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "closed");
                                    }
                                    break;
                                default:
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                                    }
                                    break;
                            }
                        }

                        if (i == dt.Rows.Count - 1)
                        {
                            sb.Append("}");
                        }
                        else
                        {
                            sb.Append("},");
                        }
                    }
                    dt = null;
                    if (sb.ToString().EndsWith(","))
                    {
                        sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
                    }
                    sb.Append("]");
                    sb.Append("}");
                    break;
                default:
                    param[0] = new SqlParameter();
                    param[0].SqlDbType = SqlDbType.VarChar;
                    param[0].ParameterName = "@TableName";
                    param[0].Direction = ParameterDirection.Input;
                    param[0].Value = "V_Better_tb_Setting_WorkProcess_L2";

                    param[1] = new SqlParameter();
                    param[1].SqlDbType = SqlDbType.VarChar;
                    param[1].ParameterName = "@FieldKey";
                    param[1].Direction = ParameterDirection.Input;
                    param[1].Value = "int_id";

                    param[2] = new SqlParameter();
                    param[2].SqlDbType = SqlDbType.VarChar;
                    param[2].ParameterName = "@FieldShow";
                    param[2].Direction = ParameterDirection.Input;
                    param[2].Value = "*";

                    param[3] = new SqlParameter();
                    param[3].SqlDbType = SqlDbType.VarChar;
                    param[3].ParameterName = "@FieldOrder";
                    param[3].Direction = ParameterDirection.Input;
                    param[3].Value = sort + " " + order;

                    param[4] = new SqlParameter();
                    param[4].SqlDbType = SqlDbType.Int;
                    param[4].ParameterName = "@PageSize";
                    param[4].Direction = ParameterDirection.Input;
                    param[4].Value = 100;//Convert.ToInt32(rows);

                    param[5] = new SqlParameter();
                    param[5].SqlDbType = SqlDbType.Int;
                    param[5].ParameterName = "@PageCurrent";
                    param[5].Direction = ParameterDirection.Input;
                    param[5].Value = Convert.ToInt32(page);

                    param[6] = new SqlParameter();
                    param[6].SqlDbType = SqlDbType.VarChar;
                    param[6].ParameterName = "@Where";
                    param[6].Direction = ParameterDirection.Input;
                    param[6].Value = " int_L1_id='" + strId.Replace("top_", "") + "' ";

                    param[7] = new SqlParameter();
                    param[7].SqlDbType = SqlDbType.Int;
                    param[7].ParameterName = "@RecordCount";
                    param[7].Direction = ParameterDirection.Output;

                    ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
                    dt = ds.Tables["result"];

                    sb = new StringBuilder("");
                    sb.Append("[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");

                        string[] strFiledArray = strFileds.Split(',');
                        for (int j = 0; j < strFiledArray.Length; j++)
                        {
                            switch (strFiledArray[j])
                            {
                                case "WorkProcess_Code":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L2_Code"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L2_Code"].ToString());
                                    }
                                    break;
                                case "WorkProcess_Text":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L2_Text"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["vchar_WorkProcess_L2_Text"].ToString());
                                    }
                                    break;
                                case "gateway":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "");
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "");
                                    }
                                    break;
                                case "CCTV_workstation_id":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["vchar_CCTV_workstation_id"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["vchar_CCTV_workstation_id"].ToString());
                                    }
                                    break;
                                case "hasChild":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "0");
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "0");
                                    }
                                    break;
                                case "parentID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], strId);
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], strId);
                                    }
                                    break;
                                case "ID":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "child_" + dt.Rows[i]["int_id"].ToString());
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "child_" + dt.Rows[i]["int_id"].ToString());
                                    }
                                    break;
                                case "state":
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], "open");
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], "open");
                                    }
                                    break;
                                default:
                                    if (j != strFiledArray.Length - 1)
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                                    }
                                    else
                                    {
                                        sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                                    }
                                    break;
                            }
                        }

                        if (i == dt.Rows.Count - 1)
                        {
                            sb.Append("}");
                        }
                        else
                        {
                            sb.Append("},");
                        }
                    }
                    dt = null;
                    if (sb.ToString().EndsWith(","))
                    {
                        sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
                    }
                    sb.Append("]");
                    break;
            }

            return sb.ToString();
        }

        [HttpPost]
        public string DeleteJobName1(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";
            ids = Server.UrlDecode(ids);

            try
            {

                if (ids != "")
                {
                    if ((new T_tb_Setting_WorkProcess_L1()).deleteJobName1(ids))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"" + "成功删除" + "\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"" + "删除过程中出现问题" + "\"}";
                    }
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "未指定需要删除的信息" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string EnableOrDisableJobName1(string ids,string iEnable)
        {
            string strType = "";
            string strRet = "";
            switch (iEnable)
            {
                case "0"://禁用
                    strType = "禁用";
                    break;
                case "1"://启用
                    strType = "启用";
                    break;
                default:
                    break;
            }
            strRet = "{\"result\":\"error\",\"message\":\"" + strType + "失败，原因未知\"}";
            ids = Server.UrlDecode(ids);

            try
            {

                if (ids != "")
                {
                    switch (iEnable)
                    {
                        case "0"://禁用
                            if ((new T_tb_Setting_WorkProcess_L2()).EnableOrDisableJobName2ByJobId1(iEnable, ids))
                            {
                                
                            }
                            if ((new T_tb_Setting_WorkProcess_L1()).EnableOrDisableJobName1(iEnable, ids))
                            {
                                strRet = "{\"result\":\"ok\",\"message\":\"" + "成功" + strType + "\"}";
                            }
                            else
                            {
                                strRet = "{\"result\":\"error\",\"message\":\"" + strType + "过程中出现问题" + "\"}";
                            }
                            break;
                        case "1"://启用
                            if ((new T_tb_Setting_WorkProcess_L1()).EnableOrDisableJobName1(iEnable, ids))
                            {
                                strRet = "{\"result\":\"ok\",\"message\":\"" + "成功" + strType + "\"}";
                            }
                            else
                            {
                                strRet = "{\"result\":\"error\",\"message\":\"" + strType + "过程中出现问题" + "\"}";
                            }
                            break;
                        default:
                            break;
                    }
                    
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "未选择需要" + strType + "的信息" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + strType + "失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string DeleteJobName2(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因未知\"}";
            ids = Server.UrlDecode(ids);

            try
            {

                if (ids != "")
                {
                    if ((new T_tb_Setting_WorkProcess_L2()).deleteJobName2(ids))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"" + "成功删除" + "\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"" + "删除过程中出现问题" + "\"}";
                    }
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "未指定需要删除的信息" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string EnableOrDisableJobName2(string ids, string iEnable)
        {
            string strType = "";
            string strRet = "";
            switch (iEnable)
            {
                case "0"://禁用
                    strType = "禁用";
                    break;
                case "1"://启用
                    strType = "启用";
                    break;
                default:
                    break;
            }
            strRet = "{\"result\":\"error\",\"message\":\"" + strType + "失败，原因未知\"}";
            ids = Server.UrlDecode(ids);

            try
            {

                if (ids != "")
                {
                    if ((new T_tb_Setting_WorkProcess_L2()).EnableOrDisableJobName2(iEnable, ids))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"" + "成功" + strType + "\"}";
                    }
                    else
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"" + strType + "过程中出现问题" + "\"}";
                    }
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "未选择需要" + strType + "的信息" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + strType + "失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string AddJobName1(string GateWay, string WorkProcess_L1_Code, string WorkProcess_L1_Text)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因未知\"}";
            M_tb_Setting_WorkProcess_L1 m_tb_Setting_WorkProcess_L1 = new M_tb_Setting_WorkProcess_L1();
            GateWay = Server.UrlDecode(GateWay);
            WorkProcess_L1_Code = Server.UrlDecode(WorkProcess_L1_Code);
            WorkProcess_L1_Text = Server.UrlDecode(WorkProcess_L1_Text);
            m_tb_Setting_WorkProcess_L1.vchar_gateway = GateWay;
            m_tb_Setting_WorkProcess_L1.vchar_WorkProcess_L1_Code = WorkProcess_L1_Code;
            m_tb_Setting_WorkProcess_L1.vchar_WorkProcess_L1_Text = WorkProcess_L1_Text;
            m_tb_Setting_WorkProcess_L1.vchar_created_user = Session["Global_UserName"].ToString();
            m_tb_Setting_WorkProcess_L1.bit_IsUse = 1;
            try
            {
                if ((new T_tb_Setting_WorkProcess_L1()).addJobName1(m_tb_Setting_WorkProcess_L1))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "添加成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "添加失败" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateJobName1(string id, string GateWay, string WorkProcess_L1_Code, string WorkProcess_L1_Text)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因未知\"}";
            M_tb_Setting_WorkProcess_L1 m_tb_Setting_WorkProcess_L1 = new M_tb_Setting_WorkProcess_L1();
            GateWay = Server.UrlDecode(GateWay);
            WorkProcess_L1_Code = Server.UrlDecode(WorkProcess_L1_Code);
            WorkProcess_L1_Text = Server.UrlDecode(WorkProcess_L1_Text);
            id = Server.UrlDecode(id);

            m_tb_Setting_WorkProcess_L1.vchar_gateway = GateWay;
            m_tb_Setting_WorkProcess_L1.vchar_WorkProcess_L1_Code = WorkProcess_L1_Code;
            m_tb_Setting_WorkProcess_L1.vchar_WorkProcess_L1_Text = WorkProcess_L1_Text;
            m_tb_Setting_WorkProcess_L1.vchar_created_user = Session["Global_UserName"].ToString();
            m_tb_Setting_WorkProcess_L1.bit_IsUse = 1;
            m_tb_Setting_WorkProcess_L1.int_id = Convert.ToInt32(id);
            try
            {
                if ((new T_tb_Setting_WorkProcess_L1()).updateJobName1(m_tb_Setting_WorkProcess_L1))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "修改成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "修改失败" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string AddJobName2(string JobName1ID, string WorkProcess_L2_Code, string WorkProcess_L2_Text, string CCTV_workstation_id)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因未知\"}";
            M_tb_Setting_WorkProcess_L2 m_tb_Setting_WorkProcess_L2 = new M_tb_Setting_WorkProcess_L2();
            JobName1ID = Server.UrlDecode(JobName1ID);
            WorkProcess_L2_Code = Server.UrlDecode(WorkProcess_L2_Code);
            WorkProcess_L2_Text = Server.UrlDecode(WorkProcess_L2_Text);
            CCTV_workstation_id = Server.UrlDecode(CCTV_workstation_id);
            m_tb_Setting_WorkProcess_L2.int_L1_id = Convert.ToInt32(JobName1ID);
            m_tb_Setting_WorkProcess_L2.vchar_WorkProcess_L2_Code = WorkProcess_L2_Code;
            m_tb_Setting_WorkProcess_L2.vchar_WorkProcess_L2_Text = WorkProcess_L2_Text;
            m_tb_Setting_WorkProcess_L2.vchar_created_user = Session["Global_UserName"].ToString();
            m_tb_Setting_WorkProcess_L2.vchar_CCTV_workstation_id = CCTV_workstation_id;
            m_tb_Setting_WorkProcess_L2.bit_IsUse = 1;
            try
            {
                if ((new T_tb_Setting_WorkProcess_L2()).addJobName2(m_tb_Setting_WorkProcess_L2))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "添加成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "添加失败" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateJobName2(string Id, string JobName1ID, string WorkProcess_L2_Code, string WorkProcess_L2_Text, string CCTV_workstation_id)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因未知\"}";
            M_tb_Setting_WorkProcess_L2 m_tb_Setting_WorkProcess_L2 = new M_tb_Setting_WorkProcess_L2();
            Id = Server.UrlDecode(Id);
            JobName1ID = Server.UrlDecode(JobName1ID);
            WorkProcess_L2_Code = Server.UrlDecode(WorkProcess_L2_Code);
            WorkProcess_L2_Text = Server.UrlDecode(WorkProcess_L2_Text);
            CCTV_workstation_id = Server.UrlDecode(CCTV_workstation_id);
            m_tb_Setting_WorkProcess_L2.int_L1_id = Convert.ToInt32(JobName1ID);
            m_tb_Setting_WorkProcess_L2.vchar_WorkProcess_L2_Code = WorkProcess_L2_Code;
            m_tb_Setting_WorkProcess_L2.vchar_WorkProcess_L2_Text = WorkProcess_L2_Text;
            m_tb_Setting_WorkProcess_L2.vchar_created_user = Session["Global_UserName"].ToString();
            m_tb_Setting_WorkProcess_L2.vchar_CCTV_workstation_id = CCTV_workstation_id;
            m_tb_Setting_WorkProcess_L2.bit_IsUse = 1;
            m_tb_Setting_WorkProcess_L2.int_id = Convert.ToInt32(Id);
            try
            {
                if ((new T_tb_Setting_WorkProcess_L2()).updateJobName2(m_tb_Setting_WorkProcess_L2))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "修改成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "修改失败" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string TestExist_JobName1Code_Add(string JobName1Code)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            JobName1Code = Server.UrlDecode(JobName1Code);
            try
            {
                if (new T_tb_Setting_WorkProcess_L1().ExistJobName1Code(JobName1Code))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此工作流程代码已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string TestExist_JobName1Code_Update(string JobName1Code, string int_Id)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            JobName1Code = Server.UrlDecode(JobName1Code);
            int_Id = Server.UrlDecode(int_Id);
            try
            {
                if (new T_tb_Setting_WorkProcess_L1().ExistJobName1Code(JobName1Code, int_Id))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此工作流程代码已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string TestExist_JobName2Code_Add(string JobName2Code)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            JobName2Code = Server.UrlDecode(JobName2Code);
            try
            {
                if (new T_tb_Setting_WorkProcess_L2().ExistJobName2Code(JobName2Code))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此工作流程代码已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string TestExist_JobName2Code_Update(string JobName2Code, string int_Id)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            JobName2Code = Server.UrlDecode(JobName2Code);
            int_Id = Server.UrlDecode(int_Id);
            try
            {
                if (new T_tb_Setting_WorkProcess_L2().ExistJobName2Code(JobName2Code, int_Id))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此工作流程代码已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }
    }
}
