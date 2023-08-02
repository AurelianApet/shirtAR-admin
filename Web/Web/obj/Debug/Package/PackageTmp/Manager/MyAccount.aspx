<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="Web.Manager.MyAccount" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(3);
            setSubMenu(1);
            if ($("#<%=isAdmin.ClientID %>").val() == 1) {
                $("#tdAccountList").css("display", "");
                $("#tdEnvSetting").css("display", "");
                $("#tdLogManager").css("display", "");
            } else {
                $("#tdAccountList").css("display", "none");
                $("#tdEnvSetting").css("display", "none");
                $("#tdLogManager").css("display", "none");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="isAdmin" runat="server" Value="0"/>
    <table cellpadding="0" cellspacing="0" width="1400" align="center" bgcolor="#FBFBF9" style="border-collapse:collapse;">
        <tr>
            <td width="204" height="850" style="border-width:1px; border-top-color:black; border-right-color:rgb(232,232,230); border-bottom-color:black; border-left-color:rgb(232,232,230); border-top-style:none; border-right-style:solid; border-bottom-style:none; border-left-style:solid;" valign="top">
                <div align="left">
                    <table cellpadding="0" cellspacing="0" width="206">
                        <tr>
                            <td width="206" height="39">&nbsp;</td>
                        </tr>
                        <tr>
                            <td id="tdMyAccount" width="206" height="60" align="center">
                                <input id="subMenu1" type="button" class="btnSubMenu" onclick="window.location='/Manager/MyAccount.aspx';" value="<%=Resources.Lang.STR_MY_ACCOUNT %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdAccountList" width="206" height="60" align="center">
                                <input id="subMenu2" type="button" class="btnSubMenu" onclick="window.location='/Manager/AccountList.aspx';" value="<%=Resources.Lang.STR_ACCOUNT_LIST %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdEnvSetting" width="206" height="60" align="center">
                                <input id="subMenu3" type="button" class="btnSubMenu" onclick="window.location='/Manager/Setting.aspx';" value="<%=Resources.Lang.STR_ENV_SETTING %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdLogManager" width="206" height="60" align="center">
                                <input id="subMenu4" type="button" class="btnSubMenu" onclick="window.location='/Manager/LogManager.aspx';" value="<%=Resources.Lang.STR_LOG_MANAGE %>"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td width="1193" height="850" bgcolor="whitesmoke" style="border-width:1px; border-top-color:black; border-right-color:rgb(232,232,230); border-bottom-color:black; border-left-color:rgb(232,232,230); border-top-style:none; border-right-style:solid; border-bottom-style:none; border-left-style:solid;" valign="top">
                <table cellpadding="0" cellspacing="0" width="732" align="center">
                    <tr>
                        <td width="732" height="39">
                        </td>
                    </tr>
                    <tr>
                        <td width="732" height="107" align="center">                                                
                            <span style="font-size:16pt; color:#BA4241; font-weight:bold;"><%=Resources.Lang.STR_MY_ACCOUNT_INFO %></span>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" align="center" width="491">
                    <tr>
                        <td width="231" height="56">                                    
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_NICKNAME%></span>
                        </td>
                        <td width="260" height="56">
                            <span style="font-size:11pt; color:#333333;"><asp:Label ID="lbl_Nickname" runat="server"></asp:Label></span>
                        </td>
                    </tr>
                    <tr>
                        <td width="231" height="56">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_LOGINID%></span>
                        </td>
                        <td width="260" height="56">
                            <span style="font-size:11pt; color:#333333;"><asp:Label ID="lbl_LoginID" runat="server"></asp:Label></span>
                        </td>
                    </tr>
                    <tr>
                        <td width="231" height="28">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_PASSWORD%></span>
                        </td>
                        <td width="260" height="56">
                            <table cellpadding="0" cellspacing="0" width="235">
                                <tr>
                                    <td width="120" height="32" align="left">
                                        <font color="#333333">
                                            <input type="text" name="txt_LoginPWD" size="14" value="●●●●●●" />
                                            <asp:HiddenField ID="hd_LoginPWD" runat="server" Value=""/>
                                        </font>
                                    </td>
                                    <td width="115" height="32">
                                        <input type="button" class="btnSmallGreen" onclick="showPopupPWD()" value="<%=Resources.Lang.STR_PASSWORD_CHANGE %>" style="width:90px; height:21px;"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="231" height="28">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_ACCOUNT_LEVEL%></span>
                        </td>
                        <td width="260" height="56">
                            <span style="font-size:11pt; color:#333333;"><asp:Label ID="lbl_Level" runat="server"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
