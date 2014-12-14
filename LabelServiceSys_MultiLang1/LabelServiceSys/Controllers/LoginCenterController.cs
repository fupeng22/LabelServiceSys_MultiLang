using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabelServiceSys.Filter;
using System.Data;
using SQLDAL;
using Model;
using LabelServiceSys.Models;

namespace LabelServiceSys.Controllers
{
    [ErrorAttribute]
    public class LoginCenterController : Controller
    {
        public const string STR_LOGIN = "LOGIN";
        public const string STR_LOGOUT = "LOGOUT";
        //
        // GET: /LoginCenter/
        [ErrorAttribute]
        public ActionResult Index()
        {
            string devicetype = Request.QueryString["devicetype"];
            string isdvir = Request.QueryString["isdvir"];
            string usernumber = Request.QueryString["usernumber"];
            string unbind = Request.QueryString["unbind"];
            string msn = Request.QueryString["msn"];
            //string workstation = Request.QueryString["workstation"];
            //string getWorkstation = Request.QueryString["getWorkstation"];

            if (devicetype == null || devicetype.Trim() == "")
            {
                //throw new Exception("未指定设备类型");
                throw new Exception(LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage1"));
            }

            if (isdvir == null || isdvir.Trim() == "")
            {
                // throw new Exception("未指定是否有DVIR");
                throw new Exception(LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage2"));
            }

            if (usernumber == null || usernumber.Trim() == "")
            {
                // throw new Exception("未指定用户名");
                throw new Exception(LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage3"));
            }

            //if (string.IsNullOrEmpty(workstation))
            //{
            //    workstation = "";
            //    getWorkstation = "0";
            //}
           
            string str_devicetype = Server.UrlDecode(devicetype).Trim();
            string str_isdvir = Server.UrlDecode(isdvir).Trim();
            string str_usernumber = Server.UrlDecode(usernumber).Trim();
            DataSet ds = null;
            DataTable dt = null;
            DataRow dr = null;
            M_LoginReturn m_LoginReturn = null;

            DataSet ds_tmp = null;
            DataTable dt_tmp = null;

            if (string.IsNullOrEmpty(unbind))//请求绑定
            {
                ds = new T_OperationLog_Login().GetLogin(str_usernumber);
                if (ds == null)//表明之前从没有进行过登录登出操作,直接返回登陆页
                {
                    m_LoginReturn = new M_LoginReturn();
                    m_LoginReturn.UserNumber = str_usernumber;
                    switch (str_isdvir)
                    {
                        case "0":
                            m_LoginReturn.IsDVIR = 0;
                            m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                            m_LoginReturn.IsDVIRDesc_b = false;
                            break;
                        case "1":
                            m_LoginReturn.IsDVIR = 1;
                            m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                            m_LoginReturn.IsDVIRDesc_b = true;
                            break;
                        default:
                            break;
                    }
                    m_LoginReturn.IsLogin = false;
                    m_LoginReturn.IsLogin_i = 0;
                    m_LoginReturn.getWorkstationId = 0;
                    m_LoginReturn.WorkstationId = "";
                    try
                    {
                        //if (!string.IsNullOrEmpty(workstation))
                        //{
                        //    if (getWorkstation == "1")
                        //    {
                        //        m_LoginReturn.getWorkstationId = 1;
                        //        m_LoginReturn.WorkstationId = workstation;
                        //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                        //        if (ds_tmp != null)
                        //        {
                        //            dt_tmp = ds_tmp.Tables[0];
                        //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                        //            {
                        //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                        //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                        //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                        //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                        //            }
                        //        }
                        //    }

                        //}
                    }
                    catch (Exception ex)
                    {
                        m_LoginReturn.getWorkstationId = 0;
                        m_LoginReturn.WorkstationId = "";
                    }
                    return View("Login", m_LoginReturn);
                }
                else
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dr = dt.Rows[0];
                        switch (dr["lgopContent"].ToString().Trim().ToUpper())
                        {
                            case STR_LOGIN://之前为登录状态,则直接返回已登录信息
                                m_LoginReturn = new M_LoginReturn();
                                switch (dr["lgopIsDvir"].ToString())
                                {
                                    case "0":
                                        m_LoginReturn.IsDVIR = 0;
                                        m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                                        m_LoginReturn.IsDVIRDesc_b = false;
                                        break;
                                    case "1":
                                        m_LoginReturn.IsDVIR = 1;
                                        m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                                        m_LoginReturn.IsDVIRDesc_b = true;
                                        break;
                                    default:
                                        break;
                                }
                                m_LoginReturn.IsLogin = true;
                                m_LoginReturn.WorkProcess_L1_Code = dr["lgopJobNameIdLv1"].ToString();
                                m_LoginReturn.WorkProcess_L2_Code = dr["lgopJobNameIdLv2"].ToString();
                                m_LoginReturn.WorkProcess_L1_Text = dr["vchar_WorkProcess_L1_Text"].ToString();
                                m_LoginReturn.WorkProcess_L2_Text = dr["vchar_WorkProcess_L2_Text"].ToString();
                                m_LoginReturn.UserNumber = str_usernumber;
                                m_LoginReturn.IsLogin_i = 1;
                                m_LoginReturn.getWorkstationId = 0;
                                m_LoginReturn.WorkstationId = "";
                                try
                                {
                                    //if (!string.IsNullOrEmpty(workstation))
                                    //{
                                    //    if (getWorkstation == "1")
                                    //    {
                                    //        m_LoginReturn.getWorkstationId = 1;
                                    //        m_LoginReturn.WorkstationId = workstation;
                                    //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                                    //        if (ds_tmp != null)
                                    //        {
                                    //            dt_tmp = ds_tmp.Tables[0];
                                    //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                                    //            {
                                    //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                                    //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                                    //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                                    //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    m_LoginReturn.getWorkstationId = 0;
                                    m_LoginReturn.WorkstationId = "";
                                }
                                return View(m_LoginReturn);
                            case STR_LOGOUT://之前为登出状态,直接返回登陆页
                                m_LoginReturn = new M_LoginReturn();
                                m_LoginReturn.UserNumber = str_usernumber;
                                switch (str_isdvir)
                                {
                                    case "0":
                                        m_LoginReturn.IsDVIR = 0;
                                        m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                                        m_LoginReturn.IsDVIRDesc_b = false;
                                        break;
                                    case "1":
                                        m_LoginReturn.IsDVIR = 1;
                                        m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                                        m_LoginReturn.IsDVIRDesc_b = true;
                                        break;
                                    default:
                                        break;
                                }
                                m_LoginReturn.IsLogin = false;
                                m_LoginReturn.IsLogin_i = 0;
                                m_LoginReturn.getWorkstationId = 0;
                                m_LoginReturn.WorkstationId = "";
                                try
                                {
                                    //if (!string.IsNullOrEmpty(workstation))
                                    //{
                                    //    if (getWorkstation == "1")
                                    //    {
                                    //        m_LoginReturn.getWorkstationId = 1;
                                    //        m_LoginReturn.WorkstationId = workstation;
                                    //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                                    //        if (ds_tmp != null)
                                    //        {
                                    //            dt_tmp = ds_tmp.Tables[0];
                                    //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                                    //            {
                                    //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                                    //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                                    //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                                    //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    m_LoginReturn.getWorkstationId = 0;
                                    m_LoginReturn.WorkstationId = "";
                                }
                                return View("Login", m_LoginReturn);
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                string str_unbind = Server.UrlDecode(unbind).Trim();
                switch (str_unbind)
                {
                    case "0"://请求绑定
                        ds = new T_OperationLog_Login().GetLogin(str_usernumber);
                        if (ds == null)//表明之前从没有进行过登录登出操作,直接返回登陆页
                        {
                            m_LoginReturn = new M_LoginReturn();
                            m_LoginReturn.UserNumber = str_usernumber;
                            switch (str_isdvir)
                            {
                                case "0":
                                    m_LoginReturn.IsDVIR = 0;
                                    m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                                    m_LoginReturn.IsDVIRDesc_b = false;
                                    break;
                                case "1":
                                    m_LoginReturn.IsDVIR = 1;
                                    m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                                    m_LoginReturn.IsDVIRDesc_b = true;
                                    break;
                                default:
                                    break;
                            }
                            m_LoginReturn.IsLogin = false;
                            m_LoginReturn.IsLogin_i = 0;
                            m_LoginReturn.getWorkstationId = 0;
                            m_LoginReturn.WorkstationId = "";
                            try
                            {
                                //if (!string.IsNullOrEmpty(workstation))
                                //{
                                //    if (getWorkstation == "1")
                                //    {
                                //        m_LoginReturn.getWorkstationId = 1;
                                //        m_LoginReturn.WorkstationId = workstation;
                                //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                                //        if (ds_tmp != null)
                                //        {
                                //            dt_tmp = ds_tmp.Tables[0];
                                //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                                //            {
                                //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                                //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                                //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                                //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                                //            }
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                m_LoginReturn.getWorkstationId = 0;
                                m_LoginReturn.WorkstationId = "";
                            }
                            return View("Login", m_LoginReturn);
                        }
                        else
                        {
                            dt = ds.Tables[0];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dr = dt.Rows[0];
                                switch (dr["lgopContent"].ToString().Trim().ToUpper())
                                {
                                    case STR_LOGIN://之前为登录状态,则直接返回已登录信息
                                        m_LoginReturn = new M_LoginReturn();
                                        switch (dr["lgopIsDvir"].ToString())
                                        {
                                            case "0":
                                                m_LoginReturn.IsDVIR = 0;
                                                m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                                                m_LoginReturn.IsDVIRDesc_b = false;
                                                break;
                                            case "1":
                                                m_LoginReturn.IsDVIR = 1;
                                                m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                                                m_LoginReturn.IsDVIRDesc_b = true;
                                                break;
                                            default:
                                                break;
                                        }
                                        m_LoginReturn.IsLogin = true;
                                        m_LoginReturn.WorkProcess_L1_Code = dr["lgopJobNameIdLv1"].ToString();
                                        m_LoginReturn.WorkProcess_L2_Code = dr["lgopJobNameIdLv2"].ToString();
                                        m_LoginReturn.WorkProcess_L1_Text = dr["vchar_WorkProcess_L1_Text"].ToString();
                                        m_LoginReturn.WorkProcess_L2_Text = dr["vchar_WorkProcess_L2_Text"].ToString();
                                        m_LoginReturn.UserNumber = str_usernumber;
                                        m_LoginReturn.IsLogin_i = 1;
                                        m_LoginReturn.getWorkstationId = 0;
                                        m_LoginReturn.WorkstationId = "";
                                        try
                                        {
                                            //if (!string.IsNullOrEmpty(workstation))
                                            //{
                                            //    if (getWorkstation == "1")
                                            //    {
                                            //        m_LoginReturn.getWorkstationId = 1;
                                            //        m_LoginReturn.WorkstationId = workstation;
                                            //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                                            //        if (ds_tmp != null)
                                            //        {
                                            //            dt_tmp = ds_tmp.Tables[0];
                                            //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                                            //            {
                                            //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                                            //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                                            //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                                            //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            m_LoginReturn.getWorkstationId = 0;
                                            m_LoginReturn.WorkstationId = "";
                                        }
                                        return View(m_LoginReturn);
                                    case STR_LOGOUT://之前为登出状态,直接返回登陆页
                                        m_LoginReturn = new M_LoginReturn();
                                        m_LoginReturn.UserNumber = str_usernumber;
                                        switch (str_isdvir)
                                        {
                                            case "0":
                                                m_LoginReturn.IsDVIR = 0;
                                                m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                                                m_LoginReturn.IsDVIRDesc_b = false;
                                                break;
                                            case "1":
                                                m_LoginReturn.IsDVIR = 1;
                                                m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                                                m_LoginReturn.IsDVIRDesc_b = true;
                                                break;
                                            default:
                                                break;
                                        }
                                        m_LoginReturn.IsLogin = false;
                                        m_LoginReturn.IsLogin_i = 0;
                                        m_LoginReturn.getWorkstationId = 0;
                                        m_LoginReturn.WorkstationId = "";
                                        try
                                        {
                                            //if (!string.IsNullOrEmpty(workstation))
                                            //{
                                            //    if (getWorkstation == "1")
                                            //    {
                                            //        m_LoginReturn.getWorkstationId = 1;
                                            //        m_LoginReturn.WorkstationId = workstation;
                                            //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                                            //        if (ds_tmp != null)
                                            //        {
                                            //            dt_tmp = ds_tmp.Tables[0];
                                            //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                                            //            {
                                            //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                                            //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                                            //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                                            //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            m_LoginReturn.getWorkstationId = 0;
                                            m_LoginReturn.WorkstationId = "";
                                        }
                                        return View("Login", m_LoginReturn);
                                    default:
                                        break;
                                }
                            }
                        }
                        break;
                    case "1"://请求解绑,先进行解绑操作，接着返回登陆页
                        if (new T_OperationLog_Login().LoginOut(str_usernumber))
                        {
                            m_LoginReturn = new M_LoginReturn();
                            m_LoginReturn.UserNumber = str_usernumber;
                            switch (str_isdvir)
                            {
                                case "0":
                                    m_LoginReturn.IsDVIR = 0;
                                    m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                                    m_LoginReturn.IsDVIRDesc_b = false;
                                    break;
                                case "1":
                                    m_LoginReturn.IsDVIR = 1;
                                    m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                                    m_LoginReturn.IsDVIRDesc_b = true;
                                    break;
                                default:
                                    break;
                            }
                            m_LoginReturn.IsLogin = false;
                            m_LoginReturn.IsLogin_i = 0;
                            m_LoginReturn.getWorkstationId = 0;
                            m_LoginReturn.WorkstationId = "";
                            try
                            {
                                //if (!string.IsNullOrEmpty(workstation))
                                //{
                                //    if (getWorkstation == "1")
                                //    {
                                //        m_LoginReturn.getWorkstationId = 1;
                                //        m_LoginReturn.WorkstationId = workstation;
                                //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(workstation);
                                //        if (ds_tmp != null)
                                //        {
                                //            dt_tmp = ds_tmp.Tables[0];
                                //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                                //            {
                                //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                                //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                                //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                                //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                                //            }
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                m_LoginReturn.getWorkstationId = 0;
                                m_LoginReturn.WorkstationId = "";
                            }
                            return View("Login", m_LoginReturn);
                        }
                        else
                        {
                            //throw new Exception("解绑失败");
                            throw new Exception(LangHelper.GetLangbyKey("LoginCenter_Controller_UnBindError"));
                        }
                    default:
                        break;
                }
            }
            return View();
        }

        [HttpPost]
        public string LoginOut(string strUserNum)
        {
            //string strRet = "{\"result\":\"error\",\"message\":\"解绑失败,原因未知\"}";
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage4") + "\"}";
            strUserNum = Server.UrlDecode(strUserNum);
            try
            {

                if (new T_OperationLog_Login().LoginOut(strUserNum))
                {
                    // strRet = "{\"result\":\"ok\",\"message\":\"" + "解绑成功,转入绑定页可重新绑定" + "\"}";
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage5") + "\"}";
                }

            }
            catch (Exception ex)
            {
                //strRet = "{\"result\":\"error\",\"message\":\"解绑失败,原因" + ex.Message + "\"}";
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage4") + "" + ex.Message + "\"}";
            }

            return strRet;

        }

        public ActionResult Login(string userNumber, string isDVIR, string WorkstationId, string getWorkstationId)
        {
            DataSet ds_tmp = null;
            DataTable dt_tmp = null;
            M_LoginReturn m_LoginReturn = new M_LoginReturn();
            m_LoginReturn.UserNumber = userNumber;
            switch (isDVIR)
            {
                case "0":
                    m_LoginReturn.IsDVIR = 0;
                    m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_No");// "No";
                    break;
                case "1":
                    m_LoginReturn.IsDVIR = 1;
                    m_LoginReturn.IsDVIRDesc = LangHelper.GetLangbyKey("LoginCenter_Controller_IsDVIRDesc_Yes");// "Yes";
                    break;
                default:
                    break;
            }
            m_LoginReturn.IsLogin = false;
            m_LoginReturn.getWorkstationId = Convert.ToInt32(getWorkstationId);
            m_LoginReturn.WorkstationId = WorkstationId;
            try
            {
                //if (!string.IsNullOrEmpty(WorkstationId))
                //{
                //    if (getWorkstationId == "1")
                //    {
                //        m_LoginReturn.getWorkstationId = 1;
                //        m_LoginReturn.WorkstationId = WorkstationId;
                //        ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByWorkstationId(WorkstationId);
                //        if (ds_tmp != null)
                //        {
                //            dt_tmp = ds_tmp.Tables[0];
                //            if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                //            {
                //                m_LoginReturn.WorkProcess_L1_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString();
                //                m_LoginReturn.WorkProcess_L1_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                //                m_LoginReturn.WorkProcess_L2_Code = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString();
                //                m_LoginReturn.WorkProcess_L2_Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                m_LoginReturn.getWorkstationId = 0;
            }

            return View("Login", m_LoginReturn);
        }

        [HttpPost]
        public string Login_Post(string strUserNum, string JobNameL1Code, string JobNameL2Code, string IsDVIR)
        {
            //string strRet = "{\"result\":\"error\",\"message\":\"绑定失败,原因未知\"}";
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage6") + "\"}";
            strUserNum = Server.UrlDecode(strUserNum);
            string strJobNameL1Code = Server.UrlDecode(JobNameL1Code);
            string strJobNameL2Code = Server.UrlDecode(JobNameL2Code);
            string strIsDVIR = Server.UrlDecode(IsDVIR);
            try
            {
                if (new T_OperationLog_Login().Login(strUserNum, strJobNameL1Code, strJobNameL2Code, strIsDVIR))
                {
                    //strRet = "{\"result\":\"ok\",\"message\":\"" + "绑定成功,查看绑定信息" + "\"}";
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage7") + "" + "\"}";
                }

            }
            catch (Exception ex)
            {
                //strRet = "{\"result\":\"error\",\"message\":\"绑定失败,原因" + ex.Message + "\"}";
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage6") + "" + ex.Message + "\"}";
            }

            return strRet;

        }
    }
}
