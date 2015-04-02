<%@ Page Language="C#" MasterPageFile="~/Dialog.master" AutoEventWireup="True" 
    Inherits="Accela.ACA.Web.APO.ParcelSearchResult" ValidateRequest="false" Codebehind="ParcelSearchResult.aspx.cs" %>
<%@ Register Src="~/Component/ParcelSearchResult.ascx" TagName="ParcelSearchResult" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <script type="text/javascript">
        function UpdateParcelAndAssociates() {
            parent.ACADialog.close();

            <%= !AppSession.IsAdmin ? "parent." + CallbackFunctionName + "();" : string.Empty %>
        }
    </script>
    <div class="ParcelSearchResult">
        <ACA:AccelaLabel ID="lblParcelSearchTitle" runat="server" LabelKey="aca_parcelsearchresult_label_title" LabelType="SectionTitle" Visible="False" />
        <ACA:ParcelSearchResult ID="ucParcelSearchResult" runat="server" />
        <div class="ButtonSection ACA_Row ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaButton ID="btnSelect" runat="server" LabelKey="aca_parcelsearchresult_label_select"
                        OnClick="SelectButton_Click" CausesValidation="true" Enabled="False"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_parcelsearchresult_label_cancel"
                        CausesValidation="false" OnClientClick="parent.ACADialog.close();return false;" CssClass="ACA_LinkButton">
                    </ACA:AccelaLinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
