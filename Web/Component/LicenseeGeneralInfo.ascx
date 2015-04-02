<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="LicenseeGeneralInfo.ascx.cs" Inherits="Accela.ACA.Web.Component.LicenseeGeneralInfo" %>
<%@ Register Src="LicenseeGeneralInfoLPTemplate.ascx" TagName="LicenseeGeneralInfoLPTemplate" TagPrefix="uc1" %>
<%@ Register Src="LicenseeGeneralInfoLPPeopleInfoTable.ascx" TagName="LicenseeGeneralInfoLPPeopleInfoTable" TagPrefix="uc1" %>
<%@ Register Src="~/Component/GenericTemplateEdit.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/textCollapse.js")%>"></script>
<ACA:AccelaSectionTitleBar ID="sectionTitleBar" LabelKey="per_licensee_generalinfo_section_title" ShowType="OnlyAdmin" runat="server" />

<div class="ACA_TabRow ACA_SmLabel">
<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaNameValueLabel ID="lblLicenseeType" LabelKey="per_licenseedetail_label_type" class="ACA_RefEducation_Font" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeNumber" LabelKey="per_licenseedetail_label_number" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeState" LabelKey="per_licenseedetail_label_licensestate" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeBoard" LabelKey="per_licenseedetail_label_board" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeBusinessName" LabelKey="per_licenseedetail_label_businessname" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblBusinessNumber" LabelKey="per_licenseedetail_label_businessnumber" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblBusinessExpirationDate" DateType="ShortDate" LabelKey="per_licenseedetail_label_businessexpirationdate" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseIssueDate" DateType="ShortDate" LabelKey="per_licenseedetail_label_issuedate" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblExpirationDate"  DateType="ShortDate" LabelKey="per_licenseedetail_label_expirationdate" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblContactType" LabelKey="per_licenseedetail_contact_type" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblContactName" LabelKey="per_licenseedetail_label_contactname" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeTitle" LabelKey="per_licenseedetail_label_title" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeAddress" LabelKey="per_licenseedetail_label_address" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeTelephone1" LabelKey="per_licenseedetail_label_telephone1" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeTelephone2" LabelKey="per_licenseedetail_label_telephone2" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeFAX"  LabelKey="per_licenseedetail_label_fax" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblLicenseeEmail" LabelKey="per_licenseedetail_label_email" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblBusinessName2" LabelKey="per_licenseedetail_label_businessname2" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblInsuranceCompany" LabelKey="per_licenseedetail_label_insurancecompany" Font-Bold="true" runat="server" />
    <ACA:AccelaNameValueLabel ID="lblInsurancePolicy" LabelKey="per_licenseedetail_label_insurancepolicy" Font-Bold="true" runat="server" />
    <uc1:LicenseeGeneralInfoLPTemplate ID="licenseeGeneralInfoLPTemplate" NeedCollapse="true" runat="server" /> 
</ACA:AccelaFormDesignerPlaceHolder>
</div>
<uc1:LicenseeGeneralInfoLPPeopleInfoTable ID="licenseeGeneralInfoLPPeopleInfoTable" runat="server" />
    <script language="javascript" type="text/javascript">
        void function() {
            var elements = $("p").filter(".break-word");
            var element, i;
            for (i = 0; element = elements[i]; i++) {
                breakWord(element);
            }
        } ();
    </script>

