<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ContactEdit" ClassName="ContactEdit" CodeBehind="ContactEdit.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/ContactAddressList.ascx" TagName="ContactAddressList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ContactView.ascx" TagName="ContactView" TagPrefix="ACA" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>
<ACA:AccelaHideLink ID="hlBegin" runat="server" AltKey="img_alt_form_begin" CssClass="ACA_Hide" />
<!--Donot change this update panel id, it is using the hard code in contact address edit-->
<asp:UpdatePanel ID="ContactEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaLabel ID="errorMessageLabel" runat="server"></ACA:AccelaLabel>
        <ACA:AccelaTextBox ID="txtValidateSectionRequired" IsHidden="True" Validate="required" runat="server" />
        <div attr="contact" class="contact_edit">
            <div id="divSelectContactSection" runat="server" class="ACA_Row ACA_LiLeft action_buttons">
                <ul>
                    <li>
                        <ACA:AccelaButton ID="btnAddFromSaved" runat="server" LabelKey="aca_contactedit_label_selectfromaccount" CausesValidation="False"
                            OnClick="AddFromSavedButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                        </ACA:AccelaButton>
                    </li>
                    <li>
                        <ACA:AccelaButton ID="btnAddNew" runat="server" LabelKey="aca_contactedit_label_addnew" CausesValidation="False"
                            OnClick="AddNewButton_Click" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                        </ACA:AccelaButton>
                    </li>
                    <li>
                        <ACA:AccelaButton ID="btnLookUp" runat="server" LabelKey="aca_contactedit_label_lookup" CausesValidation="False"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize">
                        </ACA:AccelaButton>
                    </li>
                </ul>
            </div>
            <ACA:AccelaHeightSeparate ID="sepForSelectContact" runat="server" Height="5" />
            <div id="divActionNotice" runat="server" visible="false">
                <div class="ACA_Error_Icon" runat="server" enableviewstate="False" id="divImgSuccess" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("img_alt_workflow_complete") %>" src="<%=ImageUtil.GetImageURL("complete.png") %>"/>           
                </div>
                <div class="ACA_Error_Icon" runat="server" enableviewstate="False" id="divImgFailed" visible="false">
                    <img class="ACA_NoBorder" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>"/>            
                </div>
                <div class="ACA_Notice font12px">
                    <ACA:AccelaLabel ID="lblActionNoticeAddSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_addedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_removedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditSuccess" class="Notice_Message_Success" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_editedsuccessfully" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeAddFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_addedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeDeleteFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False"  LabelKey="aca_contact_label_removedfailed" Visible="false"/>
                    <ACA:AccelaLabel ID="lblActionNoticeEditFailed" class="Notice_Message_Failure" runat="server" EnableViewState="False" LabelKey="aca_contact_label_editedfailed" Visible="false"/>
                </div>

                <div class="clear"></div>
            </div>
            <div class="ACA_Hide" id="divContactList" runat="server">
                <ACA:ContactView ID="ucContactView" runat="server" Visible="False">
                </ACA:ContactView>
                <div class="ACA_Row" id="divContactViewEdit" runat="server">
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="btnEdit" runat="server" LabelKey="aca_contactedit_label_edit" CausesValidation="false" />
                        <ACA:AccelaLinkButton ID="btnRemove" runat="server" LabelKey="aca_contactedit_label_remove" class="ACA_Unit"
                            OnClick="RemoveButton_Click" OnClientClick="return RemoveContact();" CausesValidation="false" />
                    </div>
                </div>
                <ACA:AccelaHeightSeparate ID="sepLineForContactView" runat="server" Height="5"/>
                <ACA:ContactAddressList ID="ucContactAddressList" GViewID="60134" runat="server"
                    OnContactAddressSelected="ContactAddressList_ContactAddressSelected"
                    OnContactAddressDeactivate="ContactAddressList_ContactAddressDeactivate"
                    OnDataSourceChanged="ContactAddressList_DataSourceChanged">
                </ACA:ContactAddressList>
            </div>
            <div id="divConditon" runat="server" class="conditions" Visible="False">
                <uc1:Conditions ID="ucConditon" runat="server" Visible="False" />
            </div>
        </div>
        <asp:LinkButton ID="btnRefreshContact" runat="Server" CssClass="ACA_Hide"
            OnClick="RefreshContactButton_Click" TabIndex="-1"></asp:LinkButton>
        <asp:HiddenField ID="hdnDisableStatus" runat="server" />
        <asp:HiddenField ID="hdnContactSeqNumber" runat="server" />
        <asp:HiddenField ID="hdnRefContactSeqNumber" runat="server" />
        <asp:HiddenField ID="hfIsEditContactAddress" runat="server" />
        <asp:HiddenField ID="hfLockStandardFileds" runat="server"/>
        <asp:HiddenField ID="hfContatType" runat="server"/>
        <asp:HiddenField ID="hdnIsClearRefContact" runat="server"/>
    </ContentTemplate>
