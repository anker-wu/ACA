<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.master" AutoEventWireup="true"
    CodeBehind="RegisterAccountConfirm.aspx.cs" Inherits="Accela.ACA.Web.Account.RegisterAccountConfirm" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/ContactInfo.ascx" TagName="ContactInfoEdit" TagPrefix="uc1" %>
<asp:Content ID="ContactContent" ContentPlaceHolderID="phPopup" runat="server">
    <script type="text/javascript">
        function chbSelected_onclick(obj) {
            DisableContinueButton(!obj.checked);
        }

        $(document).ready(function () {
            if ($.global.isAdmin == false) {
                if ('<%= this.NeedIdentifyCheck.ToString().ToLower() %>' == 'false') {
                    $('#<%=sepForCheckBox.ClientID %>').hide();
                    $('#divCheckConfirm').hide();
                    $('#<%=btnBack.ClientID %>').hide();
                } else {
                    DisableContinueButton(true);
                }
            }
        });

        function DisableContinueButton(disable) {
            var btnContinue = $('#<%=btnContinue.ClientID %>');
            if (disable) {
                btnContinue.parent().addClass("ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                DisableButton(btnContinue.attr("id"), disable);
            }
            else {
                btnContinue.parent().removeClass().addClass("ACA_LgButton ACA_LgButton_FontSize");
                DisableButton(btnContinue.attr("id"), disable);
                attachProcessLoading(btnContinue.get(0));
            }
        }

        function PopupClose() {
            var focusObjectID = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            
            // If not need check, no need to refresh the contact.
            if ('<%= this.NeedIdentifyCheck.ToString().ToLower() %>' == 'false') {
                return;
            }
            
            var isFromAddress = false;
            parent.<%=AppSession.GetContactSessionParameter().Process.CallbackFunctionName %>_Refresh(isFromAddress, undefined, undefined, focusObjectID);
        }

        function Back() {
            var p = new ProcessLoading();
            p.showLoading();
            var url = '<%= Page.ResolveUrl("~/People/ContactAddNew.aspx") + Request.Url.Query %>';
            url += '<%= !string.IsNullOrEmpty(Request.QueryString[UrlConstant.IS_BACK]) ? "" : "&" + UrlConstant.IS_BACK + "=" + ACAConstant.COMMON_Y %>';
            window.location.href = url;

            return false;
        }
    </script>

    <div>
        <div>
            <ACA:AccelaLabel ID="lblContactTitle" LabelKey="aca_contactaddnew_label_detail_title" runat="server" Visible="false" LabelType="SectionExText" />
            <uc1:ContactInfoEdit ID="contactEdit" NeedRunExpression="False" runat="server"/>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForCheckBox" runat="server" Height="16" />
        <div class="ACA_TabRow ACA_SmallError_Icon" id="div_error_icon" style="display: none">
            <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_mark_required") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>
        </div>
        <div id="divCheckConfirm" class="aca_checkbox aca_checkbox_fontsize">
            <input type="checkbox" id="chkTermAccept" onclick="chbSelected_onclick(this)" runat="server" />
            <ACA:AccelaLabel ID="lblAcceptAsOwnContact" AssociatedControlID="chkTermAccept" runat="server" class="ACA_SmLabel ACA_SmLabel_FontSize"/>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
        <table role='presentation' class="ACA_AlignLeftOrRight">
            <tr valign="bottom">
                <td>
                    <ACA:AccelaButton ID="btnContinue" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                        LabelKey="acc_reg_label_continue_comfirm" runat="server" OnClick="ContinueButton_OnClick" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <div class="ACA_LinkButton font11px">
                        <ACA:AccelaLinkButton ID="btnBack" CssClass="NotShowLoading" LabelKey="aca_registeraccountconfirm_label_back"
                            CausesValidation="false" OnClientClick="return Back();" runat="server"></ACA:AccelaLinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
