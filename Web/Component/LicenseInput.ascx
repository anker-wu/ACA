<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicenseInput.ascx.cs"
    Inherits="Accela.ACA.Web.Component.LicenseInput" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Register Src="TemplateEdit.ascx" TagName="TemplateEdit" TagPrefix="uc1" %>
<%@ Register Src="SupervisorTemplate.ascx" TagName="SupervisorList" TagPrefix="uc1" %>
<script language="javascript" type="text/javascript">
    var defaultSelectText = "<%=WebConstant.DropDownDefaultText%>";
    var ddlLPType = "<%=ddlLicenseType.ClientID %>";
    var templateEdit = "<%=templateEdit.TemplatePanelClientID %>";
    var templateEMSEDDLATTRIBUTEFORNAME = "<%=ACAConstant.TEMPLATE_FIELD_NAME_ATTRIBUTE_FOR_EMSEDDL%>";
    var licenseAgency = "<%=hfLicenseAgencyCode.ClientID %>";
</script>
<script type="text/javascript" src="../Scripts/Template.js"></script>
<asp:UpdatePanel ID="LicenseEditPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
            <ACA:AccelaDropDownList ID="ddlLicenseType" LabelKey="LicenseEdit_licenseSearch_label_licenseType" runat="server" IsDBRequired="true" OnSelectedIndexChanged="LicenseType_SelectedIndexChanged" AutoPostBack="true" />
            <ACA:AccelaTextBox ID="txtLicenseNum" CssClass="ACA_NLong" runat="server" LabelKey="LicenseEdit_licenseSearch_label_stateLicenseNum" MaxLength="30" Validate="maxlength"
                onBlur="GetEMSEDDLOption(this, ddlLPType, licenseAgency);" AutoPostBack="true" OnTextChanged="LicenseNumber_TextChanged"></ACA:AccelaTextBox>
            <ACA:AccelaDropDownList ID="ddlContactType" runat="server" LabelKey="licenseedit_licensepro_contacttype"/>
            <ACA:AccelaSSNText ID="txtSSN" MaxLength="11" LabelKey="licenseedit_licensepro_ssn" runat="server"/>
            <ACA:AccelaFeinText ID="txtFEIN" MaxLength="11" LabelKey="licenseedit_licensepro_fein" runat="server"/>
            <ACA:AccelaDropDownList ID="ddlSalutation" LabelKey="LicenseEdit_LicensePro_label_salutation" CssClass="ACA_NShot" runat="server" ShowType="showdescription"/>
            <ACA:AccelaTextBox ID="txtFirstName" runat="server" CssClass="ACA_NShot" MaxLength="70" LabelKey="LicenseEdit_LicensePro_label_firstName"/>
            <ACA:AccelaTextBox ID="txtMiddleName" MaxLength="70" runat="server" CssClass="ACA_NShot" LabelKey="LicenseEdit_LicensePro_label_middleName"/>
            <ACA:AccelaTextBox ID="txtLastName" runat="server" CssClass="ACA_NShot" MaxLength="70" LabelKey="LicenseEdit_LicensePro_label_lastName"/>
            <ACA:AccelaCalendarText ID="txtBirthDate" CssClass="ACA_NShot" runat="server" LabelKey="LicenseEdit_LicensePro_label_birthDate"/>
            <ACA:AccelaRadioButtonList ID="radioListGender" runat="server" LabelKey="LicenseEdit_LicensePro_label_gender" RepeatDirection="Horizontal"/>
            <ACA:AccelaTextBox ID="txtBusName" MaxLength="255" runat="server" CssClass="ACA_XLong" LabelKey="LicenseEdit_LicensePro_label_business"/>
            <ACA:AccelaTextBox ID="txtBusName2" MaxLength="255" runat="server" CssClass="ACA_XLong" LabelKey="LicenseEdit_LicensePro_label_business2"/>
            <ACA:AccelaTextBox ID="txtBusLicense" MaxLength="15" runat="server" CssClass="ACA_NLong" LabelKey="licenseedit_licensepro_label_buslicense"/>
            <ACA:AccelaCountryDropDownList ID="ddlCountry" LabelKey="LicenseEdit_LicensePro_label_country" runat="server" ShowType="showdescription"/>
            <ACA:AccelaTextBox ID="txtAddress1" MaxLength="200" runat="server" CssClass="ACA_XLong" LabelKey="LicenseEdit_LicensePro_label_addressLine1"/>
            <ACA:AccelaTextBox ID="txtAddress2" MaxLength="200" runat="server" CssClass="ACA_XLong" LabelKey="LicenseEdit_LicensePro_label_addressLine2"/>
            <ACA:AccelaTextBox ID="txtAddress3" MaxLength="40" runat="server" CssClass="ACA_XLong" LabelKey="licenseedit_licensepro_label_addressline3"/>
            <ACA:AccelaTextBox ID="txtCity" PositionID="SpearFormLicenseCity" AutoFillType="City" MaxLength="30" runat="server" CssClass="ACA_NLong" LabelKey="LicenseEdit_LicensePro_label_city"/>
            <ACA:AccelaStateControl ID="txtState" PositionID="SpearFormLicenseState" AutoFillType="State" runat="server" LabelKey="LicenseEdit_LicensePro_label_state"/>
            <ACA:AccelaZipText ID="txtZipCode" runat="server" Validate="zip" CssClass="ACA_NShot" LabelKey="LicenseEdit_LicensePro_label_zip"/>
            <ACA:AccelaTextBox ID="txtPOBox" MaxLength="40" runat="server" CssClass="ACA_NShot" LabelKey="LicenseEdit_LicensePro_label_poBox"/>
            <ACA:AccelaPhoneText ID="txtHomePhone" runat="server" LabelKey="LicenseEdit_LicensePro_label_homePhone"/>
            <ACA:AccelaPhoneText ID="txtMobilePhone" runat="server" LabelKey="LicenseEdit_LicensePro_label_mobile"/>
            <ACA:AccelaPhoneText ID="txtFax" runat="server" LabelKey="LicenseEdit_LicensePro_label_fax"/>
            <ACA:AccelaNumberText ID="txtContractorLicNO" LabelKey="licenseedit_licensepro_label_contractorlicno" CssClass="ACA_Small" runat="server" MaxLength="8" Validate="MaxLength" IsNeedDot="false"/>
            <ACA:AccelaTextBox ID="txtContractorBusiName" runat="server" LabelKey="licenseedit_licensepro_label_contractorbusiname" CssClass="ACA_NLonger" MaxLength="65"/>
            <ACA:AccelaCalendarText ID="txtIssueDate" CssClass="ACA_NShot" runat="server"/>
            <ACA:AccelaCalendarText ID="txtExpirationDate" CssClass="ACA_NShot" runat="server"/>
            <ACA:AccelaTextBox ID="txtSuffix" runat="server" CssClass="ACA_NShot" MaxLength="10" LabelKey="aca_licensee_label_suffix"/>
            <ACA:AccelaEmailText ID="txtEmail" MaxLength="70" runat="server" LabelKey="licenseedit_licensepro_label_email"/>
            <uc1:TemplateEdit ID="templateEdit" runat="server" />
        </ACA:AccelaFormDesignerPlaceHolder>
        <uc1:SupervisorList ID="supervisorList" runat="Server" />
        <asp:HiddenField ID="hfLicenseProId" runat="server" />
         <asp:HiddenField ID="hfLicenseTmpID" runat="server" />
        <asp:HiddenField ID="hfLicenseAgencyCode" runat="server" />
        <asp:HiddenField ID="hfSSN" runat="server" Visible="false" />
        <asp:HiddenField ID="hfFEIN" runat="server" Visible="false" />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">

