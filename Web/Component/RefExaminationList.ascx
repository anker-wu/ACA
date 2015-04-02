<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Education.RefExaminationList" Codebehind="RefExaminationList.ascx.cs" %>
<asp:UpdatePanel ID="refExaminationEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRefExaminationList" runat="server" AllowPaging="true" 
            SummaryKey="gdv_refexaminationlist_examinalist_summary" CaptionKey="aca_caption_refexaminationlist_examinalist"
            AllowSorting="True" GridViewNumber="60080" ShowCaption="true" AutoGenerateColumns="False"
            OnRowCommand="RefExaminationList_RowCommand" 
            OnPageIndexChanging="RefExaminationList_PageIndexChanging"
            OnGridViewSort="RefExaminationList_GridViewSort" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" ShowExportLink="true">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkName" runat="server" LabelKey="per_refexaminationList_name"
                                SortExpression="Name" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:LinkButton ID="btnName" runat="server" CommandName="selectedExaminations" CausesValidation="false" CommandArgument='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminations.refExamNbr.ToString())%>'><strong><%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminations.Name.ToString()).ToString())%></strong></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="250px" />
                    <HeaderStyle Width="250px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGradingStyle">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkGradingStyle" runat="server" LabelKey="per_refexaminationList_grading_style"
                                SortExpression="GradingStyle" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblGradingStyle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminations.GradingStyle.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <HeaderStyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPassingScore">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkPassingScore" runat="server" LabelKey="per_refexaminationList_passing_score" SortExpression="PassingScore" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPassingScore" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefExaminations.PassingScore.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <HeaderStyle Width="80px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
