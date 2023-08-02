<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="AuthedQRCodeList.aspx.cs" Inherits="Web.Manager.AuthedQRCodeList" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/BigInt.js" type="text/javascript"></script>
    <script src="/Scripts/highcharts/highcharts.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(1);
            setSubMenu(3);
            checkObject();

            $('input[type=radio][name=optSaveExcelType]').change(function () {
                if (this.value == '1') {
                    //현재 체크된 리스트만 발급
                    $("#<%=hd_ids.ClientID %>").val(get_chked_values());
                }
                else if (this.value == '2') {
                    //전체 리스트 발급
                    $("#<%=hd_ids.ClientID %>").val("all");
                }
            });

            var moveLeft = 0;
            var moveDown = 0;
            $('.popper').hover(function (e) {
                var target = '#' + ($(this).attr('data-popbox'));

                var qrcode_number = $(this).html();
                $("#divQRCodePop").html("");
                $("#divQRCodePop").qrcode({
                    width: 80,
                    height: 80,
                    text: qrcode_number
                });

                $(target).show();
                moveLeft = $(this).outerWidth();
                moveDown = ($(target).outerHeight() / 2);
            }, function () {
                var target = '#' + ($(this).attr('data-popbox'));
                if (!($(".popper").hasClass("show"))) {
                    $(target).hide();
                }
            });

            $('.popper').mousemove(function (e) {
                var target = '#' + ($(this).attr('data-popbox'));

                //leftD = e.pageX + parseInt(moveLeft);
                leftD = e.pageX + 20;
                maxRight = leftD + $(target).outerWidth();
                windowLeft = $(window).width() - 20;
                windowRight = 0;
                maxLeft = e.pageX - (parseInt(moveLeft) + $(target).outerWidth() + 10);

                if (maxRight > windowLeft && maxLeft > windowRight) {
                    leftD = maxLeft;
                }

                topD = e.pageY - parseInt(moveDown);
                maxBottom = parseInt(e.pageY + parseInt(moveDown) + 10);
                windowBottom = parseInt(parseInt($(document).scrollTop()) + parseInt($(window).height()));
                maxTop = topD;
                windowTop = parseInt($(document).scrollTop());
                if (maxBottom > windowBottom) {
                    topD = windowBottom - $(target).outerHeight() - 10;
                } else if (maxTop < windowTop) {
                    topD = windowTop + 10;
                }

                $(target).css('top', topD).css('left', leftD);
            });

        });

        function setMemo() {
            var id = $("#hd_id").val();
            var memo = $("#txtMemo").val();

            if (id == "") return;

            $.ajax({
                url: "SetMemo.aspx?id=" + id + "&memo=" + encodeURIComponent(memo),
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //메모내용이 정확히 저장되었습니다.
                        alert("<%=Resources.Lang.MSG_MEMO_REG_SUCCESS %>");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //메모저장중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_MEMO_REG_ERROR %>");
                    return;
                }
            });
        }

        function Search() {
            $("#<%=hd_search_type.ClientID %>").val($("#<%=select_search_type.ClientID %> option:selected").val());
            if ($("#<%=hd_search_type.ClientID %>").val() == 1 || $("#<%=hd_search_type.ClientID %>").val() == 2) {
                if ($("#<%=txt_searchkey.ClientID %>").val() == "") {
                    //검색문자열을 입력해주십시오.
                    alert("<%=Resources.Lang.MSG_INPUT_SEARCH_STRING %>");
                    return false;
                }
                return true;
            } else if ($("#<%=hd_search_type.ClientID %>").val() == 3 || $("#<%=hd_search_type.ClientID %>").val() == 4 || $("#<%=hd_search_type.ClientID %>").val() == 5) {
                var dateStart = new Date($("#<%=txt_dateStart.ClientID %>").val());
                var dateEnd = new Date($("#<%=txt_dateEnd.ClientID %>").val());
                if ($("#<%=txt_dateStart.ClientID %>").val() == "" || $("#<%=txt_dateEnd.ClientID %>").val() == "" || dateStart > dateEnd) {
                    //날짜범위을 정확히 입력해주십시오.
                    alert("<%=Resources.Lang.MSG_INPUT_SEARCH_DATESCALE %>");
                    return false;
                }
                return true;
            } else if ($("#<%=hd_search_type.ClientID %>").val() == 6) {
                return true;
            } else {
                return false;
            }
        }

        function checkCalendar(type) {
            if (type == 1) {
                $("#<%=txt_dateStart.ClientID %>").attr("readonly", true);
                showCal('<%=txt_dateStart.ClientID %>');
            }
            else if(type == 2) {
                $("#<%=txt_dateEnd.ClientID %>").attr("readonly", true);
                showCal('<%=txt_dateEnd.ClientID %>');
            }
        }

        function checkObject() {
            if ($("#<%=select_search_type.ClientID %> option:selected").val() == 1 || $("#<%=select_search_type.ClientID %> option:selected").val() == 2) {
                $("#divTextObj").css("display", "");
                $("#divDateObj").css("display", "none");
                $("#divproductTypeObj").css("display", "none");
            } else if ($("#<%=select_search_type.ClientID %> option:selected").val() == 3 || $("#<%=select_search_type.ClientID %> option:selected").val() == 4 || $("#<%=select_search_type.ClientID %> option:selected").val() == 5) {
                $("#divTextObj").css("display", "none");
                $("#divDateObj").css("display", "");
                $("#divproductTypeObj").css("display", "none");
            } else if ($("#<%=select_search_type.ClientID %> option:selected").val() == 6) {
                $("#divTextObj").css("display", "none");
                $("#divDateObj").css("display", "none");
                $("#divproductTypeObj").css("display", "");
            }
        }

        function showAuthedQRCodeDetailPopup(id) {
            $("#hd_id").val(id);
            $.ajax({
                url: "GetAuthedQRCodeDetail.aspx?id=" + id,
                dataType: 'json',
                async: true,
                type: 'POST',
                success: function (data) {
                    var reg_date = decodeURIComponent(data.reg_date);
                    var auth_date = decodeURIComponent(data.auth_date);
                    var login_id = decodeURIComponent(data.login_id);
                    var qrcode_number = decodeURIComponent(data.qrcode_number);
                    var apply_count = data.apply_count;
                    var product_type = data.product_type;
                    var device_data = (decodeURIComponent(data.device_data).replace(/\+/gi, " ")).split(';');
                    var memo = decodeURIComponent(data.memo);
                    //reg_date
                    $("#lbl_RegDate").html(reg_date);
                    //auth_date
                    $("#lbl_AuthDate").html(auth_date);
                    //login_id             
                    $("#lbl_LoginID").html(login_id);
                    //qrcode_number               
                    $("#lbl_QRCodeNumber").html(qrcode_number);
                    //apply_count
                    $("#lbl_ApplyCount").html(apply_count);
                    //product_type
                    if (product_type == 1)
                        $("#lbl_ProductType").html("<%=Resources.Lang.STR_REAL_PRODUCT %>");
                    else if (product_type == 2)
                        $("#lbl_ProductType").html("<%=Resources.Lang.STR_TEST_PRODUCT %>");
                    //divDeviceInfo
                    var div_html = "";
                    div_html += "<table cellpadding=\"0\" cellspacing=\"0\" align=\"left\" width=\"880\" style=\"border-collapse:collapse;\">";
                    div_html += "<tr>";
                    div_html += "<td width=\"50\" height=\"40\" bgcolor=\"#EC5565\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:11pt; color:white;\">NO</span></td>";
                    div_html += "<td width=\"100\" height=\"40\" bgcolor=\"#EC5565\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:11pt; color:white;\"><%=Resources.Lang.STR_REG_DATE %></span></td>";
                    div_html += "<td width=\"180\" height=\"40\" bgcolor=\"#EC5565\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:11pt; color:white;\"><%=Resources.Lang.STR_DEVICE_NAME %></span></td>";
                    div_html += "<td width=\"200\" height=\"40\" bgcolor=\"#EC5565\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:11pt; color:white;\"><%=Resources.Lang.STR_DEVICE_OS_VERSION %></span></td>";
                    div_html += "<td width=\"160\" height=\"40\" bgcolor=\"#EC5565\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:11pt; color:white;\"><%=Resources.Lang.STR_DEVICE_IMEI %></span></td>";
                    div_html += "<td width=\"170\" height=\"40\" bgcolor=\"#EC5565\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:11pt; color:white;\"><%=Resources.Lang.STR_AUTH_STATUS %></span></td>";
                    div_html += "</tr>";
                    if (data.device_data == "") {
                        div_html += "<tr><td width=\"170\" height=\"40\" colspan=\"6\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:10pt; color:#333333;\"><%=Resources.Lang.MSG_NO_REG_DEVICE %></span></td></tr>";
                    } else {
                        for (var i = 0; i < device_data.length; i++) {
                            var jsonObj = JSON.parse(device_data[i]);
                            var id = jsonObj.id;
                            var device_id = decodeURIComponent(jsonObj.device_id);
                            var device_name = decodeURIComponent(jsonObj.device_name);
                            var device_version = decodeURIComponent(jsonObj.device_version);
                            var is_activated = decodeURIComponent(jsonObj.is_activated);
                            var reg_date = decodeURIComponent(jsonObj.reg_date);

                            div_html += "<tr>";
                            div_html += "<td width=\"50\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:10pt; color:#333333;\">" + (i + 1) + "</span></td>";
                            div_html += "<td width=\"100\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:10pt; color:#333333;\">" + reg_date + "</span></td>";
                            div_html += "<td width=\"180\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:10pt; color:#333333;\">" + device_name + "</span></td>";
                            div_html += "<td width=\"200\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:10pt; color:#333333;\">" + device_version + "</span></td>";
                            div_html += "<td width=\"160\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border:1px solid white;\"><span style=\"font-size:10pt; color:#333333;\">" + device_id + "</span></td>";
                            if (is_activated == 0) {
                                div_html += "<td width=\"170\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border-width:1px; border-color:white; border-style:solid;\"><span style=\"font-size:10pt; color:#333333;\"><input id=\"d_img_" + id + "\" type=\"button\" class=\"btnNoAct\" onclick=\"setActivate('" + id + "')\" value=\"<%=Resources.Lang.STR_NO_ACTIVATED %>\" style=\"width:126px; height:27px;\"></span></td>";
                            } else if (is_activated == 1) {
                                div_html += "<td width=\"170\" height=\"40\" bgcolor=\"#e7e7e7\" align=\"center\" style=\"border-width:1px; border-color:white; border-style:solid;\"><span style=\"font-size:10pt; color:#333333;\"><input id=\"d_img_" + id + "\" type=\"button\" class=\"btnAct\" onclick=\"setActivate('" + id + "')\" value=\"<%=Resources.Lang.STR_ACTIVATED %>\" style=\"width:126px; height:27px;\"></span></td>";
                            }
                            div_html += "</tr>";
                        }
                    }
                    div_html += "</table>";
                    $("#divDeviceInfo").html(div_html);
                    //memo
                    $("#txtMemo").val(memo);

                    //QRcode Image
                    $("#divQRCodeCanvas").html("");
                    $("#divQRCodeCanvas").qrcode({
                        width: 120,
                        height: 120,
                        text: qrcode_number
                    });

                    showPopup("divAuthedQRCodeDetailPopup");
                    return;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //알림
                    $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                    //QR코드정보 조회중 오류가 발생하였습니다.
                    $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_LISTING_ERROR %>");
                    showPopup("divPopupAlert");
                    return;
                }
            });
        }

        function setActivate(id) {
            $.ajax({
                url: "ChangeQRCodeActivate.aspx?id=" + id + "&qrcode_id=" + $("#hd_id").val(),
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //비활성화하기 성공
                        $("#d_img_" + id).removeClass("btnAct");
                        $("#d_img_" + id).addClass("btnNoAct");
                        $("#d_img_" + id).val("<%=Resources.Lang.STR_NO_ACTIVATED %>");
                        //해당 디바이스가 비활성화 되었습니다.
                        alert("<%=Resources.Lang.MSG_DEVICE_NO_ACTIVATED %>");
                        return;
                    } else if (data == "1") {
                        //활성화하기 성공
                        $("#d_img_" + id).removeClass("btnNoAct");
                        $("#d_img_" + id).addClass("btnAct");
                        $("#d_img_" + id).val("<%=Resources.Lang.STR_ACTIVATED %>");
                        //해당 디바이스가 활성화 되었습니다.
                        alert("<%=Resources.Lang.MSG_DEVICE_ACTIVATED %>");
                        return;
                    } else if (data == "2") {
                        //QR코드가 없을때
                        //QR코드가 존재하지 않습니다.
                        alert("<%=Resources.Lang.MSG_QRCODE_NO_EXIST %>");
                        return;
                    } else if (data == "3") {
                        //디바이스가 없을때
                        //적용할 디바이스가 존재하지 않습니다.
                        alert("<%=Resources.Lang.MSG_DEVICE_NO_EXIST %>");
                        return;
                    } else if (data == "4") {
                        //적용가능한 기기수 초과했을때
                        //적용 가능한 기기 수를 확인해주세요.
                        alert("<%=Resources.Lang.MSG_APPLY_COUNT_CONFIRM %>");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //QR코드상태 수정중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_QRCODE_DETAIL_CHANGE_ERROR %>");
                    return;
                }
            });
        }

        function showSaveExcelPopup() {
            if ($('input[type=radio][name=optSaveExcelType]:checked').val() == '1') {
                //현재 체크된 리스트만 저장
                $("#<%=hd_ids.ClientID %>").val(get_chked_values());
            }
            else if ($('input[type=radio][name=optSaveExcelType]:checked').val() == '2') {
                //전체 리스트 저장
                $("#<%=hd_ids.ClientID %>").val("all");
            }
            else {
                $("#<%=hd_ids.ClientID %>").val("");
            }
            showPopup("divSaveExcelAlert");
        }

        function saveExcel() {
            if ($("#<%=hd_ids.ClientID %>").val() == "") {
                //저장확인
                $("#lbl_title").html("<%=Resources.Lang.STR_SAVE_CONFIRM %>");
                //xls파일로 인쇄할 QR코드번호를 선택해주십시오.
                $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_SAVE_CHECK %>");
                showPopup("divPopupAlert");
                return false;
            }
            closePopup();
            return true; 
        }

        function get_chked_values() {
            var chked_val = "";
            $(":checkbox[name='chkNo']:checked").each(function (pi, po) {
                chked_val += "," + po.value;
            });
            if (chked_val != "") chked_val = chked_val.substring(1);
            return chked_val;
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
                        <td width="289" height="39"></td>
                        <td width="796" height="39">&nbsp;</td>
                     </tr>
                     <tr>
                        <td width="289" height="45">
                            <span style="font-size:16pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_AUTHED_QRCODE_LIST%></span>
                        </td>
                        <td width="796" height="45">
                            <div align="right">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="130" height="38">
                                            <table cellpadding="0" cellspacing="0" width="130">
                                                <tr>
                                                    <td width="130" height="32">
                                                        <font color="#67707E">
                                                            <asp:HiddenField ID="hd_search_type" runat="server" Value="0" />
                                                            <asp:DropDownList ID="select_search_type" runat="server" onchange="checkObject()" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; border-style:solid;"></asp:DropDownList>
                                                        </font>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td height="38">
                                            <div id="divTextObj" style="display:none;">
                                                <asp:TextBox ID="txt_searchkey" runat="server" Text="" style="font-size:11pt; color:rgb(102,102,102); width:100px;"></asp:TextBox>
                                            </div>
                                            <div id="divDateObj" style="display:none;">
                                                <asp:TextBox ID="txt_dateStart" runat="server" Text="" onclick="checkCalendar('1')" style="font-size:11pt; color:rgb(102,102,102); width:100px;"></asp:TextBox> ~ <asp:TextBox ID="txt_dateEnd" runat="server" Text="" onclick="checkCalendar('2')" style="font-size:11pt; color:rgb(102,102,102); width:100px;"></asp:TextBox>
                                            </div>
                                            <div id="divproductTypeObj" style="display:none;">
                                                <asp:DropDownList ID="ddl_productType" runat="server" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; border-style:solid;">
                                                    <asp:ListItem Text="<%$Resources:Lang, STR_REAL_PRODUCT %>" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$Resources:Lang, STR_TEST_PRODUCT %>" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td width="60" height="38" align="right">
                                            <asp:Button ID="bt_search" runat="server" OnClientClick="return Search();" OnClick="btnSearch_Click" CssClass="btnSmallGray" width="58" height="23" Text="<%$Resources:Lang, STR_SEARCH %>" />
                                        </td>
                                        <td width="110" height="38" align="right">
                                            <input type="button" class="btnSmallGreen" onclick="showSaveExcelPopup()" value="<%=Resources.Lang.STR_DOWNLOAD_EXCEL %>" style="width:103px; height:23px;"/>
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
                        <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <HeaderTemplate>
                                <input type="checkbox" style="height: auto;" value="All" onclick="checkAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrCheckbox" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_CREATE_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_AUTH_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrAuthDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_CREATOR %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrLoginID" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_QRCODE_NUMBER %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrQRCodeNumber" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="120" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_APPLY_COUNT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrApplyCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_REG_DEVICE_COUNT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDeviceCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="90" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_FIRST_DEVICE_REG_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDeviceRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_PRODUCT_TYPE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrProductType" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DETAIL %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDetail" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
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
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom"
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
    <div class="clspopup" id="divAuthedQRCodeDetailPopup">
        <input id="hd_id" type="hidden" value="" />
        <table cellpadding="0" cellspacing="0" width="950" background="/img/pop_bg4.png">
            <tr>
                <td width="950" height="600" valign="top">
                    <table cellpadding="0" cellspacing="0" width="909" align="center">
                        <tr>
                            <td width="909" height="70">
                                <table cellpadding="0" cellspacing="0" width="884" align="center">
                                    <tr>
                                        <td width="416" height="35">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_DETAIL %></span>
								        </td>
                                        <td width="468" height="35" align="right">
                                            <img onclick="confirm()" src="/img/bt_close.png" width="22" height="22" border="0" alt="" style="cursor:pointer;"/>
								        </td>
							        </tr>
						        </table>
					        </td>
                        </tr>
                        <tr>
                            <td width="909" height="517" valign="top">
                                <table cellpadding="0" cellspacing="0" width="890" align="center">
                                    <tr>
                                        <td width="890" height="30">
									        <span style="font-size:12pt; font-weight:bold; color:#DA4453;"><%=Resources.Lang.STR_QRCODE_INFO %></span>
								        </td>
                                    </tr>
                                    <tr>
                                        <td width="890" height="160">
                                            <table cellpadding="0" cellspacing="0" width="890" align="center">
                                                <tr>
                                                    <td width="750" height="80" align="center">
                                                        <div style="width:750px; overflow-y:auto;">
                                                            <table cellpadding="0" cellspacing="0" align="center" width="750" style="border-collapse:collapse;">
                                                                <tr>
                                                                    <td width="100" height="40" bgcolor="#EC5565" align="center" style="border-width:1px; border-color:white; border-style:solid;">
                                                                        <span style="font-size:11pt; color:White;"><%=Resources.Lang.STR_CREATE_DATE%></span>
											                        </td>
                                                                    <td width="100" height="40" bgcolor="#EC5565" align="center" style="border-width:1px; border-color:white; border-style:solid;">
                                                                        <span style="font-size:11pt; color:White;"><%=Resources.Lang.STR_AUTH_DATE%></span>
											                        </td>
                                                                    <td width="100" height="40" bgcolor="#EC5565" align="center" style="border-width:1px; border-color:white; border-style:solid;">
                                                                        <span style="font-size:11pt; color:White;"><%=Resources.Lang.STR_CREATOR%></span>
											                        </td>
                                                                    <td width="180" height="40" bgcolor="#EC5565" align="center" style="border-width:1px; border-color:white; border-style:solid;">
                                                                        <span style="font-size:11pt; color:White;"><%=Resources.Lang.STR_QRCODE_NUMBER %></span>
											                        </td>
                                                                    <td width="150" height="40" bgcolor="#EC5565" align="center" style="border-width:1px; border-color:white; border-style:solid;">
                                                                        <span style="font-size:11pt; color:White;"><%=Resources.Lang.STR_APPLY_COUNT %></span>
											                        </td>
                                                                    <td width="120" height="40" bgcolor="#EC5565" align="center" style="border-width:1px; border-color:white; border-style:solid;">
                                                                        <span style="font-size:11pt; color:White;"><%=Resources.Lang.STR_PRODUCT_TYPE %></span>
											                        </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100" height="40" bgcolor="#e7e7e7" align="center" style="border:1px solid white">
                                                                        <span style="font-size:10pt; color:#333333;"><label id="lbl_RegDate"></label></span>
											                        </td>
                                                                    <td width="100" height="40" bgcolor="#e7e7e7" align="center" style="border:1px solid white">
                                                                        <span style="font-size:10pt; color:#333333;"><label id="lbl_AuthDate"></label></span>											                        
                                                                    </td>
                                                                    <td width="100" height="40" bgcolor="#e7e7e7" align="center" style="border:1px solid white">
                                                                        <span style="font-size:10pt; color:#333333;"><label id="lbl_LoginID"></label></span>
											                        </td>
                                                                    <td width="180" height="40" bgcolor="#e7e7e7" align="center" style="border:1px solid white">
												                        <span style="font-size:10pt; color:#333333;"><label id="lbl_QRCodeNumber"></label></span>
											                        </td>
                                                                    <td width="150" height="40" bgcolor="#e7e7e7" align="center" style="border:1px solid white">
                                                                        <span style="font-size:10pt; color:#333333;"><label id="lbl_ApplyCount"></label></span>
											                        </td>
                                                                    <td width="120" height="40" bgcolor="#e7e7e7" align="center" style="border:1px solid white">
                                                                        <span style="font-size:10pt; color:#333333;"><label id="lbl_ProductType"></label></span>
											                        </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <table cellpadding="0" cellspacing="0" align="center" width="750" style="border-collapse:collapse;">
                                                            <tr>
                                                                <td width="50" height="60" align="left">
                                                                    <span style="font-size:12pt; font-weight:bold; color:#DA4453;"><%=Resources.Lang.STR_MEMO %></span>
                                                                </td>
                                                                <td width="630" height="60">
                                                                    <input type="text" id="txtMemo" maxlength="50" placeholder="<%=Resources.Lang.STR_LIMIT_50 %>" style="width:620px;"/>
                                                                </td>
                                                                <td width="50" height="60">
                                                                    <input type="button" class="btnMiddle" onclick="setMemo()" value="<%=Resources.Lang.STR_SAVE %>" style="width:58px; height:23px;"/>
                                                                </td>
                                                            </tr>
                                                        </table>
								                    </td>
                                                    <td width="140" height="140" align="center" valign="top">
                                                        <table cellpadding="0" cellspacing="0" align="center" width="130" style="border-collapse:collapse;">
                                                            <tr>
                                                                <td width="125" height="125" align="center">
                                                                    <div id="divQRCodeCanvas"></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="890" height="30">
									        <span style="font-size:12pt; font-weight:bold; color:#DA4453;"><%=Resources.Lang.STR_APPLY_DEVICE_INFO %></span>
								        </td>
                                    </tr>
                                    <tr>
                                        <td width="890" height="220" align="center">
                                            <div id="divDeviceInfo" style="width:888px; height:200px; overflow-x:hidden; overflow-y:auto;">
                                            </div>
								        </td>
                                    </tr>
                                    <tr>
                                        <td width="890" height="60" align="center">
                                            <input type="button" class="btnMiddle" onclick="confirm()" value="<%=Resources.Lang.STR_CONFIRM %>" style="width:110px; height:32px;"/>
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
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_DOWNLOAD_EXCEL%></span>
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
                                <asp:HiddenField ID="hd_ids" runat="server" Value="1" />
                                <table cellpadding="0" cellspacing="0" align="center" width="409">
                                    <tr>
                                        <td width="100" height="50">
                                        </td>
                                        <td width="309" height="50">
                                            <span style="font-size:11pt; color:#FF0000;">
                                                <input type="radio" name="optSaveExcelType" value="1" checked="checked" /><label><%=Resources.Lang.MSG_CHECKED_LIST%></label> 
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" height="70">
                                        </td>
                                        <td width="309" height="70">
                                            <span style="font-size:11pt; color:#FF0000;">
                                                <input type="radio" name="optSaveExcelType" value="2" /><label><%=Resources.Lang.MSG_ALL_LIST %></label>
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

    <div id="divQRCodePop" class="popbox"></div>
</asp:Content>
