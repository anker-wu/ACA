<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="ValidatedContactAddressList.aspx.cs" Inherits="Accela.ACA.Web.APO.ValidatedContactAddressList" %>
<%@ Register Src="~/Component/ContactAddressList.ascx" TagName="ContactAddressList" TagPrefix="ACA" %>

<asp:Content ID="validatedContactAddressContent" ContentPlaceHolderID="phPopup" runat="server">
    <div>
        <ACA:AccelaLabel ID="lblMsgBar" runat="server" Visible="false" />
        <ACA:ContactAddressList ID="validatedContactAddressList" runat="server" ContactSectionPosition="ValidatedContactAddress" />
    </div>
    <ACA:AccelaHeightSeparate ID="sepForSearch" runat="server" Height="10" />
    <div class="ACA_TabRow">
        <table role='presentation'>
            <tr valign="bottom">
                <td>
                    <div class="ACA_LgButton ACA_LgButton_FontSize">
                        <ACA:AccelaButton ID="btnSelectValidatedAddress" LabelKey="aca_validatedcontactaddresslist_label_select"
                            OnClick="Select_Click" runat="server" />
                    </div>
                </td>
                <td class="PopupButtonSpace">
                </td>
                <td>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkCancel" LabelKey="aca_validatedcontactaddresslist_label_cancel"
                            OnClientClick="Cancel();" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hfSelectedAddress" runat="server" />
    <script language="javascript" type="text/javascript">
        var parentPageClientID = '<%=ParentPageClientID %>';

        $(document).ready(function () {
            DisableSelectButton(!<%=AppSession.IsAdmin.ToString().ToLower() %>);
        });

        function ClickValidatedAddress(radioObj) 
        {
            var inputs = document.getElementsByTagName('input');

            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == 'radio') {
                    if (inputs[i].id == radioObj.id) {
                        inputs[i].checked = true;
                        DisableSelectButton(false);
                        var hfSelectedAddress = $get("<%=hfSelectedAddress.ClientID %>");
                        hfSelectedAddress.value = radioObj.getAttribute('rowindex');
                    }
                    else {
                        inputs[i].checked = false;
                    }
                }
            }
        }

        function SelectValidatedAddress() {
            SetNotAskForSPEAR();
            eval("parent." + parentPageClientID + "_SaveValidatedContactAddress()");
            parent.ACADialog.close();

            return false;
        }

        function Cancel() 
        {
            SelectValidatedAddress();
        }

        //Disable or Enable select button.
        function DisableSelectButton(disabled) {
            var btnSelectValidatedAddress = $get("<%=btnSelectValidatedAddress.ClientID %>");

            if (btnSelectValidatedAddress) {
                if (disabled) {
                    btnSelectValidatedAddress.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    btnSelectValidatedAddress.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                }
                else {
                    btnSelectValidatedAddress.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                    btnSelectValidatedAddress.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
                }

                DisableButton(btnSelectValidatedAddress.id, disabled);
            }
        }
    </script>
</asp:Content>
