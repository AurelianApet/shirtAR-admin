<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="Web.Manager.AccountList" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(3);
            setSubMenu(2);
        });
        function showAddAccountPopup() {
            showPopup("divAddAccount");
        }
        function checkStringFormat(string) {
            var stringRegx = /[ \\|\[\]\:\'\?\/\,\-\;~!\#$%<>^&*\()=+_\’\"]/gi; 
           var isValid = true; 
           if(stringRegx.test(string)) { 
             isValid = false; 
           } 
           return isValid; 
        }
        function AddAccount() {
            if ($("#<%=div_txt_Nickname.ClientID %>").val() == "") {
                //등록자명을 입력하십시오.
                alert("<%=Resources.Lang.MSG_INPUT_NICKNAME %>");
                return;
            }

            if (!checkStringFormat($("#<%=div_txt_Nickname.ClientID %>").val())) {
                //등록자명에 특수문자가 존재합니다.
                alert("<%=Resources.Lang.MSG_NICKNAME_EXIST_SPEC %>");
                return;
            }

            if ($("#<%=div_txt_LoginID.ClientID %>").val() == "") {
                //등록자ID를 입력하십시오.
                alert("<%=Resources.Lang.MSG_INPUT_LOGINID %>");
                return;
            }

            if (!checkStringFormat($("#<%=div_txt_LoginID.ClientID %>").val())) {
                //등록자ID에 특수문자가 존재합니다.
                alert("<%=Resources.Lang.MSG_LOGINID_EXIST_SPEC %>");
                return;
            }

            if ($("#<%=div_txt_LoginPWD.ClientID %>").val() == "") {
                //패스워드를 입력하십시오.
                alert("<%=Resources.Lang.MSG_INPUT_PASSWORD %>");
                return;
            }

            if ($("#<%=div_txt_LoginConfirmPWD.ClientID %>").val() == "") {
                //확인 패스워드를 입력하십시오.
                alert("<%=Resources.Lang.MSG_INPUT_PASSWORD_CONFIRM %>");
                return;
            }

            if ($("#<%=div_txt_LoginPWD.ClientID %>").val() != $("#<%=div_txt_LoginConfirmPWD.ClientID %>").val()) {
                //입력하신 패스워드가 일치하지 않습니다.
                alert("<%=Resources.Lang.MSG_INPUT_PASSWORD_ERROR %>");
                return;
            }

            var level = $("#select_level option:selected").val();
            var qrcode_count = $("#select_qrcode_count option:selected").val();

            $.ajax({
                url: "AddAccount.aspx?nickname=" + encodeURIComponent($("#<%=div_txt_Nickname.ClientID %>").val()) + "&login_id=" + encodeURIComponent($("#<%=div_txt_LoginID.ClientID %>").val()) + "&login_pwd=" + encodeURIComponent($("#<%=div_txt_LoginPWD.ClientID %>").val()) + "&level=" + level + "&qrcode_count=" + qrcode_count,
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //정확히 등록되였습니다.
                        alert("<%=Resources.Lang.MSG_ACCOUNT_REG_SUCCESS %>");
                        document.location.href = "/Manager/AccountList.aspx";
                        return;
                    }
                    else if (data == "1") {
                        //이미 등록된 아이디입니다.
                        alert("<%=Resources.Lang.MSG_EXIST_LOGINID %>");
                        return;
                    }
                    else if (data == "2") {
                        //등록자명이 중복되었습니다.
                        alert("<%=Resources.Lang.MSG_EXIST_NICKNAME %>");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //계정등록중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_ACCOUNT_REG_ERROR %>");
                    return;
                }
            });
        }

        function showModifyAccountPopup(nickname, login_id, login_pwd, level, qrcode_count) {
            $("#div_lbl_Nickname").html(nickname);
            $("#div_lbl_LoginID").html(login_id);
            $("#<%=div_LoginPWD.ClientID %>").val(login_pwd);
            $("#<%=div_LoginConfirmPWD.ClientID %>").val(login_pwd);
            $("#sel_level option[value=" + level + "]").prop("selected", true);
            $("#sel_qrcode_count option[value=" + qrcode_count + "]").prop("selected", true);
            showPopup("divModifyAccount");
        }

        function ModifyAccount() {
            if ($("#<%=div_LoginPWD.ClientID %>").val() == "") {
                //변경할 패스워드를 입력하십시오.
                alert("<%=Resources.Lang.MSG_INPUT_CHANGE_PASSWORD %>");
                return;
            }

            if ($("#<%=div_LoginConfirmPWD.ClientID %>").val() == "") {
                //확인 패스워드를 입력하십시오.
                alert("<%=Resources.Lang.MSG_INPUT_PASSWORD_CONFIRM %>");
                return;
            }

            if ($("#<%=div_LoginPWD.ClientID %>").val() != $("#<%=div_LoginConfirmPWD.ClientID %>").val()) {
                //입력하신 패스워드가 일치하지 않습니다.
                alert("<%=Resources.Lang.MSG_INPUT_PASSWORD_ERROR %>");
                return;
            }

            var level = $("#sel_level option:selected").val();
            var qrcode_count = $("#sel_qrcode_count option:selected").val();

            $.ajax({
                url: "ChangeAccount.aspx?login_id=" + encodeURIComponent($("#div_lbl_LoginID").html()) + "&login_pwd=" + encodeURIComponent($("#<%=div_LoginPWD.ClientID %>").val()) + "&level=" + level + "&qrcode_count=" + qrcode_count,
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //계정정보를 정확히 변경하였습니다.
                        alert("<%=Resources.Lang.MSG_ACCOUNT_CHANGE_SUCCESS %>");
                        document.location.href = "/Manager/AccountList.aspx";
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //계정정보를 변경하던중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_ACCOUNT_CHANGE_ERROR %>");
                    return;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                <table cellpadding="0" cellspacing="0" width="1085" align="center">
                    <tr>
                        <td width="289" height="39"></td>
                        <td width="796" height="39">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="289" height="45">
                            <span style="font-size:14pt; color:#BA4241; font-weight:bold;"><%=Resources.Lang.STR_ACCOUNT_LIST%></span>
                        </td>
                        <td width="796" height="45">
                            <div align="right">
                                <table cellpadding="0" cellspacing="0" width="158">
                                    <tr>
                                        <td width="158" height="38" align="right">
                                            <input type="button" class="btnSmallGreen" onclick="showAddAccountPopup()" value="<%=Resources.Lang.STR_ACCOUNT_ADD %>" style="width:86px; height:22px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvContent" runat="server" AllowSorting="false" Width="1085" GridLines="None" AutoGenerateColumns="false"
                    OnRowDataBound="gvContent_RowDataBound" OnPageIndexChanging="gvContent_PageIndexChange" CssClass="clsGrid" HorizontalAlign="Center" AllowPaging="True">
                    <Columns>
                        <asp:TemplateField HeaderText="No" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#ffffff">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="73" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_NICKNAME %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#ffffff">
                            <ItemTemplate>
                                <asp:Literal ID="ltrNickname" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="170" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_LOGINID %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#ffffff">
                            <ItemTemplate>
                                <asp:Literal ID="ltrLoginID" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="165" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_ACCOUNT_LEVEL %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#ffffff">
                            <ItemTemplate>
                                <asp:Literal ID="ltrLevel" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="201" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_QRCODE_COUNT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#ffffff">
                            <ItemTemplate>
                                <asp:Literal ID="ltrSerialCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="157" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_CHANGE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#ffffff">
                            <ItemTemplate>
                                <asp:Literal ID="ltrModify" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="156" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="clsGrid" Height="40px" />
                    <HeaderStyle CssClass="clsGridHeader" Height="30px"/>
                    <SelectedRowStyle CssClass="clsSelGrid" Height="30px" />
                    <AlternatingRowStyle CssClass="clsSelGrid" />
                    <EmptyDataRowStyle VerticalAlign="Middle" />
                    <EmptyDataTemplate>
                        <table width="100%" border="0">
                        <tr>
                            <td class="clsemptyrow" style="height: 350px;">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Str, STR_NODATA %>"></asp:Literal>
                            </td>
                        </tr>
                        </table>
                    </EmptyDataTemplate>
                    <PagerSettings Mode="Numeric" Position="Bottom"
                        FirstPageText="<%$Resources:Str, STR_FIRST %>"
                        PreviousPageText="<%$Resources:Str, STR_PREV %>"
                        NextPageText="<%$Resources:Str, STR_NEXT %>"
                        LastPageText="&nbsp;<%$Resources:Str, STR_LAST %>"
                        PageButtonCount="10" />
                    <PagerStyle CssClass="clspager" HorizontalAlign="Center" />
                </asp:GridView>
            </td>
        </tr>
    </table>
    <div class="clspopup" id="divAddAccount">
        <table cellpadding="0" cellspacing="0" width="687" background="/img/pop_bg2.png">
            <tr>
                <td width="687" height="405" valign="top">
                    <table cellpadding="0" cellspacing="0" width="655" align="center">
                        <tr>
                            <td width="655" height="71">
                                <table cellpadding="0" cellspacing="0" width="624" align="center">
                                    <tr>
                                        <td width="416" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_ACCOUNT_ADD %></span>
                                        </td>
                                        <td width="208" height="31" align="right">
                                            <img src="/img/bt_close.png" width="22" height="22" border="0" alt="" onclick="closePopup()" style="cursor:pointer;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="655" height="321" valign="top">
                                <table cellpadding="0" cellspacing="0" align="center" width="497">
                                    <tr>
                                        <td width="167" height="38">                                    
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_NICKNAME %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <font color="#666666"><asp:TextBox ID="div_txt_Nickname" runat="server" Text="" Width="320"></asp:TextBox></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_LOGINID %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <font color="#666666"><asp:TextBox ID="div_txt_LoginID" runat="server" Text="" Width="320"></asp:TextBox></font>                                 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_PASSWORD %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <font color="#666666"><asp:TextBox ID="div_txt_LoginPWD" TextMode="Password" runat="server" Text="" Width="320"></asp:TextBox></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_PASSWORD_CONFIRM %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <font color="#666666"><asp:TextBox ID="div_txt_LoginConfirmPWD" TextMode="Password" runat="server" Text="" Width="320"></asp:TextBox></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_ACCOUNT_LEVEL %></span>
                                        </td>
                                        <td width="330" height="38" align="left">
                                            <font color="#666666">
                                                <select id="select_level" size="1" style="font-size:10pt; color:rgb(51,51,51); border-width:1px;">
                                                    <option value="0" selected="selected"><%=Resources.Lang.STR_LICENSER %></option>
                                                    <option value="1"><%=Resources.Lang.STR_ADMINISTRATOR %></option>
                                                </select>
                                            </font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_QRCODE_COUNT %></span>
                                        </td>
                                        <td width="330" height="38" align="left">
                                            <font color="#666666">
                                                <select id="select_qrcode_count" size="1" style="font-size:10pt; color:rgb(51,51,51); border-width:1px;">
                                                    <option value="1">1<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="10">10<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="100">100<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="200">200<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="500" selected="selected">500<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="1000">1000<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="2000">2000<%=Resources.Lang.STR_GAE %></option>
                                                </select>
                                            </font>
								        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="290" align="center">
                                    <tr>
                                        <td width="290" height="80" align="center">
                                            <input type="button" class="btnMiddle" onclick="AddAccount()" value="<%=Resources.Lang.STR_ADD %>" style="width:110px; height:32px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="clspopup" id="divModifyAccount">
        <table cellpadding="0" cellspacing="0" width="687" background="/img/pop_bg2.png">
            <tr>
                <td width="687" height="405" valign="top">
                    <table cellpadding="0" cellspacing="0" width="655" align="center">
                        <tr>
                            <td width="655" height="71">
                                <table cellpadding="0" cellspacing="0" width="624" align="center">
                                    <tr>
                                        <td width="416" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_ACCOUNT_CHANGE %></span>
                                        </td>
                                        <td width="208" height="31" align="right">
                                            <img src="/img/bt_close.png" width="22" height="22" border="0" alt="" onclick="closePopup()" style="cursor:pointer;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="655" height="321" valign="top">
                                <table cellpadding="0" cellspacing="0" align="center" width="497">
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_NICKNAME %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <span style="font-size:10pt; color:#666666;"><label id="div_lbl_Nickname"></label></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_LOGINID%></span>
                                        </td>
                                        <td width="330" height="38">
                                            <span style="font-size:10pt; color:#666666;"><label id="div_lbl_LoginID"></label></span>                                  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_PASSWORD %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <font color="#666666"><asp:TextBox ID="div_LoginPWD" TextMode="Password" runat="server" Text="" Width="320"></asp:TextBox></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_PASSWORD_CONFIRM %></span>
                                        </td>
                                        <td width="330" height="38">
                                            <font color="#666666"><asp:TextBox ID="div_LoginConfirmPWD" TextMode="Password" runat="server" Text="" Width="320"></asp:TextBox></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_ACCOUNT_LEVEL %></span>
                                        </td>
                                        <td width="330" height="38" align="left">
                                            <font color="#666666">
                                                <select id="sel_level" size="1" style="font-size:11pt; color:rgb(51,51,51); border-width:1px;">
                                                    <option value="0"><%=Resources.Lang.STR_LICENSER %></option>
                                                    <option value="1"><%=Resources.Lang.STR_ADMINISTRATOR %></option>
                                                </select>
                                            </font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="167" height="38">
                                            <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_QRCODE_COUNT %></span>
                                        </td>
                                        <td width="330" height="38" align="left">
                                            <font color="#666666">
                                                <select id="sel_qrcode_count" size="1" style="font-size:10pt; color:rgb(51,51,51); border-width:1px;">
                                                    <option value="1">1<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="10">10<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="100">100<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="200">200<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="500" selected="selected">500<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="1000">1000<%=Resources.Lang.STR_GAE %></option>
                                                    <option value="2000">2000<%=Resources.Lang.STR_GAE %></option>
                                                </select>
                                            </font>
								        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="290" align="center">
                                    <tr>
                                        <td width="290" height="80" align="center">
                                            <input type="button" class="btnMiddle" onclick="ModifyAccount()" value="<%=Resources.Lang.STR_CHANGE %>" style="width:110px; height:32px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
