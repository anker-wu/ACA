<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionPreviousItemList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.InspectionPreviousItemList" %>
<%@ Import Namespace="Accela.ACA.Web.Inspection" %>
<asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvPreviousInspectionList" runat="server" AllowPaging="true"
            GridViewNumber="60114" AllowSorting="True" 
            SummaryKey="aca_foodfacilitydetail_label_previousinspectionlistsummary"
            CaptionKey="aca_caption_foodfacilitydetail_previousinspectionlist"
            ShowCaption="true" AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" OnRowDataBound="PreviousInspectionList_RowDataBound">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionDate">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionDate" runat="server" SortExpression="ResultedDateTime"
                                LabelKey="aca_foodfacilitydetail_label_previouslistinspectiondate" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionDate" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ResultedDateText %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionType">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionType" runat="server" SortExpression="TypeText"
                                LabelKey="aca_foodfacilitydetail_label_previouslistinspectiontype" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                                <ACA:AccelaLabel ID="lblInspectionTypeText" runat="server" IsNeedEncode="false"/>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScore">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkScore" runat="server" SortExpression="Score"
                                LabelKey="aca_foodfacilitydetail_label_previouslistscore" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionScore" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ScoreText %>' IsNeedEncode="false" />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGrade">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkGrade" runat="server" 
                                LabelKey="aca_foodfacilitydetail_label_previouslistgrade" CausesValidation="false" SortExpression="Grade"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionGrade" runat="server" Text='<%#  ((InspectionViewModel)Container.DataItem).Grade %>' IsNeedEncode="false" />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkMajorViolations">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkMajorViolations" runat="server" SortExpression="MajorViolationsText"
                                LabelKey="aca_foodfacilitydetail_label_previouslistmajorviolations" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblMajorViolations" runat="server" IsNeedEncode="false" />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="270px" />
                    <ItemStyle Width="270px" />
                </ACA:AccelaTemplateField>        
                <ACA:AccelaTemplateField AttributeName="lnkScheduledDateTime">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkScheduledDateTime" runat="server" SortExpression="ScheduledDateTime"
                                LabelKey="aca_foodfacilitydetail_label_previouslistscheduleddatetime" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblScheduledDateTime" runat="server" Text='' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScheduledDate">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkScheduledDate" runat="server" SortExpression="ScheduledDateTime"
                                LabelKey="aca_foodfacilitydetail_label_previouslistscheduleddate" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblScheduledDate" runat="server" Text='' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkScheduledTime">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkScheduledTime" runat="server" SortExpression="ScheduledDateTime"
                                LabelKey="aca_foodfacilitydetail_label_previouslistscheduledtime" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblScheduledTime" runat="server" Text='' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionDateTime">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionDateTime" runat="server" SortExpression="ResultedDateTime"
                                LabelKey="aca_foodfacilitydetail_label_previouslistresulteddatetime" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblResultedDateTime" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ResultedDateTimeText %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionTime">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionTime" runat="server" SortExpression="ResultedDateTime"
                                LabelKey="aca_foodfacilitydetail_label_previouslistresultedtime" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblResultedTime" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ResultedTimeText %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionTypeID">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionTypeID" runat="server" SortExpression="TypeID"
                                LabelKey="aca_foodfacilitydetail_label_previouslistinspectiontypeid" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblTypeID" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).TypeID %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionID">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionID" runat="server" SortExpression="ID"
                                LabelKey="aca_foodfacilitydetail_label_previouslistinspectionid" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionID" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ID %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspector">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspector" runat="server" SortExpression="Inspector"
                                LabelKey="aca_foodfacilitydetail_label_previouslistinspector" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspector" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).Inspector %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                 <ACA:AccelaTemplateField AttributeName="lnkInspectionResult">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionResult" runat="server" SortExpression="Result"
                                LabelKey="aca_foodfacilitydetail_label_previouslistresult" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionResult" runat="server" IsNeedEncode="false" />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionResultComment">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionResultComment" runat="server" SortExpression="ResultComments"
                                LabelKey="aca_foodfacilitydetail_label_previouslistresultcomment" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionResultComment" EnableEllipsis="true" runat="server" Text='<%# ((InspectionViewModel)Container.DataItem).ResultComments %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<div runat="server" id="lblAvailableHelpTexts" class="ACA_Hide">
    <br />
    <br />
    <hr />
    <p>Click the area below to edit Inspection Type descriptions</p>
    <fieldset>
        <ACA:AccelaLabel ID="lblInspectionTypeHelpTextTitle" runat="server" LabelType="PopUpTitle" LabelKey="aca_foodfacilitydetail_label_inspectiontypehelptexttitle"></ACA:AccelaLabel>
        <ACA:AccelaLabel ID="lblInspectionTypeHelpText" runat="server" LabelKey="aca_foodfacilitydetail_label_inspectiontypehelptext"
            LabelType="BodyText" />
    </fieldset>
    <hr />
    <p>
        Click the area below to edit Result Type descriptions</p>
    <fieldset>
        <ACA:AccelaLabel ID="lblInspectionResultHelpTextTitle" runat="server" LabelType="PopUpTitle" LabelKey="aca_foodfacilitydetail_label_inspectionresulthelptexttitle">
            </ACA:AccelaLabel>
        <ACA:AccelaLabel ID="lblInspectionResultHelpText" runat="server" LabelKey="aca_foodfacilitydetail_label_inspectionresulthelptext"
            LabelType="BodyText" />
    </fieldset>
    <hr />
    <p>Click the area below to edit Major Violations general descriptions</p>
    <fieldset>
        <ACA:AccelaLabel ID="lblInspectionMajorViolationCommonHelpTextTitle" runat="server" LabelType="PopUpTitle"
                LabelKey="aca_foodfacilitydetail_label_majorviolationscommonhelptexttitle"></ACA:AccelaLabel>
        <ACA:AccelaLabel ID="lblInspectionMajorViolationCommonHelpText" runat="server" LabelKey="aca_foodfacilitydetail_label_majorviolationscommonhelptext"
            LabelType="BodyText" />
    </fieldset>
    <hr />
</div>

<script type="text/javascript">
    function ShowInspectionTypeHelpText(objectTargetID) {
        var url = '<%=GetHelpTextURL("aca_foodfacilitydetail_label_inspectiontypehelptexttitle","aca_foodfacilitydetail_label_inspectiontypehelptext","") %>';
        ACADialog.popup({ url: url, width: 790, height: 360, objectTarget: objectTargetID });
    }

    function ShowInspectionResultHelpText(objectTargetID) {
        var url = '<%=GetHelpTextURL("aca_foodfacilitydetail_label_inspectionresulthelptexttitle","aca_foodfacilitydetail_label_inspectionresulthelptext","") %>';
        ACADialog.popup({ url: url, width: 790, height: 560, objectTarget: objectTargetID });
    }

    var additionalHelpText = "";

    function ShowInspectionMajorViolationHelpText(additionalHelpTextID, objectTargetID) {
        additionalHelpText = $("#" + additionalHelpTextID).html();
        var url = '<%=GetHelpTextURL("aca_foodfacilitydetail_label_majorviolationscommonhelptexttitle","aca_foodfacilitydetail_label_majorviolationscommonhelptext","additionalHelpText") %>';
        ACADialog.popup({ url: url, width: 790, height: 560, objectTarget: objectTargetID });
    }
</script>

