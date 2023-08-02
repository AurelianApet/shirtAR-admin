<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Manager.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Resources.Lang.STR_TITLE %></title>
    <script type="text/javascript" src="/Scripts/jquery-1.11.2.min.js"></script>
    <link href="/Styles/shirtAR.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td width="600" height="200" align="center"></td>
            </tr>
            <tr>
                <td width="600" height="150" align="center">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="100" align="center">
                                <img src="<%=Resources.Lang.STR_LOGIN_LOGO %>" width="500" height="60" alt="" />
						    </td>
                        </tr>
                        <tr>
                            <td height="50" align="center">
                                <font style="color:#504D67; font-size:12pt;"><%=Resources.Lang.STR_LOGIN_TITLE %></font>
						    </td>
                        </tr>
                    </table>
			    </td>
            </tr>
            <tr>
                <td width="600" height="300" align="center" valign="top">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="400" height="120">
                                <table cellpadding="0" cellspacing="0" align="center">
                                    <tr>
                                        <td width="350" height="50" align="center" class="tdLogin">
                                            <asp:TextBox ID="tbxLoginID" runat="server" cssClass="txtLogin" placeholder="<%$Resources:Lang, STR_LOGIN_ID %>"></asp:TextBox>
									    </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" align="center">
                                    <tr>
                                        <td width="350" height="10">
									    </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" align="center">
                                    <tr>
                                        <td width="350" height="50" align="center" class="tdLogin">
                                            <asp:TextBox ID="tbxLoginPWD" TextMode="Password" runat="server" cssClass="txtLogin" placeholder="<%$Resources:Lang, STR_LOGIN_PWD %>"></asp:TextBox>
									    </td>
                                    </tr>
                                </table>
						    </td>
                        </tr>
                        <tr>
                            <td width="400" height="30" valign="top">
                            </td>
                        </tr>
                        <tr>
                            <td width="400" height="50" valign="top">
                                <table cellpadding="0" cellspacing="0" align="center">
                                    <tr>
                                        <td width="350" height="50" align="center">
                                            <asp:Button ID="btnLogin" runat="server" cssClass="btnLogin" Text="<%$Resources:Lang, STR_LOGIN %>" OnClick="btnLogin_Click"/>
									    </td>
                                    </tr>
	                            </table>
						    </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="400" height="50" align="center">
							    <img src="/img/logo_line.png" width="360" height="22" border="0" alt="" />
						    </td>
                        </tr>
                        <tr>
                            <td width="400" height="50">
							    <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="200" height="40" align="center">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="40" height="35">
	                                                    <asp:CheckBox ID="chkRememberMe" runat="server"/>
												    </td>
                                                    <td width="80" height="35">                                                
                                                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="chkRememberMe" CssClass="lblLogin"><%=Resources.Lang.STR_LOGIN_IDSAVE %></asp:Label>
												    </td>
			                                    </tr>
                                            </table>
                                        </td>
                                        <td width="200" height="40" align="center">
							                <table cellpadding="0" cellspacing="0">
								                <tr>
									                <td width="40" height="35">
										                <asp:CheckBox ID="chkAutoLogin" runat="server"/>
									                </td>
									                <td width="80" height="35">
										                <asp:Label ID="AutoLoginLabel" runat="server" AssociatedControlID="chkAutoLogin" CssClass="lblLogin"><%=Resources.Lang.STR_LOGIN_AUTOLOGIN %></asp:Label>
									                </td>
                                                </tr>
                                            </table>
						                </td>
                                    </tr>
                                </table>
						    </td>
					    </tr>
                    </table>
			    </td>
	        </tr>
	    </table>
    </form>
</body>
</html>
