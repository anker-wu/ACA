<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRegistration.ascx.cs"
    Inherits="Accela.ACA.Web.Component.UserRegistration" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Web.Util" %>
<%@ Register Src="~/Component/PasswordSecurityBar.ascx" TagName="PasswordSecurityBar"
    TagPrefix="uc1" %>
<script type="text/javascript">
    var globalEvent;
    var globalUserNameValidators;
    var globalPwdValidators;
    var globalEmailValidators;
    var emailValidatorIsValid=true;
    var userNamevalidatorIsValid=true;
    var passwordSecurityValidatorValid = true;
    var repeatCallTimes=0;
    var passwordErrorMsg = "";
    var isPressedEnterKey = false;
    var asyncValidators = new Array();
    var validatorNames = ["checkUserName_onblur", "checkEmailAddress_onblur", "CheckPasswordSecurity_onblur"];
    function checkUserName_onblur(source, arguments) {
        var value = GetValueById("<%=txbUserName.ClientID %>");

        if(value.length>3)
        {        
            SetCustomValidate(true,source,value,arguments);
            if(!userNamevalidatorIsValid){
                arguments.IsValid=false;
            }

            RefreshValidator(validatorNames[0], !arguments.IsValid);
        }
    }

    function RefreshValidator(validatorName, isAddToPageValidator) {
        var pageValidatorIndx = -1;
        var asyncValidatorIndx = -1;

        for (var i = 0; i < Page_Validators.length; i++) {
            if (Page_Validators[i].clientvalidationfunction == validatorName) {
                pageValidatorIndx = i;
                break;
            }
        }

        for (var j = 0; j < asyncValidators.length; j ++) {
            if (asyncValidators[j].clientvalidationfunction == validatorName) {
                asyncValidatorIndx = j;
                break;
            }
        }

        if (isAddToPageValidator) {
            if (pageValidatorIndx < 0 && asyncValidatorIndx >= 0) {
                Page_Validators.push(asyncValidators[asyncValidatorIndx]);
            }
        } 
        else {
            if (asyncValidatorIndx < 0 && pageValidatorIndx >= 0) {
                asyncValidators.push(Page_Validators[pageValidatorIndx]);
			}

            if (pageValidatorIndx >= 0) {
                Array.remove(Page_Validators, Page_Validators[pageValidatorIndx]);
            }
        }
    }

    function SetCustomValidate(isCheckUserName,source,value,arguments)
    {
        var validators = source.previousSibling.previousSibling.previousSibling.Validators;
        if (repeatCallTimes< 1)
        {
            if(isCheckUserName) {
                globalUserNameValidators=validators;
                PageMethods.IsExistUserName(value,CallSuccess);
            } else {
                globalEmailValidators = validators;
                PageMethods.IsExistEmail(value,CallSuccess);    
            }
        } 
        else
        {
            if(isCheckUserName) {
                arguments.IsValid = userNamevalidatorIsValid;    
            } else {
                arguments.IsValid = emailValidatorIsValid;
            }

            repeatCallTimes = 0;            
        }
    }

    function checkPassWordLength_onblur()
    {
        if(!isFireFox())
        {
            var value = GetValueById("<%=txbPassword1.ClientID %>");
            doErrorCallbackFun("", "<%=txbPassword1.ClientID %>", 0); 
            if(value.length>20 || value.length<8)
            {
                var obj = document.getElementById("<%=txbPassword1.ClientID %>"+"_length_vad");
                doErrorCallbackFun(obj.getAttribute("errormessage"), "<%=txbPassword1.ClientID %>", 2); 
                window.setTimeout(   function(){   obj.focus();   },   0);
            }
            return;
        }
    }

    function CheckPasswordSecurity_onblur(source, arguments)
    {
        var passwordID = "<%=txbPassword1.ClientID %>";
        var userNameID = "<%=txbUserName.ClientID %>";
   
        var password = GetValueById(passwordID);
        var userName = GetValueById(userNameID);
    
        if (password == "")
        {
            setToDefault();
            return;
        }
    
        var validators=source.previousSibling.Validators;
        if (validators == null)
        {
            validators=source.previousSibling.previousSibling.Validators;
        }

        if(repeatCallTimes<1)
        { 
            globalPwdValidators=validators;
            PageMethods.CheckPasswordSecurity(password, userName, <% =(IsForClerk && !IsForClerkEdit).ToString().ToLower() %>, PasswordCallSuccess);
        } 
        else
        {
            arguments.IsValid = passwordSecurityValidatorValid;
            repeatCallTimes=0;
        }
    
         if (!passwordSecurityValidatorValid)
         {
             source.errormessage = passwordErrorMsg;
             arguments.IsValid=passwordSecurityValidatorValid;
         }

         RefreshValidator(validatorNames[2], !arguments.IsValid);
     }

     function PasswordCallSuccess(result)
     {
         var elemID = "<%=txbPassword1.ClientID %>";

        passwordSecurityValidatorValid = true;
        var json = eval('(' + result + ')');
        if ((parseInt(json.ErrorCode) >= 100))
        {
            var obj = document.getElementById(elemID);
            passwordSecurityValidatorValid= false;
            passwordErrorMsg = json.ErrorMessage;  
        }
        else
        {
            passwordErrorMsg = "";
        }

        doChangePassworStrengthdBar(json.ErrorCode, json.ErrorMessage, json.PwdScore);
    
        repeatCallTimes++;   
        for (var i = 0; i < globalPwdValidators.length; i++)
        {
            ValidatorValidate(globalPwdValidators[i], null, globalEvent);
        }
        ValidatorUpdateIsValid(); 
    }

    function checkEmailAddress_onblur(source, arguments)
    {
        var value = GetValueById("<%=txbEmail.ClientID %>");
        SetCustomValidate(false,source,value,arguments);
        if(!emailValidatorIsValid){
            arguments.IsValid=false;
        } 

        RefreshValidator(validatorNames[1], !arguments.IsValid);
    }

    function ShowUserInfo(result)
    {
        var value = result;
    
        if(value == '')
        {
            hideMessage();
        }
        else
        {
            showNormalMessage('<%=GetTextByKey("acc_reg_error_existUser").Replace("'","\\'")%>', 'Error');
        }
    }

    function CallSuccess(result, userContext, methodName)
    {
        repeatCallTimes++;
        var validators;
        var targetId;
        var isValid = result == "";

        if (methodName.toLowerCase() == "isexistemail") {
            emailValidatorIsValid = isValid;
            validators = globalEmailValidators;
            targetId = '<%=txbEmail.ClientID %>';
        } else {
            userNamevalidatorIsValid = isValid;
            validators = globalUserNameValidators;
            targetId = '<%=txbUserName.ClientID %>';
        }

        var arrResult = result.split('<%=ACAConstant.SPLIT_CHAR %>');
        var isCitizenRegistration = arrResult[1] == '<%=ACAConstant.CITIZEN_ACCOUNT_TYPE %>' 
            && <%=(AppSession.User.IsAnonymous && AuthenticationUtil.IsInternalAuthAdapter && !StandardChoiceUtil.IsEnableLdapAuthentication()).ToString().ToLower() %>;
        changeErrorMessageByAccountType(targetId, isCitizenRegistration);

        if (validators) {
            for (var i = 0; i < validators.length; i++)
            {
                ValidatorValidate(validators[i], null, globalEvent);
            }
            
            ValidatorUpdateIsValid(); 
        }

        if (!isValid && isCitizenRegistration) {
            appendActivateExistingAccountLink(targetId, arrResult[0]);
        }
    }

    function changeErrorMessageByAccountType(targetId, isCitizenRegistration) {
        var isUserName = targetId == '<%=txbUserName.ClientID %>';
        var targetVad = $('#' + targetId + '_custom_vad');
        var orginalExistingMsg = isUserName ?
            '<%=GetTextByKey("acc_reg_error_existUser").Replace("'","\\'") %>'
            : '<%=GetTextByKey("acc_reg_error_existEmail").Replace("'","\\'") %>';
        var newExistingMsg = isUserName ?
            '<%=GetTextByKey("aca_existing_account_registeration_msg_existinguser").Replace("'","\\'") %>'
            : '<%=GetTextByKey("aca_existing_account_registeration_msg_existingemail").Replace("'","\\'") %>';
        var currentExistingMsg = isCitizenRegistration ? newExistingMsg : orginalExistingMsg;
            
        targetVad.attr('errormessage', currentExistingMsg);
    }
    
    function appendActivateExistingAccountLink(targetId, recoverSource) {
        var spanMsg = $('#' + targetId + '_label_2');
        var isUserName = targetId == '<%=txbUserName.ClientID %>';
        var lnkRecoverId = isUserName ? 'lnkRecoverByUserName' : 'lnkRecoverByEmail';
        var url = getRecoverLinkUrl(targetId, recoverSource);
        var linkText = EncodeHTMLTag('<%=RecoverMsgLinkText %>');

        var enabledNewTemplate = '<%=StandardChoiceUtil.IsEnableNewTemplate() %>';
        var linkRecoverHtml = '';
        if (enabledNewTemplate.toLowerCase() == 'true') {
            linkRecoverHtml = '<a id="' + lnkRecoverId + '" class="ACA_Error_Label aca_error_message" href="javascript:void(0);" onclick="RedirectToNewUILogin()">' + linkText + '</a>';
        } else {
            linkRecoverHtml = '<a id="' + lnkRecoverId + '" class="ACA_Error_Label aca_error_message" href="javascript:void(0);" onclick="ShowLoading();RedirectToActivatePage(\'' + url + '\');">' + linkText + '</a>'; 
        }

        var recoverMsg = isUserName
            ? '<%=GetTextByKey("aca_existing_account_registeration_msg_username_recover").Replace("'","\\'") %>'
            : '<%=GetTextByKey("aca_existing_account_registeration_msg_email_recover").Replace("'","\\'") %>';
        recoverMsg = recoverMsg.replace('{0}', linkRecoverHtml);

        if (spanMsg.find('a[id=' + lnkRecoverId + ']').length == 0) {
            spanMsg.append(recoverMsg);
        }

        var lnkRecover = spanMsg.find('a[id=' + lnkRecoverId + ']');

        if (isPressedEnterKey) {
            lnkRecover.focus();
            isPressedEnterKey = false;
        }

        if (document.activeElement && document.activeElement.id == targetId) {
            lnkRecover.focus();
            isPressedEnterKey = true;
        }
    }

    function RedirectToActivatePage(url) {
        PageMethods.ClearContact(function () {
            window.location.href = url;
        });
    }

    function RedirectToNewUILogin() {
        PageMethods.ClearContact(function () {
            parent.redirectToLogin();
        });
    }

    function getRecoverLinkUrl(targetId, recoverSource) {

        var url = '<%=FileUtil.AppendApplicationRoot("Account/RegisterEdit.aspx") %>';

        if (<%=(Request.QueryString.Count > 0).ToString().ToLower() %>) {
            url += '?<%=Request.QueryString %>&';
        } else {
            url += '?';
        }

        var encodedUserIdentifier = encodeURIComponent($('#' + targetId).val());
        url += '<%=UrlConstant.RECOVER_SOURCE %>=' + recoverSource + '&<%=UrlConstant.USER_ID_OR_EMAIL %>=' + encodedUserIdentifier;

        return url;

    }

    function blockChars(oEvent)
    {
        var keyCode = oEvent.keyCode ? oEvent.keyCode : oEvent.which;

        /*
        Resolve the special issue in Opera browser:
         - bug #45921 Can't use the Ctrl + C/A to copy or select all the value inputed in User Name on Enter Information page.
        */
        if ($.browser.opera && event.ctrlKey){
            return true;
        }
    
        if((keyCode >= 48 && keyCode <= 57)  
           || (keyCode >= 65 && keyCode <=90)
           || (keyCode >= 97 && keyCode <= 122)
           || (keyCode == 46 || keyCode == 45 || keyCode == 64 || keyCode == 95 || keyCode == 8 || keyCode == 9)
           || (keyCode >= 1776 && keyCode <= 1785)
           || (keyCode >= 1601 && keyCode <= 1610)
           || (keyCode >= 1569 && keyCode <= 1594)
           || (keyCode == 1688 || keyCode == 1648 || keyCode ==1740))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // If the password field is empty, use default css style.
    function password_onblur()
    {
        var passwordID = "<%=txbPassword1.ClientID %>";
   
        var password = GetValueById(passwordID);
    
        if (password == "")
        {
            setToDefault();
            return;
        }
    }
</script>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaTextBox runat="server" MinLength="4" MaxLength="50" ID="txbUserName" 
        Validate="MinLength;MaxLength;customvalidation" CustomValidationFunction="checkUserName_onblur"
        CustomValidationMessageKey="acc_reg_error_existUser" onkeypress="return blockChars(event)"
        onpaste="return false" LabelKey="acc_userInfoForm_label_user" SetFocusOnError="false" IsDBRequired="true"/>
    <ACA:AccelaEmailText runat="server" ID="txbEmail"  MaxLength="70"
        Validate="email;customvalidation" CustomValidationFunction="checkEmailAddress_onblur"
        CustomValidationMessageKey="acc_reg_error_existEmail" LabelKey="acc_userInfoForm_label_email"
        SetFocusOnError="false" IsDBRequired="true"/>
    <ACA:AccelaPasswordText ID="txbPassword1" runat="server" LabelKey="acc_userInfoForm_label_passoword" IsDBRequired="true"
        TextMode="Password" MinLength="8" MaxLength="21" autocomplete="off"></ACA:AccelaPasswordText>
    <ACA:AccelaPasswordText ID="txbPassword2" runat="server" LabelKey="acc_userInfoForm_label_typePassoword" IsDBRequired="true"
        TextMode="Password" MaxLength="21" Validate="compare" ToCompare="txbPassword1" autocomplete="off"></ACA:AccelaPasswordText>
    <ACA:AccelaMultipleControl ID="ddlQuestionForDaily" ValidationIgnoreCase="True" runat="server" LabelKey="acc_userInfoForm_label_securityQuestion" ChildControlSubLabel="aca_multiplesecurityquestion_label_subquestion" ChildControlSubLabelFullName="aca_multiplesecurityquestion_label_subquestionfullname" IsFieldRequired="true" ChildControlType="Text">
        <DuplicateValidate NeedValidate="True" MessageLabelKey="aca_securityquestion_edit_msg_duplicate" />
        <TextBoxSet MaxLength="150" TrimValue="True"/>
    </ACA:AccelaMultipleControl>
    <ACA:AccelaMultipleControl ID="txbAnswerForDaily" runat="server" LabelKey="acc_userInfoForm_label_answer" ChildControlSubLabel="aca_multiplesecurityquestion_label_subanswer" ChildControlSubLabelFullName="aca_multiplesecurityquestion_label_subanswerfullname" IsFieldRequired="true" ChildControlType="Text">
        <TextBoxSet MaxLength="100"/>
    </ACA:AccelaMultipleControl>
    <ACA:AccelaTextBox ID="ddlQuestion" runat="server" Visible="False" LabelKey="acc_userInfoForm_label_securityQuestion" IsDBRequired="true"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txbAnswer" runat="server" Visible="False" MaxLength="20" LabelKey="acc_userInfoForm_label_answer" IsDBRequired="true"></ACA:AccelaTextBox>
    <ACA:AccelaPhoneText ID="txbMobilePhone" runat="server" LabelKey="acc_userinfoform_label_mobile"></ACA:AccelaPhoneText>
    <ACA:AccelaCheckBox ID="cbReceiveSMS" runat="server" IsDisplayLabel="true" LabelKey="acc_userinfoform_label_receivesms">
    </ACA:AccelaCheckBox>
</ACA:AccelaFormDesignerPlaceHolder>
<ACA:AccelaInlineScript runat="server">
<% if (!AppSession.IsAdmin) {%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=cbReceiveSMS.ClientID %>').click(function () {
                verifyPhoneNumber(false);
            });

            $('#<%=txbMobilePhone.ClientID %>').bind('blur', function () {
                var isShowErrorMessage = false;

                // If first load, leave the foucs will not show error.
                // If click Registration button and exist validate fail control, the mobile phone control need show validate error if fail.
                if (Page_Validators && Page_Validators.length > 0) {
                    for (var i = 0; i < Page_Validators.length; i++) {
                        if (!Page_Validators[i].isvalid) {
                            isShowErrorMessage = true;
                            break;
                        }
                    }
                }

                verifyPhoneNumber(isShowErrorMessage);
            });
        });
    </script>
