<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="RefContactEducationExamLookUp.aspx.cs" Inherits="Accela.ACA.Web.LicenseCertification.RefContactEducationExamLookUp" %>

<%@ Register Src="~/Component/RefContactEducationList.ascx" TagName="RefContactEducationList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefContactExaminationList.ascx" TagName="RefContactExaminationList" TagPrefix="ACA" %>
<%@ Register Src="~/Component/RefContactContinuingEducationList.ascx" TagName="RefContactContinuingEducationList" TagPrefix="ACA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPopup" runat="server">
    <asp:UpdatePanel ID="selctFromContactPanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="divEduExamLookUp" class="Body EduExamlookUp">
                <div id="divLookUpByContact" class="ACA_TabRow" runat="server" Visible="False">
                    <div id="divContactInfo" runat="server">
                        <ACA:AccelaLabel ID="lblContactName" runat="server" CssClass="ACA_Label font12px"></ACA:AccelaLabel>
                        <br/>
                        <ACA:AccelaLabel ID="lblContactType" runat="server" CssClass="font11px"></ACA:AccelaLabel>
                    </div>
                </div>
                <div id="divLookUpByContacts" class="ACA_Row ACA_LiLeft" runat="server" Visible="False">
                    <ul>
                        <li>
                            <ACA:AccelaLabel ID="lblLookUpFrom" AssociatedControlID="ddlContactList" LabelKey="aca_education_exam_list_label_look_up_from" runat="server" CssClass="ACA_Label font12px"/>
                        </li>
                        <li>
                            <ACA:AccelaDropDownList ID="ddlContactList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ContactListDropDown_SelectedIndexChanged" 
                                 ToolTipLabelKey="aca_common_msg_dropdown_changecontact_tip"/>
                        </li>
                    </ul>
                </div>
                <div id="divEduExamList" class="SelectCertifcationList">
                    <div id="divEduList">
                        <ACA:AccelaLabel ID="lblEduTitle" LabelKey="aca_selectfromcontact_educationlist_label_sectionname" runat="server" LabelType="SectionTitle" />
                        <ACA:RefContactEducationList ID="refContactEducationList" runat="server"></ACA:RefContactEducationList>
                    </div>
                    <div id="divExamList" class="Section">
                        <ACA:AccelaLabel ID="lblExamTitle" LabelKey="aca_selectfromcontact_examinationlist_label_sectionname" runat="server" LabelType="SectionTitle" />
                        <ACA:RefContactExaminationList ID="refContactExaminationList" runat="server"></ACA:RefContactExaminationList>
                    </div>
                    <div id="divContEduList" class="Section">
                        <ACA:AccelaLabel ID="lblContEduTitle" LabelKey="aca_selectfromcontact_conteducationlist_label_sectionname" runat="server" LabelType="SectionTitle" />
                        <ACA:RefContactContinuingEducationList ID="refContactContEducationList" runat="server"></ACA:RefContactContinuingEducationList>
                    </div>
                </div>
            </div>
            <div class="ACA_Row ACA_LiLeft Footer">
                <ul>
                    <li id="liSelectBtn" runat="server" Visible="False">
                        <ACA:AccelaButton ID="btnSelect" LabelKey="aca_license_certification_label_select" OnClick="SaveButton_Click" runat="server" 
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"/>
                    </li>
                    <li id="liSaveAndCloseBtn" runat="server" Visible="False">
                        <ACA:AccelaButton ID="btnSaveAndClose" LabelKey="aca_license_certification_label_saveandclose" OnClick="SaveButton_Click" runat="server"
                            DivEnableCss="ACA_LgButton ACA_LgButton_FontSize" DivDisableCss="ACA_LgButtonDisable ACA_LgButtonDisable_FontSize"/>
                    </li>
                    <li id="liCancelBtn" runat="server">
                        <ACA:AccelaLinkButton ID="btnCancel" CssClass="NotShowLoading ACA_LinkButton" LabelKey="aca_license_certification_label_cancel"
                            CausesValidation="false" OnClientClick="parent.ACADialog.close(); return false;" runat="server">
                        </ACA:AccelaLinkButton>
                    </li>
                </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script type="text/javascript">
        var hdnRequiredMapping = [];
        var requiredMapping = [];
        var eduRowIndex = 0;
        var examRowIndex = 1;
        var contEduRowIndex = 2;
        var isExistCheckedItem = false;

        with (Sys.WebForms.PageRequestManager.getInstance()) {
            add_beginRequest(function () {
                ShowLoading();
            });

            add_endRequest(function () {
                HideLoading();
            });

            add_pageLoaded(function () {
                isExistCheckedItem = false;
                
                InitSelectedCheckBox(eduRowIndex, '<%=refContactEducationList.ClientID %>', 'divEduList', 'hfEduNbr');
                InitSelectedCheckBox(examRowIndex, '<%=refContactExaminationList.ClientID %>', 'divExamList', 'hfExamNbr');
                InitSelectedCheckBox(contEduRowIndex, '<%=refContactContEducationList.ClientID %>', 'divContEduList', 'hfContEduNbr');
                
                if(!$.global.isAdmin) {
                    SetWizardButtonDisable("<%= btnSelect.ClientID %>", !isExistCheckedItem);
                }
            });
        }

        function InitSelectedCheckBox(rowIndex, userCtrlId, parentDivId, hdnNbrId) {
            hdnRequiredMapping[rowIndex] = $('#' + userCtrlId + '_hdnRequiredFlags');
            requiredMapping[rowIndex] = eval('(' + hdnRequiredMapping[rowIndex].val() + ')');
            
            if (requiredMapping[rowIndex]) {
                var arrNbr = $("#" + parentDivId).find("input:hidden[name$='" + hdnNbrId + "']");
                var chkSelects = $("#" + parentDivId).find("input:checkbox[name*='$CB_']");
                chkSelects.removeAttr("checked");

                for (var i = 0; i < arrNbr.length; i++) {
                    var nbr = $(arrNbr[i]).val();

                    if (requiredMapping[rowIndex][nbr] == 'Y') {
                        isExistCheckedItem = true;
                        $(chkSelects[i]).trigger('click', true);
                    }
                }
            }

            var chks = $("#" + parentDivId).find("input:checkbox[name*='$CB_']");

            chks.bind('click', function (e, isChecked) {
                var nbr = $(this).parents("tr:eq(0)").find("input[id$='" + hdnNbrId + "']").val();
                requiredMapping[rowIndex][nbr] = isChecked != undefined && isChecked || this.checked ? 'Y' : 'N';
                hdnRequiredMapping[rowIndex].val(JSON.stringify(requiredMapping[rowIndex]));
                
                var selected = IsAnyCheckBoxSelected();
                SetWizardButtonDisable("<%= btnSelect.ClientID %>", !selected);
            });
        }

        function IsAnyCheckBoxSelected() {
            for(var ctlIndx = 0; ctlIndx < 3; ctlIndx++) {
                for (var nbr in requiredMapping[ctlIndx]) {
                    if (requiredMapping[ctlIndx][nbr] == 'Y') {
                        return true;
                    }
                }
            }
            return false;
        }

        var originCheckAll = CheckAll;

        CheckAll = function(chk, hf) {
            var rowIndex = 0;

            if (chk.id.indexOf('refContactEducationList') != -1) {
                rowIndex = eduRowIndex;
            } else if (chk.id.indexOf('refContactExaminationList') != -1) {
                rowIndex = examRowIndex;
            } else {
                rowIndex = contEduRowIndex;
            }

            for (var nbr in requiredMapping[rowIndex]) {
                requiredMapping[rowIndex][nbr] = chk.checked ? 'Y' : 'N';
            }

            hdnRequiredMapping[rowIndex].val(JSON.stringify(requiredMapping[rowIndex]));

            originCheckAll(chk, hf);
            var selected = IsAnyCheckBoxSelected();
            SetWizardButtonDisable("<%= btnSelect.ClientID %>", !selected);
        };

        function CloseLookUpDialog(selectedContactSeqNbr) {
            var parentFocusObj = parent.ACADialog.focusObject;
            parent.ACADialog.close();
            SetParentLastFocus(parentFocusObj);
            parent.RefreshEduExamList('<%=ComponentName %>', '<%=ComponentID %>', selectedContactSeqNbr);
        }
    </script>
</asp:Content>
