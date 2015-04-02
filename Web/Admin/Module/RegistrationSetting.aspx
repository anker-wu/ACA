<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Admin.RegistrationSetting" Codebehind="RegistrationSetting.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration Setting</title>
    <link rel="stylesheet" type="text/css" href="../resources/css/ext-all.css" />
    
    <script type="text/javascript" src="../../scripts/jquery.js"></script>
    <script type="text/javascript" src="../../scripts/GlobalConst.aspx"></script>
    <script type="text/javascript" src="../../scripts/global.js"></script>
    <script type="text/javascript" src="../adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../scripts/ext-all.js"></script>
    <script type="text/javascript" src="../scripts/ext-extension.js"></script>
    <script type="text/javascript" src="../TreeJSON/LabelKey.aspx"></script>

    <script type="text/javascript">    
    var ischange;
    var moduleName;
    
    //Create page information array.
    function PageItems(pageId, moduleName, chkRequireLisOption, chkInterval, 
        txtInterval, chkAddLisOption, chkRemoveLisOption, chkPasswordExpiration, txtPasswordExpirationDays,
        chkPasswordFailedAttempts, txtPasswordFailedTimes, txtPasswordFailedDurations, chkEnableRecaptchaForRegistration,
        chkEnableRecaptchaForLogin, chkEnableAutoActivation, chkEnableResetPassordOnCombine, chkEnableLoginOnRegistration,
		txtLoginExpireTime, chkSecurityQuestion, txtSecurityQuestionQuantity)
    {
        this.PageId = pageId;
        this.ModuleName = moduleName;        
        
        this.chkInterval=chkInterval;
        this.txtInterval=txtInterval;        
        this.chkRequireLisOption = chkRequireLisOption;
        this.chkAddLisOption = chkAddLisOption;
        this.chkRemoveLisOption = chkRemoveLisOption;
        this.chkPasswordExpiration = chkPasswordExpiration;
        this.txtPasswordExpirationDays = txtPasswordExpirationDays;
        this.chkPasswordFailedAttempts = chkPasswordFailedAttempts;
        this.txtPasswordFailedTimes = txtPasswordFailedTimes;
        this.txtPasswordFailedDurations = txtPasswordFailedDurations;
        this.chkEnableRecaptchaForRegistration = chkEnableRecaptchaForRegistration;
        this.chkEnableRecaptchaForLogin = chkEnableRecaptchaForLogin;
        this.chkEnableAutoActivation = chkEnableAutoActivation;
        this.chkEnableLoginOnRegistration = chkEnableLoginOnRegistration;
        this.chkEnableResetPassordOnCombine = chkEnableResetPassordOnCombine;
        this.txtLoginExpireTime = txtLoginExpireTime;
        this.chkSecurityQuestion = chkSecurityQuestion;
        this.txtSecurityQuestionQuantity = txtSecurityQuestionQuantity;
    }
   
    //Enable chkInterval checkbox.
    function EnableInterval(obj)
    {        
        var chkInterval = document.getElementById("chkIntervalActivate").checked;
        var txtInterval = document.getElementById("txtIntervalDay");
        
        if(chkInterval)
        {
            txtInterval.disabled = false;
        }
        else
        {
            txtInterval.disabled = true;
        }
        UpdateDataInfo();
    }
    
    // Enable password expiration setting
    function EnablePasswordExpriation(obj)
    {
        var chkPasswordExpiration = document.getElementById("chkPasswordExpiration").checked;
        var txtPasswordExpirationDays = document.getElementById("txtPasswordExpirationDays");
        
        if(chkPasswordExpiration)
        {
            txtPasswordExpirationDays.disabled = false;
        }
        else
        {
            txtPasswordExpirationDays.disabled = true;
        }
        
        UpdateDataInfo();
    }

    // Enable login expiration setting
    function EnableLoginExpriation(obj) {
        var chkEnableLoginOnRegistration = document.getElementById("chkEnableLoginOnRegistration").checked;
        var txtLoginExpireTime = document.getElementById("txtLoginExpireTime");

        if (chkEnableLoginOnRegistration) {
            txtLoginExpireTime.disabled = false;
        }
        else {
            txtLoginExpireTime.disabled = true;
            txtLoginExpireTime.value = "";
        }

        UpdateDataInfo();
    }

    // Enable password Failed Attempts setting
    function EnablePasswordFailedAttempts(obj)
    {
        var chkPasswordFailedAttempts = document.getElementById("chkPasswordFailedAttempts").checked;
        var txtPasswordFailedTimes = document.getElementById("txtPasswordFailedTimes");
        var txtPasswordFailedDurations = document.getElementById("txtPasswordFailedDurations");
        
        if(chkPasswordFailedAttempts)
        {
            txtPasswordFailedTimes.disabled = false;
            txtPasswordFailedDurations.disabled = false;
        }
        else
        {
            txtPasswordFailedTimes.disabled = true;
            txtPasswordFailedDurations.disabled = true;
        }
        
        UpdateDataInfo();
    }
    
    //Update email setting data.
    function UpdateDataInfo()
    {
        var needSave = true;
        if (arguments.length == 1) needSave = arguments[0];
        
        moduleName = parent.parent.Ext.Const.ModuleName;
        var chkInterval = document.getElementById("chkIntervalActivate").checked;
        var txtInterval = document.getElementById("txtIntervalDay").value;
        // password settings
        var chkPasswordExpiration = document.getElementById("chkPasswordExpiration").checked;
        var txtPasswordExpirationDays = document.getElementById("txtPasswordExpirationDays").value;
        var chkPasswordFailedAttempts = document.getElementById("chkPasswordFailedAttempts").checked;
        var txtPasswordFailedTimes = document.getElementById("txtPasswordFailedTimes").value;
        var txtPasswordFailedDurations = document.getElementById("txtPasswordFailedDurations").value;
        
        var chkRequireLicense = document.getElementById("chkRequireLicense");
        var chkRequireLisOption = null;
        var chkAddLicense = document.getElementById("chkAddLicense");
        var chkAddLisOption = null;
        var chkRemoveLicense = document.getElementById("chkRemoveLicense");
        var chkRemoveLisOption = null;
        var chkEnableRecaptchaForRegistration = document.getElementById("chkEnableRecaptchaForRegistration").checked;
        var chkEnableRecaptchaForLogin = document.getElementById("chkEnableRecaptchaForLogin").checked;

        var chkEnableAutoActivation = document.getElementById("chkEnableAutoActivation").checked;
        var chkEnableLoginOnRegistration = document.getElementById("chkEnableLoginOnRegistration").checked;
        var txtLoginExpireTime = document.getElementById("txtLoginExpireTime").value;
        var chkEnableResetPassordOnCombine = document.getElementById("chkEnableResetPassordOnCombine").checked;
        
        chkRequireLisOption = chkRequireLicense != null && chkRequireLicense.checked;
        chkAddLisOption = chkAddLicense != null && chkAddLicense.checked;
        chkRemoveLisOption = chkRemoveLicense != null && chkRemoveLicense.checked;

        var chkSecurityQuestion = document.getElementById("chkSecurityQuestion").checked;
        var txtSecurityQuestionQuantity = document.getElementById("txtSecurityQuestionQuantity").value;

        var pageItems = new PageItems('registration', moduleName, chkRequireLisOption, chkInterval, txtInterval,
         chkAddLisOption, chkRemoveLisOption, chkPasswordExpiration, txtPasswordExpirationDays, chkPasswordFailedAttempts,
         txtPasswordFailedTimes, txtPasswordFailedDurations, chkEnableRecaptchaForRegistration, chkEnableRecaptchaForLogin, chkEnableAutoActivation,
         chkEnableResetPassordOnCombine, chkEnableLoginOnRegistration, txtLoginExpireTime, chkSecurityQuestion, txtSecurityQuestionQuantity);
        
        parent.parent.pageRegistrationItems.UpdatePageItem('registration',pageItems);
        
        if(needSave == false) return;
        
        parent.parent.ModifyMark();
    }
   
    //Fill Interval information.
    function SettingIntervalActivate(registrationObj)
    {
        var divIntervalHead = document.getElementById("divIntervalHead");
        var chkIntervalActivate = document.getElementById("chkIntervalActivate");
        var txtIntervalDay = document.getElementById("txtIntervalDay");
        var divIntervalDayShow=document.getElementById("divIntervalDayShow");

        if (registrationObj.EnablePurgeExpiredAccountInterval == "A")
        {
            chkIntervalActivate.checked = true;
            txtIntervalDay.value = registrationObj.PurgeExpiredAccountInterval;
        }
        else
        {
            chkIntervalActivate.checked = false;
            txtIntervalDay.value = registrationObj.PurgeExpiredAccountInterval;
            txtIntervalDay.disabled = true; 
        }

        divIntervalHead.innerHTML = registrationObj.IntervalDataLableContent;
        divIntervalDayShow.innerHTML = registrationObj.IntervalDataLableDay;
    } 
   
    //Fill password settings.
    function SettingPassword(registrationObj)
    {
        var chkPasswordExpiration = document.getElementById("chkPasswordExpiration");
        var txtPasswordExpirationDays = document.getElementById("txtPasswordExpirationDays");
        var chkPasswordFailedAttempts = document.getElementById("chkPasswordFailedAttempts");
        var txtPasswordFailedTimes = document.getElementById("txtPasswordFailedTimes");
        var txtPasswordFailedDurations = document.getElementById("txtPasswordFailedDurations");

        if (registrationObj.EnablePasswordExpires == "Y") {
            chkPasswordExpiration.checked = true;
            txtPasswordExpirationDays.value = registrationObj.PasswordExpires;
        }
        else {
            chkPasswordExpiration.checked = false;
            txtPasswordExpirationDays.value = registrationObj.PasswordExpires;
            txtPasswordExpirationDays.disabled = true;
        }

        if (registrationObj.EnableLockAccountAfter == "Y") {
            chkPasswordFailedAttempts.checked = true;
            txtPasswordFailedTimes.value = registrationObj.LockAccountAfter;
            txtPasswordFailedDurations.value = registrationObj.LockAccountDurationHours;
        }
        else {
            chkPasswordFailedAttempts.checked = false;
            txtPasswordFailedTimes.value = registrationObj.LockAccountAfter;
            txtPasswordFailedDurations.value = registrationObj.LockAccountDurationHours;
            txtPasswordFailedTimes.disabled = true;
            txtPasswordFailedDurations.disabled = true;
        }
    }

    //Get register data information.
    function GetRegistrationDataInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetRegistrationDataInfo(CallBackForRegistration);
    }

    function CallBackForRegistration(result) {
        var registrationObj = eval("("+result+")");
        if (registrationObj != null) {
            var chkEnableRecaptchaForRegistration = document.getElementById("chkEnableRecaptchaForRegistration");
            var chkEnableRecaptchaForLogin = document.getElementById("chkEnableRecaptchaForLogin");
            var chkEnableAutoActivation = document.getElementById("chkEnableAutoActivation");

            if (registrationObj.RecaptChaForRegistration != null) {
                chkEnableRecaptchaForRegistration.checked = registrationObj.RecaptChaForRegistration.toLowerCase() == "yes" || registrationObj.RecaptChaForRegistration.toLowerCase() == "y";
            }

            if (registrationObj.RecaptChaForLogin != null) {
                chkEnableRecaptchaForLogin.checked = registrationObj.RecaptChaForLogin.toLowerCase() == "yes" || registrationObj.RecaptChaForLogin.toLowerCase() == "y";
            }

            if (registrationObj.EnableAutoActivation != null) {
                chkEnableAutoActivation.checked = registrationObj.EnableAutoActivation.toLowerCase() == "no" || registrationObj.EnableAutoActivation.toLowerCase() == "n";
            }
        }

        SettingPassword(registrationObj);
        SettingIntervalActivate(registrationObj);
        SettingRegisterLicense(registrationObj);
        SettingRegistrationLogin(registrationObj);
        SettingResetPassword(registrationObj);
    }

    //get option value for register license.
    function SettingRegisterLicense(registrationObj)
    {
        if (registrationObj != null)
        {
            var chkRequireLicense = document.getElementById("chkRequireLicense");
            var chkAddLicense = document.getElementById("chkAddLicense");
            var chkRemoveLicense = document.getElementById("chkRemoveLicense");

            if (chkRequireLicense != null && registrationObj.EnableRequireLicense != null)
            {
                chkRequireLicense.checked = registrationObj.EnableRequireLicense.toLowerCase() == "yes" || registrationObj.EnableRequireLicense.toLowerCase() == "y";
            }

            if (chkAddLicense != null && registrationObj.DisableAddLicense != null)
            {
                chkAddLicense.checked = registrationObj.DisableAddLicense.toLowerCase() == "yes" || registrationObj.DisableAddLicense.toLowerCase() == "y";
            }

            if (chkRemoveLicense != null && registrationObj.DisableRemoveLicense != null)
            {
                chkRemoveLicense.checked = registrationObj.DisableRemoveLicense.toLowerCase() == "yes" || registrationObj.DisableRemoveLicense.toLowerCase() == "y";
            }
        }
    }

    //get option value for registration login.
    function SettingRegistrationLogin(registrationObj) {
        if (registrationObj != null) {
            var chkEnableLoginOnRegistration = document.getElementById("chkEnableLoginOnRegistration");
            var txtLoginExpireTime = document.getElementById("txtLoginExpireTime");

            if (chkEnableLoginOnRegistration != null && registrationObj.EnableLoginOnRegistration != null) {
                var isChecked = registrationObj.EnableLoginOnRegistration.toLowerCase() == "yes" || registrationObj.EnableLoginOnRegistration.toLowerCase() == "y";
                chkEnableLoginOnRegistration.checked = isChecked;
                txtLoginExpireTime.value = registrationObj.LoginExpireTime;
            }
            if (!(registrationObj.EnableLoginOnRegistration.toLowerCase() == "yes" || registrationObj.EnableLoginOnRegistration.toLowerCase() == "y" || registrationObj.EnableLoginOnRegistration.toLowerCase() != "n")) {
                txtLoginExpireTime.disabled = true;
            }
        }
    }

    //get option value for reset password.
    function SettingResetPassword(registrationObj) {
        if (registrationObj != null) {
            var chkEnableResetPassordOnCombine = document.getElementById("chkEnableResetPassordOnCombine");

            if (chkEnableResetPassordOnCombine != null && registrationObj.EnableResetPasswordOnCombine != null) {
                chkEnableResetPassordOnCombine.checked = registrationObj.EnableResetPasswordOnCombine.toLowerCase() == "yes" || registrationObj.EnableResetPasswordOnCombine.toLowerCase() == "y";
            }
        }
    }

    function EnableSecurityQuestionQuantity() {
        DealSecurityQuestionQuantityStatus();
        UpdateDataInfo();
    }
    
    function DealSecurityQuestionQuantityStatus() {
        var chkSecurityQuestion = document.getElementById("chkSecurityQuestion");
        var txtSecurityQuestionQuantity = document.getElementById("txtSecurityQuestionQuantity");

        if (chkSecurityQuestion.checked) {
            txtSecurityQuestionQuantity.disabled = false;
        }
        else {
            txtSecurityQuestionQuantity.value = 1;
            txtSecurityQuestionQuantity.disabled = true;
        }
    }

    function GetAuthBySecurityQuestionInfo() {
        Accela.ACA.Web.WebService.AdminConfigureService.GetAuthBySecurityQuestionSetting(function(result) {
            var arrResult = eval('(' + result + ')');

            if (arrResult.Enable) {
                document.getElementById("chkSecurityQuestion").checked = true;
            }

            document.getElementById("txtSecurityQuestionQuantity").value = arrResult.CompulsoryQuantity;
            DealSecurityQuestionQuantityStatus();
        });
    }
    </script>

