<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="ContactAddNew.aspx.cs" Inherits="Accela.ACA.Web.People.ContactAddNew" %>

<%@ Register Src="~/Component/ContactInfo.ascx" TagName="ContactInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script src="../Scripts/DisableForm.js" type="text/javascript"></script>
    <script src="../Scripts/Expression.js" type="text/javascript"></script>
    <script type="text/javascript">
        function PopupClose(isClearRefContact) {
            var parentFocusObj = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            SetParentLastFocus(parentFocusObj);
            RefreshContactList(isClearRefContact);
        }

        function RefreshContactList(isClearRefContact) {
            if (<%= IsMultipleContact.ToString().ToLower() %>) {
                parent.<%=ParentID %>_RefreshContactList(false, "<%=RefContactSeqNbr %>", isClearRefContact);
            } else {
                parent.<%=ParentID %>_Refresh(false, "<%=RefContactSeqNbr %>", isClearRefContact);
            }
        }
        
        function RefreshToAccountManager() {
            parent.ACADialog.close();
            parent.window.location.href = '<%= Page.ResolveUrl("~/Account/AccountManager.aspx") %>';
        }

        if (typeof (myValidationErrorPanel) != "undefined") {
            myValidationErrorPanel.registerIDs4Recheck("<%=btnCancel.ClientID %>");
        }
    
        function ClearContact() {
            var msg = '<%=GetTextByKey("aca_capedit_contact_msg_clearprimarycontact").Replace("'", "\\'") %>';
        
            if (confirm(msg)) {
                return true;
            } else {
                return false;
            }
        }
    </script>
    <div id="divContactInput" class="ContactInput">
        <div id="divContactInputForm" class="ContactInputForm">
            <uc1:ContactInfo ID="ucContactInfo" runat="server" />
        </div>
        <div id="divContactInputButton" class="ContactInputButton ACA_Row ACA_LiLeft">
            <ul>
                <asp:UpdatePanel ID="upActionPanel" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <li id="liContinueBtn" runat="server" visible="False">
                            <ACA:AccelaButton ID="btnContinue" runat="server" LabelKey="aca_contactaddnew_label_continue"
                                OnClick="ContinueButton_Click" CausesValidation="true" OnClientClick="return SubmitEP(this);"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                            </ACA:AccelaButton>
                        </li>
                        <li id="liSaveBtn" runat="server">
                            <ACA:AccelaButton ID="btnSave" runat="server" LabelKey="aca_contactaddnew_label_saveandclose"
                                OnClick="SaveAndCloseButton_Click" CausesValidation="true" OnClientClick="return SubmitEP(this);"
                                DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                            </ACA:AccelaButton>
                        </li>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <li id="liClearBtn" runat="server" Visible="False">
                    <ACA:AccelaButton ID="btnClear" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" 
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server"
                        LabelKey="aca_contactedit_label_clearbutton" OnClick="ClearButton_Click" CausesValidation="false">
                    </ACA:AccelaButton>
                </li>
                <li id="liCancelBtn">
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_contactaddnew_label_discardchanges"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton">
                    </ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