var LicenseTypeConfigurationErrorMessage = '<%= GetTextByKey("aca_licensetype_readonly_error").Replace("'","\\'")%>';

if (typeof (myValidationErrorPanel) != "undefined") {
    myValidationErrorPanel.registerIDs4Recheck("<%=ddlLicenseType.ClientID %>");
}

var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_pageLoaded(PageLoaded);
prm.add_endRequest(function(sender){
    if (sender._postBackSettings.sourceElement.id == '<%=txtLicenseNum.ClientID %>'){
        $('#<%=txtLicenseNum.ClientID %>').focus();
    }
});

function PageLoaded(sender, args)
{
    /*
    Add the License Type as the CSS class to the top container.
    Allow agency to customize different look & feel of the same standard field/template field in case of different contact type.
    */
    var topContainer = $get('<%=LicenseEditPanel.ClientID %>');
    var licenseType = $get('<%=ddlLicenseType.ClientID %>');
    if(topContainer && licenseType && licenseType.selectedIndex >= 0) {
        topContainer.className = GetValidCssClassName(licenseType[licenseType.selectedIndex].value);
    }

    //License form will be refreshed when license number has been changed, so we will add a title to describe what will happen before the change.
    if (!$.global.isAdmin && IsTrue('<%=txtLicenseNum.Visible %>') && IsTrue('<%=txtLicenseNum.AutoPostBack %>')) {
        UpdateTextboxToolTip('<%=txtLicenseNum.ClientID %>', '<%=GetTextByKey("LicenseEdit_licenseSearch_label_stateLicenseNum") %>');
    }
}

function MultiLicenseButtonClientClick()
{
    if(typeof(SetNotAsk) != 'undefined')
    {
        SetNotAsk();
    }
    SetCurrentValidationSectionID('<%=ClientID %>'); 
}
  
function AfterLicenseButtonClick() 
{
    SetNotAskForSPEAR();
    ResetIsShowLicenseConditionFlg();
}
</script>
