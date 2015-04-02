<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContinuingEducationDetail.aspx.cs"
    MasterPageFile="~/Dialog.master" Inherits="Accela.ACA.Web.LicenseCertification.ContinuingEducationDetail" %>
    
<%@ Register Src="~/Component/ContinuingEducationDetail.ascx" TagPrefix="ACA" TagName="ContinuingEducationDetail" %>
<%@ Register Src="~/Component/RefContactContEducationDetail.ascx" TagPrefix="ACA" TagName="RefContactContEducationDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <div class="ACA_TabRow">
        <ACA:ContinuingEducationDetail runat="server" ID="continuingEducationDetail" Visible="False" />
        <ACA:RefContactContEducationDetail runat="server" ID="refContactContEducationDetail1" Visible="False" />
    </div>
</asp:Content>
