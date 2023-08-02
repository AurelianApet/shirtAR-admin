<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="Banner.aspx.cs" Inherits="Web.Manager.Banner" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(2);
            setSubMenu(3);

            $("#btnBannerPath").bind("change", function () {
                var file = this.files[0];
                if (file) {
                    //확장자체크
                    var imgExtension = ($("#btnBannerPath").val().substr($("#btnBannerPath").val().length - 3)).toLowerCase();
                    if (imgExtension != 'jpg' && imgExtension != 'png') {
                        //파일형식이 바르지 않습니다.
                        alert("<%=Resources.Lang.MSG_FILE_FORMAT_ERROR %>");
                        return;
                    }

                    //용량체크
                    //var file = document.getElementById("btnImgUp");
                    var size = file.size / 1053317.6;
                    if (size > 5) {
                        //5MB 이하의 파일만 업로드가능합니다.
                        alert("<%=Resources.Lang.MSG_FILE_SIZE_ERROR_5 %>");
                        return;
                    }

                    //이미지사이즈체크
                    var _URL = window.URL || window.webkitURL;
                    var img = new Image();
                    img.onload = function () {
                        if (this.width > 512) {
                            //가로 512픽셀이하 이미지만 업로드가능합니다.
                            alert("<%=Resources.Lang.MSG_FILE_SIZE_ERROR_512 %>");
                            $("#btnImgUp").val("");
                            $("#<%=tbxBannerPath.ClientID %>").val($("#btnBannerPath").val());
                            return;
                        }
                        if (this.height > 910) {
                            //세로 910픽셀이하 이미지만 업로드가능합니다.
                            alert("<%=Resources.Lang.MSG_FILE_SIZE_ERROR_910 %>");
                            $("#btnImgUp").val("");
                            $("#<%=tbxBannerPath.ClientID %>").val($("#btnBannerPath").val());
                            return;
                        }

                        //파일업로드
                        showProgress();
                        var data = new FormData();
                        var file = document.getElementById("btnBannerPath");
                        data.append("upfile", file.files[0]);
                        $.ajax({
                            url: 'FileUpload.aspx?type=1',
                            type: "post",
                            data: data,
                            processData: false,
                            contentType: false,
                            success: function (data, textStatus, jqXHR) {
                                if (data != "") {
                                    setTimeout(function () {
                                        $("#<%=tbxBannerPath.ClientID %>").val(decodeURIComponent(data));
                                        $("#<%=divBannerPrev.ClientID %>").attr("src", decodeURIComponent(data));
                                        closePopup();
                                    }, 100);
                                } else {
                                    closePopup();
                                    //배너이미지 등록과정에 오류가 발생하였습니다.
                                    alert("<%=Resources.Lang.MSG_BANNER_REG_ERROR %>");
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                closePopup();
                                //배너이미지 등록과정에 오류가 발생하였습니다.
                                alert("<%=Resources.Lang.MSG_BANNER_REG_ERROR %>");
                            }
                        });
                        //파일컨트롤 초기화
                        $("#btnBannerPath").replaceWith($("#btnBannerPath").clone(true));
                        return;
                    };
                    img.onerror = function () {
                        alert("not a valid file: " + file.type);
                        $("#btnBannerPath").val("");
                        $("#<%=tbxBannerPath.ClientID %>").val($("#btnBannerPath").val());
                    };
                    img.src = _URL.createObjectURL(file);
                }
            });

            $('input[type=radio][name=optActivate]').change(function () {
                $("#<%= hd_optActivity.ClientID%>").val($("input:radio[name='optActivate']:checked").val());
            });

            $("#<%=tbxBannerPath.ClientID %>").attr("readonly", true);
            $('input:radio[name=optActivate][value=' + $("#<%= hd_optActivity.ClientID%>").val() + ']').attr('checked', true);
            $("#<%=divBannerPrev.ClientID %>").attr("src", $("#<%=tbxBannerPath.ClientID %>").val());
        });

        function setActivate(id) {
            var is_activate = $('#ddl_is_activate_' + id + ' option:selected').val();

            showProgress();
            var data = new FormData();
            data.append("id", encodeURIComponent(id));
            data.append("is_activate", encodeURIComponent(is_activate));
            $.ajax({
                url: 'ModifyBanner.aspx',
                type: "post",
                data: data,
                processData: false,
                contentType: false,
                success: function (data, textStatus, jqXHR) {
                    if (data == "0") {
                        setTimeout(function () {
                            closePopup();
                            if (is_activate == "1")
                                //선택한 배너가 활성화되었습니다.
                                alert("<%=Resources.Lang.MSG_BANNER_ACTIVATED %>");
                            else if (is_activate == "0")
                                //선택한 배너가 비활성화되었습니다.
                                alert("<%=Resources.Lang.MSG_BANNER_NO_ACTIVATED %>");
                            location.reload(true);
                        }, 100);
                    } else if (data == "1") {
                        hideProgress();
                        //선택한 배너 수정과정에 오류가 발생하였습니다.
                        alert("<%=Resources.Lang.MSG_CHANGE_ERROR %>");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    hideProgress();
                    //선택한 배너 수정과정에 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_CHANGE_ERROR %>");
                }
            });
        }

        function bannerCheck() {
            if ($("#<%=tbxBannerName.ClientID%>").val() == '') {
                //배너명을 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_BANNER_NAME %>");
                return false;
            }

            if ($("#<%=tbxBannerPath.ClientID%>").val() == '') {
                //배너이미지를 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_BANNER_IMAGE %>");
                return false;
            }

            if ($("#<%=tbxBannerLink.ClientID%>").val() == '') {
                //배너링크를 입력해주십시오.
                alert("<%=Resources.Lang.MSG_INPUT_BANNER_LINK %>");
                return false;
            }

            if ($("#<%=tbxBannerLink.ClientID%>").val().indexOf("http://") !== 0) {
                $("#<%=tbxBannerLink.ClientID%>").val("http://" + $("#<%=tbxBannerLink.ClientID%>").val());
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

        function DelBanner() {
            $.ajax({
                url: "DelBanner.aspx?id=" + encodeURIComponent($("#hd_id").val()),
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        location.reload(true);
                        return;
                    }
                    else if (data == "1") {
                        //배너삭제중 예상치않은 오류가 발생하였습니다.
                        alert("<%=Resources.Lang.MSG_BANNER_DELETE_ERROR %>");
                        return;                            
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //배너삭제중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_BANNER_DELETE_ERROR %>");
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
                            <span style="font-size:16pt; font-weight:bold; color:#BA4241;"><%=Resources.Lang.STR_BANNER_MANAGE %></span>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="1100" align="center">
                    <tr>
                        <td width="600" height="300">
                            <input id="hd_id" type="hidden" value=""/>
                            <table cellpadding="0" cellspacing="0" width="600" align="center">
                                <tr>
                                    <td width="100" height="50">
                                        <span style="font-size:11pt;"><%=Resources.Lang.STR_BANNER_NAME %></span>
                                    </td>
                                    <td width="500" height="50">
                                        <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                            <tr>
                                                <td width="500" height="25" align="center">          
                                                    <asp:TextBox ID="tbxBannerName" runat="server" TextMode="SingleLine" Width="450" MaxLength="50" style="border-width:1px; border-style:none;" placeholder="<%$Resources:Lang, STR_LIMIT_50 %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100" height="30">
                                        <span style="font-size:11pt;"><%=Resources.Lang.STR_BANNER_IMAGE %></span>
                                    </td>
                                    <td width="500" height="30">
                                        <table cellpadding="0" cellspacing="0" width="500">
                                            <tr>
                                                <td width="350" height="27">
                                                    <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" width="350" style="width:350px; border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                                        <tr>
                                                            <td width="350" height="25" align="center"> 
                                                                <asp:TextBox ID="tbxBannerPath" runat="server" TextMode="SingleLine" style="width:300px; border-width:1px; border-style:none;"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="150" height="27" align="center">
                                                    <input id="btnBannerPath" type="file" name="btnBannerPath" accept="image/jpeg, image/png"  style="height:20px; width:20px;display:none;"/>
                                                    <input type="button" class="btnMiddle" onclick="javascript:OnBrowse('btnBannerPath')" value="<%=Resources.Lang.STR_BROWSE %>" style="width:126px; height:27px;"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100" height="20">
                                    </td>
                                    <td width="500" height="20">
                                        <table cellpadding="0" cellspacing="0" width="500">
                                            <tr>
                                                <td width="500" height="20" align="center"> 
                                                    <span style="font-size:10pt; color:#666666;"><%=Resources.Lang.STR_BANNER_WARNING%></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100" height="50">
                                        <span style="font-size:11pt;"><%=Resources.Lang.STR_BANNER_LINK%></span>
                                    </td>
                                    <td width="500" height="50">
                                        <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                            <tr>
                                                <td width="500" height="25" align="center">
                                                    <asp:TextBox ID="tbxBannerLink" runat="server" TextMode="SingleLine" style="width:450px; border-width:1px; border-style:none;"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100" height="50">
                                        <span style="font-size:11pt;"><%=Resources.Lang.STR_BANNER_STATUS%></span>
                                    </td>
                                    <td width="500" height="50">
                                        <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                            <tr>
                                                <td width="500" height="25" align="center">
                                                    <asp:HiddenField ID="hd_optActivity" runat="server" Value="0" />
                                                    <table cellpadding="0" cellspacing="0" width="500" align="center">
                                                        <tr>
                                                            <td width="250" height="25" align="center">
                                                                <span style="font-size:11pt; font-weight:bold; color:#666666;"><input type="radio" name="optActivate" value="0" checked="checked" /><%=Resources.Lang.STR_NO_ACT%></span>
                                                            </td>
                                                            <td width="250" height="25" align="center">
                                                                <span style="font-size:11pt; font-weight:bold; color:#666666;"><input type="radio" name="optActivate" value="1" /><%=Resources.Lang.STR_ACT%></span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="600" height="40" align="right" colspan="2">
                                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" OnClientClick="return bannerCheck();" CssClass="btnMiddle" width="110" height="32" Text="<%$Resources:Lang, STR_ADD %>" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="500" height="300">
                            <table cellpadding="0" cellspacing="0" width="500" align="center">
                                <tr>
                                    <td width="400" height="230" align="center">
                                        <table cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" style="border-width:1px; border-color:rgb(218,68,83); border-style:solid; border-radius:5px;">
                                            <tr>
                                                <td width="400" height="230" align="center">
                                                    <asp:Image ID="divBannerPrev" runat="server" width="384" height="216" ImageUrl="/img/preview.png" BackColor="Gray" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
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
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_BANNER_NAME %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrBannerName" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_BANNER_IMAGE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrThumbnail" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="180" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_BANNER_LINK %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrBannerLink" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="320" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_BANNER_STATUS %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrIsActivity" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_REG_DATE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrRegDate" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Lang, STR_DELETE %>" HeaderStyle-BorderStyle="None" HeaderStyle-ForeColor="#FFFFFF">
                            <ItemTemplate>
                                <asp:Literal ID="ltrDelete" runat="server">
                                </asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="150" Wrap="false" BorderStyle="None"/>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="clsGrid" Height="100px" />
                    <SelectedRowStyle CssClass="clsSelGrid" Height="100px" />
                    <AlternatingRowStyle CssClass="clsSelGrid" />
                    <HeaderStyle CssClass="clsGridHeader" Height="30px"/>
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
                                            <input type="button" class="btnMiddle" onclick="DelBanner()" value="<%=Resources.Lang.STR_YES %>" style="width:110px; height:32px;"/>
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
