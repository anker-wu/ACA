<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master"
    Inherits="Accela.ACA.Web.GeneralProperty.ContinuingEducationDetail" Codebehind="ContinuingEducationDetail.aspx.cs" %>

<%@ Register Src="../Component/RefContinuingEducationDetail.ascx" TagName="RefContinuingEducationDetail"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefProviderList.ascx" TagName="RefProviderList" TagPrefix="uc2" %>
<asp:content id="Content1" contentplaceholderid="PlaceHolderMain" runat="Server">
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
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_continuingeducation_detail" LabelType="PageInstruction"
                runat="server" />
        <h1>
            <ACA:AccelaLabel ID="lblTitle" runat="server" LabelKey="per_continuingeducationdetail_label_title"/>
            <br />
            <ACA:AccelaLabel ID="lblPropertyInfo" runat="server"/>
        </h1>
        <br />
        <ACA:AccelaLabel ID="lblContinuingEducationDetailTitle" runat="server" LabelKey="per_continuingeducationdetail_section_title" LabelType="SectionTitle"/>
        <uc1:RefContinuingEducationDetail ID="refContinuingEducationDetail" runat="server" />
        <ACA:AccelaLabel ID="lblProviderList" runat="server" LabelKey="per_continuingeducationdetail_label_listtital" LabelType="SectionTitle"/>
        <uc2:RefProviderList ID="refProviderList" runat="server" />
    </div>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</asp:content>
