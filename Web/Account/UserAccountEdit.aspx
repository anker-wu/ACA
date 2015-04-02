<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="UserAccountEdit.aspx.cs" Inherits="Accela.ACA.Web.Account.UserAccountEdit" %>
<%@ Register Src="~/Component/UserAccountEdit.ascx" TagName="UserAccountEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script type="text/javascript">
        function PopupClose() {
            parent.ACADialog.close();
            parent.RefreshAccountView();
        }
    </script>
    <div>
        <ACA:AccelaLabel ID="lblLoginInfo" LabelKey="acc_accountInfoForm_label_loginInfo" runat="server" LabelType="SectionExText"  Visible="false"/>
        <uc1:UserAccountEdit ID="userAccountEdit" runat="server" />
    </div>
    <div class="ACA_TabRow">
    <table role='presentation'>
        <tr valign="bottom">
            <td>
                <div class="ACA_LgButton ACA_LgButton_FontSize">
                    <ACA:AccelaButton ID="btnSave" OnClientClick="setValidationExcludeIds();" OnClick="SaveButton_Click" LabelKey="aca_accountcontactedit_label_save" runat="server"></ACA:AccelaButton>
                </div>
            </td>
            <td>&nbsp;</td>
            <td>
                <div id="userAccountEditItem">
                    <ACA:AccelaLinkButton ID="lnkCancel" LabelKey="aca_accountcontactedit_label_cancel" OnClientClick="parent.ACADialog.close();return false;"
                        CssClass="ACA_LinkButton" runat="server" />
                </div>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>