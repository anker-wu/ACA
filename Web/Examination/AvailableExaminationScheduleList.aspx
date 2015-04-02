<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AvailableExaminationScheduleList.aspx.cs"
       MasterPageFile="~/Dialog.master"  Inherits="Accela.ACA.Web.Examination.AvailableExaminationScheduleList" %>
<%@ Register Src="~/Component/RefAvailableExaminationScheduleList.ascx" TagName="RefAvailableExaminationScheduleList" TagPrefix="uc1" %>
<%@ Register Src="~/Component/RefAvailableExaminationScheduleSearchForm.ascx" TagName="RefAvailableExaminationScheduleSearchForm" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="Server">
    <%
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "No-Cache");
    %>
<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/textCollapse.js")%>"></script>
<asp:UpdatePanel ID="AvailableExaminationPanel" runat="server" UpdateMode="conditional">        
    <ContentTemplate>
    <div class="available_examination_schedule_list">
        <div class="font11px ACA_TabRow ACA_Popup_Title">
            <ACA:AccelaLabel ID="lblScheduleExamination" LabelKey="aca_exam_schedule_availablelist_options" runat="server"></ACA:AccelaLabel>
        </div>
        <uc1:RefAvailableExaminationScheduleSearchForm GridViewId="60131" IsInPopup="true" ID="RefAvailableExaminationScheduleSearchForm" runat="server" />
        <div class="ACA_TabRow ACA_LgButton ACA_LgButton_FontSize font11px">
            <ACA:AccelaLinkButton OnClientClick="ValidateDate();return false" OnClick="SearchButton_Click" runat="server" ID="btnSearch" LabelKey="aca_exam_schedule_availablelist_filter" ></ACA:AccelaLinkButton>
        </div>
        <div class="ACA_TabRow_NoScoll ACA_WrodWrap" id="dvSearchList">
            <uc1:RefAvailableExaminationScheduleList IsForSeach="false" GridViewId="60131" ID="RefAvailableExaminationScheduleList" runat="server" />
        </div>
         <!-- button list -->
        <div class="buttons ACA_Row ACA_LiLeft font11px">
            <ul>
                <li>
                    <div class="ACA_LgButton ACA_LgButton_FontSize">
                        <ACA:AccelaButton ID="lnkContinue" OnClick="ContinueButton_Click" LabelKey="aca_exam_schedule_action_continue" runat="server"/>
                    </div>
                </li>
                <li>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkBack" CausesValidation="false" OnClick="BackButton_Click" LabelKey="aca_exam_schedule_action_back" runat="server" />
                    </div>
                </li>
                <li>
                    <div class="ACA_LinkButton">
                        <ACA:AccelaLinkButton ID="lnkCancel" OnClientClick="parent.ACADialog.close(); return false;" LabelKey="aca_exam_schedule_action_cancel" runat="server" />
                    </div>
                </li>
            </ul> 
        </div>
    </div>
    </ContentTemplate>
 </asp:UpdatePanel>
<script type="text/javascript">
    $("#lnkBeginFocus", getParentDocument()).focus();

    function SetButtonStatus() {
        SetWizardButtonDisable('<%=lnkContinue.ClientID%>', false);
    }

    function SetSelectProviderNbr(id,value){
        $("#"+id).val(value);
    }

    function ValidateDate() {
        var module = '<%=ModuleName %>';
        var startDate = $("#<%=RefAvailableExaminationScheduleSearchForm.ClientID %>_dateFromTime").val();
        var endDate = $("#<%=RefAvailableExaminationScheduleSearchForm.ClientID %>_dateToTime").val();
        PageMethods.ValidateScheduleDate(startDate,endDate,module,ValidateDateCallBack);
    }

    function ValidateDateCallBack(errormsg) {
        if (errormsg) {
            showMessage("messageSpan", errormsg, "Error", true, 1, true);
        } else {
            WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('<%=btnSearch.UniqueID %>', "", true, "", "", false, true));
            var p = new ProcessLoading();
            p.showLoading(true);
            if (typeof(myValidationErrorPanel)!='undefined') myValidationErrorPanel.printErrors();
        }
    }
</script>
</asp:Content>
