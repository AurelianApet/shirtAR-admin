<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="Web.Manager.Setting" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(3);
            setSubMenu(3);

            $("#sel_connect_cycle option[value=" + $("#<%=connect_cycle.ClientID %>").val() + "]").attr('selected', true);
            $("#sel_count_per_page option[value=" + $("#<%=count_per_page.ClientID %>").val() + "]").attr('selected', true);
            $("#sel_lang option[value=" + $("#<%=lang.ClientID %>").val() + "]").attr('selected', true);
        });

        function Setting() {
            var data = new FormData();

            var connect_cycle = $("#sel_connect_cycle option:selected").val();
            var count_per_page = $("#sel_count_per_page option:selected").val();
            var lang = $("#sel_lang option:selected").val();

            data.append("connect_cycle", encodeURIComponent(connect_cycle));
            data.append("count_per_page", encodeURIComponent(count_per_page));
            data.append("lang", encodeURIComponent(lang));

            $.ajax({
                url: 'ChangeConfig.aspx',
                type: "post",
                data: data,
                processData: false,
                contentType: false,
                success: function (data, textStatus, jqXHR) {
                    if (data == "0") {
                        alert("<%=Resources.Lang.MSG_CONFIG_SET_SUCCESS %>");
                        location.reload(true);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //환경설정과정에 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_CONFIG_SET_ERROR %>");
                    return;
                }
            }); 
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="connect_cycle" runat="server" Value="5"/>
    <asp:HiddenField ID="count_per_page" runat="server" Value="20"/>
    <asp:HiddenField ID="lang" runat="server" Value="1"/>
    
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
                <table cellpadding="0" cellspacing="0" width="500" align="center">
                    <tr>
                        <td width="500" height="50">
                        </td>
                    </tr>
                    <tr>
                        <td width="500" height="100" align="center">                                                
                            <span style="font-size:16pt; color:#BA4241; font-weight:bold;"><%=Resources.Lang.STR_ENV_SETTING%></span>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" align="center" width="500">
                    <tr>
                        <td width="100" height="50"></td>
                        <td width="150" height="50">                                    
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_SERVER_CONNECT_CYCLE%></span>
                        </td>
                        <td width="150" height="50" align="left">
                            <font color="#666666">
                                <select id="sel_connect_cycle" size="1" style="font-size:10pt; color:rgb(51,51,51); border-width:1px; width:100px;">
                                    <option value="1">1<%=Resources.Lang.STR_DAY%></option>
                                    <option value="2">2<%=Resources.Lang.STR_DAY%></option>
                                    <option value="3">3<%=Resources.Lang.STR_DAY%></option>
                                    <option value="4">4<%=Resources.Lang.STR_DAY%></option>
                                    <option value="5">5<%=Resources.Lang.STR_DAY%></option>
                                    <option value="7">7<%=Resources.Lang.STR_DAY%></option>
                                    <option value="15">15<%=Resources.Lang.STR_DAY%></option>
                                    <option value="30">30<%=Resources.Lang.STR_DAY%></option>
                                </select>
                            </font>
						</td>
                        <td width="100" height="50"></td>
                    </tr>
                    <tr>
                        <td width="100" height="50"></td>
                        <td width="150" height="50">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_COUNT_PER_PAGE%></span>
                        </td>
                        <td width="150" height="50">
                            <font color="#666666">
                                <select id="sel_count_per_page" size="1" style="font-size:10pt; color:rgb(51,51,51); border-width:1px; width:100px;">
                                    <option value="5">5<%=Resources.Lang.STR_GAE%></option>
                                    <option value="10">10<%=Resources.Lang.STR_GAE%></option>
                                    <option value="20">20<%=Resources.Lang.STR_GAE%></option>
                                    <option value="50">50<%=Resources.Lang.STR_GAE%></option>
                                    <option value="100">100<%=Resources.Lang.STR_GAE%></option>
                                    <option value="200">200<%=Resources.Lang.STR_GAE%></option>
                                    <option value="500">500<%=Resources.Lang.STR_GAE%></option>
                                    <option value="1000">1000<%=Resources.Lang.STR_GAE%></option>
                                </select>
                            </font>
                        </td>
                        <td width="100" height="50"></td>
                    </tr>
                    <tr>
                        <td width="100" height="50"></td>
                        <td width="150" height="50">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_LANG_SETTING%></span>
                        </td>
                        <td width="150" height="50">
                            <font color="#666666">
                                <select id="sel_lang" size="1" style="font-size:10pt; color:rgb(51,51,51); border-width:1px; width:100px;">
                                    <option value="1"><%=Resources.Lang.STR_KOREAN%></option>
                                    <option value="2"><%=Resources.Lang.STR_CHINESE%></option>
                                </select>
                            </font>
                        </td>
                        <td width="100" height="50"></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="500" align="center">
                    <tr>
                        <td width="300" height="100" align="center">
                            <input type="button" class="btnMiddle" onclick="Setting()" value="<%=Resources.Lang.STR_SETTING %>" style="width:110px; height:32px;"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
