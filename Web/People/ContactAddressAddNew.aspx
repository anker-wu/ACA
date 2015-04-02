<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="ContactAddressAddNew.aspx.cs" Inherits="Accela.ACA.Web.People.ContactAddressAddNew" %>

<%@ Register Src="~/Component/ContactAddressEdit.ascx" TagName="ContactAddressEdit" TagPrefix="ACA" %>
<%@ Register SRC="~/Component/ContactAddressList.ascx" TagName="ContactAddressList" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/Expression.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var selectedAddress = $('#<%=hfSelectedAddress.ClientID %>').val();
            var isDisable = selectedAddress == "" && !<%=(AppSession.IsAdmin).ToString().ToLower() %>;
            SetWizardButtonDisable("<%=btnSelectValidatedAddress.ClientID %>", isDisable);
        });

        function PopupClose() {
            var parentFocusObj = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            SetParentLastFocus(parentFocusObj);
            RefreshContact(parentFocusObj);
        }

        function RefreshContact() {
            <%= !AppSession.IsAdmin ? "parent." + CallbackFunctionName + ";" : string.Empty %>
        }

        function ButtonClientClick() {
            if (typeof (SetNotAsk) != 'undefined') {
                SetNotAsk();
            }

            SetCurrentValidationSectionID('<%=ucContactAddressEdit.ClientID %>');
        }
        
        function ClickValidatedAddress(radioObj) {
            $(':radio').each(function() {
                if ($(this).attr('id') == radioObj.id) {
                    $(this).attr('checked', true);
                    SetWizardButtonDisable("<%=btnSelectValidatedAddress.ClientID %>",false);

                    $('#<%=hfSelectedAddress.ClientID %>').val($(radioObj).attr('rowindex'));
                } else {
                    $(this).attr('checked', false);
                }
            });
        }

        with (Sys.WebForms.PageRequestManager.getInstance()) {
            add_pageLoaded(function() {
                // restore the selected address's status
                var selectedAddress = $('#<%=hfSelectedAddress.ClientID %>').val();

                if (selectedAddress) {
                    $(':radio').each(function() {
                        var $tempCtrl = $(this);

                        if ($tempCtrl.attr('rowindex') == selectedAddress) {
                            $tempCtrl.attr('checked', true);
                        }
                    });
                }
            });
        }
    </script>
    <div id="divContactAddressInput" class="ContactAddressInput" runat="server">
        <div id="divContactAddressInputForm" class="ContactAddressInputForm">
            <ACA:AccelaLabel ID="lblEditContactTitle" LabelKey="aca_contactaddressaddnew_label_edit_title" runat="server" LabelType="SectionExText" Visible="false" />
            <ACA:ContactAddressEdit ID="ucContactAddressEdit" IsShowContactType="true" ContactSectionPosition="AddReferenceContact" runat="server" />
        </div>
        <div id="divContactAddressInputButtons" class="ContactAddressInputButtons ACA_Row ACA_LiLeft">
            <ul>
                <li id="liSaveBtn">
                    <ACA:AccelaButton ID="btnSave" runat="server" LabelKey="aca_contactaddressaddnew_label_saveandclose"
                        OnClick="SaveAndCloseButton_Click" CausesValidation="true" OnClientClick="return SubmitEP(this);"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                    </ACA:AccelaButton>
                </li>
                <li id="liSaveAndAddAnother">
                    <ACA:AccelaButton ID="btnSaveAndAddAnother" runat="server" LabelKey="aca_contactaddressaddnew_label_saveandaddanother"
                        OnClick="SaveAndAddAnotherButton_Click" CausesValidation="true" OnClientClick="return SubmitEP(this);"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                    </ACA:AccelaButton>
                </li>
                <li id="liClearBtn" runat="server">
                    <ACA:AccelaButton ID="btnClear" runat="server" LabelKey="aca_contactaddress_label_clear"
                        OnClick="ClearButton_Click" CausesValidation="false"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                    </ACA:AccelaButton>
                </li>
                <li id="liCancelBtn">
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_contactaddressaddnew_label_discardchanges"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton">
                    </ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
    <div ID="divValidatedContactAddressContent" class="ValidatedContactAddress" runat="server" Visible="False">
        <div class="ContactAddressInputForm">
            <ACA:AccelaLabel ID="lblMsgBar" runat="server" Visible="false" />
            <ACA:ContactAddressList ID="validatedContactAddressList" runat="server" ContactSectionPosition="ValidatedContactAddress" />
        </div>
        <div class="ACA_TabRow ValidatedContactAddressButtons ACA_LiLeft">
            <ul>
		        <li>
                    <ACA:AccelaButton ID="btnSelectValidatedAddress" LabelKey="aca_validatedcontactaddresslist_label_select" OnClick="Select_Click" runat="server"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"/>
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="lnkCancel" LabelKey="aca_validatedcontactaddresslist_label_cancel" OnClick="Skip_Click" runat="server" CssClass="ACA_LinkButton" />
                </li>
            </ul>
        </div>
        <asp:HiddenField ID="hfSelectedAddress" runat="server" />
        <asp:HiddenField ID="hfIsSaveAndAdd" runat="server"/>
    </div>
</asp:Content>
