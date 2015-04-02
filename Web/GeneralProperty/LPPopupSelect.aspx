<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Dialog.Master" CodeBehind="LPPopupSelect.aspx.cs" Inherits="Accela.ACA.Web.GeneralProperty.LPPopupSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <div class="ACA_PopupSelect_Container">
        <div class="ACA_Section_Instruction">
            <ACA:AccelaLabel runat="server" LabelKey="aca_fileuploadlpselect_label_selectprofessional" />
        </div>
        <ACA:AccelaDropDownList runat="server" onchange="ddlProfessional_change(this);" ID="ddlProfessional" />
    </div>

    <div class="ACA_TabRow">
        <table role='presentation'>
            <tr valign="bottom">
                <td>
                    <ACA:AccelaButton ID="btnFinish" LabelKey="aca_fileupload_label_finish" runat="server"
                        DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"
                        CssClass="NotShowLoading" OnClientClick="return SelectLP();" />
                </td>
                <td class="PopupButtonSpace">
                    &nbsp;
                </td>
                <td>
                    <ACA:AccelaLinkButton ID="btnCancel" LabelKey="aca_fileupload_label_cancel" runat="server"
                        CssClass="NotShowLoading ACA_LinkButton" OnClientClick="parent.ACADialog.notDispose = false; parent.ACADialog.close(); return false;" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            DisableFinishButton(true);
        });

        function ddlProfessional_change(obj) {
            var isEmptyValue = ($(obj).val() == '');

            DisableFinishButton(isEmptyValue);
        }

        function SelectLP() {
            var $ddlProfessional = $('#<%= ddlProfessional.ClientID %>');
            var lpValue = $ddlProfessional.val();

            if (lpValue == '') {
                return;
            }

            parent.SelectLPCallBack(lpValue);
            parent.ACADialog.notDispose = false;
            parent.ACADialog.close();
        }

        //Disable or Enable Finish button.
        function DisableFinishButton(disabled) {
            var btnFinish = document.getElementById('<%=btnFinish.ClientID %>');

            if (btnFinish) {
                if (disabled) {
                    btnFinish.parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                    btnFinish.parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                }
                else {
                    btnFinish.parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
                    btnFinish.parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
                }

                DisableButton(btnFinish.id, disabled);
            }
        }
    </script>
</asp:Content>
