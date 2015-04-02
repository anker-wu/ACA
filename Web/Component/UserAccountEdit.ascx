
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountEdit.ascx.cs"
    Inherits="Accela.ACA.Web.Component.UserAccountEdit" %>
<%@ Register Src="~/Component/PasswordSecurityBar.ascx" TagName="PasswordSecurityBar" TagPrefix="uc1" %>

<script type="text/javascript">
var globalEvent;
var globalValidators;
var globalPwdValidators;
var userNamevalidatorIsValid=true;
var passwordSecurityValidatorValid = true;
var validatorIsValid=true;
var repeatCallTimes=0;
var passwordErrorMsg = "";

function SetCustomValidate(isCheckUserName,source,value,arguments)
{
    var validators=source.previousSibling.previousSibling.previousSibling.Validators;
    if(repeatCallTimes<1)
    { 
         globalValidators=validators;
         if(isCheckUserName)
            PageMethods.IsExistUserName(value,CallSuccess);
         else
           PageMethods.IsExistEmail(value,CallSuccess);    
    } 
    else
    {
        arguments.IsValid=validatorIsValid;
        repeatCallTimes=0;
        if(isCheckUserName)
        {
            userNamevalidatorIsValid=validatorIsValid;    
        }      
    } 
}

function CheckPasswordSecurity_onblur(source, arguments)
{
    var passwordID = "<%=txbNewPassword1.ClientID %>";
    var userNameID = "<%=txbUserID.ClientID %>";
    
    var password = GetValueById(passwordID);
    var userName = GetValueById(userNameID);
    
    if (password == "")
    {
         setToDefault();
         return;
    }
    
    var validators = source.previousSibling.Validators;
    if (validators == null) {
        validators = source.previousSibling.previousSibling.Validators;
    }
    if (repeatCallTimes < 1) {
        globalPwdValidators = validators;
        PageMethods.CheckPasswordSecurity(password, userName, false, PasswordCallSuccess);
    }
    else {
        arguments.IsValid = passwordSecurityValidatorValid;
        repeatCallTimes = 0;
    }
    
    if (!passwordSecurityValidatorValid)
    {
        source.errormessage = passwordErrorMsg;
        arguments.IsValid=passwordSecurityValidatorValid;
    }
}

function PasswordCallSuccess(result)
{
    var elemID = "<%=txbNewPassword1.ClientID %>";

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


function checkEditEmailAddress_onblur(source, arguments)
{
    var value = GetValueById("<%=txbEmailID.ClientID %>");
    SetCustomValidate(false, source, value, arguments);
    if (!validatorIsValid) {
        arguments.IsValid = false;
    }
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
   validatorIsValid= result=="";    
   repeatCallTimes++;

    if(typeof(globalValidators) != 'undefined')
    {
        for (var i = 0; i < globalValidators.length; i++)
        {
            ValidatorValidate(globalValidators[i], null, globalEvent);
        }
    }
    ValidatorUpdateIsValid(); 
}

function blockChars(oEvent)
{
    var keyCode = oEvent.keyCode ? oEvent.keyCode : oEvent.which;
    
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
    var passwordID = "<%=txbNewPassword1.ClientID %>";
    
    var password = GetValueById(passwordID);
    
    if (password == "")
    {
         setToDefault();
         return;
    }
}
</script>
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaTextBox runat="server" MinLength="4" MaxLength="50" ID="txbUserID" CssClass="ACA_XLong"
        BackColor="LightGray" ReadOnly="true" LabelKey="acc_userInfoForm_label_user" IsDBRequired="true" />
    <ACA:AccelaEmailText runat="server" ID="txbEmailID" CssClass="ACA_XLong" MaxLength="70"
        Validate="required;email;customvalidation" CustomValidationFunction="checkEditEmailAddress_onblur"
        CustomValidationMessageKey="acc_reg_error_existEmail" LabelKey="acc_userInfoForm_label_email"
        SetFocusOnError="false" IsDBRequired="true" />
    <ACA:AccelaPasswordText ID="txbOldPassword" runat="server" CssClass="ACA_XLong" Validate="minlength;maxlength"
        LabelKey="acc_emailQuestionEdit_label_oldPassword" TextMode="Password" MinLength="0"
        MaxLength="21" autocomplete="off" IsDBRequired="true"></ACA:AccelaPasswordText>
    <ACA:AccelaPasswordText ID="txbNewPassword1" runat="server" CssClass="ACA_XLong"
        LabelKey="acc_emailQuestionEdit_label_newPassword" TextMode="Password" MinLength="8"
        MaxLength="21" autocomplete="off" IsDBRequired="true"></ACA:AccelaPasswordText>
    <ACA:AccelaPasswordText ID="txbNewPassword2" runat="server" CssClass="ACA_XLong"
        Validate="compare" LabelKey="acc_emailQuestionEdit_label_confirmPassword" TextMode="Password"
        MaxLength="21" ToCompare="txbNewPassword1" autocomplete="off" IsDBRequired="true"></ACA:AccelaPasswordText>
    <ACA:AccelaMultipleControl ID="ddlQuestionForDaily" runat="server" ValidationIgnoreCase="True" LabelKey="acc_userInfoForm_label_securityQuestion" ChildControlSubLabel="aca_multiplesecurityquestion_label_subquestion" ChildControlSubLabelFullName="aca_multiplesecurityquestion_label_subquestionfullname" IsFieldRequired="true" ChildControlType="Text">
        <DuplicateValidate NeedValidate="True" MessageLabelKey="aca_securityquestion_edit_msg_duplicate" />
        <TextBoxSet MaxLength="150" TrimValue="True"/>
    </ACA:AccelaMultipleControl>
    <ACA:AccelaMultipleControl ID="txbAnswerForDaily" runat="server" LabelKey="acc_userInfoForm_label_answer" ChildControlSubLabel="aca_multiplesecurityquestion_label_subanswer" ChildControlSubLabelFullName="aca_multiplesecurityquestion_label_subanswerfullname" IsFieldRequired="true" ChildControlType="Text">
        <TextBoxSet MaxLength="100"></TextBoxSet>
    </ACA:AccelaMultipleControl>
    <ACA:AccelaTextBox ID="ddlQuestion" runat="server" Visible="False" LabelKey="acc_userInfoForm_label_securityQuestion" IsDBRequired="true"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txbAnswer" runat="server" Visible="False" LabelKey="acc_userInfoForm_label_answer" MaxLength="20" IsDBRequired="true"></ACA:AccelaTextBox>
    <ACA:AccelaPhoneText ID="txbMobilePhone" runat="server" LabelKey="acc_userinfoform_label_mobile"></ACA:AccelaPhoneText>
    <ACA:AccelaCheckBox ID="cbReceiveSMS" runat="server" IsDisplayLabel="true" LabelKey="acc_userinfoform_label_receivesms">
    </ACA:AccelaCheckBox>
</ACA:AccelaFormDesignerPlaceHolder>
<ACA:AccelaInlineScript runat="server">
<% if (!AppSession.IsAdmin) {%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txbNewPassword1.ClientID %>_parentGrid").append($("#indicatorTable"));

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

            // After DOM is loaded, we should make sure the mobile phone
            // field is required if the receive SMS checkbox is checked.
            var isReceiveSms = $('#<%=cbReceiveSMS.ClientID %>').attr('checked');
            setMobilePhoneRequired(isReceiveSms);
        });
    </script>
