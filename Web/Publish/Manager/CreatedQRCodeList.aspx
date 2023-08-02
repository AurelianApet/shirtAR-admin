<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="CreatedQRCodeList.aspx.cs" Inherits="Web.Manager.CreatedQRCodeList" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(1);
            setSubMenu(2);

            checkObject();

            $('input[type=radio][name=optSaveExcelType]').change(function () {
                $("#<%=hd_saveExcelType.ClientID %>").val(this.value);
            });

            $('input[type=radio][name=optAuthType]').change(function () {
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

        function get_chked_values() {
            var chked_val = "";
            $(":checkbox[name='chkNo']:checked").each(function (pi, po) {
                chked_val += "," + po.value;
            });
            if (chked_val != "") chked_val = chked_val.substring(1);
            return chked_val;
        }

        function showDelPopup() {
            var str_ids = get_chked_values();
            if (str_ids == "") {
                //알림
                $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                //삭제할 QR코드번호를 선택해주십시오.
                $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_DELETE_CHECK %>");
                showPopup("divPopupAlert");
                return;
            }
            showPopup("divDelConfirmAlert");
            return;
        }

        function DelQRCode() {
            var str_ids = get_chked_values();

            showProgress();
            $.ajax({
                url: "DelQRCode.aspx?ids=" + encodeURIComponent(str_ids),
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //삭제확인
                        $("#lbl_title").html("<%=Resources.Lang.STR_DELETE_CONFIRM %>");
                        //선택한 QR코드번호가 정확히 삭제되었습니다. <br><br>
                        $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_DELETE_SUCCESS %>");
                        showPopup("divPopupAlert");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //QR코드번호삭제중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_QRCODE_DELETE_ERROR %>");
                    closePopup();
                    return;
                }
            });
        }

        function showAuthPopup() {
            if ($('input[type=radio][name=optAuthType]:checked').val() == '1') {
                //현재 체크된 리스트만 발급
                $("#<%=hd_ids.ClientID %>").val(get_chked_values());
            }
            else if ($('input[type=radio][name=optAuthType]:checked').val() == '2') {
                //전체 리스트 발급
                $("#<%=hd_ids.ClientID %>").val("all");
            }
            else {
                $("#<%=hd_ids.ClientID %>").val("");
            }
            showPopup("divAuthConfirmAlert");
            return;
        }

        function Auth() {
            var str_ids = $("#<%=hd_ids.ClientID %>").val();
            if (str_ids == '') {
                //발급확인
                $("#lbl_title").html("<%=Resources.Lang.STR_AUTH_CONFIRM %>");
                //발급할 QR코드번호를 선택해주십시오.
                $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_AUTH_CHECK %>");
                showPopup("divPopupAlert");
                return false;
            }
            return true;
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
            } else if ($("#<%=hd_search_type.ClientID %>").val() == 3) {
                var dateStart = new Date($("#<%=txt_dateStart.ClientID %>").val());
                var dateEnd = new Date($("#<%=txt_dateEnd.ClientID %>").val());
                if ($("#<%=txt_dateStart.ClientID %>").val() == "" || $("#<%=txt_dateEnd.ClientID %>").val() == "" || dateStart > dateEnd) {
                    //날짜범위을 정확히 입력해주십시오.
                    alert("<%=Resources.Lang.MSG_INPUT_SEARCH_DATESCALE %>");
                    return false;
                }
                return true;
            } else if ($("#<%=hd_search_type.ClientID %>").val() == 4) {
                return true;
            } else {
                return false;
            }
        }

        function img_src(path) {
            $("#<%= bt_search.ClientID %>").attr("src", path);
        }

        function conf_img_src(path) {
            $("#<%= bt_conf.ClientID %>").attr("src", path);
        }

        function showModifyQRCodePopup(id) {
            $("#hd_id").val(id);
            $.ajax({
                url: "GetQRCodeInfo.aspx?id=" + id,
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    var stats_data = decodeURIComponent(data);
                    if (stats_data == "") {
                        //알림
                        $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                        //선택하신 QR코드가 이미 발급되였기때문에 수정할수 없습니다. 
                        $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_ALREADY_AUTH %>");
                        showPopup("divPopupAlert");
                        return;
                    }
                    var strArray = stats_data.split(',');
                    $("#lbl_Nickname").html(strArray[0]);       //nickname
                    $("#lbl_LoginID").html(strArray[1]);        //loginid
                    $("#lbl_QRCodeNumber").html(strArray[2]);   //qrcodenumber

                    //QRcode Image
                    $("#divQRCodeCanvas").html("");
                    $("#divQRCodeCanvas").qrcode({
                        width: 180,
                        height: 180,
                        text: strArray[2]
                    });

                    //applycount
                    $("#select_apply_count option[value=" + strArray[3] + "]").prop("selected", true);
                    //producttype
                    $("#select_product_type option[value=" + strArray[4] + "]").prop("selected", true);
                    showPopup("divModifyQRCodePopup");
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

        function ChangeQRCode() {
            var id = $("#hd_id").val();
            var apply_count = $("#select_apply_count option:selected").val();
            var product_type = $("#select_product_type option:selected").val();

            showProgress();
            $.ajax({
                url: "ChangeQRCode.aspx?id=" + id + "&apply_count=" + apply_count + "&product_type=" + product_type,
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        closePopup();
                        //알림
                        $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                        //QR코드번호정보가 정확히 수정되었습니다. 
                        $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_CHANGE_SUCCESS %>");
                        showPopup("divPopupAlert");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //알림
                    $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                    //QR코드번호수정중 예상치않은 오류가 발생하였습니다.
                    $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_CHANGE_ERROR %>");
                    showPopup("divPopupAlert");
                    return;
                }
            });
        }

        function checkCalendar(type) {
            if (type == 1) {
                $("#<%=txt_dateStart.ClientID %>").attr("readonly", true);
                showCal('<%=txt_dateStart.ClientID %>');
            }
            else if (type == 2) {
                $("#<%=txt_dateEnd.ClientID %>").attr("readonly", true);
                showCal('<%=txt_dateEnd.ClientID %>');
            }
        }

        function checkObject() {
            if ($("#<%=select_search_type.ClientID %> option:selected").val() == 1 || $("#<%=select_search_type.ClientID %> option:selected").val() == 2) {
                $("#divTextObj").css("display", "");
                $("#divDateObj").css("display", "none");
                $("#divproductTypeObj").css("display", "none");
            } else if ($("#<%=select_search_type.ClientID %> option:selected").val() == 3) {
                $("#divTextObj").css("display", "none");
                $("#divDateObj").css("display", "");
                $("#divproductTypeObj").css("display", "none");
            } else if ($("#<%=select_search_type.ClientID %> option:selected").val() == 4) {
                $("#divTextObj").css("display", "none");
                $("#divDateObj").css("display", "none");
                $("#divproductTypeObj").css("display", "");
            } else if ($("#<%=select_search_type.ClientID %> option:selected").val() == 6) {
                $("#divTextObj").css("display", "none");
                $("#divDateObj").css("display", "none");
                $("#divproductTypeObj").css("display", "none");
            }
        }

        function showSaveExcelPopup() {
            $("#<%=hd_saveExcelType.ClientID %>").val($('input[type=radio][name=optSaveExcelType]:checked').val());
            showPopup("divSaveExcelAlert");
        }

        function saveExcel() {
            closePopup();
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
                <table cellpadding="0" cellspacing="0" width="1000" align="center">
                    <tr>
                        <td width="289" height="39"></td>
                        <td width="796" height="39">&nbsp;</td>
                     </tr>
                     <tr>
                        <td width="289" height="45">
                            <span style="font-size:14pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_CREATED_QRCODE_LIST%></span>
                        </td>
                        <td width="796" height="45">
                            <div align="right">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="125" height="38">
                                            <table cellpadding="0" cellspacing="0" width="125">
                                                <tr>
                                                    <td width="125" height="32" align="center">
                                                        <font color="#67707E">
                                                            <asp:HiddenField ID="hd_search_type" runat="server" Value="0" />
                                                            <asp:DropDownList ID="select_search_type" runat="server" onchange="checkObject()" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; border-style:solid;">
                                                            </asp:DropDownList>
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
                                            <input type="button" class="btnSmallGreen" onclick="showSaveExcelPopup()" value="<%=Resources.Lang.STR_SAVE_EXCEL %>" style="width:103px; height:23px;"/>
                                        </td>
                                        <td width="60" height="38" align="right">
                                            <input type="button" class="btnSmallGreen" onclick="showAuthPopup()" value="<%=Resources.Lang.STR_AUTH %>" style="width:58px; height:23px;"/>
                                        </td>
                                        <td width="60" height="38" align="right">
                                            <input type="button" class="btnSmallRed" onclick="showDelPopup()" value="<%=Resources.Lang.STR_DELETE %>" style="width:58px; height:23px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvContent" runat="server" AllowSorting="false" Width="1000" GridLines="None" AutoGenerateColumns="false"
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
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_CREATOR %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrLoginID" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="200" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_QRCODE_NUMBER %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrQRCodeNumber" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="200" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_APPLY_COUNT %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrApplyCount" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_PRODUCT_TYPE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrProductType" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_CHANGE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrModify" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
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
                        LastPageText="<%$Resources:Str, STR_LAST %>"
                        PageButtonCount="10" />
                    <PagerStyle CssClass="clspager" HorizontalAlign="Center" />
                </asp:GridView>
                <table cellpadding="0" cellspacing="0" width="1000" align="center">
                    <tr>
                        <td height="40"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="clspopup" id="divAuthConfirmAlert">
        <table cellpadding="0" cellspacing="0" width="560" background="/img/pop_bg3.png">
            <tr>
                <td width="560" height="337" valign="top">
                    <table cellpadding="0" cellspacing="0" width="470" align="center">
                        <tr>
                            <td width="470" height="71">
                                <table cellpadding="0" cellspacing="0" width="477" align="center">
                                    <tr>
                                        <td width="306" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_AUTH_CONFIRM %></span>
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
                                        <td width="409" height="60">                                    
                                            <span style="font-size:11pt; color:#666666;"><%=Resources.Lang.MSG_AUTH_REALLY %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="409" height="36">
                                            <asp:HiddenField ID="hd_ids" runat="server" Value="" />
                                            <span style="font-size:11pt; color:#FF0000;">
                                                <input type="radio" name="optAuthType" value="1" checked="checked" /><label><%=Resources.Lang.MSG_CHECKED_LIST %></label> 
                                                <input type="radio" name="optAuthType" value="2" /><label><%=Resources.Lang.MSG_ALL_LIST %></label>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="409" height="70">    
                                            <span style="font-size:11pt; color:#666666;"><%=Resources.Lang.MSG_CONFIRM_DESCRIPTION %></span>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="283" align="center">
                                    <tr>
                                        <td width="141" height="63" align="center">
                                            <asp:Button ID="bt_conf" runat="server" OnClick="btnConfirm_Click" OnClientClick="return Auth();" CssClass="btnMiddle" width="110" height="32" Text="<%$Resources:Lang, STR_YES %>" />
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
                                            <input type="button" class="btnMiddle" onclick="DelQRCode()" value="<%=Resources.Lang.STR_YES %>" style="width:110px; height:32px;"/>
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
    <div class="clspopup" id="divModifyQRCodePopup">
        <table cellpadding="0" cellspacing="0" width="687" background="/img/pop_bg2.png" >
            <tr>
                <td width="687" height="405" valign="top">
                    <table cellpadding="0" cellspacing="0" width="655" align="center">
                        <tr>
                            <td width="655" height="80">
                                <table cellpadding="0" cellspacing="0" width="624" align="center">
                                    <tr>
                                        <td width="416" height="31">
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_INFO_CHANGE %></span>
                                        </td>
                                        <td width="208" height="31" align="right">
                                            <img onclick="closePopup()" src="/img/bt_close.png" width="22" height="22" border="0" alt="" style="cursor:pointer;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="655" height="300" valign="top">
                                <input id="hd_id" type="hidden"/>
                                <table cellpadding="0" cellspacing="0" align="center" width="400">
                                    <tr>
                                        <td width="380">
                                            <table cellpadding="0" cellspacing="0" align="center" width="350">
                                                <tr>
                                                    <td width="150" height="50"><span style="font-size:11pt; color:#666666;"><%=Resources.Lang.STR_CREATOR %></span></td>
                                                    <td width="200" height="50"><span style="font-size:11pt; color:#666666;"><label id="lbl_LoginID"></label></span></td>
                                                </tr>
                                                <tr>
                                                    <td width="150" height="50"><span style="font-size:11pt; color:#666666;"><%=Resources.Lang.STR_QRCODE_NUMBER %></span></td>
                                                    <td width="200" height="50"><span style="font-size:11pt; color:#666666;"><label id="lbl_QRCodeNumber"></label></span></td>
                                                </tr>
                                                <tr>
                                                    <td width="150" height="50"><span style="font-size:11pt; color:#666666;"><%=Resources.Lang.STR_APPLY_COUNT %></span></td>
                                                    <td width="200" height="50" align="left">
                                                        <font color="#666666">
                                                            <select id="select_apply_count" style="font-size:11pt; color:rgb(51,51,51); border-width:1px;">
                                                                <option value="1" >1</option>
                                                                <option value="2" >2</option>
                                                                <option value="3" >3</option>
                                                            </select>
                                                        </font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150" height="50"><span style="font-size:11pt; color:#666666;"><%=Resources.Lang.STR_PRODUCT_TYPE %></span></td>
                                                    <td width="200" height="50" align="left">
                                                        <font color="#666666">
                                                            <select id="select_product_type" style="font-family:돋움; font-size:11pt; color:rgb(51,51,51); border-width:1px;">
                                                                <option value="1" ><%=Resources.Lang.STR_REAL_PRODUCT %></option>
                                                                <option value="2" ><%=Resources.Lang.STR_TEST_PRODUCT %></option>
                                                            </select>
                                                        </font>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="220">
                                            <table cellpadding="0" cellspacing="0" align="center" width="200">
                                                <tr>
                                                    <td width="200" height="200" style="border:1px solid black;" align="center">
                                                        <div id="divQRCodeCanvas"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" width="300" align="center">
                                    <tr>
                                        <td width="150" height="100" align="center">
                                            <input type="button" class="btnMiddle" onclick="ChangeQRCode()" value="<%=Resources.Lang.STR_CHANGE %>" style="width:110px; height:32px;"/>
                                        </td>
                                        <td width="150" height="100" align="center">
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
                                            <span style="font-size:12pt; font-weight:bold; color:White;"><%=Resources.Lang.STR_SAVE_EXCEL%></span>
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
                                                <input type="radio" name="optSaveExcelType" value="1" checked="checked" /><label><%=Resources.Lang.MSG_SAVE_CURRENT_LIST%></label> 
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" height="70">
                                        </td>
                                        <td width="309" height="70">
                                            <span style="font-size:11pt; color:#FF0000;">
                                                <input type="radio" name="optSaveExcelType" value="2" /><label><%=Resources.Lang.MSG_SAVE_ALL_LIST%></label>
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
