<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="DataManager.aspx.cs" Inherits="Web.Manager.DataManager" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(2);
            setSubMenu(1);

            $("#btnDataLink").bind("change", function () {
                var file = this.files[0];
                if (file) {
                    //확장자체크
                    var imgExtension = ($("#btnDataLink").val().substr($("#btnDataLink").val().length - 3)).toLowerCase();
                    if (imgExtension != 'zip') {
                        //파일형식이 바르지 않습니다.
                        alert("<%=Resources.Lang.MSG_FILE_FORMAT_ERROR %>");
                        return;
                    }

                    //용량체크
                    var fileSize = file.size;
                    var size = fileSize / 1053317.6;
                    if (size > 100) {
                        //100MB 이하의 파일만 업로드가능합니다.
                        alert("<%=Resources.Lang.MSG_FILE_SIZE_ERROR %>");
                        return;
                    }

                    //파일업로드
                    showProgress();
                    var data = new FormData();
                    var file = document.getElementById("btnDataLink");
                    data.append("upfile", file.files[0]);
                    $.ajax({
                        url: 'FileUpload.aspx?type=3',
                        type: "post",
                        data: data,
                        processData: false,
                        contentType: false,
                        success: function (data, textStatus, jqXHR) {
                            if (data != "") {
                                setTimeout(function () {
                                    $("#txtDataSize").val(fileSize); 
                                    $("#txtDataLink1").val(decodeURIComponent(data));
                                    hideProgress();
                                }, 100);
                            } else {
                                hideProgress();
                                //데이터파일 등록과정에 오류가 발생하였습니다.
                                alert("<%=Resources.Lang.MSG_FILE_REG_ERROR %>");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            hideProgress();
                            //데이터파일 과정에 오류가 발생하였습니다.
                            alert("<%=Resources.Lang.MSG_FILE_REG_ERROR %>");
                        }
                    });
                    //파일컨트롤 초기화
                    $("#btnDataLink").replaceWith($("#btnDataLink").clone(true));
                    return;
                }
            });
        });

        function dataAdd() {
            if ($("#txtDataID").val() == "") {
                //데이터식별코드를 입력해 주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_DATA_ID %>");
                return;
            }
            if ($("#txtDataVersion").val() == "") {
                //데이터버전을 입력해 주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_DATA_VERSION %>");
                return;
            }
            if ($("#txtDataLink1").val() == "") {
                //데이터링크를 등록해 주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_DATA_LINK %>");
                return;
            }

            //데이터추가
            var data_id = $("#txtDataID").val();
            var data_type = $("#select_data_type option:selected").val();
            var data_version = $("#txtDataVersion").val();
            var data_size = $("#txtDataSize").val();
            var data_description = $("#txtDataDescription").val();
            var data_link1 = $("#txtDataLink1").val();
            var data_link2 = $("#txtDataLink2").val();
            var data_link3 = $("#txtDataLink3").val();
            var data_link4 = $("#txtDataLink4").val();
            var data_link5 = $("#txtDataLink5").val();

            showProgress();
            var data = new FormData();
            data.append("data_id", encodeURIComponent(data_id));
            data.append("data_type", encodeURIComponent(data_type));
            data.append("data_version", encodeURIComponent(data_version));
            data.append("data_size", encodeURIComponent(data_size));
            data.append("data_description", encodeURIComponent(data_description));
            data.append("data_link1", encodeURIComponent(data_link1));
            data.append("data_link2", encodeURIComponent(data_link2));
            data.append("data_link3", encodeURIComponent(data_link3));
            data.append("data_link4", encodeURIComponent(data_link4));
            data.append("data_link5", encodeURIComponent(data_link5));

            $.ajax({
                url: 'AddData.aspx',
                type: "post",
                data: data,
                processData: false,
                contentType: false,
                success: function (data, textStatus, jqXHR) {
                    if (data == "0") {
                        setTimeout(function () {
                            closePopup();
                            //데이터를 정확히 등록하였습니다.
                            alert("<%=Resources.Lang.MSG_DATA_REG_SUCCESS %>");
                            location.reload(true);
                        }, 100);
                    } else if (data == "1") {
                        hideProgress();
                        $("#txtDataLink1").val("")
                        //데이터가 이미 등록되어 있습니다.
                        alert("<%=Resources.Lang.MSG_DATA_ALREADY_REG %>");
                    } else if (data == "2") {
                        hideProgress();
                        //데이터 등록과정에 오류가 발생하였습니다.
                        alert("<%=Resources.Lang.MSG_DATA_REG_ERROR %>");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    hideProgress();
                    //데이터 등록과정에 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_DATA_REG_ERROR %>");
                }
            });
        }

        function dataModify() {
            if ($("#txtDataVersion").val() == "") {
                //데이터버전을 입력해 주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_DATA_VERSION %>");
                return;
            }
            if ($("#txtDataLink1").val() == "") {
                //데이터링크를 등록해 주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_DATA_LINK %>");
                return;
            }

            var id = $("#hd_id").val();
            var data_type = $("#select_data_type option:selected").val();
            var data_version = $("#txtDataVersion").val();
            var data_size = $("#txtDataSize").val();
            var data_description = $("#txtDataDescription").val();
            var data_link1 = $("#txtDataLink1").val();
            var data_link2 = $("#txtDataLink2").val();
            var data_link3 = $("#txtDataLink3").val();
            var data_link4 = $("#txtDataLink4").val();
            var data_link5 = $("#txtDataLink5").val();

            showProgress();
            var data = new FormData();
            data.append("id", encodeURIComponent(id));
            data.append("data_type", encodeURIComponent(data_type));
            data.append("data_version", encodeURIComponent(data_version));
            data.append("data_size", encodeURIComponent(data_size));
            data.append("data_description", encodeURIComponent(data_description));
            data.append("data_link1", encodeURIComponent(data_link1));
            data.append("data_link2", encodeURIComponent(data_link2));
            data.append("data_link3", encodeURIComponent(data_link3));
            data.append("data_link4", encodeURIComponent(data_link4));
            data.append("data_link5", encodeURIComponent(data_link5));

            $.ajax({
                url: 'ModifyData.aspx',
                type: "post",
                data: data,
                processData: false,
                contentType: false,
                success: function (data, textStatus, jqXHR) {
                    if (data == "0") {
                        setTimeout(function () {
                            closePopup();
                            //데이터를 정확히 수정하였습니다.
                            alert("<%=Resources.Lang.MSG_DATA_CHANGE_SUCCESS %>");
                            location.reload(true);
                        }, 100);
                    } else if (data == "1") {
                        hideProgress();
                        //수정할 데이터가 존재하지 않습니다.
                        alert("<%=Resources.Lang.MSG_CHANGE_DATA_NO_EXIST %>");
                    } else if (data == "2") {
                        hideProgress();
                        //데이터 수정과정에 오류가 발생하였습니다.
                        alert("<%=Resources.Lang.MSG_DATA_CHANGE_ERROR %>");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    hideProgress();
                    //데이터 수정과정에 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_DATA_CHANGE_ERROR %>");
                }
            });
        }

        function OnBrowse(obj) {
            $("#" + obj).click();
        }

        function get_chked_values() {
            var chked_val = "";
            $(":checkbox[name='chkNo']:checked").each(function (pi, po) {
                chked_val += "," + po.value;
            });
            if (chked_val != "") chked_val = chked_val.substring(1);
            return chked_val;
        }

        function showAddPopup() {
            //데이터추가
            $("#spTitle").html("<%=Resources.Lang.MSG_DATA_ADD %>");
            $("#bt_s_add").css("display", "");
            showPopup('divDataAddModifyPopup');
        }

        function showModifyPopup(id) {
            $("#hd_id").val(id);

            //데이터수정
            $("#spTitle").html("<%=Resources.Lang.MSG_DATA_CHANGE %>");
            $("#bt_s_modify").css("display", "");

            $.ajax({
                url: "GetDataInfo.aspx?id=" + id,
                dataType: 'json',
                async: true,
                type: 'POST',
                success: function (data) {
                    var data_id = decodeURIComponent(data.data_id).replace(/\+/gi, " ");
                    var data_type = decodeURIComponent(data.data_type).replace(/\+/gi, " ");
                    var data_version = decodeURIComponent(data.data_version).replace(/\+/gi, " ");
                    var data_size = decodeURIComponent(data.data_size).replace(/\+/gi, " ");
                    var data_description = decodeURIComponent(data.data_description).replace(/\+/gi, " ");
                    var data_link1 = decodeURIComponent(data.data_link1).replace(/\+/gi, " ");
                    var data_link2 = decodeURIComponent(data.data_link2).replace(/\+/gi, " ");
                    var data_link3 = decodeURIComponent(data.data_link3).replace(/\+/gi, " ");
                    var data_link4 = decodeURIComponent(data.data_link4).replace(/\+/gi, " ");
                    var data_link5 = decodeURIComponent(data.data_link5).replace(/\+/gi, " ");

                    $("#txtDataID").val(data_id);
                    $("#txtDataID").attr("readonly", "readonly");
                    $("#select_data_type option[value=" + data_type + "]").prop("selected", true);
                    $("#select_data_type").attr("disabled", "disabled");
                    $("#txtDataVersion").val(data_version);
                    $("#txtDataSize").val(data_size);
                    $("#txtDataDescription").val(data_description);
                    $("#txtDataLink1").val(data_link1);
                    $("#txtDataLink2").val(data_link2);
                    $("#txtDataLink3").val(data_link3);
                    $("#txtDataLink4").val(data_link4);
                    $("#txtDataLink5").val(data_link5);

                    showPopup('divDataAddModifyPopup');
                    return;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //알림
                    $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                    //데이터정보 조회중 오류가 발생하였습니다. 
                    $("#lbl_message").html("<%=Resources.Lang.MSG_DATA_INFO_ERROR %>");
                    showPopup("divPopupAlert");
                    return;
                }
            });
        }

        function showDelPopup() {
            var str_ids = get_chked_values();
            if (str_ids == "") {
                //알림
                $("#lbl_title").html("<%=Resources.Lang.STR_ALERT %>");
                //삭제할 데이터를 선택해주십시오.
                $("#lbl_message").html("<%=Resources.Lang.MSG_DATA_DELETE_CHECK %>");
                showPopup("divPopupAlert");
                return;
            }
            showPopup("divDelConfirmAlert");
            return;
        }

        function DelData() {
            var str_ids = get_chked_values();

            showProgress();
            $.ajax({
                url: "DelData.aspx?ids=" + encodeURIComponent(str_ids),
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //삭제확인
                        $("#lbl_title").html("<%=Resources.Lang.STR_DELETE_CONFIRM %>");
                        //선택한 데이터가 정확히 삭제되었습니다. <br><br>
                        $("#lbl_message").html("<%=Resources.Lang.STR_DELETE_CONFIRM %>");
                        showPopup("divPopupAlert");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //데이터삭제중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_DATA_DELETE_ERROR %>");
                    closePopup();
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
                <table cellpadding="0" cellspacing="0" width="1080" align="center">
                    <tr>
                        <td width="289" height="39"></td>
                        <td width="796" height="39">&nbsp;</td>
                     </tr>
                     <tr>
                        <td width="289" height="45">
                            <span style="font-size:14pt; font-weight:bold; color:#BA4241;"><%=Resources.Lang.STR_DATA_LIST %></span>
                        </td>
                        <td width="796" height="45">
                            <div align="right">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="60" height="38" align="right">
                                            <input type="button" class="btnSmallGreen" onclick="showAddPopup()" value="<%=Resources.Lang.STR_ADD %>" style="width:58px; height:23px;"/>
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
                <asp:GridView ID="gvContent" runat="server" AllowSorting="false" Width="1080" GridLines="None" AutoGenerateColumns="false"
                    OnRowDataBound="gvContent_RowDataBound" OnPageIndexChanging="gvContent_PageIndexChange" CssClass="clsGrid" HorizontalAlign="Center" AllowPaging="True">
                    <Columns>
                        <asp:TemplateField HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <HeaderTemplate>
                                <input type="checkbox" style="height: auto;" value="All" onclick="checkAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrCheckbox" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_REG_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DATA_ID %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDataID" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DATA_VERSION %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDataVersion" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DATA_SIZE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDataSize" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DATA_LINK %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDataLink" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="300" Wrap="true" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DATA_LINK %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDataDescription" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DATA_TYPE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDataType" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_CHANGE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrModify" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="60" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="clsGridHeader" Height="40px"/>
                    <RowStyle CssClass="clsGrid" Height="100px" />
                    <SelectedRowStyle CssClass="clsSelGrid" Height="100px" />
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
                <table cellpadding="0" cellspacing="0" width="1080" align="center">
                    <tr>
                        <td height="40"></td>
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
                                            <input type="button" class="btnMiddle" onclick="DelData()" value="<%=Resources.Lang.STR_YES %>" style="width:110px; height:32px;"/>
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

    <div class="clspopup" id="divDataAddModifyPopup">
        <input id="hd_id" type="hidden" value=""/>
        <table cellpadding="0" cellspacing="0" width="950" background="/img/pop_bg6.png">
            <tr>
                <td width="950" height="730" valign="top">
                    <table cellpadding="0" cellspacing="0" width="909" align="center">
                        <tr>
                            <td width="909" height="70">
                                <table cellpadding="0" cellspacing="0" width="884" align="center">
                                    <tr>
                                        <td width="416" height="35">
                                            <span id="spTitle" style="font-size:12pt; font-weight:bold; color:White;"></span>
								        </td>
                                        <td width="468" height="35" align="right">
                                            <img onclick="confirm()" src="/img/bt_close.png" width="22" height="22" border="0" alt="" style="cursor:pointer;"/>
								        </td>
							        </tr>
						        </table>
					        </td>
                        </tr>
                        <tr>
                            <td width="909" height="20"></td>
                        </tr>
                        <tr>
                            <td width="909" height="517" valign="top">
                                <table cellpadding="0" cellspacing="0" align="center" width="800" style="border:1px solid #da4453;">
                                    <tr height="30">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_ID %> <font color="red">*&nbsp;</font></span>
                                                    </td>
                                                    <td width="230" align="center" style="border:1px solid #da4453;">
                                                        <input type="text" id="txtDataID" maxlength="10" placeholder="<%=Resources.Lang.STR_LIMIT_10%>" value="" style="width:200px; border-style:none;"/>
                                                    </td>
                                                    <td width="120" align="center">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_TYPE %></span>
                                                    </td>
                                                    <td width="230" align="left">
                                                        <select id="select_data_type" size="1" style="font-size:10pt; color:Black; border:1px solid #da4453; width:185px; height:30px;">
                                                            <option value="0" selected="selected">Android</option>
                                                            <option value="1">IOS</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_VERSION%> <font color="red">*&nbsp;</font></span>
                                                    </td>
                                                    <td width="230" align="center" style="border:1px solid #da4453;">
                                                        <input type="text" id="txtDataVersion" maxlength="10" placeholder="<%=Resources.Lang.STR_LIMIT_10%>" value="" style="width:200px; border-style:none;"/>
                                                    </td>
                                                    <td width="120" align="center">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_SIZE%></span>
                                                    </td>
                                                    <td width="180" align="center" style="border:1px solid #da4453;">
                                                        <input type="text" id="txtDataSize" value="" style="width:150px; border-style:none;" readonly="readonly"/>
                                                    </td>
                                                    <td width="50" align="center">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_BYTE%></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="100">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="80">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_DESCRIPTION%></span>
                                                    </td>
                                                    <td width="580" align="center" style="border:1px solid #da4453;" colspan="3">
                                                        <textarea id="txtDataDescription" rows="4" cols="40" maxlength="200" placeholder="<%=Resources.Lang.STR_LIMIT_200%>" style="width:550px; border-style:none; resize:none;"></textarea>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_LINK%>1 <font color="red">*&nbsp;</font></span>
                                                        <br /><span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_CLOUD%></span>
                                                    </td>
                                                    <td width="460" align="center" style="border:1px solid #da4453;" colspan="3">
                                                        <input type="text" id="txtDataLink1" maxlength="200" readonly="readonly" value="" style="width:450px; border-style:none;"/>
                                                    </td>
                                                    <td width="120" align="center">
                                                        <input id="btnDataLink" type="file" name="btnDataLink" accept=".zip"  style="height:20px; width:20px;display:none;"/>
                                                        <input id="btnDataLinkBrowse" type="button" class="btnMiddle" onclick="javascript:OnBrowse('btnDataLink')" value="<%=Resources.Lang.STR_BROWSE %>" style="width:100px; height:27px;"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="30">
                                        <td align="center">
                                            <span style="font-size:10pt; color:Red;"><%=Resources.Lang.MSG_UPLOAD_WARNING%></span> 
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_LINK%>2 <font color="red">*&nbsp;</font></span>
                                                        <br /><span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_CLOUD%></span>
                                                    </td>
                                                    <td width="580" align="center" style="border:1px solid #da4453;" colspan="3">
                                                        <input type="text" id="txtDataLink2" maxlength="200" placeholder="<%=Resources.Lang.STR_LIMIT_200%>" value="" style="width:550px; border-style:none;"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_LINK%>3 <font color="red">*&nbsp;</font></span>
                                                        <br /><span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_CLOUD%></span>
                                                    </td>
                                                    <td width="580" align="center" style="border:1px solid #da4453;" colspan="3">
                                                        <input type="text" id="txtDataLink3" maxlength="200" placeholder="<%=Resources.Lang.STR_LIMIT_200%>" value="" style="width:550px; border-style:none;"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_LINK%>4 <font color="red">*&nbsp;</font></span>
                                                        <br /><span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_CLOUD%></span>
                                                    </td>
                                                    <td width="580" align="center" style="border:1px solid #da4453;" colspan="3">
                                                        <input type="text" id="txtDataLink4" maxlength="200" placeholder="<%=Resources.Lang.STR_LIMIT_200%>" value="" style="width:550px; border-style:none;"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="50">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" align="center" width="720">
                                                <tr height="30">
                                                    <td width="120" align="left">
                                                        <span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_LINK%>5 <font color="red">*&nbsp;</font></span>
                                                        <br /><span style="font-size:10pt; font-weight:bold; color:Black;"><%=Resources.Lang.STR_DATA_CLOUD%></span>
                                                    </td>
                                                    <td width="580" align="center" style="border:1px solid #da4453;" colspan="3">
                                                        <input type="text" id="txtDataLink5" maxlength="200" placeholder="<%=Resources.Lang.STR_LIMIT_200%>" value="" style="width:550px; border-style:none;"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="30">
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="890" height="80" align="center">
                                <table cellpadding="0" cellspacing="0" width="400" align="center">
                                    <tr>
                                        <td width="200" height="50" align="center">
                                            <input id="bt_s_add" type="button" class="btnMiddle" onclick="dataAdd()" value="<%=Resources.Lang.STR_ADD %>" style="width:110px; height:32px; display:none;"/>
                                            <input id="bt_s_modify" type="button" class="btnMiddle" onclick="dataModify()" value="<%=Resources.Lang.STR_CHANGE %>" style="width:110px; height:32px; display:none;"/>
                                        </td>
                                        <td width="200" height="50" align="center">
                                            <input type="button" class="btnMiddle" onclick="confirm()" value="<%=Resources.Lang.STR_CANCEL %>" style="width:110px; height:32px;"/>
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
