<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Education.RefContinuingEducationList" Codebehind="RefContinuingEducationList.ascx.cs" %>
<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<asp:UpdatePanel ID="refContinuingEducationEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRefContinuingEducationList" runat="server" AllowPaging="true"
            SummaryKey="gdv_conteducation_conteducationlist_summary" CaptionKey="aca_caption_conteducation_conteducationlist"
            AllowSorting="True" GridViewNumber="60079" ShowCaption="true" AutoGenerateColumns="False"
            OnRowCommand="RefContinuingEducationList_RowCommand" 
            OnPageIndexChanging="RefContinuingEducationList_PageIndexChanging"
            OnGridViewSort="RefContinuingEducationList_GridViewSort" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" ShowExportLink="true">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkCourseName">
                    <HeaderTemplate>
                         <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkCourseName" runat="server" LabelKey="per_refcontinuingeducationlist_course_name"
                                SortExpression="CourseName" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:LinkButton ID="btnCourseName" runat="server" CommandName="selectedContinuingEducation"
                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefContinuingEducation.ContEduNbr.ToString())%>'><strong><%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.RefContinuingEducation.CourseName.ToString()).ToString())%></strong></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="250px" />
                    <headerstyle Width="250px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGradingStyle">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkGradingStyle" runat="server" LabelKey="per_refcontinuingeducationlist_grading_style"
                                SortExpression="GradingStyle" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblGradingStyle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefContinuingEducation.GradingStyle.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="130px" />
                    <headerstyle Width="130px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkPassingScore">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkPassingScore" runat="server" LabelKey="per_refcontinuingeducationlist_passing_score"
                                SortExpression="PassingScore" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblPassingScore" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefContinuingEducation.PassingScore.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                    <headerstyle Width="80px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
