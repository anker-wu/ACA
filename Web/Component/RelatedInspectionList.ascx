<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedInspectionList.ascx.cs"
    Inherits="Accela.ACA.Web.Component.RelatedInspectionList" %>
<asp:UpdatePanel ID="RelatedInspectionListPanel" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <ACA:AccelaGridView ID="gdvRelatedInspectionList" runat="server" AllowPaging="true"
            GridViewNumber="60110" AllowSorting="True" SummaryKey="relatedinspectionlist_summary" 
            CaptionKey="aca_caption_relatedinspectionlist"
            ShowCaption="true"
            AutoGenerateColumns="False" PagerStyle-HorizontalAlign="center"
            PagerStyle-VerticalAlign="bottom" ShowHorizontalScroll="false" CssClass="PopupDialogGridWidth">
            <Columns>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionID">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionID" runat="server" SortExpression="InspectionID"
                                LabelKey="relatedinspectionlist_inspectionid" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <asp:HyperLink ID="InspectionID" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "TargetURL") %>' CssClass="NotShowLoading" runat="server">
                                <ACA:AccelaLabel ID="lblInspectionID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InspectionID") %>' /></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionName">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionName" runat="server" SortExpression="InspectionName"
                                LabelKey="aca_inspection_relatedinspectionlist_inspectionname" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InspectionName") %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="300px" />
                    <ItemStyle Width="300px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkRelationShip">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkRelationShip" runat="server" SortExpression="RelationShip"
                                LabelKey="aca_inspection_relatedinspectionlist_relationship" CausesValidation="false"
                                CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblRelationShip" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RelationShip") %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                    <ItemStyle Width="100px" />
                </ACA:AccelaTemplateField>
                <ACA:AccelaTemplateField AttributeName="lnkInspectionStatus">
                    <HeaderTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:GridViewHeaderLabel ID="lnkInspectionStatus" runat="server" SortExpression="StatusText"
                                LabelKey="ins_inspectionList_label_status" CausesValidation="false" CommandName="Header" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ACA_CapListStyle">
                            <ACA:AccelaLabel ID="lblInspectionStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusText") %>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle Width="150px" />
                </ACA:AccelaTemplateField>
            </Columns>
        </ACA:AccelaGridView>
    </ContentTemplate>
</asp:UpdatePanel>
