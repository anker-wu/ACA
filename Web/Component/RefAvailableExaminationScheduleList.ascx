<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefAvailableExaminationScheduleList.ascx.cs" Inherits="Accela.ACA.Web.Component.RefAvailableExaminationScheduleList" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Util" %>

<asp:UpdatePanel ID="ExaminationScheduleListPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvExaminationScheduleList" runat="server" AllowPaging="true" AllowSorting="True" 
            SummaryKey="gdv_contact_contactlist_summary" CaptionKey="aca_caption_contact_contactlist"
            ShowCaption="true" AutoGenerateColumns="False" IsAutoWidth="True"
            OnRowDataBound="ExaminationScheduleList_RowDataBound" PagerStyle-HorizontalAlign="center"
            OnRowCommand="ExaminationScheduleList_RowCommand" OnPageIndexChanging="ExaminationScheduleList_PageIndexChanging"
            OnGridViewSort="ExaminationScheduleList_GridViewSort" PagerStyle-VerticalAlign="bottom">
            <Columns>
                <ACA:AccelaTemplateField ShowHeader="false">
                    <ItemTemplate>
                        <ACA:AccelaRadioButton ID="rdAvailableExamination" Enabled='<%# DataBinder.Eval(Container.DataItem, "Enabled")%>' runat="server" 
                        value='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.ExaminationScheduleID.ToString())%>' 
                                             Checked='<%# DataBinder.Eval(Container.DataItem, "Selected")%>'  />
                    </ItemTemplate>
                    <ItemStyle CssClass="ACA_VerticalAlign" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkProvider">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkProvider" runat="server" LabelKey="aca_exam_schedule_availablelist_header_provider"
                                SortExpression="Provider" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblProvider" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.Provider.ToString())%>' />
                        </div>                        
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                    <HeaderStyle Width="120px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkFee">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkFee" runat="server" LabelKey="aca_exam_schedule_availablelist_header_fee"
                                SortExpression="Fee" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaNumberLabel NumberLabelType="Money" ID="lblFee" runat="server" NumericText='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.Fee.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDate">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDate" runat="server" LabelKey="aca_exam_schedule_availablelist_header_date"
                                SortExpression="Date" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblDate" runat="server" DateType="ShortDate" Text2='<%#I18nDateTimeUtil.ParseFromUI(DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.Date.ToString()))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkWeekyDay">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkWeekyDay" runat="server" LabelKey="aca_exam_schedule_availablelist_header_weekyday"
                                SortExpression="WeekyDay" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblWeekyDay" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.WeekyDay.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkStartTime">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkStartTime" runat="server" LabelKey="aca_exam_schedule_availablelist_header_starttime"
                                SortExpression="StartTime" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblStartTime" runat="server" DateType="OnlyTime" Text2='<%#I18nDateTimeUtil.ParseFromUI(DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.StartTime.ToString()))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkEndTime">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkEndTime" runat="server" LabelKey="aca_exam_schedule_availablelist_header_endtime"
                                SortExpression="EndTime" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaDateLabel ID="lblEndTime" runat="server" DateType="OnlyTime" Text2='<%#I18nDateTimeUtil.ParseFromUI(DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.EndTime.ToString()))%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkExaminationSite">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkExaminationSite" runat="server" LabelKey="aca_exam_schedule_availablelist_header_examsite"
                                SortExpression="ExaminationSite" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel id="lblExaminationSite" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.ExaminationSite.ToString())%>'></ACA:AccelaLabel>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="280px" />
                    <HeaderStyle Width="280px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkAvailableSeats">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkAvailableSeats" runat="server" LabelKey="aca_exam_schedule_availablelist_header_seats"
                                SortExpression="AvailableSeats" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblAvailableSeats" runat="server" Text='<%#int.Parse(DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.AvailableSeats.ToString()).ToString())==0?String.Empty:DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.AvailableSeats.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="70px" />
                    <HeaderStyle Width="70px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkHandicapAccessible">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkHandicapAccessible" runat="server" LabelKey="aca_exam_schedule_availablelist_header_handicap"
                                SortExpression="HandicapAccessible" CommandName="Header" CausesValidation="false" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblHandicapAccessible" EnableEllipsis="true" runat="server" Text='<%#ModelUIFormat.FormatYNLabel(DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminationScheduleDetail.AccessiblityDesc.ToString()).ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="280" />
                    <HeaderStyle Width="280" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkActionsHeader">  
                    <headertemplate>
                        <div class="ACA_Header_Row">
                            <ACA:AccelaLabel IsGridViewHeadLabel="true" ID="lnkActionsHeader" runat="server" LabelKey="aca_exam_schedule_availablelist_header_action" IsNeedEncode="false"></ACA:AccelaLabel>
                        </div>
                    </headertemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLinkButton ID="lnkProviderViewDetail" runat="server" LabelKey="aca_exam_schedule_availablelist_viewdetail" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="100" />
                    <HeaderStyle Width="100" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <asp:HiddenField ID="hfSelectedID" runat="server" />
        <asp:HiddenField ID="hfSelectedProviderNbr" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var popupDialogWidth4ScheduleExam = 800;
    var popupDialogHeight4ScheduleExam = 600;

    function closeExaminationPopupDialog() {
        ACADialog.close();
    }

    function ShowExaminationPopupDialog(pageUrl, objectTargetID) {
        ACADialog.popup({ url: pageUrl, width: popupDialogWidth4ScheduleExam, height: popupDialogHeight4ScheduleExam, objectTarget: objectTargetID });
    }
</script>
