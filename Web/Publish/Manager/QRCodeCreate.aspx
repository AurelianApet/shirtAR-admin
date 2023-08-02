<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="QRCodeCreate.aspx.cs" Inherits="Web.Manager.QRCodeCreate" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            setMainMenu(1);
            setSubMenu(1);
            var qrcode_count = $("#<%=ddl_LoginID.ClientID %> option:selected").val();
            $("#lbl_qrcode_count").html(qrcode_count);
        });

        function setDetailInfo() {
            var qrcode_count = $("#<%=ddl_LoginID.ClientID %> option:selected").val();
            $("#lbl_qrcode_count").html(qrcode_count);
        }

        function CreateSerial() {
            var id = $("#<%=ddl_LoginID.ClientID %> option:selected").text();
            var apply_count = $("#select_apply_count option:selected").val();
            var process_count = $("#select_process_count option:selected").val();
            var product_type = $("#select_product_type option:selected").val();

            showProgress();
            $.ajax({
                url: "Create.aspx?id=" + id + "&apply_count=" + apply_count + "&process_count=" + process_count + "&product_type=" + product_type,
                dataType: 'text',
                async: true,
                type: 'POST',
                success: function (data) {
                    if (data == "0") {
                        //생성확인
                        $("#lbl_title").html("<%=Resources.Lang.STR_CREATE_CONFIRM %>");
                        //QR코드번호생성이 완료되었습니다. <br><br> 생성한 QR코드목록에서 확인하실수 있습니다.
                        $("#lbl_message").html("<%=Resources.Lang.MSG_QRCODE_CREATE_SUCCESS %>");
                        showPopup("divPopupAlert");
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //QR코드번호생성중 예상치않은 오류가 발생하였습니다.
                    alert("<%=Resources.Lang.MSG_QRCODE_CREATE_ERROR %>");
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
                <table cellpadding="0" cellspacing="0" width="750" align="center">
                    <tr>
                        <td width="750" height="39">
                        </td>
                    </tr>
                    <tr>
                        <td width="750" height="107" align="center">                                                
                            <span style="font-size:16pt; font-weight:bold; color:#42485a;"><%=Resources.Lang.STR_QRCODE_CREATE%></span>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" align="center" width="450">
                    <tr>
                        <td width="50" height="56"></td>
                        <td width="250" height="56">                                    
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_CREATOR %></span>
                        </td>
                        <td width="150" height="56">
                            <span style="font-size:11pt; color:#333333;">
                                <asp:DropDownList ID="ddl_LoginID" runat="server" onchange="setDetailInfo()" Width="100"></asp:DropDownList>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td width="50" height="56"></td>
                        <td width="250" height="28">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_QRCODE_COUNT %></span>
                        </td>
                        <td width="150" height="56">
                            <span style="font-size:11pt; color:#333333;"><label id="lbl_qrcode_count"></label><%=Resources.Lang.STR_GAE %></span>
                        </td>
                    </tr>
                    <tr>
                        <td width="50" height="56"></td>
                        <td width="250" height="28">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_APPLY_COUNT %></span>
                        </td>
                        <td width="150" height="56" align="left">
                            <font color="#666666">
                                <select id="select_apply_count" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; width:100px;">
                                    <option value="1" >1</option>
                                    <option value="2" >2</option>
                                    <option value="3" selected="selected">3</option>
                                </select>
                            </font>
                        </td>
                    </tr>
                    <tr>
                        <td width="50" height="56"></td>
                        <td width="250" height="56">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_CREATE_PROCESS %></span>
                        </td>
                        <td width="150" height="56" align="left">
                            <font color="#666666">
                                <select id="select_process_count" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; width:100px;">
                                    <option value="1" selected="selected" >1</option>
                                    <option value="2" >2</option>
                                    <option value="3" >3</option>
                                    <option value="4" >4</option>
                                    <option value="5" >5</option>
                                    <option value="6" >6</option>
                                    <option value="7" >7</option>
                                    <option value="8" >8</option>
                                    <option value="9" >9</option>
                                    <option value="10">10</option>
                                </select>
                            </font>
                        </td>
                    </tr>
                    <tr>
                        <td width="50" height="56"></td>
                        <td width="250" height="56">
                            <span style="font-size:11pt; color:#333333;"><%=Resources.Lang.STR_PRODUCT_TYPE %></span>
                        </td>
                        <td width="150" height="56">
                            <table cellpadding="0" cellspacing="0" width="91">
                                <tr>
                                    <td width="91" height="32" align="left">
                                        <font color="#666666">
                                            <select id="select_product_type" style="font-size:11pt; color:rgb(51,51,51); border-width:1px; width:100px;">
                                                <option value="1" selected="selected" ><%=Resources.Lang.STR_REAL_PRODUCT %></option>
                                                <option value="2" ><%=Resources.Lang.STR_TEST_PRODUCT%></option>
                                            </select>
                                        </font>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="462" align="center">
                    <tr>
                        <td width="462" height="144" align="center">
                            <input type="button" class="btnMiddle" onclick="CreateSerial()" value="<%=Resources.Lang.STR_CREATE %>" style="width:129px; height:36px;"/>                                                
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
