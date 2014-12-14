﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginCenter.aspx.cs" Inherits="LabelServiceSys.LoginCenter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <title></title>
</head>
<body style="background-color: #FFFF80; padding: 0px; margin: 0">
    <form id="form1" runat="server">
    User:<asp:Label ID="lblCurrentUser" runat="server"></asp:Label>
    <table style="padding: 0px; font-size: 12px;">
        <%-- <tr style="display: none">
            <td>
                <asp:Label ID="lblTips" runat="server"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td>
                <asp:CheckBox ID="chkDVIR" runat="server" Text="Has Dvir:        " TextAlign="Left"
                    Enabled="false" />
                <input type="hidden" id="hd_isDVIR" runat="server" name="hd_isDVIR" />
            </td>
        </tr>
        <tr>
            <td>
                Job content1:
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;<asp:DropDownList ID="ddlJobContent1" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlJobContent1_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Job content2:
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;<asp:DropDownList ID="ddlJobContent2" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnLogin" runat="server" Text="Bind" OnClick="btnLogin_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
