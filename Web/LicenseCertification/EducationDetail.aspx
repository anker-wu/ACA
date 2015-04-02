<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EducationDetail.aspx.cs"
    MasterPageFile="~/Dialog.master" Inherits="Accela.ACA.Web.LicenseCertification.EducationDetail" %>

<%@ Register Src="~/Component/EducationDetail.ascx" TagPrefix="ACA" TagName="EducationDetail" %>
<%@ Register Src="~/Component/RefContactEducationDetail.ascx" TagPrefix="ACA" TagName="RefContactEducationDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <div class="ACA_TabRow">
        <ACA:EducationDetail runat="server" ID="educationDetail" Visible="False" />
        <ACA:RefContactEducationDetail runat="server" ID="refContactEducationDetail" Visible="False" />
    </div>
</asp:Content>
