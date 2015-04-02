<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.DelegateManagement" Codebehind="DelegateManagement.ascx.cs" %>

<%@ Register src="DelegateEdit.ascx" tagname="DelegateEdit" tagprefix="ACA" %>
<%@ Register src="Recaptcha.ascx" tagname="Recaptcha" tagprefix="ACA" %>  
<script src="../Scripts/popUpDialog.js" type="text/javascript"></script>	    
<script type="text/javascript">
var validatorIsValid = true;
var globalEvent;
var repeatCallTimes = 0;
var globalValidators;
var moduleListPopUpDialog;

function ConfirmRemoveProxyUser() {
    if (typeof (SetNotAsk) != 'undefined') {
        SetNotAsk();
    }

    return confirm('<%=GetTextByKey("aca_message_confirm_removeproxy").Replace("'","\\'") %>')
}

function ShowModuleList(proxyUserRoleType, backFocusId) {
    moduleListPopUpDialog = new popUpDialog($get('divPopupBox'), $get("<%=hlBegin.ClientID%>"), null, $get('divDeleteManagement'), backFocusId);
    moduleListPopUpDialog.showPopUp();

    $get("<%=hdnAddProxyUserRoleType.ClientID %>").value = proxyUserRoleType;
}

function CloseModuleListWindow() {
    moduleListPopUpDialog.cancel();
}

function RefreshParent() {
    CloseDelegeteWindow();
    window.parent.RefreshParent();
}

function CloseDelegeteWindow() {
    CloseDialog();
}


var delegateEditPopUpDialog

function ShowEditModuleList(proxyUserRoleType, hdnRoleTypeId, hdnBeginFocus, backFocusId) {
    delegateEditPopUpDialog = new popUpDialog($get('divDelegateEditPopupBox'), $get(hdnBeginFocus), null, $get('divDeleteManagement'), backFocusId);
    delegateEditPopUpDialog.showPopUp();

    $get(hdnRoleTypeId).value = proxyUserRoleType;
}

function CloseDelegateEditWindow() {
    delegateEditPopUpDialog.cancel();

    SetFocusBack();
}

function ShowOrHiddenPersonNote() {
    var divPersonNoteInput = $get("<%=divPersonNoteInput.ClientID %>");
    var lnkRemoveNote = $get("<%=lnkRemoveNote.ClientID %>");
    var lnkAddNote = $get("<%=lnkAddNote.ClientID %>");
    var hdnExpand = $get("<%=hdnExpand.ClientID %>");
    var tbPersonNote = $get("<%=tbPersonNote.ClientID %>");

    if (hdnExpand.value == "0") {
        divPersonNoteInput.className = "ACA_Show";
        lnkRemoveNote.className = "ACA_Show";
        lnkAddNote.className = "ACA_Hide";
        hdnExpand.value = "1";
    }
    else {
        divPersonNoteInput.className = "ACA_Hide";
        lnkRemoveNote.className = "ACA_Hide";
        lnkAddNote.className = "ACA_Show";
        hdnExpand.value = "0";
    }

    SetValue(tbPersonNote,'');
}

function handleButton(chkObj) {
    var inputs = chkObj.getElementsByTagName("input");
    var isAllUnchecked = true;
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            isAllUnchecked = false;
            break;
        }
    }

    if ($get('<%=btnSaveModule.ClientID %>') != null) {
        if (isAllUnchecked) {
            $get('<%=btnSaveModule.ClientID %>').parentNode.setAttribute("onclick","return false;");
            $get('<%=btnSaveModule.ClientID %>').disabled = true;
            $get('<%=btnSaveModule.ClientID %>').parentNode.setAttribute("class", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
            $get('<%=btnSaveModule.ClientID %>').parentNode.setAttribute("className", "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize");
        }
        else {
            $get('<%=btnSaveModule.ClientID %>').parentNode.removeAttribute("onclick");
            $get('<%=btnSaveModule.ClientID %>').disabled = false;
            $get('<%=btnSaveModule.ClientID %>').parentNode.setAttribute("class", "ACA_LgButton ACA_LgButton_FontSize");
            $get('<%=btnSaveModule.ClientID %>').parentNode.setAttribute("className", "ACA_LgButton ACA_LgButton_FontSize");
        }
    }
}

