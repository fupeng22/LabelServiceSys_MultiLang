using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using LabelServiceSys.Models;
using LabelServiceSys.Filter;
using Util;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class OperationLog_HawbController : Controller
    {
        T_OperationLog_Hawb tOperationLog_Hawb = new T_OperationLog_Hawb();

        public const string strFileds = "hawbop_urID,hawbopJobNameIdLv1,hawbopJobNameIdLv2,hawbopContent,hawbopDateTime,hawbopIsDvir,CCTV_workstation_id,urNum,urName,urStaffNum,urDept,urDuty,urUnitCode,vchar_WorkProcess_L1_Text,vchar_WorkProcess_L2_Text,hawbopID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/OperationLog_Hawb.rdlc";

        List<string> lstTitle = new List<string> { "用户号", "用户姓名", "一级工作流程", "二级工作流程", "口岸代码", "操作时间", "工作岗位", "操作内容" };

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
        public string GetData(string order, string page, string rows, string sort, string dBegin, string dEnd, string urName)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog_Hawb";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "hawbopID";

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
            param[4].Value = Convert.ToInt32(rows);

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
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            urName = Server.UrlDecode(urName);

            if (strWhereTemp != "")
            {
                strWhereTemp = strWhereTemp + string.Format(" and (urName like '%{0}%') ", urName.Trim());
            }
            else
            {
                strWhereTemp = strWhereTemp + string.Format("   (urName like '%{0}%') ", urName.Trim());
            }

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

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

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            sb.Append("{");
            sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
            sb.Append(",\"rows\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("{");

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
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
            return sb.ToString();
        }


        [HttpGet]
        public ActionResult Print(string order, string page, string rows, string sort, string dBegin, string dEnd, string urName)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog_Hawb";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "hawbopID";

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
            param[4].Value = Convert.ToInt32(rows);

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
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            urName = Server.UrlDecode(urName);

            if (strWhereTemp != "")
            {
                strWhereTemp = strWhereTemp + string.Format(" and (urName like '%{0}%') ", urName.Trim());
            }
            else
            {
                strWhereTemp = strWhereTemp + string.Format("   (urName like '%{0}%') ", urName.Trim());
            }

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

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

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //hawbop_urID,hawbopJobNameIdLv1,hawbopJobNameIdLv2,hawbopContent,hawbopDateTime,hawbopIsDvir,urNum,urName,urStaffNum,urDept,urDuty,urUnitCode,hawbopID
            dtCustom.Columns.Add("hawbop_urID", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopJobNameIdLv1", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopJobNameIdLv2", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopContent", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopDateTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopIsDvir", Type.GetType("System.String"));
            dtCustom.Columns.Add("CCTV_workstation_id", Type.GetType("System.String"));
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("vchar_WorkProcess_L1_Text", Type.GetType("System.String"));
            dtCustom.Columns.Add("vchar_WorkProcess_L2_Text", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopID", Type.GetType("System.String"));

            DataRow drCustom = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["hawbopID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("OperationLog_Hawb_DS", dtCustom);

            localReport.DataSources.Add(reportDataSource);
            string reportType = "PDF";
            string mimeType;
            string encoding = "UTF-8";
            string fileNameExtension;

            string deviceInfo = "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>12in</PageWidth>" +
                " <PageHeigth>11in</PageHeigth>" +
                " <MarginTop>0.5in</MarginTop>" +
                " <MarginLeft>1in</MarginLeft>" +
                " <MarginRight>1in</MarginRight>" +
                " <MarginBottom>0.5in</MarginBottom>" +
                " </DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return File(renderedBytes, mimeType);
        }


        [HttpGet]
        public ActionResult Excel1(string order, string page, string rows, string sort, string dBegin, string dEnd, string urName, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog_Hawb";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "hawbopID";

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
            param[4].Value = Convert.ToInt32(rows);

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
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            urName = Server.UrlDecode(urName);

            if (strWhereTemp != "")
            {
                strWhereTemp = strWhereTemp + string.Format(" and (urName like '%{0}%') ", urName.Trim());
            }
            else
            {
                strWhereTemp = strWhereTemp + string.Format("   (urName like '%{0}%') ", urName.Trim());
            }

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

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

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("hawbop_urID", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopJobNameIdLv1", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopJobNameIdLv2", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopContent", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopDateTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopIsDvir", Type.GetType("System.String"));
            dtCustom.Columns.Add("CCTV_workstation_id", Type.GetType("System.String"));
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("vchar_WorkProcess_L1_Text", Type.GetType("System.String"));
            dtCustom.Columns.Add("vchar_WorkProcess_L2_Text", Type.GetType("System.String"));
            dtCustom.Columns.Add("hawbopID", Type.GetType("System.String"));

            DataRow drCustom = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["hawbopID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("OperationLog_Hawb_DS", dtCustom);

            localReport.DataSources.Add(reportDataSource);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = localReport.Render(
               "Excel", null, out mimeType, out encoding, out extension,
               out streamids, out warnings);
            string strFileName = Server.MapPath(STR_TEMPLATE_EXCEL);
            FileStream fs = new FileStream(strFileName, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            string strOutputFileName = LangHelper.GetLangbyKey("OperationLog_Hawb_Controller_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            switch (browserType.ToLower())
            {
                case "safari":
                    break;
                case "mozilla":
                    break;
                default:
                    strOutputFileName = HttpUtility.UrlEncode(strOutputFileName);
                    break;
            }

            return File(strFileName, "application/vnd.ms-excel", strOutputFileName);
        }

        [HttpGet]
        public ActionResult Excel(string order, string page, string rows, string sort, string dBegin, string dEnd, string urName, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog_Hawb";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "hawbopID";

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
            param[4].Value = Convert.ToInt32(rows);

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
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            urName = Server.UrlDecode(urName);

            if (strWhereTemp != "")
            {
                strWhereTemp = strWhereTemp + string.Format(" and (urName like '%{0}%') ", urName.Trim());
            }
            else
            {
                strWhereTemp = strWhereTemp + string.Format("   (urName like '%{0}%') ", urName.Trim());
            }

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (hawbopDateTime>='{0}' and hawbopDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

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

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");

            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");
            //urID,JobName1,JobName2,JobContent,OpTime,IsDvir,CCTV_workstation_id,urNum,urName,urStaffNum,urDept,urDuty,urUnitCode,vchar_WorkProcess_L1_Text,vchar_WorkProcess_L2_Text,cId
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urNum"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urName"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["vchar_WorkProcess_L1_Text"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["vchar_WorkProcess_L2_Text"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urUnitCode"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["hawbopDateTime"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["CCTV_workstation_id"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["hawbopContent"].ToString());
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            string strOutputFileName = LangHelper.GetLangbyKey("OperationLog_Hawb_Controller_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            switch (browserType.ToLower())
            {
                case "safari":
                    break;
                case "mozilla":
                    break;
                default:
                    strOutputFileName = HttpUtility.UrlEncode(strOutputFileName);
                    break;
            }

            return File(fileContents, "application/vnd.ms-excel", strOutputFileName);
            //return File(fileContents, "application/ms-excel", LangHelper.GetLangbyKey("OperationLog_Hawb_Controller_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public string Delete(string ids)
        {
            //string strRet = "{\"result\":\"error\",\"message\":\"删除日志失败，原因未知\"}";
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Public_Controller_ErrorMessage1") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tOperationLog_Hawb.deleteOperationLog_Hawb(ids))
                {
                    //strRet = "{\"result\":\"ok\",\"message\":\"删除日志成功\"}";
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Public_Controller_ErrorMessage2") + "\"}";
                }
                else
                {
                    //  strRet = "{\"result\":\"error\",\"message\":\"删除日志失败\"}";
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Public_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                // strRet = "{\"result\":\"error\",\"message\":\"删除日志失败，原因:" + ex.Message + "\"}";
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Public_Controller_ErrorMessage1") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }
    }
}
