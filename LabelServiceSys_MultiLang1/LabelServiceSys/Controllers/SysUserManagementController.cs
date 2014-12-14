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
using Model;
using Util;
using LabelServiceSys.Filter;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class SysUserManagementController : Controller
    {
        T_SysUsers tUser = new T_SysUsers();

        public const string strFileds = "auNum,auPSW,auUnitCodeIDs,auUnitCodeIDs_Des,auDelflag,auID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/SysUsers.rdlc";

        List<string> lstTitle = new List<string> { "用户名", "口岸" };

        public const string STR_BARCODE_PATH = "~/Temp/imgs/";
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
            param[0].Value = "Better_AdminUser";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "auID";

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
                        case "auUnitCodeIDs_Des":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], GetUnitCodeByIds(dt.Rows[i]["auUnitCodeIDs"] == DBNull.Value ? "" : (dt.Rows[i]["auUnitCodeIDs"].ToString().Replace("\r\n", ""))));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], GetUnitCodeByIds(dt.Rows[i]["auUnitCodeIDs"] == DBNull.Value ? "" : (dt.Rows[i]["auUnitCodeIDs"].ToString().Replace("\r\n", ""))));
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
            param[0].Value = "Better_AdminUser";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "auID";

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

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //"auNum,auPSW,auUnitCodeIDs,auUnitCodeIDs_Des,auDelflag,auID"
            dtCustom.Columns.Add("auNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("auPSW", Type.GetType("System.String"));
            dtCustom.Columns.Add("auUnitCodeIDs", Type.GetType("System.String"));
            dtCustom.Columns.Add("auUnitCodeIDs_Des", Type.GetType("System.String"));
            dtCustom.Columns.Add("auDelflag", Type.GetType("System.String"));
            dtCustom.Columns.Add("auID", Type.GetType("System.String"));

            DataRow drCustom = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "auUnitCodeIDs_Des":
                            drCustom[strFiledArray[j]] = GetUnitCodeByIds(dt.Rows[i]["auUnitCodeIDs"] == DBNull.Value ? "" : (dt.Rows[i]["auUnitCodeIDs"].ToString().Replace("\r\n", "")));
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["auID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("SysUsers_DS", dtCustom);

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
            param[0].Value = "Better_AdminUser";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "auID";

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

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //Username,Password,iEnable,iEnableDes,cId
            dtCustom.Columns.Add("auNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("auPSW", Type.GetType("System.String"));
            dtCustom.Columns.Add("auUnitCodeIDs", Type.GetType("System.String"));
            dtCustom.Columns.Add("auUnitCodeIDs_Des", Type.GetType("System.String"));
            dtCustom.Columns.Add("auDelflag", Type.GetType("System.String"));
            dtCustom.Columns.Add("auID", Type.GetType("System.String"));

            DataRow drCustom = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "auUnitCodeIDs_Des":
                            drCustom[strFiledArray[j]] = GetUnitCodeByIds(dt.Rows[i]["auUnitCodeIDs"] == DBNull.Value ? "" : (dt.Rows[i]["auUnitCodeIDs"].ToString().Replace("\r\n", "")));
                            break;
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["auID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("SysUsers_DS", dtCustom);

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

            string strOutputFileName = "系统用户信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
            param[0].Value = "Better_AdminUser";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "auID";

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
            // ",auPSW,auUnitCodeIDs,auUnitCodeIDs_Des,auDelflag,auID";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["auNum"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", GetUnitCodeByIds(dt.Rows[i]["auUnitCodeIDs"] == DBNull.Value ? "" : (dt.Rows[i]["auUnitCodeIDs"].ToString().Replace("\r\n", ""))));
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());

            string strOutputFileName = "系统用户信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //return File(fileContents, "application/ms-excel", strOutputFileName);

            //return File(fileContents, "application/ms-excel", "系统用户信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        [HttpGet]
        public ActionResult Exce2(string order, string page, string rows, string sort, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "Better_AdminUser";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "auID";

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

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            List<SysUserManagement_Cls> list = new List<SysUserManagement_Cls>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new SysUserManagement_Cls()
                {
                    用户名 = dt.Rows[i]["auNum"].ToString(),
                    口岸 = GetUnitCodeByIds(dt.Rows[i]["auUnitCodeIDs"] == DBNull.Value ? "" : (dt.Rows[i]["auUnitCodeIDs"].ToString().Replace("\r\n", "")))
                });
            }
           
            string strDownFileName = "系统用户信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            string strOutputFileName = Server.MapPath("~/Temp/Xls/") + strDownFileName;
            switch (browserType.ToLower())
            {
                case "safari":
                    break;
                case "mozilla":
                    break;
                default:
                    strDownFileName = HttpUtility.UrlEncode(strDownFileName);
                    break;
            }
            if (ExcelHelper.ExportExcel(list, strOutputFileName))
            {
                return File(strOutputFileName, "application/vnd.ms-excel", strDownFileName);
            }

            return View();
            //return File(fileContents, "application/ms-excel", "系统用户信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        public class SysUserManagement_Cls
        {
            public string 用户名
            {
                get;
                set;
            }

            public string 口岸
            {
                get;
                set;
            }
        }

        [HttpPost]
        public string AddUsers(string UserName, string Password, string auUnitCodeIDs)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"添加失败，原因未知\"}";
            M_SysUsers m_Users = new M_SysUsers();
            UserName = Server.UrlDecode(UserName);
            Password = MD5Util.EncodingString(Server.UrlDecode(Password));
            auUnitCodeIDs = Server.UrlDecode(auUnitCodeIDs);
            m_Users.auNum = UserName;
            m_Users.auPSW = Password;
            m_Users.auUnitCodeIDs = auUnitCodeIDs;
            m_Users.auDelflag = 0;
            try
            {
                if ((new T_SysUsers()).addUser(m_Users))
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
        public string UpdateUsers(string UserId, string UserName, string Password, string auUnitCodeIDs)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"修改失败，原因未知\"}";
            M_SysUsers m_Users = new M_SysUsers();
            UserId = Server.UrlDecode(UserId);
            UserName = Server.UrlDecode(UserName);
            Password = MD5Util.EncodingString(Server.UrlDecode(Password));
            auUnitCodeIDs = Server.UrlDecode(auUnitCodeIDs);
            m_Users.auNum = UserName;
            m_Users.auPSW = Password;
            m_Users.auUnitCodeIDs = auUnitCodeIDs;
            m_Users.auID = Convert.ToInt32(UserId);
            m_Users.auDelflag = 0;
            try
            {
                if ((new T_SysUsers()).updateUser(m_Users))
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
            string strRet = "{\"result\":\"error\",\"message\":\"删除用户信息失败，原因未知\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tUser.deleteUsers(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"删除用户信息成功\"}";
                    //      strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"删除用户信息失败\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"删除用户信息失败，原因:" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpGet]
        public string ExistUserNum(string strUserNum)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            strUserNum = Server.UrlDecode(strUserNum);
            try
            {

                if (tUser.UserExists(strUserNum))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此用户号已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }


        [HttpGet]
        public string ExistUserNum_Update(string id, string strUserNum)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            id = Server.UrlDecode(id);
            strUserNum = Server.UrlDecode(strUserNum);
            try
            {

                if (tUser.UserExists(Convert.ToInt32(id), strUserNum))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此用户号已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;

        }

        protected string GetUnitCodeByIds(string Ids)
        {
            DataSet ds = null;
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();
            if (Ids != null)
            {
                ds = new T_UnitCode().GetUnitCodeByIds(Ids);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sb.AppendFormat("{0}", dt.Rows[i]["ucName"].ToString());
                            if (i != dt.Rows.Count - 1)
                            {
                                sb.Append(",");
                            }

                        }
                    }
                }
            }

            return sb.ToString();
        }

    }
}
