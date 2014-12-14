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
using LabelServiceSys.Filter;

namespace LabelServiceSys.Controllers
{
     [ErrorAttribute]
    public class OperationLogController : Controller
    {
        T_OperationLog tOperationLog = new T_OperationLog();

        public const string strFileds = "op_urID,op_urStaffNum,op_urUnitCode,opJobNameIdLv1,opJobNameIdLv2,opType,opTypeDesc,opContent,opDateTime,urNum,urName,urStaffNum,urDept,urDuty,urUnitCode,opID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/OperationLog.rdlc";
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
        public string GetData(string order, string page, string rows, string sort, string dBegin, string dEnd, string opType, string urName)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "opID";

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
            opType = Server.UrlDecode(opType);
            urName = Server.UrlDecode(urName);

            if (!string.IsNullOrEmpty(urName.Trim()))
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (urName='{0}') ", urName.Trim());
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (urName='{0}') ", urName.Trim());
                }
            }
            else
            {
                if (dBegin != "" && dEnd != "")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (opDateTime>='{0}' and opDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("  (opDateTime>='{0}' and opDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

                if (opType!="" && opType != "-99")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (opType={0}) ", opType.Trim());
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (opType={0}) ", opType.Trim());
                    }
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
        public ActionResult Print(string order, string page, string rows, string sort, string dBegin, string dEnd, string opType, string urName)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "opID";

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
            opType = Server.UrlDecode(opType);
            urName = Server.UrlDecode(urName);

            if (!string.IsNullOrEmpty(urName.Trim()))
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and urName='{0}') ", urName.Trim());
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   urName='{0}') ", urName.Trim());
                }
            }
            else
            {
                if (dBegin != "" && dEnd != "")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (opDateTime>='{0}' and opDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("  (opDateTime>='{0}' and opDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

                if (opType != "" && opType != "-99")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and opType={0}) ", opType.Trim());
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   opType={0}) ", opType.Trim());
                    }
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
            //op_urID,op_urStaffNum,op_urUnitCode,opJobNameIdLv1,opJobNameIdLv2,opType,opContent,opDateTime,urNum,urName,urStaffNum,urDept,urDuty,urUnitCode,opID
            dtCustom.Columns.Add("op_urID", Type.GetType("System.String"));
            dtCustom.Columns.Add("op_urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("op_urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("opJobNameIdLv1", Type.GetType("System.String"));
            dtCustom.Columns.Add("opJobNameIdLv2", Type.GetType("System.String"));
            dtCustom.Columns.Add("opType", Type.GetType("System.String"));
            dtCustom.Columns.Add("opTypeDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("opContent", Type.GetType("System.String"));
            dtCustom.Columns.Add("opDateTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("opID", Type.GetType("System.String"));

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
                if (drCustom["opID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("OperationLog_DS", dtCustom);

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
        public ActionResult Excel(string order, string page, string rows, string sort, string dBegin, string dEnd, string opType, string urName, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Better_OperationLog";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "opID";

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
            opType = Server.UrlDecode(opType);
            urName = Server.UrlDecode(urName);

            if (!string.IsNullOrEmpty(urName.Trim()))
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and urName='{0}') ", urName.Trim());
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   urName='{0}') ", urName.Trim());
                }
            }
            else
            {
                if (dBegin != "" && dEnd != "")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (opDateTime>='{0}' and opDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("  (opDateTime>='{0}' and opDateTime<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

                if (opType != "" && opType != "-99")
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and opType={0}) ", opType.Trim());
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   opType={0}) ", opType.Trim());
                    }
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
            dtCustom.Columns.Add("op_urID", Type.GetType("System.String"));
            dtCustom.Columns.Add("op_urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("op_urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("opJobNameIdLv1", Type.GetType("System.String"));
            dtCustom.Columns.Add("opJobNameIdLv2", Type.GetType("System.String"));
            dtCustom.Columns.Add("opType", Type.GetType("System.String"));
            dtCustom.Columns.Add("opTypeDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("opContent", Type.GetType("System.String"));
            dtCustom.Columns.Add("opDateTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("opID", Type.GetType("System.String"));

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
                if (drCustom["opID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("OperationLog_DS", dtCustom);

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

            string strOutputFileName = "操作日志信息_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public string Delete(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除操作日志失败，原因未知\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tOperationLog.deleteOperationLog(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除操作日志成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除操作日志失败\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除操作日志失败，原因:" + ex.Message + "\"}";
            }
            return strRet;
        }
       
    }
}
