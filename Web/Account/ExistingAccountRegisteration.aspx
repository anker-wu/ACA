<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="ExistingAccountRegisteration.aspx.cs"
    Inherits="Accela.ACA.Web.Account.ExistingAccountRegisteration" ValidateRequest="false" %>

<asp:Content ID="ACAContent" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="ACA_Content">
        <div class="existing_account_register_title">
            <ACA:AccelaLabel ID="lblRegistrationPageTitle" LabelKey="aca_existing_account_registeration_label_title" runat="server" />
        </div>
        <div class="existing_account_register_instruction_text">
            <p>
                <ACA:AccelaLabel ID="lblRegistrationInstruction" runat="server" LabelKey="aca_existing_account_registeration_label_not_registered"></ACA:AccelaLabel>
            </p>
        </div>
        <div class="existing_account_register_buttons">
            <ul>
                <li>
                    <ACA:AccelaButton runat="server" ID="btnRegister" LabelKey="aca_existing_account_registeration_label_register"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="RegisterButton_Click"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"></ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaLinkButton ID="btnCancel" runat="server" LabelKey="aca_existing_account_registeration_label_cancel"
                        CausesValidation="false" OnClick="CancelButton_Click" CssClass="ACA_LinkButton" />
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
