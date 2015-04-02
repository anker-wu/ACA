<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="UserLicenseDetail.aspx.cs" Inherits="Accela.ACA.Web.Account.UserLicenseDetail" %>

<asp:Content ID="licenseDetailContent" ContentPlaceHolderID="phPopup" runat="server">
    <div class="userlicensedetailpage">
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblLicenseType" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_licensetype" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblContactName" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_contactname" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblStateLicense" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_statelicense" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblBusinessName" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_businessname" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblLicenseIssueDate" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_issued" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblBusinessName2" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_businessname2" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblLicenseExpirationDate" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_expires" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblBusinessLicense" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_businesslicensenumber" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblInsuredMax" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_insuredMax" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblAddress1" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_address1" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblContractorLicNO" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userlicenseview_label_contractorlicno" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblAddress2" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_address2" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblContractorBusiName" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userlicenseview_label_contractorbusiname" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblAddress3" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_address3" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblStatus" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_status" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblCity" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_city" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblState" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_state" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblHomePhone" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_home" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FRight ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblZip" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_zip" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblMobilePhone" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_mobile" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Row">
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblFax" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="acc_userLicenseView_label_fax" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="detailfield ACA_FLeft ACA_HalfWidthTable">
            <ACA:AccelaNameValueLabel ID="lblCountry" CssClass="fieldlabel" LayoutType="Horizontal"
                LabelKey="aca_userlicensedetail_label_country" runat="server"></ACA:AccelaNameValueLabel>
        </div>
        <div class="ACA_Height20"></div>
    </div>
</asp:Content>