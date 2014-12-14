using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQLDAL;
using System.Data;
using LabelServiceSys.Models;

namespace LabelServiceSys
{
    public partial class LoginCenter : System.Web.UI.Page
    {
        string devicetype = "";
        string isdvir = "";
        string usernumber = "";
        string unbind = "";
        string msn = "";
        //string workstation = "";
        //string getWorkstation = "";

        public const string STR_LOGIN = "LOGIN";
        public const string STR_LOGOUT = "LOGOUT";

        string strViewLoginURL = "ViewLoginInfo.aspx?devicetype=";

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            DataRow dr = null;

            devicetype = Request.QueryString["devicetype"];
            isdvir = Request.QueryString["isdvir"];
            usernumber = Request.QueryString["usernumber"];
            unbind = Request.QueryString["unbind"];
            msn = Request.QueryString["msn"];
            //workstation = Request.QueryString["workstation"];
            //getWorkstation = Request.QueryString["getWorkstation"];

            if (string.IsNullOrEmpty(devicetype))
            {
                SetTips("The device type is not specified", 1);
                return;
            }

            if (string.IsNullOrEmpty(isdvir))
            {
                SetTips("Do not specify whether DVIR", 1);
                return;
            }

            if (string.IsNullOrEmpty(usernumber))
            {
                SetTips("The user name not specified", 1);
                return;
            }

            lblCurrentUser.Text = usernumber;
            switch (isdvir)
            {
                case "0":
                    chkDVIR.Checked = false;
                    break;
                default:
                    chkDVIR.Checked = true;
                    break;
            }
            hd_isDVIR.Value = isdvir;

            if (!IsPostBack)
            {
                BindJobContent1(usernumber);
            }

            if (!new T_User().UserExists(usernumber))
            {
                SetTips("The user name is not exist!", 1);
                return;
            }