</asp:UpdatePanel>
<ACA:AccelaHideLink ID="hlEnd" runat="server" AltKey="aca_common_msg_formend" CssClass="ACA_Hide" />
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var contactSectionPosition = '<%=ContactSectionPosition %>';

    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=btnEdit.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=btnRemove.ClientID %>");
    }

    prm.add_endRequest(<%=ClientID %>_EndRequest);
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);

    function <%=ClientID %>_EndRequest(sender, args) {

    }

    function <%=ClientID %>_PageLoaded(sender, args)
    {
        /*
        Add the Contact Type as the CSS class to the top container.
        Allow agency to customize different look & feel of the same standard field/template field in case of different contact type.
        */
        var topContainer = $get('<%=ContactEditPanel.ClientID %>');
        var contactType = $get('<%=hfContatType.ClientID %>');

        if(topContainer && contactType && contactType.selectedIndex >= 0)
        {
            topContainer.className = GetValidCssClassName(contactType[contactType.selectedIndex].value);
        }
    }

    function <%=ClientID %>_ButtonClientClick()
    {
        if(typeof(SetNotAsk) != 'undefined')
        {
            SetNotAsk();
        }

        var contactAddressFormIDs = typeof(<%=ucContactAddressList.ClientID %>_ContactAddressFormControlIDs) != 'undefined' ? <%=ucContactAddressList.ClientID %>_ContactAddressFormControlIDs : null;
        SetCurrentValidationSectionID('<%=ClientID %>', contactAddressFormIDs); 
    }
    
    function <%=CreateContactSessionFunction%>(processesType, contactIndex, contactAddressIndex, func) {
        var processShowloading = new ProcessLoading();
        processShowloading.showLoading();
        PageMethods.CreateContactParametersSession('<%=ModuleName %>', contactIndex, contactAddressIndex, processesType, '<%=SessionParameterString %>', func);
    }
    
    function <%=AddNewClientFunction %>() {
        var needSelectType = <%=(ComponentID == (int)PageFlowComponent.CONTACT_LIST || NeedSelectType).ToString().ToLower()%>;
        
        if (needSelectType) {			
			<%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People") %>/ContactTypeSelect.aspx', '<%=btnAddNew.ClientID%>', 400, 200);
        } else {
			<%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People") %>/ContactAddNew.aspx', '<%=btnAddNew.ClientID%>');
        }        
    }
    
    function <%=ClientID %>_AddFromOtherAgencies() {
        var isRegisteringExistingAccount = '<%=IsNewUserRegisterExistingAccount %>' == 'True';
        
        if (!isRegisteringExistingAccount) {
            <%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactLookUp.aspx") %>', '<%=btnAddFromSaved.ClientID%>', 400, 200);
        } 
        else {
            validatePassword();
        }
    }
    
    function <%=ClientID %>_AddFromSavedContact() {
        if (<%=(ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount).ToString().ToLower()%>) {			
			return <%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactLookUp.aspx") %>', '<%=btnAddFromSaved.ClientID%>', 400, 200);
        } else {
			return <%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactLookUp.aspx") %>', '<%=btnAddFromSaved.ClientID%>');
        }
    }

    function <%=ClientID %>_LookUpContact() {
        return <%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactLookUp.aspx") %>', '<%=btnLookUp.ClientID%>');
    }
    
    function <%=EditContactFunction %>(typeEmptyOrDisabled) {
        $("#<%=divActionNotice.ClientID %>").hide();
        if (typeof (<%=ucContactAddressList.ClientID %>_ClearMessage) != "undefined") {
            <%=ucContactAddressList.ClientID %>_ClearMessage();
        }

        var focusId = $("#__LASTFOCUS_ID").val();

        if (<%=(ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm).ToString().ToLower()%>
            && <%=(ComponentID == (int)PageFlowComponent.CONTACT_LIST).ToString().ToLower()%>
            && typeEmptyOrDisabled) {
            return <%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactTypeSelect.aspx") %>', focusId, 400, 200);
        } else {
            return <%=ClientID %>_OpenContact('<%= Page.ResolveUrl("~/People/ContactAddNew.aspx") %>', focusId);
        }
    }
    
    function <%=ClientID %>_RemoveContact(msg) {
        $("#<%=divActionNotice.ClientID %>").hide();
        <%=ClientID %>_HideCondition();
        var defaultMsg = '<%=GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'") %>';
        
        if(msg != undefined && msg != '') {
            defaultMsg = msg;
        }
        
        if (confirm(defaultMsg)) {
            <%=ClientID %>_ButtonClientClick();
            return true;
        } else {
            return false;
        }
    }

    function <%=ClientID %>_OpenContact(url, targetId, width, height) {
        var actualWidth = width ? width : 800;
        var actualHeight = height ? height : 600;
        <%=ClientID %>_HideCondition();
        <%=ClientID %>_ButtonClientClick();

        url += '?<%= ACAConstant.MODULE_NAME + "=" + ModuleName %>';
        url += '&<%= UrlConstant.AgencyCode + "=" + ConfigManager.AgencyCode %>';
        url += '&<%= ACAConstant.IS_SUBAGENCY_CAP + "=" + IsSubAgencyCap %>';
        url += '&<%= UrlConstant.IS_FOR_CLERK + "=" + Request.QueryString[UrlConstant.IS_FOR_CLERK] %>';
        url += '&<%= UrlConstant.ValidateFlag + "=" + ValidateFlag %>';
        <%if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm)
          { %>
        url += '&<%= UrlConstant.CONTACT_SEQ_NUMBER + "=" + Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER] %>';
        <% }%>
        
        <% if (IsLoginUseExistingAccount || IsNewUserRegisterExistingAccount)
           {
        %>
        url += '&<%=UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT %>=Y&<%=UrlConstant.USER_ID_OR_EMAIL %>=<%= HttpUtility.UrlEncode(ExistingAccountRegisterationUserID) %>';
        <% } %>
        
        ACADialog.popup({ url: url, width: actualWidth, height:actualHeight, objectTarget: targetId });

        return false;
    }

    // Refresh the contact information after saving the single contact by 'Select from Account'/'Add New'/'Look Up'
    function <%=ClientID %>_Refresh(isFromAddress, refContactSeqNbr, isClearRefContact) {
        window.setTimeout(function() {
            delayShowLoading();
        }, 100);
        
        if (isFromAddress != undefined && isFromAddress) {
            $("#<%=hfIsEditContactAddress.ClientID %>").val("1");
        } else {
            $("#<%=hfIsEditContactAddress.ClientID %>").val("0");
        }
        
        if (refContactSeqNbr != undefined) {
            $("#<%=hdnRefContactSeqNumber.ClientID %>").val(refContactSeqNbr);
        }
        
        if (isClearRefContact != undefined) {
            $("#<%=hdnIsClearRefContact.ClientID %>").val(isClearRefContact);
        }

        if (!<%=IsMultipleContact.ToString().ToLower() %>) {
            SetLastFocus('<%=btnEdit.ClientID %>');
        }

        __doPostBack('<%=btnRefreshContact.UniqueID %>', '');
    }
    
    function <%=ClientID %>_HideCondition() {
        SetNotAskForSPEAR();

        $("#<%=ucConditon.ClientID %>").hide();
    }
    
    function <%= ClientID %>_CheckRequired4Contact() {
        if (!$.exists('#<%=ucContactView.ClientID %>')) {
            var imgUrl = '<%=ImageUtil.GetImageURL("error_16.gif")%>';
            var errorMsg ='<%= GetTextByKey("aca_common_msg_validaterequiredfailed").Replace("'", "\\'")%>';
            ShowSectionRequiredMsg('<%= errorMessageLabel.ClientID %>', errorMsg, imgUrl);
            return false;
        } 
        
        return true;
    }
    
    function <%=ClientID %>_ValidateFieldRequired4Contact() {
        var errorMsg ='<%= GetTextByKey("per_contactlist_required_validate_msg").Replace("'", "\\'")%>';
        showMessage('<%=errorMessageLabel.ClientID %>', errorMsg, "Error", true, 1, false);
        
        return false;
    }
</script>