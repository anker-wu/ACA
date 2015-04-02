<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ContinuingEducationEdit"
    CodeBehind="ContinuingEducationEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="ContinuingEducationList.ascx" TagName="contEducationList" TagPrefix="uc2" %>
<%@ Register Src="ContinuingEducationSummaryList.ascx" TagName="summaryContEducationList"
    TagPrefix="uc3" %>
<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="ACA_Row">
            <div id="divAddButton" class="ACA_Row ACA_LiLeft" runat="server">
                <ul>
                    <li id="liAddFromSaved" runat="server">
                        <ACA:AccelaButton ID="btnAddFromSaved" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="False" LabelKey="aca_education_list_label_add_from_saved"
                            runat="server" />
                    </li>
                    <li id="liAddNew" runat="server">
                        <ACA:AccelaButton ID="btnAddNew" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" CausesValidation="False" runat="server" />
                    </li>
                </ul>
                <ACA:AccelaHeightSeparate ID="sepForAddButton" runat="server" Height="15" />
            </div>
            <div id="divContEducationDetailForm">
                <div id="divIncompleteMark" runat="server" visible="false">
                    <table role='presentation' class="ACA_Message_Error ACA_Message_Error_FontSize" style="width: 100%">
                        <tr>
                            <td>
                                <div class="ACA_Error_Icon">
                                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_24.gif") %>"/>
                                </div>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltScriptForIncomplete" Mode="PassThrough"></asp:Literal>
                                <ACA:AccelaLabel ID="lblIncomplete" runat="server" LabelKey="per_conteducationlist_required_validate_msg" />
                            </td>
                        </tr>
                    </table>
                    <ACA:AccelaHeightSeparate ID="sepForMark" runat="server" Height="15" />
                </div>
                <div id="divDuplicateRecord" runat="server" visible="false" class="ACA_TabRow">
                    <table role='presentation' class="ACA_Message_Error ACA_Message_Error_FontSize" style="width: 100%">
                        <tr>
                            <td>
                                <div class="ACA_Error_Icon">
                                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_24.gif") %>"/>
                                </div>
                            </td>
                            <td>
                                <ACA:AccelaLabel ID="lblDuplicateMessage" runat="server" LabelKey="continuing_education_validation_duplicate_message" />
                            </td>
                        </tr>
                    </table>
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" runat="server" Height="15" />
                </div>
                <div id="divSummaryList" runat="server">
                    <uc3:summaryContEducationList ID="SummaryContEducation" runat="server" />
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" runat="server" Height="5" />
                </div>
                <div class="ACA_Row">
                    <uc2:contEducationList ID="ContEducationList" GViewID="60083" runat="server" />
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
                </div>
                <asp:LinkButton ID="btnRefreshContEduList" runat="Server" CssClass="ACA_Hide" OnClick="RefreshContEduListButton_Click"
                    TabIndex="-1"></asp:LinkButton>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHeightSeparate ID="sepForContEducationEdit" runat="server" Height="25" Visible="False" />
<script language="javascript" type="text/javascript">
    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%= ContEducationList.ClientID %>");
    }
    
    // validate continuing education list.
    function ClientEvent4ContEducation()
    {
        if(typeof(SetNotAsk) != 'undefined')
        {
            SetNotAsk();
        }
    }
    
    // display continuing education section in ACA admin.
    function DisplayContEducation4Admin()
    {   
        $get("imgEditContEducation").onclick='return false;';
    }
                 
    // show edit form and list form.
    function ShowEditAndListForm()
    {
        ExpandCollapseContEducationList(true);
    }

    function openContEducationFormDialog(sender, isFromRefContact, opt, rowIndex, editable, contactIsFromExternal) {
        var url = '<%=FileUtil.AppendApplicationRoot("LicenseCertification/ContinuingEducationEdit.aspx") %>?opt=' + opt + '&rowIndex=' + rowIndex;
        
        if(isFromRefContact == "Y") {
            url += '&contactSeqNbr=<%=ContactSeqNbr %>&<%=UrlConstant.CONTACT_IS_FROM_EXTERNAL %>=' + contactIsFromExternal;
        }
        else {
            url += '&editable=' + editable + '&Module=<%=ModuleName %>&<%=UrlConstant.CONTACT_IS_FROM_EXTERNAL %>=' + contactIsFromExternal;
        }

        ACADialog.popup({ url: url, width: 800, height: 550, objectTarget: sender.id });
        return false;
    }

    function RefreshContEducationList(isUpdate) {
        window.setTimeout(function () {
            delayShowLoading();
        }, 100);

        __doPostBack('<%=btnRefreshContEduList.UniqueID %>', isUpdate);
    }

</script>
