<%@ Page Language="C#" MasterPageFile="~/Dialog.master" AutoEventWireup="True" 
    Inherits="Accela.ACA.Web.APO.AddressSearchResult" ValidateRequest="false" Codebehind="AddressSearchResult.aspx.cs" %>
<%@ Register Src="~/Component/AddressSearchResult.ascx" TagName="AddressSearchResult" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <script type="text/javascript">
        function UpdateAddressAndAssociates() {
            parent.ACADialog.close();

            <%= !AppSession.IsAdmin ? "parent." + CallbackFunctionName + "();" : string.Empty %>
        }
    </script>
    <div class="AddressSearchResult">
        <ACA:AccelaLabel ID="lblAddressSearchTitle" runat="server" LabelKey="aca_addresssearchresult_label_title" LabelType="SectionTitle" Visible="False" />
        <ACA:AddressSearchResult ID="ucAddressSearchResult" runat="server" />
        <div class="ButtonSection ACA_Row ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaButton ID="btnSelect" runat="server" LabelKey="aca_addresssearchresult_label_select"
                        OnClick="SelectButton_Click" CausesValidation="true" Enabled="False"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_addresssearchresult_label_cancel"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton">
                    </ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
