<%@ Page Language="C#"  MasterPageFile="~/Default.master" AutoEventWireup="true" 
Title="Delegate Settings" CodeBehind="ProxyPopupSettings.aspx.cs" Inherits="Accela.ACA.Web.Account.ProxyPopupSettings" %>
<%@ Register TagPrefix="ACA" TagName="Recaptcha" Src="~/Component/Recaptcha.ascx" %>  

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
<div class="ACA_Content">
<div class="ACA_RegisterConfirm_Width">
<div id="ImgParentDiv" class="ACA_AlignRightOrLeft CloseImage ACA_FRight Proxy_PopupColseButton">
    <ACA:AccelaLinkButton id="lnkCancel" runat="server"><img id="imgCreateClose" class="ACA_ActionIMG" runat="server"  alt="close" /></ACA:AccelaLinkButton>
</div>
<div id="divCreateDelegte" class="ACA_ProxyUser_Form" runat="server">
<ACA:AccelaLabel ID="lblAddProxyPageTitle" LabelType="PopUpTitle" LabelKey="aca_add_delegate_section_title" runat="server" />
<div class="ACA_Page font11px">
    <div class="ACA_FLeft ACA_BlueLable">
        <ACA:AccelaTextBox ID="tbNickName" MaxLength="100" CssClass="ACA_MLong" runat="server" LabelKey="aca_delegate_nick_name"></ACA:AccelaTextBox>
    </div> 
    <div class="ACA_FLeft ACA_BlueLable">
        <ACA:AccelaEmailText runat="server" ID="tbEmailAddress"  CssClass="ACA_MLong" MaxLength="50" CustomValidationFunction="checkProxyEmailAddress_onblur"
             LabelKey="aca_delegate_email_address"/> 
     </div>
     <div class="ACA_TabRow ACA_Popup_Title">
        <ACA:AccelaLabel ID="lblSelectPermission" runat="server" LabelKey="aca_delegate_select_permission"></ACA:AccelaLabel>
     </div>
     <div class="ACA_TabRow_Italic">     
        <ACA:AccelaLabel ID="lblViewRecordTitle" runat="server" LabelKey="aca_delegate_viewrecord_title"></ACA:AccelaLabel>
     </div>
    <div class="ACA_FLeft ACA_Label font12px Proxy_ViewRecord_Margin"><ACA:AccelaLabel ID="lblViewRecordRoleTitle" runat="server" LabelKey="aca_view_record_description"></ACA:AccelaLabel></div>
    <div class="ACA_FLeft ACA_LinkButton">(<ACA:AccelaLinkButton ID="btnViewRecordPermission"  runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)</div>
    <div id="divHeight" style="height:35px"></div>
    <div class="ACA_TabRow_Italic">
        <ACA:AccelaLabel ID="lblModuleLevelDescription" runat="server" LabelKey="aca_delegate_module_level_description"></ACA:AccelaLabel>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkCreateApplication" runat="server" AutoPostBack="false" LabelKey="aca_create_application_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnCreateApplication" runat="server"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkRenewRecord" runat="server" AutoPostBack="false" LabelKey="aca_renew_record_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnRenew" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">    
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkAmendment" runat="server" AutoPostBack="false" LabelKey="aca_amend_record"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnAmendment" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkManageInspections" runat="server" AutoPostBack="false" LabelKey="aca_manage_inspections_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnManageInspections" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkManageDocuments" runat="server" AutoPostBack="false" LabelKey="aca_manage_document_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnManageDocuments" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">    
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="chkMakePayments" runat="server" AutoPostBack="false" LabelKey="aca_make_paments_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="btnMakePayments" runat="server"  LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div id="divSplit1" class="ProxyUser_Permissions_Divide"></div>
    <div class="ACA_TabRow" runat="server" id="divPersonNote">
        <div class="ACA_LinkButton font12px">
            <div>
                <ACA:AccelaLinkButton ID="lnkAddNote" runat="server" LabelKey="aca_delegate_add_note"></ACA:AccelaLinkButton>
            </div>
            <div>
                <ACA:AccelaLinkButton ID="lnkRemoveNote" runat="server" LabelKey="aca_delegate_remove_note"></ACA:AccelaLinkButton>
            </div>
        </div>
        <div id="divPersonNoteInput" runat="server">
            <ACA:AccelaTextBox ID="tbPersonNote" LabelKey="aca_proxy_personnote" TextMode="MultiLine" runat="server" Rows="6" MaxLength="2000"></ACA:AccelaTextBox>
        </div>
        <asp:HiddenField ID="hdnExpand" runat="server" Value="0" />
    </div>
    <div id="divSplit2" class="ProxyUser_Normal_Divide"></div>
    <div class="ACA_TabRow">
        <ACA:Recaptcha ID="reCaptcha" runat="server" />
    </div>
	<br />
    <div class="ACA_TabRow">
    <table role='presentation'>
    <tr valign="bottom">
        <td>
        <div class="ACA_LgButton ACA_LgButton_FontSize">
            <ACA:AccelaButton ID="btnInviteDelegate" LabelKey="aca_invite_delegate" runat="server"></ACA:AccelaButton>
        </div>
        </td>
        <td class="PopupButtonSpace">&nbsp;</td>
        <td>
        <div class="ACA_LinkButton">
            <ACA:AccelaLinkButton ID="btnCancel" LabelKey="aca_delegate_cancel" runat="server"></ACA:AccelaLinkButton>
        </div>
        </td>
    </tr>
    </table>
    </div>
    </div>
