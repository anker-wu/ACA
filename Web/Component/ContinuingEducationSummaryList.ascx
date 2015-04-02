<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Education.ContinuingEducationSummaryList" Codebehind="ContinuingEducationSummaryList.ascx.cs" %>

<div>
    <ACA:AccelaLabel ID="lblContinuingEducationTitle" LabelKey="per_summaryconteducationlist_title"
        runat="server" LabelType="SubSectionText" />
</div>
<div class="ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize">
    <ACA:AccelaLabel ID="lblTotalRequiredHours" runat="server" LabelKey="per_summaryconteducationlist_required_total_hours" />
    <ACA:AccelaLabel ID="lblTotalNum" runat="server" />
    &nbsp;&nbsp;
    <ACA:AccelaLabel ID="lblRemainingHours" runat="server" LabelKey="aca_conteducsummarylist_label_remaining_hours" />
    <ACA:AccelaLabel ID="lblRemainingHoursNum" runat="server" />
</div>
<div id="divContEducationSummaryList" class="ACA_Grid_OverFlow_None_Width">
    <ACA:AccelaLabel ID="lblEmptyMsg" runat="server" LabelKey="per_permitList_messagel_noRecord"
        Visible="false" CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
    <ACA:AccelaGridView ID="gdvSummaryContEduList" HeaderStyle-CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize"
        runat="server" AllowSorting="True" AllowPaging="true" SummaryKey="gdv_conteducaionsummary_summarylist_summary"
        CaptionKey="aca_caption_conteducaionsummary_summarylist"
        GridViewNumber="60082" ShowCaption="false" AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center" PagerStyle-VerticalAlign="bottom">
        <Columns>
          <ACA:AccelaTemplateField ShowHeader="false">
                <itemtemplate>
                <div runat="server" id="divEmpty"></div>              
                </itemtemplate>
                <itemstyle Width="1px"/>
                <HeaderStyle Width="1px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkContEducationName">
                <HeaderTemplate>
                    <div class="ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkContEducationName" IsGridViewHeadLabel="true" runat="server" LabelKey="per_summaryconteducationlist_name" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <strong>
                            <ACA:AccelaLabel ID="lblContEducationName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EducaitonName")%>' />
                        </strong>
                    </div>
                </ItemTemplate>
               <ItemStyle Width="160px" />
               <HeaderStyle Width="160px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkRequiredHoursHours">
                <HeaderTemplate>
                    <div  class="ACA_FRight ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkRequiredHoursHours" runat="server" IsGridViewHeadLabel="true"
                                LabelKey="per_summaryconteducationlist_required_hours" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblRequiredHours" runat="server" CssClass="ACA_FRight" Text='<%#DataBinder.Eval(Container.DataItem, "RequiredHours")%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
               <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkCompletedHours">
                <HeaderTemplate>
                    <div  class="ACA_FRight ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkCompletedHours" runat="server" IsGridViewHeadLabel="true" LabelKey="per_summaryconteducationlist_completed_hours" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblCompletedHours" runat="server" CssClass="ACA_FRight" Text='<%#DataBinder.Eval(Container.DataItem, "CompletedHours")%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="100px" />
               <HeaderStyle Width="100px" />
            </ACA:AccelaTemplateField>
            <ACA:AccelaTemplateField AttributeName="lnkBalanceHoursDue">
                <HeaderTemplate>
                    <div class="ACA_FRight ACA_Header_Row">
                            <ACA:AccelaLabel ID="lnkBalanceHoursDue" runat="server" IsGridViewHeadLabel="true" LabelKey="per_summaryconteducationlist_balance_hours_due" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <ACA:AccelaLabel ID="lblBalanceHoursDue" runat="server" CssClass="ACA_FRight" Text='<%#DataBinder.Eval(Container.DataItem, "BalanceHours")%>' />
                    </div>
                </ItemTemplate>
                <ItemStyle Width="130px" />
               <HeaderStyle Width="130px" />
            </ACA:AccelaTemplateField>
        </Columns>
    </ACA:AccelaGridView>
</div>


