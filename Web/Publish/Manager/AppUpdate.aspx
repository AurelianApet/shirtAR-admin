<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="AppUpdate.aspx.cs" Inherits="Web.Manager.AppUpdate" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(2);
            setSubMenu(4);
        });

        function Check() {
            if ($("#<%=txtUpdateVersion.ClientID%>").val() == '') {
                //업데이트버전을 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_UPDATE_VERSION %>");
                return false;
            }

            if ($("#<%=txtUpdateMemo.ClientID%>").val() == '') {
                //업데이트내용를 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_UPDATE_MEMO %>");
                return false;
            }

            return true;
        }

        function OnBrowse(obj) {
            $("#" + obj).click();
        }

        function showDelPopup(id) {
            $("#hd_id").val(id);
            showPopup("divDelConfirmAlert");
        }

        function DelUpdateInfo() {
            $.ajax({
                url: "DelUpdateInfo.aspx?id=" + encodeURIComponent($("#hd_id").val()),
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        location.reload(true);
                        return;
                    }
                    else if (data == "1") {
                        //업데이트정보삭제중 예상치않은 오류가 발생하였습니다.
                        alert("<%=Resources.Lang.MSG_UPDATE_INFO_DELETE_ERROR %>");
                        return;                            
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //업데이트정보삭제중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_UPDATE_INFO_DELETE_ERROR %>");
                    return;
                }
            });
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
                <table cellpadding="0" cellspacing="0" width="1000" align="center">
                    <tr>
                        <td width="1100" height="40">
                        </td>
                    </tr>
                    <tr>
                        <td width="1100" height="40" align="center">
                            <span style="font-size:16pt; font-weight:bold; color:#BA4241;"><%=Resources.Lang.STR_UPDATE_INFO%></span>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="1000" align="center">
                    <input id="hd_id" type="hidden" value=""/>
                    <tr>
                        <td width="80" height="50" align="center">
                            <span style="font-size:11pt; font-weight:bold;"><%=Resources.Lang.STR_UPDATE_VERSION %></span>
                        </td>
                        <td width="200" height="50">
                            <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                <tr>
                                    <td width="200" height="25" align="center">          
                                        <asp:TextBox ID="txtUpdateVersion" runat="server" TextMode="SingleLine" Width="150" MaxLength="10" style="border-width:1px; border-style:none;" placeholder="<%$Resources:Lang, STR_LIMIT_10 %>"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="80" height="50" align="center">
                            <span style="font-size:11pt; font-weight:bold;"><%=Resources.Lang.STR_MEMO %></span>
                        </td>
                        <td width="500" height="50">
                            <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                <tr>
                                    <td width="500" height="25" align="center">          
                                        <asp:TextBox ID="txtUpdateMemo" runat="server" TextMode="SingleLine" Width="450" MaxLength="50" style="border-width:1px; border-style:none;" placeholder="<%$Resources:Lang, STR_LIMIT_50 %>"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="100" height="50" align="right">
                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" OnClientClick="Check()" CssClass="btnSmallGreen" width="110" height="32" Text="<%$Resources:Lang, STR_ADD %>" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvContent" runat="server" AllowSorting="false" Width="1100" GridLines="None" AutoGenerateColumns="false"
                    OnRowDataBound="gvContent_RowDataBound" OnPageIndexChanging="gvContent_PageIndexChange" CssClass="clsGrid" HorizontalAlign="Center" AllowPaging="True">
                    <Columns>
                        <asp:TemplateField HeaderText="No" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_REG_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_UPDATE_VERSION %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrUpdateVersion" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="180" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_MEMO %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrUpdateMemo" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="320" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DELETE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDelete" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="clsGridHeader" Height="40px"/>
                    <RowStyle CssClass="clsGrid" Height="30px" />
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
                <table cellpadding="0" cellspacing="0" width="1100" align="center">
                    <tr>
                        <td width="600" height="50"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="clspopup" id="divDelConfirmAlert">
        <table cellpadding="0" cellspacing="0" width="560" background="/img/pop_bg3.png">
            <tr>
                <td width="560" height="337" valign="top">
                    <table cellpadding="0" cellspacing="0" width="470" align="center">
                        <tr>
                            <td width="470" height="71">
                                <table cellpadding="0" cellspacing="0" width="477" align="center">
                                    <tr>
                                        <td width="306" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_DELETE_CONFIRM %></span>
                                        </td>
                                        <td width="171" height="31" align="right">
                                            <img onclick="closePopup()" src="/img/bt_close.png" width="22" height="22" border="0" alt="" style="cursor:pointer;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="470" height="241">
                                <table cellpadding="0" cellspacing="0" align="center" width="409">
                                    <tr>
                                        <td width="409" height="166">                                    
                                            <span style="font-size:11pt; color:#666666;"><%=Resources.Lang.MSG_DELETE_REALLY %></span><br />
                                            <span style="font-size:11pt; color:#666666;"><%=Resources.Lang.MSG_CONFIRM_DESCRIPTION %></span>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="283" align="center">
                                    <tr>
                                        <td width="141" height="63" align="center">
                                            <input type="button" class="btnMiddle" onclick="DelUpdateInfo()" value="<%=Resources.Lang.STR_YES %>" style="width:110px; height:32px;"/>
                                        </td>
                                        <td width="142" height="63" align="center">
                                            <input type="button" class="btnMiddle" onclick="closePopup()" value="<%=Resources.Lang.STR_NO %>" style="width:110px; height:32px;"/>
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
