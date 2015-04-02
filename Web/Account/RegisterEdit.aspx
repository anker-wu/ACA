<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master"
    Inherits="Accela.ACA.Web.Account.RegisterEdit" ValidateRequest="false" Codebehind="RegisterEdit.aspx.cs" %>

<%@ Register Src="~/Component/UserRegistration.ascx" TagName="UserRegistration" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ContactEdit.ascx" TagName="ContactEdit" TagPrefix="ACA" %>
<%@ Register src="~/Component/Recaptcha.ascx" tagname="Recaptcha" tagprefix="ACA" %>
<%@ Register Src="~/Component/UserAccountEdit.ascx" TagName="UserAccountEdit" TagPrefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script src="../Scripts/DisableForm.js" type="text/javascript"></script>

    <div id="divRegisterEdit" class="ACA_Content">
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_account_registration" LabelType="PageInstruction"
                runat="server" />
        <div id="divRegisterTitle" runat="server">
            <h1>
                <ACA:AccelaLabel ID="lblRegisterAccStepInfo" LabelKey="acc_registerStart_label_step2"
                    runat="server" />
                <br />
                <ACA:AccelaLabel ID="lblRegisterAccTitle" CssClass="ACA_Show" LabelKey="acc_registerStart_label_enterAccountInfo" runat="server" />
            </h1>
        </div>
        <h1 runat="server" ID="h1ClerkTitle">
            <ACA:AccelaLabel ID="lblClerkTitle" runat="server" />
        </h1>
        <div class="ACA_TabRow">
            <p class="ACA_FRight">
                <span class="ACA_Indicator">*</span>
                <ACA:AccelaLabel ID="lblIndicate" LabelKey="acc_accountInfoForm_label_indicate"
                    runat="server" /></p>
        </div>
        <div>
            <!--for bug #49347,
                special case on firefox and opera, destory JS function, cause the CPU usage too high.
                add the always refresh panel, because contact edit control update panel postback need refresh this part.
            -->
            <asp:UpdatePanel ID="AccountEditPanel" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <ACA:AccelaLabel ID="lblLoginInfo" LabelKey="acc_accountInfoForm_label_loginInfo"
                        runat="server" LabelType="SectionExText" />
                    <ACA:UserRegistration ID="UserRegistration" runat="server" />
                    <ACA:UserAccountEdit ID="userAccountEdit" runat="server" Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <ACA:AccelaLabel ID="lblContactTitle" LabelKey="acc_accountInfoForm_label_contact"
                runat="server" LabelType="SectionTitle" />
            <div id="divAmmendment" class="ACA_LgButton ACA_LgButton_FontSize" runat="server" visible="false">
                <ACA:AccelaButton ID="lnkCreateAmendment" EnableRecordTypeFilter="true" LabelKey="aca_account_management_contact_amendment" runat="server" 
                    OnClick="CreateAmendmentButton_Click" CausesValidation="false">
                </ACA:AccelaButton>
            </div>
            <ACA:ContactEdit EnableViewState="true" ID="contactEdit" IsShowContactType="true" runat="server" EnableLookUp="False"
                             NeedSelectType="True" IsShowDetail="False" />
        </div>
        <div class="ACA_TabRow ACA_Line_Content">&nbsp;</div>
        <div id="divRecaptcha" runat="server">
            <ACA:Recaptcha ID="reCaptcha" runat="server" />
        </div>
        <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
        <div class="ACA_Row ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaButton ID="StartNextButton2" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"  LabelKey="acc_reg_label_continueReg"
                        runat="server" OnClick="NextCreateAccountButton_OnClick" />
                    <ACA:AccelaButton ID="btnSave" OnClientClick="setValidationExcludeIds();" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" OnClick="SaveButton_Click" 
                        LabelKey="aca_authagent_editclerk_label_save" runat="server" Visible="false"></ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaButton ID="lnkCancel" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" 
                        LabelKey="aca_authagent_editclerk_label_cancel" OnClick="CancelButton_Click" CausesValidation="false" runat="server" Visible="false" />
                </li>
            </ul>
        </div>

    <!-- message box begin -->
    <div id="divDuplicateKeyBox" class="ACA_Hide PopUpDlg contact_duplicate_dialog">
            <ACA:AccelaHideLink ID="hlDuplicateKeyBegin" Width="0" runat="server" NextControlID="btnContinue" AltKey="img_alt_form_begin"/>
            <div id='ACA_Popup_Header'>
                <h1 class='ACA_Dialog_Title'>
                    <ACA:AccelaLabel ID="lblDuplicateIndentityTitle" runat="server" LabelKey="aca_registeredit_label_duplicate_indentity"></ACA:AccelaLabel>
                </h1>
                <div class='ACA_Dialog_Close_Div'>
                    <ACA:AccelaLinkButton ID="divDuplicateKeyBoxClose" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClientClick="duplicateKeyDialog.cancel();return false;"><img class="ACA_ActionIMG" src='<%=ImageUtil.GetImageURL("closepopup.png") %>' alt="close" /></ACA:AccelaLinkButton>
                </div>
            </div>
            <div class="clear"></div>
            <div class='ACA_Dialog_Frame_DIV ACA_Dialog_Content_Padding'>
                <div id='ACA_Popup_Content'>
                    <div class="ACA_TabRow">
                        <div class="ACA_Section_Instruction ACA_Section_Instruction_FontSize">
                            <ACA:AccelaLabel ID="lblDuplicateIndentityMessage" IsNeedEncode="false" runat="server" />
                        </div>
                    </div>
                    <table role='presentation' class="ACA_AlignLeftOrRight">
                        <tr valign="bottom">
                            <td>
                                <div class="ACA_LgButton ACA_LgButton_FontSize">
                                    <ACA:AccelaButton ID="btnContinue" OnClick="ContinueButton_OnClick" CausesValidation="false" LabelKey="aca_registeredit_label_continue" runat="server"></ACA:AccelaButton>
                                </div>
                            </td>
                            <td>
                                <div class="ACA_LinkButton">
                                    <ACA:AccelaLinkButton ID="btnDuplicateKeyCancel" CssClass="NotShowLoading" LabelKey="aca_registeredit_label_cancel" CausesValidation="false" OnClientClick="duplicateKeyDialog.cancel(); return false;" runat="server"></ACA:AccelaLinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <ACA:AccelaHideLink ID="hlDuplicateKeyEnd" Width="0" runat="server" NextControlID="btnContinue" AltKey="img_alt_form_end" />
            <!-- message box end -->
        </div>
    </div>
   <script type="text/javascript">
       
    var EnabledAutoFillDDLIDs = '';

    function EnabledAutoFillDDL() {
        var ids = EnabledAutoFillDDLIDs.split('|');
        for (var i = 0; i < ids.length; i++) {
            if (ids[i] != '') {
                $get(ids[i]).disabled = false;
            }
        }
    }

    var duplicateKeyDialog;

    function showDuplicateMessage() {
        /*
        To waiting the body element load complete, otherwise the "document.body.appendChild" in popUpDialog will occurs below error:
        "Unable to modify the parent container element before the child element is closed(KB927917)"
        */
        setTimeout(function () {
            duplicateKeyDialog = new popUpDialog($get('divDuplicateKeyBox'), $get('<%=hlDuplicateKeyBegin.ClientID%>'), null, null, '<%= StartNextButton2.ClientID %>');
            duplicateKeyDialog.showPopUp();
        }, 50);
    }
    
    function validatePassword() {
        var userIdentifier = '<%=ExistingAccountRegisterationUserID %>';
        var pwdId = '<%=UserRegistration.PasswordClientId %>';
        var password = $('#' + pwdId).val();

        if (isNullOrEmpty(password)) {
            doErrorCallbackFun('', pwdId, 2);
            $('#' + pwdId).focus();
            HideLoading();
            return;
        }

        doErrorCallbackFun('', pwdId, 0);
        PageMethods.IsPasswordCorrect(userIdentifier, password, function(result) {
            var validationResult = eval('(' + result + ')');
            var passValidation = validationResult.passValidation == 'True';
            var errorMessage = validationResult.errorMessage;
            var incorrectPasswordMsg = '<%=GetTextByKey("aca_existing_account_registeration_msg_incorrectpwd").Replace("'","\\'") %>';
        
            if (passValidation) {
                hideMessage();
                <%=contactEdit.ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactLookUp.aspx") %>', '<%=contactEdit.AddFromSavedClientID%>', 400, 200);
            } 
            else {
                var message = '';
            
                if (String.IsNullOrEmpty(errorMessage)) {
                    message = incorrectPasswordMsg;
                    $('#' + pwdId).focus();
                } 
                else {
                    message = errorMessage;
                }
                
                $('#' + pwdId).val('');
                showNormalMessage(message, 'Error');
                HideLoading();
            }
        });
    }
    
    with (Sys.WebForms.PageRequestManager.getInstance()) {
        add_endRequest(function () {
            if (typeof (myValidationErrorPanel) != "undefined") {
                myValidationErrorPanel.reservedAllErrors();
            }
        });
    }
   </script> 
</asp:Content>
