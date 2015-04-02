<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuthorizedAgentCustomerEdit.ascx.cs" Inherits="Accela.ACA.Web.Component.AuthorizedAgentCustomerEdit" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="~/Component/ContactAddressList.ascx" TagName="ContactAddressList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefContactList.ascx" TagName="RefContactList" TagPrefix="ACA" %>
<asp:UpdatePanel ID="CustomerEditPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="UpdatePanel4ContactType" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
                    <ACA:AccelaDropDownList ID="ddlContactType" LabelKey="aca_authagent_customersearch_label_contacttype" AutoPostBack="True" OnSelectedIndexChanged="ContactType_SelectedIndexChanged" runat="server"/>
                    <ACA:AccelaNumberText ID="txtCustomerID" LabelKey="aca_authagent_customersearch_label_customerid" runat="server"/>
                    <ACA:AccelaTextBox ID="txtLicenseID" LabelKey="aca_authagent_customersearch_label_licenseid" runat="server"/>
                    <ACA:AccelaTextBox ID="txtFirstName" LabelKey="aca_authagent_customersearch_label_firstname" EnableSoundexSearch="True" runat="server"/>
                    <ACA:AccelaTextBox ID="txtLastName" LabelKey="aca_authagent_customersearch_label_lastname" EnableSoundexSearch="True" runat="server"/>
                    <ACA:AccelaTextBox ID="txtMiddleName" LabelKey="aca_authagent_customersearch_label_middlename" EnableSoundexSearch="True" runat="server"/>
                    <ACA:AccelaTextBox ID="txtFullName" LabelKey="aca_authagent_customersearch_label_fullname" runat="server"/>
                    <ACA:AccelaCalendarText ID="txtBirthDate" LabelKey="aca_authagent_customersearch_label_birthdate" runat="server"/>
                    <ACA:AccelaTextBox ID="txtBirthCity" LabelKey="aca_authagent_customersearch_label_birthplace_city" runat="server"/>
                    <ACA:AccelaStateControl ID="txtBirthState" LabelKey="aca_authagent_customersearch_label_birthplace_state" PositionID="SpearFormContactBirthplaceState" AutoFillType="State" runat="server"/>
                    <ACA:AccelaCountryDropDownList ID="ddlBirthCountry" LabelKey="aca_authagent_customersearch_label_birthplace_country" runat="server"/>
                    <ACA:AccelaPhoneText ID="txtPhone1" LabelKey="aca_authagent_customersearch_label_phone1" runat="server"/>
                    <ACA:AccelaPhoneText ID="txtPhone2" LabelKey="aca_authagent_customersearch_label_phone2" runat="server"/>
                    <ACA:AccelaPhoneText ID="txtPhone3" LabelKey="aca_authagent_customersearch_label_phone3" runat="server"/>
                    <ACA:AccelaTextBox ID="txtAddressLine1" LabelKey="aca_authagent_customersearch_label_addressline1" runat="server"/>
                    <ACA:AccelaTextBox ID="txtAddressLine2" LabelKey="aca_authagent_customersearch_label_addressline2" runat="server"/>
                    <ACA:AccelaTextBox ID="txtAddressLine3" LabelKey="aca_authagent_customersearch_label_addressline3" runat="server"/>
                    <ACA:AccelaTextBox ID="txtCity" LabelKey="aca_authagent_customersearch_label_city" PositionID="SpearFormContactsCity" AutoFillType="City" runat="server"/>
                    <ACA:AccelaStateControl ID="txtState" LabelKey="aca_authagent_customersearch_label_state" PositionID="SpearFormContactsState" AutoFillType="State" runat="server"/>
                    <ACA:AccelaZipText ID="txtZip" LabelKey="aca_authagent_customersearch_label_zip" runat="server"/>
                    <ACA:AccelaCountryDropDownList ID="ddlCountry" LabelKey="aca_authagent_customersearch_label_country" ShowType="ShowDescription" IsAlwaysShow="true" runat="server"/>
                    <ACA:AccelaTextBox ID="txtPostOfficeBox" LabelKey="aca_authagent_customersearch_label_pobox" runat="server"/>
                    <ACA:AccelaPhoneText ID="txtFax" LabelKey="aca_authagent_customersearch_label_fax" runat="server"/>
                    <ACA:AccelaEmailText ID="txtEmail" LabelKey="aca_authagent_customersearch_label_email" MaxLength="70" runat="server"/>
                    <ACA:AccelaDropDownList ID="ddlSalutation" LabelKey="aca_authagent_customersearch_label_salutation" ShowType="ShowDescription" runat="server"/>
                    <ACA:AccelaRadioButtonList ID="rdolistGender" LabelKey="aca_authagent_customersearch_label_gender" RepeatDirection="Horizontal" runat="server"/>
                    <ACA:AccelaDropDownList ID="ddlContactTypeFlag" IsFieldRequired="true" LabelKey="aca_authagent_customersearch_label_contacttype_flag" ToolTipLabelKey="aca_common_msg_dropdown_refreshfields_tip" runat="server"/>
                    <ACA:AccelaTextBox ID="txtTradeName" LabelKey="aca_authagent_customersearch_label_tradename" runat="server"/>
                    <ACA:AccelaFeinText ID="txtFein" LabelKey="aca_authagent_customersearch_label_fein" MaxLength="11" runat="server"/>
                    <ACA:AccelaTextBox ID="txtTitle" LabelKey="aca_authagent_customersearch_label_title" runat="server"/>
                    <ACA:AccelaSSNText ID="txtSSN" LabelKey="aca_authagent_customersearch_label_ssn" MaxLength="11" runat="server"/>
                    <ACA:AccelaTextBox ID="txtSuffix" LabelKey="aca_authagent_customersearch_label_suffix" MaxLength="10" runat="server"/>
                    <ACA:AccelaTextBox ID="txtBusinessName" LabelKey="aca_authagent_customersearch_label_organization" runat="server"/>
                    <ACA:AccelaTextBox ID="txtBusinessName2" LabelKey="aca_authagent_customersearch_label_business_name2" runat="server"/>
                    <ACA:AccelaDropDownList ID="ddlRace" LabelKey="aca_authagent_customersearch_label_race" runat="server"/>
                    <ACA:AccelaCalendarText ID="txtDeceasedDate" LabelKey="aca_authagent_customersearch_label_deceased_date" runat="server"/>
                    <ACA:AccelaTextBox ID="txtPassportNumber" LabelKey="aca_authagent_customersearch_label_passport_number" runat="server"/>
                    <ACA:AccelaTextBox ID="txtDriverLicenseNbr" LabelKey="aca_authagent_customersearch_label_driverlicensenumber" runat="server"/>
                    <ACA:AccelaStateControl ID="txtDriverLicenseState" LabelKey="aca_authagent_customersearch_label_driverlicensestate" PositionID="SpearFormContactDriverLicenseState" AutoFillType="State" runat="server"/>
                    <ACA:AccelaTextBox ID="txtStateIdNbr" LabelKey="aca_authagent_customersearch_label_state_id_number" runat="server"/>
                    <ACA:AccelaTextBox ID="txtNotes" LabelKey="aca_authagent_customersearch_label_notes" CssClass="ACA_XLong" Rows="4" TextMode="MultiLine" Validate="MaxLength" MaxLength="240" runat="server"/>
                    <ACA:AccelaDropDownList ID="ddlRecordType" LabelKey="aca_authagent_customersearch_label_recordtype" runat="server"/>
                    <ACA:TemplateEdit ID="peopleTemplate" runat="server" />
                    <ACA:GenericTemplate ID="genericTemplate" runat="server" />
                    <ACA:AccelaDropDownList ID="ddlPreferredChannel" runat="server" LabelKey="aca_authagent_customerdetail_label_preferredchannel"></ACA:AccelaDropDownList>
                </ACA:AccelaFormDesignerPlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
        <ACA:AccelaCheckBox ID="chkEnableSoundexSearch" runat="server" LabelKey="aca_authagent_customersearch_label_enablesoundexsearch" />

        <!-- The Contact Address section -->
        <div id="divContactAddress" class="SectionContactAddress" visible="false" runat="server">
            <ACA:ContactAddressList ID="ucContactAddressList" ContactSectionPosition="AuthAgentCustomerDetail" runat="server" NeedCreateSession="True"
                OnDataSourceChanged="ContactAddressList_DataSourceChanged">
            </ACA:ContactAddressList>
        </div>

        <asp:HiddenField ID="hfSSN" runat="server" />
        <asp:HiddenField ID="hfFEIN" runat="server" />
        <asp:HiddenField ID="hdnContactSeqNumber" runat="server" />

        <!-- The button section -->
        <div class="ACA_Row ACA_LiLeft SectionButton" runat="server" id="divButtons">
            <ul>
                <span class="ACA_Error_Indicator" runat="server" id="imgErrorIcon" style="position: relative;margin-top: 4px;" visible="false">
                    <img class="ACA_Error_Indicator_In_Same_Row" alt="<%=GetTextByKey("aca_global_js_showerror_alt") %>" src="<%=ImageUtil.GetImageURL("error_16.gif") %>" />
                </span>
                <li id="liSearchBtn" runat="server">
                    <ACA:AccelaButton ID="btnSearch" runat="server" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" LabelKey="aca_authagent_customerdetail_label_searchbutton"
                        OnClick="SearchButton_Click">
                    </ACA:AccelaButton>
                </li>
                <li>
                    <ACA:AccelaButton ID="btnClear" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
                        DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize" runat="server"
                        LabelKey="aca_authagent_customerdetail_label_clearbutton" OnClick="ClearButton_Click" CausesValidation="false">
                    </ACA:AccelaButton>
                </li>
            </ul>
        </div>

        <!-- The search result section -->
        <div id="divAddCustomer" class="ACA_Row ACA_LiLeft ACA_SmLabel ACA_SmLabel_FontSize SectionSearchResult" runat="server" Visible="False">
            <ACA:AccelaLabel ID="lblEmptyResult" LabelKey="aca_authagent_customersearch_label_emptyresultpart1" runat="server"></ACA:AccelaLabel>

            <span class="ACA_LinkButton">
                <ACA:AccelaLinkButton ID="lnkAddCustomer" LabelKey="aca_authagent_customersearch_label_emptyresultpart2" runat="server" OnClick="AddCustomerLink_Click" CausesValidation="False"></ACA:AccelaLinkButton>.
            </span>
            <div class="ACA_TabRow ACA_Line_Content">&nbsp;</div>
        </div>
        <div id="divResultNotice" class="ACA_TabRow" runat="server" visible="false">
            <p>
                <ACA:AccelaLabel ID="lblResultNotice" LabelKey="aca_authagent_customersearch_msg_searchresultnotice" IsNeedEncode="false" runat="server"></ACA:AccelaLabel>
            </p>
        </div>
        <div class="ACA_Row">
            <ACA:RefContactList ID="refContactList" GViewID="60157" Visible="false" runat="server"></ACA:RefContactList>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    if (typeof (myValidationErrorPanel) != "undefined") {
        myValidationErrorPanel.registerIDs4Recheck("<%=ddlContactType.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=btnSearch.ClientID %>");
        myValidationErrorPanel.registerIDs4Recheck("<%=btnClear.ClientID %>");
    }

    with (Sys.WebForms.PageRequestManager.getInstance()) {
        add_pageLoaded(function () {
            if (!$.global.isAdmin) {
                var chkEnableSoundexSearch = document.getElementById("<%=chkEnableSoundexSearch.ClientID %>");

                if (chkEnableSoundexSearch && chkEnableSoundexSearch.checked) {
                    SwitchSoundexSearch(true, '<%=UpdatePanel4ContactType.ClientID %>');
                }

                //Support Enter key for search function.
                if ("<%=SectionPosition %>" == "SearchForm") {
                    $('#<%=UpdatePanel4ContactType.ClientID%>').find("input:text").keydown(function (e) {
                        if (e.keyCode == 13) {
                            var eleOnClickattr = $('#<%=btnSearch.ClientID%>').attr('href');
                            eval(eleOnClickattr);
                            return false;
                        }
                    });
                }
            }
            var editableStatus = IsTrue('<%=IsEditable %>');

            if (editableStatus) {
                ddlContactTypeFlag_onchange();
            }
        });
    }

    //if current form is searchForm, it wouldn't check control whether required or not. 
    function IsNeedValidation(id) {
        var isSearchFrom = <%=(SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm).ToString().ToLower()%>;

        if (!isSearchFrom) {
            return true;
        }

        var elementToValidate = $('#' + id)[0];
        var value = GetValue(elementToValidate);
        if (typeof (value) != 'undefined') {
            // For checkbox
            if (elementToValidate.type == 'checkbox')
                return false;
            else
                return value.trim() != '';
        } else {
            return true;
        }
    }

    function ddlContactTypeFlag_onchange() {
        var ddlcontactTypeFlag = $get('<%=ddlContactTypeFlag.ClientID %>');
        var contactTypeFlagVal = '<%=ContactTypeFlag %>';

        if(!$.global.isAdmin && (ddlcontactTypeFlag != null || contactTypeFlagVal != ""))
        {
            SetFieldToDisabled('<%=txtBusinessName.ClientID %>', false);
            SetFieldToDisabled('<%=txtTradeName.ClientID %>', false);
            SetFieldToDisabled('<%=txtFein.ClientID %>', false);
            SetFieldToDisabled('<%=txtFirstName.ClientID %>', false);
            SetFieldToDisabled('<%=txtLastName.ClientID %>', false);
            SetFieldToDisabled('<%=txtFullName.ClientID %>', false);
            SetFieldToDisabled('<%=txtMiddleName.ClientID %>', false);
            SetFieldToDisabled('<%=txtTitle.ClientID %>', false);
            SetFieldToDisabled('<%=ddlSalutation.ClientID %>', false);
            SetFieldToDisabled('<%=rdolistGender.ClientID %>', false);
            SetFieldToDisabled('<%=txtBirthDate.ClientID %>', false);

            SetFieldToDisabled('<%=txtSSN.ClientID %>', false);
            SetFieldToDisabled('<%=txtBirthCity.ClientID %>', false);
            SetFieldToDisabled('<%=txtBirthState.ClientID %>', false);
            SetFieldToDisabled('<%=ddlBirthCountry.ClientID %>', false);
            SetFieldToDisabled('<%=ddlRace.ClientID %>', false);
            SetFieldToDisabled('<%=txtDeceasedDate.ClientID %>', false);
            SetFieldToDisabled('<%=txtPassportNumber.ClientID %>', false);
            SetFieldToDisabled('<%=txtDriverLicenseNbr.ClientID %>', false);
            SetFieldToDisabled('<%=txtDriverLicenseState.ClientID %>', false);
            SetFieldToDisabled('<%=txtStateIdNbr.ClientID %>', false);

            var selectContactTypeFlag = "";

            if (ddlcontactTypeFlag != null && ddlcontactTypeFlag.selectedIndex != null && ddlcontactTypeFlag.selectedIndex > -1) {
                selectContactTypeFlag = ddlcontactTypeFlag[ddlcontactTypeFlag.selectedIndex].value;
            }
            else {
                selectContactTypeFlag = contactTypeFlagVal;
            }

            if (selectContactTypeFlag != "") 
            {
                if (selectContactTypeFlag.toLowerCase() == "individual") {
                    doErrorCallbackFun('', '<%=txtBusinessName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtTradeName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtFein.ClientID %>', 0);

                    SetFieldToDisabled('<%=txtBusinessName.ClientID %>', true);
                    $('#<%=txtBusinessName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtTradeName.ClientID %>', true);
                    $('#<%=txtTradeName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtFein.ClientID %>', true);
                    $('#<%=txtFein.ClientID %>').val("");
                } else if (selectContactTypeFlag.toLowerCase() == "organization") {
                    doErrorCallbackFun('', '<%=txtFirstName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtLastName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtFullName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtMiddleName.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtSSN.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtTitle.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlSalutation.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=rdolistGender.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtBirthDate.ClientID %>', 0);

                    doErrorCallbackFun('', '<%=txtBirthCity.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtBirthState.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlBirthCountry.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=ddlRace.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtDeceasedDate.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtPassportNumber.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtDriverLicenseNbr.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtDriverLicenseState.ClientID %>', 0);
                    doErrorCallbackFun('', '<%=txtStateIdNbr.ClientID %>', 0);

                    SetFieldToDisabled('<%=txtFirstName.ClientID %>', true);
                    $('#<%=txtFirstName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtLastName.ClientID %>', true);
                    $('#<%=txtLastName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtFullName.ClientID %>', true);
                    $('#<%=txtFullName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtMiddleName.ClientID %>', true);
                    $('#<%=txtMiddleName.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtSSN.ClientID %>', true);
                    $('#<%=txtSSN.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtTitle.ClientID %>', true);
                    $('#<%=txtTitle.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlSalutation.ClientID %>', true);
                    $('#<%=ddlSalutation.ClientID %>').val("");
                    SetFieldToDisabled('<%=rdolistGender.ClientID %>', true);
                    SetRadioListValue('<%=rdolistGender.ClientID%>', "");
                    SetFieldToDisabled('<%=txtBirthDate.ClientID %>', true);
                    $('#<%=txtBirthDate.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtBirthCity.ClientID %>', true);
                    $('#<%=txtBirthCity.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtBirthState.ClientID %>', true);
                    $('#<%=txtBirthState.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlBirthCountry.ClientID %>', true);
                    $('#<%=ddlBirthCountry.ClientID %>').val("");
                    SetFieldToDisabled('<%=ddlRace.ClientID %>', true);
                    $('#<%=ddlRace.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtDeceasedDate.ClientID %>', true);
                    $('#<%=txtDeceasedDate.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtPassportNumber.ClientID %>', true);
                    $('#<%=txtPassportNumber.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtDriverLicenseNbr.ClientID %>', true);
                    $('#<%=txtDriverLicenseNbr.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtDriverLicenseState.ClientID %>', true);
                    $('#<%=txtDriverLicenseState.ClientID %>').val("");
                    SetFieldToDisabled('<%=txtStateIdNbr.ClientID %>', true);
                    $('#<%=txtStateIdNbr.ClientID %>').val("");
                }
            }
        }
    }
</script>
