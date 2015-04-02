<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="ContactCloseMatchConfirm.aspx.cs" Inherits="Accela.ACA.Web.People.ContactCloseMatchConfirm" %>

<%@ Register TagPrefix="ACA" TagName="ContactInfoEdit" Src="~/Component/ContactInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    
    <script type="text/javascript">
        function PopupClose(refContactSeqNbr) {
            var focusObjectID = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            RefreshContactList(refContactSeqNbr, focusObjectID);
        }
        
        function RefreshContactList(refContactSeqNbr, focusObjectID) {
            if (<%= IsMultipleContact.ToString().ToLower() %>) {
                parent.<%=ParentID %>_RefreshContactList(false, refContactSeqNbr, false, focusObjectID);
            } else {
                parent.<%=ParentID %>_Refresh(false, refContactSeqNbr, false, focusObjectID);
            }
        }
        function CloseMatchConfirmClick() {
            SetWizardButtonDisable('<%=btnContinue.ClientID %>', false);
        }
    </script>

    <div id="divContactCloseMatch" class="ContactInput">
        <div id="divContactCloseMatchForm" class="ContactInputForm">
            <ACA:ContactInfoEdit ID="ucContactEdit" SupportAlwaysEditable="False" ContactSectionPosition="SpearFormCloseMatchConfirm" NeedRunExpression="False" runat="server"/>
        </div>
        <div id="divContactCloseMatchButton" class="ContactInputButton ACA_Row ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaButton ID="btnContinue" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="ContinueButton_Click"
                         LabelKey="aca_contactaddnew_label_saveandclose" CausesValidation="True">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_contactaddnew_label_discardchanges"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton">
                    </ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
