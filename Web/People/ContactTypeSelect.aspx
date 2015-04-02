<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="ContactTypeSelect.aspx.cs" Inherits="Accela.ACA.Web.People.ContactTypeSelect" %>

<%@ Import Namespace="Accela.ACA.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script src="../Scripts/Expression.js" type="text/javascript"></script>
    <div id="divContactTypeSelection" class="ContactTypeSelection">
        <div id="divContactTypeSelectionInput" class="<%=AppSession.IsAdmin ? "InputBlockAdmin" : "InputBlock" %>">
            <ACA:AccelaLabel ID="lblSelectTitle" LabelKey="aca_contacttypeselect_label_title" runat="server" Visible="false" LabelType="SectionTitle" />
            <ACA:AccelaDropDownList ID="ddlContactType" AutoPostBack="False" CssClass="ACA_Unit" LayoutType="Horizontal" runat="server" LabelKey="per_appInfoEdit_label_contactType" Required="True">
            </ACA:AccelaDropDownList>
        </div>
        <div id="divContactTypeSelectionButton" class="ButtonBlock ACA_TabRow ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaButton ID="btnContinue" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="ContinueButton_Click"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="aca_contacttypeselect_label_continue" CausesValidation="True">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_contactaddressaddnew_label_discardchanges"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton">
                    </ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function EditContactType(contactType) {
            if (typeof (parent.updateContactType) != "undefined") {
                parent.ACADialog.close();
                parent.updateContactType(contactType);
            }
        }

        $(document).ready(function () {
            if (!$.global.isAdmin) {
                parent.ACADialog.fixWidth(400);
                var $ddlContactType = $("#<%=ddlContactType.ClientID %>");
                var helpWidth = 270;
                var scrollWidth = 20;
                var iconWidth = 16;
                var margin = 5;

                if ($(".ACA_Help_Icon").is(":visible")
                    && $ddlContactType.offset().left < helpWidth + iconWidth + margin
                    && $ddlContactType.offset().left + helpWidth + scrollWidth + margin > $("#divContactTypeSelection").width()) {
                    parent.ACADialog.fixWidth($ddlContactType.offset().left + helpWidth + scrollWidth + margin);
                }
            }
        });
    </script>
</asp:Content>
