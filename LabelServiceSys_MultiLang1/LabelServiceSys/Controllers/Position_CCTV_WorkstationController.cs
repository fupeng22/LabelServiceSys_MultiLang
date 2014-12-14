using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabelServiceSys.Filter;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using Model;
using LabelServiceSys.Models;
using Util;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class Position_CCTV_WorkstationController : Controller
    {

        T_Position_CCTV_Workstation tPosition_CCTV_Workstation = new T_Position_CCTV_Workstation();

        public const string strFileds = "PositionNO,PositionNO_Des,CCTV_workstation_id,PId";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/PositionCCTVWorkstation.rdlc";

        List<string> lstTitle = new List<string> { "用户名", "口岸" };

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
        public string GetData(string order, string page, string rows, string sort)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Position_CCTV_Workstation";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PId";

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
        public ActionResult Print(string order, string page, string rows, string sort)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Position_CCTV_Workstation";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PId";

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
            //PositionNO,PositionNO_Des,CCTV_workstation_id,PId
            dtCustom.Columns.Add("PositionNO", Type.GetType("System.String"));
            dtCustom.Columns.Add("PositionNO_Des", Type.GetType("System.String"));
            dtCustom.Columns.Add("CCTV_workstation_id", Type.GetType("System.String"));
            dtCustom.Columns.Add("PId", Type.GetType("System.String"));

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
                if (drCustom["PId"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("PositionCCTVWorkstation_DS", dtCustom);

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
        public ActionResult Excel1(string order, string page, string rows, string sort, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Position_CCTV_Workstation";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PId";

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
            //PositionNO,PositionNO_Des,CCTV_workstation_id,PId
            dtCustom.Columns.Add("PositionNO", Type.GetType("System.String"));
            dtCustom.Columns.Add("PositionNO_Des", Type.GetType("System.String"));
            dtCustom.Columns.Add("CCTV_workstation_id", Type.GetType("System.String"));
            dtCustom.Columns.Add("PId", Type.GetType("System.String"));

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
                if (drCustom["PId"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("PositionCCTVWorkstation_DS", dtCustom);

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

            string strOutputFileName = "工位标签绑定" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
        public ActionResult Excel(string order, string page, string rows, string sort, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Position_CCTV_Workstation";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PId";

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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["PositionNO_Des"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["CCTV_workstation_id"].ToString());
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            string strOutputFileName = "工位标签绑定" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //return File(fileContents, "application/ms-excel", "工位标签绑定信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        [HttpPost]
        public string AddPosition_CCTV_Workstation(string PositionNO, string CCTV_workstation_id)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因未知\"}";
            M_Position_CCTV_Workstation m_Position_CCTV_Workstation = new M_Position_CCTV_Workstation();
            PositionNO = Server.UrlDecode(PositionNO);
            CCTV_workstation_id = Server.UrlDecode(CCTV_workstation_id);
            m_Position_CCTV_Workstation.PositionNO = Convert.ToInt32(PositionNO);
            m_Position_CCTV_Workstation.CCTV_workstation_id = CCTV_workstation_id;

            try
            {
                if ((new T_Position_CCTV_Workstation()).addPosition_CCTV_Workstation(m_Position_CCTV_Workstation))
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
        public string UpdatePosition_CCTV_Workstation(string pID, string PositionNO, string CCTV_workstation_id)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因未知\"}";
            M_Position_CCTV_Workstation m_Position_CCTV_Workstation = new M_Position_CCTV_Workstation();
            pID = Server.UrlDecode(pID);
            PositionNO = Server.UrlDecode(PositionNO);
            CCTV_workstation_id = Server.UrlDecode(CCTV_workstation_id);
            m_Position_CCTV_Workstation.PositionNO = Convert.ToInt32(PositionNO);
            m_Position_CCTV_Workstation.CCTV_workstation_id = CCTV_workstation_id;
            m_Position_CCTV_Workstation.PId = Convert.ToInt32(pID);
            try
            {
                if ((new T_Position_CCTV_Workstation()).updatePosition_CCTV_Workstation(m_Position_CCTV_Workstation))
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
        public string Delete(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"删除信息失败，原因未知\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if ((new T_Position_CCTV_Workstation()).deletePosition_CCTV_Workstation(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除信息成功\"}";
                    //      strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除信息失败\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除信息失败，原因:" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpPost]
        public string ExistPositionNO(string strPositionNO)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            strPositionNO = Server.UrlDecode(strPositionNO);
            try
            {

                if ((new T_Position_CCTV_Workstation()).PositionNOExists(strPositionNO))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此工位标签已经使用" + "\"}";
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
        public string ExistPositionNO_Update(string pID, string strPositionNO)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            pID = Server.UrlDecode(pID);
            strPositionNO = Server.UrlDecode(strPositionNO);
            try
            {

                if ((new T_Position_CCTV_Workstation()).PositionNOExists(Convert.ToInt32(pID), strPositionNO))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此工位标签已经使用" + "\"}";
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
        public string GetPosition_Add()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = null;
            List<string> listPosition = new List<string>();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---Select---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            for (int i = 1; i <= 999; i++)
            {
                listPosition.Add("CP" + i.ToString("000"));

            }
            ds = new T_Position_CCTV_Workstation().getAllPositionCCTVWorkstation();
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listPosition.Remove("CP" + Convert.ToInt32(dt.Rows[i]["PositionNO"].ToString()).ToString("000"));
                    }
                }
            }
            for (int i = 0; i < listPosition.Count; i++)
            {
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", Convert.ToInt32(listPosition[i].Substring(2, 3)), listPosition[i]);
                if (i != listPosition.Count - 1)
                {
                    sb.Append("},");
                }
                else
                {
                    sb.Append("}");
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
        public string GetPosition_Update(string positionNO)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = null;
            List<string> listPosition = new List<string>();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---Select---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultItem"));
            sb.Append("},");
            for (int i = 1; i <= 999; i++)
            {
                listPosition.Add("CP" + i.ToString("000"));

            }
            ds = new T_Position_CCTV_Workstation().getAllPositionCCTVWorkstation();
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (positionNO!=dt.Rows[i]["PositionNO"].ToString())
                        {
                            listPosition.Remove("CP" + Convert.ToInt32(dt.Rows[i]["PositionNO"].ToString()).ToString("000"));    
                        }
                    }
                }
            }
            for (int i = 0; i < listPosition.Count; i++)
            {
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", Convert.ToInt32(listPosition[i].Substring(2, 3)), listPosition[i]);
                if (i != listPosition.Count - 1)
                {
                    sb.Append("},");
                }
                else
                {
                    sb.Append("}");
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
        public string GetCCTVWorkstationId_Add()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = new DataTable();
            List<string> listPosition = new List<string>();
            //当前登录用户为系统超级管理员
            if (PubVariables.SysUserNames.Contains(Session["Global_UserName"].ToString().ToLower()))
            {
                ds = new T_tb_Setting_WorkProcess_L2().GetCCTVWorkstationId();
            }
            else//当前登录用户为非超级管理员
            {
                ds = new T_tb_Setting_WorkProcess_L2().GetCCTVWorkstationIdByGateWays(Session["Global_UnitCode"].ToString());
            }

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
                        listPosition.Add(dt.Rows[i]["vchar_CCTV_workstation_id"].ToString());
                    }
                }
            }
            ds = new T_Position_CCTV_Workstation().getAllPositionCCTVWorkstation();
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listPosition.Remove(dt.Rows[i]["CCTV_workstation_id"].ToString());
                    }
                }
            }
            for (int i = 0; i < listPosition.Count; i++)
            {
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", listPosition[i], listPosition[i]);
                if (i != listPosition.Count - 1)
                {
                    sb.Append("},");
                }
                else
                {
                    sb.Append("}");
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
        public string GetCCTVWorkstationId_Update(string CCTVWorkstationId)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;// new T_tb_Setting_WorkProcess_L2().GetCCTVWorkstationId();
            DataTable dt = new DataTable();
            List<string> listPosition = new List<string>();
            //当前登录用户为系统超级管理员
            if (PubVariables.SysUserNames.Contains(Session["Global_UserName"].ToString().ToLower()))
            {
                ds = new T_tb_Setting_WorkProcess_L2().GetCCTVWorkstationId();
            }
            else//当前登录用户为非超级管理员
            {
                ds = new T_tb_Setting_WorkProcess_L2().GetCCTVWorkstationIdByGateWays(Session["Global_UnitCode"].ToString());
            }
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
                        listPosition.Add(dt.Rows[i]["vchar_CCTV_workstation_id"].ToString());
                    }
                }
            }
            ds = new T_Position_CCTV_Workstation().getAllPositionCCTVWorkstation();
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CCTV_workstation_id"].ToString()!=CCTVWorkstationId)
                        {
                            listPosition.Remove(dt.Rows[i]["CCTV_workstation_id"].ToString());
                        }
                    }
                }
            }
            for (int i = 0; i < listPosition.Count; i++)
            {
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", listPosition[i], listPosition[i]);
                if (i != listPosition.Count - 1)
                {
                    sb.Append("},");
                }
                else
                {
                    sb.Append("}");
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
