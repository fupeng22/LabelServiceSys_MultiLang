using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using Util;
using System.Drawing;
using System.Drawing.Text;
using System.Threading;
using LabelServiceSys.Models;
using LabelServiceSys.Filter;
using System.Drawing.Imaging;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class UserController : Controller
    {
        T_User tUser = new SQLDAL.T_User();

        public const string strFileds = "urNum,urPSW,urName,urSex,urSexDesc,urAge,urStaffNum,urDept,urDuty,urUnitCode,urMemo,urDelflag,urDelflag_Des,urID";
        public const string strFileds_Barcode = "urNum,urName,urSex,urAge,urStaffNum,urDept,urDuty,urUnitCode,urMemo,urID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/User.rdlc";
        public const string STR_PRINT_BARCODE_URL = "~/Content/Reports/PrintBarcode.rdlc";

        public const string STR_BARCODE_PATH = "~/Temp/imgs/";

        List<string> lstTitle = new List<string> { "用户号", "用户姓名", "性别", "年龄", "员工工号", "员工部门", "员工职位", "状态", "口岸" };

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
            param[0].Value = "V_Better_User";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "urID";

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
            param[0].Value = "V_Better_User";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "urID";

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
            //urNum,urPSW,urName,urSex,urSexDesc,urAge,urStaffNum,urDept,urDuty,urUnitCode,urMemo,urDelflag,urID
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urPSW", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urSex", Type.GetType("System.String"));
            dtCustom.Columns.Add("urSexDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("urAge", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("urMemo", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDelflag", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDelflag_Des", Type.GetType("System.String"));
            dtCustom.Columns.Add("urID", Type.GetType("System.String"));

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
                if (drCustom["urID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("User_DS", dtCustom);

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
            param[0].Value = "V_Better_User";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "urID";

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
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urPSW", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urSex", Type.GetType("System.String"));
            dtCustom.Columns.Add("urSexDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("urAge", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("urMemo", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDelflag", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDelflag_Des", Type.GetType("System.String"));
            dtCustom.Columns.Add("urID", Type.GetType("System.String"));

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
                if (drCustom["urID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("User_DS", dtCustom);

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

            string strOutputFileName = LangHelper.GetLangbyKey("User_Controller_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
            param[0].Value = "V_Better_User";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "urID";

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
            //urNum,urPSW,urName,urSex,urSexDesc,urAge,urStaffNum,urDept,urDuty,urUnitCode,urMemo,urDelflag,urDelflag_Des,urID
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urNum"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urName"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urSexDesc"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urAge"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urStaffNum"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urDept"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urDuty"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urDelflag_Des"].ToString());
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i]["urUnitCode"].ToString());
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            string strOutputFileName = LangHelper.GetLangbyKey("User_Controller_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
            //return File(fileContents, "application/ms-excel", "员工信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                M_User m_user = new M_User();

                string txturNum = collection["txturNum"].ToString();
                string txturPSW = collection["txturPSW"].ToString();
                string txtReurPSW = collection["txtReurPSW"].ToString();
                string txturName = collection["txturName"].ToString();
                string txturSex = collection["txturSex"].ToString();
                string txturAge = collection["txturAge"].ToString();
                string txturStaffNum = collection["txturStaffNum"].ToString();
                string txturDept = collection["txturDept"].ToString();
                string txturDuty = collection["txturDuty"].ToString();
                string txturUnitCode = collection["txturUnitCode"].ToString();
                string txturMemo = collection["areaurMemo"].ToString();

                m_user.urNum = txturNum.ToUpper();
                m_user.urPSW = MD5Util.EncodingString(txturPSW);
                m_user.urName = txturName;
                try
                {
                    m_user.urSex = Convert.ToInt32(txturSex); ;
                }
                catch (Exception)
                {
                    m_user.urSex = 0; ;
                }
                try
                {
                    m_user.urAge = Convert.ToInt32(txturAge);
                }
                catch (Exception)
                {
                    m_user.urAge = 0;
                }

                m_user.urStaffNum = txturStaffNum;
                m_user.urDept = txturDept;
                m_user.urDuty = txturDuty;
                m_user.urUnitCode = txturUnitCode;
                m_user.urMemo = txturMemo;
                m_user.urDelflag = 0;

                new T_User().addUser(m_user);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            M_User m_User = new M_User();

            DataSet ds = tUser.GetUserByurID(id.ToString());
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    m_User.urID = Convert.ToInt32(dt.Rows[0]["urID"]);
                    m_User.urNum = dt.Rows[0]["urNum"].ToString();
                    m_User.urPSW = "";// dt.Rows[0]["urPSW"].ToString();
                    m_User.urName = dt.Rows[0]["urName"].ToString();
                    m_User.urSex = Convert.ToInt32(dt.Rows[0]["urSex"].ToString());
                    m_User.urAge = Convert.ToInt32(dt.Rows[0]["urAge"].ToString());
                    m_User.urStaffNum = dt.Rows[0]["urStaffNum"].ToString();
                    m_User.urDept = dt.Rows[0]["urDept"].ToString();
                    m_User.urDuty = dt.Rows[0]["urDuty"].ToString();
                    m_User.urUnitCode = dt.Rows[0]["urUnitCode"].ToString();
                    m_User.urMemo = dt.Rows[0]["urMemo"].ToString();
                }
            }
            else
            {

            }

            return View(m_User);
        }


        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                M_User m_user = new Model.M_User();
                string txturNum = collection["txturNum"].ToString();
                string txturPSW = collection["txturPSW"].ToString();
                string txtReurPSW = collection["txtReurPSW"].ToString();
                string txturName = collection["txturName"].ToString();
                string txturSex = collection["txturSex"].ToString();
                string txturAge = collection["txturAge"].ToString();
                string txturStaffNum = collection["txturStaffNum"].ToString();
                string txturDept = collection["txturDept"].ToString();
                string txturDuty = collection["txturDuty"].ToString();
                string txturUnitCode = collection["txturUnitCode"].ToString();
                string txturMemo = collection["areaurMemo"].ToString();
                string txturID = collection["txturID"].ToString();

                m_user.urNum = txturNum.ToUpper();
                m_user.urPSW = MD5Util.EncodingString(txturPSW);
                m_user.urName = txturName;
                try
                {
                    m_user.urSex = Convert.ToInt32(txturSex); ;
                }
                catch (Exception)
                {
                    m_user.urSex = 0; ;
                }
                try
                {
                    m_user.urAge = Convert.ToInt32(txturAge);
                }
                catch (Exception)
                {
                    m_user.urAge = 0;
                }

                m_user.urStaffNum = txturStaffNum;
                m_user.urDept = txturDept;
                m_user.urDuty = txturDuty;
                m_user.urUnitCode = txturUnitCode;
                m_user.urMemo = txturMemo;
                m_user.urID = Convert.ToInt32(txturID);

                new T_User().updateUser(m_user);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            return View();
        }


        [HttpPost]
        public string Delete(string ids)
        {
            //string strRet = "{\"result\":\"error\",\"message\":\"删除用户信息失败，原因未知\"}";
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tUser.deleteUsers(ids))
                {
                    //strRet = "{\"result\":\"ok\",\"message\":\"删除用户信息成功\"}";
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    //strRet = "{\"result\":\"error\",\"message\":\"删除用户信息失败\"}";
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                //strRet = "{\"result\":\"error\",\"message\":\"删除用户信息失败，原因:" + ex.Message + "\"}";
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpPost]
        public string EnableUser(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"启用用户失败，原因未知\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tUser.enableUsers(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"启用用户成功\"}";
                    //strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"启用用户失败\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"启用用户失败，原因:" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpPost]
        public string DisableUser(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"禁用用户失败，原因未知\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tUser.disableUsers(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"禁用用户成功\"}";
                    //strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"禁用用户失败\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"禁用用户失败，原因:" + ex.Message + "\"}";
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
                    //strRet = "{\"result\":\"error\",\"message\":\"" + "此用户号已经使用" + "\"}";
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }


        [HttpGet]
        public string ExistUserNum_Update(int id, string strUserNum)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            strUserNum = Server.UrlDecode(strUserNum);
            try
            {

                if (tUser.UserExists(id, strUserNum))
                {
                    //strRet = "{\"result\":\"error\",\"message\":\"" + "此用户号已经使用" + "\"}";
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;

        }

        [HttpGet]
        public ActionResult PrintBarcode(string urIDs)
        {
            DataSet ds = null;
            DataTable dt = null;
            urIDs = Server.UrlDecode(urIDs);
            ds = new T_User().GetUserByurIDs(urIDs);

            DataTable dtCustom = new DataTable();
            //"urNum,urName,urSex,urSexDesc,urAge,urStaffNum,urDept,urDuty,urUnitCode,urMemo,urID";
            dtCustom.Columns.Add("urNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urName", Type.GetType("System.String"));
            dtCustom.Columns.Add("urSex", Type.GetType("System.String"));
            dtCustom.Columns.Add("urAge", Type.GetType("System.String"));
            dtCustom.Columns.Add("urStaffNum", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDept", Type.GetType("System.String"));
            dtCustom.Columns.Add("urDuty", Type.GetType("System.String"));
            dtCustom.Columns.Add("urUnitCode", Type.GetType("System.String"));
            dtCustom.Columns.Add("urMemo", Type.GetType("System.String"));
            dtCustom.Columns.Add("urID", Type.GetType("System.String"));
            DataRow drCustom = null;

            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        drCustom = dtCustom.NewRow();

                        string[] strFiledArray = strFileds_Barcode.Split(',');
                        for (int j = 0; j < strFiledArray.Length; j++)
                        {
                            switch (strFiledArray[j])
                            {
                                default:
                                    drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                                    break;
                            }

                        }
                        if (drCustom["urID"].ToString() != "")
                        {
                            dtCustom.Rows.Add(drCustom);
                        }
                    }
                    dt = null;


                }
            }

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_PRINT_BARCODE_URL);
            ReportDataSource reportDataSource = new ReportDataSource("PrintBarcode_DS", dtCustom);
            localReport.EnableExternalImages = true;

            ReportParameter var_BarcodePath = new ReportParameter("BarcodePath", GetBarcodePath_());

            localReport.SetParameters(new ReportParameter[] { var_BarcodePath });

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
        public ActionResult CreateBarcodeByUserNumber(string strUserNumber)
        {
            DataSet ds = null;
            DataTable dt = null;
            string strBarcode = "";

            byte[] byteImage = null;
            string BarcodePath = "";

            ds = new T_User().GetUserByurNumber(strUserNumber);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    strBarcode = "#2#" + dt.Rows[0]["urNum"].ToString();
                    //using (Bitmap bitMap = new System.Drawing.Bitmap(strBarcode.Length * 40, 80))
                    //{
                    //    using (Graphics graphics = Graphics.FromImage(bitMap))
                    //    {
                    //        PrivateFontCollection fonts = new PrivateFontCollection();
                    //        fonts.AddFontFile(Server.MapPath("~/Font/IDAutomationHC39M.ttf"));
                    //        FontFamily ff = new FontFamily("IDAutomationHC39M", fonts);
                    //        Font oFont = new Font(ff, 16);
                    //        PointF point = new PointF(2f, 2f);
                    //        SolidBrush blackBrush = new SolidBrush(Color.Black);
                    //        SolidBrush whiteBrush = new SolidBrush(Color.White);
                    //        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    //        graphics.DrawString(strUserNumber, oFont, blackBrush, point);
                    //    }
                    //    using (MemoryStream ms = new MemoryStream())
                    //    {
                    //        bitMap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                    //        byteImage = ms.ToArray();
                    //        Convert.ToBase64String(byteImage);
                    //        ms.Close();
                    //    }
                    //}
                    Code128 _Code = new Code128();
                    _Code.ValueFont = new Font("宋体", 20);
                    //_Code.ValueFont = null;
                    System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(strBarcode, Code128.Encode.Code128A);
                    BarcodePath = Server.MapPath(STR_BARCODE_PATH) + "BarCode" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".gif";
                    //imgTemp.Save(BarcodePath, System.Drawing.Imaging.ImageFormat.Gif);

                    Bitmap bmpTemp = new Bitmap(imgTemp);
                    Bitmap bmp = new Bitmap(bmpTemp);
                    bmpTemp.Dispose();
                    imgTemp.Save(BarcodePath, ImageFormat.Gif);

                    //Thread.Sleep(2000);
                }
            }
            return File(BarcodePath, "image/jpeg");
        }

        [HttpGet]
        public string GetBarcodePath_()
        {
            string strUrl = "";
            string strAbsoluteUri = Request.Url.AbsoluteUri;
            string[] UrlArr = strAbsoluteUri.Split('/');
            for (int i = 0; i < UrlArr.Length; i++)
            {
                if (i != UrlArr.Length - 1)
                {
                    strUrl = strUrl + UrlArr[i] + "/";
                }
            }
            strUrl = strUrl + "CreateBarcodeByUserNumber?strUserNumber=";
            return strUrl;
        }
    }
}
