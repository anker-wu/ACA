<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.EducationRelationSearchForm" Codebehind="EducationRelationSearchForm.ascx.cs" %>

<ACA:AccelaFormDesignerPlaceHolder ID="phContent" runat="server">
    <ACA:AccelaDropDownList ID="lblProviderType" runat="server" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_providertype" />
    <ACA:AccelaTextBox ID="txtLicenseNbr" runat="server" MaxLength="30" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_licensenumber" />

    <ACA:AccelaDropDownList ID="ddlModuleSearchEdu" runat="server" LabelKey="educationrelationsearchform_modulename"  OnSelectedIndexChanged="Module_IndexChanged" AutoPostBack="true"></ACA:AccelaDropDownList>
                                    
    <ACA:AccelaDropDownList ID="ddlRecordTypeSearchEdu" runat="server" LabelKey="educationrelationsearchform_recordtypename"></ACA:AccelaDropDownList>

    <ACA:AccelaTextBox ID="txtProviderName" runat="server" MaxLength="30" CssClass="ACA_XLong" LabelKey="educationrelationsearchform_providername" />
    <ACA:AccelaTextBox ID="txtProviderNumber" runat="server" MaxLength="30" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_providernumber" />
    <ACA:AccelaTextBox ID="txtAddress1" runat="server" MaxLength="30" CssClass="ACA_XLong" LabelKey="educationrelationsearchform_address1" />
    <ACA:AccelaTextBox ID="txtAddress2" runat="server" MaxLength="30" CssClass="ACA_XLong" LabelKey="educationrelationsearchform_address2" />
    <ACA:AccelaTextBox ID="txtAddress3" runat="server" MaxLength="30" CssClass="ACA_XLong" LabelKey="educationrelationsearchform_address3" />
    <ACA:AccelaCountryDropDownList ID="ddlCountry" runat="server" LabelKey="aca_educationrelationsearchform_label_country"></ACA:AccelaCountryDropDownList>
    <ACA:AccelaTextBox ID="txtCity" PositionID="LookUpEducationCity" AutoFillType="City" runat="server" MaxLength="30" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_city" />
    <ACA:AccelaStateControl ID="txtState" PositionID="LookUpEducationState" AutoFillType="State" LabelKey="educationrelationsearchform_state" runat="server" CssClass="ACA_NLong" />
    <ACA:AccelaZipText ID="txtZip" runat="server" IsIgnoreValidate="true" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_zip" />
    <ACA:AccelaPhoneText ID="txtPhone1" runat="server" IsIgnoreValidate="true" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_phone1" />
    <ACA:AccelaPhoneText ID="txtPhone2" runat="server" IsIgnoreValidate="true" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_phone2" />
    <ACA:AccelaPhoneText ID="txtFax" runat="server" IsIgnoreValidate="true" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_fax" />
    <div id="divHrforProvider" class="ACA_TabRow ACA_Line_Content" runat="server">&nbsp;</div>
    <ACA:AccelaTextBox ID="txtName" runat="server" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_name" />
    <ACA:AccelaTextBox ID="txtDegree" runat="server" MaxLength="30" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_degree" />
    <div id="divHrforDegree" class="ACA_TabRow ACA_Line_Content" runat="server">&nbsp;</div>
    <ACA:AccelaTextBox ID="txtCourseName" runat="server" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_course_name" />
    <ACA:AccelaNumberText ID="txtTotalHours" runat="server" MaxLength="8" DecimalDigitsLength="2" MinimumValue="0" MaximumValue="99999" Validate="MinimumValue;MaximumValue;DecimalDigitsLength" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_total_hours" />
    <div id="divHrForContEdu" class="ACA_TabRow ACA_Line_Content" runat="server">&nbsp;</div>
    <ACA:AccelaTextBox ID="txtExaminationName" runat="server" CssClass="ACA_NLong" LabelKey="educationrelationsearchform_exam_name" />
</ACA:AccelaFormDesignerPlaceHolder>