            if (string.IsNullOrEmpty(unbind))//请求绑定
            {
                ds = new T_OperationLog_Login().GetLogin(usernumber);
                if (ds == null)//表明之前从没有进行过登录登出操作,直接返回登陆页
                {
                    try
                    {
                        switch (isdvir)
                        {
                            case "0":
                                break;
                            default:
                                defaultBind(isdvir);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
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
                                strViewLoginURL = "ViewLoginInfo.aspx?devicetype=" + Server.UrlEncode(devicetype) + "&isdvir=" + Server.UrlEncode(isdvir) + "&usernumber=" + Server.UrlEncode(usernumber) ;
                                Response.Redirect(strViewLoginURL);
                                break;
                            case STR_LOGOUT://之前为登出状态,直接返回登陆页
                                try
                                {
                                    switch (isdvir)
                                    {
                                        case "0":
                                            break;
                                        default:
                                            defaultBind(isdvir);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                break;
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
                        ds = new T_OperationLog_Login().GetLogin(usernumber);
                        if (ds == null)//表明之前从没有进行过登录登出操作,直接返回登陆页
                        {
                            try
                            {
                                switch (isdvir)
                                {
                                    case "0":
                                        break;
                                    default:
                                        defaultBind(isdvir);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
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
                                        strViewLoginURL = "ViewLoginInfo.aspx?devicetype=" + Server.UrlEncode(devicetype) + "&isdvir=" + Server.UrlEncode(isdvir) + "&usernumber=" + Server.UrlEncode(usernumber) ;
                                        Response.Redirect(strViewLoginURL);
                                        break;
                                    case STR_LOGOUT://之前为登出状态,直接返回登陆页
                                        try
                                        {
                                            switch (isdvir)
                                            {
                                                case "0":
                                                    break;
                                                default:
                                                    defaultBind(isdvir);
                                                    break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        break;
                    case "1"://请求解绑,先进行解绑操作，接着返回登陆页
                        if (new T_OperationLog_Login().LoginOut(usernumber))
                        {
                            try
                            {
                                switch (isdvir)
                                {
                                    case "0":
                                        break;
                                    default:
                                        defaultBind(isdvir);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //throw new Exception("解绑失败");
                            throw new Exception(LangHelper.GetLangbyKey("LoginCenter_Controller_UnBindError"));
                        }
                        break;
                    default:
                        break;
                }
            }

            switch (isdvir)
            {
                case "0":
                    chkDVIR.Checked = false;
                    break;
                default:
                    chkDVIR.Checked = true;
                    break;
            }
            hd_isDVIR.Value = isdvir;

            lblCurrentUser.Text = usernumber;
        }

        protected void BindJobContent1(string userNumber)
        {
            DataSet ds = null;
            DataTable dt = null;
            ds = new T_User().GetJobNameL1ByUserNumber(userNumber);
            ddlJobContent1.Items.Clear();
            if (ds != null)
            {
                dt = ds.Tables[0];
                ddlJobContent1.DataSource = dt;
                ddlJobContent1.DataValueField = "vchar_WorkProcess_L1_Code";
                ddlJobContent1.DataTextField = "vchar_WorkProcess_L1_Text";
                ddlJobContent1.DataBind();
            }
            ddlJobContent1.Items.Insert(0, new ListItem()
            {
                Text = "--Please select--",
                Value = "-99",
                Selected = true
            });
        }

        protected void ddlJobContent1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            ds = new T_User().GetJobNameL2ByJobNameL1Code(ddlJobContent1.SelectedValue);
            ddlJobContent2.Items.Clear();
            if (ds != null)
            {
                dt = ds.Tables[0];
                ddlJobContent2.DataSource = dt;
                ddlJobContent2.DataValueField = "vchar_WorkProcess_L2_Code";
                ddlJobContent2.DataTextField = "vchar_WorkProcess_L2_Text";
                ddlJobContent2.DataBind();
            }
            ddlJobContent2.Items.Insert(0, new ListItem()
            {
                Text = "--Please select--",
                Value = "-99",
                Selected = true
            });
        }

        protected void SetTips(string msg, int tipType)
        {
            //switch (tipType)
            //{
            //    case 0:
            //        lblTips.ForeColor = System.Drawing.Color.Blue;
            //        lblTips.Text = msg;
            //        break;
            //    case 1:
            //        lblTips.ForeColor = System.Drawing.Color.Red;
            //        lblTips.Text = msg;
            //        break;
            //    default:
            //        break;
            //}
        }

        protected void defaultBind(string postionNO)
        {
            DataSet ds_tmp = null;
            DataTable dt_tmp = null;
            ds_tmp = new T_tb_Setting_WorkProcess_L2().GetL1_L2InfoByPositionNO(postionNO);
            if (ds_tmp != null)
            {
                ddlJobContent1.Items.Clear();
                ddlJobContent2.Items.Clear();
                dt_tmp = ds_tmp.Tables[0];
                if (dt_tmp != null && dt_tmp.Rows.Count > 0)
                {
                    ddlJobContent1.Items.Add(new ListItem()
                    {
                        Value = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Code"].ToString(),
                        Text = dt_tmp.Rows[0]["vchar_WorkProcess_L1_Text"].ToString()
                    });
                    ddlJobContent2.Items.Add(new ListItem()
                    {
                        Value = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Code"].ToString(),
                        Text = dt_tmp.Rows[0]["vchar_WorkProcess_L2_Text"].ToString()
                    });
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strUserNum = lblCurrentUser.Text;
            string strJobNameL1Code = ddlJobContent1.SelectedValue;
            string strJobNameL2Code = ddlJobContent2.SelectedValue;
            string strIsDVIR = hd_isDVIR.Value;

            strViewLoginURL = "ViewLoginInfo.aspx?devicetype=" + Server.UrlEncode(devicetype) + "&isdvir=" + Server.UrlEncode(isdvir) + "&usernumber=" + Server.UrlEncode(usernumber) ;

            if (strJobNameL1Code == "" || strJobNameL1Code == "-99")
            {
                SetTips("Please select job content1", 1);
                return;
            }
            if (strJobNameL2Code == "" || strJobNameL2Code == "-99")
            {
                SetTips("Please select job content2", 1);
                return;
            }
            try
            {
                if (new T_OperationLog_Login().Login(strUserNum, strJobNameL1Code, strJobNameL2Code, strIsDVIR))
                {
                    //strRet = "{\"result\":\"ok\",\"message\":\"" + "绑定成功,查看绑定信息" + "\"}";
                    //strRet = "{\"result\":\"ok\",\"message\":\"" + "" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage7") + "" + "\"}";
                    SetTips("Binding  successfully,view bind infomation", 0);
                    Response.Redirect(strViewLoginURL);
                }
                else
                {
                    SetTips("Bind failed", 1);
                }
            }
            catch (Exception ex)
            {
                //strRet = "{\"result\":\"error\",\"message\":\"绑定失败,原因" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("LoginCenter_Controller_ErrorMessage6") + "" + ex.Message + "\"}";
                SetTips("Bind failed, reason:" + ex.Message, 1);
            }
        }
    }
}