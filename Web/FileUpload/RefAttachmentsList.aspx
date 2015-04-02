<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Dialog.Master" CodeBehind="RefAttachmentsList.aspx.cs"
    Inherits="Accela.ACA.Web.FileUpload.RefAttachmentsList" %>

<%@ Register Src="~/Component/RefAttachmentList.ascx" TagName="refAttachmentList"
    TagPrefix="ucl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script type="text/javascript" src="../Scripts/textCollapse.js"></script>
    <div class="RefAttachment">
        <div class="RefAttachmentList">
            <ucl:refAttachmentList ID="refAttachmentList" runat="server" />
        </div>
        <div class="RefAttachmentListButtons ACA_LiLeft" runat="server">
            <ul>
                <li>
                    <ACA:AccelaButton runat="server" ID="btnContinue" LabelKey="aca_contactlookup_label_continuecontactaddress" OnClick="BtnContinueClick"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" />
                </li>
                <li>
                    <ACA:AccelaLinkButton runat="server" LabelKey="aca_contactlookup_label_cancelcontactaddress"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;"
                        CssClass="ACA_LinkButton" />
                </li>
            </ul>
        </div>
    </div>

    <script type="text/javascript">
        // This function will be used by UserControl [RefAttachmentList.ascx], do NOT delete.
        function GetCurrentContinueButtonClientID() {
            return "<%=btnContinue.ClientID %>";
        }

        $(document).ready(function () {
            SetWizardButtonDisable("<%=btnContinue.ClientID %>", !<%=(AppSession.IsAdmin || refAttachmentList.HasCheckedItems).ToString().ToLower() %>);
        });

        function ClosePopup(files) {
            parent.<%= Request[UrlConstant.IFRAME_ID] %>_FillDocEditForm(JSON.stringify(files), null, "", true);
            parent.ACADialog.close();
        }

    </script>

</asp:Content>
