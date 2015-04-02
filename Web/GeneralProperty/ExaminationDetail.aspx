<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Default.master"
    Inherits="Accela.ACA.Web.GeneralProperty.ExaminationDetail" Codebehind="ExaminationDetail.aspx.cs" %>

<%@ Register Src="../Component/RefExaminationDetail.ascx" TagName="RefExaminationDetail"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/RefProviderList.ascx" TagName="RefProviderList" TagPrefix="uc2" %>
<%@ Register Src="../Component/RefAvailableExaminationScheduleSearchForm.ascx" TagName="RefAvailableExaminationScheduleSearchForm" TagPrefix="uc3" %>
<%@ Register Src="../Component/RefAvailableExaminationScheduleList.ascx" TagName="RefAvailableExaminationScheduleList" TagPrefix="uc4" %>

<asp:content id="Content1" contentplaceholderid="PlaceHolderMain" runat="Server">
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/textCollapse.js")%>"></script>
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
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_page_instruction_examination_detail" LabelType="PageInstruction"
                runat="server" />
        <h1>
            <ACA:AccelaLabel ID="lblTitle" runat="server" LabelKey="per_examination_label_title"/>
            <br />
            <ACA:AccelaLabel ID="lblPropertyInfo" runat="server"/>
        </h1>
        <br />
        <ACA:AccelaLabel ID="lblExaminationDetailTitle" runat="server" LabelKey="per_examination_section_title" LabelType="SectionTitle"/>
        <uc1:RefExaminationDetail ID="refExaminationDetail" runat="server" />
        <ACA:AccelaLabel ID="lblProviderList" runat="server" LabelKey="per_examination_label_listtitle" LabelType="SectionTitle"/>
        <uc2:RefProviderList ID="refProviderList" runat="server" />

        <ACA:AccelaLabel ID="lblAvailableScheduleTitle" runat="server" LabelKey="aca_exam_detail_section_title" LabelType="SectionTitle"/>
        <uc3:RefAvailableExaminationScheduleSearchForm GridViewId="60132" ID="RefAvailableExaminationScheduleSearchForm" runat="server" />
        <div class="ACA_TabRow ACA_LgButton ACA_LgButton_FontSize">
            <ACA:AccelaLinkButton  OnClientClick="ValidateDate();return false" OnClick="SearchButton_Click" runat="server" ID="btnSearch" LabelKey="aca_exam_detail_filter" ></ACA:AccelaLinkButton>
        </div>
        <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="dvSearchList">
            <uc4:RefAvailableExaminationScheduleList GridViewId="60132" IsForSeach="true" ID="RefAvailableExaminationScheduleList" runat="server" />
        </div>

    </div>
    <script type="text/javascript">
        function ValidateDate() {
            var module = '<%=ModuleName %>';
            var startDate = $("#<%=RefAvailableExaminationScheduleSearchForm.ClientID %>_dateFromTime").val();
            var endDate = $("#<%=RefAvailableExaminationScheduleSearchForm.ClientID %>_dateToTime").val();
            PageMethods.ValidateScheduleDate(startDate, endDate, module, ValidateDateCallBack);
        }

        function ValidateDateCallBack(errormsg) {
            if (errormsg) {
                showMessage("messageSpan", errormsg, "Error", true, 1, false);
            } else {
                WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('<%=btnSearch.UniqueID %>', "", true, "", "", false, true));
                var p = new ProcessLoading();
                p.showLoading(true);
                if (typeof (myValidationErrorPanel) != 'undefined') myValidationErrorPanel.printErrors();
            }
        }
    </script>
    <noscript><%=LabelUtil.GetGlobalTextByKey("aca_message_noscript")%></noscript>
</asp:content>