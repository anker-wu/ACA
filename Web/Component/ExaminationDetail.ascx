<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExaminationDetail.ascx.cs" Inherits="Accela.ACA.Web.Component.ExaminationDetail" %>

<%@ Register TagPrefix="ACA" TagName="GenericTemplate" Src="~/Component/GenericTemplateView.ascx" %>

<div class="edudetailpage">
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblName" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_name" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblProviderName" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_providername" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblExaminationDate" DateType="LongDate" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_date" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblProviderNumber" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_providernumber" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblStartTime" DateType="OnlyTime" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_starttime" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblAddressLines" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_address" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblEndTime" DateType="OnlyTime" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_endtime" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblCity" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_city" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblRosterID" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_rosterid" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblState" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_state" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblRequired" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_required" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblCountry" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_country" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblApproved" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_examinationdetail_label_approved" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblZip" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_zip" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblFinalScore" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="examination_detail_final_score" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPhone1" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_phone1" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblComments" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_comments" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblPhone2" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_phone2" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblFax" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_fax" runat="server"></ACA:AccelaNameValueLabel>
    </div>
    <div class="ACA_Row"></div>
    <div class="detailfield ACA_FRight ACA_HalfWidthTable"></div>
    <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
        <ACA:AccelaNameValueLabel ID="lblEmail" CssClass="fieldlabel" LayoutType="Horizontal"
            LabelKey="aca_exam_pcompleted_label_email" runat="server"></ACA:AccelaNameValueLabel>
    </div>

    <ACA:GenericTemplate ID="genericTemplate" runat="server" />
</div>
<script type="text/javascript">
    //Word break for file name label.
    $('p.break-word:has(#<%=lblName.ValueLabelClientID %>)').each(function () { breakWord(this); });
</script>