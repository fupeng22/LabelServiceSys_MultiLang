using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SQLDAL;
using System.Text;

namespace LabelServiceSys
{
    public partial class ViewLoginInfo : System.Web.UI.Page
    {
        string devicetype = "";
        string isdvir = "";
        string usernumber = "";
        string unbind = "";
        string msn = "";
        //string workstation = "";
        //string getWorkstation = "";

        string strLoginURL = "LoginCenter.aspx?devicetype=";
        //strViewLoginURL = "ViewLoginInfo.aspx?devicetype=" + Server.UrlEncode(devicetype) + "&isdvir=" + Server.UrlEncode(isdvir) + "&usernumber=" + Server.UrlEncode(usernumber) + "&workstation=" + Server.UrlEncode(workstation) + "&getWorkstation=" + Server.UrlEncode(getWorkstation);
        protected void Page_Load(object sender, EventArgs e)
        {
            devicetype = Request.QueryString["devicetype"];
            isdvir = Request.QueryString["isdvir"];
            usernumber = Request.QueryString["usernumber"];
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
            SetJobNameInfo(usernumber);
        }

        protected void SetJobNameInfo(string usernumber)
        {
            DataSet ds = null;
            DataTable dt = null;
            ds = new T_OperationLog_Login().GetLogin(usernumber);
            lblJobContentText1.Text = "";
            lblJobContentText2.Text = "";
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblJobContentText1.Text = dt.Rows[0]["vchar_WorkProcess_L1_Text"].ToString();
                    lblJobContentText2.Text = dt.Rows[0]["vchar_WorkProcess_L2_Text"].ToString();
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (new T_OperationLog_Login().LoginOut(lblCurrentUser.Text))
            {
                strLoginURL = "LoginCenter.aspx?devicetype=" + Server.UrlEncode(devicetype) + "&isdvir=" + Server.UrlEncode(isdvir) + "&usernumber=" + Server.UrlEncode(usernumber);
                Response.Redirect(strLoginURL);
            }
            else
            {
                SetTips("Unbing failure!", 1);
            }

        }

        protected void SetTips(string msg, int tipType)
        {
            switch (tipType)
            {
                case 0:
                    lblTips.ForeColor = System.Drawing.Color.Blue;
                    lblTips.Text = msg;
                    break;
                case 1:
                    lblTips.ForeColor = System.Drawing.Color.Red;
                    lblTips.Text = msg;
                    break;
                default:
                    break;
            }
        }
    }
}