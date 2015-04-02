<%@ Import Namespace="Accela.ACA.Common" %>
<%@ Import Namespace="Accela.ACA.Common.Common" %>
<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Accela.ACA.Web.Education.RefEducationList" Codebehind="RefEducationList.ascx.cs" %>
<asp:UpdatePanel ID="EducationEditPanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRefEducationList" runat="server" AllowPaging="true" AllowSorting="True" 
            SummaryKey="gdv_refeducationlist_educationlist_summary" CaptionKey="aca_caption_refeducationlist_educationlist"
            GridViewNumber="60074" ShowCaption="true" AutoGenerateColumns="False" OnRowCommand="EducationList_RowCommand"
            OnPageIndexChanging="RefEducationList_PageIndexChanging"
            OnGridViewSort="RefEducationList_GridViewSort" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" ShowExportLink="true">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkName">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkName" runat="server" LabelKey="per_refeducationlist_label_name" SortExpression="Name" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:LinkButton ID="btnMajorDiscipline" runat="server" CommandName="selectedEducation"
                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefEducation.Number.ToString())%>'><strong><%#ScriptFilter.FilterScript(DataBinder.Eval(Container.DataItem, ColumnConstant.RefEducation.Name.ToString()).ToString())%></strong></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="300px" />
                    <HeaderStyle Width="300px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkDegree">
                    <HeaderTemplate>
                        <div class="ACA_Header_Row">
                            <ACA:GridViewHeaderLabel ID="lnkDegree" runat="server" LabelKey="per_refeducationlist_label_degree" SortExpression="Degree" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblDegree" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, ColumnConstant.RefEducation.Degree.ToString())%>' />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="250px" />
                    <HeaderStyle Width="250px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
