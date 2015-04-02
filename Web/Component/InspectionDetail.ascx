<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionDetail.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionDetail" %>
<%@ Register Src="~/Component/RelatedInspectionList.ascx" TagName="RelatedInspection"
    TagPrefix="uc1" %>
<%@ Register Src="~/Component/InspectionStatusHistoryList.ascx" TagName="StatusHistory"
    TagPrefix="uc2" %>
<%@ Register Src="~/Component/InspectionResultCommentList.ascx" TagName="ResultComment"
    TagPrefix="uc3" %>
<%@ Register Src="~/Component/AddressView.ascx" TagName="Address" TagPrefix="uc4" %>

<script type="text/javascript" src="<%=FileUtil.AppendApplicationRoot("Scripts/textCollapse.js")%>"></script>

<div class="inspection_detail">
    <table role='presentation'>
        <tr>
            <td colspan="3">
                <div id="divInspectionField" class="parameter_area" visible="false" runat="server">
                    <span>Click the area below to edit inspection name available variables according to format:</span><br />
                    <span><%= PARAMETER_INSPECTION_TYPE %>: The inspection type</span><br />
                    <span><%= PARAMETER_INSPECTION_ID %>: The inspection ID</span><br />
                    <span><%= PARAMETER_INSPECTION_REQUIRED_STATUS %>: The inspection required status</span>
                </div>
                <ACA:AccelaLabel ID="lblInspectionName" LabelKey="aca_inspectiondetail_label_inspectiontype_pattern" runat="server"></ACA:AccelaLabel>
            </td>
        </tr>
        <tr>
            <td class="">
                <div id="divInspectionSummary">
                    <div class="ACA_Section_Instruction ACA_Page_Instruction_FontSize">
                        <uc4:Address ID="AddressText" runat="server"></uc4:Address>
                    </div>
                </div>
            </td>
            <td class="ACA_XShot">
            </td>
            <td class="">
                <div id="divInspectionButton" class="InspectionSummary" runat="server">
                    <ACA:AccelaButton ID="btnRequest" runat="server" OnClientClick="parent.adjustPopupDialogWidthHeight4Scheduling();" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
                        LabelKey="ins_inspectionlist_label_requestinspection">
                    </ACA:AccelaButton>
                    <ACA:AccelaButton ID="btnSchedule" runat="server" OnClientClick="parent.adjustPopupDialogWidthHeight4Scheduling();" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
                        LabelKey="ins_inspectionList_label_scheduleInspection">
                    </ACA:AccelaButton>
                    <ACA:AccelaButton ID="btnReschedule" runat="server" OnClientClick="parent.adjustPopupDialogWidthHeight4Scheduling();" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
                        LabelKey="ins_inspectionList_label_reSchedule">
                    </ACA:AccelaButton>
                    <div runat="server" id="divMainActionSpacer" class="ACA_FLeft">
                        &nbsp;&nbsp;</div>
                    <ACA:AccelaButton ID="btnCancel" OnClientClick="parent.adjustPopupDialogWidthHeight4Scheduling();" runat="server" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize"
                        LabelKey="aca_inspection_action_cancelinspection">
                    </ACA:AccelaButton>
                    <ACA:AccelaButton ID="btnPrint" runat="server" class="NotShowLoading" DivEnableCss="ACA_SmButton ACA_SmButton_FontSize ACA_FRight ACA_SmButton_Print"
                        LabelKey="inspectiondetail_print" OnClientClick="print_onclick();return false;">
                    </ACA:AccelaButton>
                 </div>
                    <script type="text/javascript">
                        //to avoid postback
                        $("#<%=btnPrint.ClientID %>").attr("href", "javascript:void(0)");
                    </script>
            </td>
        </tr>
        <tr>
            <td width="40%">
                <ACA:AccelaLabel ID="lblStatusTitle" runat="server" LabelKey="ins_inspectionList_label_status"
                    LabelType="SectionTitle">
                </ACA:AccelaLabel>
                <div class="SectionBody font12px fontbold">
                    <div>
                        <ACA:AccelaLabel ID="lblStatus" runat="server">
                        </ACA:AccelaLabel>
                    </div>
                    <div>
                        <ACA:AccelaLabel ID="lblStatusDate" runat="server">
                        </ACA:AccelaLabel>
                    </div>
                    <div id="divEstimatedArrivalTime" class="SectionItem" runat="server">
                        <ACA:AccelaLabel ID="lblEstimatedArrivalTime" LabelKey="aca_inspectiondetail_estimated_arrivaltime" runat="server"></ACA:AccelaLabel>
                        <ACA:AccelaLabel ID="lblEstimatedArrivalTimeValue" runat="server"></ACA:AccelaLabel>
                    </div>
                    <div id="divDesiredDate" class="SectionItem" runat="server">
                        <ACA:AccelaLabel ID="lblDesiredDate" LabelKey="aca_inspectiondetail_label_desired_date" runat="server"></ACA:AccelaLabel>
                        <ACA:AccelaLabel ID="lblDesiredDateValue" runat="server"></ACA:AccelaLabel>
                    </div>
                </div>
                <ACA:AccelaHeightSeparate ID="separate" Height="13" runat="server">
                </ACA:AccelaHeightSeparate>
                <div class="SectionBody ACA_Label ACA_Label_FontSize_Smaller">
                    <div>
                        <ACA:AccelaLabel ID="lblLastUpdatedTitle" runat="server" LabelKey="inspectiondetail_lastupdated"
                            CssClass="ACA_TabRow_Italic">
                        </ACA:AccelaLabel>
                    </div>
                    <div>
                        <ACA:AccelaLabel ID="lblLastUpdatedBy" runat="server">
                        </ACA:AccelaLabel>
                    </div>
                    <div>
                        <ACA:AccelaLabel ID="lblLastUpdated" runat="server">
                        </ACA:AccelaLabel>
                    </div>
                </div>
            </td>
            <td class="ACA_XShot" width="2%">
            </td>
            <td class="ACA_VerticalAlign" width="60%">
                <ACA:AccelaLabel ID="lblDetailTilte" runat="server" LabelKey="inspectiondetail_details"
                    LabelType="SectionTitle">
                </ACA:AccelaLabel>
                <table role='presentation' class="ACA_FullWidthTable">
                    <tr>
                        <td class="ACA_HalfWidthTable ACA_Table_Align_Top">
                            <div id="divRelatedCap" runat="server" class="SectionBody ACA_FLeft" width="50%">
                                <div class="Header_h2"> 
                                    <ACA:AccelaLabel ID="lblRecordTitle" LabelKey="inspectiondetail_record" runat="server"></ACA:AccelaLabel>
                                </div>
                                <div>
                                    <ACA:AccelaLabel ID="lblRecordAltID" runat="server">
                                    </ACA:AccelaLabel>
                                </div>
                                <div>
                                    <ACA:AccelaLabel ID="lblRecordType" runat="server">
                                    </ACA:AccelaLabel>
                                </div>
                            </div>
                        </td>
                        <td class="ACA_HalfWidthTable ACA_Table_Align_Top">
                            <div id="divPrimaryContact" runat="server" class="SectionBody ACA_FLeft" width="50%">
                                <div class="Header_h2">
                                    <ACA:AccelaLabel ID="lblContactTitle" LabelKey="aca_inspection_contact_label" runat="server">
                                    </ACA:AccelaLabel>
                                </div>
                                <div>
                                    <ACA:AccelaLabel ID="lblContactName" runat="server">
                                    </ACA:AccelaLabel>
                                </div>
                                <div>
                                    <ACA:AccelaLabel ID="lblContactPhoneNumber" runat="server">
                                    </ACA:AccelaLabel>
                                </div>
                                <div>
                                    <ACA:AccelaLabel ID="lblRequestorName" runat="server"></ACA:AccelaLabel>
                                </div>
                                <div>
                                    <ACA:AccelaLabel ID="lblRequestorPhoneNumber" runat="server"></ACA:AccelaLabel>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div class="SectionBody ACA_LinkButton font11px">
                    <a id="lnkShowStatusHistory" onclick="showOrHideListSection('<%=divStatusHistory.ClientID %>');" href="javascript:void(0);" class="NotShowLoading">
                        <ACA:AccelaLabel ID="lblShowStatusHistory" runat="server" LabelKey="inspectiondetail_viewstatushistory">
                        </ACA:AccelaLabel>
                    </a>
                </div>
                <br />
                <div class="SectionBody ACA_LinkButton font11px">
                    <a id="lnkShowResultComments" onclick="showOrHideListSection('<%=divResultComments.ClientID %>');" href="javascript:void(0);" class="NotShowLoading">
                        <ACA:AccelaLabel ID="lblShowResultComments" runat="server" LabelKey="inspectiondetail_viewresultcomments">
                        </ACA:AccelaLabel>
                    </a>
                </div>
                <div id="divStatusHistory" runat="server">
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate3" Height="20" runat="server"></ACA:AccelaHeightSeparate>
                    <ACA:AccelaLabel ID="lblStatusHistory" LabelKey="inspectiondetail_statushistory"
                        LabelType="SectionTitle" runat="server">
                    </ACA:AccelaLabel>
                    <uc2:StatusHistory ID="StatusHistoryList" runat="server"></uc2:StatusHistory>
                </div>
                <div id="divResultComments" runat="server">
                    <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate4" Height="20" runat="server"></ACA:AccelaHeightSeparate>
                    <ACA:AccelaLabel ID="lblComments" LabelKey="inspectiondetail_resultcomments" LabelType="SectionTitle"
                        runat="server">
                    </ACA:AccelaLabel>
                    <uc3:ResultComment ID="ResultCommentList" runat="server"></uc3:ResultComment>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate1" Height="20" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="divRelatedInspection">
                    <ACA:AccelaLabel ID="lblRelatedInspections" LabelKey="aca_inspection_sectionname_relatedinspection"
                        LabelType="SectionTitle" runat="server">
                    </ACA:AccelaLabel>
                    <uc1:RelatedInspection ID="RelatedInspections" runat="server"></uc1:RelatedInspection>
                </div>
                <ACA:AccelaHeightSeparate ID="AccelaHeightSeparate2" Height="20" runat="server" />
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">

    function showOrHideListSection(divSectionID) {
        var objSection = $get(divSectionID);

        if (objSection) {
            Sys.UI.DomElement.toggleCssClass(objSection, "ACA_Hide");
        }

        var collapse = '<%=GetTextByKey("inspection_resultcommentlist_collapselink") %>';
        var readMore = '<%=GetTextByKey("inspection_resultcommentlist_readmorelink") %>';
        RebuildEllipsis({ readMore: readMore, collapse: collapse });
    }

    function print_onclick() {
        var url = "InspectionPrint.aspx";
        var a = window.open(url, "_blank", "top=200,left=200,height=550,width=850,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes");
    }
</script>

