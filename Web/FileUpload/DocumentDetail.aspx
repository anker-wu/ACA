<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="DocumentDetail.aspx.cs" Inherits="Accela.ACA.Web.Attachment.DocumentDetail" %>
<%@ Register Src="~/Component/GenericTemplateView.ascx" TagName="GenericTemplate" TagPrefix="ACA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
<script type="text/javascript" src="../Scripts/textCollapse.js"></script>
    <div class="docdetailpage">
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblFileName" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_filename" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblFileSize" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_filesize" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row"></div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
                    <ACA:AccelaNameValueLabel ID="lblEntity" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_entity" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblEntityType" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_entitytype" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row"></div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblDocumentStatus" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_documentstatus" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblStatusDate" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_statusdate" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row"></div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblRecordNumber" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_recordnumber" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblRecordType" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_recordtype" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row"></div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblUploadDate" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_uploaddate" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblLatestUpdateDate" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_latestupdatedate" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row"></div>
        <div class="detailfield ACA_FLeft ACA_FullWidthTable">
             <ACA:AccelaNameValueLabel ID="lblDocType" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_documentdetail_label_documenttype" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row"></div>
        <div class="detailfield ACA_FLeft ACA_FullWidthTable">
            <ACA:AccelaNameValueLabel ID="lblVirtualFolders" CssClass="fieldlabel" LayoutType="Vertical"
                LabelKey="aca_documentdetail_label_virtualfolders" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_FullWidthTable">
            <ACA:AccelaNameValueLabel ID="lblDescription" CssClass="fieldlabel" LayoutType="Vertical"
                LabelKey="aca_documentdetail_label_description" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <ACA:GenericTemplate ID="genericTemplate" runat="server"/>
    </div>
    <script type="text/javascript">
        //Word break for file name label.
        $('p.break-word:has(#<%=lblFileName.ValueLabelClientID %>)').each(function () { breakWord(this); });
    </script>
</asp:Content>