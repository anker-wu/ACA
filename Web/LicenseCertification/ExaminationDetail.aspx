<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExaminationDetail.aspx.cs"
    Inherits="Accela.ACA.Web.LicenseCertification.ExaminationDetail" MasterPageFile="~/Dialog.master" %>
    
<%@ Register Src="~/Component/ExaminationDetail.ascx" TagPrefix="ACA" TagName="ExaminationDetail" %>
<%@ Register Src="~/Component/RefContactExaminationDetail.ascx" TagPrefix="ACA" TagName="RefContactExaminationDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <div class="ACA_TabRow">
        <ACA:ExaminationDetail runat="server" ID="examDetail" Visible="False" />
        <ACA:RefContactExaminationDetail runat="server" ID="refContactExamDetail" Visible="False" />
    </div>
</asp:Content>
