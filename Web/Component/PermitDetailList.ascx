<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.PermitDetailList" Codebehind="PermitDetailList.ascx.cs" %>
<%@ Register Src="AppSpecInfoTableView.ascx" TagName="AppSpecInfoTableView" TagPrefix="uc2" %>
<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="div_parent_detail">
    <table role='presentation' id="TBPermitDetailTest" runat="server" class="table_parent_detail">
    </table>
</div>

<div id="tbMoreDetail" runat="server">
    <table role='presentation' border="0" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="3" style="height: 24px">
                <h1 class="ACA_FLeftForStyle">
                    <a href="javascript:void(0);" class="NotShowLoading" onclick='ControlDisplay($get("TRMoreDetail"),$get("imgMoreDetail"),false,$get("lnkMoreDetail"),$get("<%=lblMoreDetail.ClientID %>"))'
                     title="<%=GetTitleByKey("img_alt_expand_icon", "per_permitConfirm_label_moreDetail") %>" id="lnkMoreDetail">
                    <img  style="cursor: pointer; border-width:0px;" alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                        src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("caret_collapsed.gif") %>"
                        id="imgMoreDetail" /></a><ACA:AccelaLabel ID="lblMoreDetail" runat="server" IsDisplayLabel="True"
                            IsNeedEncode="False" LabelType="LabelText" LabelKey="per_permitConfirm_label_moreDetail" /></h1>
            </td>
        </tr>
        <tr id="TRMoreDetail" style="display: none;">
            <td class="moredetail_td">
                &nbsp;</td>
            <td colspan="2" style="height: 24px;">
                <div style="text-align: center;">
                    <span id="tbRCList" runat="server">
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                            <tr>
                                <td class="MoreDetail_BlockTitle">
                                    <h1>
                                       <a href="javascript:void(0);" onclick='ControlDisplay($get("trRCList"),$get("imgRc"),true,$get("lnkRc"),$get("<%=lblRelatContact.ClientID %>"))' 
                                       title="<%=GetTitleByKey("img_alt_expand_icon", "per_permitConfirm_label_relatedContacts") %>" id="lnkRc" class="NotShowLoading">
                                       <img style="cursor: pointer; border-width:0px;" alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>"
                                            id="imgRc" /></a>&nbsp;<ACA:AccelaLabel ID="lblRelatContact" runat="server" IsDisplayLabel="True"
                                                IsNeedEncode="False" LabelType="LabelText" LabelKey="per_permitConfirm_label_relatedContacts" /></h1>
                                </td>
                            </tr>
                            <tr id="trRCList" style="display: none">
                                <td class="MoreDetail_BlockContent">
                                    <asp:DataList ID="RelatContactList" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" role='presentation'>
                                        <ItemTemplate>
                                            <%#Eval(" ContactContent")%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ACA_Table_Align_Top"/>
                                    </asp:DataList></td>
                            </tr>
                        </table>
                    </span><span id="tbADIList" runat="server">
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                            <tr>
                                <td class="MoreDetail_BlockTitle">
                                    <h1>
                                        <a href="javascript:void(0)" class="NotShowLoading" onclick='ControlDisplay($get("trADIList"),$get("imgAddtional"),true,$get("lnkAddtional"),$get("<%=lblAddtionalInfor.ClientID %>"))'
                                         title="<%=GetTitleByKey("img_alt_expand_icon", "per_permitConfirm_label_description") %>" id="lnkAddtional">
                                        <img style="cursor: pointer; border-width:0px;"  alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>"
                                            id="imgAddtional" /></a>&nbsp;<ACA:AccelaLabel ID="lblAddtionalInfor" runat="server"
                                                IsDisplayLabel="True" IsNeedEncode="False" LabelType="LabelText"
                                                LabelKey="per_permitConfirm_label_description"></ACA:AccelaLabel></h1>
                                </td>
                            </tr>
                            <tr id="trADIList" style="display: none">
                                <td class="MoreDetail_BlockContent" id="tdADIContent" runat="server">
                                </td>
                            </tr>
                        </table>
                    </span><span id="tbASIList" runat="server">
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                            <tr>
                                <td class="MoreDetail_BlockTitle">
                                    <h1>
                                        <a href="javascript:void(0);" onclick='ControlDisplay($get("trASIList"),$get("imgASI"),true,$get("lnkASI"),$get("<%=lblASIList.ClientID %>"))'
                                         title="<%=GetTitleByKey("img_alt_expand_icon", "per_permitConfirm_label_appSpecInfo") %>" id="lnkASI" class="NotShowLoading">
                                        <img style="cursor: pointer; border-width:0px;"  alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>"
                                            id="imgASI" /></a>&nbsp;<ACA:AccelaLabel ID="lblASIList" runat="server" IsDisplayLabel="True"
                                                IsNeedEncode="False" LabelType="LabelText" LabelKey="per_permitConfirm_label_appSpecInfo"></ACA:AccelaLabel></h1>
                                </td>
                            </tr>
                            <tr id="trASIList" style="display: none;">
                                <td class="MoreDetail_BlockContent">
                                    <asp:Panel ID="phPlumbingGroup" runat="server">
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </span><span id="tbASITList" runat="server">
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="width: 98%;">
                            <tr>
                                <td class="MoreDetail_BlockTitle">
                                    <h1>
                                        <a href="javascript:void(0);" onclick='ControlDisplay($get("trASITList"),$get("imgASIT"),true,$get("lnkASITableList"),$get("<%=lblASITableList.ClientID %>"))'
                                        title="<%=GetTitleByKey("img_alt_expand_icon", "per_permitConfirm_label_appSpecInfoTable") %>" id="lnkASITableList" class="NotShowLoading">
                                        <img  style="cursor: pointer; border-width:0px;"  alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>"
                                            id="imgASIT" /></a>&nbsp;<ACA:AccelaLabel ID="lblASITableList" runat="server" IsDisplayLabel="True"
                                                IsNeedEncode="False" LabelType="LabelText" LabelKey="per_permitConfirm_label_appSpecInfoTable"></ACA:AccelaLabel></h1>
                                </td>
                            </tr>
                            <tr id="trASITList" style="display: none">
                                <td class="MoreDetail_BlockContent">
                                    <asp:PlaceHolder ID="pnlASITable" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
                        </table>
                    </span><span id="tbParcelList" runat="server">
                        <table role='presentation' border="0" cellpadding="0" cellspacing="0" style="width: 98%;">
                            <tr>
                                <td class="MoreDetail_BlockTitle">
                                    <h1>
                                        <a href="javascript:void(0);" onclick='ControlDisplay($get("trParcelList"),$get("imgParcel"),true,$get("lnkParcelList"),$get("<%=lblParcelList.ClientID %>"))'
                                        title="<%=GetTitleByKey("img_alt_expand_icon", "per_permitConfirm_label_parcel") %>" id="lnkParcelList" class="NotShowLoading">
                                        <img  style="cursor: pointer; border-width:0px;"  alt="<%=GetTextByKey("img_alt_expand_icon") %>"
                                            src="<%=Accela.ACA.Web.Common.ImageUtil.GetImageURL("plus_expand.gif") %>"
                                            id="imgParcel" /></a>&nbsp;<ACA:AccelaLabel ID="lblParcelList" runat="server" IsDisplayLabel="True"
                                                IsNeedEncode="False" LabelType="LabelText" LabelKey="per_permitConfirm_label_parcel"></ACA:AccelaLabel></h1>
                                </td>
                            </tr>
                            <tr id="trParcelList" style="display: none">
                                <td class="MoreDetail_BlockContent">
                                    <asp:Panel ID="palParceList" runat="server">
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </span>
                </div>
            </td>
        </tr>
    </table>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<script language="javascript" type="text/javascript">
    var oldALHeight = "16px";
    var oldALPHeight = "16px";
    function DisplayAdditionInfo(name, obj, tblID) {
        var table = document.getElementById(tblID);
        var title = obj.innerHTML;
        var hideAL = '<%=GetTextByKey("aca_permitDetail_label_hideAdditionalLocations")%>';
        var showAL = '<%=GetTextByKey("aca_permitDetail_label_showAdditionalLocations")%>';
        var hideALP = '<%=GetTextByKey("aca_permitDetail_label_hideAdditionalLPs")%>';
        var showALP = '<%=GetTextByKey("aca_permitDetail_label_showAdditionalLPs")%>';

        var marginHeight = table.style.marginBottom;
        if (title == showAL) {
            if (marginHeight != oldALHeight && marginHeight == "16px") {
                oldALHeight = marginHeight;
            }
            table.style.marginBottom = "16px";
            obj.innerHTML = hideAL;
            SetTrStyle(name, "block");
        } else if (title == hideAL) {
            table.style.marginBottom = oldALHeight;
            obj.innerHTML = showAL;
            SetTrStyle(name, "none");
        } else if (title == showALP) {
            if (marginHeight != oldALPHeight && marginHeight == "16px") {
                oldALPHeight = marginHeight;
            }
            table.style.marginBottom = "16px";
            obj.innerHTML = hideALP;
            SetTrStyle(name, "block");
        } else if (title == hideALP) {
            table.style.marginBottom = oldALPHeight;
            obj.innerHTML = showALP;
            SetTrStyle(name, "none");
        }

    }
    function SetTrStyle(name, css) {
        var trs = document.getElementsByTagName("tr");
        for (var i = 0; i < trs.length; i++) {
            var title = trs[i].getAttribute("tips");
            if (title == name) {
                trs[i].style.display = css == "block" ? "table-row" : "none";
            }
        }
    }


    function hideControls(controlIDs) {
        if (controlIDs && controlIDs.length > 0) {
            var prefix = '<%=TBPermitDetailTest.ClientID %>'.replace('TBPermitDetailTest', '');

            for (var i = 0; i < controlIDs.length; i++) {
                var clientID = prefix + controlIDs[i];
                document.getElementById(clientID).style.display = "none";
            }

            var displayMoreDetail = "none";
            var moreDetails = ['tbRCList', 'tbADIList', 'tbASIList', 'tbASITList', 'tbParcelList'];

            //Display "More Details" section if one or more items show.
            for (var i = 0; i < moreDetails.length; i++) {
                if (document.getElementById(prefix + moreDetails[i]).style.display != "none") {
                    displayMoreDetail = "block";
                    break;
                }
            }

            document.getElementById('<%=tbMoreDetail.ClientID %>').style.display = displayMoreDetail;
        }
    }

    // Expand More Details in Admin
    if ($.global.isAdmin) {
        ControlDisplay($get("TRMoreDetail"), $get("imgMoreDetail"), false, $get("lnkMoreDetail"), $get("<%=lblMoreDetail.ClientID %>"));
    }

</script>

