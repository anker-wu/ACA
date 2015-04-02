<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Component.DelegateEdit" Codebehind="DelegateEdit.ascx.cs" %>

<script type="text/javascript">
    function handleButtonDelegateEdit(chkObj) {
        var inputs = chkObj.getElementsByTagName("input");
        var isAllUnchecked = true;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                isAllUnchecked = false;
                break;
            }
        }
 
        if (isAllUnchecked) {
            $get('<%=btnSaveModule.ClientID %>').disabled = true;
            $get('<%=btnSaveModule.ClientID %>').parentNode.className = "ACA_LgButtonDisable ACA_LgButtonDisable_FontSize";
        }
        else {
            $get('<%=btnSaveModule.ClientID %>').disabled = false;
            $get('<%=btnSaveModule.ClientID %>').parentNode.className = "ACA_LgButton ACA_LgButton_FontSize";
        }
    }

    function focusBegginning() {
        var focusObj = $get('<%=hdnEditDeletate.ClientID %>');
        if (focusObj != null) {
            focusObj.focus();
        }
    }
</script>

<div>
    <ACA:AccelaHideLink ID="hdnEditDeletate" Width="0" runat="server" AltKey="img_alt_form_begin"/>
    <div class="ACA_Page font11px">
    <div id="divUserLabel" runat="server">
        <div class="ACA_Popup_Title">
            <ACA:AccelaLabel ID="lblDelegateUserName" runat="server" IsNeedEncode="False"></ACA:AccelaLabel>
            (<ACA:AccelaLabel ID="lblProxyUserEmailAddress" runat="server"></ACA:AccelaLabel>)
        </div>
        <div class="ACA_Label ACA_TabRow_Italic">
            <div>
                <ACA:AccelaLabel ID="lblAddedTime" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </div>
            <div>
                <ACA:AccelaLabel ID="lblLastAccessedTime" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </div>
        </div>
    </div> 
    <div id="divSplit1" class="ProxyUser_Permissions_Divide"></div>
    <div class="ACA_TabRow ACA_Popup_Title"> 
        <ACA:AccelaLabel ID="lblSelectPermission" runat="server" LabelKey="aca_select_permission_description"></ACA:AccelaLabel>
    </div>
    <div class="ACA_FLeft ACA_Label font12px ACA_DivMargin6">
        <ACA:AccelaLabel ID="lblViewRecordRoleTitle" runat="server" LabelKey="aca_view_record_description"></ACA:AccelaLabel></div>
        <div class="ACA_FLeft ACA_LinkButton Proxy_ChangeButton_Margin">(<ACA:AccelaLinkButton ID="btnViewRecordPermission" CssClass="NotShowLoading" runat="server" OnClick="ViewRecordPermissionButton_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
    </div>
    <div id="divHeight" class="ProxyUser_ViewPermission_Divide"></div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkCreateApplication" runat="server" AutoPostBack="false" LabelKey="aca_create_application_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnCreateApplication" CssClass="NotShowLoading" runat="server" OnClick="CreateApplicationButton_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkRenewRecord" runat="server" AutoPostBack="false" LabelKey="aca_renew_record_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnRenew" CssClass="NotShowLoading" runat="server" OnClick="RenewButton_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkAmendment" runat="server" AutoPostBack="false" LabelKey="aca_amend_record"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">    
            (<ACA:AccelaLinkButton ID="btnAmendment" CssClass="NotShowLoading" runat="server" OnClick="AmendmentLink_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>   
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkManageInspections" runat="server" AutoPostBack="false" LabelKey="aca_manage_inspections_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnManageInspections" CssClass="NotShowLoading" runat="server" OnClick="ManageInspectionsButton_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkManageDocuments" runat="server" AutoPostBack="false" LabelKey="aca_manage_document_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">    
            (<ACA:AccelaLinkButton ID="btnManageDocuments" CssClass="NotShowLoading" runat="server" OnClick="ManageDocumentsButton_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkMakePayments" runat="server" AutoPostBack="false" LabelKey="aca_make_paments_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">    
            (<ACA:AccelaLinkButton ID="btnMakePayments" CssClass="NotShowLoading" runat="server" OnClick="MakePaymentsLink_OnClick" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div id="divSplit2" class="ProxyUser_ViewPermission_Divide"></div>
    <table role='presentation' class="ACA_ProxyUser_SaveButton">
    <tr valign="bottom">
        <td>
            <div class="ACA_LgButton ACA_LgButton_FontSize">
                <ACA:AccelaButton ID="btnSaveChanges" OnClick="SaveChangesButton_OnClick" LabelKey="aca_delegate_save_changes" runat="server"></ACA:AccelaButton>
            </div>
        </td>
        <td class="PopupButtonSpace">&nbsp;</td>
        <td>
            <div id="divDelegateManagementEditCancelButton" class="ACA_LinkButton">
                <ACA:AccelaLinkButton ID="btnCancel" LabelKey="aca_delegate_cancel" OnClientClick="CloseDelegeteWindow();return false;" runat="server"></ACA:AccelaLinkButton>
            </div>
        </td>
    </tr>
    </table>

    <!-- message box begin -->
    <div id="divDelegateEditPopupBox" class="ACA_Hide PopUpDlg ProxyUser_Popup_Width">
        <ACA:AccelaHideLink ID="hlBeginFocus" AltKey="img_alt_form_begin" Width="0" runat="server" NextControlID="btnOK" />
         <div class="ACA_TabRow ACA_Popup_Title">
            <div class="ACA_AlignRightOrLeft CloseImage ACA_FRight">
                <ACA:AccelaLinkButton id="lnkCancelModuleSave" CssClass="NotShowLoading" runat="server" CausesValidation="false" OnClientClick="delegateEditPopUpDialog.cancel();return false;"><img class="ACA_ActionIMG" src='<%=ImageUtil.GetImageURL("closepopup.png") %>' alt="close" /></ACA:AccelaLinkButton>
            </div>
            <ACA:AccelaLabel ID="lblCategories_Edit" runat="server" LabelKey="aca_delegate_category_description"></ACA:AccelaLabel>
         </div>
         <div class="ACA_ProxyUser_ScrollX">
           <ACA:AccelaCheckBoxList  runat="server" ID="cblModuleList"></ACA:AccelaCheckBoxList>
         </div>
        <table role='presentation' class="ACA_AlignLeftOrRight">
        <tr valign="bottom">
            <td>
                <div class="ACA_LgButton ACA_LgButton_FontSize">
                    <ACA:AccelaLinkButton ID="btnSaveModule" OnClick="SaveModuleButton_OnClick" LabelKey="aca_delegate_save" runat="server"></ACA:AccelaLinkButton>
                </div>
            </td>
            <td class="PopupButtonSpace">&nbsp;</td>
            <td>
                <div id="divDelegateManagementEditCancelPopupButton" class="ACA_LinkButton">
                    <ACA:AccelaLinkButton ID="btnCancelPopup" LabelKey="aca_delegate_cancel" OnClientClick="delegateEditPopUpDialog.cancel();return false;" runat="server"></ACA:AccelaLinkButton>
                </div> 
            </td>
          </tr>
        </table>
    </div>    
    <!-- message box end -->

    <asp:HiddenField ID="hdnEditProxyUserRoleType" runat="server" /> 
    </div>
</div>