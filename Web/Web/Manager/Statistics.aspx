<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Web.Manager.Statistics" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript" src="/Scripts/highcharts/highcharts.js"></script>
    <script language="javascript" type="text/javascript" src="/Scripts/highcharts/modules/data.js"></script>
    <script language="javascript" type="text/javascript" src="/Scripts/highcharts/modules/exporting.js"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(1);
            setSubMenu(4);

            var vn_date_today = new Date($("#<%=hd_curdate.ClientID %>").val());
            var vn_year = vn_date_today.getFullYear();
            var vn_month = vn_date_today.getMonth() + 1;
            var vn_day = vn_date_today.getDate();
            vn_date_select = new Date(vn_year, vn_month, 1);
            vn_date_select_next = new Date(vn_year, vn_month + 1, 1);
            var vn_count_day = (vn_date_select_next - vn_date_select) / 1000 / 60 / 60 / 24;

            var vn_html = "<select id=\"select_year\" onchange=\"changeDate()\">";
            for (var i = vn_year - 1; i <= vn_year + 1; i++) {
                if (vn_year == i) { vn_html += '<option value=' + i + ' selected>' + i + '</option>'; }
                else { vn_html += '<option value=' + i + '>' + i + '</option>'; }
            }
            vn_html += "</select> <%=Resources.Lang.STR_YEAR%> ";
            vn_html += "<select id=\"select_month\" onchange=\"changeDate()\">";
            for (var i = 1; i <= 12; i++) {
                if (vn_month == i) { vn_html += '<option value=' + i + ' selected>' + i + '</option>'; }
                else { vn_html += '<option value=' + i + '>' + i + '</option>'; }
            }
            vn_html += "</select> <%=Resources.Lang.STR_MONTH%> ";
            vn_html += "<select id=\"select_day\" onchange=\"changeDate()\">";
            for (var i = 1; i <= 31; i++) {
                if (vn_day == i) { vn_html += '<option value=' + i + ' selected>' + i + '</option>'; }
                else { vn_html += '<option value=' + i + '>' + i + '</option>'; }
            }
            vn_html += "</select> <%=Resources.Lang.STR_DAY%> ";

            $("#div_select").html(vn_html);

            setSelectObject($("#<%=hd_type.ClientID %>").val());
            $('input:radio[name=chk][value=' + $("#<%=hd_type.ClientID %>").val() + ']').attr('checked', true);

            drawGraph();
        });

        function setSelectObject(type) {
            $("#<%=hd_type.ClientID %>").val(type);
            if (type == 1) {
                $("#select_month").attr("disabled", true);
                $("#select_day").attr("disabled", true);
            } else if (type == 2) {
                $("#select_month").attr("disabled", false);
                $("#select_day").attr("disabled", true);
            } else if (type == 3) {
                $("#select_month").attr("disabled", false);
                $("#select_day").attr("disabled", false);
            }
        }

        function changeDate() {
            var vn_date = $("#select_year option:selected").val() + "-" + $("#select_month option:selected").val() + "-" + $("#select_day option:selected").val();
            $("#<%=hd_curdate.ClientID %>").val(vn_date);
        }

        function drawGraph() {
            $('#divStatGraph').highcharts({
                data: {
                    table: 'tblStatData'
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.name + '</b>' + ' ' + this.point.y;
                    }
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
                            <td width="206" height="60" align="center">
                                <input id="subMenu1" type="button" class="btnSubMenu" onclick="window.location='/Manager/QRCodeCreate.aspx';" value="<%=Resources.Lang.STR_QRCODE_CREATE %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="206" height="60" align="center">
                                <input id="subMenu2" type="button" class="btnSubMenu" onclick="window.location='/Manager/CreatedQRCodeList.aspx';" value="<%=Resources.Lang.STR_CREATED_QRCODE_LIST %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="206" height="60" align="center">
                                <input id="subMenu3" type="button" class="btnSubMenu" onclick="window.location='/Manager/AuthedQRCodeList.aspx';" value="<%=Resources.Lang.STR_AUTHED_QRCODE_LIST %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td width="206" height="60" align="center">
                                <input id="subMenu4" type="button" class="btnSubMenu" onclick="window.location='/Manager/Statistics.aspx';" value="<%=Resources.Lang.STR_STATISTICS %>"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td width="1193" height="850" bgcolor="whitesmoke" style="border-width:1px; border-top-color:black; border-right-color:rgb(232,232,230); border-bottom-color:black; border-left-color:rgb(232,232,230); border-top-style:none; border-right-style:solid; border-bottom-style:none; border-left-style:solid;" valign="top">
                <table cellpadding="0" cellspacing="0" width="1085" align="center">
                    <tr>
                        <td width="1000" height="39">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="1000" height="45" align="center">
                           <span style="font-size:16pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_STATISTICS%></span>
                        </td>
                    </tr>
                    <tr>
                        <td width="1000" height="45" align="center">
                            <table cellpadding="0" cellspacing="0">
                                <tr height="38">
                                    <td><span style="font-size:10pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_CREATOR%></span></td>
                                    <td>
                                        <asp:DropDownList ID="select_login_id" runat="server" style="margin-left:10px; font-size:10pt; color:#67707E; border-width:1px; border-style:solid;"></asp:DropDownList>
                                    </td>
                                    <td><span style="margin-left:20px; font-size:10pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_PRODUCT_TYPE%></span></td>
                                    <td>
                                        <asp:DropDownList ID="select_product_type" runat="server" style="margin-left:10px; font-size:10pt; color:#67707E; border-width:1px; border-style:solid;"></asp:DropDownList>
                                    </td>
                                    <td><span style="margin-left:20px; font-size:10pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_SCALE_SETTING%></span></td>
                                    <td>
                                        <asp:HiddenField ID="hd_curdate" runat="server" />
                                        <asp:HiddenField ID="hd_type" runat="server" />
                                        <span style="margin-left:10px; font-size:10pt; color:#42485a;">
                                            <input type="radio" name="chk" onclick="setSelectObject(1)" value="1"/><%=Resources.Lang.STR_YEAR_STAT%>
                                            <input type="radio" name="chk" onclick="setSelectObject(2)" value="2"/><%=Resources.Lang.STR_MONTH_STAT%>
                                            <input type="radio" name="chk" onclick="setSelectObject(3)" value="3"/><%=Resources.Lang.STR_DAY_STAT%>
                                        </span>
                                    </td>
                                    <td>
                                        <span id="div_select" style="margin-left:10px; font-size:10pt; color:#42485a;">
                                        </span>
                                    </td>
                                    <td width="20"></td>
                                    <td width="60" align="right">
                                        <asp:Button ID="bt_search" runat="server" OnClientClick="return Stat();" OnClick="btnStat_Click" CssClass="btnSmallGreen" width="58" height="23" Text="<%$Resources:Lang, STR_STAT %>" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="1085" align="center">
                    <tr>
                        <td width="1000" height="370" align="center">
                            <div style="height: 380px; overflow: hidden">
                                <div id="divStatGraph" style="min-width: 1000px; height: 400px; margin: 0 auto"></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="1000" height="30" align="center"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStatTable" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="1085" align="center">
                    <tr>
                        <td height="40"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
