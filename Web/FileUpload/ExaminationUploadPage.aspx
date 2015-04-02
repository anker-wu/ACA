<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" Inherits="Accela.ACA.Web.Attachment.ExaminationUploadPage" Codebehind="ExaminationUploadPage.aspx.cs" %>
<%@ Register Src="~/Component/UploadScoreForm.ascx" TagName="UploadScoreForm" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">
    <script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/FileUpload.js")%>"></script>
    <div class="ACA_RightItem">
        <div class="ACA_TabRow">
             <ACA:AccelaLabel ID="lblExaminationTitle" LabelKey="examination_upload_document" CssClass="ACA_Title" runat="server" LabelType="SubSectionText" />
        </div>
        <div class="ACA_TabRow">
         <ACA:UploadScoreForm ID="uploadScoreForm" runat="server" />
        </div>
    </div>
</asp:Content>
