<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefContactExaminationDetail.ascx.cs" Inherits="Accela.ACA.Web.Component.RefContactExaminationDetail" %>

<%@ Register TagPrefix="ACA" TagName="GenericTemplate" Src="~/Component/GenericTemplateView.ascx" %>

<div class="edudetailpage">
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblName" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_name" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblProviderName" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_provider_name" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblFinalScore" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_final_score" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblProviderNumber" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_provider_number" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPassScore" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_passingscore" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblAddressLines" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examinationdetail_label_address" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblExaminationDate" DateType="LongDate" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_examination_date" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblCity" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_city" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
         <ACA:AccelaNameValueLabel ID="lblStartTime" DateType="OnlyTime" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_start_time" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblState" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_state" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblEndTime" DateType="OnlyTime" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_end_time" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblCountry" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_country" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
         <ACA:AccelaNameValueLabel ID="lblApproved" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examinationdetail_label_approved" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblZip" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_zip" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblComments" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_comments" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPhone1" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_phone_number1" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPhone2" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_phone_number2" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblFax" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_fax" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblEmail" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_contact_examination_detail_label_email" runat="server"></ACA:AccelaNameValueLabel>
    </div>

    <ACA:GenericTemplate ID="genericTemplate" runat="server" />
</div>
<script type="text/javascript">
    //Word break for file name label.
    $('p.break-word:has(#<%=lblName.ValueLabelClientID %>)').each(function () { breakWord(this); });
</script>