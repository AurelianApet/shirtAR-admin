<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="LogManager.aspx.cs" Inherits="Web.Manager.LogManager" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(3);
            setSubMenu(4);
            $('input[type=radio][name=optSaveExcelType]').change(function () {
                $("#<%=hd_saveExcelType.ClientID %>").val(this.value);
            });
        });

        function img_src(path) {
            $("#<%= bt_search.ClientID %>").attr("src", path);
        }

        function excel_img_src(path) {
            $("#<%= bt_save_excel.ClientID %>").attr("src", path);
        }

        function showSaveExcelPopup() {
            $("#<%=hd_saveExcelType.ClientID %>").val($('input[type=radio][name=optSaveExcelType]:checked').val());
            showPopup("divSaveExcelAlert");
        }

        function saveExcel() {
            closePopup();
            return true;
        }

        function Search() {
            $("#<%=hd_search_type.ClientID %>").val($("#<%=select_search_type.ClientID %> option:selected").val());
            if ($("#<%=txt_searchkey.ClientID %>").val() == "") {
                //검색문자열을 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_SEARCH_STRING %>");
                return false;
            }
            return true;
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
                            <span style="font-size:14pt; font-weight:bold; color:#BA4241;"><%=Resources.Lang.MSG_LOG_HISTORY %></span>
                        </td>
                        <td width="796" height="45">
                            <div align="right">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="90" height="38">
                                            <table cellpadding="0" cellspacing="0" width="90">
                                                <tr>
                                                    <td width="90" height="32" align="center">
                                                        <font color="#67707E">
                                                            <asp:HiddenField ID="hd_search_type" runat="server" Value="0" />
                                                            <asp:DropDownList ID="select_search_type" runat="server" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; border-style:solid;">
                                                            </asp:DropDownList>
                                                        </font>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td height="38">
                                            <asp:TextBox ID="txt_searchkey" runat="server" Text="" style="font-size:11pt; color:rgb(102,102,102); width:100px;"></asp:TextBox>
                                        </td>
                                        <td width="60" height="38" align="right">
                                            <asp:Button ID="bt_search" runat="server" OnClientClick="return Search();" OnClick="btnSearch_Click" CssClass="btnSmallGray" width="58" height="23" Text="<%$Resources:Lang, STR_SEARCH %>" />
                                        </td>
                                        <td width="110" height="38" align="right">
                                            <input type="button" class="btnSmallGreen" onclick="showSaveExcelPopup()" value="<%=Resources.Lang.STR_SAVE_EXCEL %>" style="width:108px; height:23px;"/>
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
                        <asp:TemplateField HeaderText="No" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="73" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_REG_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="163" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_NICKNAME %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrNickname" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="132" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_LOGINID %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrLoginID" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="147" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_EVENT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrEvent" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="280" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_QRCODE_COUNT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrSerialCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="134" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_PRODUCT_TYPE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrProductType" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="156" Wrap="false" BorderStyle="None"/>
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
                <table cellpadding="0" cellspacing="0" width="1085" align="center">
                    <tr>
                        <td height="40"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="clspopup" id="divSaveExcelAlert">
        <table cellpadding="0" cellspacing="0" width="560" background="/img/pop_bg3.png">
            <tr>
                <td width="560" height="337" valign="top">
                    <table cellpadding="0" cellspacing="0" width="470" align="center">
                        <tr>
                            <td width="470" height="71">
                                <table cellpadding="0" cellspacing="0" width="477" align="center">
                                    <tr>
                                        <td width="306" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_SAVE_EXCEL %></span>
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
                                <asp:HiddenField ID="hd_saveExcelType" runat="server" Value="1" />
                                <table cellpadding="0" cellspacing="0" align="center" width="409">
                                    <tr>
                                        <td width="100" height="50">
                                        </td>
                                        <td width="309" height="50">
                                            <span style="font-size:11pt; color:#FF0000;">
                                                <input type="radio" name="optSaveExcelType" value="1" checked="checked" /><label><%=Resources.Lang.MSG_SAVE_CURRENT_LIST %></label> 
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" height="70">
                                        </td>
                                        <td width="309" height="70">
                                            <span style="font-size:11pt; color:#FF0000;">
                                                <input type="radio" name="optSaveExcelType" value="2" /><label><%=Resources.Lang.MSG_SAVE_ALL_LIST %></label>
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="283" align="center">
                                    <tr>
                                        <td width="141" height="63" align="center">
                                            <asp:Button ID="bt_save_excel" runat="server" OnClick="btnExcel_Click" OnClientClick="return saveExcel();" CssClass="btnMiddle" width="110" height="32" Text="<%$Resources:Lang, STR_CONFIRM %>" />
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
