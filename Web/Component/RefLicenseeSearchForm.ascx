<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefLicenseeSearchForm" Codebehind="RefLicenseeSearchForm.ascx.cs" %>

<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaDropDownList ID="ddlLicenseType" runat="server" LabelKey="aca_licensee_licenseType"></ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtLicenseNumber" runat="server" LabelKey="aca_licensee_stateLicenseNum" CssClass="ACA_NLonger" MaxLength="30"></ACA:AccelaTextBox>
    <ACA:AccelaStateControl ID="txtLicenseState" runat="server" LabelKey="aca_licensee_licensestate"></ACA:AccelaStateControl>
    <ACA:AccelaDropDownList ID="ddlContactType" runat="server" LabelKey="aca_licensee_contacttype"></ACA:AccelaDropDownList>
    <ACA:AccelaSSNText ID="txtSSN" MaxLength="11" LabelKey="aca_licensee_ssn" runat="server" IsIgnoreValidate="true"></ACA:AccelaSSNText>
    <ACA:AccelaFeinText ID="txtFEIN" MaxLength="11" LabelKey="aca_licensee_fein" IsIgnoreValidate="true" runat="server"></ACA:AccelaFeinText>
    <ACA:AccelaTextBox ID="txtProviderName" runat="server" LabelKey="aca_licensee_providername" MaxLength="30" CssClass="ACA_XLong"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtProviderNumber" runat="server" LabelKey="aca_licensee_providernumber" MaxLength="30" CssClass="ACA_NLong"></ACA:AccelaTextBox>
    <ACA:AccelaDropDownList ID="ddlLicensingBoard" runat="server" LabelKey="aca_licensee_licensingboard"></ACA:AccelaDropDownList>
    <ACA:AccelaTextBox ID="txtFirstName" runat="server" LabelKey="aca_licensee_firstname" CssClass="ACA_NLong" MaxLength="70"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtMiddleInitial" runat="server" LabelKey="aca_licensee_middleinitial" CssClass="ACA_NShot" MaxLength="70"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtLastName" runat="server" LabelKey="aca_licensee_lastname" CssClass="ACA_NLong" MaxLength="70"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtBusiName" runat="server" LabelKey="aca_licensee_businessname" CssClass="ACA_XLong" MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtBusiName2" runat="server" LabelKey="aca_licensee_businessname2" CssClass="ACA_XLong" MaxLength="255"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtBusiLicense" runat="server" LabelKey="aca_licensee_businesslicense" CssClass="ACA_NLong" MaxLength="15"></ACA:AccelaTextBox>
    <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="aca_licensee_country"></ACA:AccelaCountryDropDownList>
    <ACA:AccelaTextBox ID="txtAddress1" MaxLength="200" runat="server" CssClass="ACA_MLonger" LabelKey="aca_licensee_address1"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtAddress2" MaxLength="200" runat="server" CssClass="ACA_MLonger" LabelKey="aca_licensee_address2"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtAddress3" MaxLength="200" runat="server" CssClass="ACA_MLonger" LabelKey="aca_licensee_address3"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtCity" MaxLength="30" runat="server" CssClass="ACA_NLong" LabelKey="aca_licensee_city" AutoFillType="City"></ACA:AccelaTextBox>
    <ACA:AccelaStateControl ID="txtState" runat="server" LabelKey="aca_licensee_state" AutoFillType="State"></ACA:AccelaStateControl>
    <ACA:AccelaZipText ID="txtZipCode" runat="server" IsIgnoreValidate="true" CssClass="ACA_Medium" LabelKey="aca_licensee_zip"></ACA:AccelaZipText>
    <ACA:AccelaPhoneText ID="txtPhone1" runat="server" LabelKey="aca_licensee_phone1" CssClass="ACA_NLong" IsIgnoreValidate="true"></ACA:AccelaPhoneText>
    <ACA:AccelaPhoneText ID="txtPhone2" runat="server" LabelKey="aca_licensee_phone2" CssClass="ACA_NLong" IsIgnoreValidate="true"></ACA:AccelaPhoneText>
    <ACA:AccelaPhoneText ID="txtFax" runat="server" LabelKey="aca_licensee_fax" CssClass="ACA_NLong" IsIgnoreValidate="true"></ACA:AccelaPhoneText>
    <ACA:AccelaNumberText ID="txtContractorLicNO" LabelKey="aca_licensee_contractorlicno" CssClass="ACA_Small" runat="server" MaxLength="8" IsNeedDot="false"></ACA:AccelaNumberText>
    <ACA:AccelaTextBox ID="txtContractorBusiName" runat="server" LabelKey="aca_licensee_contractorbusiname" CssClass="ACA_NLonger" MaxLength="65"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtTitle" MaxLength="255" runat="server" CssClass="ACA_MLonger" LabelKey="aca_licensee_title"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtInsurancePolicy" MaxLength="30" runat="server" CssClass="ACA_MLonger" LabelKey="aca_licensee_insurancepolicy"></ACA:AccelaTextBox>
    <ACA:AccelaTextBox ID="txtInsuranceCompany" MaxLength="65" runat="server" CssClass="ACA_MLonger" LabelKey="aca_licensee_insurancecompany"></ACA:AccelaTextBox>
</ACA:AccelaFormDesignerPlaceHolder>
