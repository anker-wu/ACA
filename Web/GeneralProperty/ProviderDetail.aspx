<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.GeneralProperty.ProviderDetail" Codebehind="ProviderDetail.aspx.cs" %>

<%@ Register Src="../Component/RefProviderDetail.ascx" TagName="RefProviderDetail"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefEducationList.ascx" TagName="RefEducationList"
    TagPrefix="uc2" %>
<%@ Register Src="../Component/RefContinuingEducationList.ascx" TagName="RefContinuingEducationList"
    TagPrefix="uc3" %>
<%@ Register Src="../Component/RefExaminationList.ascx" TagName="RefExaminationList"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script language="javascript" type="text/javascript">
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         prm.add_endRequest(EndRequest);

         function EndRequest(sender, args){           
             //export file.
             ExportCSV(sender, args);
         }
    </script>

    <div id="MainContent" class="ACA_Content">
        <iframe width="0" height="0" id="iframeExport" class="ACA_Hide" title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>"><%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
         <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_provider_detail" LabelType="PageInstruction"
                runat="server" />
        <h1>
            <ACA:AccelaLabel ID="lblTitalDisplay" runat="server" LabelKey="per_providerdetail_label_titaldisplay" />
            <br />
            <ACA:AccelaLabel ID="lblPropertyInfo" runat="server" />
        </h1>
        <br />
        <ACA:AccelaLabel ID="lblProviderDetailTital" runat="server" LabelKey="per_providerdetail_label_detailtital" LabelType="SectionTitle" />
        <uc1:RefProviderDetail ID="refProviderDetail" runat="server" />
        <ACA:AccelaLabel ID="lblEducationList" runat="server" LabelKey="per_providerdetail_label_listtital" LabelType="SectionTitle" />
        <uc2:RefEducationList ID="refEducationList" runat="server" />
        <ACA:AccelaLabel ID="lblContinuingEducationTitle" runat="server" LabelKey="per_providerdetail_continuing_education_title"
                LabelType="SectionTitle" />
        <uc3:RefContinuingEducationList ID="refContinuingEducationList" runat="server" />
        <ACA:AccelaLabel ID="lblExaminationTitle" runat="server" LabelKey="per_providerdetail_examination_title" LabelType="SectionTitle" />
        <uc4:RefExaminationList ID="refExaminationList" runat="server" />
    </div>
</asp:Content>
