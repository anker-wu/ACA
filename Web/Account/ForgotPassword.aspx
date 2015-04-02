<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Account.ForgotPassword" ValidateRequest="false" Codebehind="ForgotPassword.aspx.cs" %>
<%@ Register Src="~/Component/MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>
<%@ Register Src="~/Component/LoginBox.ascx" TagName="loginBox" TagPrefix="uc1" %>  

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
     <script type="text/javascript">
         var repeatCallTimes = 0;
         var originEmail = '';

         function disableCombineSecurityAnswer(disabled) {
             $("#<%=txtCombineSecurityAnswer.ClientID %>").attr("disabled", disabled);
             if (disabled) {
                 $("#divSecurityQuestionCombine").hide();
                 $("#ctl00_PlaceHolderMain_txtCombineSecurityAnswer").val("");
                 doErrorCallbackFun('', '<%=txtCombineSecurityAnswer.ClientID %>', 0);
             }
             else {
                 $("#divSecurityQuestionCombine").show();
             }
             
             disableCombineSendEmailButton(disabled);
         };

         function disableCombineSendEmailButton(disabled) {
             var btnSendEmail = $('#<%=btnCombineSendEmail.ClientID %>');
             if (disabled) {
                 btnSendEmail.parent().attr("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
                
             } else {
                 btnSendEmail.parent().attr("class", "ACA_LgButton ACA_LgButton_FontSize");
             }

             DisableButton('<%=btnCombineSendEmail.ClientID %>', disabled);
         }

         function trimEmailBlankSpace(email) {
             email = email.replace(/^(\s|\u00A0)+/, '');
             for (var i = email.length - 1; i >= 0; i--) {
                 if (/\S/.test(email.charAt(i))) {
                     email = email.substring(0, i + 1);
                     break;
                 }
             }

             return email;
         }

         function checkCombineEmailAddress_onkeyup(key, evt) {
             var keyCode = evt.keyCode ? evt.keyCode : evt.which ? evt.which : evt.charCode;
             //if key up is "Backspace" or "Delete" or "Ctrl+X"
             if (keyCode == 8 || keyCode == 46 || (evt.ctrlKey & keyCode == 88)) {
                 var value = $("#<%=txtCombineEmail.ClientID %>").val();
                 if (trimEmailBlankSpace(value) == "") {
                     $("#<%=lblSecurityQuestionCombine.ClientID %>").html("");
                     disableCombineSecurityAnswer(true);
                 }
             }
         }

         function checkCombineEmailAddress_onblur() {
             var value = $("#<%=txtCombineEmail.ClientID %>").val();
             if (trimEmailBlankSpace(value) != "") {
                 SetCustomValidate(value);
             }
         }

         function SetQuestionArea(result, isdisplay) {
             if (result != 'undefined' && result != "" && isdisplay) {
                 $("#<%=lblSecurityQuestionCombine.ClientID %>").html(result);
                 disableCombineSecurityAnswer(false);
             } else {
                 $("#<%=lblSecurityQuestionCombine.ClientID %>").html("");
                 disableCombineSecurityAnswer(true);
             }
         }

         function CallSuccess(result, userContext, methodName) {
             repeatCallTimes = 0;
             HideLoading();
             hideMessage("messageSpan");

             if (result != '') {
                 SetQuestionArea(result, true);
             }
             else {
                 disableCombineSendEmailButton(false);
             }
         }

         function SetCustomValidate(value) {
             hideMessage();
             var isValid = true;

             // Get the page whether validation passed.
             if (Page_Validators && Page_Validators.length > 0) {
                 for (var i = 0; i < Page_Validators.length; i++) {
                     if (!Page_Validators[i].isvalid) {
                         isValid = false;
                         break;
                     }
                 }
             }

             /*
                Request the Security Question below,
                1. If not in requestion process.
                2. If input email not same with last input.
                3. If page validation passed.
             */
             if (repeatCallTimes < 1
                 && (originEmail == "" || originEmail != value)
                 && isValid) {
                 repeatCallTimes = 1;
                 originEmail = value;
                 ShowLoading();
                 PageMethods.GetQuestionByEmail(value, CallSuccess, CallFailed);
             }
         }

         function CallFailed(error) {
             repeatCallTimes = 0;
             HideLoading();
             SetQuestionArea('', false);
             showNormalMessage(error.get_message(), 'Error');
         }

         with (Sys.WebForms.PageRequestManager.getInstance()) {
             add_pageLoaded(function(sender, args) {
                 $('#<%=txtCombineEmail.ClientID%>').change(function() {
                     checkCombineEmailAddress_onblur();
                 });
             });
         }

     </script>
    <!-- start custom content -->
    <asp:Panel ID="pnlEmail" runat="server" CssClass="ACA_RightContent">
        <h1>
            <ACA:AccelaLabel ID="aca_findPassword_label_resetPassword" LabelKey="aca_findPassword_label_resetPassword"
                runat="server">
            </ACA:AccelaLabel>
        </h1>
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="acc_findPassword_label_enterEmail" LabelKey="acc_findPassword_label_enterEmail"
                runat="server" LabelType="BodyText">
            </ACA:AccelaLabel>
            <br />
        </div>
        <div class="ACA_TabRow">
            <ACA:AccelaEmailText ID="txtEmail" LabelKey="acc_userInfoForm_label_email" AutoPostBack="false"
                MaxLength="50" Validate="required;email" CssClass="ACA_XLong" runat="server" SetFocusOnError="true">                  
            </ACA:AccelaEmailText>
        </div>
        <div class="ACA_TabRow">
            <ACA:AccelaTextBox ID="hidden" runat="server" Visible="false"></ACA:AccelaTextBox>
        </div>
        <div class="ACA_TabRow">
            &nbsp;</div>
        <div class="ACA_LgButton ACA_LgButton_FontSize ACA_Button_Text">
                <ACA:AccelaButton ID="lbkContinue" runat="server" TabIndex="0" LabelKey="acc_FindPassword_lable_continue"
                    OnClick="ContinueButton_Click">
                </ACA:AccelaButton>
            <div style="display: none;">
                <asp:Button ID="btnEmailHidden" OnClick="ContinueButton_Click" UseSubmitBehavior="false"
                    runat="server" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSecurityAnswer" runat="server" CssClass="ACA_RightContent" Visible="false">
        <h1>
            <ACA:AccelaLabel ID="aca_findPassword_label_resetPassword1" LabelKey="aca_findPassword_label_resetPassword"
                runat="server">
            </ACA:AccelaLabel>
        </h1>
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="acc_findPassword_label_enterSecurityAnswer" LabelKey="acc_findPassword_label_enterSecurityAnswer"
                runat="server" LabelType="BodyText">
            </ACA:AccelaLabel>
            <br />
        </div>
        <div class="ACA_TabRow">
            <ACA:AccelaLabel ID="acc_findPassword_label_SecurityAnswerInfo" CssClass="ACA_Label ACA_Label_FontSize"
                LabelKey="acc_findPassword_label_SecurityAnswerInfo" runat="server">
            </ACA:AccelaLabel>
            <p>
                <ACA:AccelaLabel ID="lblSecurityQuestion" runat="server" /><br />
                <br />
            </p>
        </div>
        <div class="ACA_TabRow ACA_Label">
            <ACA:AccelaTextBox ID="txtSecurityAnswer" AutoPostBack="false" Validate="required"
                LabelKey="acc_findpassword_label_securityAnswer" runat="server" CssClass="ACA_XLong">
            </ACA:AccelaTextBox>
        </div>
        <div class="ACA_Button_Text">
            <div class="ACA_TabRow">&nbsp;</div>
                <ACA:AccelaButton ID="lbkSendEmail" runat="server" LabelKey="acc_findPassword_Label_SendEmail"
                    OnClick="SendEmailButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize">
                </ACA:AccelaButton>
        </div>
        <div style="display: none;">
            <asp:Button ID="btnSendEmailHidden" runat="server" OnClick="SendEmailButton_Click" UseSubmitBehavior="false" />
        </div>
    </asp:Panel>
     <asp:Panel ID="pnlCombine" runat="server" Visible="false"  CssClass="ACA_RightContent">
               <h1>
            <ACA:AccelaLabel ID="lblResetPasswordCombine" LabelKey="aca_findPassword_label_resetPassword"
                runat="server">
            </ACA:AccelaLabel>
        </h1>
        <div class="ACA_Page ACA_Page_FontSize">
            <ACA:AccelaLabel ID="lblFindPasswordCombine" LabelKey="aca_forgotpassword_label_combinedisclaimer"
                runat="server" LabelType="BodyText">
            </ACA:AccelaLabel>
            <br />
        </div>
        <div id="divCombineEmail" class="ACA_TabRow">
            <ACA:AccelaEmailText ID="txtCombineEmail" LabelKey="aca_forgotpassword_label_combineemailaddress"
                MaxLength="50" Validate="required;email;customvalidation"
                CustomValidationMessageKey="acc_forgotPassword_error_emailInvalidate" CssClass="ACA_XLong" runat="server" SetFocusOnError="true">
            </ACA:AccelaEmailText>
        </div>
        <div id="divSecurityQuestionCombine">
            <div class="ACA_Page ACA_Page_FontSize">
                <ACA:AccelaLabel ID="lblFindPasswordEnterSecurityAnswerCombine" LabelKey="acc_forgotpassword_label_combineentersecurityanswer"
                    runat="server" LabelType="BodyText">
                </ACA:AccelaLabel>
                <br />
            </div>
            <div class="ACA_TabRow">
                <ACA:AccelaLabel ID="lblFindPasswordSecurityAnswerInfoCombine" CssClass="ACA_Label ACA_Label_FontSize"
                    LabelKey="acc_findPassword_label_SecurityAnswerInfo" runat="server">
                </ACA:AccelaLabel>
                <p>
                    <ACA:AccelaLabel ID="lblSecurityQuestionCombine" runat="server" />
                    <ACA:AccelaHeightSeparate ID="sepHeightForForgotPassword" runat="server" Height="15"/>
                </p>
            </div>
        </div>
        <div id="divCombineSecurityAnswer" class="ACA_Tab_Row ACA_Label">
            <ACA:AccelaTextBox ID="txtCombineSecurityAnswer" Validate="required"
                LabelKey="aca_forgotpassword_label_combinesecurityanswer" runat="server" CssClass="ACA_XLong">
            </ACA:AccelaTextBox>
        </div>
        <div id="divCombineSendEmail" class="ACA_Button_Text">
            <div class="ACA_TabRow">&nbsp;</div>
                <ACA:AccelaButton ID="btnCombineSendEmail" runat="server" LabelKey="acc_findPassword_Label_SendEmail"
                    OnClick="SendEmailButton_Click" DivEnableCss="ACA_LgButton ACA_LgButtonForRight ACA_LgButtonForRight_FontSize">
                </ACA:AccelaButton>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" class="ACA_Content">
        <table role='presentation' width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr style="vertical-align:top;">
                <td>
                     <div class="ACA_RightContent">
                        <div class="ACA_Row">
                            <uc1:MessageBar ID = "findPasswordSuccessMessage" runat="Server" />
                        </div>
                        <div class="ACA_Page ACA_Page_FontSize">
                            <br />
                            <ACA:AccelaLabel ID="lblFindPasswordSuccuessInfo" LabelKey="acc_findPassword_label_SuccuessInfo"
                                runat="server" LabelType="BodyText" />
                            <br />
                        </div>
                    </div>
                </td>
                <td>
                    <uc1:loginBox ID="LoginBox" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
