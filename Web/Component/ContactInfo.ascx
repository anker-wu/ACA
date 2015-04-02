<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.ContactInfo" ClassName="ContactInfo" CodeBehind="ContactInfo.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<%@ Register Src="~/Component/ContactPermissionControl.ascx" TagName="ContactPermissionControl" TagPrefix="uc1" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>
<%@ Register Src="~/Component/ContactAddressList.ascx" TagName="ContactAddressList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc1" %>
<!--Donot change this update panel id, it is using the hard code in contact address edit-->
<asp:UpdatePanel ID="ContactEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <div id="divContactEdit" attr="contact">
            <div id="divConditon">
                <uc1:Conditions ID="ucConditon" runat="server" Visible="False" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel4ContactType" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <ACA:AccelaSectionTitleBar ID="sectionTitleBar" ShowType="OnlyAdmin" runat="server" Visible="false" />
                    <ACA:AccelaLabel ID="lblConfirmCloseMatchConfirmTitle" Visible="False" runat="server" LabelKey="aca_contactclosematchconfirm_label_confirmtitle" LabelType="PopUpTitle" CssClass="ACA_TabRow_Italic"/>
                    <ACA:AccelaDropDownList ID="ddlContactType" AutoPostBack="true" runat="server" LabelKey="per_appInfoEdit_label_contactType"
                            IsHidden="True" OnSelectedIndexChanged="ContactType_SelectedIndexChanged">
                        </ACA:AccelaDropDownList>
                    <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
                        <ACA:AccelaDropDownList ID="ddlAppSalutation" runat="server" LabelKey="per_appInfoEdit_label_salutation"
                            ShowType="showdescription">
                        </ACA:AccelaDropDownList>
                        <ACA:AccelaTextBox ID="txtTitle" MaxLength="255" runat="server" LabelKey="per_appinfoedit_label_title" />
                        <ACA:AccelaTextBox ID="txtAppFirstName" MaxLength="70" runat="server" LabelKey="per_appInfoEdit_label_firstName"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppMiddleName" runat="server" LabelKey="per_appInfoEdit_label_middleName"
                            MaxLength="70"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppLastName" MaxLength="70" runat="server" LabelKey="per_appInfoEdit_label_lastName"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppFullName" MaxLength="220" runat="server" LabelKey="aca_contactedit_label_fullname"></ACA:AccelaTextBox>
                        <ACA:AccelaCalendarText ID="txtAppBirthDate" runat="server" LabelKey="per_appInfoEdit_label_birthDate"></ACA:AccelaCalendarText>
                        <ACA:AccelaRadioButtonList ID="radioListAppGender" runat="server" LabelKey="per_appInfoEdit_label_gender"
                            RepeatDirection="Horizontal" />
                        <ACA:AccelaTextBox ID="txtAppOrganizationName" MaxLength="255" runat="server" LabelKey="per_appInfoEdit_label_organization"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppTradeName" MaxLength="100" runat="server" LabelKey="per_appinfoedit_label_tradename"></ACA:AccelaTextBox>
                        <ACA:AccelaSSNText ID="txtSSN" MaxLength="11" LabelKey="aca_contactedit_ssn" runat="server"></ACA:AccelaSSNText>
                        <ACA:AccelaFeinText ID="txtAppFein" MaxLength="11" LabelKey="per_appinfoedit_label_fein"
                            runat="server"></ACA:AccelaFeinText>
                        <ACA:AccelaCountryDropDownList ID="ddlAppCountry" LabelKey="per_appInfoEdit_label_country"
                            runat="server" ShowType="showdescription" IsAlwaysShow="true">
                        </ACA:AccelaCountryDropDownList>
                        <ACA:AccelaTextBox ID="txtAppStreetAdd1" MaxLength="200" runat="server" LabelKey="per_appInfoEdit_label_addressLine1"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppStreetAdd2" MaxLength="200" runat="server" LabelKey="per_appInfoEdit_label_addressLine2"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppStreetAdd3" MaxLength="200" runat="server" LabelKey="per_appinfoedit_label_addressline3"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtAppCity" PositionID="SpearFormContactsCity" AutoFillType="City"
                            runat="server" MaxLength="30" LabelKey="per_appInfoEdit_label_city"></ACA:AccelaTextBox>
                        <ACA:AccelaStateControl ID="txtAppState" PositionID="SpearFormContactsState" AutoFillType="State"
                            LabelKey="per_appInfoEdit_label_state" runat="server" />
                        <ACA:AccelaZipText ID="txtAppZipApplicant" runat="server" LabelKey="per_appInfoEdit_label_zip"></ACA:AccelaZipText>
                        <ACA:AccelaTextBox ID="txtAppPOBox" MaxLength="40" runat="server" LabelKey="per_appInfoEdit_label_poBox"></ACA:AccelaTextBox>
                        <ACA:AccelaPhoneText ID="txtAppPhone1" runat="server" LabelKey="per_appInfoEdit_label_phone1"></ACA:AccelaPhoneText>
                        <ACA:AccelaPhoneText ID="txtAppPhone3" runat="server" LabelKey="per_appInfoEdit_label_phone3"></ACA:AccelaPhoneText>
                        <ACA:AccelaPhoneText ID="txtAppPhone2" runat="server" LabelKey="per_appInfoEdit_label_phone2"></ACA:AccelaPhoneText>
                        <ACA:AccelaPhoneText ID="txtAppFax" runat="server" LabelKey="per_appInfoEdit_label_fax"></ACA:AccelaPhoneText>
                        <ACA:AccelaEmailText ID="txtAppEmail" MaxLength="70" runat="server" LabelKey="per_appInfoEdit_label_email"></ACA:AccelaEmailText>
                        <ACA:AccelaTextBox ID="txtAppSuffix" MaxLength="10" runat="server" LabelKey="aca_contact_label_suffix"></ACA:AccelaTextBox>
                        <ACA:AccelaDropDownList ID="ddlContactTypeFlag" AutoPostBack="false" IsFieldRequired="true" runat="server" LabelKey="aca_per_appinfoedit_label_contacttype_flag" ToolTipLabelKey="aca_common_msg_dropdown_refreshfields_tip" OnSelectedIndexChanged="ContactTypeFlag_SelectedIndexChanged">
                        </ACA:AccelaDropDownList>
                        <ACA:AccelaTextBox ID="txtBusinessName2" LabelKey="aca_contactedit_label_business_name2" runat="server"></ACA:AccelaTextBox>
                        <ACA:AccelaTextBox ID="txtBirthplaceCity" PositionID="SpearFormContactBirthplaceCity" AutoFillType="City" LabelKey="aca_contactedit_label_birthplace_city" runat="server" />
                        <ACA:AccelaStateControl ID="ddlBirthplaceState" PositionID="SpearFormContactBirthplaceState" AutoFillType="State" LabelKey="aca_contactedit_label_birthplace_state" runat="server" />
                        <ACA:AccelaCountryDropDownList ID="ddlBirthplaceCountry" LabelKey="aca_contactedit_label_country" runat="server" />
                        <ACA:AccelaDropDownList ID="ddlRace" LabelKey="aca_contactedit_label_race" runat="server" />
                        <ACA:AccelaCalendarText ID="txtDeceasedDate" LabelKey="aca_contactedit_label_deceased_date" runat="server" />
                        <ACA:AccelaTextBox ID="txtPassportNumber" LabelKey="aca_contactedit_label_passport_number" runat="server" />
                        <ACA:AccelaTextBox ID="txtDriverLicenseNumber" LabelKey="aca_contactedit_label_driver_license_number" runat="server" />
                        <ACA:AccelaStateControl ID="ddlDriverLicenseState" PositionID="SpearFormContactDriverLicenseState" AutoFillType="State" LabelKey="aca_contactedit_label_driver_license_state" runat="server" />
                        <ACA:AccelaTextBox ID="txtStateNumber" LabelKey="aca_contactedit_label_state_id_number" runat="server" />
                        <uc1:TemplateEdit ID="templateEdit" runat="server" />
                        <ACA:GenericTemplate ID="genericTemplate" runat="server" />
                        <uc1:ContactPermissionControl ID="radioListContactPermission" runat="server" />
                        <ACA:AccelaAKA ID="txtAKAName" LabelKey="aca_contactedit_label_akaname" runat="server"
                            FirstNameLabelKey="per_appInfoEdit_label_firstName" MiddleNameLabelKey="per_appInfoEdit_label_middleName" LastNameLabelKey="per_appInfoEdit_label_lastName" FullNameLabelKey="aca_contactedit_label_fullname"
                            AddButtonLabelKey="aca_aka_button_label_add" DelButtonLabelKey="aca_aka_button_label_delete" MaxLength="70" />
                        <ACA:AccelaDropDownList ID="ddlPreferredChannel" runat="server" LabelKey="aca_contactedit_label_preferredchannel"></ACA:AccelaDropDownList>
                        <ACA:AccelaTextBox ID="txtNotes" runat="server" CssClass="ACA_XLong" Rows="4" TextMode="MultiLine" Validate="MaxLength" LabelKey="aca_contactedit_label_notes" MaxLength="240"></ACA:AccelaTextBox>
                    </ACA:AccelaFormDesignerPlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="divContactAddressList" runat="server">
                <asp:UpdatePanel ID="panelContactView" UpdateMode="conditional" runat="server">
                    <ContentTemplate>
                        <ACA:ContactAddressList ID="contactAddressList" GViewID="60134" runat="server"
                            OnContactAddressSelected="ContactAddressList_ContactAddressSelected"
                            NeedCreateSession="False"
                            OnContactAddressDeactivate="ContactAddressList_ContactAddressDeactivate"
                            OnDataSourceChanged="ContactAddressList_DataSourceChanged">
                        </ACA:ContactAddressList>
                        <asp:LinkButton ID="btnRefreshAddressList" runat="Server" CssClass="ACA_Hide"
                            OnClick="RefreshAddressListButton_Click" TabIndex="-1"></asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divAcceptCloseMatchConfirm" Visible="False" runat="server" class="CheckContainer">
                <ACA:AccelaRadioButton ID="rdoAcceptCloseMatchConfirm" LabelKey="aca_contactclosematchconfirm_label_acceptreferencecontact" runat="server" GroupName="CloseMatchConfirm" />
            </div>
            <div id="divUpdateCloseMatchConfirm" Visible="False" runat="server" class="CheckContainer">
                <ACA:AccelaRadioButton ID="rdoUpdateCloseMatchConfirm" LabelKey="aca_contactclosematchconfirm_label_updatereferencecontact" runat="server" GroupName="CloseMatchConfirm" />
            </div>
            <div id="divRejectCloseMatchConfirm" Visible="False" runat="server" class="CheckContainer">
                <ACA:AccelaRadioButton ID="rdoRejectCloseMatchConfirm" LabelKey="aca_contactclosematchconfirm_label_rejectreferencecontact" runat="server" GroupName="CloseMatchConfirm" />
            </div>
        </div>
        <asp:HiddenField ID="hdnDisableStatus" runat="server" />
        <asp:HiddenField ID="hdnContactSeqNumber" runat="server" />
        <asp:HiddenField ID="hdnRefContactSeqNumber" runat="server" />
        <asp:HiddenField ID="hfIsForNewContactAddress" runat="server" />
        <asp:HiddenField ID="hfSSN" runat="server" />
        <asp:HiddenField ID="hfFEIN" runat="server" />
        <asp:HiddenField ID="hfLockStandardFileds" runat="server"/>
        <asp:HiddenField ID="hfContatType" runat="server"/>
        <!--
        0: Default status.
        1: Postback triggered by auto-fill function, so need to re-fill & re-validate edit form in the EndRequest event.
         -->
        <asp:HiddenField ID="hdfPostbackFlag" Value="0" runat="server" />
        <asp:HiddenField ID="hdfAutoFillData" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var contactSectionPosition = '<%=ContactSectionPosition %>';
    var <%=ClientID %>_DisabledInputArray4ContactTypeFlag = { };

    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=ddlContactType.ClientID %>");
    }

    prm.add_endRequest(<%=ClientID %>_EndRequest);
    prm.add_pageLoaded(<%=ClientID %>_PageLoaded);

    function <%=ClientID %>_EndRequest(sender, args) {
        <%=ClientID %>_LockStandardFields();
    }

    /*
     * If Sync Contact switch is enabled, needs to lock standard fields for below scenario:
     * If DataSource property of Contact section is 'NoLimitation', all standard fields will
     *   be locked except the field is requried but filed value is empty.
     */
    function <%=ClientID %>_LockStandardFields(){
        var lockStandardFileds = $("#"+"<%=hfLockStandardFileds.ClientID %>").val();
        if(lockStandardFileds == "Y" && $("#<%=hdnRefContactSeqNumber.ClientID %>").val() != ""){
            var permissionValue = '<%=ScriptFilter.EncodeJson(Permission.permissionValue) %>';

            <%=ClientID %>_GetStandardFieldsRequriedInfo(permissionValue);
        }
    }

    function <%=ClientID %>_PageLoaded(sender, args)
    {
        /*
        Add the Contact Type as the CSS class to the top container.
        Allow agency to customize different look & feel of the same standard field/template field in case of different contact type.
        */
        var topContainer = $get('<%=ContactEditPanel.ClientID %>');
        var contactType = $get('<%=ddlContactType.ClientID %>');

        if(topContainer && contactType && contactType.selectedIndex >= 0)
        {
            topContainer.className = GetValidCssClassName(contactType[contactType.selectedIndex].value);
        }

        var isSectionEditable = <%=IsEditable.ToString().ToLower() %>;
        var disableStatus = IsTrue($get('<%=hdnDisableStatus.ClientID %>').value) || !isSectionEditable;
        var isLockStandardFields = $("#<%=hfLockStandardFileds.ClientID %>").val() == "Y" && $("#<%=hdnRefContactSeqNumber.ClientID %>").val() != "";

        if(!$.global.isAdmin && contactSectionPosition == "SpearForm" && !isLockStandardFields)
        {
            <%=ClientID %>_disableContactForm(disableStatus);
        }

        if(!disableStatus) 
		{
            <%=ddlContactTypeFlag.ClientID %>_onchange(false);
        }
    }
    
    function <%=ParentID %>_Refresh(isFromAddress, isForNew) {
        if (isForNew != undefined && isForNew) {
            $("#<%=hfIsForNewContactAddress.ClientID %>").val("1");
        } else {
            $("#<%=hfIsForNewContactAddress.ClientID %>").val("0");
        }
        
        __doPostBack('<%=btnRefreshAddressList.UniqueID %>', null);
    }

    function <%=ddlContactType.ClientID %>_onchange() {
        $('#<%=hdfPostbackFlag.ClientID%>').val('0');
    }

    function <%=ClientID %>_GetStandardFieldsRequriedInfo(permissionValue){
        var controPrefix =  '<%= ClientID + ClientIDSeparator %>';
        PageMethods.GetStandardFieldsRequriedInfo('<%=ConfigManager.AgencyCode %>', '<%=ModuleName %>', permissionValue, controPrefix, <%=ClientID %>_CallbackStandardFieldsRequiredInfo);
    }

    function <%=ClientID %>_CallbackStandardFieldsRequiredInfo(result){
        if(result != ''){
            var results=eval('(' + result + ')');
            var isUseZip = $("#<%=txtAppZipApplicant.ClientID %>").attr("isusezip") == 'true';

            for(var i=0; i < results.length; i++){
                var controlId = results[i].ControlId;
                var isContactType = '<%=ddlContactType.ClientID %>' == controlId;
                var control = $get(controlId);
                var isRequiredControlNoValue = control != null && (GetValue(control) == '' || GetValue(control) == null) && results[i].Required == 'true';
                
                if(controlId == '<%=radioListAppGender.ClientID %>'){
                    isRequiredControlNoValue = GetRadioButtonSelectedValue(control) == '' && results[i].Required == 'true';
                }

                var isControlHasIDD = controlId == '<%=txtAppPhone1.ClientID %>' || controlId == '<%=txtAppPhone2.ClientID %>' || controlId =='<%=txtAppPhone3.ClientID %>' || controlId == '<%=txtAppFax.ClientID %>';

                if (controlId == '<%=txtAKAName.ClientID%>') {
                    continue;
                }
                /*
                 1. Contact type field always editable.
                 2. If field is not required, disabled
                 3. If field is required and not empty, disabled.
                 4. If field is required and value is empty but not Zip field, editable.
                 5. If field is required and valie is empty but is Zip field: a. if current regional use Zip - editable; b. if current regional not use Zip - disabled.
                 */
                if(!isContactType &&
                    (!isRequiredControlNoValue || (isRequiredControlNoValue && controlId == '<%=txtAppZipApplicant.ClientID %>' && !isUseZip))) {
                    SetFieldToDisabled(controlId, true);

                    //Disable IDD control.
                    if(isControlHasIDD){
                        SetFieldToDisabled(controlId + "_IDD", true);
                    }

                    doErrorCallbackFun('',controlId,0);
                }
            }
        }
    }

    function ButtonClientClick()
    {
        if(typeof(SetNotAsk) != 'undefined')
        {
            SetNotAsk();
        }

        SetCurrentValidationSectionID('<%=ClientID %>'); 
    }

    function ContactRelationShipsRadioListChange(ctlId, checkCtlId)
    {
        var ctl = $get(ctlId);
        var noSelected = true;

        if(ctl != null)
        {
            var radioItems = ctl.getElementsByTagName("INPUT");

            for(var i=0; i<radioItems.length; i++)
            {
                if(radioItems[i].checked)
                {
                    if (radioItems[i].readOnly)
                    {
                        continue;
                    }

                    if(radioItems[i].value != "C")
                    {
                        DisableCheckBoxList(checkCtlId, true);
                    }
                    else
                    {
                        DisableCheckBoxList(checkCtlId, false);
                        //ContactRelationShipsCheckListChange(checkCtlId);
                    }

                    noSelected = false;
                }   
            }

            if(noSelected)
            {
                DisableCheckBoxList(checkCtlId, true);
            }
        }
    }

    function ContactRelationShipsCheckListChange(ctlId)
    {
        var ctl = $get(ctlId);

        if(ctl != null)
        {
            var radioItems = ctl.getElementsByTagName("INPUT");

            for(var i=0; i < radioItems.length; i++)
            {
                if(radioItems[i].value == "C" && !radioItems[i].readOnly)
                {
                    radioItems[i].checked = true;
                }
            }  
        }
    }

    function DisableCheckBoxList(ctlId, isDisabled)
    {
        var ctl = $get(ctlId);

        if(ctl != null)
        {
            ctl.disabled = isDisabled;

            var checkItems = ctl.getElementsByTagName("INPUT");

            for(var i=0; i<checkItems.length; i++)   
            {   
                checkItems[i].disabled = isDisabled;  
            }  
        }
    }

    function <%=ClientID %>_disableContactForm(isDisable) {
        var form = document.getElementById("<%=ContactEditPanel.ClientID %>");
        disableForm(form, "", "", isDisable);

        if (typeof(DisableAKAField) != 'undefined') {
            if (isDisable) {
                DisableAKAField('<% =txtAKAName.ClientID%>');
            } else {
                EnableAKAField('<% =txtAKAName.ClientID%>');
            }
        }
    }

    var <%=ddlContactTypeFlag.ClientID %>_IsFirstTime = true;
    function <%=ddlContactTypeFlag.ClientID %>_onchange(isSelectedIndexChange) {
        if(isSelectedIndexChange == true && <%=ddlContactTypeFlag.ClientID %>_IsFirstTime == true) {
            <%=ClientID %>_SaveDisabledStatus4ContactTypeFlag();
            <%=ddlContactTypeFlag.ClientID %>_IsFirstTime = false;
        }
        
        var ddlcontactTypeFlag = $get('<%=ddlContactTypeFlag.ClientID %>');
        var contactTypeFlagVal = '<%=ContactTypeFlag %>';

        if(!$.global.isAdmin && (ddlcontactTypeFlag != null || contactTypeFlagVal != ""))
        {
            var lockStandardFileds = $("#"+"<%=hfLockStandardFileds.ClientID %>").val();
            var hdnRefContactSeqNumber = $("#<%=hdnRefContactSeqNumber.ClientID %>").val();

            if (lockStandardFileds == "Y" && hdnRefContactSeqNumber != "") {
                //no need enable fields.
            }
            else
            {
                SetFieldToDisabled('<%=txtAppOrganizationName.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppTradeName.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppFein.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppFirstName.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppLastName.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppFullName.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppMiddleName.ClientID %>',false);
                SetFieldToDisabled('<%=txtTitle.ClientID %>',false);
                SetFieldToDisabled('<%=ddlAppSalutation.ClientID %>',false);
                SetFieldToDisabled('<%=radioListAppGender.ClientID %>',false);
                SetFieldToDisabled('<%=txtAppBirthDate.ClientID %>',false);

                if (typeof(EnableAKAField) != "undefined") {
                    EnableAKAField('<% =txtAKAName.ClientID%>');
                }

                SetFieldToDisabled('<%=txtSSN.ClientID %>', false);
                SetFieldToDisabled('<%=txtBirthplaceCity.ClientID %>', false);
                SetFieldToDisabled('<%=ddlBirthplaceState.ClientID %>', false);
                SetFieldToDisabled('<%=ddlBirthplaceCountry.ClientID %>', false);
                SetFieldToDisabled('<%=ddlRace.ClientID %>', false);
                SetFieldToDisabled('<%=txtDeceasedDate.ClientID %>', false);
                SetFieldToDisabled('<%=txtPassportNumber.ClientID %>', false);
                SetFieldToDisabled('<%=txtDriverLicenseNumber.ClientID %>', false);
                SetFieldToDisabled('<%=ddlDriverLicenseState.ClientID %>', false);
                SetFieldToDisabled('<%=txtStateNumber.ClientID %>', false);
            }

            var selectContactTypeFlag = "";

            if (ddlcontactTypeFlag != null && ddlcontactTypeFlag.selectedIndex != null && ddlcontactTypeFlag.selectedIndex > -1) {
                selectContactTypeFlag = ddlcontactTypeFlag[ddlcontactTypeFlag.selectedIndex].value;
            }
            else 
            {
                selectContactTypeFlag = contactTypeFlagVal;
            }

            if (selectContactTypeFlag != "") {
                if (selectContactTypeFlag.toLowerCase() == "<%=INDIVIDUAL %>") {
                    doErrorCallbackFun('', '<%=txtAppOrganizationName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtAppTradeName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtAppFein.ClientID %>', 0);

                    SetFieldToDisabled('<%=txtAppOrganizationName.ClientID %>', true);
                    $('#<%=txtAppOrganizationName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtAppTradeName.ClientID %>', true);
                    $('#<%=txtAppTradeName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtAppFein.ClientID %>', true);
                    $('#<%=txtAppFein.ClientID %>').val("");
                }
				else if(isSelectedIndexChange)
				{
                    <%=ClientID %>_ResetEnableControl('<%=txtAppOrganizationName.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtAppTradeName.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtAppFein.ClientID %>');
                }

                if (selectContactTypeFlag.toLowerCase() == "<%=ORGANIZATION %>") {
                    doErrorCallbackFun('', '<%=txtAppFirstName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtAppLastName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtAppFullName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtAppMiddleName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtSSN.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtTitle.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlAppSalutation.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=radioListAppGender.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtAppBirthDate.ClientID %>', 0);

                    doErrorCallbackFun('', '<%=txtBirthplaceCity.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlBirthplaceState.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlBirthplaceCountry.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlRace.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtDeceasedDate.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtPassportNumber.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtDriverLicenseNumber.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlDriverLicenseState.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtStateNumber.ClientID %>', 0);

                    SetFieldToDisabled('<%=txtAppFirstName.ClientID %>', true);
                    $('#<%=txtAppFirstName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtAppLastName.ClientID %>', true);
                    $('#<%=txtAppLastName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtAppFullName.ClientID %>', true);
                    $('#<%=txtAppFullName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtAppMiddleName.ClientID %>', true);
                    $('#<%=txtAppMiddleName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtSSN.ClientID %>', true);
                    $('#<%=txtSSN.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtTitle.ClientID %>', true);
                    $('#<%=txtTitle.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlAppSalutation.ClientID %>', true);
                    $('#<%=ddlAppSalutation.ClientID %>').val("");
                    SetFieldToDisabled('<%=radioListAppGender.ClientID %>', true);
                    SetRadioListValue('<%=radioListAppGender.ClientID%>', "");
                    SetFieldToDisabled('<%=txtAppBirthDate.ClientID %>', true);
                    $('#<%=txtAppBirthDate.ClientID %>').val("");

                    if (typeof(DisableAKAField) != "undefined") {
                        DisableAKAField('<% =txtAKAName.ClientID%>');
                        SetAKAValue('<% = txtAKAName.ClientID %>', '');
                    }

                    SetFieldToDisabled('<%=txtBirthplaceCity.ClientID %>', true);
                    $('#<%=txtBirthplaceCity.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlBirthplaceState.ClientID %>', true);
                    $('#<%=ddlBirthplaceState.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlBirthplaceCountry.ClientID %>', true);
                    $('#<%=ddlBirthplaceCountry.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlRace.ClientID %>', true);
                    $('#<%=ddlRace.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtDeceasedDate.ClientID %>', true);
                    $('#<%=txtDeceasedDate.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtPassportNumber.ClientID %>', true);
                    $('#<%=txtPassportNumber.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtDriverLicenseNumber.ClientID %>', true);
                    $('#<%=txtDriverLicenseNumber.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlDriverLicenseState.ClientID %>', true);
                    $('#<%=ddlDriverLicenseState.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtStateNumber.ClientID %>', true);
                    $('#<%=txtStateNumber.ClientID %>').val("");
                }
                else if(isSelectedIndexChange)
                {
                    <%=ClientID %>_ResetEnableControl('<%=txtAppFirstName.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtAppLastName.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtAppFullName.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtAppMiddleName.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtSSN.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtTitle.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=ddlAppSalutation.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=radioListAppGender.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtAppBirthDate.ClientID %>');
                    
                    if(typeof(EnableAKAField) != "undefined" && typeof (<%=ClientID %>_DisabledInputArray4ContactTypeFlag['<% =txtAKAName.ClientID%>']) == 'undefined') {
                        EnableAKAField('<% =txtAKAName.ClientID%>');
                    }
                    
                    <%=ClientID %>_ResetEnableControl('<%=txtBirthplaceCity.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=ddlBirthplaceState.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=ddlBirthplaceCountry.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=ddlRace.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtDeceasedDate.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtPassportNumber.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtDriverLicenseNumber.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=ddlDriverLicenseState.ClientID %>');
                    <%=ClientID %>_ResetEnableControl('<%=txtStateNumber.ClientID %>');
                }
            }
        }
    }

    function <%=ClientID %>_ResetEnableControl(controlId) {
        if(typeof (<%=ClientID %>_DisabledInputArray4ContactTypeFlag[controlId]) == 'undefined' || isRequiredAndEmptyField(controlId)) {
            SetFieldToDisabled(controlId, false);
        }
    }

    function <%=ClientID %>_SaveDisabledStatus4ContactTypeFlag() {
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppOrganizationName.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppTradeName.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppFein.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppFirstName.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppLastName.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppFullName.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppMiddleName.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtSSN.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtTitle.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=ddlAppSalutation.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=radioListAppGender.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtAppBirthDate.ClientID %>');

        if($('input:visible[id^=<%=txtAKAName.ClientID %>]').hasClass("ACA_ReadOnly")) {
            <%=ClientID %>_DisabledInputArray4ContactTypeFlag['<%=txtAKAName.ClientID %>'] = '<%=txtAKAName.ClientID %>';
        }
        <%=ClientID %>_SaveReadonlyControl('<%=txtBirthplaceCity.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=ddlBirthplaceState.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=ddlBirthplaceCountry.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=ddlRace.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtDeceasedDate.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtPassportNumber.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtDriverLicenseNumber.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=ddlDriverLicenseState.ClientID %>');
        <%=ClientID %>_SaveReadonlyControl('<%=txtStateNumber.ClientID %>');
    }

    function <%=ClientID %>_SaveReadonlyControl(controlId) {
        if($('#' + controlId).hasClass("ACA_ReadOnly")) {
            <%=ClientID %>_DisabledInputArray4ContactTypeFlag[controlId] = controlId;
        }
    }
</script>