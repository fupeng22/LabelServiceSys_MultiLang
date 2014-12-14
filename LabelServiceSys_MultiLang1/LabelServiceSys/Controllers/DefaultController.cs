using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using SQLDAL;
using LabelServiceSys.Models;
using LabelServiceSys.Filter;
using Util;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class DefaultController : Controller
    {
        //
        // GET: /Default/
        [LoginValidate]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public string LoadCommonVariables(string type)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_CommonVariables().GetCommonVariable(type);
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["vName"].ToString(), dt.Rows[i]["vValue"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadUnitCode()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = new DataTable();
            //当前登录用户为系统超级管理员
            if (PubVariables.SysUserNames.Contains(Session["Global_UserName"].ToString().ToLower()))
            {
                ds = new T_UnitCode().GetUnitCode();
            }
            else//当前登录用户为非超级管理员
            {
                ds = new T_UnitCode().GetUnitCodeByIds(Session["Global_UnitCode"].ToString());
            }

            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["ucName"].ToString(), dt.Rows[i]["ucName"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadUnitCodeWithId()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_UnitCode().GetUnitCode();
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["ucID"].ToString(), dt.Rows[i]["ucName"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadSexJSON()
        {
            // return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"男\"},{\"id\":\"1\",\"text\":\"女\"}]";
            return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultItem") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_Sex_Man") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_Sex_Woman") + "\"}]";
        }

        [HttpPost]
        public string LoadOperationLogTypeJSON()
        {
            //return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"作业扫描日志\"},{\"id\":\"1\",\"text\":\"登录日志\"},{\"id\":\"2\",\"text\":\"登出日志\"}]";
            return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultItem") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultItem") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultItem") + "\"},{\"id\":\"2\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultItem") + "\"}]";
        }

        [HttpPost]
        public string LoadJobNameIdLv1ByUsernumber(string userNumber)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_User().GetJobNameL1ByUserNumber(userNumber);
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---Select---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["vchar_WorkProcess_L1_Code"].ToString(), dt.Rows[i]["vchar_WorkProcess_L1_Text"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadJobNameIdLv1()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_tb_Setting_WorkProcess_L1().GetJobNameL1();
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "请选择");
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["int_id"].ToString(), dt.Rows[i]["vchar_WorkProcess_L1_Text"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }


        [HttpPost]
        public string LoadJobNameIdLv2ByJobNameL1Id(string JobNameL1Id)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_User().GetJobNameL2ByJobNameL1Id(JobNameL1Id);
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---Select---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["vchar_WorkProcess_L2_Code"].ToString(), dt.Rows[i]["vchar_WorkProcess_L2_Text"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadJobNameIdLv2ByJobNameL1Code(string JobNameL1Code)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_User().GetJobNameL2ByJobNameL1Code(JobNameL1Code);
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---Select---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["vchar_WorkProcess_L2_Code"].ToString(), dt.Rows[i]["vchar_WorkProcess_L2_Text"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }
    }
}