</head>
<body style="margin-left:6px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="../../WebService/AdminConfigureService.asmx" />
            </Services>
        </asp:ScriptManager>
        <span style="display:none">
            <ACA:AccelaHideLink ID="hlSkipToolBar" runat="server" AltKey="img_alt_modulemenu_skiplink" IsSkippingToAnchor="true" NextControlClientID="FirstAnchorInACAMainContent" TabIndex="-1"/>
            <a name="FirstAnchorInACAMainContent" id="FirstAnchorInACAMainContent" tabindex="-1"></a>
        </span>
        <span id="FirstAnchorInAdminMainContent" tabindex="-1"></span>
        <div class="ACA_PaddingStyle">
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset style="padding: 8px; width: 96%;">
                        <legend>
                                <ACA:AccelaLabel ID="admin_register_setting_label_license_title" runat="server" LabelKey="admin_register_setting_label_license_title" CssClass="ACA_New_Title_Label font12px" />
                        </legend>
                        <br />
                        <div class="ACA_NewDiv_Text">
                            <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                                <div id="div1" class="ACA_New_Head_Label_Width_100 font11px">
                                    <ACA:AccelaLabel ID="admin_register_setting_label_option_disclaimer" runat="server" LabelKey="admin_register_setting_label_option_disclaimer"/>
                                </div>
                            </div>
                        </div>
                        <div class="ACA_NewDiv_Text_TabRow_Margin_Top_10 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                            <table role="presentation">
                                <tr>
                                    <td>
                                        <ACA:AccelaCheckBox ID="chkRequireLicense" runat="server" LabelKey="admin_register_setting_label_requireLicense" />
                                    </td>
                                    <td>
                                        <ACA:AccelaCheckBox ID="chkAddLicense" runat="server" LabelKey="admin_register_setting_disable_add_license" />
                                    </td>
                                    <td>
                                        <ACA:AccelaCheckBox ID="chkRemoveLicense" runat="server" LabelKey="admin_register_setting_disable_remove_license" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset> 
                    <fieldset style="padding: 8px; width: 96%">
                      <legend>
                          <ACA:AccelaLabel ID="IbIIntervalTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize_Smaller" LabelKey="admin_registration_setting_label_interval_title"></ACA:AccelaLabel>
                      </legend> 
                     <div class="ACA_NewDiv_Text">
                         <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                             <div id="divIntervalHead" class="ACA_New_Head_Label_Width_100 font11px">
                             </div> 
                         </div>
                     </div> 
                     <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                       <table role="presentation"> 
                         <tr>
                           <td>
                                <ACA:AccelaCheckBox ID="chkIntervalActivate" runat="server" LabelKey="admin_registration_setting_label_interval_activate">
                                 </ACA:AccelaCheckBox> 
                           </td>
                          <td>
                             <div>
                                <ACA:AccelaNumberText ID="txtIntervalDay" MaxLength="6" IsNeedDot="false" runat="server" ToolTip="Please input a purge expired account interval day." >
                                </ACA:AccelaNumberText>
                              </div>
                          </td>
                          <td>
                             <div style="color:#666666;font-size:12px;font-weight:bold;font-family:Arial, sans-serif;">
                               <ACA:AccelaLabel ID="divIntervalDayShow" runat="server">
                               </ACA:AccelaLabel>
                              </div> 
                          </td>    
                         </tr>                      
                       </table>
                     </div> 
                    </fieldset>
                    <fieldset class="RegistrationSetting_Section">
                      <legend>
                          <ACA:AccelaLabel ID="lblPasswordTitle" runat="server" CssClass="ACA_New_Title_Label ACA_Label_FontSize_Smaller" LabelKey="admin_registration_setting_label_password_title"/>
                      </legend> 
                     <div class="ACA_NewDiv_Text">
                         <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                             <div class="ACA_New_Head_Label_Width_100 font11px">
                             <ACA:AccelaLabel ID="lblPasswordExpirationDisclaimer" runat="server" LabelKey="admin_registration_setting_label_password_expiration_disclaimer"/>
                             </div>
                         </div>
                         <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15">
                             <div class="ACA_New_Head_Label_Width_100 font11px">
                             <ACA:AccelaLabel ID="lblPasswordFailAttemptsDisclaimer" runat="server" LabelKey="admin_registration_setting_label_password_failedattempts_disclaimer"/>
                             </div>
                         </div>
                         <div class="RegistrationSetting_Section_Disclaimer">
                             <div class="ACA_New_Head_Label_Width_100">
                             <ACA:AccelaLabel ID="lblAuthBySecurityQuestion" runat="server" LabelKey="acaadmin_registrationsetting_label_authbysecurityquestion_disclaimer"/>
                             </div>
                         </div>
                     </div> 
                     <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                       <table role="presentation">
                          <tr>
                          <td>
                            <ACA:AccelaCheckBox ID="chkPasswordExpiration" runat="server" LabelKey="admin_registration_setting_label_password_expiration_activate"/>
                          </td>
                          <td>
                            <ACA:AccelaNumberText ID="txtPasswordExpirationDays" IsNeedDot="false" runat="server" MaxLength="6" ToolTip="Please input a password expiration day." >
                            </ACA:AccelaNumberText>
                          </td>
                          <td>
                            <ACA:AccelaLabel ID="lblPasswordExpiationUnit" runat="server" LabelKey="admin_registration_setting_label_password_expiration_unit" CssClass="ACA_Label ACA_Label_FontSize">
                            </ACA:AccelaLabel>
                          </td>
                          </tr>
                       </table>
                       <table role="presentation">
                        <tr>
                        <td>
                           <ACA:AccelaCheckBox ID="chkPasswordFailedAttempts" runat="server" LabelKey="admin_registration_setting_label_password_failedattempts_activate">
                           </ACA:AccelaCheckBox> 
                         </td>
                         <td>
                           <ACA:AccelaNumberText ID="txtPasswordFailedTimes" IsNeedDot="false" runat="server" MaxLength="6" ToolTip="Please input a password max failed times." >
                           </ACA:AccelaNumberText>
                         </td>
                         <td>
                           <ACA:AccelaLabel ID="lblPasswordFailedAttemptsUnit1" runat="server" LabelKey="admin_registration_setting_label_password_failedattempts_unit1"  CssClass="ACA_Label ACA_Label_FontSize">
                           </ACA:AccelaLabel>
                         </td>
                         <td>
                            <ACA:AccelaNumberText ID="txtPasswordFailedDurations" IsNeedDot="false" runat="server" MaxLength="6" ToolTip="Please input a password failed durations hours.">
                            </ACA:AccelaNumberText>
                         </td>
                         <td>
                            <ACA:AccelaLabel ID="lblPasswordFailedAttemptsUnit2" runat="server" LabelKey="admin_registration_setting_label_password_failedattempts_unit2"  CssClass="ACA_Label ACA_Label_FontSize">
                            </ACA:AccelaLabel>
                         </td>
                         </tr>
                         </table>
                         <table role="presentation">
                           <tr>
                               <td colspan="2">
                                    <ACA:AccelaCheckBox ID="chkSecurityQuestion" runat="server" LabelKey="acaadmin_registrationsetting_label_authbysecurityquestion"></ACA:AccelaCheckBox>
                               </td>
                            </tr>
                            <tr>
                               <td class="CompulsorySecurityQuestion">
                                    <ACA:AccelaLabel ID="lblSecurityQuestion"  CssClass="ACA_Label ACA_Label_FontSize" LabelKey="acaadmin_registrationsetting_label_compulsory_securityquestions" runat="Server"></ACA:AccelaLabel>
                               </td>
                               <td>
                                    <ACA:AccelaNumberText MaxLength="6" ID="txtSecurityQuestionQuantity" IsNeedDot="false" Width="46" runat="server"></ACA:AccelaNumberText>
                               </td>
                           </tr>
                       </table>
                     </div> 
                    </fieldset>
                    <fieldset style="padding: 8px; width: 96%">
                      <legend>
                          <ACA:AccelaLabel ID="lblRecaptChaTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize_Smaller" LabelKey="aca_admin_registration_recaptcha_title"></ACA:AccelaLabel>
                      </legend> 
                     <div class="ACA_NewDiv_Text">
                         <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                             <div class="ACA_New_Head_Label_Width_100 font11px">
                                <ACA:AccelaLabel ID="lblRegistrationEnableRecaptChaDisclaimer" runat="server" LabelKey="aca_admin_registration_enable_recaptcha_disclaimer"></ACA:AccelaLabel>
                             </div> 
                             <div class="ACA_New_Head_Label_Width_100 font11px">
                                <ACA:AccelaLabel ID="lblLoginEnableRecaptChaDisclaimer" runat="server" LabelKey="aca_admin_login_enable_recaptcha_disclaimer"></ACA:AccelaLabel>
                             </div>
                         </div>
                     </div> 
                     <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                       <table role="presentation">
                         <tr>
                           <td>
                                <ACA:AccelaCheckBox ID="chkEnableRecaptchaForRegistration" runat="server" LabelKey="aca_admin_registration_enable_recaptcha_label"/>
                           </td>
                         </tr>
                         <tr>
                           <td>
                                <ACA:AccelaCheckBox ID="chkEnableRecaptchaForLogin" runat="server" LabelKey="aca_admin_login_enable_recaptcha_label"/>
                           </td>
                         </tr>
                       </table>
                     </div> 
                    </fieldset>
                    <fieldset style="padding: 8px; width: 96%">
                      <legend>
                          <ACA:AccelaLabel ID="lblAutoActivationTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize_Smaller" LabelKey="acaadmin_registration_setting_label_autoactivation_title"></ACA:AccelaLabel>
                      </legend> 
                     <div class="ACA_NewDiv_Text">
                         <div class="ACA_NewDiv_Text_TabRow_Margin_Top_5 ACA_NewDiv_Text_TabRow_Margin_Left_15 ACA_NewDiv_Text_TabRow_Margin_Right_15 ACA_NewDiv_Text_TabRow_Padding_Bottom_8">
                             <div class="ACA_New_Head_Label_Width_100 font11px">
                                <ACA:AccelaLabel ID="lblAutoActivationHead" runat="server" LabelKey="acaadmin_registration_setting_label_autoactivation_head"></ACA:AccelaLabel>
                             </div>
                         </div>
                     </div> 
                     <div class="ACA_NewDiv_Text_TabRow_Margin_Top_8 ACA_NewDiv_Text_TabRow_Margin_Left_8 ACA_NewDiv_Text_TabRow_Margin_Right_8">
                       <table role="presentation"> 
                         <tr>
                           <td>
                                <ACA:AccelaCheckBox ID="chkEnableAutoActivation" runat="server" LabelKey="acaadmin_registration_setting_label_enable_autoactivation">
                                 </ACA:AccelaCheckBox> 
                           </td>
                         </tr>
                       </table>
                     </div> 
                    </fieldset>
                     <fieldset class="RegistrationSetting_Section">
                      <legend>
                          <ACA:AccelaLabel ID="lblLoginOnRegistrationTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize_Smaller" LabelKey="acaadmin_registrationsetting_label_logintitle"></ACA:AccelaLabel>
                      </legend> 
                     <div class="ACA_NewDiv_Text">
                         <div class="RegistrationSetting_Section_Disclaimer">
                             <div class="ACA_New_Head_Label_Width_100">
                                <ACA:AccelaLabel ID="lblEnableLoginOnRegistration" runat="server" LabelKey="acaadmin_registrationsetting_label_logindisclaimer"></ACA:AccelaLabel>
                             </div> 
                         </div>
                     </div> 
                     <div class="RegistrationSetting_Section_Item">
                       <table role="presentation">
                         <tr>
                           <td>
                                <ACA:AccelaCheckBox ID="chkEnableLoginOnRegistration" runat="server" LabelKey="acaadmin_registrationsetting_label_loginenable"/>
                           </td>
                           <td>
                            <ACA:AccelaNumberText ID="txtLoginExpireTime" IsNeedDot="false" runat="server" MaxLength="6">
                            </ACA:AccelaNumberText>
                          </td>
                           <td>
                            <ACA:AccelaLabel ID="lblLoginExpireTime" runat="server" LabelKey="acaadmin_registrationsetting_label_loginexpiretimeunit" CssClass="ACA_Label ACA_Label_FontSize">
                            </ACA:AccelaLabel>
                         </td>
                         </tr>
                       </table>
                     </div> 
                    </fieldset>
                    <fieldset class="RegistrationSetting_Section">
                      <legend>
                          <ACA:AccelaLabel ID="lblResetPasswordTitle" runat="server" CssClass="ACA_Label ACA_Label_FontSize_Smaller" LabelKey="acaadmin_registrationsetting_label_resetpasswordtitle"></ACA:AccelaLabel>
                      </legend> 
                     <div class="ACA_NewDiv_Text">
                         <div class="RegistrationSetting_Section_Disclaimer">
                             <div class="ACA_New_Head_Label_Width_100">
                                <ACA:AccelaLabel ID="lblResetPasswordDisclaimer" runat="server" LabelKey="acaadmin_registrationsetting_label_resetpassworddisclaimer"></ACA:AccelaLabel>
                             </div> 
                         </div>
                     </div> 
                     <div class="RegistrationSetting_Section_Item">
                       <table role="presentation">
                         <tr>
                           <td>
                                <ACA:AccelaCheckBox ID="chkEnableResetPassordOnCombine" runat="server" LabelKey="acaadmin_registrationsetting_label_combineresetpassword"/>
                           </td>
                         </tr>
                       </table>
                     </div> 
                    </fieldset>
                    <div>
                        <asp:HiddenField ID="hdfFlag" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
    </form>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</body>
</html>