</div>

<br />
<div id="divViewDelegate" class="ACA_ProxyUser_Form" runat="server">
<div id="divViewCloseSection" class="ACA_AlignRightOrLeft CloseImage ACA_FRight">
    <ACA:AccelaLinkButton id="lnkViewClose" runat="server"><img id="imgViewClose" class="ACA_ActionIMG" runat="server"  alt="close" /></ACA:AccelaLinkButton>
</div>
<ACA:AccelaLabel ID="lblManageDelegate" LabelType="PopUpTitle" LabelKey="aca_delegate_manage_section_title" runat="server"/>
<div id="divSplit0" class="ProxyUser_Normal_Divide"></div>
<div class="ACA_Page font11px" >
        <div class="ACA_TabRow_Italic">
            <div>
                <ACA:AccelaLabel ID="lblAddedTime" LabelKey="aca_delegate_add_on" runat="server"></ACA:AccelaLabel>
            </div>
            <div>
                <ACA:AccelaLabel ID="lblLastAccessedTime" LabelKey="aca_delegate_last_accessed_on" runat="server"></ACA:AccelaLabel>
            </div>
            <div>
                <ACA:AccelaLabel ID="lblLastAccessedDelegate" LabelKey="aca_last_accessed_on_delegate" runat="server"></ACA:AccelaLabel>
            </div>            
        </div>
        <div class="ProxyUser_Permissions_Divide"></div>
        <div class="ACA_Popup_Title">
            <ACA:AccelaLabel ID="lblPermissionsTitle" runat="server" LabelKey="aca_delegate_permission_description"></ACA:AccelaLabel>
        </div> 
        <div id="divSplit3" class="ProxyUser_Normal_Divide"></div>
    
    <ACA:AccelaLabel ID="lblViewStruction" runat="server" LabelKey="aca_delegate_view_struction"></ACA:AccelaLabel>
    <div>
        <ACA:AccelaLabel ID="lblViewStructionDelegate" runat="server" LabelKey="aca_delegate_view_struction_delegate"></ACA:AccelaLabel>
    </div>
    <div class="ACA_PrmissionList">
        <ul class="ACA_ProxyUserUL">
            <li runat="server" id="liViewRecord">
                <ACA:AccelaLabel ID="lblViewRecord_ViewModel" runat="server" LabelKey="aca_view_record_prefix"></ACA:AccelaLabel>
                <%--<ACA:AccelaLabel ID="lblViewRecord_ModelSuffixal" LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
            <li runat="server" id="liCreateRecord">
                <ACA:AccelaLabel ID="lblCreateRecord_ViewModel" runat="server" LabelKey="aca_create_record_prefix"></ACA:AccelaLabel>
                <%--<ACA:AccelaLabel ID="lblCreateRecord_ModelSuffixal" LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
            <li runat="server" id="liRenewRecord">
               <ACA:AccelaLabel ID="lblRenewRecord_ViewModel" runat="server" LabelKey="aca_renew_record_prefix"></ACA:AccelaLabel>
               <%--<ACA:AccelaLabel ID="lblRenewRecord_ModelSuffixal" LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
            <li runat="server" id="liAmendment">
               <ACA:AccelaLabel ID="lblAmentMent_viewModel" runat="server" LabelKey="aca_amend_records_prefix"></ACA:AccelaLabel>
               <%--<ACA:AccelaLabel ID="lblAmendment_ModelSuffixal" LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
            <li runat="server" id="liManageInspections">
               <ACA:AccelaLabel ID="lblManageInspections_ViewModel" LabelKey="aca_manage_inspections_prefix" runat="server"></ACA:AccelaLabel>
               <%--<ACA:AccelaLabel ID="lblManageInspections_ModelSuffixal"  LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
            <li runat="server" id="liManageDocuments">
               <ACA:AccelaLabel ID="lblManageDocuments_ViewModel" runat="server" LabelKey="aca_manage_documents_prefix"></ACA:AccelaLabel>  
               <%--<ACA:AccelaLabel ID="lblManageDocuments_ModelSuffixal"  LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
            <li runat="server" id="liMakePayments">
               <ACA:AccelaLabel ID="lblMakePayments_ViewModel" runat="server" LabelKey="aca_make_paments_prefix"></ACA:AccelaLabel>
               <%--<ACA:AccelaLabel ID="lblMakePayments_ModelSuffixal" LabelKey="aca_delegate_all_categories" runat="server"></ACA:AccelaLabel>--%>
            </li>
        </ul>
    </div>
    <div class="ProxyUser_Normal_Divide"></div>
    <div class="ACA_LinkButton">
        <ACA:AccelaLinkButton ID="lnkEditPermissions" LabelKey="aca_delegate_edit_these_permission" runat="server"></ACA:AccelaLinkButton>
    </div>
    <div class="ProxyUser_Normal_Divide"></div>
        <table role='presentation' class="ACA_ProxyUser_SaveButton">
    <tr valign="bottom">
        <td>
            <div class="ACA_LgButton ACA_LgButton_FontSize"><ACA:AccelaLinkButton ID="btnBackToAccount" LabelKey="aca_delegate_backtoaccount" OnClientClick="CloseDelegeteWindow();" runat="server"></ACA:AccelaLinkButton></div>
         </td>
         <td class="PopupButtonSpace">&nbsp;</td>
         <td>
            <div class="ACA_LinkButton">
                <ACA:AccelaLinkButton ID="btnRemoveAccount" LabelKey="aca_remove_delegate" runat="server"></ACA:AccelaLinkButton>
                <ACA:AccelaLinkButton ID="btnRemoveInviter" LabelKey="aca_remove_inviter" runat="server"></ACA:AccelaLinkButton>
            </div>
         </td>
    </tr>
    </table>
