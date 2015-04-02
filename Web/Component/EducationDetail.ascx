<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EducationDetail.ascx.cs" Inherits="Accela.ACA.Web.Component.EducationDetail" %>
<%@ Register TagPrefix="ACA" TagName="GenericTemplate" Src="~/Component/GenericTemplateView.ascx" %>

<div class="edudetailpage">
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblName" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_majordiscipline" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblProviderName" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_providername" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblDegree" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_degree" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblProviderNumber" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_providernumber" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblYearJoined" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_yearjoined" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblAddressLines" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_address" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblYearGraduated" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_yeargraduated" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblCity" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_city" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblRequire" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_required" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblState" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_state" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblApproved" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_educationdetail_label_approved" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblCountry" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_view_label_country" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblComments" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_comments" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblZip" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_zip" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPhone1" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_phone1" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPhone2" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_phone2" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblFax" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_fax" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblEmail" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_education_label_email" runat="server"></ACA:AccelaNameValueLabel>
    </div>

    <ACA:GenericTemplate ID="genericTemplate" runat="server" />
</div>
<script type="text/javascript">
    //Word break for file name label.
    $('p.break-word:has(#<%=lblName.ValueLabelClientID %>)').each(function () { breakWord(this); });
</script>