function checkProxyEmailAddress_onblur(source, arguments) {
    var value = GetValueById("<%=tbEmailAddress.ClientID %>");
    if (!isFireFox() && !$.browser.opera) {
        globalEvent = event;
        if (event != null) {
            if (event.srcElement != null && repeatCallTimes < 1) {
                globalValidators = event.srcElement.Validators;
                PageMethods.ValidateProxyUserEmail(value, ValidateCallSuccess); 
            }
            else {
                arguments.IsValid = validatorIsValid;
                repeatCallTimes = 0;
            }
        }
        else {
            arguments.IsValid = validatorIsValid;
            repeatCallTimes = 0;
        }
    }
    else {
        SetProxyCustomValidate(source, value, arguments);
    }
       
    source.errormessage = errorMessage;
    arguments.IsValid = validatorIsValid;
}

function SetProxyCustomValidate(source, value, arguments) {
    var validators = source.previousSibling.previousSibling.previousSibling.Validators;
    if (repeatCallTimes < 1) {
        globalValidators = validators;
        PageMethods.ValidateProxyUserEmail(value, ValidateCallSuccess);
    }
    else {
        arguments.IsValid = validatorIsValid;
        repeatCallTimes = 0; 
    }
}

var errorMessage;
function ValidateCallSuccess(result, userContext, methodName) {
    validatorIsValid = result == "";
    errorMessage = result;
    repeatCallTimes++;
    for (var i = 0; i < globalValidators.length; i++) {
        ValidatorValidate(globalValidators[i], null, globalEvent);
    }
    ValidatorUpdateIsValid();
}
</script>

