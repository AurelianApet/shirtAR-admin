<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="PushAlarm.aspx.cs" Inherits="Web.Manager.PushAlarm" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(2);
            setSubMenu(2);

            $("#<%=tbxDate.ClientID%>").attr("disabled", true);
            $("#<%=ddlHour.ClientID%>").attr("disabled", true);
            $("#<%=ddlMinute.ClientID%>").attr("disabled", true);

            $('input[type=radio][name=optType]').change(function () {
                if (this.value == '1') {
                    $("#<%=tbxDate.ClientID%>").attr("disabled", true);
                    $("#<%=ddlHour.ClientID%>").attr("disabled", true);
                    $("#<%=ddlMinute.ClientID%>").attr("disabled", true);
                }
                else if (this.value == '2') {
                    $("#<%=tbxDate.ClientID%>").attr("disabled", false);
                    $("#<%=ddlHour.ClientID%>").attr("disabled", false);
                    $("#<%=ddlMinute.ClientID%>").attr("disabled", false);
                }
            });

        });

        function pushSend() {
            if ($("#<%=tbxMsgTitle.ClientID%>").val() == '') {
                //메시지 타이틀을 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_MESSAGE_TITLE %>");
                return false;
            }

            if ($("#<%=tbxMsgContent.ClientID%>").val() == '') {
                //메시지 내용을 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_MESSAGE_CONTENT %>");
                return false;
            }

            if ($("input:radio[name='optType']:checked").val() == '2') {
                if ($("#<%=tbxDate.ClientID %>").val() == '') {
                    //전송일시를 정확히 입력해주세요.
                    alert("<%=Resources.Lang.MSG_INPUT_SEND_DATE %>");
                    return false;
                }
            }

            return true;
        }

        function checkCalendar() {
            $("#<%=tbxDate.ClientID %>").attr("readonly", true);
            showCal('<%=tbxDate.ClientID %>');
        }

        function OnBrowse(obj) {
            $("#" + obj).click();
        }

        function showSendPopup() {
            $("#<%=tbxMsgContent.ClientID%>").val("");
            $("#<%=tbxMsgContent.ClientID%>").attr("maxlength", 50);
            showPopup("divSendPopup");
        }

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
                            <td width="206" height="60" align="center">
                                <input id="subMenu1" type="button" class="btnSubMenu" onclick="window.location='/Manager/DataManager.aspx';" value="<%=Resources.Lang.STR_DATA_MANAGE %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="206" height="60" align="center">
                                <input id="subMenu2" type="button" class="btnSubMenu" onclick="window.location='/Manager/PushAlarm.aspx';" value="<%=Resources.Lang.STR_PUSH_ALARM %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="206" height="60" align="center">
                                <input id="subMenu3" type="button" class="btnSubMenu" onclick="window.location='/Manager/Banner.aspx';" value="<%=Resources.Lang.STR_BANNER %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="206" height="60" align="center">
                                <input id="subMenu4" type="button" class="btnSubMenu" onclick="window.location='/Manager/APPUpdate.aspx';" value="<%=Resources.Lang.STR_APP_UPDATE %>"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td width="1193" height="850" bgcolor="whitesmoke" style="border-width:1px; border-top-color:black; border-right-color:rgb(232,232,230); border-bottom-color:black; border-left-color:rgb(232,232,230); border-top-style:none; border-right-style:solid; border-bottom-style:none; border-left-style:solid;" valign="top">
                <table cellpadding="0" cellspacing="0" width="1085" align="center">
                    <tr>
                        <td width="1085" height="39">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="289" height="45">
                            <span style="font-size:14pt; font-weight:bold; color:#BA4241;"><%=Resources.Lang.STR_PUSH_LIST %></span>
                        </td>
                        <td width="796" height="45">
                            <div align="right">
                                <table cellpadding="0" cellspacing="0" width="95">
                                    <tr>
                                        <td width="95" height="38">
                                            <input type="button" class="btnMiddle" onclick="showSendPopup()" value="<%=Resources.Lang.STR_SEND %>" style="width:88px; height:27px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvContent" runat="server" AllowSorting="false" Width="1095" GridLines="None" AutoGenerateColumns="false"
                    OnRowDataBound="gvContent_RowDataBound" OnPageIndexChanging="gvContent_PageIndexChange" CssClass="clsGrid" HorizontalAlign="Center" AllowPaging="True">
                    <Columns>
                        <asp:TemplateField HeaderText="No" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_SEND_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_MESSAGE_TITLE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrMsgTitle" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_MESSAGE_CONTENT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrMsgContent" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="450" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_ALL_DEVICE_NUMBER %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrTotalCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_SENDED_DEVICE_NUMBER %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrSendCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="120" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_STATUS %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrStatus" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="clsGrid" Height="30px" />
                    <SelectedRowStyle CssClass="clsSelGrid" Height="30px" />
                    <AlternatingRowStyle CssClass="clsSelGrid" />
                    <HeaderStyle CssClass="clsGridHeader" Height="45px"/>
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
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom"
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
    <div class="clspopup" id="divSendPopup">
        <table cellpadding="0" cellspacing="0" width="790" background="/img/pop_bg5.png">
            <tr>
                <td width="790" height="293" valign="top">
                    <table cellpadding="0" cellspacing="0" width="762" align="center">
                        <tr>
                            <td width="760" height="71">
                                <table cellpadding="0" cellspacing="0" width="740" align="center">
                                    <tr>
                                        <td width="400" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_PUSH_SEND %></span>
                                        </td>
                                        <td width="320" height="31" align="right">
                                            <img onclick="closePopup()" src="/img/bt_close.png" width="22" height="22" border="0" alt="" style="cursor:pointer;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="760" height="205" valign="top">
                                <table cellpadding="0" cellspacing="0" align="center" width="717">
                                    <tr>
                                        <td width="116" height="50"><span style="font-size:11pt; font-weight:bold; color:#666666;"><%=Resources.Lang.STR_MESSAGE_TITLE%><font color="red">*&nbsp;</font></span></td>
                                        <td width="601" height="50">
                                            <table cellpadding="0" cellspacing="0" width="597">
                                                <tr>
                                                    <td width="597" height="32" align="left">
                                                        <font color="#666666"><asp:TextBox ID="tbxMsgTitle" runat="server" MaxLength="50" style="text-align:left; width:590px;" placeholder="<%$Resources:Lang, STR_LIMIT_50 %>"></asp:TextBox></font>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="116" height="100"><span style="font-size:11pt; font-weight:bold; color:#666666;"><%=Resources.Lang.STR_MESSAGE_CONTENT%><font color="red">*&nbsp;</font></span></td>
                                        <td width="601" height="100">
                                            <table cellpadding="0" cellspacing="0" width="597">
                                                <tr>
                                                    <td width="597" height="32" align="left">
                                                        <font color="#666666"><asp:TextBox ID="tbxMsgContent" TextMode="multiline" Rows="5" runat="server" MaxLength="200" style="text-align:left; width:590px;" placeholder="<%$Resources:Lang, STR_LIMIT_200 %>"></asp:TextBox></font>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="116" height="80">
                                            <span style="font-size:11pt; font-weight:bold; color:#666666;"><%=Resources.Lang.STR_SEND_DATE%></span>
                                        </td>
                                        <td width="601" height="80">
                                            <table cellpadding="0" cellspacing="0" width="597" align="center">
                                                <tr>
                                                    <td width="116" height="51">
                                                        <span style="font-size:11pt;">
                                                            <font color="#666666">
                                                                <input type="radio" name="optType" value="1" checked="checked"/><%=Resources.Lang.STR_FAST_SEND%>
                                                            </font>
                                                        </span>
                                                    </td>
                                                    <td width="116" height="51">
                                                        <span style="font-size:11pt;">
                                                            <font color="#666666">
                                                                <input type="radio" name="optType" value="2" /><%=Resources.Lang.STR_SLOW_SEND%>
                                                            </font>
                                                        </span>
                                                    </td>
                                                    <td width="91" height="51" align="right">
                                                        <span style="font-size:11pt;"><font color="#666666"><a onmouseout="na_restore_img_src('pop_bt_calen')" onmouseover="na_change_img_src('pop_bt_calen', '/img/pop_bt_calen_r.png')"><img name="pop_bt_calen" src="/img/pop_bt_calen.png" width="18" height="20" border="0" alt="" style="cursor:pointer;"/></a></font></span>
                                                    </td>
                                                    <td width="95" height="51" align="center">
                                                        <span style="font-size:11pt; color:#666666;">
                                                            <asp:TextBox ID="tbxDate" runat="server" Text="" size="10" onclick="checkCalendar()" style="font-size:11pt; color:rgb(102,102,102); cursor:pointer;"></asp:TextBox>
                                                        </span>
                                                    </td>
                                                    <td width="69" height="51" align="right">
                                                        <asp:DropDownList ID="ddlHour" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td width="24" height="51" align="right">
                                                        <span style="font-size:11pt; color:#666666;"><%=Resources.Lang.STR_HOUR%></span>
                                                    </td>
                                                    <td width="64" height="51" align="right">
                                                        <asp:DropDownList ID="ddlMinute" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td width="22" height="51" align="right">
                                                        <span style="font-size:11pt; color:#666666;"><%=Resources.Lang.STR_MINUTE%></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="301" align="center">
                                    <tr>
                                        <td width="870" height="100">
                                            <table cellpadding="0" cellspacing="0" width="301" align="center">
                                                <tr>
                                                    <td width="141" height="56" align="center">
                                                        <asp:Button ID="bt_send" runat="server" OnClick="btnSend_Click" OnClientClick="return pushSend();" CssClass="btnMiddle" width="110" height="32" Text="<%$Resources:Lang, STR_SEND %>" />
                                                    </td>
                                                    <td width="131" height="56" align="center">
                                                        <input type="button" class="btnMiddle" onclick="closePopup()" value="<%=Resources.Lang.STR_CANCEL %>" style="width:110px; height:32px;"/>
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
    </div>
</asp:Content>
