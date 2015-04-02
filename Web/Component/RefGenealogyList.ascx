<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.RefGenealogyList"
    CodeBehind="RefGenealogyList.ascx.cs" %>
    
<asp:UpdatePanel ID="refGenHistroyUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaLabel LabelKey="aca_genealogyhistory_label_sectiontitle" ID="lblGenealogyHistoryTitle"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
        <ACA:AccelaGridView GridViewNumber="60112" ID="dgvRefGenealogyHistory"  runat="server" AutoGenerateColumns="false" OnRowDataBound="RefGenealogyHistory_RowDataBound"
            ShowCaption="true" AllowSorting="true" AllowPaging="true" PagerStyle-HorizontalAlign="center"
            CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize"  SummaryKey="gdv_genealogylisthistroy_summary" CaptionKey="aca_caption_genealogylisthistroy">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkGenHistoryDate">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenHistoryDate" SortExpression="Date" runat="server" LabelKey="genealogy_history_date"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGenHistoryDate" Text='<%# DataBinder.Eval(Container.DataItem, "Date")%>' runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenHistoryDescription">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenHistoryDescription" SortExpression="Description" runat="server" LabelKey="genealogy_history_description"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGenHistoryDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description")%>' runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenHistoryChildren">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenHistoryChildren" SortExpression="Children" runat="server" LabelKey="genealogy_history_children"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:PlaceHolder ID="phGenHistoryChildren" runat="server"></asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenHistoryAction">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenHistoryAction" SortExpression="Action" runat="server" LabelKey="genealogy_history_action"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGenHistoryAction" runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenHistoryParentsID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenHistoryParentsID" SortExpression="Parents" runat="server" LabelKey="genealogy_history_parents"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:PlaceHolder ID="phGenHistoryParents" runat="server"></asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
            </Columns>
            <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
            <HeaderStyle CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" />
            <RowStyle CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" />
            <PagerStyle CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" VerticalAlign="Bottom" />
            <AlternatingRowStyle CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" />
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="refGenChildUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <ACA:AccelaLabel LabelKey="aca_genealogychild_label_sectiontitle" ID="lblGenealogyChildTitle"
                    runat="server" LabelType="SectionTitle"></ACA:AccelaLabel>
       <ACA:AccelaGridView GridViewNumber="60113" ID="dgvRefGenealogyChild" runat="server" AutoGenerateColumns="false" OnRowDataBound="RefGenealogyChild_RowDataBound"
            ShowCaption="true" AllowSorting="true" AllowPaging="true" PagerStyle-HorizontalAlign="center"
            CaptionCssClass="ACA_SmLabel ACA_SmLabel_FontSize"  SummaryKey="gdv_genealogylistchild_summary" CaptionKey="aca_caption_genealogylistchild">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkGenChildDate">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenChildDate" runat="server" SortExpression="Date" LabelKey="genealogy_child_date"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGenChildDate" Text='<%# DataBinder.Eval(Container.DataItem, "Date")%>' runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenChildDescription">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenChildDescription" SortExpression="Description" runat="server" LabelKey="genealogy_child_description"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGenChildDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description")%>' runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenChildParentsID">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenChildParentsID" SortExpression="Parents" runat="server" LabelKey="genealogy_child_parents"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:PlaceHolder ID="phGenChildParents" runat="server"></asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenChildAction">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenChildAction" SortExpression="Action" runat="server" LabelKey="genealogy_child_action"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <ACA:AccelaLabel ID="lblGenealogyChildAction" runat="server" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkGenChildChildren">
                    <HeaderTemplate>
                        <div>
                            <ACA:GridViewHeaderLabel ID="lnkGenChildChildren" SortExpression="Children" runat="server" LabelKey="genealogy_child_children"></ACA:GridViewHeaderLabel>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:PlaceHolder ID="phGenealogyChildChildren" runat="server"></asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                    <HeaderStyle Width="150px" />
                </ACA:AccelaTemplateField>
            </Columns>
            <EmptyDataRowStyle CssClass="ACA_SmLabel ACA_SmLabel_FontSize" />
            <HeaderStyle CssClass="ACA_TabRow_Header ACA_TabRow_Header_FontSize" />
            <RowStyle CssClass="ACA_TabRow_Odd ACA_TabRow_Odd_FontSize" />
            <PagerStyle CssClass="ACA_Table_Pages ACA_Table_Pages_FontSize" VerticalAlign="Bottom" />
            <AlternatingRowStyle CssClass="ACA_TabRow_Even ACA_TabRow_Even_FontSize" />
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>