<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" Inherits="Accela.ACA.Web.GeneralProperty.FoodFacilityInspectionDetail"
    ValidateRequest="false" CodeBehind="FoodFacilityInspectionDetail.aspx.cs" %>

<%@ Import Namespace="Accela.ACA.Web.Inspection" %>
<%@ Register Src="~/Component/Conditions.ascx" TagName="Conditions" TagPrefix="uc2" %>
<%@ Register Src="../Component/FoodFacilityGeneralInfo.ascx" TagName="FoodFacilityGeneralInfo"
    TagPrefix="uc1" %>
<%@ Register Src="../Component/InspectionPreviousItemList.ascx" TagName="InspectionPreviousItemList"
    TagPrefix="uc2" %>
<%@ Register Src="~/Component/SectionHeader.ascx" TagName="SectionHeader" TagPrefix="uc1"%>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

    <script type="text/javascript" src="../Scripts/textCollapse.js"></script>

    <div id="MainContent" class="ACA_Content ACA_Page">
        <ACA:AccelaLabel ID="lblPageInstruction" LabelKey="aca_foodfacilitydetail_label_pageinstruction"
            LabelType="PageInstruction" runat="server" />
        <h1>
            <ACA:AccelaLabel ID="lblTitle" runat="server" LabelKey="aca_foodfacilitydetail_label_title" />
            <br />
            <ACA:AccelaLabel ID="lblPropertyInfo" runat="server" />
        </h1>
        <div id="divCondition" runat="server" visible="false">
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:Conditions ID="ucConditon" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!--Food facility general information Begin-->
        <div>
            <uc1:SectionHeader runat="server" ID="shGeneralInfo" TitleLabelType="SectionTextWithField" IsUsingCustomizedLayout ="true" TitleLabelKey="aca_foodfacilitydetail_label_licensesectiontitle"></uc1:SectionHeader>
            <uc1:FoodFacilityGeneralInfo ID="foodFacilityGeneralInfo" runat="server" />
        </div>
        <!--Food facility general information End-->
        <!--Inspection status section begin-->
        <uc1:SectionHeader runat="server" ID="shInspectionStatus" TitleLabelKey="aca_foodfacilitydetail_label_statussectiontitle"></uc1:SectionHeader>
        <div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize ACA_FLeft Inspection_Status_Detail">
            <table role='presentation' border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <div runat="server" id="divGradeImagePlaceHolder" class="ACA_Image_PlaceHolder">
                            <iframe width="0" height="0" id="iframeExport" name="iframeExport" class="ACA_Hide"
                                title="<%=GetTextByKey("iframe_nonsrc_nonsupport_message") %>">
                                <%=GetTextByKey("iframe_nonsrc_nonsupport_message") %></iframe>
                            <table role='presentation' border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <a runat="server" id="lnkGradeImage" class="NotShowLoading" target="iframeExport">
                                            <img runat="server" id="imgGradeImage" class="GradeImage" />
                                        </a>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td valign="top">
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <ACA:AccelaLabel ID="lblLastInspectionDateLabel" runat="server" LabelKey="aca_foodfacilitydetail_label_lastinspectiondate"
                                        CssClass="ACA_RefEducation_Font" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <ACA:AccelaLabel ID="lblLastInspectionDateValue" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <ACA:AccelaLabel ID="lblScoreLabel" runat="server" LabelKey="aca_foodfacilitydetail_label_score"
                                        CssClass="ACA_RefEducation_Font" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <ACA:AccelaLabel ID="lblScoreValue" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <table role='presentation'>
                            <tr>
                                <td>
                                    <ACA:AccelaLabel ID="lblGradeLabel" runat="server" LabelKey="aca_foodfacilitydetail_label_grade"
                                        CssClass="ACA_RefEducation_Font" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <ACA:AccelaLabel ID="lblGradeValue" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table> 
        </div>
        <!--Inspection status section end-->
        <!--Previous inspections section begin-->
        <uc1:SectionHeader runat="server" ID="shPreviousInspections" TitleLabelKey="aca_foodfacilitydetail_label_previousinspectionsectiontitle"></uc1:SectionHeader>
        <uc2:InspectionPreviousItemList ID="inspectionPreviousItemList" runat="server" />
        <!--Previous inspections section end-->
        <br />
        <br />
        <div class="ACA_Page_Notes">
            <ACA:AccelaLabel ID="lblInspectionDelayNotice" runat="server" LabelKey="aca_foodfacilitydetail_label_showinginspectiondelaynotice" />
        </div>
    </div>

    <script type="text/javascript">
        var IsShowlicenseCondition = false;
        var initialLicenseConditionStatus = "0";

        function showMorelicenseCondition(div, a) {
            if (div == 'undefined') return;
            if (initialLicenseConditionStatus == "0") {
                IsShowlicenseCondition = false;
                initialLicenseConditionStatus = "1";
            }
            $get(div).style.display = IsShowlicenseCondition ? 'none' : '';
            $get(a).innerHTML = IsShowlicenseCondition ? '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>' : '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
            IsShowlicenseCondition = !IsShowlicenseCondition;
        }

        function EndlicenseRequest(linkId, divConditions) {

            var lnk = document.getElementById(linkId);
            if (lnk != null) {
                if (IsShowlicenseCondition) {
                    lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_hide").Replace("'","\\'") %>';
                    $get(divConditions).style.display = '';
                }
                else {
                    lnk.innerHTML = '<%=GetTextByKey("per_condition_Label_show").Replace("'","\\'") %>';
                    $get(divConditions).style.display = 'none';
                }
            }
        }
    </script>

</asp:Content>