<% }%>
</ACA:AccelaInlineScript>
<uc1:passwordsecuritybar id="ucPasswordSecurityBar2" runat="server" />
<ACA:AccelaHeightSeparate ID="sepHeightForAccountEdit" runat="server" Height="5" />
<script type="text/javascript">
    <% if (!AppSession.IsAdmin) {%>
    with (Sys.WebForms.PageRequestManager.getInstance()) {
        add_endRequest(function () {
            ValidatorValidate(document.getElementById("<%=txbEmailID.ClientID%>_custom_vad"));
            verifyPhoneNumber(false, true);
        });

        add_pageLoaded(function () {
            attachChangingEvent4PwdFields();
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
        setMobilePhoneRequired(isFieldRequired);

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

    var pwdFieldIds = ['<%=txbOldPassword.ClientID %>', '<%=txbNewPassword1.ClientID %>', '<%=txbNewPassword2.ClientID %>'];

    //Get the exclude id array which needn't to validate.
    function getValidationExcludedIds() {
        var result = [];
        var isPwdFilled = false;

        $(pwdFieldIds).each(function(index, id) {
            if ($.exists('#' + id)) {
                if (!isNullOrEmpty($('#' + id).val())) {
                    isPwdFilled = true;
                    return false;
                }

                result.push(id);
            }
        });

        if (isPwdFilled) {
            result = [];
        }

        return result;
    }

    //Attach changing event for password fields.
    function attachChangingEvent4PwdFields() {
        $(pwdFieldIds).each(function(index, id) {
            if ($.exists('#' + id)) {
                obj = document.getElementById(id);

                if ($.browser.msie) { //for IE
                    obj.onpropertychange = setValidationExcludeIds;
                } else { //for other browsers
                    obj.addEventListener("input", setValidationExcludeIds, false);
                }
            }
        });
    }
        
    //Set the validation exclude ids.
    function setValidationExcludeIds(target) {
        var excludeIds = getValidationExcludedIds();
        AddValidationSectionID("<%=ClientID %>");
        SetCurrentValidationSectionID("<%=ClientID %>", excludeIds);

        //Clear the exclude fields' validation error message when clear password value.
        if (typeof (target) == "object" && excludeIds.length > 0) {
            $(excludeIds).each(function(index, id) {
                doErrorCallbackFun('', id, 0);
            });
        }
    }

    function setMobilePhoneRequired(isReceiveSms) {
        var $requiredTag = $('#<%=txbMobilePhone.ClientID %>_label_0');
        var mobilePhoneClientId = '<%= txbMobilePhone.ClientID %>';

        if (isReceiveSms) {
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
    }
</script>