</div>
</div>
<br />

<div id="divEditDelegate" class="ACA_ProxyUser_Form">
<div id="divEditCloseSection" class="ACA_AlignRightOrLeft CloseImage ACA_FRight">
    <ACA:AccelaLinkButton id="lnkEditCloseButton" runat="server"><img id="imgEditClose" class="ACA_ActionIMG" runat="server"  alt="close" /></ACA:AccelaLinkButton>
</div>
    <ACA:AccelaLabel ID="lblManageProxyUser" LabelType="PopUpTitle" LabelKey="aca_delegate_manage_section_title" runat="server"/>
    <div class="ACA_Page font11px">
    <div id="div1" runat="server">
        <div class="ACA_Label ACA_TabRow_Italic">
            <div>
                <ACA:AccelaLabel ID="AccelaLabel2" LabelKey="aca_invitation_sent_on" runat="server"></ACA:AccelaLabel>
            </div>
            <div>
                <ACA:AccelaLabel ID="AccelaLabel3" LabelKey="aca_delegate_last_accessed_on" runat="server"></ACA:AccelaLabel>
            </div>
        </div>
    </div> 
    <div id="div2" class="ProxyUser_Permissions_Divide"></div>
    <div class="ACA_TabRow ACA_Popup_Title"> 
        <ACA:AccelaLabel ID="AccelaLabel4" runat="server" LabelKey="aca_select_permission_description"></ACA:AccelaLabel>
    </div>
    <div class="ACA_FLeft ACA_Label font12px Proxy_ViewRecord_Margin">
        <ACA:AccelaLabel ID="AccelaLabel5" runat="server" LabelKey="aca_view_record_description"></ACA:AccelaLabel></div>
        <div class="ACA_FLeft ACA_LinkButton">(<ACA:AccelaLinkButton ID="AccelaLinkButton1" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
    </div>
    <div id="div3" class="ProxyUser_ViewPermission_Divide"></div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="AccelaCheckBox1" runat="server" AutoPostBack="false" LabelKey="aca_create_application_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="AccelaLinkButton2" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="AccelaCheckBox2" runat="server" AutoPostBack="false" LabelKey="aca_renew_record_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="AccelaLinkButton3" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="AccelaCheckBox6" runat="server" AutoPostBack="false" LabelKey="aca_amend_record"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">    
            (<ACA:AccelaLinkButton ID="AccelaLinkButton7" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>  
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="AccelaCheckBox3" runat="server" AutoPostBack="false" LabelKey="aca_manage_inspections_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">
            (<ACA:AccelaLinkButton ID="AccelaLinkButton4" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="AccelaCheckBox4" runat="server" AutoPostBack="false" LabelKey="aca_manage_document_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">    
            (<ACA:AccelaLinkButton ID="AccelaLinkButton5" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>
    <div class="ACA_TabRow">
        <div class="ACA_FLeft">
            <ACA:AccelaCheckBox ID="AccelaCheckBox5" runat="server" AutoPostBack="false" LabelKey="aca_make_paments_description"/>
        </div>
        <div class="ACA_FLeft ACA_LinkButton ProxyUser_CheckBoxList_Margin">    
            (<ACA:AccelaLinkButton ID="AccelaLinkButton6" runat="server" LabelKey="aca_delegate_change"></ACA:AccelaLinkButton>)
        </div>
    </div>   
    <div id="div4" class="ProxyUser_ViewPermission_Divide"></div>
    <table role='presentation' class="ACA_ProxyUser_SaveButton">
    <tr valign="bottom">
        <td>
            <div class="ACA_LgButton ACA_LgButton_FontSize">
                <ACA:AccelaButton ID="btnSaveChanges" LabelKey="aca_delegate_save_changes" runat="server"></ACA:AccelaButton>
            </div>
        </td>
        <td class="PopupButtonSpace">&nbsp;</td>
        <td>
            <div class="ACA_LinkButton">
                <ACA:AccelaLinkButton ID="AccelaLinkButton8" LabelKey="aca_delegate_cancel" runat="server"></ACA:AccelaLinkButton>
            </div>
        </td>
    </tr>
    </table>
    </div>
</div>
</div>
</div>

</asp:Content>
