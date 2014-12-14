using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Util;
using SQLDAL;
using System.Text;

namespace LabelServiceSys.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public string Login(string strUserName, string strUserPwd)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"登陆失败，原因未知\"}";
            DataSet ds = null;
            DataTable dt = null;
            DataSet dsTmp = null;
            DataTable dtTmp = null;
            StringBuilder sb = new StringBuilder();
            strUserName = Server.UrlDecode(strUserName);
            strUserPwd = Server.UrlDecode(strUserPwd);
            try
            {
                if (new T_SysUsers().Login(strUserName, MD5Util.EncodingString(strUserPwd)))
                {
                    Session["Global_UserName"] = strUserName;
                    ds = new T_SysUsers().GetUseByUsername(strUserName);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //保存登陆用户的信息
                            Session["Global_UnitCode"] = dt.Rows[0]["auUnitCodeIDs"].ToString();
                            //当前登录用户为系统超级管理员
                            if (PubVariables.SysUserNames.Contains(Session["Global_UserName"].ToString().ToLower()))
                            {
                                Session["Global_Roles"] = "超级管理员";
                            }
                            else//当前登录用户为非超级管理员
                            {
                                Session["Global_Roles"] = "";
                                dsTmp = new T_UnitCode().GetUnitCodeByIds(Session["Global_UnitCode"].ToString());
                                if (dsTmp != null)
                                {
                                    dtTmp=dsTmp.Tables[0];
                                    if (dtTmp!=null && dtTmp.Rows.Count>0)
                                    {
                                        sb = new StringBuilder("");
                                        for (int j = 0; j < dtTmp.Rows.Count; j++)
                                        {
                                            sb.AppendFormat("{0}", dtTmp.Rows[j]["ucName"].ToString().Trim());
                                            if (j!=dtTmp.Rows.Count-1)
                                            {
                                                sb.Append(",");
                                            }
                                        }
                                    }
                                }
                                Session["Global_Roles"] = sb.ToString();
                            }
                        }
                    }
                    strRet = "{\"result\":\"ok\",\"message\":\"登录成功\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"登录失败，用户名与密码不匹配\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"登录失败，原因:" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpPost]
        public string Logout()
        {
            string strRet = "{\"result\":\"error\",\"message\":\"注销失败，原因未知\"}";

            try
            {
                Session["Global_UserName"] = null;
                strRet = "{\"result\":\"ok\",\"message\":\"注销成功\"}";
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"注销失败，原因:" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpPost]
        public string IsAdminRole()
        {
            string strRet = "{\"result\":\"error\",\"message\":\"\"}";

            try
            {
                if (PubVariables.SysUserNames.Contains(Session["Global_UserName"].ToString().ToLower()))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"1\"}";
                }
                else//当前登录用户为非超级管理员
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"0\"}";
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