<div id="divDeleteManagement">
<div id="divCreateDelegte" runat="server">
<div class="ACA_Page font11px">
 <asp:UpdatePanel runat="server" ID="upCreate"><ContentTemplate>
    <table role="presentation">
        <tr>
            <td>
                <div class="ACA_BlueLable">
                    <ACA:AccelaTextBox ID="tbNickName" MaxLength="70" CssClass="ACA_MLong" runat="server" LabelKey="aca_delegate_nick_name"></ACA:AccelaTextBox>
                </div> 
            </td>
            <td>
                <div class="ACA_BlueLable">
                    <ACA:AccelaEmailText runat="server" ID="tbEmailAddress"  CssClass="ACA_MLong" MaxLength="50" CustomValidationFunction="checkProxyEmailAddress_onblur"
                        CustomValidationMessageKey="acc_reg_error_existEmail" LabelKey="aca_delegate_email_address" SetFocusOnError="false" /> 
                 </div>
            </td>
        </tr>
    </table>
     <div class="ACA_TabRow ACA_Popup_Title">
        <ACA:AccelaLabel ID="lblSelectPermission" runat="server" LabelKey="aca_delegate_select_permission"></ACA:AccelaLabel>
     </div>
     <div class="ACA_TabRow_Italic">     
        <ACA:AccelaLabel ID="lblViewRecordTitle" runat="server" LabelKey="aca_delegate_viewrecord_title"></ACA:AccelaLabel>
     </div>
    <div class="ACA_FLeft ACA_Label font12px Proxy_ViewRecord_Margin"><ACA:AccelaLabel ID="lblViewRecordRoleTitle" runat="server" IsNeedEncode="false" LabelKey="aca_view_record_description"></ACA:AccelaLabel></div>
    <div class="ACA_FLeft ACA_LinkButton Proxy_ChangeButton_Margin">(<ACA:AccelaLinkButton ID="btnViewRecordPermission" CssClass="NotShowLoading" runat="server" OnClick="ViewRecordPermissionButton_OnClick" CausesValidation="false" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)</div>
    <div id="divHeight" class="ProxyUser_ViewPermission_Divide"></div>
    <div class="ACA_TabRow_Italic">
        <ACA:AccelaLabel ID="lblModuleLevelDescription" runat="server" LabelKey="aca_delegate_module_level_description"></ACA:AccelaLabel>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkCreateApplication" runat="server" AutoPostBack="false" LabelKey="aca_create_application_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnCreateApplication" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClick="CreateApplicationButton_OnClick"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkRenewRecord" runat="server" AutoPostBack="false" LabelKey="aca_renew_record_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnRenew" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClick="RenewButton_OnClick"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">    
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkAmendment" runat="server" AutoPostBack="false" LabelKey="aca_amend_record"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnAmendment" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClick="AmendmentLink_OnClick"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkManageInspections" runat="server" AutoPostBack="false" LabelKey="aca_manage_inspections_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnManageInspections" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClick="ManageInspectionsButton_OnClick"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkManageDocuments" runat="server" AutoPostBack="false" LabelKey="aca_manage_document_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnManageDocuments" CssClass="NotShowLoading" runat="server" OnClick="ManageDocumentsButton_OnClick" CausesValidation="false"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">    
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkMakePayments" runat="server" AutoPostBack="false" LabelKey="aca_make_paments_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnMakePayments" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClick="MakePaymentsLink_OnClick"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div id="divSplit1" class="ProxyUser_PersonNote_Divide"></div>
    <div class="ACA_TabRow" runat="server" id="divPersonNote">
        <span class="ACA_LinkButton font12px nav_bar_separator">
            <ACA:AccelaLinkButton ID="lnkAddNote" runat="server" OnClientClick="ShowOrHiddenPersonNote(); return false;" LabelKey="aca_delegate_add_note"></ACA:AccelaLinkButton>
            <ACA:AccelaLinkButton ID="lnkRemoveNote" class="ACA_Hide" runat="server" OnClientClick="ShowOrHiddenPersonNote(); return false;" LabelKey="aca_delegate_remove_note"></ACA:AccelaLinkButton>
        </span>
        <div id="divPersonNoteInput" class="ACA_Hide" runat="server">
            <ACA:AccelaTextBox ID="tbPersonNote" LabelKey="aca_proxy_personnote" TextMode="MultiLine" runat="server" Rows="6" MaxLength="2000"></ACA:AccelaTextBox>
        </div>
        <asp:HiddenField ID="hdnExpand" runat="server" Value="0" />
    </div>
    <div id="divSplit2" class="ProxyUser_Normal_Divide"></div>
     </ContentTemplate></asp:UpdatePanel>
    <div class="ACA_TabRow">
        <ACA:Recaptcha ID="reCaptcha" runat="server" />
    </div>
    <br />
    <div class="ACA_TabRow">
    <table role='presentation' class="ACA_AlignLeftOrRight">
    <tr valign="bottom">
        <td>
            <div class="ACA_LgButton ACA_LgButton_FontSize">
                <ACA:AccelaButton ID="btnInviteDelegate" OnClick="InviteDelegateButton_OnClick" LabelKey="aca_invite_delegate" runat="server"></ACA:AccelaButton>
            </div>
        </td>
        <td class="PopupButtonSpace">&nbsp;</td>
        <td>
        <div id="divDelegateManagementCancelButton" class="ACA_LinkButton">
            <ACA:AccelaLinkButton ID="btnCancel" LabelKey="aca_delegate_cancel" CausesValidation="false" OnClientClick="CloseDelegeteWindow();return false;" runat="server"></ACA:AccelaLinkButton>
        </div>
        </td>
    </tr>
    </table>
    </div>
    </div>
</div>

<asp:HiddenField ID="hdnAddProxyUserRoleType" runat="server" />
	
	<asp:UpdatePanel ID="upMessage" runat="server"><ContentTemplate>
        <!-- message box begin -->
        <div class="ACA_Page font11px" >
