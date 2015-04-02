<%@ Page Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="ReprintReason.aspx.cs" Inherits="Accela.ACA.Web.AuthorizedAgent.FishingAndHunttingLicense.ReprintReason" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            if (!<% =AppSession.IsAdmin.ToString().ToLower()%>) {
                var href = $('#<%=lnkOK.ClientID %>').attr('href');
                $('#<%=lnkOK.ClientID %>').attr('href', href + ';sendReason();');
            }
        });
        
        function sendReason() {
            if(!isAllValidatorsValid()) {
                return false;
            }

            var reprintReason = $('#<%=ddlReprintReason.ClientID%>').val();

            parent.print_onclick(parent.ACADialog.returnUrl + "&ReasonId=" + reprintReason);

            hanlderLastChance();
            return true;
        }

        function hanlderLastChance() {
            if ('<%=LastChance%>'.toLowerCase() == 'true') {
                parent.DisablePrintButton('<%=ParentButtonClientId%>', true);
            }
            parent.ACADialog.close();
        }
    </script>
    <div>
        <ACA:AccelaDropDownList ID="ddlReprintReason" runat="server" LabelKey="aca_auth_agent_label_reprint_reason" Required="True">
        </ACA:AccelaDropDownList>
    </div>
    <div class="ACA_TabRow ACA_LiLeft">
        <table role='presentation' cellpadding="0" cellspacing="0" border="0">
            <tr valign="bottom">
                <td>
                    <div class="ACA_LgButton ACA_LgButton_FontSize ACA_FLeft">
                        <ACA:AccelaButton ID="lnkOK" LabelKey="aca_auth_agent_label_reprint_ok" runat="server" />
                    </div>
                </td>
                <td class="button_space">&nbsp;</td>
                <td>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkCancel" CssClass="NotShowLoading" OnClientClick="hanlderLastChance();return false;" LabelKey="aca_auth_agent_label_reprint_cancel" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
