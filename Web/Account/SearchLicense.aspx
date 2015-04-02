<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Account.SearchLicense"
    Title="Account Add License" ValidateRequest="false" CodeBehind="SearchLicense.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <div class="ACA_RightContent" id="divSearchLicense">
        <h1>
            <ACA:AccelaLabel ID="lblAddLicenseTitle" runat="server" LabelKey="acc_reg_label_title" />
            <br />
            <ACA:AccelaLabel ID="lblSearchLicenseExplain" LabelKey="acc_reg_label_searchExplain"
                runat="server" />
            <br />
            <br />
        </h1>
        <ACA:AccelaLabel ID="lblSearchLicenseDisclaimer" LabelKey="acc_reg_text_license"
            runat="server" LabelType="bodyText" />
        <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" runat="server" Height="10" />
        <div class="ACA_TabRow">
            <p class="ACA_FRight">
                <span class="ACA_Required_Indicator">*</span>
                <ACA:AccelaLabel ID="lblIndicate" LabelKey="acc_reg_label_indicate" runat="server" /></p>
        </div>
        <ACA:AccelaLabel ID="lblSearchLicenseTitle" LabelKey="lic_licenseSearch_label_licenseInfo"
            runat="server" LabelType="SectionTitle" />
        <div class="ACA_TabRow ACA_LiLeft">
            <ul>
                <li>
                    <ACA:AccelaDropDownList ID="ddlLicenseType" LabelKey="lic_licenseSearch_label_licenseType"
                        runat="server" Required="true" />
                </li>
                <li>
                    <ACA:AccelaTextBox ID="txtLicenseNum" CssClass="ACA_NLonger" runat="server" LabelKey="lic_licenseSearch_label_stateLicenseNum"
                        MaxLength="30" Validate="required;maxlength" /></li>
            </ul>
        </div>
        <ACA:AccelaHeightSeparate ID="sepForButton" runat="server" Height="25" />
        <ACA:AccelaButton ID="lbkFindLicense" DivEnableCss="ACA_LgButton ACA_LgButton_FontSize"
            LabelKey="acc_reg_label_findLicense" runat="server" OnClick="FindLicenseButton_Click" />
    </div>
</asp:Content>
