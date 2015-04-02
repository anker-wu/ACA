<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.MultiContactsEdit" Codebehind="MultiContactsEdit.ascx.cs" %>
<%@ Register Src="~/Component/ContactList.ascx" TagName="ContactList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ContactEdit.ascx" TagName="ContactEdit" TagPrefix="uc2" %>
<%@ Register src="RequiredContactTypeIndicator.ascx" tagName="Indicator" tagPrefix="ACA" %>

<asp:UpdatePanel ID="MultiContactPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<ACA:AccelaLabel ID="errorMessageLabel" runat="server"></ACA:AccelaLabel>
<div style="width:770px">
    <div id="divActionNotice" runat="server" visible="false" >
        <div class="ACA_Error_Icon" EnableViewState="False" runat="server" id="divImgSuccess" visible="false">
            <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>"/>           
        </div>
        <div class="ACA_Error_Icon" EnableViewState="False" runat="server" id="divImgFailed" visible="false">
            <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>            
        </div>
        <div class="ACA_Notice font12px">
            <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_addedsuccessfully" Visible="false"/>
            <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_removedsuccessfully" Visible="false"/>
            <ACA:AccelaLabel ID="lblActionNoticeEditSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_editedsuccessfully" Visible="false"/>
            <ACA:AccelaLabel ID="lblActionNoticeAddFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_addedfailed" Visible="false"/>
            <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_removedfailed" Visible="false"/>
            <ACA:AccelaLabel ID="lblActionNoticeEditFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_editedfailed" Visible="false"/>
        </div>
    </div>
    <div class="ACA_MutipleContactNewLine">
        <asp:UpdatePanel ID="contactListPanel" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <ACA:Indicator runat="server" ID="contactTypeIndicator" IsShowIndicator="true" />
                <div>
                    <uc2:ContactEdit ID="contactEdit" runat="server" IsShowContactType="true" IsMultipleContact="true"/>
                </div>
                <div id="divContactList" runat="server">
                    <uc1:ContactList ID="ucContactList" runat="server" Location="SpearForm" GViewID ="60067" FocusElementId="lnkEditContact" IsShowAddressCount="True"/>
                    <asp:LinkButton ID="btnRefreshContactList" runat="Server" CssClass="ACA_Hide"
                        OnClick="RefreshContactListButton_Click" TabIndex="-1"></asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<ACA:AccelaHeightSeparate ID="sepForContactEdit" runat="server" Height="25" />
<asp:HiddenField ID="hfRefContactSeqNbr" runat="server" />
<asp:HiddenField ID="hdnIsClearRefContact" runat="server"/>
</ContentTemplate>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    AddValidationSectionID('<%=contactEdit.ClientID %>');

    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=ucContactList.ClientID %>");
    }

    function <%=contactEdit.ClientID %>_RefreshContactList(isFromAddress, refContactSeqNbr, isClearRefContact) {
        window.setTimeout(function () {
            delayShowLoading();
        }, 100);
        
        if (refContactSeqNbr != undefined) {
            $("#<%=hfRefContactSeqNbr.ClientID %>").val(refContactSeqNbr);
        }
        
        if (isClearRefContact != undefined) {
            $("#<%=hdnIsClearRefContact.ClientID %>").val(isClearRefContact);
        }

        __doPostBack('<%=btnRefreshContactList.UniqueID %>');
    }
</script>