<% }%>
</ACA:AccelaInlineScript>
<uc1:PasswordSecurityBar ID="ucPasswordSecurityBar1" runat="server" />
<script type="text/javascript">
<% if (!AppSession.IsAdmin) {%>
    with (Sys.WebForms.PageRequestManager.getInstance()) {
        add_pageLoaded(function () {
            if ($('#<%=txbPassword1.ClientID %>').is(':visible') && $('#<%=txbPassword2.ClientID %>').is(':visible')) {
                $("#<%=txbPassword1.ClientID %>_parentGrid").append($("#indicatorTable"));
            }
            else {
                //Hide the password security bar if password field is hidden.
                $('#indicatorTable').hide();
            }

            for (var i = 0; i < validatorNames.length; i++) {
                RefreshValidator(validatorNames[i], false);
            }
        });
        add_endRequest(function () {
            if ($('#<%=txbUserName.ClientID%>').is(':visible')) {
                ValidatorValidate(document.getElementById("<%=txbUserName.ClientID%>_custom_vad"));
            }

            if ($('#<%=txbEmail.ClientID%>').is(':visible')) {
                ValidatorValidate(document.getElementById("<%=txbEmail.ClientID%>_custom_vad"));
            }

            if (!<%=IsAccountRecoverAction.ToString().ToLower() %>) {
                verifyPhoneNumber(false, true);
            }
        });
    }

<% }%>

    function verifyPhoneNumber(isShowErrorMsg, isFromEndRequest) {
        var objLabel0 = document.getElementById('<%=cbReceiveSMS.ClientID %>');
        var mobilePhoneClientId = "<%=txbMobilePhone.ClientID %>";
        var objLabel1 = document.getElementById(mobilePhoneClientId);
        var mobileValidations = '<%=txbMobilePhone.Validate %>';
        var mobileIsRequired = mobileValidations.indexOf('required') > -1;

        // return true indicate valid if not exist the control
        if (objLabel0 == null || objLabel1 == null) {
            return true;
        }

        var isFieldRequired = objLabel0.checked || mobileIsRequired;

        var $requiredTag = $('#<%=txbMobilePhone.ClientID %>_label_0');

        if (isFieldRequired) {
            // add the required tag and validate error message
            var requiredText = '<%=LabelUtil.GetGlobalTextByKey("aca_required_field")%>';

            $requiredTag.html('<div class="ACA_Required_Indicator">*<span id="span' + mobilePhoneClientId + '"></div>');
            setAccelaControlTip(mobilePhoneClientId, requiredText);
        }
        else {
            // clear required tag and validate error message
            $requiredTag.html('');
            setAccelaControlTip(mobilePhoneClientId);
            RemoveValidationFromRequiredControl(mobilePhoneClientId, "span" + mobilePhoneClientId);
        }

        if (objLabel0.checked && !mobileIsRequired) {
            //if phone’s number is invalid the cursor can not locate to other filed.
            AddValidationToRequiedControl(mobilePhoneClientId, "span" + mobilePhoneClientId, false);
        }
        
        if (isFieldRequired && GetValue(objLabel1) == '') {
            if (isShowErrorMsg) {
                doErrorCallbackFun('', objLabel1.id, 2);
                
                if($.exists(("span" + mobilePhoneClientId))) {
                    ValidatorValidate(document.getElementById("span" + mobilePhoneClientId));
                }
            }

            return false;
        }

        // clear validate error message
        if (!isFromEndRequest && (!isFieldRequired || GetValue(objLabel1) != '')) {
            doErrorCallbackFun('', objLabel1.id, 0);
            Field_ClientValidate(mobilePhoneClientId, '');
        }
        return true;
    }
</script>