<div id="divPopupBox" class="ACA_Hide PopUpDlg ProxyUser_Popup_Width">
    <ACA:AccelaHideLink ID="hlBegin" AltKey="img_alt_form_begin" Width="0" runat="server" NextControlID="btnSaveModule" />
     <div class="ACA_TabRow ACA_Popup_Title">
        <div class="ACA_AlignRightOrLeft CloseImage ACA_FRight">
            <ACA:AccelaLinkButton id="lnkCancelModuleSave" CssClass="NotShowLoading" runat="server" CausesValidation="false" onclick="CancelModuleSaveButton_OnClick"><img class="ACA_ActionIMG" src='<%=ImageUtil.GetImageURL("closepopup.png") %>' alt="close" /></ACA:AccelaLinkButton>
        </div>
        <ACA:AccelaLabel ID="lblCategories" runat="server" LabelKey="aca_delegate_category_description"></ACA:AccelaLabel>
     </div>
     <div class="ACA_ProxyUser_ScrollX">
    <ACA:AccelaCheckBoxList onclick="handleButton(this)" runat="server" ID="cblModuleList"></ACA:AccelaCheckBoxList>
    </div>
    <table role='presentation' class="ACA_AlignLeftOrRight">
    <tr valign="bottom">
        <td>
            <div class="ACA_LgButton ACA_LgButton_FontSize">
                <ACA:AccelaButton ID="btnSaveModule" OnClick="SaveModuleButton_OnClick" CausesValidation="false" LabelKey="aca_delegate_save" runat="server"></ACA:AccelaButton>
            </div>
            <div class="ACA_XShot" runat="server" id="divBlank" visible="false"></div>
        </td>
        <td class="PopupButtonSpace">&nbsp;</td>
        <td>
        <div id="divDelegateManagementCancelPopupButton" class="ACA_LinkButton">
            <ACA:AccelaLinkButton ID="btnCancelPopup" CssClass="NotShowLoading" LabelKey="aca_delegate_cancel" CausesValidation="false" OnClick="CancelModuleSaveButton_OnClick" runat="server"></ACA:AccelaLinkButton>
        </div> 
        </td>
      </tr>
    </table>
</div>
</div> 
<!-- message box end -->
</ContentTemplate></asp:UpdatePanel>

<div id="ViewDelegate" runat="server" visible="false">
<div id="divSplit0" class="ProxyUser_Normal_Divide"></div>
<div class="ACA_Page font11px" >
    <div id="divUserLabel" visible="false" runat="server">
        <div class="ACA_Popup_Title">
            <ACA:AccelaLabel ID="lblDelegateUserName" runat="server" IsNeedEncode="False"></ACA:AccelaLabel>
            (<ACA:AccelaLabel ID="lblDelegateEmailAddress" runat="server"></ACA:AccelaLabel>)
        </div>
        <div class="ACA_TabRow_Italic">
            <div>
                <ACA:AccelaLabel ID="lblAddedTime" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </div>
            <div>
                <ACA:AccelaLabel ID="lblLastAccessedTime" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </div>
        </div>
        <div class="ProxyUser_Permissions_Divide"></div>
        <div class="ACA_Popup_Title">
            <ACA:AccelaLabel ID="lblPermissionsTitle" runat="server" LabelKey="aca_delegate_permission_description"></ACA:AccelaLabel>
        </div> 
        <div id="divSplit3" class="ProxyUser_Normal_Divide"></div>
    </div>
    
    <ACA:AccelaLabel ID="lblViewStruction" runat="server" IsNeedEncode="false" LabelKey="aca_delegate_view_struction"></ACA:AccelaLabel>
    <div class="ACA_PrmissionList">
        <ul class="ACA_ProxyUserUL">
            <li runat="server" id="liViewRecord">
                <ACA:AccelaLabel ID="lblViewRecord_ViewModel" runat="server" LabelKey="aca_view_record_prefix"></ACA:AccelaLabel>
                <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkViewRecord" CssClass="NotShowLoading" OnClick="ViewRecordPermissionButton_OnClick" runat="server"></ACA:AccelaLinkButton>
                </span>
                <ACA:AccelaLabel ID="lblViewRecord_ModelSuffixal" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </li>
            <li runat="server" id="liCreateRecord">
                <ACA:AccelaLabel ID="lblCreateRecord_ViewModel" runat="server" LabelKey="aca_create_record_prefix"></ACA:AccelaLabel>
                <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkCreateRecord" CssClass="NotShowLoading" OnClick="CreateApplicationButton_OnClick" runat="server"></ACA:AccelaLinkButton>
                </span>
                <ACA:AccelaLabel ID="lblCreateRecord_ModelSuffixal" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </li>
            <li runat="server" id="liRenewRecord">
               <ACA:AccelaLabel ID="lblRenewRecord_ViewModel" runat="server" LabelKey="aca_renew_record_prefix"></ACA:AccelaLabel>
               <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkRenewRecord" CssClass="NotShowLoading" OnClick="RenewButton_OnClick" runat="server"></ACA:AccelaLinkButton>
               </span>
               <ACA:AccelaLabel ID="lblRenewRecord_ModelSuffixal" IsNeedEncode="false" Visible="false" runat="server"></ACA:AccelaLabel>
            </li>
            <li runat="server" id="liAmendment">
               <ACA:AccelaLabel ID="lblAmentMent_viewModel" runat="server" LabelKey="aca_amend_records_prefix"></ACA:AccelaLabel>
               <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkAmendment" CssClass="NotShowLoading" OnClick="AmendmentLink_OnClick" runat="server"></ACA:AccelaLinkButton>
                </span>
               <ACA:AccelaLabel ID="lblAmendment_ModelSuffixal" IsNeedEncode="false" Visible="false" runat="server"></ACA:AccelaLabel>
            </li>
            <li runat="server" id="liManageInspections">
               <ACA:AccelaLabel ID="lblManageInspections_ViewModel" LabelKey="aca_manage_inspections_prefix" runat="server"></ACA:AccelaLabel>
               <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkManageInspections" CssClass="NotShowLoading" OnClick="ManageInspectionsButton_OnClick" runat="server"></ACA:AccelaLinkButton>
                </span>
               <ACA:AccelaLabel ID="lblManageInspections_ModelSuffixal" IsNeedEncode="false" Visible="false" runat="server"></ACA:AccelaLabel>
            </li>
            <li runat="server" id="liManageDocuments">
               <ACA:AccelaLabel ID="lblManageDocuments_ViewModel" runat="server" LabelKey="aca_manage_documents_prefix"></ACA:AccelaLabel>
               <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkManageDocuments" CssClass="NotShowLoading" OnClick="ManageDocumentsButton_OnClick" runat="server"></ACA:AccelaLinkButton>
               </span>     
               <ACA:AccelaLabel ID="lblManageDocuments_ModelSuffixal" IsNeedEncode="false" Visible="false" runat="server"></ACA:AccelaLabel>
            </li>
            <li runat="server" id="liMakePayments">
               <ACA:AccelaLabel ID="lblMakePayments_ViewModel" runat="server" LabelKey="aca_make_paments_prefix"></ACA:AccelaLabel>
               <span class="ACA_RefEducation_Font ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="lnkMakePayments" CssClass="NotShowLoading" OnClick="MakePaymentsLink_OnClick" runat="server"></ACA:AccelaLinkButton>
                </span>
               <ACA:AccelaLabel ID="lblMakePayments_ModelSuffixal" IsNeedEncode="false" Visible="false" runat="server"></ACA:AccelaLabel>
            </li>
        </ul>
    </div>
    <div class="ProxyUser_Normal_Divide"></div>
    <div class="ACA_LinkButton">
        <ACA:AccelaLinkButton ID="lnkEditPermissions" LabelKey="aca_delegate_edit_these_permission" OnClick="EditPermissionsLink_OnClick" runat="server"></ACA:AccelaLinkButton>
    </div>
    <div class="ProxyUser_Normal_Divide"></div>
        <table role='presentation' class="ACA_AlignLeftOrRight ACA_ProxyUser_SaveButton">
    <tr valign="bottom">
        <td>
            <div class="ACA_LgButton ACA_LgButton_FontSize"><ACA:AccelaLinkButton ID="btnBackToAccount" LabelKey="aca_delegate_backtoaccount" OnClientClick="CloseDelegeteWindow();return false;" runat="server"></ACA:AccelaLinkButton></div>
         </td>
         <td class="PopupButtonSpace">&nbsp;</td>
         <td><div class="ACA_LinkButton"><ACA:AccelaLinkButton ID="btnRemoveAccount" LabelKey="aca_remove_delegate" OnClientClick="return ConfirmRemoveProxyUser();" OnClick="RemoveAccountButton_OnClick" runat="server"></ACA:AccelaLinkButton></div>
    </td>
    </tr>
    </table>
</div> 
</div>

<div id="divEditDelegate" runat="server">
    <ACA:DelegateEdit ID="delegateEdit" runat="server" />
</div>
</div>
